using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 随机事件命令
    /// </summary>
    internal class CommandRandRade : BaseCommand
    {
        #region 字段

        private readonly int _value, _address;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 随机事件命令
        /// </summary>
        /// <param name="context"></param>
        public CommandRandRade(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        {
            _value = data.Get2BytesUInt(0);
            _address = data.Get2BytesUInt(2);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            if ((Context.Random.Next() % 1000) <= _value)
            {
                Context.ScriptProcess.GotoAddress(_address);
            }
            return null;
        }

        #endregion 方法
    }
}