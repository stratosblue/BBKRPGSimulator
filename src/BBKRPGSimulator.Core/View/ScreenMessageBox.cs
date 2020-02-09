using System;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View
{
    /// <summary>
    /// 消息框界面
    /// </summary>
    internal class ScreenMessageBox : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 背景
        /// </summary>
        private ImageBuilder _background;

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 确认的回调
        /// </summary>
        private readonly Action _callback;

        /// <summary>
        /// 消息内容
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// 当前选择的索引
        /// </summary>
        private int _selectedIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 消息框界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message">消息</param>
        /// <param name="callback">确认的回调</param>
        public ScreenMessageBox(SimulatorContext context, string message, Action callback) : base(context)
        {
            //TODO 确认修改后是否正常运行
            _callback = callback;

            _message = message ?? string.Empty;

            InitBackground(context);
        }

        private void InitBackground(SimulatorContext context)
        {
            _background = context.GraphicsFactory.NewImageBuilder(137 - 27 + 1, 81 - 15 + 1);

            ICanvas canvas = Context.GraphicsFactory.NewCanvas(_background); ;
            canvas.DrawColor(Constants.COLOR_WHITE);

            Paint paint = new Paint(PaintStyle.STROKE, Constants.COLOR_BLACK);

            canvas.DrawRect(1, 1, _background.Width - 5, _background.Height - 5, paint);
            canvas.DrawRect(43 - 27, 51 - 15, 70 - 27, 70 - 15, paint);
            canvas.DrawRect(91 - 27, 51 - 15, 118 - 27, 70 - 15, paint);
            paint.SetStyle(PaintStyle.FILL_AND_STROKE);
            canvas.DrawRect(32 - 27, 77 - 15, 137 - 27, 81 - 15, paint);
            canvas.DrawRect(133 - 27, 20 - 15, _background.Width - 1, _background.Height - 1, paint);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 27, 15);
            TextRender.DrawText(canvas, _message, 33, 23);
            if (_selectedIndex == 0)
            {
                TextRender.DrawSelText(canvas, "是 ", 45, 53);
                TextRender.DrawText(canvas, "否 ", 93, 53);
            }
            else if (_selectedIndex == 1)
            {
                TextRender.DrawText(canvas, "是 ", 45, 53);
                TextRender.DrawSelText(canvas, "否 ", 93, 53);
            }
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_LEFT || key == SimulatorKeys.KEY_RIGHT)
            {
                _selectedIndex = 1 - _selectedIndex;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                if (_selectedIndex == 0)
                {
                    _callback?.Invoke();
                }
                Exit();
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                Exit();
            }
        }

        public override void Update(long delta)
        {
        }

        private void Exit()
        {
            Context.PopScreen();
        }

        #endregion 方法
    }
}