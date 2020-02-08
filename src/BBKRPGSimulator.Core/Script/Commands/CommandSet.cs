using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// Set命令
    /// </summary>
    internal class CommandSet : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// Set命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSet(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var varIndex = Data.Get2BytesUInt(0);
            var targetValue = Data.Get2BytesUInt(2);
            Context.ScriptProcess.ScriptState.Variables[varIndex] = targetValue;
            return null;
        }

        #endregion 方法
    }
}