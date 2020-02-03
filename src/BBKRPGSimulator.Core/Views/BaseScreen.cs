using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View
{
    /// <summary>
    /// 基础Screen
    /// </summary>
    internal abstract class BaseScreen : ContextDependent
    {
        #region 属性

        /// <summary>
        /// 文本呈现器
        /// </summary>
        public TextRender TextRender => Context.TextRender;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 基础Screen
        /// </summary>
        /// <param name="context"></param>
        public BaseScreen(SimulatorContext context) : base(context)
        { }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 将当前屏幕绘制到指定canvas上
        /// </summary>
        /// <param name="canvas"></param>
        public abstract void Draw(ICanvas canvas);

        /// <summary>
        /// 是否弹出
        /// </summary>
        /// <returns></returns>
        public virtual bool IsPopup()
        {
            return false;
        }

        /// <summary>
        /// 按下键
        /// </summary>
        /// <param name="key"></param>
        public abstract void OnKeyDown(int key);

        /// <summary>
        /// 放开键
        /// </summary>
        /// <param name="key"></param>
        public abstract void OnKeyUp(int key);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="delta"></param>
        public abstract void Update(long delta);

        #endregion 方法
    }
}