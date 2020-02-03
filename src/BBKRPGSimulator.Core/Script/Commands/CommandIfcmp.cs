namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// Ifcmp命令？？
    /// </summary>
    internal class CommandIfcmp : BaseCommand
    {
        #region 构造函数

        /// <summary>
        ///  Ifcmp命令？？
        /// </summary>
        /// <param name="context"></param>
        public CommandIfcmp(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 6;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandIfcmpOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// Ifcmp命令的操作？？
        /// </summary>
        private class CommandIfcmpOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// Ifcmp命令的操作？？
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandIfcmpOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                if (Context.ScriptProcess.ScriptState.Variables[_code.Get2BytesUInt(_start)] == _code.Get2BytesUInt(_start + 2))
                {
                    Context.ScriptProcess.GotoAddress(_code.Get2BytesUInt(_start + 4));
                }
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}