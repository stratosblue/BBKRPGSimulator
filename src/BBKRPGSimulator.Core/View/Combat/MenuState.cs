using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.View.Combat
{
    /// <summary>
    /// 战斗中显示玩家异常状态界面
    /// </summary>
    internal class MenuState : BaseCombatScreen
    {
        #region 字段

        /// <summary>
        /// 背景图
        /// </summary>
        private ResImage _background;

        /// <summary>
        /// 当前角色索引
        /// </summary>
        private int _curCharacterIndex;

        /// <summary>
        /// 图标
        /// 1↑2↓3×4√5回
        /// </summary>
        private ResImage _marker;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 战斗中显示玩家异常状态界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="combatUI"></param>
        public MenuState(SimulatorContext context, CombatUI combatUI) : base(context, combatUI)
        {
            _background = Context.LibData.GetImage(2, 11);
            _marker = Context.LibData.GetImage(2, 12);

            _curCharacterIndex = _combatUI.CurCharacterIndex;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            int x = (160 - _background.Width) / 2;
            int y = (96 - _background.Height) / 2;
            _background.Draw(canvas, 1, x, y);
            PlayerCharacter character = _combatUI.PlayerCharacters[_curCharacterIndex];
            character.DrawHead(canvas, x + 7, y + 4);
            Context.Util.DrawSmallNum(canvas, character.HP, x + 50, y + 9); // 命
            Context.Util.DrawSmallNum(canvas, character.Attack, x + 50, y + 21); // 攻
            Context.Util.DrawSmallNum(canvas, character.Luck, x + 87, y + 9); // 运
            Context.Util.DrawSmallNum(canvas, character.Speed, x + 87, y + 21); // 身

            _marker.Draw(canvas, 1, x + 9, y + 48); // 攻
            _marker.Draw(canvas, 2, x + 25, y + 48); // 防
            _marker.Draw(canvas, 5, x + 41, y + 48); // 身
            _marker.Draw(canvas, 3, x + 57, y + 48); // 毒
            _marker.Draw(canvas, 4, x + 73, y + 48); // 乱
            _marker.Draw(canvas, 3, x + 88, y + 48); // 封
            _marker.Draw(canvas, 4, x + 104, y + 48); // 眠
            Context.Util.DrawSmallNum(canvas, 5, x + 10, y + 57); // 攻
            Context.Util.DrawSmallNum(canvas, 5, x + 26, y + 57); // 防
            Context.Util.DrawSmallNum(canvas, 5, x + 42, y + 57); // 身
            Context.Util.DrawSmallNum(canvas, 5, x + 58, y + 57); // 毒
            Context.Util.DrawSmallNum(canvas, 5, x + 74, y + 57); // 乱
            Context.Util.DrawSmallNum(canvas, 5, x + 90, y + 57); // 封
            Context.Util.DrawSmallNum(canvas, 5, x + 106, y + 57); // 眠
        }

        public override void OnKeyDown(int key)
        {
            switch (key)
            {
                case SimulatorKeys.KEY_RIGHT:
                case SimulatorKeys.KEY_DOWN:
                case SimulatorKeys.KEY_PAGEDOWN:
                case SimulatorKeys.KEY_ENTER:
                    ++_curCharacterIndex;
                    _curCharacterIndex %= _combatUI.PlayerCharacters.Count;
                    break;

                case SimulatorKeys.KEY_LEFT:
                case SimulatorKeys.KEY_UP:
                case SimulatorKeys.KEY_PAGEUP:
                    --_curCharacterIndex;
                    _curCharacterIndex = (_curCharacterIndex + _combatUI.PlayerCharacters.Count) % _combatUI.PlayerCharacters.Count;
                    break;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_CANCEL)
            {
                _combatUI.ScreenStack.Pop();
                _combatUI.ScreenStack.Push(new MenuMisc(Context, _combatUI));
            }
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}