using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 创建NPC命令
    /// </summary>
    internal class CommandCreateNpc : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 创建NPC命令
        /// </summary>
        /// <param name="context"></param>
        public CommandCreateNpc(ArraySegment<byte> data, SimulatorContext context) : base(data, 8, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var id = Data.Get2BytesUInt(0);
            var npcIndex = Data.Get2BytesUInt(2);
            var x = Data.Get2BytesUInt(4);
            var y = Data.Get2BytesUInt(6);
            Context.SceneMap.CreateNpc(id, npcIndex, x, y);
            return null;
        }

        #endregion 方法
    }
}