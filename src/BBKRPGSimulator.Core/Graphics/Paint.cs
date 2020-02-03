namespace BBKRPGSimulator.Graphics
{
    /// <summary>
    /// 画笔信息
    /// </summary>
    public class Paint
    {
        #region 属性

        /// <summary>
        /// 颜色
        /// </summary>
        public int Color { get; private set; }

        /// <summary>
        /// 轮廓宽度
        /// </summary>
        public int StrokeWidth { get; set; } = 0;

        /// <summary>
        /// 绘制风格
        /// </summary>
        public PaintStyle Style { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 画笔信息
        /// </summary>
        public Paint()
        {
        }

        /// <summary>
        /// 画笔信息
        /// </summary>
        /// <param name="style"></param>
        public Paint(PaintStyle style)
        {
            Style = style;
        }

        /// <summary>
        /// 画笔信息
        /// </summary>
        /// <param name="colorValue"></param>
        public Paint(int colorValue)
        {
            Color = colorValue;
        }

        /// <summary>
        /// 画笔信息
        /// </summary>
        /// <param name="style"></param>
        /// <param name="colorValue"></param>
        public Paint(PaintStyle style, int colorValue)
        {
            Style = style;
            Color = colorValue;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="colorValue"></param>
        public void SetColor(int colorValue)
        {
            Color = colorValue;
        }

        /// <summary>
        /// 设置轮廓宽度
        /// </summary>
        /// <param name="width"></param>
        public void SetStrokeWidth(int width)
        {
            StrokeWidth = width;
        }

        /// <summary>
        /// 设置风格
        /// </summary>
        /// <param name="style"></param>
        public void SetStyle(PaintStyle style)
        {
            Style = style;
        }

        #endregion 方法
    }
}