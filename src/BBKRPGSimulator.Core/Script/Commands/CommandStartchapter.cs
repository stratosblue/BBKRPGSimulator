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
        public CommandStartChapter(SimulatorContext context) : base(context)
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
            return new CommandStartchapterOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 开始章节命令的操作
        /// </summary>
        private class CommandStartchapterOperate : OperateAdapter
        {
            #region 字段

            /// <summary>
            /// 章节的资源类型和索引
            /// </summary>
            private readonly int _type, _index;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 开始章节命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandStartchapterOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _type = ((int)code[start] & 0xFF) | ((int)code[start + 1] << 8 & 0xFF);
                _index = ((int)code[start + 2] & 0xFF) | ((int)code[start + 3] << 8 & 0xFF);
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.ScriptProcess.StartChapter(_type, _index);
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}