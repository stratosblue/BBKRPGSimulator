using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 08暗器 物品
    /// </summary>
    internal class GoodsHiddenWeapon : BaseGoods
    {
        #region 字段

        /// <summary>
        /// 000 全体否 毒乱封眠
        /// </summary>
        private int _effectBitMask;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 当该值为正时表示敌人损失多少生命，为负时表示从敌人身上吸取多少生命到投掷者身上
        /// </summary>
        public int AffectHp { get; private set; }

        /// <summary>
        /// 当该值为正时表示敌人损失多少真气，为负时表示从敌人身上吸取多少真气到投掷者身上
        /// </summary>
        public int AffectMp { get; private set; }

        /// <summary>
        /// 暗器使用动画
        /// </summary>
        public ResSrs Animation { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 08暗器 物品
        /// </summary>
        /// <param name="context"></param>
        public GoodsHiddenWeapon(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 是否作用于全体
        /// </summary>
        /// <returns></returns>
        public override bool IsEffectAll()
        {
            return (_effectBitMask & 0x10) != 0;
        }

        protected override void SetOtherData(byte[] buf, int offset)
        {
            AffectHp = buf.Get2BytesInt(offset + 0x16);
            AffectMp = buf.Get2BytesInt(offset + 0x18);
            Animation = Context.LibData.GetSrs(buf[offset + 0x1b] & 0xff, buf[offset + 0x1a] & 0xff);
            _effectBitMask = (int)buf[offset + 0x1c] & 0xff;
        }

        #endregion 方法
    }
}