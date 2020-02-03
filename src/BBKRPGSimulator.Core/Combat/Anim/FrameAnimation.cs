using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Combat.Anim
{
    /// <summary>
    /// 帧动画
    /// </summary>
    internal class FrameAnimation
    {
        #region 字段

        private int DELTA = 1000 / 5;

        private int mCurFrame;
        private int mEndFrame;
        private ResImage mImage;

        private int mStartFrame;
        private long mTimeCnt = 0;

        #endregion 字段

        #region 构造函数

        public FrameAnimation(ResImage img) : this(img, 1, img.Number)
        {
        }

        public FrameAnimation(ResImage img, int startFrame, int endFrame)
        {
            mImage = img;
            mStartFrame = startFrame;
            mEndFrame = endFrame;
            mCurFrame = startFrame;
        }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas, int x, int y)
        {
            mImage.Draw(canvas, mCurFrame, x, y);
        }

        public void SetFPS(int fps)
        {
            DELTA = 1000 / fps;
        }

        public void Update(long delta)
        {
            mTimeCnt += delta;
            if (mTimeCnt >= DELTA)
            {
                mTimeCnt = 0;

                if (++mCurFrame > mEndFrame)
                {
                    mCurFrame = mStartFrame;
                }
            }
        }

        #endregion 方法
    }
}