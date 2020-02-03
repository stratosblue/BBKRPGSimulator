using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 定时消息？
    /// </summary>
    internal class CommandTimeMsg : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 定时消息？
        /// </summary>
        /// <param name="context"></param>
        public CommandTimeMsg(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            int i = 2;
            while (code[start + i] != 0)
            {
                ++i;
            }
            return start + i + 1;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}