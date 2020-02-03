using System.Collections.Generic;
using System.IO;
using System.Linq;

using BBKRPGSimulator.Interface;

namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 物品管理
    /// </summary>
    internal class GoodsManage : ContextDependent, ICustomSerializeable
    {
        #region 属性

        /// <summary>
        /// 装备链表
        /// </summary>
        public List<BaseGoods> EquipList { get; private set; } = new List<BaseGoods>();

        /// <summary>
        /// 装备的种类总数
        /// </summary>
        public int EquitTypeCount => EquipList.Count;

        /// <summary>
        /// 物品链表
        /// 只能用一次的物品，暗器、药品等
        /// </summary>
        public List<BaseGoods> GoodsList { get; private set; } = new List<BaseGoods>();

        /// <summary>
        /// 一次性物品的种类总数
        /// </summary>
        public int GoodsTypeCount => GoodsList.Count;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 物品管理
        /// </summary>
        /// <param name="context"></param>
        public GoodsManage(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 在物品列表中增加指定type和index的物品num个
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="num"></param>
        public void AddGoods(int type, int index, int num)
        {
            if (type >= 1 && type <= 7)
            {
                // 装备
                if (EquipList.Where(m => m.Type == type && m.Index == index).FirstOrDefault() is BaseGoods equip)   //已有物品
                {
                    equip.AddGoodsNum(num);
                }
                else    //增加物品
                {
                    BaseGoods newGoods = Context.LibData.GetGoods(type, index);
                    newGoods.GoodsNum = num;
                    EquipList.Add(newGoods);
                }
            }
            else if (type >= 8 && type <= 14)
            {
                // 物品
                if (GoodsList.Where(m => m.Type == type && m.Index == index).FirstOrDefault() is BaseGoods goods)   //已有物品
                {
                    goods.AddGoodsNum(num);
                }
                else    //增加物品
                {
                    BaseGoods newGoods = Context.LibData.GetGoods(type, index);
                    newGoods.GoodsNum = num;
                    GoodsList.Add(newGoods);
                }
            }
        }

        /// <summary>
        /// 在物品列表中增加一个指定type和index的物品
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        public void AddGoods(int type, int index)
        {
            AddGoods(type, index, 1);
        }

        /// <summary>
        /// 清空物品列表
        /// </summary>
        public void Clear()
        {
            EquipList.Clear();
            GoodsList.Clear();
        }

        /// <summary>
        /// 在物品列表中减少一个指定type和index的物品
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns>删除成功返回true不存在该物品返回false</returns>
        public bool DropGoods(int type, int index)
        {
            return DropGoods(type, index, 1);
        }

        /// <summary>
        /// 在物品列表中减少指定type和index的物品num个
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns>使用成功返回true不存在该物品，或者数量不够返回false</returns>
        public bool DropGoods(int type, int index, int num)
        {
            if (type >= 1 && type <= 7)
            {
                // 装备
                if (EquipList.Where(m => m.Type == type && m.Index == index).FirstOrDefault() is BaseGoods equip)   //有物品
                {
                    if (equip.GoodsNum < num)
                    {
                        return false;
                    }
                    else if (equip.GoodsNum == num)
                    {
                        equip.GoodsNum = 0;
                        EquipList.Remove(equip);
                    }
                    else
                    {
                        equip.AddGoodsNum(-num);
                    }
                    return true;
                }
                else    //没有物品
                {
                    return false;
                }
            }
            else if (type >= 8 && type <= 14)
            {
                // 物品
                if (GoodsList.Where(m => m.Type == type && m.Index == index).FirstOrDefault() is BaseGoods goods)   //有物品
                {
                    if (goods.GoodsNum < num)
                    {
                        return false;
                    }
                    else if (goods.GoodsNum == num)
                    {
                        goods.GoodsNum = 0;
                        GoodsList.Remove(goods);
                    }
                    else
                    {
                        goods.AddGoodsNum(-num);
                    }
                    return true;
                }
                else    //没有物品
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取所有物品
        /// </summary>
        /// <returns></returns>
        public List<BaseGoods> GetAllGoods()
        {
            var result = new List<BaseGoods>();
            result.AddRange(GoodsList);
            result.AddRange(EquipList);
            return result;
        }

        /// <summary>
        /// 获取链表中type index号物品
        /// 当该种物品不存在时返回null
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public BaseGoods GetGoods(int type, int index)
        {
            if (type >= 1 && type <= 7)
            {
                return EquipList.Where(m => m.Type == type && m.Index == index).FirstOrDefault();
            }
            else if (type >= 8 && type <= 14)
            {
                return GoodsList.Where(m => m.Type == type && m.Index == index).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 获取链表中type index号物品的数量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetGoodsNum(int type, int index)
        {
            //TODO 此处需要确认改为LINQ后工作是否正常
            int num = 0;
            if (type >= 1 && type <= 7 && EquipList.Where(m => m.Type == type && m.Index == index).FirstOrDefault() is BaseGoods equip)
            {
                num = equip.GoodsNum;
            }
            else if (type >= 8 && type <= 14 && GoodsList.Where(m => m.Type == type && m.Index == index).FirstOrDefault() is BaseGoods goods)
            {
                num = goods.GoodsNum;
            }
            return num;
        }

        #endregion 方法

        #region 序列化

        public void Deserialize(BinaryReader binaryReader)
        {
            Clear();

            int size = binaryReader.ReadInt32();

            for (int i = 0; i < size; i++)
            {
                BaseGoods equipment = Context.LibData.GetGoods(binaryReader.ReadInt32(), binaryReader.ReadInt32());
                equipment.GoodsNum = binaryReader.ReadInt32();
                EquipList.Add(equipment);
            }

            size = binaryReader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                BaseGoods goods = Context.LibData.GetGoods(binaryReader.ReadInt32(), binaryReader.ReadInt32());
                goods.GoodsNum = binaryReader.ReadInt32();
                GoodsList.Add(goods);
            }
        }

        public void Serialize(BinaryWriter binaryWriter)
        {
            int size = EquipList.Count;
            binaryWriter.Write(size);

            for (int i = 0; i < size; i++)
            {
                BaseGoods equipment = EquipList[i];

                binaryWriter.Write(equipment.Type);
                binaryWriter.Write(equipment.Index);
                binaryWriter.Write(equipment.GoodsNum);
            }

            size = GoodsList.Count;
            binaryWriter.Write(size);

            for (int i = 0; i < size; i++)
            {
                BaseGoods goods = GoodsList[i];

                binaryWriter.Write(goods.Type);
                binaryWriter.Write(goods.Index);
                binaryWriter.Write(goods.GoodsNum);
            }
        }

        #endregion 序列化
    }
}