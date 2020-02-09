using System;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.View;
using BBKRPGSimulator.View.GameMenu;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 菜单命令
    /// </summary>
    internal class CommandMenu : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 菜单命令
        /// </summary>
        /// <param name="context"></param>
        public CommandMenu(ArraySegment<byte> data, SimulatorContext context) : base(data, -1, context)
        {
            Length = data.GetStringLength(2);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandMenuOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandMenuOperate : Operate
        {
            #region 字段

            private readonly BaseScreen _menu;
            private readonly string[] _menuItems;
            private readonly int _varIndex;
            private bool _finished = false;

            #endregion 字段

            #region 构造函数

            public CommandMenuOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                _varIndex = data.Get2BytesUInt(0);
                _menuItems = data.GetString(2).Split(' ');
                _menu = new ScreenCommonMenu(_menuItems, selectedIndex =>
                {
                    Context.ScriptProcess.ScriptState.Variables[_varIndex] = selectedIndex;
                    _finished = true;
                }, Context);
                Context.PushScreen(_menu);
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                _menu.Draw(canvas);
            }

            public override void OnKeyDown(int key) => _menu.OnKeyDown(key);

            public override void OnKeyUp(int key) => _menu.OnKeyUp(key);

            public override bool Update(long delta)
            {
                if (_finished)
                {
                    return false;
                }
                _menu.Update(delta);
                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}