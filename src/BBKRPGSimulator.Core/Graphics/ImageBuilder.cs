namespace BBKRPGSimulator.Graphics
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
        public ImageBuilder(byte[] data, int width, int height)
        {
            Data = data;
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
        /// <param name="offset"></param>
        /// <param name="stride"></param>
        public abstract void SetPixels(int left, int top, int width, int height, byte[] pixels, int offset, int stride);

        #endregion 方法
    }
}