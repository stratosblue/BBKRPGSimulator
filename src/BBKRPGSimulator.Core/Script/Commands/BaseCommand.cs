namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 命令基类
    /// </summary>
    internal abstract class BaseCommand : ContextDependent, ICommand
    {
        #region 构造函数

        /// <summary>
        /// 命令基类
        /// </summary>
        /// <param name="context"></param>
        public BaseCommand(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 获取下一个命令偏移
        /// </summary>
        /// <param name="code"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public abstract int GetNextPos(byte[] code, int start);

        /// <summary>
        /// 获取操作
        /// </summary>
        /// <param name="code"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public abstract Operate GetOperate(byte[] code, int start);

        #endregion 方法
    }
}