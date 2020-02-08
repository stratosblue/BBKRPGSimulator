using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// Ifcmp命令？？
    /// </summary>
    internal class CommandIfcmp : BaseCommand
    {
        #region 构造函数

        /// <summary>
        ///  Ifcmp命令？？
        /// </summary>
        /// <param name="context"></param>
        public CommandIfcmp(ArraySegment<byte> data, SimulatorContext context) : base(data, 6, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var varIndex = Data.Get2BytesUInt(0);
            var tartgetValue = Data.Get2BytesUInt(2);
            var address = Data.Get2BytesUInt(4);

            if (Context.ScriptProcess.ScriptState.Variables[varIndex] == tartgetValue)
            {
                Context.ScriptProcess.GotoAddress(address);
            }
            return null;
        }

        #endregion 方法
    }
}