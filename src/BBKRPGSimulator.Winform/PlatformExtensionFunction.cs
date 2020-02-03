using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace BBKRPGSimulator.Winform
{
    public static class PlatformExtensionFunction
    {
        #region 方法

        [DebuggerStepThrough]
        public static Image GetImageFromBuffer(ImageBuilder bitmapDataBuilder)
        {
            Bitmap result = new Bitmap(bitmapDataBuilder.Width, bitmapDataBuilder.Height, PixelFormat.Format32bppArgb);
            //锁定内存数据
            BitmapData data = result.LockBits(
                new Rectangle(0, 0, result.Width, result.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);
            //输入颜色数据
            System.Runtime.InteropServices.Marshal.Copy(bitmapDataBuilder.Data, 0, data.Scan0, bitmapDataBuilder.Data.Length);
            result.UnlockBits(data);//解锁

            return result;
        }

        [DebuggerStepThrough]
        public static Image GetImageFromBufferP(ImageBuilder bitmapDataBuilder)
        {
            int i = 0;
            Bitmap result = new Bitmap(bitmapDataBuilder.Width, bitmapDataBuilder.Height, PixelFormat.Format32bppArgb);

            for (int v = 0; v < bitmapDataBuilder.Height; v++)    //控制行
            {
                for (int h = 0; h < bitmapDataBuilder.Width; h++)    //控制当前行的位置
                {
                    if (i >= bitmapDataBuilder.Data.Length)
                    {
                        break;
                    }
                    int a = bitmapDataBuilder.Data[i + 3];
                    int r = bitmapDataBuilder.Data[i + 2];
                    int g = bitmapDataBuilder.Data[i + 1];
                    int b = bitmapDataBuilder.Data[i];
                    Color test = Color.FromArgb(a, r, g, b);
                    result.SetPixel(h, v, test);
                    i += 4;
                }
            }
            return result;
        }

        #endregion 方法
    }
}