namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 音乐命令
    /// </summary>
    internal class CommandMusic : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 音乐命令
        /// </summary>
        /// <param name="context"></param>
        public CommandMusic(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 4;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return null;
        }

        #endregion 方法
    }
}