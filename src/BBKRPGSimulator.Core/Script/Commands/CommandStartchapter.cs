using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 开始章节命令
    /// </summary>
    internal class CommandStartChapter : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 开始章节命令
        /// </summary>
        /// <param name="context"></param>
        public CommandStartChapter(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var type = Data.Get2BytesUInt(0);
            var index = Data.Get2BytesUInt(2);

            Context.ScriptProcess.StartChapter(type, index);

            return null;
        }

        #endregion 方法
    }
}