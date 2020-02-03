namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 删除箱子命令
    /// </summary>
    internal class CommandDeleteBox : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 删除箱子命令
        /// </summary>
        /// <param name="context"></param>
        public CommandDeleteBox(SimulatorContext context) : base(context)
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
            return new CommandDeleteBoxOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 删除箱子命令的操作
        /// </summary>
        internal class CommandDeleteBoxOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 删除箱子命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandDeleteBoxOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.SceneMap.DeleteBox(_code.Get2BytesUInt(_start));
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}