using System;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Goods
{
    /// <summary>
    /// 物品基础类
    /// </summary>
    internal abstract class BaseGoods : ResBase
    {
        #region 字段

        /// <summary>
        /// 物品可用等级，最低位为主角1
        /// </summary>
        private int _enableLevel;

        /// <summary>
        /// 物品图片
        /// </summary>
        private ResImage _image;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 买价
        /// </summary>
        public int BuyPrice { get; private set; }

        /// <summary>
        /// 道具说明
        /// </summary>
        public string Description { get; private set; } = string.Empty;

        /// <summary>
        /// 不为0时装备该道具时，就会设置该事件，而卸下时，就会取消该事件，不能用来典当。
        /// </summary>
        public int EventId { get; private set; } = 0;

        /// <summary>
        /// 物品数量
        /// </summary>
        public int GoodsNum { get; set; } = 0;

        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 卖价
        /// </summary>
        public int SellPrice { get; private set; }

        /// <summary>
        /// 持续回合
        /// </summary>
        public int SumRound { get; protected set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 物品基础类
        /// </summary>
        /// <param name="context"></param>
        public BaseGoods(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 增加物品数量
        /// </summary>
        /// <param name="num"></param>
        public void AddGoodsNum(int num)
        {
            GoodsNum += num;
        }

        /// <summary>
        /// 检查当前物品指定角色是否可用
        /// </summary>
        /// <param name="playerId">1-4</param>
        /// <returns></returns>
        [Obsolete("这个东西应该由角色检测，而非物品")]
        public bool CanPlayerUse(int playerId)
        {
            if (playerId >= 1 && playerId <= 4)
            {
                return (_enableLevel & (1 << (playerId - 1))) != 0;
            }
            return false;
        }

        /// <summary>
        /// 绘制物品到指定位置
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Draw(ICanvas canvas, int x, int y)
        {
            _image.Draw(canvas, 1, x, y);
        }

        /// <summary>
        /// 物品是否具有全体效果
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEffectAll()
        {
            return false;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        public override void SetData(byte[] buf, int offset)
        {
            Type = (int)buf[offset] & 0xFF;
            Index = (int)buf[offset + 1] & 0xFF;
            _enableLevel = (int)buf[offset + 3] & 0xFF;
            SumRound = (int)buf[offset + 4] & 0xff;
            _image = Context.LibData.GetGoodsImage(Type, (int)buf[offset + 5] & 0xff);
            Name = buf.GetString(offset + 6);
            BuyPrice = buf.Get2BytesUInt(offset + 0x12);
            SellPrice = buf.Get2BytesUInt(offset + 0x14);
            Description = buf.GetString(offset + 0x1e);
            EventId = buf.Get2BytesUInt(offset + 0x84);
            SetOtherData(buf, offset);
        }

        /// <summary>
        /// 设置其它数据？
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        protected abstract void SetOtherData(byte[] buf, int offset);

        #endregion 方法
    }
}