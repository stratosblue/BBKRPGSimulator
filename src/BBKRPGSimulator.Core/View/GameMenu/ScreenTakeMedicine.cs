using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 吃药界面
    /// </summary>
    internal class ScreenTakeMedicine : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 角色选择索引
        /// </summary>
        private int _characterIndex = -1;

        /// <summary>
        /// 要使用的药品
        /// </summary>
        private BaseGoods _medicine;

        /// <summary>
        /// 人物属性页索引，共两页
        /// </summary>
        private int _statePageIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 吃药界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="goods"></param>
        public ScreenTakeMedicine(SimulatorContext context, BaseGoods goods) : base(context)
        {
            _medicine = goods;
            if (Context.PlayContext.PlayerCharacters.Count > 0)
            {
                _characterIndex = 0;
            }
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawColor(Constants.COLOR_WHITE);
            Context.PlayContext.PlayerCharacters[_characterIndex].DrawState(canvas, _statePageIndex);
            Context.PlayContext.PlayerCharacters[_characterIndex].DrawHead(canvas, 5, 60);
            if (_medicine.GoodsNum > 0)
            {
                _medicine.Draw(canvas, 5, 10);
                TextRender.DrawText(canvas, "" + _medicine.GoodsNum, 13, 35);
            }
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_PAGEDOWN)
            {
                _statePageIndex = 1;
            }
            else if (key == SimulatorKeys.KEY_PAGEUP)
            {
                _statePageIndex = 0;
            }
            else if (key == SimulatorKeys.KEY_LEFT && _characterIndex > 0)
            {
                --_characterIndex;
            }
            else if (key == SimulatorKeys.KEY_RIGHT && _characterIndex < Context.PlayContext.PlayerCharacters.Count - 1)
            {
                ++_characterIndex;
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
                if (_medicine.GoodsNum > 0)
                {
                    if (_medicine.Type == 9 && ((GoodsMedicine)_medicine).IsEffectAll())    //普通药物，并全体有效
                    {
                        for (int i = Context.PlayContext.PlayerCharacters.Count - 1; i >= 0; i--)
                        {
                            Context.PlayContext.PlayerCharacters[i].UseMedicine(_medicine);
                        }
                    }
                    else    //仙药、灵药 不具有全体效果的普通药物
                    {
                        Context.PlayContext.PlayerCharacters[_characterIndex].UseMedicine(_medicine);
                    }
                }
                else
                {
                    Context.PopScreen();
                }
            }
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}