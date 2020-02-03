using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 绘制一次的操作？
    /// </summary>
    internal abstract class OperateDrawOnce : Operate
    {
        #region 字段

        /// <summary>
        /// 绘制计数
        /// </summary>
        private int _drawCount = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 绘制一次的操作？
        /// </summary>
        /// <param name="context"></param>
        public OperateDrawOnce(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            DrawOnce(canvas);
            ++_drawCount;
        }

        public abstract void DrawOnce(ICanvas canvas);

        public override void OnKeyDown(int key)
        {
        }

        public override void OnKeyUp(int key)
        {
        }

        public override bool Update(long delta)
        {
            if (_drawCount >= 3)
            {
                _drawCount = 0;
                return false;
            }
            return true;
        }

        #endregion 方法
    }
}