using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Lib
{
    /// <summary>
    /// 库文件资源
    /// </summary>
    internal sealed class DatLib : ContextDependent
    {
        #region 字段

        /// <summary>
        /// 角色图片
        /// </summary>
        public const int RES_ACP = 8;

        /// <summary>
        /// 角色资源
        /// </summary>
        public const int RES_ARS = 3;

        /// <summary>
        /// 道具图片
        /// </summary>
        public const int RES_GDP = 9;

        /// <summary>
        /// 特效图片
        /// </summary>
        public const int RES_GGJ = 10;

        /// <summary>
        /// 道具资源
        /// </summary>
        public const int RES_GRS = 6;

        /// <summary>
        /// 剧情脚本
        /// </summary>
        public const int RES_GUT = 1;

        /// <summary>
        /// 地图资源
        /// </summary>
        public const int RES_MAP = 2;

        /// <summary>
        /// 链资源
        /// </summary>
        public const int RES_MLR = 12;

        /// <summary>
        /// 魔法资源
        /// </summary>
        public const int RES_MRS = 4;

        /// <summary>
        /// 杂类图片
        /// </summary>
        public const int RES_PIC = 11;

        /// <summary>
        /// 特效资源
        /// </summary>
        public const int RES_SRS = 5;

        /// <summary>
        /// tile资源
        /// </summary>
        public const int RES_TIL = 7;

        /// <summary>
        /// 库文件内容缓存
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// 保存资源数据相对文件首字节的偏移量
        /// </summary>
        private Dictionary<int, int> _dataOffset = new Dictionary<int, int>(2048);

        #endregion 字段

        #region 属性

        /// <summary>
        /// 数据的摘要
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// lib的名称
        /// </summary>
        public string Name { get; private set; }

        #endregion 属性

        #region 构造函数

        public DatLib(Stream stream, SimulatorContext context) : base(context)
        {
            Load(stream);
        }

        public DatLib(byte[] data, SimulatorContext context) : base(context)
        {
            Load(data);
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 获取资源
        /// </summary>
        /// <param name="resType">资源文件类型号1-12</param>
        /// <param name="type">资源类型</param>
        /// <param name="index">资源索引号</param>
        /// <returns>资源对象，不存在则返回</returns>
        public ResBase GetRes(int resType, int type, int index)
        {
            ResBase rtn = null;
            int offset = GetDataOffset(resType, type, index);

            //TODO 超过索引的资源直接不处理？？？
            if (offset != -1 && offset < _data.Length)
            {
                switch (resType)
                {
                    case RES_GUT:
                        rtn = new ResGut(Context);
                        break;

                    case RES_MAP:
                        rtn = new ResMap(Context);
                        break;

                    case RES_ARS:
                        switch (type)
                        {
                            case 1: // 玩家角色
                                rtn = new PlayerCharacter(Context);
                                break;

                            case 2: // NPC角色
                                rtn = new NPC(Context);
                                break;

                            case 3: // 敌人角色
                                rtn = new Monster(Context);
                                break;

                            case 4: // 场景对象
                                rtn = new SceneObj(Context);
                                break;

                            default:
                                rtn = null;
                                break;
                        }
                        break;

                    case RES_MRS:
                        rtn = InternalGetMagic(type, index);
                        break;

                    case RES_SRS:
                        rtn = new ResSrs(Context);
                        break;

                    case RES_GRS:
                        rtn = InternalGetGoods(type, index);
                        break;

                    case RES_TIL:
                    case RES_ACP:
                    case RES_GDP:
                    case RES_GGJ:
                    case RES_PIC:
                        rtn = new ResImage(Context);
                        break;

                    case RES_MLR:
                        if (type == 1)
                        {
                            rtn = new ResMagicChain(Context);
                        }
                        else if (type == 2)
                        {
                            rtn = new ResLevelupChain(Context);
                        }
                        break;
                }
                rtn.SetData(_data, offset);
            }
            else
            { // 资源不存在
              //Log.e("Context.LibData.GetRes", "resType:" + resType + " type:" + type + " index:" + index + " not found.");
            }

            return rtn;
        }

        #region 获取资源的封装

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Character GetCharacter(int type, int index)
        {
            return GetRes(RES_ARS, type, index) as Character;
        }

        /// <summary>
        /// 获取角色图片
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResImage GetCharacterImage(int type, int index)
        {
            return GetRes(RES_ACP, type, index) as ResImage;
        }

        /// <summary>
        /// 获取物品
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public BaseGoods GetGoods(int type, int index)
        {
            return GetRes(RES_GRS, type, index) as BaseGoods;
        }

        /// <summary>
        /// 获取道具图片
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResImage GetGoodsImage(int type, int index)
        {
            return GetRes(RES_GDP, type, index) as ResImage;
        }

        /// <summary>
        /// 获取脚本
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResGut GetGut(int type, int index)
        {
            return GetRes(RES_GUT, type, index) as ResGut;
        }

        /// <summary>
        /// 获取杂类图片
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResImage GetImage(int type, int index)
        {
            return GetRes(RES_PIC, type, index) as ResImage;
        }

        /// <summary>
        /// 获取等级链资源
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResLevelupChain GetLevelupChain(int index)
        {
            return GetRes(RES_MLR, 2, index) as ResLevelupChain;
        }

        /// <summary>
        /// 获取魔法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public BaseMagic GetMagic(int type, int index)
        {
            return GetRes(RES_MRS, type, index) as BaseMagic;
        }

        /// <summary>
        /// 获取魔法链资源
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResMagicChain GetMagicChain(int index)
        {
            return GetRes(RES_MLR, 1, index) as ResMagicChain;
        }

        /// <summary>
        /// 获取地图
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResMap GetMap(int type, int index)
        {
            return GetRes(RES_MAP, type, index) as ResMap;
        }

        /// <summary>
        /// 获取特效图片
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResImage GetSEImage(int type, int index)
        {
            return GetRes(RES_GGJ, type, index) as ResImage;
        }

        /// <summary>
        /// 获取特效资源
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResSrs GetSrs(int type, int index)
        {
            return GetRes(RES_SRS, type, index) as ResSrs;
        }

        /// <summary>
        /// 获取tile图片
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ResImage GetTileImage(int type, int index)
        {
            return GetRes(RES_TIL, type, index) as ResImage;
        }

        #endregion 获取资源的封装

        #endregion 方法

        #region 加载

        private void Load(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            Load(buffer);
        }

        private void Load(byte[] data)
        {
            _data = data;

            Hash = BitConverter.ToString(SHA1.Create().ComputeHash(_data)).Replace("-", string.Empty).ToUpperInvariant();

            GetAllResOffset();
        }

        #endregion 加载

        #region Internal

        /// <summary>
        /// 获取所有资源的偏移
        /// </summary>
        private void GetAllResOffset()
        {
            Name = Utilities.GetGameName(_data);

            int i = 0x10, j = 0x2000;

            while (i < _data.Length - 3 && j < _data.Length - 3)
            {
                int key = GetKey(_data[i++], _data[i++], _data[i++] & 0xFF);

                int block = _data[j++] & 0xFF;
                int low = _data[j++] & 0xFF;
                int high = _data[j++] & 0xFF;
                int value = block * 0x4000 | (high << 8 | low);

                if (!_dataOffset.ContainsKey(key))
                {
                    _dataOffset.Add(key, value);
                }
            }
        }

        /// <summary>
        /// 获取资源所在位置
        /// </summary>
        /// <param name="resType">资源文件类型号1-12</param>
        /// <param name="type">资源类型</param>
        /// <param name="index">资源索引号</param>
        /// <returns>资源所在位置, 返回-1表示不存在</returns>
        private int GetDataOffset(int resType, int type, int index)
        {
            if (!_dataOffset.ContainsKey(GetKey(resType, type, index)))
            {
                return -1;
            }
            int i = _dataOffset[GetKey(resType, type, index)];

            return i;

            //return mDataOffset.get(getKey(resType, type, index), -1);
        }

        /// <summary>
        /// 获取资源的KEY
        /// </summary>
        /// <param name="resType">资源文件类型号1-12</param>
        /// <param name="type">资源类型</param>
        /// <param name="index">资源索引号</param>
        /// <returns></returns>
        private int GetKey(int resType, int type, int index)
        {
            return (resType << 16) | (type << 8) | index;
        }

        /// <summary>
        /// 获取物品
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private BaseGoods InternalGetGoods(int type, int index)
        {
            if (type >= 1 && type <= 5)
            {
                return new GoodsEquipment(Context);
            }
            BaseGoods rtn = null;
            switch (type)
            {
                case 6:
                    rtn = new GoodsDecorations(Context);
                    break;

                case 7:
                    rtn = new GoodsWeapon(Context);
                    break;

                case 8:
                    rtn = new GoodsHiddenWeapon(Context);
                    break;

                case 9:
                    rtn = new GoodsMedicine(Context);
                    break;

                case 10:
                    rtn = new GoodsLifeMedicine(Context);
                    break;

                case 11:
                    rtn = new GoodsAttributesMedicine(Context);
                    break;

                case 12:
                    rtn = new GoodsStimulant(Context);
                    break;

                case 13:
                    rtn = new GoodsTudun(Context);
                    break;

                case 14:
                    rtn = new GoodsDrama(Context);
                    break;
            }
            return rtn;
        }

        /// <summary>
        /// 获取魔法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private ResBase InternalGetMagic(int type, int index)
        {
            switch (type)
            {
                case 1: return new MagicAttack(Context);
                case 2: return new MagicEnhance(Context);
                case 3: return new MagicRestore(Context);
                case 4: return new MagicAuxiliary(Context);
                case 5: return new MagicSpecial(Context);
            }
            return null;
        }

        #endregion Internal
    }
}