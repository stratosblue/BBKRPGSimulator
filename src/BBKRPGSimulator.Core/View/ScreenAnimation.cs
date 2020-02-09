using System;

using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.View
{
    /// <summary>
    /// 动画界面
    /// </summary>
    internal class ScreenAnimation : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 动画资源
        /// </summary>
        private ResSrs _animation;

        /// <summary>
        /// 资源索引号
        /// </summary>
        private int _index;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 动画界面
        /// 247、248、249分别代表游戏开发组的商标、游戏的名称 以及游戏战斗失败后的过场动画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="index"></param>
        public ScreenAnimation(SimulatorContext context, int index) : base(context)
        {
            if (index != 247 && index != 248 && index != 249)
            {
                throw new Exception("只能是247,248,249");
            }
            _index = index;
            _animation = Context.LibData.GetSrs(1, index);
            _animation.SetIteratorNum(4);
            _animation.StartAni();
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawColor(Constants.COLOR_WHITE);
            _animation.Draw(canvas, 0, 0);
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_CANCEL && (_index == 247 || _index == 248))
            {
                Context.ChangeScreen(ScreenEnum.SCREEN_MENU);
            }
        }

        public override void OnKeyUp(int key)
        {
        }

        public override void Update(long delta)
        {
            if (!_animation.Update(delta))
            {
                if (_index == 247)
                {
                    // 转到游戏动画
                    Context.ChangeScreen(ScreenEnum.SCREEN_GAME_LOGO);
                }
                else if (_index == 248)
                {
                    // 转到游戏菜单
                    Context.ChangeScreen(ScreenEnum.SCREEN_MENU);
                }
                else if (_index == 249)
                {
                    Context.ChangeScreen(ScreenEnum.SCREEN_MENU);
                }
            }
        }

        #endregion 方法
    }
}