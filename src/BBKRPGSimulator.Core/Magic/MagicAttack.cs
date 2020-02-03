using System.Collections.Generic;

using BBKRPGSimulator.Characters;

namespace BBKRPGSimulator.Magic
{
    /// <summary>
    /// 01攻击型魔法
    /// </summary>
    internal class MagicAttack : BaseMagic
    {
        #region 属性

        /// <summary>
        /// 0~100，表示敌人的攻击力减弱的百分比
        /// </summary>
        public int AffectAt { get; private set; }

        /// <summary>
        /// 高四位 持续回合，低四位毒、乱、封、眠
        /// </summary>
        public int AffectBuff { get; private set; }

        /// <summary>
        /// 0~100，表示敌人的防御力减弱的百分比
        /// </summary>
        public int AffectDf { get; private set; }

        /// <summary>
        /// -8000~+8000，为正数时表示敌人损失生命的基数，为负数时表示从敌人身上吸取生命的基数
        /// </summary>
        public int AffectHp { get; private set; }

        /// <summary>
        /// -8000~+8000，为正数时表示敌人损失真气的基数，为负数时表示从敌人身上吸取真气的基数
        /// </summary>
        public int AffectMp { get; private set; }

        /// <summary>
        /// 速 0~100，表示敌人的身法减慢的百分比
        /// </summary>
        public int AffectSpeed { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 01攻击型魔法
        /// </summary>
        /// <param name="context"></param>
        public MagicAttack(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 单体使用
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        public override void Use(FightingCharacter user, FightingCharacter target)
        {
            user.MP = user.MP - CostMp;
            target.HP = target.HP - AffectHp;
            //TODO 计算BUFF
        }

        /// <summary>
        /// 群体使用
        /// </summary>
        /// <param name="user"></param>
        /// <param name="targets"></param>
        public void Use(FightingCharacter user, List<FightingCharacter> targets)
        {
            user.MP = user.MP - CostMp;

            foreach (FightingCharacter fc in targets)
            {
                fc.HP -= AffectHp;
            }
            //TODO 计算BUFF
        }

        protected override void SetOtherData(byte[] buf, int offset)
        {
            AffectHp = buf.Get2BytesInt(offset + 0x12);
            AffectMp = buf.Get2BytesInt(offset + 0x14);
            AffectDf = (int)buf[offset + 0x16] & 0xff;
            AffectAt = (int)buf[offset + 0x17] & 0xff;
            AffectBuff = (int)buf[offset + 0x18] & 0xff;
            AffectSpeed = (int)buf[offset + 0x19] & 0xff;
        }

        #endregion 方法
    }
}