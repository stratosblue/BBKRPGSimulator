using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置事件命令
    /// </summary>
    internal class CommandSetEvent : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置事件命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSetEvent(ArraySegment<byte> data, SimulatorContext context) : base(data, 2, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var _eventId = Data.Get2BytesUInt(0);
            Context.ScriptProcess.ScriptState.SetEvent(_eventId);
            return null;
        }

        #endregion 方法
    }
}