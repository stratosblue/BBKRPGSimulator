namespace BBKRPGSimulator
{
    /// <summary>
    /// 依赖上下文的对象
    /// </summary>
    internal abstract class ContextDependent
    {
        #region 属性

        /// <summary>
        /// 上下文对象
        /// </summary>
        protected SimulatorContext Context { get; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 依赖上下文的对象
        /// </summary>
        /// <param name="context"></param>
        public ContextDependent(SimulatorContext context)
        {
            Context = context;
        }

        #endregion 构造函数
    }
}