using System;

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
        public CommandNpcMoveMod(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var id = Data.Get2BytesUInt(0);
            var state = Data.Get2BytesUInt(2);
            var npc = Context.SceneMap.SceneNPCs[id];
            npc.State = (CharacterActionState)state;
            return null;
        }

        #endregion 方法
    }
}