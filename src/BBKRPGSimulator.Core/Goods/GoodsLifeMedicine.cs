namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 10灵药类
    /// 对生命的恢复0~100,表示恢复被使用者??%的生命，
    /// 并解除死亡状态，但被使用者必须是死亡状态。
    /// </summary>
    internal class GoodsLifeMedicine : BaseGoods
    {
        #region 属性

        /// <summary>
        /// 恢复百分比
        /// </summary>
        public int RestorePercent { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 10灵药类
        /// 对生命的恢复0~100,表示恢复被使用者??%的生命，
        /// 并解除死亡状态，但被使用者必须是死亡状态。
        /// </summary>
        /// <param name="context"></param>
        public GoodsLifeMedicine(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override void SetOtherData(byte[] buf, int offset)
        {
            RestorePercent = buf[offset + 0x17] & 0xff;
            if (RestorePercent > 100)
            {
                RestorePercent = 100;
            }
        }

        #endregion 方法
    }
}