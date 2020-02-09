using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.View
{
    /// <summary>
    /// 魔法界面
    /// </summary>
    internal class ScreenMagic : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 界面上显示的条目数
        /// </summary>
        private const int ITEM_NUM = 2;

        /// <summary>
        /// 底部矩形
        /// </summary>
        private static readonly Rectangle _bottomRectangle = new Rectangle(10, 41, 137, 35);

        /// <summary>
        /// 描述矩形
        /// </summary>
        private static readonly Rectangle _descriptionRectangle = new Rectangle(11, 42, 135, 33);

        /// <summary>
        /// 画笔
        /// </summary>
        private static readonly Paint _paint = new Paint(PaintStyle.STROKE, Constants.COLOR_BLACK) { StrokeWidth = 1 };

        /// <summary>
        /// 消耗真气 文本位置
        /// </summary>
        private static readonly Point _textPos = new Point(10, 77);

        /// <summary>
        /// 顶部矩形
        /// </summary>
        private static readonly Rectangle _topRectangle = new Rectangle(10, 4, 137, 35);

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 选择光标
        /// </summary>
        private readonly ImageBuilder _cursor;

        /// <summary>
        /// 界面上显示的第一个魔法的序号
        /// </summary>
        private int _firstIndex = 0;

        /// <summary>
        /// 魔法链资源
        /// </summary>
        private ResMagicChain _magicChain;

        /// <summary>
        /// 标记1
        /// </summary>
        private ImageBuilder _marker = null;

        /// <summary>
        /// 标记2
        /// </summary>
        private ImageBuilder _marker2 = null;

        /// <summary>
        /// 下一个要画的魔法描述中的字节
        /// </summary>
        private int _nextToDraw = 0;

        /// <summary>
        /// 选择回调
        /// </summary>
        private Action<BaseMagic> _onItemSelectedCallBack;

        /// <summary>
        /// 当前光标所在位置魔法的序号
        /// </summary>
        private int _selectedIndex = 0;

        /// <summary>
        /// 保存上次魔法描述所画位置
        /// </summary>
        private Stack<int> _stackLastToDraw = new Stack<int>();

        /// <summary>
        /// 当前要画的魔法描述中的字节
        /// </summary>
        private int _toDraw = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 魔法界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="magicChain">魔法链</param>
        /// <param name="selectedCallBack">选择回调</param>
        public ScreenMagic(SimulatorContext context, ResMagicChain magicChain, Action<BaseMagic> selectedCallBack) : base(context)
        {
            if (magicChain == null || selectedCallBack == null)
            {
                throw new Exception("ScreenMagic construtor params can't be null.");
            }
            _magicChain = magicChain;
            _onItemSelectedCallBack = selectedCallBack;

            _cursor = Context.GraphicsFactory.NewImageBuilder(12, 11);
            _marker = Context.GraphicsFactory.NewImageBuilder(5, 8);
            _marker2 = Context.GraphicsFactory.NewImageBuilder(5, 8);

            CreateImage();
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawColor(Constants.COLOR_WHITE);
            canvas.DrawRect(_topRectangle, _paint);
            canvas.DrawRect(_bottomRectangle, _paint);
            TextRender.DrawText(canvas, _magicChain[_firstIndex].Name, _topRectangle.Left + 1, _topRectangle.Top + 1);
            if (_firstIndex + 1 < _magicChain.LearnCount)
            {
                TextRender.DrawText(canvas, _magicChain[_firstIndex + 1].Name, _topRectangle.Left + 1, _topRectangle.Top + 1 + 16);
            }
            _nextToDraw = TextRender.DrawText(canvas, _magicChain[_selectedIndex].MagicDescription, _toDraw, _descriptionRectangle);
            TextRender.DrawText(canvas, "耗真气:" + _magicChain[_selectedIndex].CostMp, _textPos.X, _textPos.Y);
            canvas.DrawBitmap(_cursor, 100, _firstIndex == _selectedIndex ? 10 : 26);
            canvas.DrawBitmap(_firstIndex == 0 ? _marker : _marker2, 135, 6);
            canvas.DrawBitmap(_marker, 135, 6 + 8);
            canvas.DrawBitmap(_marker, 135, 6 + 16);
            canvas.DrawBitmap(_firstIndex + 2 < _magicChain.LearnCount ? _marker2 : _marker, 135, 6 + 24);
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP && _selectedIndex > 0)
            {
                --_selectedIndex;
                if (_selectedIndex < _firstIndex)
                {
                    --_firstIndex;
                }
                _toDraw = _nextToDraw = 0;
                _stackLastToDraw.Clear();
            }
            else if (key == SimulatorKeys.KEY_DOWN && _selectedIndex + 1 < _magicChain.LearnCount)
            {
                ++_selectedIndex;
                if (_selectedIndex >= _firstIndex + ITEM_NUM)
                {
                    ++_firstIndex;
                }
                _toDraw = _nextToDraw = 0;
                _stackLastToDraw.Clear();
            }
            else if (key == SimulatorKeys.KEY_PAGEDOWN)
            {
                try
                {
                    int len = _magicChain[_selectedIndex].MagicDescription.GetBytes().Length;
                    if (_nextToDraw < len)
                    {
                        _stackLastToDraw.Push(_toDraw); // 保存旧位置
                        _toDraw = _nextToDraw; // 更新位置
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else if (key == SimulatorKeys.KEY_PAGEUP && _toDraw != 0)
            {
                if (_stackLastToDraw.Count > 0)
                {
                    _toDraw = _stackLastToDraw.Pop();
                }
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                // 回调接口
                _onItemSelectedCallBack(_magicChain[_selectedIndex]);
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.PopScreen();
            }
        }

        public override void Update(long delta)
        {
        }

        /// <summary>
        /// 创建相关图片
        /// </summary>
        private void CreateImage()
        {
            var canvas = Context.GraphicsFactory.NewCanvas();
            Paint paint = new Paint(PaintStyle.STROKE, Constants.COLOR_BLACK) { StrokeWidth = 1 };

            canvas.SetBitmap(_cursor);
            canvas.DrawColor(Constants.COLOR_WHITE);
            canvas.DrawLine(8, 0, 11, 0, paint);
            canvas.DrawLine(11, 1, 11, 4, paint);
            canvas.DrawRect(6, 1, 7, 4, paint);
            canvas.DrawRect(7, 4, 10, 5, paint);
            canvas.DrawLine(7, 4, 0, 11, paint);
            canvas.DrawLine(8, 5, 2, 11, paint);

            canvas.SetBitmap(_marker);
            canvas.DrawColor(Constants.COLOR_WHITE);
            float[] points = { 2, 0, 4, 2, 4, 2, 4, 6, 4, 6, 2, 8, 2, 7, 0, 5, 0, 5, 0, 2, 0, 3, 3, 0, 2, 3, 2, 5 };
            canvas.DrawLines(points, paint);

            canvas.SetBitmap(_marker2);
            canvas.DrawColor(Constants.COLOR_WHITE);
            canvas.DrawLines(points, paint);
            float[] points2 = { 1, 1, 1, 6, 2, 0, 2, 8, 3, 2, 3, 6 };
            canvas.DrawLines(points2, paint);
        }

        #endregion 方法
    }
}