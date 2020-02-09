using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 角色选择界面
    /// </summary>
    internal class ScreenSelectCharacter : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 角色总数
        /// </summary>
        private int _characterCount;

        /// <summary>
        /// 框的图片？
        /// </summary>
        private ImageBuilder _frameImg;

        /// <summary>
        /// 框的矩形？
        /// </summary>
        private Rectangle _frameRect;

        /// <summary>
        /// 获取下一步要进行的操作的委托
        /// </summary>
        private Func<int, BaseScreen> _getNextScreenDelegate;

        /// <summary>
        /// 角色名字数组
        /// </summary>
        private string[] _names;

        /// <summary>
        /// 选择的索引
        /// </summary>
        private int _selectedIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 角色选择界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="getNextScreenDelegate">获取下一步要进行的操作的委托</param>
        public ScreenSelectCharacter(SimulatorContext context, Func<int, BaseScreen> getNextScreenDelegate) : base(context)
        {
            _getNextScreenDelegate = getNextScreenDelegate;
            _frameRect = new Rectangle(39, 29, 86, 6 + Context.PlayContext.PlayerCharacters.Count * 16); //new Rectangle(39, 29, 125, 67 - 32 + Context.PlayInfo.PlayerCharacters.Count * 16);
            _frameImg = Context.Util.GetFrameBitmap(_frameRect.Width, _frameRect.Height);

            List<PlayerCharacter> list = Context.PlayContext.PlayerCharacters;
            _characterCount = list.Count;
            _names = new string[_characterCount];
            for (int i = 0; i < _characterCount; i++)
            {
                _names[i] = Format(list[i].Name);
            }
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(this._frameImg, this._frameRect.Left, this._frameRect.Top);
            for (int i = 0; i < _characterCount; i++)
            {
                if (i == _selectedIndex)
                {
                    TextRender.DrawSelText(canvas, _names[i], this._frameRect.Left + 3, this._frameRect.Top + 3 + 16 * i);
                }
                else
                {
                    TextRender.DrawText(canvas, _names[i], this._frameRect.Left + 3, this._frameRect.Top + 3 + 16 * i);
                }
            }
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_DOWN)
            {
                ++_selectedIndex;
                if (_selectedIndex >= _characterCount)
                {
                    _selectedIndex = 0;
                }
            }
            else if (key == SimulatorKeys.KEY_UP)
            {
                --_selectedIndex;
                if (_selectedIndex < 0)
                {
                    _selectedIndex = _characterCount - 1;
                }
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
                if (_getNextScreenDelegate?.Invoke(_selectedIndex) is BaseScreen nextScreen)
                {
                    Context.PushScreen(nextScreen);
                }
                //TODO 此处需要确认是否还正常运行
            }
        }

        public override void Update(long delta)
        { }

        /// <summary>
        /// 补够字符串长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string Format(string str)
        {
            try
            {
                while (str.GetBytes().Length < 10)
                {
                    str += " ";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return str;
        }

        #endregion 方法
    }
}