using System;

namespace BBKRPGSimulator.Definitions
{
    /// <summary>
    /// 战斗BUFF
    /// </summary>
    [Flags]
    internal enum CombatBuff
    {
        /// <summary>
        /// 没有
        /// </summary>
        NULL = 0,

        /// <summary>
        /// 眠
        /// </summary>
        BUFF_MASK_MIAN = 1,

        /// <summary>
        /// 封
        /// </summary>
        BUFF_MASK_FENG = 2,

        /// <summary>
        /// 乱
        /// </summary>
        BUFF_MASK_LUAN = 4,

        /// <summary>
        /// 毒
        /// </summary>
        BUFF_MASK_DU = 8,

        /// <summary>
        /// 攻击全部、治疗全部
        /// </summary>
        BUFF_MASK_ALL = 16,

        /// <summary>
        /// 攻击
        /// </summary>
        BUFF_MASK_GONG = 32,

        /// <summary>
        /// 防御
        /// </summary>
        BUFF_MASK_FANG = 64,

        /// <summary>
        /// 身法
        /// </summary>
        BUFF_MASK_SU = 128,
    }
}