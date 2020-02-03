namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 属性测试命令
    /// </summary>
    internal class CommandAttributeTest : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 属性测试命令
        /// </summary>
        /// <param name="context"></param>
        public CommandAttributeTest(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 10;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return null;
        }

        #endregion 方法
    }
}