namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 删除所有NPC命令
    /// </summary>
    internal class CommandDelAllNpc : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 删除所有NPC命令
        /// </summary>
        /// <param name="context"></param>
        public CommandDelAllNpc(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.SceneMap.DeleteAllNpc();
            return null;
        }

        #endregion 方法
    }
}