namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 跳转命令
    /// </summary>
    internal class CommandGoto : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 跳转命令
        /// </summary>
        /// <param name="context"></param>
        public CommandGoto(SimulatorContext context) : base(context)
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
            return new CommandGotoOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 跳转命令的操作
        /// </summary>
        private class CommandGotoOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            public CommandGotoOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.ScriptProcess.GotoAddress(_code.Get2BytesUInt(_start));
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}