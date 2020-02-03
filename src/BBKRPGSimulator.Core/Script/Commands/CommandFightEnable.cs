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
        public CommandFightEnable(SimulatorContext context) : base(context)
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
            return new CommandFightEnableOperate(Context);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 启用随机战斗命令的操作
        /// </summary>
        private class CommandFightEnableOperate : OperateAdapter
        {
            #region 构造函数

            public CommandFightEnableOperate(SimulatorContext context) : base(context)
            {
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.CombatManage.EnableCombat();
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}