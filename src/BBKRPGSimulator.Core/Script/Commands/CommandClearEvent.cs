namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 清除事件命令
    /// </summary>
    internal class CommandClearEvent : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 清除事件命令
        /// </summary>
        /// <param name="context"></param>
        public CommandClearEvent(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 2;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandClearEventOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 清除事件命令的操作
        /// </summary>
        private class CommandClearEventOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 清除事件命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandClearEventOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.ScriptProcess.ScriptState.ClearEvent(_code.Get2BytesUInt(_start));
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}