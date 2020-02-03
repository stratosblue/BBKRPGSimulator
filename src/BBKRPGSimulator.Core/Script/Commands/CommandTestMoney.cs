namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 测试金钱命令
    /// </summary>
    internal class CommandTestMoney : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 测试金钱命令
        /// </summary>
        /// <param name="context"></param>
        public CommandTestMoney(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 6;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandTestMoneyOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 测试金钱命令的操作
        /// </summary>
        private class CommandTestMoneyOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 测试金钱命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandTestMoneyOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                if (Context.PlayContext.Money < _code.Get4BytesInt(_start))
                {
                    Context.ScriptProcess.GotoAddress(_code.Get2BytesUInt(_start + 4));
                }
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}