using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View
{
    /// <summary>
    /// 物品购买界面
    /// </summary>
    internal class ScreenBuyGoods : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 背景
        /// </summary>
        private ImageBuilder _background;

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 当前购买总数
        /// </summary>
        private int _buyCount;

        /// <summary>
        /// 当前物品
        /// </summary>
        private BaseGoods _goods = null;

        /// <summary>
        /// 玩家金钱
        /// </summary>
        private int _money;

        /// <summary>
        /// 当前货物总价
        /// </summary>
        private int _totalPrice;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 当前购买总数
        /// </summary>
        public int BuyCount
        {
            get => _buyCount;
            set
            {
                if (value > 99)
                {
                    _buyCount = 99;
                }
                else if (value < 0)
                {
                    _buyCount = 0;
                }
                else
                {
                    _buyCount = value;
                }

                _totalPrice = _buyCount * _goods.BuyPrice;
            }
        }

        /// <summary>
        /// 玩家剩余金钱
        /// </summary>
        public int Money { get => _money - _totalPrice; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 物品购买界面
        /// </summary>
        /// <param name="context"></param>
        public ScreenBuyGoods(SimulatorContext context) : base(context)
        {
            InitBackground(context);
        }

        private void InitBackground(SimulatorContext context)
        {
            _background = context.GraphicsFactory.NewImageBuilder(136, 55);

            ICanvas canvas = Context.GraphicsFactory.NewCanvas(_background); ;
            canvas.DrawColor(Constants.COLOR_WHITE);
            canvas.DrawRect(1, 1, 136 - 2, 55 - 2, new Paint(PaintStyle.STROKE, Constants.COLOR_BLACK));
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 12, 21);
            TextRender.DrawText(canvas, "金钱：" + Money, 15, 24);
            TextRender.DrawText(canvas, _goods.Name, 15, 40);
            TextRender.DrawText(canvas, ": " + _goods.GoodsNum, 93, 40);
            TextRender.DrawText(canvas, "买入个数　：" + BuyCount, 15, 56);
        }

        /// <summary>
        /// 初始化物品的购买界面
        /// </summary>
        /// <param name="goods"></param>
        public void Init(BaseGoods goods)
        {
            _goods = goods;
            BuyCount = 0;
            _money = Context.PlayContext.Money;
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP && _goods.GoodsNum < 99)
            {
                if (Money >= _goods.BuyPrice)
                {
                    BuyCount++;
                    _goods.AddGoodsNum(1);
                }
                else
                {
                    Context.ShowMessage("金钱不足!", 1000);
                }
            }
            else if (key == SimulatorKeys.KEY_DOWN && BuyCount > 0)
            {
                BuyCount--;
                _goods.AddGoodsNum(-1);
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)    //确认购买
            {
                Context.PlayContext.Money -= _totalPrice;
                if (BuyCount == _goods.GoodsNum && BuyCount > 0)
                {
                    Context.GoodsManage.AddGoods(_goods.Type, _goods.Index, BuyCount);
                }
                Context.PopScreen();
            }
            else if (key == SimulatorKeys.KEY_CANCEL)  //取消购买
            {
                _goods.AddGoodsNum(-BuyCount);
                Context.PopScreen();
            }
        }

        public override void Update(long delta)
        { }

        #endregion 方法
    }
}