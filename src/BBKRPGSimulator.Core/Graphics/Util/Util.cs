using System;
using System.Diagnostics;
using System.Drawing;

using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Graphics.Util
{
    internal class Util : ContextDependent
    {
        #region 字段

        public ImageBuilder bmpChuandai;

        public Paint sBlackPaint = new Paint(PaintStyle.STROKE, Constants.COLOR_BLACK);

        // 显示message的方框
        private ImageBuilder[] bmpInformationBg;

        // 屏幕两边留白
        private ImageBuilder bmpSideFrame;

        private ImageBuilder bmpTriangleCursor;

        // 用于菜单的矩形框，黑框白边
        private Paint drawFramePaint = new Paint(PaintStyle.STROKE, Constants.COLOR_BLACK);

        private ResImage imgSmallNum;

        private TextRender TextRender => Context.TextRender;

        #endregion 字段

        #region 构造函数

        public Util(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public void DrawSideFrame(ICanvas canvas)
        {
            canvas.DrawBitmap(bmpSideFrame, 0, 0);
            canvas.DrawBitmap(bmpSideFrame, 152, 0);
        }

        /// <summary>
        ///
        /// @return 画出的num宽度(像素)
        /// </summary>
        public int DrawSmallNum(ICanvas canvas, int num, int x, int y)
        {
            if (num < 0)
            {
                num = -num;
            }
            byte[] nums = num.ToString().GetBytes();

            for (int i = 0; i < nums.Length; i++)
            {
                imgSmallNum.Draw(canvas, nums[i] - '0' + 1, x, y);
                x += imgSmallNum.Width + 1;
            }

            return nums.Length * imgSmallNum.Width;
        }

        public void DrawTriangleCursor(ICanvas canvas, int x, int y)
        {
            canvas.DrawBitmap(bmpTriangleCursor, x, y);
        }

        public ImageBuilder GetFrameBitmap(int w, int h)
        {
            // 先创建Bitmap
            ImageBuilder bmp = Context.GraphicsFactory.NewImageBuilder(w, h);
            var tmpC = Context.GraphicsFactory.NewCanvas(bmp);
            tmpC.DrawColor(Constants.COLOR_WHITE);
            tmpC.DrawRect(1, 1, w - 2, h - 2, drawFramePaint);
            return bmp;
        }

        public ImageBuilder GetSmallSignedNumBitmap(int num)
        {
            byte[] nums = (num > 0 ? num : -num).ToString().GetBytes();
            ResImage sign = Context.LibData.GetImage(2, num > 0 ? 6 : 7);
            ImageBuilder bmp = Context.GraphicsFactory.NewImageBuilder(sign.Width + nums.Length * imgSmallNum.Width + 1 + nums.Length, imgSmallNum.Height);

            var c = Context.GraphicsFactory.NewCanvas(bmp);
            sign.Draw(c, 1, 0, 0);

            int x = sign.Width + 1;
            for (int i = 0; i < nums.Length; i++)
            {
                imgSmallNum.Draw(c, nums[i] - '0' + 1, x, 0);
                x += imgSmallNum.Width + 1;
            }

            return bmp;
        }

        public void Init()
        {
            var canvas = Context.GraphicsFactory.NewCanvas();

            Paint paint = new Paint();
            paint.SetColor(Constants.COLOR_WHITE);
            paint.SetStyle(PaintStyle.FILL_AND_STROKE);

            if (bmpInformationBg == null)
            {
                bmpInformationBg = new ImageBuilder[5];
                for (int i = 0; i < 5; i++)
                {
                    bmpInformationBg[i] = Context.GraphicsFactory.NewImageBuilder(138, 23 + 16 * i);
                    canvas.SetBitmap(bmpInformationBg[i]);
                    canvas.DrawColor(Constants.COLOR_BLACK);
                    canvas.DrawRect(1, 1, 135, 20 + 16 * i, paint);
                    canvas.DrawRect(136, 0, 138, 3, paint);
                    canvas.DrawLine(0, 21 + 16 * i, 3, 21 + 16 * i, paint);
                    canvas.DrawLine(0, 22 + 16 * i, 3, 22 + 16 * i, paint);
                }
            }

            if (bmpSideFrame == null)
            {
                bmpSideFrame = Context.GraphicsFactory.NewImageBuilder(8, 96);
                canvas.SetBitmap(bmpSideFrame);
                canvas.DrawColor(Constants.COLOR_WHITE);
                paint.SetColor(Constants.COLOR_BLACK);
                for (int i = 0; i < 8; i += 2)
                {
                    canvas.DrawLine(i, 0, i, 96, paint);
                }
            }

            if (bmpTriangleCursor == null)
            {
                bmpTriangleCursor = Context.GraphicsFactory.NewImageBuilder(7, 13);
                canvas.SetBitmap(bmpTriangleCursor);
                canvas.DrawColor(Constants.COLOR_WHITE);
                for (int i = 0; i < 7; ++i)
                {
                    canvas.DrawLine(i, i, i, 13 - i, paint);
                }
            }

            if (imgSmallNum == null)
            {
                imgSmallNum = Context.LibData.GetImage(2, 5);
            }

            if (bmpChuandai == null)
            {
                bmpChuandai = Context.GraphicsFactory.NewImageBuilder(22, 39);
                int b = Constants.COLOR_BLACK, w = Constants.COLOR_WHITE;
                int[] pixels = {
                    w,w,w,w,w,w,w,w,w,b,b,b,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,b,b,w,w,b,b,b,b,b,b,b,b,b,b,b,b,b,w,w,
                    w,w,b,b,b,b,b,w,w,w,w,w,w,w,w,b,b,b,b,b,w,w,
                    w,w,b,b,w,w,w,b,b,b,w,w,b,b,b,w,w,w,b,b,w,w,
                    w,w,w,b,w,w,b,w,w,w,w,w,w,w,w,w,w,w,b,b,w,w,
                    w,w,w,b,w,w,b,b,b,b,b,b,b,b,b,b,b,w,w,w,w,w,
                    w,w,w,w,w,w,w,b,w,w,w,w,b,b,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,b,b,b,b,b,b,b,b,b,b,b,b,b,b,b,b,
                    w,w,w,w,w,w,b,b,w,w,w,b,b,b,b,w,b,b,b,b,b,b,
                    w,w,w,w,w,w,w,w,w,b,b,b,b,b,b,w,w,w,b,b,b,w,
                    w,w,w,w,w,w,b,b,b,b,w,w,b,b,b,w,w,w,w,w,w,w,
                    b,b,b,b,b,b,b,b,w,w,w,w,b,b,b,w,w,w,w,w,w,w,
                    w,b,b,b,b,w,w,w,w,w,w,w,b,b,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,b,b,b,b,b,b,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,b,b,b,b,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,
                    w,w,w,w,w,w,b,w,w,w,w,b,b,w,w,w,b,w,w,w,w,w,
                    w,w,w,b,b,b,b,b,b,b,w,b,b,w,w,b,b,w,w,w,w,w,
                    w,w,b,b,w,w,b,w,w,w,w,b,b,w,b,w,w,w,w,w,w,w,
                    w,w,b,b,b,b,b,b,b,b,b,b,b,b,b,b,b,b,w,w,w,w,
                    b,b,b,b,b,b,b,b,b,b,b,b,b,b,b,b,b,w,w,w,w,w,
                    w,w,w,b,b,b,b,b,b,b,w,b,b,w,b,b,w,w,w,w,w,w,
                    w,w,w,b,b,w,b,b,b,b,w,b,b,w,b,b,w,w,w,w,w,w,
                    w,w,w,b,w,b,b,w,w,b,w,w,b,w,b,b,w,w,w,w,w,w,
                    w,w,w,b,b,b,b,b,b,b,w,w,b,b,b,b,w,w,w,w,w,w,
                    w,w,w,b,b,b,b,b,b,b,w,w,b,b,b,w,w,w,w,w,w,w,
                    w,w,w,w,b,b,b,b,b,w,w,w,w,b,b,b,w,w,w,w,w,w,
                    b,b,b,b,b,w,w,w,w,b,b,b,b,b,b,b,b,b,b,w,w,w,
                    w,w,w,b,b,b,w,w,b,b,w,b,b,w,w,b,b,b,b,b,b,b,
                    w,w,b,b,w,w,w,w,w,w,b,w,w,w,w,w,b,b,b,b,b,w,
                    w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,w,b,b,w,w,
            };
                bmpChuandai.SetPixels(0, 0, 22, 39, ImageBuilderUtil.PixelsToBuffer(pixels), 0, 22);
            }
        }

        // 用于showscenename
        public void ShowInformation(ICanvas canvas, string msg)
        {
            canvas.DrawBitmap(bmpInformationBg[0], 11, 37);
            TextRender.DrawText(canvas, msg, 16, 39);
        }

        // 显示message,每行最多显示8个汉字，最多可显示5行
        public void ShowMessage(ICanvas canvas, string msg)
        {
            try
            {
                byte[] text = msg.GetBytes();
                int lineNum = text.Length / 16; // 所需行数
                if (lineNum >= 5)
                {
                    lineNum = 4;
                }
                int textY = 39 - lineNum * 8;
                canvas.DrawBitmap(bmpInformationBg[lineNum], 11, textY - 2);
                TextRender.DrawText(canvas, text, 0, new Rectangle(16, textY, 16 * 8, 16 * lineNum + 16));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        // 显示message,每行最多显示8个汉字，最多可显示5行
        public void ShowMessage(ICanvas canvas, byte[] msg)
        {
            int lineNum = msg.Length / 16;
            if (lineNum >= 5)
            {
                lineNum = 4;
            }
            int textY = 39 - lineNum * 8;
            canvas.DrawBitmap(bmpInformationBg[lineNum], 11, textY - 2);
            TextRender.DrawText(canvas, msg, 0, new Rectangle(16, textY, 16 * 8, 16 * lineNum + 16));
        }

        #endregion 方法
    }
}