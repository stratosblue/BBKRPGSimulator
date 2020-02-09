using System;
using System.Collections.Generic;

using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.View;
using BBKRPGSimulator.View.GameMenu;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 买命令
    /// </summary>
    internal class CommandBuy : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 买命令
        /// </summary>
        /// <param name="context"></param>
        public CommandBuy(ArraySegment<byte> data, SimulatorContext context) : base(data, -1, context)
        {
            Length = data.GetStringLength(0);
        }

        protected override Operate ProcessAndGetOperate() => new CommandBuyOperate(Data, Context);

        #endregion 构造函数

        #region 类

        public class CommandBuyOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 购买界面
            /// </summary>
            private ScreenBuyGoods _buyScreen = null;

            /// <summary>
            /// 物品列表
            /// </summary>
            private List<BaseGoods> _goodsList = new List<BaseGoods>();

            #endregion 字段

            #region 构造函数

            public CommandBuyOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                var start = data.Offset;
                var code = data.Array;

                _goodsList.Clear();
                var i = start;
                while (code[i] != 0)
                {
                    BaseGoods goods = Context.GoodsManage.GetGoods((int)code[i + 1] & 0xff, (int)code[i] & 0xff);

                    if (goods == null)
                    {
                        goods = Context.LibData.GetGoods((int)code[i + 1] & 0xff, (int)code[i] & 0xff);
                        goods.GoodsNum = 0;
                    }

                    _goodsList.Add(goods);
                    i += 2;
                }

                _buyScreen = new ScreenBuyGoods(Context);

                Context.PushScreen(new ScreenGoodsList(Context, _goodsList, (goods) =>
                {
                    if (Context.PlayContext.Money < goods.BuyPrice)
                    {
                        Context.ShowMessage("金钱不足!", 1000);
                    }
                    else
                    {
                        _buyScreen.Init(goods);
                        Context.PushScreen(_buyScreen);
                    }
                }, GoodsOperateMode.Buy));
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                //TODO 此处需要确认是否正常运行
                Context.SceneMap.DrawScene(canvas);
            }

            public override bool Update(long delta)
            {
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}