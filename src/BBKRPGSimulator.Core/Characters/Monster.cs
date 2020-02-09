using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// 怪物
    /// </summary>
    internal class Monster : FightingCharacter
    {
        #region 字段

        /// <summary>
        /// 怪物在屏幕上的偏移
        /// </summary>
        private static readonly int[][] _monsterPos = new int[][] { new int[] { 12, 25 }, new int[] { 44, 14 }, new int[] { 82, 11 } };

        /// <summary>
        /// 携带的物品，可以能被偷 type id num
        /// </summary>
        private readonly int[] _carryGoods = new int[3];

        /// <summary>
        /// 可掉落物品 type id num
        /// </summary>
        private readonly int[] _dropGoods = new int[3];

        #endregion 字段

        #region 属性

        /// <summary>
        /// 异常状态持续回合
        /// </summary>
        public int BuffLastRound { get; private set; }

        /// <summary>
        /// 击杀怪物的经验
        /// </summary>
        public int EXP { get; private set; }

        /// <summary>
        /// 击杀怪物的金钱
        /// </summary>
        public int Money { get; private set; }

        /// <summary>
        /// 智商，影响魔法使用率
        /// </summary>
        public int MonsterIQ { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 怪物
        /// </summary>
        /// <param name="context"></param>
        public Monster(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 怪物掉落的物品
        /// </summary>
        public BaseGoods GetDropGoods()
        {
            if (_dropGoods[0] == 0 || _dropGoods[1] == 0 || _dropGoods[2] == 0)
            {
                return null;
            }

            BaseGoods goods = Context.LibData.GetGoods(_dropGoods[0], _dropGoods[1]);
            goods.GoodsNum = _dropGoods[2];
            return goods;
        }

        public override void SetData(byte[] buf, int offset)
        {
            Type = buf[offset] & 0xff;
            Index = buf[offset + 1] & 0xff;
            ResMagicChain magicChain = Context.LibData.GetMagicChain(buf[offset + 0x2f] & 0xff);
            if (magicChain != null)
            {
                magicChain.LearnFromChain(buf[offset + 2] & 0xff);
                MagicChain = magicChain;
            }

            AddBuff((CombatBuff)(buf[offset + 3] & 0xff));
            AttackBuff = (CombatBuff)(buf[offset + 4] & 0xff);
            BuffLastRound = buf[offset + 0x17] & 0xff;
            Name = buf.GetString(offset + 6);
            Level = buf[offset + 0x12] & 0xff;
            MaxHP = buf.Get2BytesUInt(offset + 0x18);
            HP = buf.Get2BytesUInt(offset + 0x1a);
            MaxMP = buf.Get2BytesUInt(offset + 0x1c);
            MP = buf.Get2BytesUInt(offset + 0x1e);
            Attack = buf.Get2BytesUInt(offset + 0x20);
            Defend = buf.Get2BytesUInt(offset + 0x22);
            Speed = buf[offset + 0x13] & 0xff;
            Lingli = buf[offset + 0x14] & 0xff;
            Luck = buf[offset + 0x16] & 0xff;
            MonsterIQ = buf[offset + 0x15] & 0xff;
            Money = buf.Get2BytesUInt(offset + 0x24);
            EXP = buf.Get2BytesUInt(offset + 0x26);
            _carryGoods[0] = (int)buf[offset + 0x28] & 0xff;
            _carryGoods[1] = (int)buf[offset + 0x29] & 0xff;
            _carryGoods[2] = (int)buf[offset + 0x2a] & 0xff;
            _dropGoods[0] = (int)buf[offset + 0x2b] & 0xff;
            _dropGoods[1] = (int)buf[offset + 0x2c] & 0xff;
            _dropGoods[2] = (int)buf[offset + 0x2d] & 0xff;
            FightingSprite = new FightingSprite(Context, true, (int)buf[offset + 0x2e] & 0xff);
        }

        /// <summary>
        ///
        /// @param i 屏幕上的位置
        /// </summary>
        public void SetOriginalCombatPos(int i)
        {
            FightingSprite fightingSprite = FightingSprite;
            fightingSprite.SetCombatPos(_monsterPos[i][0] - (fightingSprite.Width / 6) + fightingSprite.Width / 2,
                    _monsterPos[i][1] - (fightingSprite.Height / 10) + fightingSprite.Height / 2);
        }

        #endregion 方法
    }
}