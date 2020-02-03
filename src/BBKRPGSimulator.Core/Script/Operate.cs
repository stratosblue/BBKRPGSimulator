using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 操作
    /// </summary>
    internal abstract class Operate : ContextDependent
    {
        #region 构造函数

        public Operate(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 绘制到指定画板
        /// </summary>
        /// <param name="canvas"></param>
        public abstract void Draw(ICanvas canvas);

        /// <summary>
        /// 是否全屏
        /// </summary>
        /// <returns></returns>
        public bool IsPopup()
        {
            return false;
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="key"></param>
        public abstract void OnKeyDown(int key);

        /// <summary>
        /// 按键松开
        /// </summary>
        /// <param name="key"></param>
        public abstract void OnKeyUp(int key);

        /// <summary>
        /// 处理一条指令
        /// </summary>
        /// <returns>true继续执行Update&Draw;false指令执行完毕</returns>
        public abstract bool Process();

        /// <summary>
        /// 更新
        /// 返回false退出当前操作
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public abstract bool Update(long delta);

        #endregion 方法
    }
}