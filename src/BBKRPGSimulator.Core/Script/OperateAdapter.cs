using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 命令的操作适配器
    /// </summary>
    internal abstract class OperateAdapter : Operate
    {
        #region 构造函数

        /// <summary>
        /// 命令的操作适配器
        /// </summary>
        /// <param name="context"></param>
        public OperateAdapter(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
        }

        public override void OnKeyDown(int key)
        {
        }

        public override void OnKeyUp(int key)
        {
        }

        public override bool Update(long delta)
        {
            return false;
        }

        #endregion 方法
    }
}