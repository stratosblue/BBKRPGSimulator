using System.IO;
using System.Text;

using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Interface;
using BBKRPGSimulator.Lib;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// 玩家角色
    /// </summary>
    internal class PlayerCharacter : FightingCharacter, ICustomSerializeable
    {
        #region 字段

        /// <summary>
        /// 装备界面从左至右的装备类型号
        /// </summary>
        public static readonly int[] EquipTypes = new int[] { 6, 6, 5, 3, 7, 2, 4, 1 };

        /// <summary>
        /// 头像
        /// </summary>
        private ResImage _imgHead;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 当前经验值
        /// </summary>
        public int CurrentExp { get; private set; }

        /// <summary>
        /// 0装饰 1装饰 2护腕 3脚蹬 4手持 5身穿 6肩披 7头戴
        /// </summary>
        public GoodsEquipment[] Equipments { get; } = new GoodsEquipment[8];

        /// <summary>
        /// 升级链
        /// </summary>
        public ResLevelupChain LevelupChain { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 玩家角色
        /// </summary>
        /// <param name="context"></param>
        public PlayerCharacter(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public void DrawHead(ICanvas canvas, int x, int y)
        {
            if (_imgHead != null)
            {
                _imgHead.Draw(canvas, 1, x, y);
            }
        }

        public void DrawState(ICanvas canvas, int page)
        {
            canvas.DrawLine(37, 10, 37, 87, Context.Util.sBlackPaint);
            if (page == 0)
            {
                TextRender.DrawText(canvas, "等级   " + Level, 41, 4);
                TextRender.DrawText(canvas, "生命   " + HP + "/" + MaxHP, 41, 23);
                TextRender.DrawText(canvas, "真气   " + MP + "/" + MaxMP, 41, 41);
                TextRender.DrawText(canvas, "攻击力 " + Attack, 41, 59);
                TextRender.DrawText(canvas, "防御力 " + Defend, 41, 77);
            }
            else if (page == 1)
            {
                TextRender.DrawText(canvas, "经验值", 41, 4);
                int w = Context.Util.DrawSmallNum(canvas, CurrentExp, 97, 4);
                TextRender.DrawText(canvas, "/", 97 + w + 2, 4);
                Context.Util.DrawSmallNum(canvas, LevelupChain.GetNextLevelExp(Level), 97 + w + 9, 10);
                TextRender.DrawText(canvas, "身法   " + Speed, 41, 23);
                TextRender.DrawText(canvas, "灵力   " + Lingli, 41, 41);
                TextRender.DrawText(canvas, "幸运   " + Luck, 41, 59);
                StringBuilder sb = new StringBuilder("免疫   ");
                StringBuilder tmp = new StringBuilder();
                if (HasBuff(CombatBuff.BUFF_MASK_DU))
                {
                    tmp.Append('毒');
                }
                if (HasBuff(CombatBuff.BUFF_MASK_LUAN))
                {
                    tmp.Append('乱');
                }
                if (HasBuff(CombatBuff.BUFF_MASK_FENG))
                {
                    tmp.Append('封');
                }
                if (HasBuff(CombatBuff.BUFF_MASK_MIAN))
                {
                    tmp.Append('眠');
                }
                if (tmp.Length > 0)
                {
                    sb.Append(tmp);
                }
                else
                {
                    sb.Append('无');
                }
                TextRender.DrawText(canvas, sb.ToString(), 41, 77);
            }
        }

        /// <summary>
        /// 获得经验值
        /// </summary>
        /// <param name="exp"></param>
        public void GainExperience(int exp)
        {
            CurrentExp += exp;
        }

        /// <summary>
        /// 获取当前装备的指定类型的装备
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GoodsEquipment GetCurrentEquipment(int type)
        {
            for (int i = 0; i < 8; i++)
            {
                if (EquipTypes[i] == type)
                {
                    return Equipments[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 是否已经装备该装备，对装饰检测空位
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasEquipt(int type, int id)
        {
            if (type == 6)
            {
                // 两个位置都装备同一件装备才返回真
                if ((Equipments[0] != null && Equipments[0].Type == type && Equipments[0].Index == id) &&
                    (Equipments[1] != null && Equipments[1].Type == type && Equipments[1].Index == id))
                {
                    return true;
                }
                return false;
            }

            for (int i = 2; i < 8; i++)
            {
                if (Equipments[i] != null && Equipments[i].Type == type
                        && Equipments[i].Index == id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// type型装备位置是否已经有装备
        /// </summary>
        /// <param name="type">装备类型号</param>
        /// <returns></returns>
        public bool HasSpace(int type)
        {
            if (type == 6)
            {
                // 饰品
                if (Equipments[0] == null || Equipments[1] == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    if (EquipTypes[i] == type && Equipments[i] == null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 穿上goods装备
        /// </summary>
        /// <param name="goods"></param>
        public void PutOn(GoodsEquipment goods)
        {
            for (int i = 0; i < 8; i++)
            {
                if (goods.Type == EquipTypes[i])
                {
                    if (Equipments[i] == null)
                    { // 适用2个装饰
                        goods.PutOn(this);
                        Equipments[i] = goods;
                        break;
                    }
                }
            }
        }

        public override void SetData(byte[] buf, int offset)
        {
            Type = buf[offset] & 0xFF;
            Index = buf[offset + 1] & 0xFF;
            _imgHead = Context.LibData.GetImage(1, Index);
            SetWalkingSprite(new WalkingSprite(Context, Type, buf[offset + 0x16] & 0xFF));
            FightingSprite = new FightingSprite(Context, false, Index);
            Direction direction = Direction.North;
            switch (buf[offset + 2] & 0xFF)
            {
                case 1:
                    direction = Direction.North;
                    break;

                case 2:
                    direction = Direction.East;
                    break;

                case 3:
                    direction = Direction.South;
                    break;

                case 4:
                    direction = Direction.West;
                    break;
            }
            Direction = direction;
            SetStep(buf[offset + 3] & 0xff);
            SetPosInMap(buf[offset + 5] & 0xFF, buf[offset + 6] & 0xFF);
            MagicChain = Context.LibData.GetMagicChain(buf[offset + 0x17] & 0xff);
            if (MagicChain != null)
            {
                MagicChain.LearnFromChain(buf[offset + 9] & 0xff);
            }
            else
            {
                MagicChain = new ResMagicChain(Context);
            }
            Name = buf.GetString(offset + 0x0a);
            Level = buf[offset + 0x20] & 0xff;
            MaxHP = buf.Get2BytesUInt(offset + 0x26);
            HP = buf.Get2BytesUInt(offset + 0x28);
            MaxMP = buf.Get2BytesUInt(offset + 0x2a);
            MP = buf.Get2BytesUInt(offset + 0x2c);
            Attack = buf.Get2BytesUInt(offset + 0x2e);
            Defend = buf.Get2BytesUInt(offset + 0x30);
            Speed = buf[offset + 0x36] & 0xff;
            Lingli = buf[offset + 0x37] & 0xff;
            Luck = buf[offset + 0x38] & 0xff;

            CurrentExp = buf.Get2BytesUInt(offset + 0x32);

            LevelupChain = Context.LibData.GetLevelupChain(Index);

            int tmp;

            tmp = buf[offset + 0x1e] & 0xff;
            if (tmp != 0)
            {
                Equipments[0] = Context.LibData.GetGoods(6, tmp) as GoodsEquipment;
            }

            tmp = buf[offset + 0x1f] & 0xff;
            if (tmp != 0)
            {
                Equipments[1] = Context.LibData.GetGoods(6, tmp) as GoodsEquipment;
            }

            tmp = buf[offset + 0x1b] & 0xff;
            if (tmp != 0)
            {
                Equipments[2] = Context.LibData.GetGoods(5, tmp) as GoodsEquipment;
            }

            tmp = buf[offset + 0x1d] & 0xff;
            if (tmp != 0)
            {
                Equipments[3] = Context.LibData.GetGoods(3, tmp) as GoodsEquipment;
            }

            tmp = buf[offset + 0x1c] & 0xff;
            if (tmp != 0)
            {
                Equipments[4] = Context.LibData.GetGoods(7, tmp) as GoodsEquipment;
            }

            tmp = buf[offset + 0x19] & 0xff;
            if (tmp != 0)
            {
                Equipments[5] = Context.LibData.GetGoods(2, tmp) as GoodsEquipment;
            }

            tmp = buf[offset + 0x1a] & 0xff;
            if (tmp != 0)
            {
                Equipments[6] = Context.LibData.GetGoods(4, tmp) as GoodsEquipment;
            }

            tmp = buf[offset + 0x18] & 0xff;
            if (tmp != 0)
            {
                Equipments[7] = Context.LibData.GetGoods(1, tmp) as GoodsEquipment;
            }
        }

        public void SetFrameByState()
        {
            if (IsAlive)
            {
                if (HasDebuff(CombatBuff.BUFF_MASK_MIAN) || HP < MaxHP / 10)
                {
                    FightingSprite.CurrentFrame = 11;
                }
                else
                {
                    FightingSprite.CurrentFrame = 1;
                }
            }
            else
            {
                FightingSprite.CurrentFrame = 12;
            }
        }

        /// <summary>
        /// 脱下类型号为type的装备
        /// </summary>
        /// <param name="type"></param>
        public void TakeOff(int type)
        {
            for (int i = 0; i < 8; i++)
            {
                if (type == EquipTypes[i])
                {
                    if (Equipments[i] != null)
                    {
                        Equipments[i].TakeOff(this);
                        Equipments[i] = null;
                        break;
                    }
                }
            }
        }

        #region 序列化

        public void Deserialize(BinaryReader binaryReader)
        {
            Type = binaryReader.ReadInt32();
            Index = binaryReader.ReadInt32();
            _imgHead = Context.LibData.GetImage(1, Index);
            LevelupChain = Context.LibData.GetLevelupChain(Index);

            SetWalkingSprite(new WalkingSprite(Context, Type, binaryReader.ReadInt32()));
            FightingSprite = new FightingSprite(Context, false, Index);
            Direction = (Direction)binaryReader.ReadInt32();
            SetStep(binaryReader.ReadInt32());
            SetPosInMap(binaryReader.ReadInt32(), binaryReader.ReadInt32());

            var hasMagicChain = binaryReader.ReadBoolean();

            if (hasMagicChain)
            {
                MagicChain = new ResMagicChain(Context);
                MagicChain.Deserialize(binaryReader);
            }

            Name = binaryReader.ReadString();
            Level = binaryReader.ReadInt32();
            MaxHP = binaryReader.ReadInt32();
            HP = binaryReader.ReadInt32();
            MaxMP = binaryReader.ReadInt32();
            MP = binaryReader.ReadInt32();
            Attack = binaryReader.ReadInt32();
            Defend = binaryReader.ReadInt32();
            Speed = binaryReader.ReadInt32();
            Lingli = binaryReader.ReadInt32();
            Luck = binaryReader.ReadInt32();
            CurrentExp = binaryReader.ReadInt32();

            for (int i = 0; i < 8; i++)
            {
                var type = binaryReader.ReadInt32();
                var index = binaryReader.ReadInt32();
                if (type != 0 && index != 0)
                {
                    Equipments[i] = Context.LibData.GetGoods(type, index) as GoodsEquipment;
                }
            }
        }

        public void Serialize(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Type);
            binaryWriter.Write(Index);
            binaryWriter.Write(GetWalkingSpriteId());
            binaryWriter.Write((int)Direction);
            binaryWriter.Write(GetStep());
            binaryWriter.Write(PosInMap.X);
            binaryWriter.Write(PosInMap.Y);

            if (MagicChain != null)
            {
                binaryWriter.Write(true);
                MagicChain.Serialize(binaryWriter);
            }
            else
            {
                binaryWriter.Write(false);
            }

            binaryWriter.Write(Name);
            binaryWriter.Write(Level);
            binaryWriter.Write(MaxHP);
            binaryWriter.Write(HP);
            binaryWriter.Write(MaxMP);
            binaryWriter.Write(MP);
            binaryWriter.Write(Attack);
            binaryWriter.Write(Defend);
            binaryWriter.Write(Speed);
            binaryWriter.Write(Lingli);
            binaryWriter.Write(Luck);
            binaryWriter.Write(CurrentExp);

            for (int i = 0; i < 8; i++)
            {
                if (Equipments[i] == null)
                {
                    binaryWriter.Write(0);
                    binaryWriter.Write(0);
                }
                else
                {
                    binaryWriter.Write(Equipments[i].Type);
                    binaryWriter.Write(Equipments[i].Index);
                }
            }
        }

        #endregion 序列化

        #endregion 方法
    }
}