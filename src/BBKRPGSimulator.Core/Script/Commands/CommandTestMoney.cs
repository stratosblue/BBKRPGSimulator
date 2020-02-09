using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 测试金钱命令
    /// </summary>
    internal class CommandTestMoney : BaseCommand
    {
        #region 字段

        private readonly int _value, _address;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 测试金钱命令
        /// </summary>
        /// <param name="context"></param>
        public CommandTestMoney(ArraySegment<byte> data, SimulatorContext context) : base(data, 6, context)
        {
            _value = data.Get4BytesInt(0);
            _address = data.Get2BytesUInt(4);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            if (Context.PlayContext.Money < _value)
            {
                Context.ScriptProcess.GotoAddress(_address);
            }
            return null;
        }

        #endregion 方法
    }
}