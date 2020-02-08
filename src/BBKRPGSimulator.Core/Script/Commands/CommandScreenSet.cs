using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置屏幕命令
    /// </summary>
    internal class CommandScreenSet : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置屏幕命令
        /// </summary>
        /// <param name="context"></param>
        public CommandScreenSet(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var x = Data.Get2BytesUInt(0);
            var y = Data.Get2BytesUInt(2);
            Context.SceneMap.SetMapScreenPos(x, y);

            return null;
        }

        #endregion 方法
    }
}