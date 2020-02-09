using System;
using System.Diagnostics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 调用章节命令
    /// </summary>
    internal class CommandCallChapter : BaseCommand
    {
        #region 字段

        private readonly int _type, _index;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 调用章节命令
        /// </summary>
        /// <param name="context"></param>
        public CommandCallChapter(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        {
            _type = data.Get2BytesUInt(0);
            _index = data.Get2BytesUInt(2);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            //HACK 测试是否正确
            Debug.WriteLine("确认 CommandCallChapter 是否正常工作");
            Context.CallChapter(_type, _index);
            return null;
        }

        #endregion 方法
    }
}