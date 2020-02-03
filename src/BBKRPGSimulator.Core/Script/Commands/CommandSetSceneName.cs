namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置场景名称命令
    /// </summary>
    internal class CommandSetSceneName : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 设置场景名称命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSetSceneName(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            int i = 0;
            while (code[start + i] != 0)
            {
                ++i;
            }
            return start + i + 1;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandSetSceneNameOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 设置场景名称命令的操作
        /// </summary>
        private class CommandSetSceneNameOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 设置场景名称命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandSetSceneNameOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                Context.SceneMap.SceneName = _code.GetString(_start);
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}