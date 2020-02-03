namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// NPC移动命令
    /// </summary>
    internal class CommandNpcMoveMod : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// NPC移动命令
        /// </summary>
        /// <param name="context"></param>
        public CommandNpcMoveMod(SimulatorContext context) : base(context)
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
            return new CommandNpcMoveModOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// NPC移动命令的操作
        /// </summary>
        internal class CommandNpcMoveModOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// NPC移动命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandNpcMoveModOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                var npc = Context.SceneMap.SceneNPCs[_code.Get2BytesUInt(_start)];
                npc.State = (CharacterActionState)_code.Get2BytesUInt(_start + 2);
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}