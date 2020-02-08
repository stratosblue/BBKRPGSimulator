using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 删除角色命令
    /// </summary>
    internal class CommandDeleteCharacter : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 删除角色命令
        /// </summary>
        /// <param name="context"></param>
        public CommandDeleteCharacter(ArraySegment<byte> data, SimulatorContext context) : base(data, 2, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var id = Data.Get2BytesUInt(0);
            Context.PlayContext.DeleteActor(id);
            return null;
        }

        #endregion 方法
    }
}