using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 选择命令
    /// </summary>
    internal class CommandChoice : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 选择命令
        /// </summary>
        /// <param name="context"></param>
        public CommandChoice(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            int i = 0;
            while (code[start + i] != 0) ++i;
            ++i;
            while (code[start + i] != 0) ++i;
            return start + i + 3;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandChoiceOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 选择命令的操作
        /// </summary>
        private class CommandChoiceOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 地址偏移
            /// </summary>
            private readonly int _addrOffset;

            /// <summary>
            /// 背景的XY坐标
            /// </summary>
            private readonly int _backgroundX, _backgroundY;

            /// <summary>
            /// 背景图片
            /// </summary>
            private ImageBuilder _background;

            /// <summary>
            /// 选项1的字符串数据
            /// </summary>
            private byte[] _choice1;

            /// <summary>
            /// 选项2的字符串数据
            /// </summary>
            private byte[] _choice2;

            private byte[] _code;

            /// <summary>
            /// 是否已选择
            /// </summary>
            private bool _hasSelected;

            /// <summary>
            /// 最后一次按下的键
            /// </summary>
            private int _lastDownKey = -1;

            /// <summary>
            /// 当前选择的索引
            /// </summary>
            private int _selectedIndex;

            private int _start;

            #endregion 字段

            #region 属性

            private TextRender TextRender => Context.TextRender;

            #endregion 属性

            #region 构造函数

            /// <summary>
            /// 选择命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandChoiceOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;

                _choice1 = code.GetStringBytes(start);
                _choice2 = code.GetStringBytes(start + _choice1.Length);

                _addrOffset = _choice1.Length + _choice2.Length;

                int maxLength = _choice1.Length > _choice2.Length ? _choice1.Length : _choice2.Length;
                int width = maxLength * 8 - 8 + 6;
                _choice1 = _choice1.FixStringLength(maxLength);
                _choice2 = _choice2.FixStringLength(maxLength);

                _background = Context.Util.GetFrameBitmap(width, 16 * 2 + 6);
                _backgroundX = (160 - _background.Width) / 2;
                _backgroundY = (96 - _background.Height) / 2;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.SceneMap.DrawScene(canvas);
                canvas.DrawBitmap(_background, _backgroundX, _backgroundY);
                if (_selectedIndex == 0)
                {
                    TextRender.DrawSelText(canvas, _choice1, _backgroundX + 3, _backgroundY + 3);
                    TextRender.DrawText(canvas, _choice2, _backgroundX + 3, _backgroundY + 3 + 16);
                }
                else
                {
                    TextRender.DrawText(canvas, _choice1, _backgroundX + 3, _backgroundY + 3);
                    TextRender.DrawSelText(canvas, _choice2, _backgroundX + 3, _backgroundY + 3 + 16);
                }
            }

            public override void OnKeyDown(int key)
            {
                if (key == SimulatorKeys.KEY_DOWN ||
                    key == SimulatorKeys.KEY_UP ||
                    key == SimulatorKeys.KEY_LEFT ||
                    key == SimulatorKeys.KEY_RIGHT)
                {
                    _selectedIndex = 1 - _selectedIndex;
                }
                _lastDownKey = key;
            }

            public override void OnKeyUp(int key)
            {
                if (key == SimulatorKeys.KEY_ENTER && _lastDownKey == key)
                {
                    _hasSelected = true;
                }
            }

            public override bool Process()
            {
                _selectedIndex = 0;
                _hasSelected = false;
                return true;
            }

            public override bool Update(long delta)
            {
                if (_hasSelected)
                {
                    if (_selectedIndex == 1)
                    {
                        Context.ScriptProcess.GotoAddress(_code.Get2BytesUInt(_start + _addrOffset));
                    }
                    return false;
                }
                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}