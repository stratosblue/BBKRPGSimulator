using System;

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
            //TODO 完成返回 参照 cmd_return
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}