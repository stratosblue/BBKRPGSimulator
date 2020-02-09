using System;
using System.Collections.Generic;
using System.Drawing;

using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 物品列表界面
    /// </summary>
    internal class ScreenGoodsList : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 界面上显示的条目数
        /// </summary>
        private const int ITEM_NUM = 4;

        /// <summary>
        /// 显示物品的矩形位置
        /// </summary>
        private static readonly Rectangle _goodsDisplayRectangle = new Rectangle(44, 61, 112, 33);

        /// <summary>
        /// 背景图片
        /// </summary>
        private ImageBuilder _background;

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 当前光标所在位置物品的序号
        /// </summary>
        private int _curItemIndex = 0;

        /// <summary>
        /// 描述文字
        /// </summary>
        private byte[] _descText;

        /// <summary>
        /// 界面上显示的第一个物品的序号
        /// </summary>
        private int _firstItemIndex = 0;

        /// <summary>
        /// 物品列表
        /// </summary>
        private List<BaseGoods> _goodsList;

        /// <summary>
        /// 最后按下的键
        /// </summary>
        private int _lastDownKey = -1;

        /// <summary>
        /// 操作模式
        /// </summary>
        private GoodsOperateMode _mode;

        /// <summary>
        /// 下一个要画的描述中的字节
        /// </summary>
        private int _nextToDraw = 0;

        /// <summary>
        /// 选择物品的操作委托
        /// </summary>
        private Action<BaseGoods> _onItemSelected;

        /// <summary>
        /// 保存上次描述所画位置
        /// </summary>
        private Stack<int> _stackLastToDraw = new Stack<int>();

        /// <summary>
        /// 当前要画的描述中的字节
        /// </summary>
        private int _toDraw = 0;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 物品列表
        /// </summary>
        private List<BaseGoods> GoodsList
        {
            get => _goodsList;
            set
            {
                _goodsList = value;
                if (_goodsList.Count > 0)
                {
                    _descText = _goodsList[0].Description.GetBytes();
                }
            }
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 物品列表界面
        /// </summary>
        /// <param name="list">物品列表</param>
        /// <param name="selectAction">选择物品回调</param>
        /// <param name="mode">物品操作模式</param>
        public ScreenGoodsList(SimulatorContext context, List<BaseGoods> list, Action<BaseGoods> selectAction, GoodsOperateMode mode) : base(context)
        {
            if (list == null || selectAction == null)
            {
                throw new Exception("ScreenGoodsList construtor params can't be null.");
            }
            InitBackground(context);
            GoodsList = list;
            _onItemSelected = selectAction;
            _mode = mode;
        }

        private void InitBackground(SimulatorContext context)
        {
            _background = context.GraphicsFactory.NewImageBuilder(160, 96);

            float[] points = {40,21, 40,95, 40,95, 0,95, 0,95, 0,5, 0,5, 5,0, 5,0, 39,0,
                    39,0, 58,19, 38,0, 57,19, 57,19, 140,19, 41,20, 140,20, 41,21, 159,21,
                    54,0, 140,0, 40,95, 159,95, 40,57, 160,57, 40,58, 140,58, 40,59, 159,59,
                    41,20, 41,95, 42,20, 42,95, 159,21, 159,57, 159,59, 159,96};
            var canvas = context.GraphicsFactory.NewCanvas(_background);
            canvas.DrawColor(Constants.COLOR_WHITE);
            canvas.DrawLines(points, new Paint(PaintStyle.STROKE, Constants.COLOR_BLACK));
            TextRender.DrawText(canvas, "名:", 45, 23);
            TextRender.DrawText(canvas, "价:", 45, 40);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 0, 0);
            if (GoodsList.Count <= 0)
            {
                return;
            }

            while (_curItemIndex >= GoodsList.Count)
            {
                ShowPreItem();
            }

            BaseGoods goods = GoodsList[_curItemIndex];
            TextRender.DrawText(canvas, _mode == GoodsOperateMode.Buy ? "金钱:" + Context.PlayContext.Money : "数量:" + goods.GoodsNum, 60, 2);
            TextRender.DrawText(canvas, goods.Name, 69, 23);
            TextRender.DrawText(canvas, "" + (_mode == GoodsOperateMode.Buy ? goods.BuyPrice : goods.SellPrice), 69, 40);
            Context.Util.DrawTriangleCursor(canvas, 4, 8 + 23 * (_curItemIndex - _firstItemIndex));

            for (int i = _firstItemIndex; i < _firstItemIndex + ITEM_NUM && i < GoodsList.Count; i++)
            {
                GoodsList[i].Draw(canvas, 14, 2 + 23 * (i - _firstItemIndex));
            }

            _nextToDraw = TextRender.DrawText(canvas, _descText, _toDraw, _goodsDisplayRectangle);
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP && _curItemIndex > 0)
            {
                ShowPreItem();
            }
            else if (key == SimulatorKeys.KEY_DOWN && _curItemIndex + 1 < GoodsList.Count)
            {
                ShowNextItem();
            }
            else if (key == SimulatorKeys.KEY_PAGEDOWN)
            {
                int len = _descText.Length;
                if (_nextToDraw < len)
                {
                    _stackLastToDraw.Push(_toDraw);
                    _toDraw = _nextToDraw;
                }
            }
            else if (key == SimulatorKeys.KEY_PAGEUP && _toDraw != 0)
            {
                if (_stackLastToDraw.Count > 0)
                {
                    _toDraw = _stackLastToDraw.Pop();
                }
            }
            _lastDownKey = key;
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER && _lastDownKey == SimulatorKeys.KEY_ENTER)
            {
                _onItemSelected(GoodsList[_curItemIndex]);
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.PopScreen();
            }
        }

        /// <summary>
        /// 重新设置物品列表
        /// </summary>
        /// <param name="list"></param>
        public void ReSetGoodsList(List<BaseGoods> list)
        {
            GoodsList = list;
        }

        public override void Update(long delta)
        {
            if (GoodsList.Count <= 0)
            {
                Context.PopScreen();
            }
        }

        private void ShowNextItem()
        {
            ++_curItemIndex;
            _descText = GoodsList[_curItemIndex].Description.GetBytes();
            if (_curItemIndex >= _firstItemIndex + ITEM_NUM)
            {
                ++_firstItemIndex;
            }
            _toDraw = _nextToDraw = 0;
            _stackLastToDraw.Clear();
        }

        private void ShowPreItem()
        {
            --_curItemIndex;
            _descText = GoodsList[_curItemIndex].Description.GetBytes();
            if (_curItemIndex < _firstItemIndex)
            {
                --_firstItemIndex;
            }
            _toDraw = _nextToDraw = 0;
            _stackLastToDraw.Clear();
        }

        #endregion 方法
    }
}