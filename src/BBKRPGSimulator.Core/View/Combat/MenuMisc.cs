using System.Drawing;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Actions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.Combat
{
    /// <summary>
    /// 围攻、道具、防御、逃跑、状态界面
    /// </summary>
    internal class MenuMisc : BaseCombatScreen
    {
        #region 静态定义

        /// <summary>
        /// 菜单选项文本数据
        /// </summary>
        private static readonly byte[] _menuItems = "围攻道具防御逃跑状态".GetBytes();

        /// <summary>
        /// 菜单选项当前选择文本数据
        /// </summary>
        private static readonly byte[][] _menuSelectItems = new byte[][]{
                "围攻".GetBytes(),
                "道具".GetBytes(),
                "防御".GetBytes(),
                "逃跑".GetBytes(),
                "状态".GetBytes()};

        /// <summary>
        /// 展示的矩形位置
        /// </summary>
        private static readonly Rectangle _showRectangle = new Rectangle(12, 7, 33, 80);//new Rectangle(9 + 3, 4 + 3, 9 + 4 + 16 * 2, 4 + 3 + 16 * 5);

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 背景图
        /// </summary>
        private readonly ImageBuilder _background = null;

        /// <summary>
        /// 当前选项索引
        /// </summary>
        private int _curSelectedIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 围攻、道具、防御、逃跑、状态界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="combatUI"></param>
        public MenuMisc(SimulatorContext context, CombatUI combatUI) : base(context, combatUI)
        {
            _background = Context.Util.GetFrameBitmap(2 * 16 + 6, 5 * 16 + 6);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 9, 4);
            TextRender.DrawText(canvas, _menuItems, 0, _showRectangle);
            TextRender.DrawSelText(canvas, _menuSelectItems[_curSelectedIndex], _showRectangle.Left, _showRectangle.Top + _curSelectedIndex * 16);
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP)
            {
                --_curSelectedIndex;
                _curSelectedIndex = (_menuSelectItems.Length + _curSelectedIndex) % _menuSelectItems.Length;
            }
            else if (key == SimulatorKeys.KEY_DOWN)
            {
                ++_curSelectedIndex;
                _curSelectedIndex %= _menuSelectItems.Length;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                switch (_curSelectedIndex)
                {
                    case 0://围攻
                        if (_combatUI.mCallBack != null)
                        {
                            _combatUI.mCallBack.OnAutoAttack();
                        }
                        break;

                    case 1://道具
                        _combatUI.ScreenStack.Push(new MenuGoods(Context, _combatUI));
                        break;

                    case 2://防御
                        PlayerCharacter p = _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex];
                        p.FightingSprite.CurrentFrame = 9;
                        _combatUI.OnActionSelected(new ActionDefend(Context, p));
                        break;

                    case 3://逃跑
                        if (_combatUI.mCallBack != null)
                        {
                            _combatUI.mCallBack.OnFlee();
                        }
                        break;

                    case 4://状态
                        _combatUI.ScreenStack.Pop();
                        _combatUI.ScreenStack.Push(new MenuState(Context, _combatUI));
                        break;
                }
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                _combatUI.ScreenStack.Pop();
            }
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}