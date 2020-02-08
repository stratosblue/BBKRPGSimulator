using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// IF命令
    /// </summary>
    internal class CommandIf : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// IF命令
        /// </summary>
        /// <param name="context"></param>
        public CommandIf(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var eventId = Data.Get2BytesUInt(0);
            var address = Data.Get2BytesUInt(2);

            if (Context.ScriptProcess.ScriptState.GlobalEvents[eventId])
            {
                Context.ScriptProcess.GotoAddress(address);
            }
            return null;
        }

        #endregion 方法
    }
}