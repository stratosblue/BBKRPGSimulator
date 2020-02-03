using System;

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
        public CommandDisableShowpos(SimulatorContext context) : base(context)
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
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}