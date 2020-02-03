using System.IO;

using BBKRPGSimulator.Interface;

namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// NPC信息
    /// </summary>
    internal class NPC : Character, ICustomSerializeable
    {
        #region 字段

        /// <summary>
        /// 活动总时间
        /// </summary>
        private long _activeCount = 0;

        /// <summary>
        /// 暂停行动总时间
        /// </summary>
        private long _pauseCount = 0;

        /// <summary>
        /// 行走总时间
        /// </summary>
        private long _walkingCount = 0;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 暂停状态，等到延时到了后转变为巡逻状态
        /// </summary>
        protected int Delay { get; set; } = 0;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// NPC信息
        /// </summary>
        /// <param name="context"></param>
        public NPC(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void SetData(byte[] buf, int offset)
        {
            Type = (int)buf[offset] & 0xFF;
            Index = (int)buf[offset + 1] & 0xFF;
            // 动作状态
            State = (CharacterActionState)(buf[offset + 4] & 0xFF);
            // 姓名
            Name = buf.GetString(offset + 9);
            // 延时
            Delay = (int)buf[offset + 0x15] & 0xFF;
            if (Delay == 0)
            {
                State = CharacterActionState.STATE_STOP;
            }
            // 行走图
            SetWalkingSprite(new WalkingSprite(Context, 2,
                    (int)buf[offset + 0x16] & 0xFF));
            // 面向
            int faceto = (int)buf[offset + 2] & 0xFF;
            Direction d = Direction.North;
            switch (faceto)
            {
                case 1: d = Direction.North; break;
                case 2: d = Direction.East; break;
                case 3: d = Direction.South; break;
                case 4: d = Direction.West; break;
            }
            Direction = d;
            // 脚步
            SetStep((int)buf[offset + 3] & 0xFF);
        }

        public void Update(long delta)
        {
            switch (State)
            {
                case CharacterActionState.STATE_STOP:
                    break;

                case CharacterActionState.STATE_FORCE_MOVE:
                case CharacterActionState.STATE_WALKING:
                    _walkingCount += delta;
                    if (_walkingCount < 500)
                    {
                        break;
                    }
                    _walkingCount = 0;
                    if (Context.Random.Next(5) == 0)
                    {
                        // 五分之一的概率暂停
                        _pauseCount = Delay * 100;
                        State = CharacterActionState.STATE_PAUSE;
                    }
                    else if (Context.Random.Next(5) == 0)
                    {
                        // 五分之一的概率改变方向
                        int i = Context.Random.Next(4);
                        Direction d = Direction.North;
                        switch (i)
                        {
                            case 0: d = Direction.North; break;
                            case 1: d = Direction.East; break;
                            case 2: d = Direction.South; break;
                            case 3: d = Direction.West; break;
                        }
                        Direction = d;
                        Walk();
                    }
                    else
                    {
                        Walk();
                    }
                    break;

                case CharacterActionState.STATE_PAUSE:
                    _pauseCount -= delta;
                    if (_pauseCount < 0)
                    {
                        State = CharacterActionState.STATE_WALKING;
                    }
                    break;

                case CharacterActionState.STATE_ACTIVE:
                    _activeCount += delta;
                    if (_activeCount > 100)
                    {
                        _activeCount = 0;
                        WalkStay();
                    }
                    break;

                default:
                    break;
            }
        }

        public override void Walk()
        {
            int x = PosInMap.X;
            int y = PosInMap.Y;
            switch (Direction)
            {
                case Direction.North:
                    --y;
                    break;

                case Direction.East:
                    ++x;
                    break;

                case Direction.South:
                    ++y;
                    break;

                case Direction.West:
                    --x;
                    break;
            }
            if (Context.SceneMap.CanNPCWalk(x, y))
            {
                base.Walk();
            }
        }

        #endregion 方法

        #region 序列化

        public void Deserialize(BinaryReader binaryReader)
        {
            Type = binaryReader.ReadInt32();

            Index = binaryReader.ReadInt32();

            State = (CharacterActionState)binaryReader.ReadInt32();
            Name = binaryReader.ReadString();
            Delay = binaryReader.ReadInt32();

            WalkingSprite = WalkingSprite.DeserializeFromStream(Context, binaryReader);
            Direction = (Direction)binaryReader.ReadInt32();
            SetStep(binaryReader.ReadInt32());
            _pauseCount = binaryReader.ReadInt64();
            _activeCount = binaryReader.ReadInt64();
            _walkingCount = binaryReader.ReadInt64();

            SetPosInMap(binaryReader.ReadInt32(), binaryReader.ReadInt32());
        }

        public void Serialize(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Type);
            binaryWriter.Write(Index);
            binaryWriter.Write((int)State);
            binaryWriter.Write(Name);
            binaryWriter.Write(Delay);
            WalkingSprite.Serialize(binaryWriter);
            binaryWriter.Write((int)Direction);
            binaryWriter.Write(GetStep());
            binaryWriter.Write(_pauseCount);
            binaryWriter.Write(_activeCount);
            binaryWriter.Write(_walkingCount);
            binaryWriter.Write(PosInMap.X);
            binaryWriter.Write(PosInMap.Y);
        }

        #endregion 序列化
    }
}