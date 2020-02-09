using System;

namespace BBKRPGSimulator.Graphics
{
    /// <summary>
    /// 图片构建器
    /// </summary>
    internal class InternalImageBuilder : ImageBuilder
    {
        #region 构造函数

        /// <summary>
        /// 内部实现的图像构建器
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public InternalImageBuilder(int width, int height) : base(width, height)
        {
            Data = new byte[width * height * 4];
        }

        /// <summary>
        /// 内部实现的图像构建器
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public InternalImageBuilder(byte[] data, int width, int height) : base(data, width, height)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 设置RGB数据
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="offset"></param>
        /// <param name="stride"></param>
        public override void SetPixels(int left, int top, int width, int height, byte[] pixels, int offset, int stride)
        {
            var imageBuffer = pixels;

            //像素数组的游标
            int bufferIndex = 0;

            for (int v = 0; v < height; v++)    //控制行
            {
                for (int h = 0; h < width; h++)    //控制当前行的位置
                {
                    var tempX = left + h;
                    var tempY = top + v;
                    if (tempX < 0 || tempX > Width || tempY < 0 || tempY > Height)
                    {
                        continue;
                    }
                    var index = (Width * (v + top) + (h + left)) * 4;
                    Data[index] = imageBuffer[bufferIndex];
                    Data[index + 1] = imageBuffer[bufferIndex + 1];
                    Data[index + 2] = imageBuffer[bufferIndex + 2];
                    Data[index + 3] = imageBuffer[bufferIndex + 3];

                    bufferIndex += 4;
                }
            }
        }

        #endregion 方法

        #region 绘制

        /// <summary>
        /// 将另一个图片绘制到当前图片的指定位置
        /// </summary>
        /// <param name="imageBuilder">另一个图片的构建器</param>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        public override void Draw(ImageBuilder imageBuilder, int left, int top)
        {
            byte[] simageByte = Data;
            byte[] dimageByte = imageBuilder.Data;

            int dpixelWidth = imageBuilder.Width;
            int dpixelHeight = imageBuilder.Height;

            int index = 0;
            int doffset = 0;

            for (int v = 0; v < dpixelHeight; v++)
            {
                for (int h = 0; h < dpixelWidth; h++)
                {
                    if (dimageByte[doffset + 3] == 0)   //透明色，不处理
                    {
                        doffset += 4;
                        continue;
                    }

                    index = (Width * (v + top) + (h + left)) * 4;

                    if (index < 0)
                    {
                        doffset += 4;
                        continue;
                    }
                    else if (index >= simageByte.Length - 3)
                    {
                        break;
                    }

                    simageByte[index] = dimageByte[doffset];
                    simageByte[index + 1] = dimageByte[doffset + 1];
                    simageByte[index + 2] = dimageByte[doffset + 2];
                    simageByte[index + 3] = dimageByte[doffset + 3];

                    doffset += 4;
                }
            }
        }

        /// <summary>
        /// 绘制线段
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x1">起始x坐标</param>
        /// <param name="y1">起始y坐标</param>
        /// <param name="x2">终止x坐标</param>
        /// <param name="y2">终止y坐标</param>
        public override void DrawLine(int color, int x1, int y1, int x2, int y2)
        {
            ImageBuilderUtil.ColorToArgb(color, out var a, out var r, out var g, out var b);
            int index = 0;

            if (x1 != x2 && y1 != y2)
            {
                //TODO 未全面测试斜线绘制

                //ColorToArgb(-65536, out a, out r, out g, out b);
                int drawX = x1;
                int drawY = y1;
                int drawEndX = x2;
                int drawEndY = y2;

                //横向偏移量
                float hoffset = 0;
                //纵向偏移量
                float voffset = 0;

                var hlength = drawX - drawEndX;
                var vlength = drawY - drawEndY;
                int step = 0;
                var hlength_abs = Math.Abs(hlength);
                var vlength_abs = Math.Abs(vlength);

                if (hlength_abs == vlength_abs) //斜线
                {
                    hoffset = hlength > 0 ? -1 : 1;
                    voffset = vlength > 0 ? -1 : 1;
                    step = hlength_abs;
                }
                else if (hlength_abs > vlength_abs)    //横向位移较大
                {
                    hoffset = hlength > 0 ? -1 : 1;
                    voffset = vlength > 0 ? -vlength / hlength_abs : vlength / hlength_abs;
                    step = hlength_abs;
                }
                else    //纵向位移较大
                {
                    hoffset = hlength > 0 ? -hlength / vlength_abs : hlength / vlength_abs;
                    voffset = vlength > 0 ? -1 : 1;
                    step = vlength_abs;
                }

                //System.Diagnostics.Debug.WriteLine($"x1：{x1},y1：{y1}, x2：{x2}, y2：{y2}，step：{step}");
                for (; step > 0; drawX = (int)(drawX + hoffset), drawY = (int)(drawY + voffset), step--)
                {
                    index = (Width * drawY + drawX) * 4;
                    //System.Diagnostics.Debug.WriteLine($"step：{step}，x:{drawX},y:{drawY},index:{index}");
                    if (index < 0)
                    {
                        continue;
                    }
                    else if (index > Data.Length)
                    {
                        continue;
                    }
                    Data[index] = b;
                    Data[index + 1] = g;
                    Data[index + 2] = r;
                    Data[index + 3] = a;
                }
            }
            else
            {
                //TODO 可能需要处理两个边界值问题。
                if (x1 == x2)
                {
                    var drawY = Math.Min(y1, y2);
                    var drawEndY = Math.Max(y1, y2);
                    for (; drawY < drawEndY; drawY++)
                    {
                        index = (drawY * Width + x1) * 4;
                        Data[index] = b;
                        Data[index + 1] = g;
                        Data[index + 2] = r;
                        Data[index + 3] = a;
                    }
                }
                else if (y1 == y2)
                {
                    var drawX = Math.Min(x1, x2);
                    var drawEndX = Math.Max(x1, x2);
                    for (; drawX < drawEndX; drawX++)
                    {
                        index = (y1 * Width + drawX) * 4;
                        Data[index] = b;
                        Data[index + 1] = g;
                        Data[index + 2] = r;
                        Data[index + 3] = a;
                    }
                }
            }
        }

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        /// <param name="width">矩形的宽度</param>
        /// <param name="height">矩形的高度</param>
        public override void DrawRectangle(int color, int left, int top, int width, int height)
        {
            DrawLine(color, left, top, left + width, top);
            DrawLine(color, left, top, left, top + height);
            DrawLine(color, left + width, top, left + width, top + height);
            DrawLine(color, left, top + height, left + width, top + height);
        }

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        /// <param name="width">填充的宽度</param>
        /// <param name="height">填充的高度</param>
        public override void FillRectangle(int color, int left, int top, int width, int height)
        {
            ImageBuilderUtil.ColorToArgb(color, out var a, out var r, out var g, out var b);

            for (int v = 0; v < height; v++)    //控制行
            {
                for (int h = 0; h < width; h++)    //控制当前行的位置
                {
                    var tempX = left + h;
                    var tempY = top + v;
                    if (tempX < 0 || tempX > Width || tempY < 0 || tempY > Height)
                    {
                        continue;
                    }
                    var index = (Width * (v + top) + (h + left)) * 4;
                    Data[index] = b;
                    Data[index + 1] = g;
                    Data[index + 2] = r;
                    Data[index + 3] = a;
                }
            }
        }

        #endregion 绘制
    }
}