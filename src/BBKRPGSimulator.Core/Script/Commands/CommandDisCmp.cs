using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// ???
    /// </summary>
    internal class CommandDisCmp : BaseCommand
    {
        #region 字段

        private readonly int _varIndex, _value, _addr1, _addr2;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="context"></param>
        public CommandDisCmp(ArraySegment<byte> data, SimulatorContext context) : base(data, 8, context)
        {
            _varIndex = data.Get2BytesUInt(0);
            _value = data.Get2BytesUInt(2);
            _addr1 = data.Get2BytesUInt(4);
            _addr2 = data.Get2BytesUInt(6);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            int var = Context.ScriptProcess.ScriptState.Variables[_varIndex];
            if (var < _value)
            {
                Context.ScriptProcess.GotoAddress(_addr1);
            }
            else if (var > _value)
            {
                Context.ScriptProcess.GotoAddress(_addr2);
            }
            return null;
        }

        #endregion 方法
    }
}