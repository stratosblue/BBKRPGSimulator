namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// 场景物品
    /// </summary>
    internal class SceneObj : NPC
    {
        #region 构造函数

        /// <summary>
        /// 场景物品
        /// </summary>
        /// <param name="context"></param>
        public SceneObj(SimulatorContext context) : base(context)
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
            // 行走图
            SetWalkingSprite(new WalkingSprite(Context, 4, (int)buf[offset + 0x16] & 0xFF));
            // 面向
            Direction = Direction.North;
            // 脚步
            SetStep((int)buf[offset + 3] & 0xFF);
        }

        public override void Walk()
        {
        }

        public override void Walk(Direction direction)
        {
        }

        public override void WalkStay(Direction direction)
        {
        }

        #endregion 方法

        //TODO Java中这里还要写序列化？序列化！！！
        //      public void writeExternal(ObjectOutput out)
        //      {
        //out.writeInt(mType);
        //out.writeInt(mIndex);
        //out.writeInt(getCharacterState());
        //out.writeObject(getName());
        //out.writeInt(mDelay);
        //out.writeInt(getWalkingSpriteId());
        //out.writeObject(getDirection());
        //out.writeInt(getStep());
        //out.writeInt(getPosInMap().x);
        //out.writeInt(getPosInMap().y);
        //      }

        //      public void readExternal(ObjectInput in)
        //      {
        //          mType = in.readInt();
        //          mIndex = in.readInt();

        //          setCharacterState(in.readInt());
        //          setName((String)in.readObject());
        //          mDelay = in.readInt();

        //          setWalkingSprite(new WalkingSprite(4, in.readInt()));
        //          setDirection((Direction)in.readObject());
        //          setStep(in.readInt());
        //          setPosInMap(in.readInt(), in.readInt());
        //      }
    }
}