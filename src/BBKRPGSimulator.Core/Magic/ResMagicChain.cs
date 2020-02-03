using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Magic
{
    /// <summary>
    /// 魔法链资源
    /// </summary>
    internal class ResMagicChain : ResBase
    {
        #region 字段

        /// <summary>
        /// 魔法列表
        /// </summary>
        private BaseMagic[] _magics;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 已经学会的魔法数量
        /// </summary>
        public int LearnCount { get; set; }

        /// <summary>
        /// 魔法数量
        /// </summary>
        public int MagicCount { get; private set; }

        #endregion 属性

        #region 索引器

        /// <summary>
        /// 获取链中的第index个魔法
        /// 不存在则返回空
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BaseMagic this[int index]
        {
            get
            {
                if (_magics?.Length > index)
                {
                    return _magics[index];
                }
                return null;
            }
        }

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
        /// 学会魔法数量加一
        /// </summary>
        public void LearnNextMagic()
        {
            ++LearnCount;
        }

        public override void SetData(byte[] buf, int offset)
        {
            Type = (int)buf[offset] & 0xff;
            Index = (int)buf[offset + 1] & 0xff;
            MagicCount = (int)buf[offset + 2] & 0xff;

            int index = offset + 3;
            _magics = new BaseMagic[MagicCount];
            for (int i = 0; i < MagicCount; i++)
            {
                _magics[i] = Context.LibData.GetMagic(buf[index++], buf[index++]);
            }
        }

        #endregion 方法
    }
}