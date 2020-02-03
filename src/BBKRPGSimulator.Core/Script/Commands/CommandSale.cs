using BBKRPGSimulator.GameMenu;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.View;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 卖东西命令
    /// </summary>
    internal class CommandSale : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 卖东西命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSale(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandSaleOperate(Context);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 卖出操作
        /// </summary>
        private class CommandSaleOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 物品列表界面
            /// </summary>
            private ScreenGoodsList _goodsListScreen = null;

            /// <summary>
            /// 卖出界面
            /// </summary>
            private ScreenSaleGoods _saleScreen = null;

            #endregion 字段

            #region 构造函数

            public CommandSaleOperate(SimulatorContext context) : base(context)
            {
                _goodsListScreen = new ScreenGoodsList(Context, Context.GoodsManage.GetAllGoods(), (goods) =>
                {
                    if (goods is GoodsDrama)
                    {
                        Context.Util.ShowMessage("任务物品!", 1000);
                    }
                    else
                    {
                        _saleScreen.Init(goods);
                        Context.PushScreen(_saleScreen);
                    }
                }, GoodsOperateMode.Sale);

                _saleScreen = new ScreenSaleGoods(Context, () =>
                {
                    _goodsListScreen.ReSetGoodsList(Context.GoodsManage.GetAllGoods());
                });
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            { }

            public override void OnKeyDown(int key)
            { }

            public override void OnKeyUp(int key)
            { }

            public override bool Process()
            {
                Context.PushScreen(_goodsListScreen);
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