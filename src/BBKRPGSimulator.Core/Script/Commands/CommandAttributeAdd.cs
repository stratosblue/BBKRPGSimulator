namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 属性加命令
    /// </summary>
    internal class CommandAttributeAdd : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 属性加命令
        /// </summary>
        /// <param name="context"></param>
        public CommandAttributeAdd(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 6;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return null;
        }

        #endregion 方法
    }
}