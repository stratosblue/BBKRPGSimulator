namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// SetTo命令？
    /// </summary>
    internal class CommandSetTo : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// SetTo命令?
        /// </summary>
        /// <param name="context"></param>
        public CommandSetTo(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 4;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandSetToOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// SetTo命令的操作
        /// </summary>
        private class CommandSetToOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// SetTo命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandSetToOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.ScriptProcess.ScriptState.Variables[_code.Get2BytesUInt(_start + 2)] = Context.ScriptProcess.ScriptState.Variables[_code.Get2BytesUInt(_start)];
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}