using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 菜单命令
    /// </summary>
    internal class CommandMenu : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 菜单命令
        /// </summary>
        /// <param name="context"></param>
        public CommandMenu(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            throw new NotImplementedException();
            //int i = 2;
            //while (code[start + i] != 0)
            //{
            //    ++i;
            //}
            //return start + i + 1;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}