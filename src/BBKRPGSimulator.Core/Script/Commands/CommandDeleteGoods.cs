using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 删除物品命令
    /// </summary>
    internal class CommandDeleteGoods : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 删除物品命令
        /// </summary>
        /// <param name="context"></param>
        public CommandDeleteGoods(ArraySegment<byte> data, SimulatorContext context) : base(data, 6, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var type = Data.Get2BytesUInt(0);
            var index = Data.Get2BytesUInt(2);
            var address = Data.Get2BytesUInt(2);
            bool success = Context.GoodsManage.DropGoods(type, index);
            if (!success)
            {
                Context.ScriptProcess.GotoAddress(address);
            }
            return null;
        }

        #endregion 方法
    }
}