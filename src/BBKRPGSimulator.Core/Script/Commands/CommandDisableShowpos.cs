using System;
using System.Diagnostics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// ???
    /// </summary>
    internal class CommandDisableShowpos : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="context"></param>
        public CommandDisableShowpos(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Debug.WriteLine($"{nameof(CommandDisableShowpos)} - NotImplementedException return null;");
            return null;
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}