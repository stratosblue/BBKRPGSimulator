namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// IF命令
    /// </summary>
    internal class CommandIf : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// IF命令
        /// </summary>
        /// <param name="context"></param>
        public CommandIf(SimulatorContext context) : base(context)
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
            return new CommandIfOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// IF命令的操作
        /// </summary>
        private class CommandIfOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// IF命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandIfOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                if (Context.ScriptProcess.ScriptState.GlobalEvents[_code.Get2BytesUInt(_start)])
                {
                    Context.ScriptProcess.GotoAddress(_code.Get2BytesUInt(_start + 2));
                }
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}