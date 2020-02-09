using System;

using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View
{
    /// <summary>
    /// 出售界面
    /// </summary>
    internal class ScreenSaleGoods : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 背景
        /// </summary>
        private ImageBuilder _background;

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 刷新物品列表回调委托
        /// </summary>
        private readonly Action _refreshCallBack;

        /// <summary>
        /// 当前选择物品
        /// </summary>
        private BaseGoods _goods;

        /// <summary>
        /// 玩家出售后的金钱
        /// </summary>
        private int _money;

        /// <summary>
        /// 出售总量
        /// </summary>
        private int _saleCount;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 出售界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="refreshCallBack">刷新物品列表回调委托</param>
        public ScreenSaleGoods(SimulatorContext context, Action refreshCallBack) : base(context)
        {
            InitBackground(context);
            _refreshCallBack = refreshCallBack;
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
            TextRender.DrawText(canvas, "金钱：" + _money, 15, 24);
            TextRender.DrawText(canvas, _goods.Name, 15, 40);
            TextRender.DrawText(canvas, ": " + (_goods.GoodsNum - _saleCount), 93, 40);
            TextRender.DrawText(canvas, "卖出个数　：" + _saleCount, 15, 56);
        }

        /// <summary>
        /// 初始化出售的物品
        /// </summary>
        /// <param name="goods"></param>
        public void Init(BaseGoods goods)
        {
            _goods = goods;
            _saleCount = 0;
            _money = Context.PlayContext.Money;
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP && _saleCount > 0)
            {
                --_saleCount;
                _money -= _goods.SellPrice;
            }
            else if (key == SimulatorKeys.KEY_DOWN && _goods.GoodsNum > _saleCount)
            {
                ++_saleCount;
                _money += _goods.SellPrice;
                if (_money > 99999)
                {
                    _money = 99999;
                }
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                Context.PlayContext.Money = _money;
                if (_saleCount > 0)
                {
                    Context.GoodsManage.DropGoods(_goods.Type, _goods.Index, _saleCount);
                }
                Context.PopScreen();
                _refreshCallBack?.Invoke();
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.PopScreen();
            }
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}