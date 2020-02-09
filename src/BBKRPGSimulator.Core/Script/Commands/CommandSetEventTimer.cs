using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置事件Timer命令
    /// </summary>
    internal class CommandSetEventTimer : BaseCommand
    {
        #region 字段

        private readonly int _timer, _eventId;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 设置事件Timer命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSetEventTimer(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        {
            _timer = data.Get2BytesUInt(0);
            _eventId = data.Get2BytesUInt(2);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.ScriptProcess.ScriptExecutor.SetTimer(_timer, _eventId);
            return null;
        }

        #endregion 方法
    }
}