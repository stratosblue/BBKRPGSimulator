using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Definitions;

namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 07武器类
    /// </summary>
    internal class GoodsWeapon : GoodsEquipment
    {
        #region 构造函数

        /// <summary>
        /// 07武器类
        /// </summary>
        /// <param name="context"></param>
        public GoodsWeapon(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 是否攻击全部
        /// </summary>
        /// <returns></returns>
        public bool AttackAll()
        {
            return Buff.HasFlag(CombatBuff.BUFF_MASK_ALL);
        }

        public override void PutOn(PlayerCharacter character)
        {
            base.PutOn(character);
            character.AddAtbuff(Buff, SumRound);
        }

        public override void TakeOff(PlayerCharacter character)
        {
            base.TakeOff(character);
            character.DelAtbuff(Buff);
        }

        #endregion 方法
    }
}