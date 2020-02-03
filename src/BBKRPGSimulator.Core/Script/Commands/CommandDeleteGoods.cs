namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 删除物品命令
    /// </summary>
    internal class CommandDeleteGoods : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 删除物品命令
        /// </summary>
        /// <param name="context"></param>
        public CommandDeleteGoods(SimulatorContext context) : base(context)
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
            return new CommandDeleteGoodsOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 删除物品命令的操作
        /// </summary>
        internal class CommandDeleteGoodsOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 删除物品命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandDeleteGoodsOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                bool success = Context.GoodsManage.DropGoods(_code.Get2BytesUInt(_start), _code.Get2BytesUInt(_start + 2));
                if (!success)
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