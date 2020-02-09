using System.Collections.Generic;
using System.IO;

using BBKRPGSimulator.Interface;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Magic
{
    /// <summary>
    /// 魔法链资源
    /// </summary>
    internal class ResMagicChain : ResBase, ICustomSerializeable
    {
        #region 字段

        /// <summary>
        /// 已学会的魔法列表
        /// </summary>
        private readonly List<BaseMagic> _learnMagics = new List<BaseMagic>();

        /// <summary>
        /// 魔法链的所有魔法列表
        /// </summary>
        private BaseMagic[] _chainMagics;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 已经学会的魔法数量
        /// </summary>
        public int LearnCount => _learnMagics.Count;

        /// <summary>
        /// 魔法数量
        /// </summary>
        public int MagicCount { get; private set; }

        #endregion 属性

        #region 索引器

        /// <summary>
        /// 获取已学会的第index个魔法
        /// 不存在则返回空
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BaseMagic this[int index] => _learnMagics?.Count > index ? _learnMagics[index] : null;

        #endregion 索引器

        #region 构造函数

        /// <summary>
        /// 魔法链资源
        /// </summary>
        /// <param name="context"></param>
        public ResMagicChain(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 学习指定的技能
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public bool LearnMagic(int type, int index)
        {
            var magic = Context.LibData.GetMagic(type, index);
            if (magic != null)
            {
                if (!_learnMagics.Contains(magic))
                {
                    _learnMagics.Add(magic);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从链中学习魔法
        /// </summary>
        /// <param name="index"></param>
        public void LearnFromChain(int index)
        {
            for (int i = 0; i < index; i++)
            {
                if (i < MagicCount)
                {
                    LearnMagic(_chainMagics[i].Type, _chainMagics[i].Index);
                }
            }
        }

        public override void SetData(byte[] buf, int offset)
        {
            Type = buf[offset] & 0xff;
            Index = buf[offset + 1] & 0xff;
            MagicCount = buf[offset + 2] & 0xff;

            int index = offset + 3;
            _chainMagics = new BaseMagic[MagicCount];
            for (var i = 0; i < MagicCount; i++)
            {
                _chainMagics[i] = Context.LibData.GetMagic(buf[index++], buf[index++]);
            }
        }

        public void Deserialize(BinaryReader binaryReader)
        {
            Type = binaryReader.ReadInt32();
            Index = binaryReader.ReadInt32();
            MagicCount = binaryReader.ReadInt32();
            var learnCount = binaryReader.ReadInt32();

            if (MagicCount > 0)
            {
                var tempChain = Context.LibData.GetMagicChain(Index);
                _chainMagics = tempChain._chainMagics;
            }

            if (learnCount > 0)
            {
                for (int i = 0; i < learnCount; i++)
                {
                    var type = binaryReader.ReadInt32();
                    var index = binaryReader.ReadInt32();
                    LearnMagic(type, index);
                }
            }
        }

        public void Serialize(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Type);
            binaryWriter.Write(Index);
            binaryWriter.Write(MagicCount);

            binaryWriter.Write(LearnCount);

            if (LearnCount > 0)
            {
                foreach (var magic in _learnMagics)
                {
                    binaryWriter.Write(magic.Type);
                    binaryWriter.Write(magic.Index);
                }
            }
        }

        #endregion 方法
    }
}