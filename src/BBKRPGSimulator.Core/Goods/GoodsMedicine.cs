using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 09药物类
    /// 普通药物，任何人都可以用
    /// </summary>
    internal class GoodsMedicine : BaseGoods
    {
        #region 属性

        /// <summary>
        /// 吃药对Hp的影响
        /// </summary>
        public int AffectHp { get; private set; }

        /// <summary>
        /// 吃药对MP的影响
        /// </summary>
        public int AffectMp { get; private set; }

        /// <summary>
        /// 吃药动画
        /// </summary>
        public ResSrs Animation { get; private set; }

        /// <summary>
        /// 治疗 毒、乱、封、眠
        /// </summary>
        public CombatBuff EffectBuff { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 09药物类
        /// 普通药物，任何人都可以用
        /// </summary>
        /// <param name="context"></param>
        public GoodsMedicine(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 是具有全体治疗效果
        /// </summary>
        public override bool IsEffectAll()
        {
            return EffectBuff.HasFlag(CombatBuff.BUFF_MASK_ALL);
        }

        protected override void SetOtherData(byte[] buf, int offset)
        {
            AffectHp = buf.Get2BytesUInt(offset + 0x16);
            AffectMp = buf.Get2BytesUInt(offset + 0x18);
            Animation = Context.LibData.GetSrs(2/*(int)buf[offset + 0x1b] & 0xff*/, (int)buf[offset + 0x1a] & 0xff);
            EffectBuff = (CombatBuff)(buf[offset + 0x1c] & 0xff);
        }

        #endregion 方法
    }
}