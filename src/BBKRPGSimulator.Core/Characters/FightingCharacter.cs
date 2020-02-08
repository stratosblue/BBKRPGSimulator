using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// 战斗角色信息
    /// </summary>
    internal abstract class FightingCharacter : Character
    {
        #region 字段

        /// <summary>
        /// 攻击
        /// </summary>
        private int _attack;

        /// <summary>
        /// 防御
        /// </summary>
        private int _defend;

        /// <summary>
        /// 当前HP
        /// </summary>
        private int _hp;

        /// <summary>
        /// 灵力
        /// </summary>
        private int _lingli;

        /// <summary>
        /// 幸运
        /// </summary>
        private int _luck;

        /// <summary>
        /// 最大血量
        /// </summary>
        private int _maxHP;

        /// <summary>
        /// 最大MP
        /// </summary>
        private int _maxMP;

        /// <summary>
        /// 当前MP
        /// </summary>
        private int _mp;

        /// <summary>
        /// 身法
        /// </summary>
        private int _speed;

        #endregion 字段

        #region 属性

        #region BUFF

        /// <summary>
        /// 普通攻击产生(全体)毒乱封眠，对于主角，只有武器具有该效果
        /// </summary>
        protected CombatBuff AttackBuff { get; set; }

        /// <summary>
        /// 普通攻击产生的Buff持续回合数
        /// 毒乱封眠
        /// </summary>
        protected int[] AttackBuffRound { get; } = new int[4];

        /// <summary>
        /// 身中异常状态
        /// </summary>
        protected CombatBuff DeBuff { get; set; }

        /// <summary>
        /// 异常状态持续时间
        /// 毒乱封眠
        /// </summary>
        public int[] DeBuffRound { get; } = new int[4];

        /// <summary>
        /// 免疫异常状态，不同装备可能具有相同的免疫效果，叠加之
        /// 毒乱封眠
        /// </summary>
        protected int[] ResistanceBuff { get; } = new int[4];

        /// <summary>
        /// 免疫状态持续回合
        /// 毒乱封眠
        /// </summary>
        protected int[] ResistanceBuffRound { get; } = new int[4];

        #endregion BUFF

        /// <summary>
        /// 攻击
        /// </summary>
        public int Attack
        {
            get => _attack;
            set
            {
                if (value > 999)
                {
                    _attack = 999;
                }
                else
                {
                    _attack = value;
                }
            }
        }

        /// <summary>
        /// 防御
        /// </summary>
        public int Defend
        {
            get => _defend;
            set
            {
                if (value > 999)
                {
                    _defend = 999;
                }
                else
                {
                    _defend = value;
                }
            }
        }

        /// <summary>
        /// 人物战斗图
        /// </summary>
        public FightingSprite FightingSprite { get; protected set; }

        /// <summary>
        /// 当前HP
        /// </summary>
        public int HP
        {
            get => _hp;
            set
            {
                if (value > MaxHP)
                {
                    _hp = MaxHP;
                }
                else
                {
                    _hp = value;
                }
            }
        }

        /// <summary>
        /// 是否存活
        /// </summary>
        public bool IsAlive => _hp > 0;

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool IsVisiable { get; set; } = true;

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 灵力
        /// </summary>
        public int Lingli
        {
            get => _lingli;
            set
            {
                if (value > 99)
                {
                    _lingli = 99;
                }
                else
                {
                    _lingli = value;
                }
            }
        }

        /// <summary>
        /// 幸运
        /// </summary>
        public int Luck
        {
            get => _luck;
            set
            {
                if (value > 99)
                {
                    _luck = 99;
                }
                else
                {
                    _luck = value;
                }
            }
        }

        /// <summary>
        /// 魔法链
        /// </summary>
        public ResMagicChain MagicChain { get; protected set; }

        /// <summary>
        /// 最大血量
        /// </summary>
        public int MaxHP
        {
            get => _maxHP;
            set
            {
                if (value > 999)
                {
                    _maxHP = 999;
                }
                else
                {
                    _maxHP = value;
                }
            }
        }

        /// <summary>
        /// 最大MP
        /// </summary>
        public int MaxMP
        {
            get => _maxMP;
            set
            {
                if (value > 999)
                {
                    _maxMP = 999;
                }
                else
                {
                    _maxMP = value;
                }
            }
        }

        /// <summary>
        /// 当前MP
        /// </summary>
        public int MP
        {
            get => _mp;
            set
            {
                if (value > MaxMP)
                {
                    _mp = MaxMP;
                }
                else
                {
                    _mp = value;
                }
            }
        }

        /// <summary>
        /// 身法
        /// </summary>
        public int Speed
        {
            get => _speed;
            set
            {
                if (value > 99)
                {
                    _speed = 99;
                }
                else
                {
                    _speed = value;
                }
            }
        }

        #endregion 属性

        #region 构造函数

        public FightingCharacter(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 增加角色攻击能够产生的异常状态
        /// </summary>
        public void AddAtbuff(CombatBuff buff, int rounds)
        {
            AttackBuff |= buff;
            if (buff.HasFlag(CombatBuff.BUFF_MASK_DU))
            {
                AttackBuffRound[0] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_LUAN))
            {
                AttackBuffRound[1] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_FENG))
            {
                AttackBuffRound[2] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_MIAN))
            {
                AttackBuffRound[3] = rounds;
            }
        }

        /// <summary>
        /// 增加角色能够免疫的状态
        /// </summary>
        public void AddBuff(CombatBuff buff)
        {
            AddBuff(buff, int.MaxValue);
        }

        /// <summary>
        /// 增加角色能够免疫的状态
        /// </summary>
        /// <param name="buff">状态</param>
        /// <param name="rounds">持续回合</param>
        public void AddBuff(CombatBuff buff, int rounds)
        {
            if (buff.HasFlag(CombatBuff.BUFF_MASK_DU))
            {
                ++ResistanceBuff[0];
                ResistanceBuffRound[0] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_LUAN))
            {
                ++ResistanceBuff[1];
                ResistanceBuffRound[1] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_FENG))
            {
                ++ResistanceBuff[2];
                ResistanceBuffRound[2] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_MIAN))
            {
                ++ResistanceBuff[3];
                ResistanceBuffRound[3] = rounds;
            }
        }

        /// <summary>
        /// 增加角色身中的异常状态
        /// </summary>
        public void AddDebuff(CombatBuff buff, int rounds)
        {
            DeBuff |= buff;
            if (buff.HasFlag(CombatBuff.BUFF_MASK_DU))
            {
                DeBuffRound[0] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_LUAN))
            {
                DeBuffRound[1] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_FENG))
            {
                DeBuffRound[2] = rounds;
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_MIAN))
            {
                DeBuffRound[3] = rounds;
            }
        }

        public void DelAtbuff(CombatBuff buff)
        {
            AttackBuff &= (~buff);
        }

        public void DelBuff(CombatBuff buff)
        {
            if (buff.HasFlag(CombatBuff.BUFF_MASK_DU))
            {
                if (--ResistanceBuff[0] < 0)
                {
                    ResistanceBuff[0] = 0;
                }
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_LUAN))
            {
                if (--ResistanceBuff[1] < 0)
                {
                    ResistanceBuff[1] = 0;
                }
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_FENG))
            {
                if (--ResistanceBuff[2] < 0)
                {
                    ResistanceBuff[2] = 0;
                }
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_MIAN))
            {
                if (--ResistanceBuff[3] < 0)
                {
                    ResistanceBuff[3] = 0;
                }
            }
        }

        public void DelDebuff(CombatBuff buff)
        {
            DeBuff &= (~buff);
        }

        /// <summary>
        /// 获取状态持续回合
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public int GetBuffRound(CombatBuff buff)
        {
            if (buff.HasFlag(CombatBuff.BUFF_MASK_DU))
            {
                return ResistanceBuffRound[0];
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_LUAN))
            {
                return ResistanceBuffRound[1];
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_FENG))
            {
                return ResistanceBuffRound[2];
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_MIAN))
            {
                return ResistanceBuffRound[3];
            }
            return 0;
        }

        public int GetCombatLeft()
        {
            return FightingSprite.CombatX - FightingSprite.Width / 2;
        }

        public int GetCombatTop()
        {
            return FightingSprite.CombatY - FightingSprite.Height / 2;
        }

        /// <summary>
        /// 中心坐标
        /// </summary>
        public int GetCombatX()
        {
            return FightingSprite.CombatX;
        }

        /// <summary>
        /// 中心坐标
        /// </summary>
        public int GetCombatY()
        {
            return FightingSprite.CombatY;
        }

        /// <summary>
        /// 攻击是否能够产生异常状态
        /// @param mask 只能为下面几个值，或者他们的位或中的任意一个<p>
        /// <code>BUFF_MASK_DU</code>，
        /// <code>BUFF_MASK_LUAN</code>，
        /// <code>BUFF_MASK_FENG</code>，
        /// <code>BUFF_MASK_MIAN</code>，
        /// @return 物理攻击是否具有mask效果
        /// </summary>
        public bool HasAtbuff(CombatBuff buff)
        {
            return (AttackBuff & buff) == buff;
        }

        /// <summary>
        /// 是否免疫异常状态
        /// </summary>
        public bool HasBuff(CombatBuff buff)
        {
            switch (buff)
            {
                case CombatBuff.BUFF_MASK_MIAN:
                    return ResistanceBuff[3] > 0;

                case CombatBuff.BUFF_MASK_FENG:
                    return ResistanceBuff[2] > 0;

                case CombatBuff.BUFF_MASK_LUAN:
                    return ResistanceBuff[1] > 0;

                case CombatBuff.BUFF_MASK_DU:
                    return ResistanceBuff[0] > 0;
            }
            return false;
        }

        /// <summary>
        /// 是否身中异常状态
        /// </summary>
        public bool HasDebuff(CombatBuff buff)
        {
            return DeBuff.HasFlag(buff);
        }

        /// <summary>
        /// 设置中心坐标
        /// </summary>
        public void SetCombatPos(int x, int y)
        {
            FightingSprite.SetCombatPos(x, y);
        }

        /// <summary>
        /// 使用药品
        /// 返回是否成功使用
        /// </summary>
        /// <param name="goods"></param>
        public bool UseMedicine(BaseGoods goods)
        {
            if (goods is GoodsMedicine medicine)    //普通药物
            {
                if (Context.GoodsManage.DropGoods(medicine.Type, medicine.Index))   //使用成功
                {
                    HP += medicine.AffectHp;
                    MP += medicine.AffectMp;

                    DelDebuff(medicine.EffectBuff);
                    return true;
                }
            }
            else if (goods is GoodsLifeMedicine lifeMedicine)   //灵药
            {
                if (Context.GoodsManage.DropGoods(lifeMedicine.Type, lifeMedicine.Index))   //使用成功
                {
                    HP += (int)(MaxHP * (lifeMedicine.RestorePercent / 100.0));

                    return true;
                }
            }
            else if (goods is GoodsAttributesMedicine attributesMedicine)   //仙药
            {
                if (Context.GoodsManage.DropGoods(attributesMedicine.Type, attributesMedicine.Index))   //使用成功
                {
                    MaxMP += attributesMedicine.MaxMP;
                    MaxHP += attributesMedicine.MaxHP;
                    Defend += attributesMedicine.Defend;
                    Attack += attributesMedicine.Attack;
                    Lingli += attributesMedicine.Lingli;
                    Speed += attributesMedicine.Speed;
                    Luck += attributesMedicine.Luck;

                    return true;
                }
            }
            return false;
        }

        #endregion 方法

        public override void SetData(byte[] buf, int offset)
        {
        }
    }
}