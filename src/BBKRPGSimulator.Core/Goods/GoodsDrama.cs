namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 14剧情类 物品
    /// </summary>
    internal class GoodsDrama : BaseGoods
    {
        #region 构造函数

        /// <summary>
        /// 14剧情类 物品
        /// </summary>
        /// <param name="context"></param>
        public GoodsDrama(SimulatorContext context) : base(context)
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