using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 获取金钱命令
    /// </summary>
    internal class CommandGainMoney : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 获取金钱命令
        /// </summary>
        /// <param name="context"></param>
        public CommandGainMoney(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var money = Data.Array.Get4BytesInt(Data.Offset);
            Context.PlayContext.Money += money;
            return null;
        }

        #endregion 方法
    }
}