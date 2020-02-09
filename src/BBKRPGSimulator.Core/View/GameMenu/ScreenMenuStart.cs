using System.Collections.Generic;

using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.View
{
    /// <summary>
    /// 游戏开始菜单
    /// </summary>
    internal class ScreenMenuStart : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 光标
        /// </summary>
        private readonly IReadOnlyList<ResSrs> _cursors;

        /// <summary>
        /// 菜单在屏幕的左和上坐标
        /// </summary>
        private readonly int _left, _top;

        /// <summary>
        /// 菜单图片
        /// </summary>
        private readonly ResImage _menuImg;

        /// <summary>
        /// 当前选择索引
        /// </summary>
        private int _selectedIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 游戏开始菜单
        /// </summary>
        /// <param name="context"></param>
        public ScreenMenuStart(SimulatorContext context) : base(context)
        {
            _menuImg = Context.LibData.GetImage(2, 14);

            var cursors = new List<ResSrs>();
            for (int i = 250; i <= 255; i++)
            {
                if (Context.LibData.GetSrs(1, i) is ResSrs resSrs)
                {
                    resSrs.StartAni();
                    cursors.Add(resSrs);
                }
                else
                {
                    break;
                }
            }
            _cursors = cursors;

            _left = (160 - _menuImg.Width) / 2;
            _top = (96 - _menuImg.Height) / 2;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawColor(Constants.COLOR_WHITE);
            _menuImg.Draw(canvas, 1, _left, _top);
            _cursors[_selectedIndex].Draw(canvas, 0, 0);
        }

        public override void OnKeyDown(int key)
        {
            switch (key)
            {
                case SimulatorKeys.KEY_UP:
                    if (_selectedIndex > 0)
                    {
                        _selectedIndex--;
                    }
                    break;

                case SimulatorKeys.KEY_DOWN:
                    if (_selectedIndex < _cursors.Count - 1)
                    {
                        _selectedIndex++;
                    }
                    break;

                case SimulatorKeys.KEY_CANCEL:
                    break;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                if (_selectedIndex == 0)
                {
                    // 新游戏
                    Context.ChangeScreen(ScreenEnum.SCREEN_MAIN_GAME, true);
                }
                else if (_selectedIndex == 1)
                {
                    // 读取进度
                    Context.PushScreen(new ScreenSaveLoadGame(Context, SaveLoadOperate.LOAD));
                }
                else
                {
                    //TODO 这里如何实现？
                    Context.ShowMessage("模拟器还未兼容该功能！！！", 1500);
                }
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.Simulator.InvokeExitRequest();
            }
        }

        public override void Update(long delta)
        {
            if (!_cursors[_selectedIndex].Update(delta))
            {
                _cursors[_selectedIndex].StartAni();
            }
        }

        #endregion 方法
    }
}