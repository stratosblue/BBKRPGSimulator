namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 初始化战斗命令
    /// </summary>
    internal class CommandInitFight : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 初始化战斗命令
        /// </summary>
        /// <param name="context"></param>
        public CommandInitFight(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 22;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandInitFightOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 初始化战斗命令的操作
        /// </summary>
        private class CommandInitFightOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 初始化战斗命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandInitFightOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                int[] monsterType = new int[8];
                for (int i = 0; i < 8; i++)
                {
                    monsterType[i] = _code.Get2BytesUInt(_start + i * 2);
                }
                Context.CombatManage.InitRandomCombat(monsterType, _code.Get2BytesUInt(_start + 16), _code.Get2BytesUInt(_start + 18), _code.Get2BytesUInt(_start + 20));
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}