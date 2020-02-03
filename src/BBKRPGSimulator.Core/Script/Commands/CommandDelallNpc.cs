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
        public CommandDelAllNpc(SimulatorContext context) : base(context)
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
            return new CommandDelAllNpcOperate(Context);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 删除所有NPC命令的操作
        /// </summary>
        private class CommandDelAllNpcOperate : OperateAdapter
        {
            #region 构造函数

            public CommandDelAllNpcOperate(SimulatorContext context) : base(context)
            {
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.SceneMap.DeleteAllNpc();
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}