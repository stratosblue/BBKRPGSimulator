using System;
using System.Collections.Generic;

using BBKRPGSimulator.Lib;
using BBKRPGSimulator.Script.Commands;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 脚本处理
    /// </summary>
    internal class ScriptProcess : ContextDependent
    {
        #region 字段

        /// <summary>
        /// 命令数组
        /// </summary>
        private ICommand[] _commands;

        /// <summary>
        /// 脚本
        /// </summary>
        private ResGut _script;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 是否允许执行脚本
        /// </summary>
        public bool EnableExecuteScript { get; set; } = true;

        /// <summary>
        /// 脚本执行器
        /// </summary>
        public ScriptExecutor ScriptExecutor { get; set; }

        /// <summary>
        /// 当前是否在执行脚本
        /// </summary>
        public bool ScriptRunning { get; set; } = true;

        /// <summary>
        /// 脚本状态
        /// </summary>
        public ScriptResources ScriptState { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 脚本处理
        /// </summary>
        public ScriptProcess(SimulatorContext context) : base(context)
        {
            _commands = new ICommand[]
            {
                new CommandMusic(Context),
                new CommandLoadMap(Context),
                new CommandCreateCharacter(Context),
                new CommandDeleteNpc(Context),
                null,
                null,
                new CommandMove(Context),
                null,
                null,
                new CommandCallback(Context),
                new CommandGoto(Context),
                new CommandIf(Context),
                new CommandSet(Context),
                new CommandSay(Context),
                new CommandStartChapter(Context),
                null,
                new CommandScreenSet(Context),
                null,
                null,
                null,
                new CommandGameover(Context),
                new CommandIfcmp(Context),
                new CommandAdd(Context),
                new CommandSub(Context),
                new CommandSetControlId(Context),  //伏魔记未用到
                null,
                new CommandSetEvent(Context),
                new CommandClearEvent(Context),
                new CommandBuy(Context),
                new CommandFaceToFace(Context),
                new CommandMovie(Context),
                new CommandChoice(Context),
                new CommandCreateBox(Context),
                new CommandDeleteBox(Context),
                new CommandGainGoods(Context),
                new CommandInitFight(Context),
                new CommandFightEnable(Context),
                new CommandFightDisEnable(Context),
                new CommandCreateNpc(Context),
                new CommandEnterFight(Context),
                new CommandDeleteCharacter(Context),
                new CommandGainMoney(Context),
                new CommandUseMoney(Context),
                new CommandSetMoney(Context),
                new CommandLearnMagic(Context),
                new CommandSale(Context),
                new CommandNpcMoveMod(Context),
                new CommandMessage(Context),
                new CommandDeleteGoods(Context),
                new CommandRestoreCharacterHp(Context),
                new CommandActorLayerUp(Context),
                new CommandBoxOpen(Context),
                new CommandDelAllNpc(Context),
                new CommandNpcStep(Context),
                new CommandSetSceneName(Context),
                new CommandShowSceneName(Context),
                new CommandShowScreen(Context),
                new CommandUseGoods(Context),
                new CommandAttributeTest(Context),    //伏魔记未用到
                new CommandAttributeSet(Context), //伏魔记未用到
                new CommandAttributeAdd(Context), //伏魔记未用到
                new CommandShowGut(Context),
                new CommandUseGoodsNum(Context),
                new CommandRandRade(Context),
                new CommandMenu(Context),  //0-6中用到
                new CommandTestMoney(Context),
                new CommandCallChapter(Context),   //伏魔记未用到
                new CommandDisCmp(Context),
                new CommandReturn(Context),
                new CommandTimeMsg(Context),   //伏魔记未用到
                new CommandDisableSave(Context),   //0-6
                new CommandEnableSave(Context),    //0-6
                new CommandGameSave(Context),  //伏魔记未用到
                new CommandSetEventTimer(Context), //伏魔记未用到
                new CommandEnableShowpos(Context),
                new CommandDisableShowpos(Context),
                new CommandSetTo(Context),
                new CommandTestGoodsNum(Context),
            };

            ScriptState = new ScriptResources();
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 退出脚本
        /// </summary>
        public void ExitScript()
        {
            ScriptRunning = false;
            EnableExecuteScript = false;
        }

        /// <summary>
        /// 跳转到指定的脚本地址
        /// </summary>
        /// <param name="address"></param>
        public void GotoAddress(int address)
        {
            ScriptExecutor.GotoAddress(address);
            ScriptRunning = true;
        }

        /// <summary>
        /// 加载脚本
        /// </summary>
        /// <param name="resGut"></param>
        public void LoadScript(ResGut resGut)
        {
            _script = resGut;
            ReSetScriptExecutor();
        }

        /// <summary>
        /// 加载脚本
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool LoadScript(int type, int index)
        {
            _script = Context.LibData.GetGut(type, index);
            if (_script != null)
            {
                LoadScript(_script);
                return true;
            }
            return false;
        }

        public void RunScript()
        {
            ScriptRunning = true;
        }

        /// <summary>
        /// 开始指定章节
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public void StartChapter(int type, int index)
        {
            //System.out.println("ScreenMainGame.startChapter " + type + " " + index);
            LoadScript(type, index);
            //		update(0);
            EnableExecuteScript = false;
            for (int i = 1; i <= 40; i++)
            {
                Context.SceneMap.SceneNPCs[i] = null;
            }
            ScriptState.InitLocalVar();
            Context.SceneMap.ScriptType = type;
            Context.SceneMap.ScriptIndex = index;
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventId"></param>
        public void TriggerEvent(int eventId)
        {
            if (ScriptExecutor != null)
            {
                ScriptRunning = ScriptExecutor.TriggerEvent(eventId);
            }
        }

        /// <summary>
        /// 重置脚本执行器
        /// </summary>
        /// <returns></returns>
        private void ReSetScriptExecutor()
        {
            if (_script == null)
            {
                throw new Exception("no script data loaded!");
            }

            byte[] code = _script.ScriptData;
            int pointer = 0;

            Dictionary<int, int> map = new Dictionary<int, int>(128); // offsetAddr----index of operate
            int iOfOper = 0;

            List<Operate> operateList = new List<Operate>();

            while (pointer < code.Length)
            {
                //HACK 同KEY直接跳过了
                if (map.ContainsKey(pointer))
                {
                    break;
                }
                map.Add(pointer, iOfOper);
                ++iOfOper;
                //HACK 超出索引 加了判断
                var cmdIndex = code[pointer];
                if (cmdIndex < _commands.Length)
                {
                    ICommand cmd = _commands[cmdIndex];
                    operateList.Add(cmd.GetOperate(code, pointer + 1));
                    pointer = cmd.GetNextPos(code, pointer + 1);
                }
            }

            int[] events = _script.SceneEvent;
            int[] eventIndex = new int[events.Length];
            for (int i = 0; i < events.Length; i++)
            {
                if (events[i] == 0)
                {
                    eventIndex[i] = -1; // 未使用的事件，存在于前40个中
                }
                else
                {
                    eventIndex[i] = map[events[i] - events.Length * 2 - 3];
                }
            }

            ScriptExecutor = new ScriptExecutor(Context, operateList, eventIndex, map, events.Length * 2 + 3);
        }

        #endregion 方法
    }
}