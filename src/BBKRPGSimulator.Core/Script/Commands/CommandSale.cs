using BBKRPGSimulator.Goods;
using BBKRPGSimulator.View;
using BBKRPGSimulator.View.GameMenu;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 卖东西命令
    /// </summary>
    internal class CommandSale : BaseCommand
    {
        #region 字段

        /// <summary>
        /// 物品列表界面
        /// </summary>
        private readonly ScreenGoodsList _goodsListScreen = null;

        /// <summary>
        /// 卖出界面
        /// </summary>
        private readonly ScreenSaleGoods _saleScreen = null;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 卖东西命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSale(SimulatorContext context) : base(0, context)
        {
            _goodsListScreen = new ScreenGoodsList(Context, Context.GoodsManage.GetAllGoods(), (goods) =>
            {
                if (goods is GoodsDrama)
                {
                    Context.ShowMessage("任务物品!", 1000);
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

        protected override Operate ProcessAndGetOperate()
        {
            Context.PushScreen(_goodsListScreen);
            return null;
        }

        #endregion 方法
    }
}