using System.Diagnostics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 返回命令
    /// </summary>
    internal class CommandReturn : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 返回命令
        /// </summary>
        /// <param name="context"></param>
        public CommandReturn(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            //HACK 测试是否正确
            Debug.WriteLine("确认 CommandReturn 是否正常工作");
            Context.ReturnChapter();

            return null;
        }

        #endregion 方法
    }
}