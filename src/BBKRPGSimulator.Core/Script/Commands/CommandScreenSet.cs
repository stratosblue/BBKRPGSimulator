namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置屏幕命令
    /// </summary>
    internal class CommandScreenSet : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置屏幕命令
        /// </summary>
        /// <param name="context"></param>
        public CommandScreenSet(SimulatorContext context) : base(context)
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
            return new CommandScreensOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 设置屏幕命令的操作
        /// </summary>
        private class CommandScreensOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 设置屏幕命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandScreensOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.SceneMap.SetMapScreenPos(_code.Get2BytesUInt(_start), _code.Get2BytesUInt(_start + 2));
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}