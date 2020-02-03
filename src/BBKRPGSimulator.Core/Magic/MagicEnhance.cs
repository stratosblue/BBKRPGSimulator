namespace BBKRPGSimulator.Magic
{
    /// <summary>
    /// 02增强型
    /// </summary>
    internal class MagicEnhance : BaseMagic
    {
        #region 属性

        /// <summary>
        /// 0~100，被施展者的攻击力增强的百分比
        /// </summary>
        public int Attack { get; private set; }

        /// <summary>
        /// 0~100，被施展者的防御力增强的百分比
        /// </summary>
        public int Defend { get; private set; }

        /// <summary>
        /// 持续回合
        /// </summary>
        public int EnhanceRound { get; private set; }

        /// <summary>
        /// 速 0~100，被施展者的身法加快的百分比
        /// </summary>
        public int Speed { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 02增强型
        /// </summary>
        /// <param name="context"></param>
        public MagicEnhance(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override void SetOtherData(byte[] buf, int offset)
        {
            Defend = (int)buf[offset + 0x16] & 0xff;
            Attack = (int)buf[offset + 0x17] & 0xff;
            EnhanceRound = (int)(buf[offset + 0x18] >> 4) & 0xf;
            Speed = (int)buf[offset + 0x19] & 0xff;
        }

        #endregion 方法
    }
}