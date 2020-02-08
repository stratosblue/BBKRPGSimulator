using System;
using System.Drawing;

using BBKRPGSimulator.Characters;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 面对面命令
    /// </summary>
    internal class CommandFaceToFace : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 面对面命令
        /// </summary>
        /// <param name="context"></param>
        public CommandFaceToFace(ArraySegment<byte> data, SimulatorContext context) : base(data, 4, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandFaceToFaceOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandFaceToFaceOperate : OperateDrawScene
        {
            #region 字段

            private readonly int _characterId1, _characterId2;

            #endregion 字段

            #region 构造函数

            public CommandFaceToFaceOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                _characterId1 = data.Get2BytesUInt(0);
                _characterId2 = data.Get2BytesUInt(2);

                Character character1 = GetCharacter(_characterId1);
                Character character2 = GetCharacter(_characterId2);
                Point point1 = character1.PosInMap;
                Point point2 = character2.PosInMap;
                if (point1.X > point2.X)
                {
                    character2.Direction = Direction.East;
                }
                else if (point1.X < point2.X)
                {
                    character2.Direction = Direction.West;
                }
                else
                {
                    if (point1.Y > point2.Y)
                    {
                        character2.Direction = Direction.South;
                    }
                    else if (point1.Y < point2.Y)
                    {
                        character2.Direction = Direction.North;
                    }
                }
            }

            #endregion 构造函数

            #region 方法

            /// <summary>
            /// 获取指定ID的角色
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            private Character GetCharacter(int id)
            {
                if (id == 0)
                {
                    return Context.PlayContext.PlayerCharacter;
                }
                return Context.SceneMap.SceneNPCs[id];
            }

            #endregion 方法
        }

        #endregion 类
    }
}