namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 允许存档命令
    /// </summary>
    internal class CommandEnableSave : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 允许存档命令
        /// </summary>
        /// <param name="context"></param>
        public CommandEnableSave(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.PlayContext.DisableSave = false;
            return null;
        }

        #endregion 方法
    }
}