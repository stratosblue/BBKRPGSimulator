namespace BBKRPGSimulator.Magic
{
    /// <summary>
    /// 05特殊型 妙手空空
    /// </summary>
    internal class MagicSpecial : BaseMagic
    {
        #region 构造函数

        /// <summary>
        /// 05特殊型 妙手空空
        /// </summary>
        /// <param name="context"></param>
        public MagicSpecial(SimulatorContext context) : base(context)
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