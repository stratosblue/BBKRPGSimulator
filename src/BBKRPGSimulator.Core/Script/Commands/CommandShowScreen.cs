namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 显示界面命令？
    /// </summary>
    internal class CommandShowScreen : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 显示界面命令？
        /// </summary>
        /// <param name="context"></param>
        public CommandShowScreen(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandShowScreenOperate(Context);

        #endregion 方法

        #region 类

        public class CommandShowScreenOperate : OperateDrawScene
        {
            #region 构造函数

            public CommandShowScreenOperate(SimulatorContext context) : base(context)
            {
            }

            #endregion 构造函数
        }

        #endregion 类
    }
}