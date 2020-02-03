namespace BBKRPGSimulator.Magic
{
    /// <summary>
    /// 04辅助型
    /// </summary>
    internal class MagicAuxiliary : BaseMagic
    {
        #region 属性

        /// <summary>
        /// 0~100，表示被施展者恢复生命的百分比（起死回生）
        /// </summary>
        public int HpPercent { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 04辅助型
        /// </summary>
        /// <param name="context"></param>
        public MagicAuxiliary(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override void SetOtherData(byte[] buf, int offset)
        {
            HpPercent = buf.Get2BytesUInt(offset + 0x12);
        }

        #endregion 方法
    }
}