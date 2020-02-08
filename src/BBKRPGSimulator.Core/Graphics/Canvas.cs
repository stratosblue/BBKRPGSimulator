using System;
using System.Drawing;

namespace BBKRPGSimulator.Graphics
{
    /// <summary>
    /// 画布
    /// </summary>
    internal class Canvas : ICanvas
    {
        #region 字段

        /// <summary>
        /// 当前画布背景
        /// </summary>
        public ImageBuilder Background { get; private set; }

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 新建画布并设置背景
        /// </summary>
        /// <param name="bitmap"></param>
        internal Canvas(ImageBuilder bitmap)
        {
            Background = bitmap;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void DrawBitmap(ImageBuilder bitmap, int left, int top)
        {
            Background.Draw(bitmap, left, top);
        }

        /// <summary>
        /// 填充颜色
        /// </summary>
        /// <param name="color"></param>
        public void DrawColor(int color)
        {
            Background.FillRectangle(color, 0, 0, Background.Width, Background.Height);
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="stopX"></param>
        /// <param name="stopY"></param>
        /// <param name="paint"></param>
        public void DrawLine(int startX, int startY, int stopX, int stopY, Paint paint)
        {
            Background.DrawLine(paint.Color, startX, startY, stopX, stopY);
        }

        /// <summary>
        /// 绘制连续线段？？？
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="paint"></param>
        public void DrawLines(float[] pts, Paint paint)
        {
            int size = pts.Length / 4;
            for (int i = 0; i < size; i++)
            {
                Background.DrawLine(paint.Color, (int)pts[i * 4], (int)pts[(i * 4) + 1], (int)pts[(i * 4) + 2], (int)pts[((i * 4)) + 3]);
            }
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="paint"></param>
        public void DrawRect(int left, int top, int right, int bottom, Paint paint)
        {
            DrawRectangle(left, top, right - left, bottom - top, paint);
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="pait"></param>
        public void DrawRect(Rectangle rectangle, Paint pait)
        {
            DrawRectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height, pait);
        }

        /// <summary>
        /// 缩放画面
        /// </summary>
        /// <param name="mScale"></param>
        /// <param name="mScale2"></param>
        public void Scale(float mScale, float mScale2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置画布背景
        /// </summary>
        /// <param name="bitmap"></param>
        public void SetBitmap(ImageBuilder bitmap)
        {
            Background = bitmap;
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="paint"></param>
        private void DrawRectangle(int left, int top, int width, int height, Paint paint)
        {
            if (paint.Style == PaintStyle.FILL)
            {
                Background.FillRectangle(paint.Color, left, top, width, height);
            }
            else if (paint.Style == PaintStyle.STROKE)
            {
                Background.DrawRectangle(paint.Color, left, top, width, height);
            }
            else
            {
                Background.FillRectangle(paint.Color, left, top, width, height);
            }
        }

        #endregion 方法
    }
}