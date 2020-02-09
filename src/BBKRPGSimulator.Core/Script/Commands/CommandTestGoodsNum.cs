using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 测试物品数量命令
    /// </summary>
    internal class CommandTestGoodsNum : BaseCommand
    {
        #region 字段

        private readonly int _type, _index, _num, _addr1, _addr2;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 测试物品数量命令
        /// </summary>
        /// <param name="context"></param>
        public CommandTestGoodsNum(ArraySegment<byte> data, SimulatorContext context) : base(data, 10, context)
        {
            _type = data.Get2BytesUInt(0);
            _index = data.Get2BytesUInt(2);
            _num = data.Get2BytesUInt(4);
            _addr1 = data.Get2BytesUInt(6);
            _addr2 = data.Get2BytesUInt(8);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            int goodsnum = Context.GoodsManage.GetGoodsNum(_type, _index);
            if (goodsnum == _num)
            {
                Context.ScriptProcess.GotoAddress(_addr1);
            }
            else if (goodsnum > _num)
            {
                Context.ScriptProcess.GotoAddress(_addr2);
            }
            return null;
        }

        #endregion 方法
    }
}