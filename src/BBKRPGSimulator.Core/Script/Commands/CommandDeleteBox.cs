using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 删除箱子命令
    /// </summary>
    internal class CommandDeleteBox : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 删除箱子命令
        /// </summary>
        /// <param name="context"></param>
        public CommandDeleteBox(ArraySegment<byte> data, SimulatorContext context) : base(data, 2, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var _id = Data.Get2BytesUInt(0);
            Context.SceneMap.DeleteBox(_id);
            return null;
        }

        #endregion 方法
    }
}