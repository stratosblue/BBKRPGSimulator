using System;

using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// 等级链资源
    /// </summary>
    internal class ResLevelupChain : ResBase
    {
        #region 字段

        /// <summary>
        /// 一个级别数据所占字节数
        /// </summary>
        private const int LEVEL_BYTES = 20;

        /// <summary>
        /// 等级数据
        /// </summary>
        private byte[] _levelData;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 最高级别
        /// </summary>
        public int MaxLevel { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 等级链资源
        /// </summary>
        /// <param name="context"></param>
        public ResLevelupChain(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 获取指定等级的攻击力
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetAttack(int level)
        {
            if (level <= MaxLevel)
            {
                return _levelData.Get2BytesUInt(8 + level * LEVEL_BYTES - LEVEL_BYTES);
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的防御力
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetDefend(int level)
        {
            if (level <= MaxLevel)
            {
                return _levelData.Get2BytesUInt(10 + level * LEVEL_BYTES - LEVEL_BYTES);
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的HP
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetHP(int level)
        {
            if (level <= MaxLevel)
            {
                return _levelData.Get2BytesUInt(2 + level * LEVEL_BYTES - LEVEL_BYTES);
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的魔法学习数量
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetLearnMagicNum(int level)
        {
            if (level <= MaxLevel)
            {
                return (int)_levelData[level * LEVEL_BYTES - LEVEL_BYTES + 19] & 0xff;
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的灵力
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetLingli(int level)
        {
            if (level <= MaxLevel)
            {
                return (int)_levelData[level * LEVEL_BYTES - LEVEL_BYTES + 17] & 0xff;
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的幸运
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetLuck(int level)
        {
            if (level <= MaxLevel)
            {
                return (int)_levelData[level * LEVEL_BYTES - LEVEL_BYTES + 18] & 0xff;
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的最大HP
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetMaxHP(int level)
        {
            if (level <= MaxLevel)
            {
                return _levelData.Get2BytesUInt(level * LEVEL_BYTES - LEVEL_BYTES);
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的最大MP
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetMaxMP(int level)
        {
            if (level <= MaxLevel)
            {
                return _levelData.Get2BytesUInt(4 + level * LEVEL_BYTES - LEVEL_BYTES);
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的MP
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetMP(int level)
        {
            if (level <= MaxLevel)
            {
                return _levelData.Get2BytesUInt(6 + level * LEVEL_BYTES - LEVEL_BYTES);
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级升级所需要的经验
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetNextLevelExp(int level)
        {
            if (level <= MaxLevel)
            {
                return _levelData.Get2BytesUInt(14 + level * LEVEL_BYTES - LEVEL_BYTES);
            }
            return 0;
        }

        /// <summary>
        /// 获取指定等级的速度
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public int GetSpeed(int level)
        {
            if (level <= MaxLevel)
            {
                return (int)_levelData[level * LEVEL_BYTES - LEVEL_BYTES + 16] & 0xff;
            }
            return 0;
        }

        public override void SetData(byte[] buf, int offset)
        {
            Type = (int)buf[offset] & 0xff;
            Index = (int)buf[offset + 1] & 0xff;
            MaxLevel = (int)buf[offset + 2] & 0xff;

            _levelData = new byte[MaxLevel * LEVEL_BYTES];

            Array.Copy(buf, offset + 4, _levelData, 0, _levelData.Length);
        }

        #endregion 方法
    }
}