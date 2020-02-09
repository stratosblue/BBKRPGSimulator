using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 使用指定数量的物品命令
    /// </summary>
    internal class CommandUseGoodsNum : BaseCommand
    {
        #region 字段

        private readonly int _type, _index, _num, _address;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 使用指定数量的物品命令
        /// </summary>
        /// <param name="context"></param>
        public CommandUseGoodsNum(ArraySegment<byte> data, SimulatorContext context) : base(data, 8, context)
        {
            _type = data.Get2BytesUInt(0);
            _index = data.Get2BytesUInt(2);
            _num = data.Get2BytesUInt(4);
            _address = data.Get2BytesUInt(6);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            bool success = Context.GoodsManage.DropGoods(_type, _index, _num);
            if (!success)
            {
                Context.ScriptProcess.GotoAddress(_address);
            }
            return null;
        }

        #endregion 方法
    }
}