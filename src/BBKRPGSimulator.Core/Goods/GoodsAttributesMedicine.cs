namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 11仙药类
    /// 永久性改变人物属性
    /// </summary>
    internal class GoodsAttributesMedicine : BaseGoods
    {
        #region 属性

        /// <summary>
        /// 攻击
        /// </summary>
        public int Attack { get; private set; }

        /// <summary>
        /// 防御
        /// </summary>
        public int Defend { get; private set; }

        /// <summary>
        /// 灵力
        /// </summary>
        public int Lingli { get; private set; }

        /// <summary>
        /// 幸运
        /// </summary>
        public int Luck { get; private set; }

        /// <summary>
        /// 最大HP
        /// </summary>
        public int MaxHP { get; private set; }

        /// <summary>
        /// 最大MP
        /// </summary>
        public int MaxMP { get; private set; }

        /// <summary>
        /// 速度
        /// </summary>
        public int Speed { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 11仙药类
        /// 永久性改变人物属性
        /// </summary>
        /// <param name="context"></param>
        public GoodsAttributesMedicine(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override void SetOtherData(byte[] buf, int offset)
        {
            MaxMP = buf.Get1ByteInt(offset + 0x16);
            MaxHP = buf.Get1ByteInt(offset + 0x17);
            Defend = buf.Get1ByteInt(offset + 0x18);
            Attack = buf.Get1ByteInt(offset + 0x19);
            Lingli = buf.Get1ByteInt(offset + 0x1a);
            Speed = buf.Get1ByteInt(offset + 0x1b);
            Luck = buf.Get1ByteInt(offset + 0x1d);
        }

        #endregion 方法
    }
}