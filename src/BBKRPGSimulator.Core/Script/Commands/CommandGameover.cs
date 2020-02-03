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
        public CommandGameover(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandGameoverOperate(Context);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 游戏结束命令操作
        /// </summary>
        private class CommandGameoverOperate : OperateAdapter
        {
            #region 构造函数

            /// <summary>
            /// 游戏结束命令操作
            /// </summary>
            /// <param name="context"></param>
            public CommandGameoverOperate(SimulatorContext context) : base(context)
            {
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.ChangeScreen(ScreenEnum.SCREEN_MENU);
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}