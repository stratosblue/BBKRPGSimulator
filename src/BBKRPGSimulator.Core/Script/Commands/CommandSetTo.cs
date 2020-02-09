using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// SetTo命令？
    /// </summary>
    internal class CommandSetTo : BaseCommand
    {
        #region 字段

        private readonly int _varIndex1, _varIndex2;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// SetTo命令?
        /// </summary>
        /// <param name="context"></param>
        public CommandSetTo(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        {
            _varIndex1 = data.Get2BytesUInt(2);
            _varIndex2 = data.Get2BytesUInt(0);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.ScriptProcess.ScriptState.Variables[_varIndex1] = Context.ScriptProcess.ScriptState.Variables[_varIndex2];
            return null;
        }

        #endregion 方法
    }
}