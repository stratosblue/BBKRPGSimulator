using BBKRPGSimulator.Definitions;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 游戏结束命令
    /// </summary>
    internal class CommandGameover : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 游戏结束命令
        /// </summary>
        /// <param name="context"></param>
        public CommandGameover(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.ChangeScreen(ScreenEnum.SCREEN_MENU);
            return null;
        }

        #endregion 方法
    }
}