using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 清除事件命令
    /// </summary>
    internal class CommandClearEvent : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 清除事件命令
        /// </summary>
        /// <param name="context"></param>
        public CommandClearEvent(ArraySegment<byte> data, SimulatorContext context) : base(data, 2, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var eventId = Data.Get2BytesUInt(0);
            Context.ScriptProcess.ScriptState.ClearEvent(eventId);
            return null;
        }

        #endregion 方法
    }
}