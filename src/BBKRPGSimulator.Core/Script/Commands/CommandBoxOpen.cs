using BBKRPGSimulator.Characters;

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
        public CommandBoxOpen(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 2;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandBoxOpenOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 箱子打开命令的操作
        /// </summary>
        private class CommandBoxOpenOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 箱子打开命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandBoxOpenOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                NPC box = Context.SceneMap.SceneNPCs[_code.Get2BytesUInt(_start)];
                if (box != null)
                {
                    box.SetStep(1);
                }
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}