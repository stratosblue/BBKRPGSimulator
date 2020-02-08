namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// CallBack？命令
    /// </summary>
    internal class CommandCallback : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// CallBack？命令
        /// </summary>
        /// <param name="context"></param>
        public CommandCallback(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.ScriptProcess.ExitScript();

            //TODO 此处需要确认是否正常运行
            //if (ScriptResources.globalEvents[ScriptProcess.get2ByteInt(_code, _start)])
            //{
            //    ScriptProcess.MScreenMainGame.gotoAddress(ScriptProcess.get2ByteInt(_code, _start + 2));
            //}
            return null;
        }

        #endregion 方法
    }
}