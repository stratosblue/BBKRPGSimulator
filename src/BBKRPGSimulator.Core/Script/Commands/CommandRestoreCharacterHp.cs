using System;

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
        public CommandRestoreCharacterHp(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var id = Data.Get2BytesUInt(0);
            var hp = Data.Get2BytesUInt(2);
            PlayerCharacter character = Context.PlayContext.GetPlayer(id);
            if (character != null)
            {
                character.HP = (int)(character.MaxHP * hp / 100.0);
            }
            return null;
        }

        #endregion 方法
    }
}