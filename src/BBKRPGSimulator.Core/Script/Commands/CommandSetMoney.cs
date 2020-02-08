using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置金钱命令
    /// </summary>
    internal class CommandSetMoney : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置金钱命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSetMoney(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var _money = Data.Array.Get4BytesInt(Data.Offset);
            Context.PlayContext.Money = _money;
            return null;
        }

        #endregion 方法
    }
}