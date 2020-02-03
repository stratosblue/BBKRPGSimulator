namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置金钱命令
    /// </summary>
    internal class CommandSetMoney : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置金钱命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSetMoney(SimulatorContext context) : base(context)
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
            return new CommandSetMoneyOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 设置金钱命令的操作
        /// </summary>
        internal class CommandSetMoneyOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 设置金钱命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandSetMoneyOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.PlayContext.Money = _code.Get4BytesInt(_start);
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}