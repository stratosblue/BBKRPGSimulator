namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 启用随机战斗命令
    /// </summary>
    internal class CommandFightEnable : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 启用随机战斗命令
        /// </summary>
        /// <param name="context"></param>
        public CommandFightEnable(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.CombatManage.EnableRandomCombat = true;
            return null;
        }

        #endregion 方法
    }
}