using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 角色状态界面
    /// </summary>
    internal class ScreenCharacterState : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 当前角色ID
        /// </summary>
        private int _curCharacterId = 0;

        /// <summary>
        /// 当前显示页
        /// </summary>
        private int _page = 0;

        /// <summary>
        /// 角色列表
        /// </summary>
        private List<PlayerCharacter> _playerList = null;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 角色状态界面
        /// </summary>
        /// <param name="context"></param>
        public ScreenCharacterState(SimulatorContext context) : base(context)
        {
            _playerList = Context.PlayContext.PlayerCharacters;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawColor(Constants.COLOR_WHITE);

            int i = 0;
            while (i < _playerList.Count)
            {
                _playerList[i].DrawHead(canvas, 10, 2 + 32 * i);
                ++i;
            }

            if (_playerList.Count > 0)
            {
                _playerList[_curCharacterId].DrawState(canvas, _page);
                Context.Util.DrawTriangleCursor(canvas, 3, 10 + 32 * _curCharacterId);
            }
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_PAGEDOWN || key == SimulatorKeys.KEY_PAGEUP)
            {
                _page = 1 - _page;
            }
            else if (key == SimulatorKeys.KEY_DOWN)
            {
                ++_curCharacterId;
                if (_curCharacterId >= _playerList.Count)
                {
                    _curCharacterId = 0;
                }
            }
            else if (key == SimulatorKeys.KEY_UP)
            {
                --_curCharacterId;
                if (_curCharacterId < 0)
                {
                    _curCharacterId = _playerList.Count - 1;
                }
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.PopScreen();
            }
        }

        public override void Update(long delta)
        { }

        #endregion 方法
    }
}