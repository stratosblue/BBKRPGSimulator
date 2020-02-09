using System.Drawing;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.View.GameMenu;

namespace BBKRPGSimulator.View
{
    /// <summary>
    /// 主界面
    /// </summary>
    internal class ScreenMainGame : BaseScreen
    {
        #region 构造函数

        /// <summary>
        /// 主界面
        /// </summary>
        /// <param name="context">上下文对象</param>
        /// <param name="newGame">是否新游戏</param>
        public ScreenMainGame(SimulatorContext context, bool newGame) : base(context)
        {
            if (newGame)  //开始新游戏
            {
                Context.CombatManage.EnableRandomCombat = false;
                Context.ScriptProcess.ScriptState.InitGlobalVar();
                Context.ScriptProcess.ScriptState.InitGlobalEvents();
                Context.SceneMap.SceneNPCs = new NPC[41];
                Context.PlayContext.PlayerCharacters.Clear();
                Context.GoodsManage.Clear();
                Context.PlayContext.Money = 0;
                Context.ScriptProcess.StartChapter(1, 1);
                Context.ScriptProcess.EnableExecuteScript = true;
                Context.ScriptProcess.ScriptRunning = true;
            }
            else    //再续前缘
            {
                Context.SceneMap.ReLoadMap();

                if (Context.PlayContext.PlayerCharacter == null)
                {
                    Context.PlayContext.CreateActor(1, 4, 3);
                    //Log.e("error", "存档读取出错");
                }
                Context.ScriptProcess.LoadScript(Context.SceneMap.ScriptType, Context.SceneMap.ScriptIndex);
                Context.ScriptProcess.EnableExecuteScript = true;
                Context.ScriptProcess.ScriptRunning = false;
            }
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            if (Context.ScriptProcess.ScriptRunning && Context.ScriptProcess.ScriptExecutor != null)
            {
                if (Context.CombatManage.IsActive)
                {
                    Context.CombatManage.Draw(canvas);
                }
                Context.ScriptProcess.ScriptExecutor.Draw(canvas);
            }
            else if (Context.CombatManage.IsActive)
            {
                Context.CombatManage.Draw(canvas);
            }
            else
            {
                Context.SceneMap.DrawScene(canvas);
            }
        }

        public override void OnKeyDown(int key)
        {
            if (Context.ScriptProcess.ScriptRunning && Context.ScriptProcess.ScriptExecutor != null)
            {
                Context.ScriptProcess.ScriptExecutor.KeyDown(key);
            }
            else if (Context.CombatManage.IsActive)
            {
                Context.CombatManage.KeyDown(key);
                return;
            }
            else if (Context.PlayContext.PlayerCharacter != null)
            {
                switch (key)
                {
                    case SimulatorKeys.KEY_LEFT:
                        PlayerCharacterWalk(Direction.West);
                        break;

                    case SimulatorKeys.KEY_RIGHT:
                        PlayerCharacterWalk(Direction.East);
                        break;

                    case SimulatorKeys.KEY_UP:
                        PlayerCharacterWalk(Direction.North);
                        break;

                    case SimulatorKeys.KEY_DOWN:
                        PlayerCharacterWalk(Direction.South);
                        break;

                    case SimulatorKeys.KEY_ENTER:
                        Context.SceneMap.TriggerSceneObjEvent();
                        break;
                }
            }
        }

        public override void OnKeyUp(int key)
        {
            if (Context.ScriptProcess.ScriptRunning && Context.ScriptProcess.ScriptExecutor != null)
            {
                Context.ScriptProcess.ScriptExecutor.KeyUp(key);
            }
            else if (Context.CombatManage.IsActive)
            {
                Context.CombatManage.KeyUp(key);
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.PushScreen(new ScreenGameMainMenu(Context));
            }
        }

        public override void Update(long delta)
        {
            if (Context.ScriptProcess.ScriptRunning && Context.ScriptProcess.ScriptExecutor != null)
            {
                Context.ScriptProcess.ScriptExecutor.Process();
                Context.ScriptProcess.ScriptExecutor.Update(delta);
                Context.ScriptProcess.ScriptExecutor.TimerStep(delta);
            }
            else if (Context.CombatManage.IsActive)
            {
                // TODO fix this test
                Context.CombatManage.Update(delta);
            }
            else
            {
                Context.SceneMap.Update(delta);
                Context.ScriptProcess.ScriptExecutor.TimerStep(delta);
            }
        }

        /// <summary>
        /// 玩家角色移动
        /// </summary>
        /// <param name="direction"></param>
        private void PlayerCharacterWalk(Direction direction)
        {
            var playerCharacter = Context.PlayContext.PlayerCharacter;
            Point target = playerCharacter.PosInMap;

            int offsetX = 0, offsetY = 0;

            switch (direction)
            {
                case Direction.North:
                    offsetY = -1;
                    break;

                case Direction.East:
                    offsetX = 1;
                    break;

                case Direction.South:
                    offsetY = +1;
                    break;

                case Direction.West:
                    offsetX = -1;
                    break;
            }
            target.X += offsetX;
            target.Y += offsetY;

            Context.SceneMap.TriggerMapEvent(target.X, target.Y);
            if (Context.SceneMap.CanPlayerWalk(target.X, target.Y))
            {
                playerCharacter.Walk(direction);
                Context.SceneMap.MapScreenPos.X += offsetX;
                Context.SceneMap.MapScreenPos.Y += offsetY;
                Context.SceneMap.MapScreenX = Context.SceneMap.MapScreenPos.X;
                Context.SceneMap.MapScreenY = Context.SceneMap.MapScreenPos.Y;
            }
            else
            {
                playerCharacter.WalkStay(direction);
            }
        }

        #endregion 方法
    }
}