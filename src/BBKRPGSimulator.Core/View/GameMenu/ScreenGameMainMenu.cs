using System.Drawing;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 主菜单界面
    /// </summary>
    internal class ScreenGameMainMenu : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 菜单项名称列表
        /// </summary>
        private static readonly string[] _operateItems = { "属性", "魔法", "物品", "系统" };

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 菜单矩形
        /// </summary>
        private Rectangle _menuItemsRect;

        /// <summary>
        /// 菜单的文本数据
        /// </summary>
        private byte[] _menuItemsText;

        /// <summary>
        /// 左侧框
        /// </summary>
        private ImageBuilder _rectangleLeftImg = null;

        /// <summary>
        /// 顶部框
        /// </summary>
        private ImageBuilder _rectangleTopImg = null;

        /// <summary>
        /// 角色选择界面
        /// </summary>
        private BaseScreen _screenSelectCharacter;

        /// <summary>
        /// 当前选择的项
        /// </summary>
        private int _selectIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 主菜单界面
        /// </summary>
        /// <param name="context"></param>
        public ScreenGameMainMenu(SimulatorContext context) : base(context)
        {
            _rectangleTopImg = Context.Util.GetFrameBitmap(93, 16 + 6);
            _rectangleLeftImg = Context.Util.GetFrameBitmap(32 + 6, 64 + 6);

            _menuItemsText = "属性魔法物品系统".GetBytes();

            _menuItemsRect = new Rectangle(9 + 3, 3 + 16 + 6 - 1 + 3, 32, 64);

            //TODO ScreenSelectCharacter独立出去后，还未测试多人情况下是否正常运行
            _screenSelectCharacter = new ScreenSelectCharacter(Context, (index) =>
            {
                return GetScreenMagic(index);
            });
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_rectangleTopImg, 9, 3);
            TextRender.DrawText(canvas, "金钱:" + Context.PlayContext.Money, 9 + 3, 3 + 3);
            canvas.DrawBitmap(_rectangleLeftImg, 9, 3 + 16 + 6 - 1);
            TextRender.DrawText(canvas, _menuItemsText, 0, _menuItemsRect);
            TextRender.DrawSelText(canvas, _operateItems[_selectIndex], _menuItemsRect.Left,
                    _menuItemsRect.Top + _selectIndex * 16);
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP)
            {
                if (--_selectIndex < 0)
                {
                    _selectIndex = 3;
                }
            }
            else if (key == SimulatorKeys.KEY_DOWN)
            {
                if (++_selectIndex > 3)
                {
                    _selectIndex = 0;
                }
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                BaseScreen screen = null;
                switch (_selectIndex)
                {
                    case 0:
                        screen = new ScreenMenuProperties(Context);
                        break;

                    case 1:
                        screen = Context.PlayContext.PlayerCharacters.Count > 1 ? _screenSelectCharacter : GetScreenMagic(0);
                        break;

                    case 2:
                        screen = new ScreenMenuGoods(Context);
                        break;

                    case 3:
                        screen = new ScreenMenuSystem(Context);
                        break;
                }
                if (screen != null)
                {
                    Context.PushScreen(screen);
                }
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
        /// 获取魔法使用界面
        /// </summary>
        /// <param name="id">0 1 2</param>
        /// <returns></returns>
        private ScreenMagic GetScreenMagic(int id)
        {
            if (Context.PlayContext.PlayerCharacters[id].MagicChain?.LearnCount > 0)
            {
                return new ScreenMagic(Context, Context.PlayContext.PlayerCharacters[id].MagicChain,
                    (magic) =>
                    {
                        if (magic is MagicRestore)
                        {
                            Context.PushScreen(new ScreenUseMagic(Context, (MagicRestore)magic, Context.PlayContext.PlayerCharacters[id]));
                        }
                        else
                        {
                            Context.ShowMessage("此处无法使用!", 1000);
                        }
                    });
            }
            return null;
        }

        #endregion 方法
    }
}