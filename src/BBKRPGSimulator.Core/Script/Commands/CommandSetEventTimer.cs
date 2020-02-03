using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置事件Timer命令
    /// </summary>
    internal class CommandSetEventTimer : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置事件Timer命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSetEventTimer(SimulatorContext context) : base(context)
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
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}