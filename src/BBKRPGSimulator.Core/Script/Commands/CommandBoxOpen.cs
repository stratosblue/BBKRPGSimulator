using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 箱子打开命令
    /// </summary>
    internal class CommandBoxOpen : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 箱子打开命令
        /// </summary>
        /// <param name="context"></param>
        public CommandBoxOpen(ArraySegment<byte> data, SimulatorContext context) : base(data, 2, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var id = Data.Get2BytesUInt(0);
            var box = Context.SceneMap.SceneNPCs[id];
            if (box != null)
            {
                box.SetStep(1);
            }
            return null;
        }

        #endregion 方法
    }
}