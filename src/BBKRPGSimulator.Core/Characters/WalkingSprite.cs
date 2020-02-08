using System.IO;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Interface;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// 行走精灵
    /// </summary>
    internal class WalkingSprite : ContextDependent, ICustomSerializeable
    {
        #region 字段

        /// <summary>
        /// 行走的图像索引偏移
        /// </summary>
        private static readonly int[] OFFSET = new int[] { 0, 1, 2, 1 };

        /// <summary>
        /// 面向
        /// </summary>
        private int _face = 1;

        /// <summary>
        /// 图像资源
        /// </summary>
        private ResImage _image;

        /// <summary>
        /// 脚步
        /// </summary>
        private int _step = 0;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 资源ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 脚步
        /// </summary>
        public int Step
        {
            get => _step;
            set
            {
                _step = value % 4;
            }
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        public int Type { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 行走精灵
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public WalkingSprite(SimulatorContext context, int type, int id) : base(context)
        {
            Type = type;
            Id = id;
            RefreshData();
        }

        /// <summary>
        /// 行走精灵
        /// </summary>
        /// <param name="context"></param>
        private WalkingSprite(SimulatorContext context) : base(context)
        { }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas, int x, int y)
        {
            y = y + 16 - _image.Height;
            if (x + _image.Width > 0 && x < 160 - 16 &&
                    y + _image.Height > 0 && y < 96)
            {
                _image.Draw(canvas, _face + OFFSET[Step], x + Constants.MAP_LEFT_OFFSET, y);
            }
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        public void RefreshData()
        {
            _image = Context.LibData.GetCharacterImage(Type, Id);
        }

        public void SetDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    _face = 1;
                    break;

                case Direction.East:
                    _face = 4;
                    break;

                case Direction.South:
                    _face = 7;
                    break;

                case Direction.West:
                    _face = 10;
                    break;
            }
        }

        public override string ToString()
        {
            return $"{nameof(WalkingSprite)} - Tyep: {Type} - Id: {Id}";
        }

        public void Walk(Direction direction)
        {
            SetDirection(direction);
            Walk();
        }

        public void Walk()
        {
            ++Step;
            Step %= 4;
        }

        #endregion 方法

        #region 序列化

        public static WalkingSprite DeserializeFromStream(SimulatorContext context, BinaryReader binaryReader)
        {
            var result = new WalkingSprite(context);
            result.Deserialize(binaryReader);
            return result;
        }

        public void Deserialize(BinaryReader binaryReader)
        {
            Type = binaryReader.ReadInt32();
            Id = binaryReader.ReadInt32();
            _face = binaryReader.ReadInt32();
            Step = binaryReader.ReadInt32();
        }

        public void Serialize(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Type);
            binaryWriter.Write(Id);
            binaryWriter.Write(_face);
            binaryWriter.Write(Step);
        }

        #endregion 序列化
    }
}