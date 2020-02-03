using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 06饰品类物品
    /// </summary>
    internal class GoodsDecorations : GoodsEquipment
    {
        #region 字段

        /// <summary>
        /// 表示战斗时，每回合恢复或扣除多少生命
        /// </summary>
        private int _combatRestoreHp;

        /// <summary>
        /// 表示战斗时，每回合恢复或扣除多少真气
        /// </summary>
        private int _combatRestoreMp;

        /// <summary>
        /// 合体魔方序号
        /// </summary>
        private int _jointMagicIndex;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 06饰品类物品
        /// </summary>
        /// <param name="context"></param>
        public GoodsDecorations(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public MagicAttack GetCoopMagic()
        {
            return Context.LibData.GetMagic(1, _jointMagicIndex) as MagicAttack;
        }

        public override void PutOn(PlayerCharacter p)
        {
            base.PutOn(p);
            // TODO 每回合的恢复扣除
        }

        public override void TakeOff(PlayerCharacter p)
        {
            base.TakeOff(p);
            // TODO 每回合的恢复扣除
        }

        protected override void SetOtherData(byte[] buf, int offset)
        {
            _combatRestoreMp = buf.Get1ByteInt(offset + 0x16);
            _combatRestoreHp = buf.Get1ByteInt(offset + 0x17);
            Defend = buf.Get1ByteInt(offset + 0x18);
            Attack = (int)buf[offset + 0x19] & 0xff;
            Lingli = buf.Get1ByteInt(offset + 0x1a);
            Speed = buf.Get1ByteInt(offset + 0x1b);
            _jointMagicIndex = (int)buf[offset + 0x1c] & 0xff;
            Luck = buf.Get1ByteInt(offset + 0x1d);
        }

        #endregion 方法
    }
}