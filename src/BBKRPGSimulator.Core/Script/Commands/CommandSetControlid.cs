using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置控制ID？？
    /// </summary>
    internal class CommandSetControlId : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置控制ID？？
        /// </summary>
        /// <param name="context"></param>
        public CommandSetControlId(ArraySegment<byte> data, SimulatorContext context) : base(data, 2, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var _id = Data.Get2BytesUInt(0);
            //TODO 完成它
            throw new NotImplementedException();
            //game.mainScene.setControlPlayer(id);
            return null;
        }

        #endregion 方法
    }
}