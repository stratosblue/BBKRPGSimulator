using System.Drawing;

namespace BBKRPGSimulator.Graphics
{
    public interface ICanvas
    {
        #region 属性

        ImageBuilder Background { get; }

        #endregion 属性

        #region 方法

        void DrawBitmap(ImageBuilder bitmap, int left, int top);

        void DrawColor(int color);

        void DrawLine(int startX, int startY, int stopX, int stopY, Paint paint);

        void DrawLines(float[] pts, Paint paint);

        void DrawRect(int left, int top, int right, int bottom, Paint paint);

        void DrawRect(Rectangle rectangle, Paint pait);

        void Scale(float mScale, float mScale2);

        void SetBitmap(ImageBuilder bitmap);

        #endregion 方法
    }
}