using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 系统菜单界面
    /// </summary>
    internal class ScreenMenuSystem : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 菜单项
        /// </summary>
        private static readonly string[] _operateItems = { "读入进度", "存储进度", "游戏设置", "结束游戏" };

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 箭头图片的显示索引
        /// </summary>
        private int _arrowImgIndex = 0;

        /// <summary>
        /// 箭头图片
        /// </summary>
        private ImageBuilder[] _arrowImgs;

        /// <summary>
        /// 箭头显示的XY坐标
        /// </summary>
        private int _arrowX = 70, _arrowY = 82;

        /// <summary>
        /// 背景
        /// </summary>
        private ImageBuilder _background = null;

        /// <summary>
        /// 起始项索引
        /// </summary>
        private int _firstIndex = 0;

        /// <summary>
        /// 当前选择的操作索引
        /// </summary>
        private int _selectedIndex = 0;

        /// <summary>
        /// 当前选择项显示的Y坐标
        /// </summary>
        private int _selectedY = 32;

        /// <summary>
        /// 字符显示的XY坐标
        /// </summary>
        private int _strX = 42, _strY = 32;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 系统菜单界面
        /// </summary>
        /// <param name="context"></param>
        public ScreenMenuSystem(SimulatorContext context) : base(context)
        {
            _background = Context.Util.GetFrameBitmap(109 - 39 + 1, 91 - 29 + 1);
            var canvas = context.GraphicsFactory.NewCanvas();
            Paint paint = new Paint(Constants.COLOR_BLACK);

            ImageBuilder arrowUpImg = Context.GraphicsFactory.NewImageBuilder(7, 4);

            canvas.SetBitmap(arrowUpImg);
            canvas.DrawColor(Constants.COLOR_WHITE);
            canvas.DrawLine(3, 0, 4, 0, paint);
            canvas.DrawLine(2, 1, 5, 1, paint);
            canvas.DrawLine(1, 2, 6, 2, paint);
            canvas.DrawLine(0, 3, 7, 3, paint);

            ImageBuilder arrowDownImg = Context.GraphicsFactory.NewImageBuilder(7, 4);
            canvas.SetBitmap(arrowDownImg);
            canvas.DrawColor(Constants.COLOR_WHITE);
            canvas.DrawLine(0, 0, 7, 0, paint);
            canvas.DrawLine(1, 1, 6, 1, paint);
            canvas.DrawLine(2, 2, 5, 2, paint);
            canvas.DrawLine(3, 3, 4, 3, paint);

            _arrowImgs = new ImageBuilder[] { arrowDownImg, arrowUpImg };
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 39, 29);
            TextRender.DrawText(canvas, _operateItems[_firstIndex], _strX, _strY);
            TextRender.DrawText(canvas, _operateItems[_firstIndex + 1], _strX, _strY + 16);
            TextRender.DrawText(canvas, _operateItems[_firstIndex + 2], _strX, _strY + 32);
            TextRender.DrawSelText(canvas, _operateItems[_selectedIndex], _strX, _selectedY);
            canvas.DrawBitmap(_arrowImgs[_arrowImgIndex], _arrowX, _arrowY);
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP)
            {
                --_selectedIndex;
                _selectedY -= 16;
            }
            else if (key == SimulatorKeys.KEY_DOWN)
            {
                ++_selectedIndex;
                _selectedY += 16;
            }

            if (_selectedIndex == 0 || _selectedIndex == 4)
            {
                _selectedIndex = 0;
                _selectedY = 32;
                _arrowY = 82;
                _arrowImgIndex = 0;
                _firstIndex = 0;
                _strY = 32;
            }
            else if (_selectedIndex == 3 || _selectedIndex == -1)
            {
                _selectedIndex = 3;
                _selectedY = 72;
                _arrowY = 34;
                _arrowImgIndex = 1;
                _firstIndex = 1;
                _strY = 40;
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
                switch (_selectedIndex)
                {
                    case 0:
                        Context.PushScreen(new ScreenSaveLoadGame(Context, SaveLoadOperate.LOAD));
                        break;

                    case 1:
                        if (Context.PlayContext.DisableSave)
                        {
                            Context.ShowMessage("当前不能存档", 1000);
                        }
                        //TODO 完成这里
                        //else if (game.mainScene.scriptProcess.prev != null)
                        //        showMessage("副本中不能存档")
                        else
                        {
                            Context.PushScreen(new ScreenSaveLoadGame(Context, SaveLoadOperate.SAVE));
                        }
                        break;

                    case 2:
                        break;

                    case 3:
                        Context.ChangeScreen(ScreenEnum.SCREEN_MENU);
                        break;
                }
            }
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}