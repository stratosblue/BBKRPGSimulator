namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 12兴奋剂
    /// </summary>
    internal class GoodsStimulant : BaseGoods
    {
        #region 字段

        /// <summary>
        /// 是否影响全部
        /// </summary>
        private bool _isEffectAll;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 攻击增加百分比
        /// </summary>
        public int AttackPercent { get; private set; }

        /// <summary>
        /// 防御增加百分比
        /// </summary>
        public int DefendPercent { get; private set; }

        /// <summary>
        /// 速度增加百分比
        /// </summary>
        public int SpeedPercent { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 12兴奋剂
        /// </summary>
        /// <param name="context"></param>
        public GoodsStimulant(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 是否影响全部
        /// </summary>
        /// <returns></returns>
        public override bool IsEffectAll()
        {
            return _isEffectAll;
        }

        protected override void SetOtherData(byte[] buf, int offset)
        {
            DefendPercent = (int)buf[offset + 0x18] & 0xff;
            AttackPercent = (int)buf[offset + 0x19] & 0xff;
            SpeedPercent = (int)buf[offset + 0x1b] & 0xff;
            _isEffectAll = ((int)buf[offset + 0x1c] & 0x10) != 0;
        }

        #endregion 方法
    }
}