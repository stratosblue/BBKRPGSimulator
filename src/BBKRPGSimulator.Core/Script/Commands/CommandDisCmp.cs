namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// ???
    /// </summary>
    internal class CommandDisCmp : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="context"></param>
        public CommandDisCmp(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 8;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandDisCmpOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        private class CommandDisCmpOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            public CommandDisCmpOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                int var = Context.ScriptProcess.ScriptState.Variables[_code.Get2BytesUInt(_start)];
                int num = _code.Get2BytesUInt(_start + 2);
                if (var < num)
                {
                    Context.ScriptProcess.GotoAddress(_code.Get2BytesUInt(_start + 4));
                }
                else if (var > num)
                {
                    Context.ScriptProcess.GotoAddress(_code.Get2BytesUInt(_start + 6));
                }
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}