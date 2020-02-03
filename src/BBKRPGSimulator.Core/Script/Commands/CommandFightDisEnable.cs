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
        public CommandFightDisEnable(SimulatorContext context) : base(context)
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
            return new CommandFightDisEnableOperate(Context);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 禁用随机战斗命令的操作
        /// </summary>
        private class CommandFightDisEnableOperate : OperateAdapter
        {
            #region 构造函数

            public CommandFightDisEnableOperate(SimulatorContext context) : base(context)
            {
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.CombatManage.DisableCombat();
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}