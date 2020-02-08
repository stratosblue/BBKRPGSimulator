using System;

using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 获取物品命令
    /// </summary>
    internal class CommandGainGoods : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 获取物品命令
        /// </summary>
        /// <param name="context"></param>
        public CommandGainGoods(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandGainGoodsOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandGainGoodsOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 要获取的物品
            /// </summary>
            private readonly BaseGoods _goods;

            /// <summary>
            /// 消息
            /// </summary>
            private readonly string _message;

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyPressed;

            /// <summary>
            /// 展示时间
            /// </summary>
            private long _showTime;

            public CommandGainGoodsOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                var start = data.Offset;
                var code = data.Array;

                _goods = Context.LibData.GetGoods(code.Get2BytesUInt(start), code.Get2BytesUInt(start + 2));
                _message = $"获得:{_goods.Name}";

                _goods.GoodsNum = 1;
                Context.GoodsManage.AddGoods(_goods.Type, _goods.Index);
                _showTime = 0;
                _isAnyKeyPressed = false;
            }

            #endregion 字段

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.Util.ShowMessage(canvas, _message);
            }

            public override void OnKeyUp(int key)
            {
                _isAnyKeyPressed = true;
            }

            public override bool Update(long delta)
            {
                _showTime += delta;
                if (_showTime > 1000 || _isAnyKeyPressed)
                {
                    return false;
                }

                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}