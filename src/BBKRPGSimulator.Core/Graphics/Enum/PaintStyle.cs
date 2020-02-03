namespace BBKRPGSimulator.Graphics
{
    /// <summary>
    /// 绘制风格
    /// </summary>
    public enum PaintStyle
    {
        /// <summary>
        /// Geometry and text drawn with this style will be filled, ignoring all
        /// stroke-related settings in the paint.
        /// </summary>
        FILL = 0,

        /// <summary>
        /// Geometry and text drawn with this style will be stroked, respecting
        /// the stroke-related fields on the paint.
        /// </summary>
        STROKE = 1,

        /// <summary>
        /// Geometry and text drawn with this style will be both filled and
        /// stroked at the same time, respecting the stroke-related fields on
        /// the paint. This mode can give unexpected results if the geometry
        /// is oriented counter-clockwise. This restriction does not apply to
        /// either FILL or STROKE.
        /// </summary>
        FILL_AND_STROKE = 2,
    }
}