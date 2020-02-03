using System.Collections.Generic;

using BBKRPGSimulator.GameMenu;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.View;

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
        public CommandBuy(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            int i = 0;
            while (code[start + i] != 0) ++i;
            return start + i + 1;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandBuyOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 购买操作
        /// </summary>
        private class CommandBuyOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 购买界面
            /// </summary>
            private ScreenBuyGoods _buyScreen = null;

            /// <summary>
            /// 脚本data
            /// </summary>
            private byte[] _data;

            /// <summary>
            /// 物品列表
            /// </summary>
            private List<BaseGoods> _goodsList = new List<BaseGoods>();

            /// <summary>
            /// 脚本其实位置
            /// </summary>
            private int _start;

            #endregion 字段

            #region 构造函数

            public CommandBuyOperate(SimulatorContext context, byte[] data, int start) : base(context)
            {
                _data = data;
                _start = start;
                _buyScreen = new ScreenBuyGoods(Context);
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                //TODO 此处需要确认是否正常运行
                Context.SceneMap.DrawScene(canvas);
            }

            public override void OnKeyDown(int key)
            {
            }

            public override void OnKeyUp(int key)
            {
            }

            public override bool Process()
            {
                _goodsList.Clear();
                int i = _start;
                while (_data[i] != 0)
                {
                    BaseGoods goods = Context.GoodsManage.GetGoods((int)_data[i + 1] & 0xff, (int)_data[i] & 0xff);

                    if (goods == null)
                    {
                        goods = Context.LibData.GetGoods((int)_data[i + 1] & 0xff, (int)_data[i] & 0xff);
                        goods.GoodsNum = 0;
                    }

                    _goodsList.Add(goods);
                    i += 2;
                }
                Context.PushScreen(new ScreenGoodsList(Context, _goodsList, (goods) =>
                {
                    if (Context.PlayContext.Money < goods.BuyPrice)
                    {
                        Context.Util.ShowMessage("金钱不足!", 1000);
                    }
                    else
                    {
                        _buyScreen.Init(goods);
                        Context.PushScreen(_buyScreen);
                    }
                }, GoodsOperateMode.Buy));
                return true;
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