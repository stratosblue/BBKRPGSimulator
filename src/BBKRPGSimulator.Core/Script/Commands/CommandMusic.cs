namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 音乐命令
    /// </summary>
    internal class CommandMusic : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 音乐命令
        /// </summary>
        /// <param name="context"></param>
        public CommandMusic(SimulatorContext context) : base(4, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => null;

        #endregion 方法
    }
}