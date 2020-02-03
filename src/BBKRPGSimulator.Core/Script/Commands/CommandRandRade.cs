namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 随机事件命令
    /// </summary>
    internal class CommandRandRade : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 随机事件命令
        /// </summary>
        /// <param name="context"></param>
        public CommandRandRade(SimulatorContext context) : base(context)
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
            return new CommandRandRadeOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 随机事件命令的操作
        /// </summary>
        internal class CommandRandRadeOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 随机事件命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandRandRadeOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                if ((Context.Random.Next() % 1000) <= _code.Get2BytesUInt(_start))
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