using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Magic
{
    /// <summary>
    /// 魔法基础类
    /// </summary>
    internal abstract class BaseMagic : ResBase
    {
        #region 属性

        /// <summary>
        /// 战斗中使用魔法时播放的动画
        /// </summary>
        public ResSrs Animation { get; private set; }

        /// <summary>
        /// 耗费MP
        /// </summary>
        public int CostMp { get; private set; }

        /// <summary>
        /// 是否影响全体
        /// </summary>
        public bool IsEffectAll { get; private set; }

        /// <summary>
        /// 魔法描述
        /// </summary>
        public string MagicDescription { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 魔法的持续回合
        /// </summary>
        public int Round { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 魔法基础类
        /// </summary>
        /// <param name="context"></param>
        public BaseMagic(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void SetData(byte[] buf, int offset)
        {
            Type = buf[offset] & 0xFF;
            Index = buf[offset + 1] & 0xFF;
            Round = buf[offset + 3] & 0x7f;
            IsEffectAll = (buf[offset + 3] & 0x80) != 0;
            CostMp = buf[offset + 4];
            Animation = Context.LibData.GetSrs(2, buf[offset + 5] & 0xFF);
            Name = buf.GetString(offset + 6);
            if ((buf[offset + 2] & 0xff) > 0x70)
            {
                // 魔法描述过长
                buf[offset + 0x70] = 0;
            }
            MagicDescription = buf.GetString(offset + 0x1a);
            SetOtherData(buf, offset);
        }

        /// <summary>
        /// 使用魔法
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        public virtual void Use(FightingCharacter user, FightingCharacter target)
        {
        }

        protected abstract void SetOtherData(byte[] buf, int offset);

        #endregion 方法
    }
}