using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 通用菜单
    /// </summary>
    internal class ScreenCommonMenu : BaseScreen
    {
        #region 字段

        private const int Padx = 3;

        private const int Pady = 3;

        /// <summary>
        /// 背景
        /// </summary>
        private readonly ImageBuilder _background = null;

        /// <summary>
        /// 菜单列表
        /// </summary>
        private readonly IReadOnlyList<byte[]> _menuItems;

        /// <summary>
        /// 菜单矩形
        /// </summary>
        private Rectangle _menuItemsRect;

        /// <summary>
        /// 当前选择的项
        /// </summary>
        private int _selectIndex = 0;

        #endregion 字段

        #region 构造函数

        public Action<int> Callback { get; }

        /// <summary>
        /// 通用菜单
        /// </summary>
        /// <param name="context"></param>
        public ScreenCommonMenu(IEnumerable<string> items, Action<int> Callback, SimulatorContext context) : base(context)
        {
            var byteItmes = items.Select(m => m.GetBytes()).ToArray();

            var colCount = byteItmes.Max(m => m.Length);

            var width = 8 * colCount;
            var height = 16 * byteItmes.Length;

            _background = Context.Util.GetFrameBitmap(width + Padx * 2, height + Pady * 2);

            _menuItemsRect = new Rectangle(
                (Constants.SCREEN_WIDTH - width) / 2,
                (Constants.SCREEN_HEIGHT - height) / 2,
                width,
                height);

            _menuItems = byteItmes.Select(m =>
            {
                if (m.Length < colCount)
                {
                    var nm = new byte[colCount];
                    Array.Copy(m, 0, nm, 0, m.Length);
                    return nm;
                }
                else
                {
                    return m;
                }
            }).ToArray();
            this.Callback = Callback;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, _menuItemsRect.Left - Padx, _menuItemsRect.Top - Pady);

            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (i != _selectIndex)
                {
                    TextRender.DrawText(canvas, _menuItems[i], _menuItemsRect.Left, _menuItemsRect.Top + 16 * i);
                }
                else
                {
                    TextRender.DrawSelText(canvas, _menuItems[i], _menuItemsRect.Left, _menuItemsRect.Top + 16 * i);
                }
            }
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP && _selectIndex > 0)
            {
                _selectIndex--;
            }
            else if (key == SimulatorKeys.KEY_DOWN
                && _selectIndex < _menuItems.Count - 1)
            {
                _selectIndex++;
            }
        }

        public override void OnKeyUp(int key)
        {
            switch (key)
            {
                case SimulatorKeys.KEY_ENTER:
                    Context.PopScreen();
                    Callback(_selectIndex + 1);
                    break;

                case SimulatorKeys.KEY_CANCEL:
                    Context.PopScreen();
                    Callback(0);
                    break;
            }
        }

        public void Reset()
        {
            _selectIndex = 0;
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}