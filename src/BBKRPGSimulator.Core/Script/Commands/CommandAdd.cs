using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 加命令
    /// </summary>
    internal class CommandAdd : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 加命令
        /// </summary>
        /// <param name="context"></param>
        public CommandAdd(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var varIndex = Data.Get2BytesUInt(0);
            var tartgetValue = Data.Get2BytesUInt(2);
            Context.ScriptProcess.ScriptState.Variables[varIndex] += tartgetValue;

            return null;
        }

        #endregion 方法
    }
}