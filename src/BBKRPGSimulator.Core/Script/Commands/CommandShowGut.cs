using System;
using System.Drawing;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 显示脚本命令
    /// </summary>
    internal class CommandShowGut : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 显示脚本命令
        /// </summary>
        /// <param name="context"></param>
        public CommandShowGut(ArraySegment<byte> data, SimulatorContext context) : base(data, -1, context)
        {
            Length = data.GetStringLength(4) + 4;
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandShowGutOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandShowGutOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 显示的内容
            /// </summary>
            private readonly string _content;

            /// <summary>
            /// 文本位置
            /// </summary>
            private readonly Rectangle _textArea;

            /// <summary>
            /// 脚本的图片
            /// </summary>
            private readonly ResImage _topImg, _bottomImg;

            /// <summary>
            /// 当前显示的Y坐标
            /// </summary>
            private int _curShowY;

            /// <summary>
            /// 显示的时间间隔
            /// </summary>
            private long _interval = 50;

            /// <summary>
            /// 是否跳过
            /// </summary>
            private bool _skip = false;

            /// <summary>
            /// 显示速度
            /// </summary>
            private int _speed = 1;

            /// <summary>
            /// 显示时间计数
            /// </summary>
            private long _timeCount = 0;

            #endregion 字段

            #region 属性

            private TextRender TextRender => Context.TextRender;

            #endregion 属性

            #region 构造函数

            public CommandShowGutOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                var start = data.Offset;
                var code = data.Array;

                int topImgIndex = code[start] & 0xFF | code[start + 1] << 8 & 0xFF00;
                int bottomImgIndex = code[start + 2] & 0xFF | code[start + 3] << 8 & 0xFF00;
                _topImg = Context.LibData.GetImage(5, topImgIndex);
                _bottomImg = Context.LibData.GetImage(5, bottomImgIndex);
                _content = code.GetString(start + 4);
                _curShowY = _bottomImg != null ? 96 - _bottomImg.Height : 96;

                int rectTop = _topImg != null ? _topImg.Height : 0;
                _textArea = new Rectangle(0, rectTop, 160, _curShowY - rectTop);

                _skip = false;
                _interval = 50;
                _timeCount = 0;
                _speed = 1;
                _curShowY = _bottomImg != null ? 96 - _bottomImg.Height : 96;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                canvas.DrawColor(Constants.COLOR_WHITE);
                int showResult = TextRender.DrawText(canvas, _content, _textArea, _curShowY);
                if (showResult != 1 && showResult != 2)
                {
                    _skip = true;
                }
                if (_topImg != null)
                {
                    _topImg.Draw(canvas, 1, 0, 0);
                }
                if (_topImg != null)
                {
                    _bottomImg.Draw(canvas, 1, 0, 96 - _bottomImg.Height);
                }
            }

            public override void OnKeyDown(int key)
            {
                _speed = 3;
                _interval = 20;
            }

            public override void OnKeyUp(int key)
            {
                if (key == SimulatorKeys.KEY_CANCEL)
                {
                    _skip = true;
                }
                _speed = 1;
                _interval = 50;
            }

            public override bool Update(long delta)
            {
                if (_skip)
                {
                    return false;
                }
                _timeCount += delta;
                if (_timeCount >= _interval)
                {
                    _timeCount = 0;
                    _curShowY -= _speed;
                }
                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}