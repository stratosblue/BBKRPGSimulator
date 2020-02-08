using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 绘制一次的操作？
    /// </summary>
    internal abstract class OperateDrawScene : Operate
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
        public OperateDrawScene(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            DrawScene(canvas);
            ++_drawCount;
        }

        public virtual void DrawScene(ICanvas canvas)
        {
            Context.SceneMap.DrawScene(canvas);
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