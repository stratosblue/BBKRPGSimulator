using System;
using System.Drawing;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 说话命令
    /// </summary>
    internal class CommandSay : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 说话命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSay(ArraySegment<byte> data, SimulatorContext context) : base(data, -1, context)
        {
            var start = data.Offset;
            var code = data.Array;

            int i = 2;
            while (code[start + i] != 0) ++i;
            Length = i + 1;
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandSayOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandSayOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 说话内容
            /// </summary>
            private readonly byte[] _content;

            /// <summary>
            /// 头像图片
            /// </summary>
            private readonly ResImage _headImg;

            /// <summary>
            /// 画笔
            /// </summary>
            private readonly Paint _paint = new Paint();

            /// <summary>
            /// 有图大框边框
            /// </summary>
            private readonly Rectangle _rectangle;

            /// <summary>
            /// 底部框
            /// </summary>
            private readonly Rectangle _rectangleBottom;

            /// <summary>
            /// 顶部框
            /// </summary>
            private readonly Rectangle _rectangleTop;

            /// <summary>
            /// 接下来的文字索引
            /// </summary>
            private int _indexOfNext = 0;

            /// <summary>
            /// 当前文字索引
            /// </summary>
            private int _indexOfText = 0;

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyDown = false;

            #endregion 字段

            #region 属性

            private TextRender TextRender => Context.TextRender;

            #endregion 属性

            #region 构造函数

            public CommandSayOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                var start = data.Offset;
                var code = data.Array;

                _content = code.GetStringBytes(start + 2);

                //获取头像资源索引
                var _picIndex = code.Get2BytesUInt(start);
                if (_picIndex == 0) //没有头像
                {
                    _rectangle = new Rectangle(9, 55, 142, 41 - 1); // new RectangleF(9, 55, 142, 41 - 0.5f);
                    _rectangleTop = new Rectangle(14, 58, 131, 17);
                    _rectangleBottom = new Rectangle(14, 76, 131, 17);
                }
                else    //有头像
                {
                    _headImg = Context.LibData.GetImage(1, _picIndex);

                    _rectangle = new Rectangle(9, 50, 142, 46 - 1); // new RectangleF(9, 50, 142, 46 - 0.5f);
                    _rectangleTop = new Rectangle(44, 58, 101, 17);
                    _rectangleBottom = new Rectangle(14, 76, 131, 17);
                }

                _paint.SetColor(Constants.COLOR_BLACK);
                _paint.SetStyle(PaintStyle.FILL_AND_STROKE);

                _indexOfText = 0;
                _indexOfNext = 0;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                if (!Context.CombatManage.IsActive)
                {
                    Context.SceneMap.DrawScene(canvas);
                }

                //TODO 确认精简逻辑后的正确性
                // 画矩形
                _paint.SetColor(Constants.COLOR_WHITE);
                _paint.SetStyle(PaintStyle.FILL);
                canvas.DrawRect(_rectangle, _paint);
                // 画边框
                _paint.SetColor(Constants.COLOR_BLACK);
                _paint.SetStyle(PaintStyle.STROKE);
                _paint.SetStrokeWidth(1);
                canvas.DrawRect(_rectangle, _paint);
                if (_headImg != null)   //画头像
                {
                    canvas.DrawLine(38, 50, 44, 56, _paint);
                    //HACK 强制改为Int了，之前绘制有浮点数？
                    //canvas.DrawLine(43.5f, 56, 151, 56, paint);
                    canvas.DrawLine(44, 56, 151, 56, _paint);
                    _headImg.Draw(canvas, 1, 13, 46);
                }
                _indexOfNext = TextRender.DrawText(canvas, _content, _indexOfText, _rectangleTop);
                _indexOfNext = TextRender.DrawText(canvas, _content, _indexOfNext, _rectangleBottom);
            }

            public override void OnKeyDown(int key)
            {
                _isAnyKeyDown = true;
            }

            public override bool Update(long delta)
            {
                if (_isAnyKeyDown)
                {
                    if (_indexOfNext >= _content.Length - 1)
                    {
                        // 最后一位是0
                        return false;
                    }
                    else
                    {
                        _indexOfText = _indexOfNext;
                        _isAnyKeyDown = false;
                    }
                }
                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}