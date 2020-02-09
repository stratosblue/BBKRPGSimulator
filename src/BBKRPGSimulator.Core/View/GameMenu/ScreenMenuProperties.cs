using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 属性界面
    /// </summary>
    internal class ScreenMenuProperties : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 操作项
        /// </summary>
        private static readonly string[] _operateItems = { "状态", "穿戴" };

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 背景图片
        /// </summary>
        private ImageBuilder _background = null;

        /// <summary>
        /// 当前选择的操作索引
        /// </summary>
        private int _selectedIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 属性界面
        /// </summary>
        /// <param name="context"></param>
        public ScreenMenuProperties(SimulatorContext context) : base(context)
        {
            _background = Context.Util.GetFrameBitmap(77 - 39 + 1, 54 - 16 + 1);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 39, 16);
            if (_selectedIndex == 0)
            {
                TextRender.DrawSelText(canvas, _operateItems[0], 39 + 3, 16 + 3);
                TextRender.DrawText(canvas, _operateItems[1], 39 + 3, 16 + 3 + 16);
            }
            else if (_selectedIndex == 1)
            {
                TextRender.DrawText(canvas, _operateItems[0], 39 + 3, 16 + 3);
                TextRender.DrawSelText(canvas, _operateItems[1], 39 + 3, 16 + 3 + 16);
            }
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP || key == SimulatorKeys.KEY_DOWN)
            {
                _selectedIndex = 1 - _selectedIndex;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.PopScreen();
            }
            else if (key == SimulatorKeys.KEY_ENTER)
            {
                Context.PopScreen();
                if (_selectedIndex == 0)
                {
                    Context.PushScreen(new ScreenCharacterState(Context));
                }
                else
                {
                    Context.PushScreen(new ScreenCharacterWearing(Context));
                }
            }
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}