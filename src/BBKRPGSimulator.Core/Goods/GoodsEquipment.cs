using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Definitions;

namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 装备物品
    /// 01冠类，02衣类，03鞋类，04护甲类，05护腕类
    /// </summary>

    internal class GoodsEquipment : BaseGoods
    {
        #region 属性

        /// <summary>
        ///  攻击
        /// </summary>
        public int Attack { get; protected set; }

        /// <summary>
        /// 特殊效果
        /// 毒、乱、封、眠（影响免疫效果，07武器类为攻击效果）
        /// </summary>
        public CombatBuff Buff { get; protected set; }

        /// <summary>
        /// 防御
        /// </summary>
        public int Defend { get; protected set; }

        /// <summary>
        /// 加生命上限
        /// </summary>
        public int Hp { get; protected set; }

        /// <summary>
        /// 灵力
        /// </summary>
        public int Lingli { get; protected set; }

        /// <summary>
        /// 吉运
        /// </summary>
        public int Luck { get; protected set; }

        /// <summary>
        /// 加真气上限
        /// </summary>
        public int Mp { get; protected set; }

        /// <summary>
        /// 身法
        /// </summary>
        public int Speed { get; protected set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 装备物品
        /// 01冠类，02衣类，03鞋类，04护甲类，05护腕类
        /// </summary>
        /// <param name="context"></param>
        public GoodsEquipment(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 角色穿戴装备
        /// </summary>
        /// <param name="character"></param>
        public virtual void PutOn(PlayerCharacter character)
        {
            if (CanPlayerUse(character.Index))
            {
                character.MaxMP = character.MaxMP + Mp;
                character.MaxHP = character.MaxHP + Hp;
                character.Defend = character.Defend + Defend;
                character.Attack = character.Attack + Attack;
                character.Lingli = character.Lingli + Lingli;
                character.Speed = character.Speed + Speed;
                if (!(this is GoodsWeapon))
                {
                    character.AddBuff(Buff); // 添加免疫效果
                }
                character.Luck = character.Luck + Luck;
                if (EventId != 0)
                {
                    // 设置装备触发的事件
                    Context.ScriptProcess.ScriptState.SetEvent(EventId);
                }
            }
        }

        /// <summary>
        /// 角色卸下装备
        /// </summary>
        /// <param name="character"></param>
        public virtual void TakeOff(PlayerCharacter character)
        {
            character.MaxMP = character.MaxMP - Mp;
            character.MaxHP = character.MaxHP - Hp;
            character.Defend = character.Defend - Defend;
            character.Attack = character.Attack - Attack;
            character.Lingli = character.Lingli - Lingli;
            character.Speed = character.Speed - Speed;
            if (!(this is GoodsWeapon))
            {
                character.DelBuff(Buff); // 删掉免疫效果
            }
            character.Luck = character.Luck - Luck;
            if (EventId != 0)
            {
                // 取消该事件
                Context.ScriptProcess.ScriptState.ClearEvent(EventId);
            }
        }

        protected override void SetOtherData(byte[] buf, int offset)
        {
            Mp = buf.Get1ByteInt(offset + 0x16);
            Hp = buf.Get1ByteInt(offset + 0x17);
            Defend = buf.Get1ByteInt(offset + 0x18);
            Attack = (int)buf[offset + 0x19] & 0xff;
            Lingli = buf.Get1ByteInt(offset + 0x1a);
            Speed = buf.Get1ByteInt(offset + 0x1b);
            Buff = (CombatBuff)(buf[offset + 0x1c] & 0xff);
            Luck = buf.Get1ByteInt(offset + 0x1d);
        }

        #endregion 方法
    }
}