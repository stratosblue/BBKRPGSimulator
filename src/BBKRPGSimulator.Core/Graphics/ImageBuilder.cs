namespace BBKRPGSimulator
{
    /// <summary>
    /// 图像构造器
    /// </summary>
    public abstract class ImageBuilder
    {
        #region 字段

        /// <summary>
        /// 图像高度
        /// </summary>
        private int _height;

        /// <summary>
        /// 图像宽度
        /// </summary>
        private int _width;

        #endregion 字段

        #region 属性

        /// <summary>
        /// BitMap数据
        /// </summary>
        public byte[] Data { get; protected set; }

        /// <summary>
        /// 图像高度
        /// </summary>
        public int Height { get => GetHeight(); protected set => _height = value; }

        /// <summary>
        /// 图像宽度
        /// </summary>
        public int Width { get => GetWidth(); protected set => _width = value; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 图像构造器
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ImageBuilder(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 图像构造器
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public ImageBuilder(int[] data, int width, int height)
        {
            Width = width;
            Height = height;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 将另一个图片绘制到当前图片的指定位置
        /// </summary>
        /// <param name="imageBuilder">另一个图片的构建器</param>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        public abstract void Draw(ImageBuilder imageBuilder, int left, int top);

        /// <summary>
        /// 绘制线段
        /// </summary>
        /// <param name="color"></param>
        /// <param name="x1">起始x坐标</param>
        /// <param name="y1">起始y坐标</param>
        /// <param name="x2">终止x坐标</param>
        /// <param name="y2">终止y坐标</param>
        public abstract void DrawLine(int color, int x1, int y1, int x2, int y2);

        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        /// <param name="width">矩形的宽度</param>
        /// <param name="height">矩形的高度</param>
        public abstract void DrawRectangle(int color, int left, int top, int width, int height);

        /// <summary>
        /// 填充矩形
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="left">左坐标</param>
        /// <param name="top">上坐标</param>
        /// <param name="width">填充的宽度</param>
        /// <param name="height">填充的高度</param>
        public abstract void FillRectangle(int color, int left, int top, int width, int height);

        /// <summary>
        /// 获取高度
        /// </summary>
        /// <returns></returns>
        public virtual int GetHeight()
        {
            return _height;
        }

        /// <summary>
        /// 获取宽度
        /// </summary>
        /// <returns></returns>
        public virtual int GetWidth()
        {
            return _width;
        }

        /// <summary>
        /// 设置RGB数据
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="imageBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="stride"></param>
        public abstract void SetPixels(int left, int top, int width, int height, int[] pixels, int offset, int stride);

        #region 工具方法

        /// <summary>
        /// 获取ARGB
        /// </summary>
        /// <param name="color"></param>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        protected static void ColorToArgb(int color, out byte a, out byte r, out byte g, out byte b)
        {
            var bytes = PixelToByte(color);
            b = bytes[0];
            g = bytes[1];
            r = bytes[2];
            a = bytes[3];
        }

        /// <summary>
        /// 像素集合转换为ARGB图像buffer
        /// 注意：顺序为BGRA
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        protected static byte[] PixelsToBuffer(int[] pixels)
        {
            byte[] result = new byte[pixels.Length * 4];
            for (int i = 0, index = 0; i < pixels.Length && index < result.Length; i++, index += 4)
            {
                result[index] = (byte)(pixels[i] & 0x000000FF); //B
                result[index + 1] = (byte)((pixels[i] & 0x0000FF00) >> 8);  //G
                result[index + 2] = (byte)((pixels[i] & 0x00FF0000) >> 16); //R
                result[index + 3] = (byte)(pixels[i] >> 24);    //A
            }
            return result;
        }

        /// <summary>
        /// 像素转换为ARGB数组
        /// 注意：顺序为BGRA
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        protected static byte[] PixelToByte(int pixel)
        {
            byte[] result = new byte[4];

            result[0] = (byte)(pixel & 0x000000FF); //B
            result[1] = (byte)((pixel & 0x0000FF00) >> 8);  //G
            result[2] = (byte)((pixel & 0x00FF0000) >> 16); //R
            result[3] = (byte)(pixel >> 24);    //A

            return result;
        }

        /// <summary>
        /// 判断区域值
        /// </summary>
        /// <param name="inValue"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        protected int RangeValue(int inValue, int minValue, int maxValue)
        {
            if (inValue > maxValue)
            {
                return maxValue;
            }
            if (inValue < minValue)
            {
                return minValue;
            }
            return inValue;
        }

        #endregion 工具方法

        #endregion 方法
    }
}