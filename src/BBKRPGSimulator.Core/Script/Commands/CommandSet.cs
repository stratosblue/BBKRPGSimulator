namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// Set命令
    /// </summary>
    internal class CommandSet : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// Set命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSet(SimulatorContext context) : base(context)
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
            return new CommandSetOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// Set命令的操作
        /// </summary>
        internal class CommandSetOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// Set命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandSetOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.ScriptProcess.ScriptState.Variables[_code.Get2BytesUInt(_start)] = _code.Get2BytesUInt(_start + 2);
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}