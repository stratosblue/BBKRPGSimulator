namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 禁用随机战斗命令
    /// </summary>
    partial class CommandFightDisEnable : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 禁用随机战斗命令
        /// </summary>
        /// <param name="context"></param>
        public CommandFightDisEnable(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.CombatManage.EnableRandomCombat = false;
            return null;
        }

        #endregion 方法
    }
}