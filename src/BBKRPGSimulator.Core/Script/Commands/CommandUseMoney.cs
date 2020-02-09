using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 使用金钱命令
    /// </summary>
    internal class CommandUseMoney : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 使用金钱命令
        /// </summary>
        /// <param name="context"></param>
        public CommandUseMoney(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var money = Data.Get4BytesInt(0);
            Context.PlayContext.Money -= money;
            return null;
        }

        #endregion 方法
    }
}