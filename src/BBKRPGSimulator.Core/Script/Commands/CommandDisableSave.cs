using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 禁用存档命令
    /// </summary>
    internal class CommandDisableSave : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 禁用存档命令
        /// </summary>
        /// <param name="context"></param>
        public CommandDisableSave(SimulatorContext context) : base(context)
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