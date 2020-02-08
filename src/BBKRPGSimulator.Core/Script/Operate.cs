using System;
using System.Diagnostics;

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
            DebugLog();
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 绘制到指定画板
        /// </summary>
        /// <param name="canvas"></param>
        public virtual void Draw(ICanvas canvas) { }

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
        public virtual void OnKeyDown(int key) { }

        /// <summary>
        /// 按键松开
        /// </summary>
        /// <param name="key"></param>
        public virtual void OnKeyUp(int key) { }

        /// <summary>
        /// 更新
        /// 返回false退出当前操作
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public virtual bool Update(long delta) { return false; }

        [Conditional("DEBUG")]
        protected void DebugLog()
        {
            Debug.WriteLine($"{DateTime.Now}: Operate - {GetType().Name} - {ToString()}");
        }

        #endregion 方法
    }
}