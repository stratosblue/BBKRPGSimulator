using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 跳转命令
    /// </summary>
    internal class CommandGoto : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 跳转命令
        /// </summary>
        /// <param name="context"></param>
        public CommandGoto(ArraySegment<byte> data, SimulatorContext context) : base(data, 2, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var address = Data.Get2BytesUInt(0);
            Context.ScriptProcess.GotoAddress(address);
            return null;
        }

        #endregion 方法
    }
}