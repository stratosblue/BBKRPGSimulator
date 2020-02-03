namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// CallBack？命令
    /// </summary>
    internal class CommandCallback : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// CallBack？命令
        /// </summary>
        /// <param name="context"></param>
        public CommandCallback(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandCallbackOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// CallBack？命令的操作
        /// </summary>
        private class CommandCallbackOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// CallBack？命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandCallbackOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.ScriptProcess.ExitScript();

                //TODO 此处需要确认是否正常运行
                //if (ScriptResources.globalEvents[ScriptProcess.get2ByteInt(_code, _start)])
                //{
                //    ScriptProcess.MScreenMainGame.gotoAddress(ScriptProcess.get2ByteInt(_code, _start + 2));
                //}
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}