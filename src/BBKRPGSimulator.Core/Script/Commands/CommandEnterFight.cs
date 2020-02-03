namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 进入战斗命令
    /// </summary>
    internal class CommandEnterFight : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 进入战斗命令
        /// </summary>
        /// <param name="context"></param>
        public CommandEnterFight(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 30;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandEnterFightOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 进入战斗命令的操作
        /// </summary>
        private class CommandEnterFightOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            public CommandEnterFightOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                //Context.ScriptProcess.GotoAddress(_code.Get2BytesInt(_start + 28));    //直接胜利
                int[] monstersType = {_code.Get2BytesUInt(_start + 2),
                            _code.Get2BytesUInt(_start + 4),
                            _code.Get2BytesUInt(_start + 6)};
                int[] scr = {_code.Get2BytesUInt(_start + 8),
                            _code.Get2BytesUInt(_start + 10),
                            _code.Get2BytesUInt(_start + 12)};
                int[] evtRnds = {_code.Get2BytesUInt(_start + 14),
                            _code.Get2BytesUInt(_start + 16),
                            _code.Get2BytesUInt(_start + 18)};
                int[] evts = {_code.Get2BytesUInt(_start + 20),
                            _code.Get2BytesUInt(_start + 22),
                            _code.Get2BytesUInt(_start + 24)};
                int lossto = _code.Get2BytesUInt(_start + 26);
                int winto = _code.Get2BytesUInt(_start + 28);
                Context.CombatManage.EnterStoryCombat(_code.Get2BytesUInt(_start), monstersType, scr, evtRnds, evts, lossto, winto);
                Context.ScriptProcess.ExitScript();
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}