using BBKRPGSimulator.Characters;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 恢复角色HP命令
    /// </summary>
    internal class CommandRestoreCharacterHp : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 恢复角色HP命令
        /// </summary>
        /// <param name="context"></param>
        public CommandRestoreCharacterHp(SimulatorContext context) : base(context)
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
            return new CommandRestoreCharacterHpOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 恢复角色HP命令的操作
        /// </summary>
        private class CommandRestoreCharacterHpOperate : OperateAdapter
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 恢复角色HP命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandRestoreCharacterHpOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override bool Process()
            {
                PlayerCharacter character = Context.PlayContext.GetPlayer(_code.Get2BytesUInt(_start));
                if (character != null)
                {
                    character.HP = (int)(character.MaxHP * _code.Get2BytesUInt(_start + 2) / 100.0);
                }
                return false;
            }

            #endregion 方法
        }

        #endregion 类
    }
}