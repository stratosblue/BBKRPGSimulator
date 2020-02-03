using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 调用章节命令
    /// </summary>
    internal class CommandCallChapter : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 调用章节命令
        /// </summary>
        /// <param name="context"></param>
        public CommandCallChapter(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 4;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            throw new Exception("此处需要确认是否正常运行");
            //return null;
        }

        #endregion 方法
    }
}