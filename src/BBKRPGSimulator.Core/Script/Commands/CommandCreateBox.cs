namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 创建箱子命令
    /// </summary>
    internal class CommandCreateBox : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 创建箱子命令
        /// </summary>
        /// <param name="context"></param>
        public CommandCreateBox(SimulatorContext context) : base(context)
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
            return new CommandCreateBoxOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 创建箱子命令的操作
        /// </summary>
        private class CommandCreateBoxOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 创建箱子命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandCreateBoxOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.SceneMap.CreateBox(_code.Get2BytesUInt(_start),
                        _code.Get2BytesUInt(_start + 2),
                        _code.Get2BytesUInt(_start + 4),
                        _code.Get2BytesUInt(_start + 6));
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}