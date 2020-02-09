using System;
using System.Collections.Generic;

using BBKRPGSimulator.Lib;
using BBKRPGSimulator.Script.Commands;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 脚本处理
    /// </summary>
    internal sealed class ScriptProcess : ContextDependent
    {
        #region 字段

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
        public ScriptExecutor ScriptExecutor { get; private set; }

        /// <summary>
        /// 当前是否在执行脚本
        /// </summary>
        public bool ScriptRunning { get; set; } = true;

        /// <summary>
        /// 脚本状态
        /// </summary>
        public ScriptResources ScriptState { get; private set; }

        /// <summary>
        /// 上一个ScriptProcess
        /// </summary>
        public ScriptProcess PreScriptProcess { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 脚本处理
        /// </summary>
        public ScriptProcess(SimulatorContext context) : base(context)
        {
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

            var commands = new List<ICommand>();

            while (pointer < code.Length)
            {
                //HACK 同KEY直接跳过了
                if (map.ContainsKey(pointer))
                {
                    break;
                }
                map.Add(pointer, iOfOper);
                ++iOfOper;

                if (CommandFactory.CreateCommand(pointer, code, Context) is ICommand cmd)
                {
                    commands.Add(cmd);
                    pointer += cmd.Length;
                }
                pointer++;
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

            ScriptExecutor = new ScriptExecutor(Context, commands, eventIndex, map, events.Length * 2 + 3);
        }

        #endregion 方法

        #region 类

        private static class CommandFactory
        {
            #region 字段

            private static readonly IReadOnlyList<Func<ArraySegment<byte>, SimulatorContext, ICommand>> _cmdCreateDelegates = null;

            #endregion 字段

            #region 构造函数

            static CommandFactory()
            {
                ICommand CreateCommandMusic(ArraySegment<byte> data, SimulatorContext context) => new CommandMusic(context);
                ICommand CreateCommandLoadMap(ArraySegment<byte> data, SimulatorContext context) => new CommandLoadMap(data, context);
                ICommand CreateCommandCreateCharacter(ArraySegment<byte> data, SimulatorContext context) => new CommandCreateCharacter(data, context);
                ICommand CreateCommandDeleteNpc(ArraySegment<byte> data, SimulatorContext context) => new CommandDeleteNpc(data, context);
                ICommand CreateCommandMove(ArraySegment<byte> data, SimulatorContext context) => new CommandMove(data, context);
                ICommand CreateCommandCallback(ArraySegment<byte> data, SimulatorContext context) => new CommandCallback(context);
                ICommand CreateCommandGoto(ArraySegment<byte> data, SimulatorContext context) => new CommandGoto(data, context);
                ICommand CreateCommandIf(ArraySegment<byte> data, SimulatorContext context) => new CommandIf(data, context);
                ICommand CreateCommandSet(ArraySegment<byte> data, SimulatorContext context) => new CommandSet(data, context);
                ICommand CreateCommandSay(ArraySegment<byte> data, SimulatorContext context) => new CommandSay(data, context);
                ICommand CreateCommandStartChapter(ArraySegment<byte> data, SimulatorContext context) => new CommandStartChapter(data, context);
                ICommand CreateCommandScreenSet(ArraySegment<byte> data, SimulatorContext context) => new CommandScreenSet(data, context);
                ICommand CreateCommandGameover(ArraySegment<byte> data, SimulatorContext context) => new CommandGameover(context);
                ICommand CreateCommandIfcmp(ArraySegment<byte> data, SimulatorContext context) => new CommandIfcmp(data, context);
                ICommand CreateCommandAdd(ArraySegment<byte> data, SimulatorContext context) => new CommandAdd(data, context);
                ICommand CreateCommandSub(ArraySegment<byte> data, SimulatorContext context) => new CommandSub(data, context);
                ICommand CreateCommandSetControlId(ArraySegment<byte> data, SimulatorContext context) => new CommandSetControlId(data, context);
                ICommand CreateCommandSetEvent(ArraySegment<byte> data, SimulatorContext context) => new CommandSetEvent(data, context);
                ICommand CreateCommandClearEvent(ArraySegment<byte> data, SimulatorContext context) => new CommandClearEvent(data, context);
                ICommand CreateCommandBuy(ArraySegment<byte> data, SimulatorContext context) => new CommandBuy(data, context);
                ICommand CreateCommandFaceToFace(ArraySegment<byte> data, SimulatorContext context) => new CommandFaceToFace(data, context);
                ICommand CreateCommandMovie(ArraySegment<byte> data, SimulatorContext context) => new CommandMovie(data, context);
                ICommand CreateCommandChoice(ArraySegment<byte> data, SimulatorContext context) => new CommandChoice(data, context);
                ICommand CreateCommandCreateBox(ArraySegment<byte> data, SimulatorContext context) => new CommandCreateBox(data, context);
                ICommand CreateCommandDeleteBox(ArraySegment<byte> data, SimulatorContext context) => new CommandDeleteBox(data, context);
                ICommand CreateCommandGainGoods(ArraySegment<byte> data, SimulatorContext context) => new CommandGainGoods(data, context);
                ICommand CreateCommandInitFight(ArraySegment<byte> data, SimulatorContext context) => new CommandInitFight(data, context);
                ICommand CreateCommandFightEnable(ArraySegment<byte> data, SimulatorContext context) => new CommandFightEnable(context);
                ICommand CreateCommandFightDisEnable(ArraySegment<byte> data, SimulatorContext context) => new CommandFightDisEnable(context);
                ICommand CreateCommandCreateNpc(ArraySegment<byte> data, SimulatorContext context) => new CommandCreateNpc(data, context);
                ICommand CreateCommandEnterFight(ArraySegment<byte> data, SimulatorContext context) => new CommandEnterFight(data, context);
                ICommand CreateCommandDeleteCharacter(ArraySegment<byte> data, SimulatorContext context) => new CommandDeleteCharacter(data, context);
                ICommand CreateCommandGainMoney(ArraySegment<byte> data, SimulatorContext context) => new CommandGainMoney(data, context);
                ICommand CreateCommandUseMoney(ArraySegment<byte> data, SimulatorContext context) => new CommandUseMoney(data, context);
                ICommand CreateCommandSetMoney(ArraySegment<byte> data, SimulatorContext context) => new CommandSetMoney(data, context);
                ICommand CreateCommandLearnMagic(ArraySegment<byte> data, SimulatorContext context) => new CommandLearnMagic(data, context);
                ICommand CreateCommandSale(ArraySegment<byte> data, SimulatorContext context) => new CommandSale(context);
                ICommand CreateCommandNpcMoveMod(ArraySegment<byte> data, SimulatorContext context) => new CommandNpcMoveMod(data, context);
                ICommand CreateCommandMessage(ArraySegment<byte> data, SimulatorContext context) => new CommandMessage(data, context);
                ICommand CreateCommandDeleteGoods(ArraySegment<byte> data, SimulatorContext context) => new CommandDeleteGoods(data, context);
                ICommand CreateCommandRestoreCharacterHp(ArraySegment<byte> data, SimulatorContext context) => new CommandRestoreCharacterHp(data, context);
                ICommand CreateCommandActorLayerUp(ArraySegment<byte> data, SimulatorContext context) => new CommandActorLayerUp(context);
                ICommand CreateCommandBoxOpen(ArraySegment<byte> data, SimulatorContext context) => new CommandBoxOpen(data, context);
                ICommand CreateCommandDelAllNpc(ArraySegment<byte> data, SimulatorContext context) => new CommandDelAllNpc(context);
                ICommand CreateCommandNpcStep(ArraySegment<byte> data, SimulatorContext context) => new CommandNpcStep(data, context);
                ICommand CreateCommandSetSceneName(ArraySegment<byte> data, SimulatorContext context) => new CommandSetSceneName(data, context);
                ICommand CreateCommandShowSceneName(ArraySegment<byte> data, SimulatorContext context) => new CommandShowSceneName(context);
                ICommand CreateCommandShowScreen(ArraySegment<byte> data, SimulatorContext context) => new CommandShowScreen(context);
                ICommand CreateCommandUseGoods(ArraySegment<byte> data, SimulatorContext context) => new CommandUseGoods(data, context);
                ICommand CreateCommandAttributeTest(ArraySegment<byte> data, SimulatorContext context) => new CommandAttributeTest(data, context);
                ICommand CreateCommandAttributeSet(ArraySegment<byte> data, SimulatorContext context) => new CommandAttributeSet(data, context);
                ICommand CreateCommandAttributeAdd(ArraySegment<byte> data, SimulatorContext context) => new CommandAttributeAdd(data, context);
                ICommand CreateCommandShowGut(ArraySegment<byte> data, SimulatorContext context) => new CommandShowGut(data, context);
                ICommand CreateCommandUseGoodsNum(ArraySegment<byte> data, SimulatorContext context) => new CommandUseGoodsNum(data, context);
                ICommand CreateCommandRandRade(ArraySegment<byte> data, SimulatorContext context) => new CommandRandRade(data, context);
                ICommand CreateCommandMenu(ArraySegment<byte> data, SimulatorContext context) => new CommandMenu(data, context);
                ICommand CreateCommandTestMoney(ArraySegment<byte> data, SimulatorContext context) => new CommandTestMoney(data, context);
                ICommand CreateCommandCallChapter(ArraySegment<byte> data, SimulatorContext context) => new CommandCallChapter(data, context);
                ICommand CreateCommandDisCmp(ArraySegment<byte> data, SimulatorContext context) => new CommandDisCmp(data, context);
                ICommand CreateCommandReturn(ArraySegment<byte> data, SimulatorContext context) => new CommandReturn(context);
                ICommand CreateCommandTimeMsg(ArraySegment<byte> data, SimulatorContext context) => new CommandTimeMsg(data, context);
                ICommand CreateCommandDisableSave(ArraySegment<byte> data, SimulatorContext context) => new CommandDisableSave(context);
                ICommand CreateCommandEnableSave(ArraySegment<byte> data, SimulatorContext context) => new CommandEnableSave(context);
                ICommand CreateCommandGameSave(ArraySegment<byte> data, SimulatorContext context) => new CommandGameSave(context);
                ICommand CreateCommandSetEventTimer(ArraySegment<byte> data, SimulatorContext context) => new CommandSetEventTimer(data, context);
                ICommand CreateCommandEnableShowpos(ArraySegment<byte> data, SimulatorContext context) => new CommandEnableShowpos(context);
                ICommand CreateCommandDisableShowpos(ArraySegment<byte> data, SimulatorContext context) => new CommandDisableShowpos(context);
                ICommand CreateCommandSetTo(ArraySegment<byte> data, SimulatorContext context) => new CommandSetTo(data, context);
                ICommand CreateCommandTestGoodsNum(ArraySegment<byte> data, SimulatorContext context) => new CommandTestGoodsNum(data, context);

                ICommand CreateCommandSetFightMiss(ArraySegment<byte> data, SimulatorContext context) => new NotImplementedCommand(2);
                ICommand CreateCommandSetarmstoss(ArraySegment<byte> data, SimulatorContext context) => new NotImplementedCommand(2);

                _cmdCreateDelegates = new Func<ArraySegment<byte>, SimulatorContext, ICommand>[]
                {
                    CreateCommandMusic,
                    CreateCommandLoadMap,
                    CreateCommandCreateCharacter,
                    CreateCommandDeleteNpc,
                    null,
                    null,
                    CreateCommandMove,
                    null,
                    null,
                    CreateCommandCallback,
                    CreateCommandGoto,
                    CreateCommandIf,
                    CreateCommandSet,
                    CreateCommandSay,
                    CreateCommandStartChapter,
                    null,
                    CreateCommandScreenSet,
                    null,
                    null,
                    null,
                    CreateCommandGameover,
                    CreateCommandIfcmp,
                    CreateCommandAdd,
                    CreateCommandSub,
                    CreateCommandSetControlId,  //伏魔记未用到
                    null,
                    CreateCommandSetEvent,
                    CreateCommandClearEvent,
                    CreateCommandBuy,
                    CreateCommandFaceToFace,
                    CreateCommandMovie,
                    CreateCommandChoice,
                    CreateCommandCreateBox,
                    CreateCommandDeleteBox,
                    CreateCommandGainGoods,
                    CreateCommandInitFight,
                    CreateCommandFightEnable,
                    CreateCommandFightDisEnable,
                    CreateCommandCreateNpc,
                    CreateCommandEnterFight,
                    CreateCommandDeleteCharacter,
                    CreateCommandGainMoney,
                    CreateCommandUseMoney,
                    CreateCommandSetMoney,
                    CreateCommandLearnMagic,
                    CreateCommandSale,
                    CreateCommandNpcMoveMod,
                    CreateCommandMessage,
                    CreateCommandDeleteGoods,
                    CreateCommandRestoreCharacterHp,
                    CreateCommandActorLayerUp,
                    CreateCommandBoxOpen,
                    CreateCommandDelAllNpc,
                    CreateCommandNpcStep,
                    CreateCommandSetSceneName,
                    CreateCommandShowSceneName,
                    CreateCommandShowScreen,
                    CreateCommandUseGoods,
                    CreateCommandAttributeTest,    //伏魔记未用到
                    CreateCommandAttributeSet, //伏魔记未用到
                    CreateCommandAttributeAdd, //伏魔记未用到
                    CreateCommandShowGut,
                    CreateCommandUseGoodsNum,
                    CreateCommandRandRade,
                    CreateCommandMenu,  //0-6中用到
                    CreateCommandTestMoney,
                    CreateCommandCallChapter,   //伏魔记未用到
                    CreateCommandDisCmp,
                    CreateCommandReturn,
                    CreateCommandTimeMsg,   //伏魔记未用到
                    CreateCommandDisableSave,   //0-6
                    CreateCommandEnableSave,    //0-6
                    CreateCommandGameSave,  //伏魔记未用到
                    CreateCommandSetEventTimer, //伏魔记未用到
                    CreateCommandEnableShowpos,
                    CreateCommandDisableShowpos,
                    CreateCommandSetTo,
                    CreateCommandTestGoodsNum,
                    CreateCommandSetFightMiss,
                    CreateCommandSetarmstoss,
                };
            }

            #endregion 构造函数

            #region 方法

            public static ICommand CreateCommand(int index, byte[] data, SimulatorContext context)
            {
                //HACK 超出索引 加了判断
                var cmdIndex = data[index];
                if (cmdIndex < _cmdCreateDelegates.Count)
                {
                    return _cmdCreateDelegates[data[index]]?.Invoke(new ArraySegment<byte>(data, index + 1, data.Length - index - 1), context);
                }
                return null;
            }

            #endregion 方法
        }

        #endregion 类
    }
}