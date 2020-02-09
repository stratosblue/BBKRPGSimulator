using System.Drawing;
using System.Drawing.Imaging;

using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Winform
{
    /// <summary>
    /// 新的图像构建器
    /// </summary>
    internal class NewImageBuilder : ImageBuilder
    {
        #region 字段

        /// <summary>
        /// 默认图像格式
        /// </summary>
        private const PixelFormat DEFAULT_PIXEL_FORMAT = PixelFormat.Format32bppArgb;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 图像实例
        /// </summary>
        public Bitmap Instance { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 新的图像构建器
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public NewImageBuilder(int width, int height) : base(width, height)
        {
            Instance = new Bitmap(width, height, DEFAULT_PIXEL_FORMAT);
        }

        /// <summary>
        /// 新的图像构建器
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public NewImageBuilder(byte[] data, int width, int height) : base(data, width, height)
        {
            Instance = new Bitmap(width, height, DEFAULT_PIXEL_FORMAT);
            SetPixels(0, 0, width, height, data, 0, 0);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ImageBuilder imageBuilder, int left, int top)
        {
            using (var g = GetGraphics())
            {
                g.DrawImage((imageBuilder as NewImageBuilder).Instance, left, top);
            }
        }

        public override void DrawLine(int color, int x1, int y1, int x2, int y2)
        {
            using (var g = GetGraphics())
            {
                g.DrawLine(new Pen(Color.FromArgb(color)), x1, y1, x2, y2);
            }
        }

        public override void DrawRectangle(int color, int left, int top, int width, int height)
        {
            using (var g = GetGraphics())
            {
                g.DrawRectangle(new Pen(Color.FromArgb(color)), left, top, width, height);
            }
        }

        public override void FillRectangle(int color, int left, int top, int width, int height)
        {
            using (var g = GetGraphics())
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(color)), left, top, width, height);
            }
        }

        public override int GetHeight()
        {
            return Instance == null ? 0 : Instance.Height;
        }

        public override int GetWidth()
        {
            return Instance == null ? 0 : Instance.Width;
        }

        public override void SetPixels(int left, int top, int width, int height, byte[] pixels, int offset, int stride)
        {
            var buffer = pixels;
            //锁定内存数据
            BitmapData data = Instance.LockBits(
                new Rectangle(left, top, width, height),
                ImageLockMode.WriteOnly,
                DEFAULT_PIXEL_FORMAT);
            //输入颜色数据
            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
            Instance.UnlockBits(data);//解锁
        }

        private System.Drawing.Graphics GetGraphics()
        {
            return System.Drawing.Graphics.FromImage(Instance);
        }

        #endregion 方法
    }
}