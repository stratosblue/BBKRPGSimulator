namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 13土遁类
    /// 各作用字段均无意义,起作用的程序由脚本编写者决定(目前作用是从迷宫中直接返回)。
    /// 当使用土遁类道具时，会触发当前执行脚本的第255号事件。
    /// </summary>
    internal class GoodsTudun : BaseGoods
    {
        #region 构造函数

        /// <summary>
        /// 13土遁类
        /// 各作用字段均无意义,起作用的程序由脚本编写者决定(目前作用是从迷宫中直接返回)。
        /// 当使用土遁类道具时，会触发当前执行脚本的第255号事件。
        /// </summary>
        /// <param name="context"></param>
        public GoodsTudun(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override void SetOtherData(byte[] buf, int offset)
        {
        }

        #endregion 方法
    }
}