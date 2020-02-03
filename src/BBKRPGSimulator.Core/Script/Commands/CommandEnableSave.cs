using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 允许存档命令
    /// </summary>
    internal class CommandEnableSave : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 允许存档命令
        /// </summary>
        /// <param name="context"></param>
        public CommandEnableSave(SimulatorContext context) : base(context)
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