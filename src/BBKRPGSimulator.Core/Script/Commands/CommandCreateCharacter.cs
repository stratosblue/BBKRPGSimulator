using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 创建角色命令
    /// </summary>
    internal class CommandCreateCharacter : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 创建角色命令
        /// </summary>
        /// <param name="context"></param>
        public CommandCreateCharacter(ArraySegment<byte> data, SimulatorContext context) : base(data, 6, context)
        {
        }

        protected override Operate ProcessAndGetOperate() => new CommandCreateCharacterOperate(Data, Context);

        #endregion 构造函数

        #region 类

        public class CommandCreateCharacterOperate : OperateDrawScene
        {
            #region 字段

            private readonly int _id, _x, _y;

            #endregion 字段

            #region 构造函数

            public CommandCreateCharacterOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                _id = data.Get2BytesUInt(0);
                _x = data.Get2BytesUInt(2);
                _y = data.Get2BytesUInt(4);

                Context.PlayContext.CreateActor(_id, _x, _y);
            }

            #endregion 构造函数
        }

        #endregion 类
    }
}