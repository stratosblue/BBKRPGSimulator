using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置控制ID？？
    /// 伏魔记未用到
    /// </summary>
    internal class CommandSetControlId : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置控制ID？？
        /// 伏魔记未用到
        /// </summary>
        /// <param name="context"></param>
        public CommandSetControlId(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 2;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}