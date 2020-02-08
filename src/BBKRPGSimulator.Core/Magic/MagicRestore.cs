using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Definitions;

namespace BBKRPGSimulator.Magic
{
    /// <summary>
    /// 03恢复型
    /// </summary>
    internal class MagicRestore : BaseMagic
    {
        #region 属性

        //HACK 吃药状态相关需要确认
        /// <summary>
        /// debuff抵御
        /// 低四位，毒、乱、封、眠 是否具有医疗相应异常状态的能力
        /// </summary>
        public CombatBuff DefendDeBuff { get; private set; }

        /// <summary>
        /// 0~8000，表示被施展者恢复生命的数值。
        /// </summary>
        public int RestoreHp { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 03恢复型
        /// </summary>
        /// <param name="context"></param>
        public MagicRestore(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void Use(FightingCharacter user, FightingCharacter target)
        {
            if (user.MP < CostMp)
            {
                return;
            }

            user.MP -= CostMp;

            target.HP += RestoreHp;
            if (target.HP > target.MaxHP)
            {
                target.HP = target.MaxHP;
            }
            target.DelDebuff(DefendDeBuff);
        }

        protected override void SetOtherData(byte[] buf, int offset)
        {
            RestoreHp = buf.Get2BytesUInt(offset + 0x12);
            DefendDeBuff = (CombatBuff)buf[offset + 0x18];
        }

        #endregion 方法
    }
}