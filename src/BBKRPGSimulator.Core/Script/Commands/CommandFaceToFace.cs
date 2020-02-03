using System.Drawing;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;

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
        public CommandFaceToFace(SimulatorContext context) : base(context)
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
            return new CommandFaceToFaceOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 面对面命令的操作
        /// </summary>
        private class CommandFaceToFaceOperate : OperateDrawOnce
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 面对面命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandFaceToFaceOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override void DrawOnce(ICanvas canvas)
            {
                Context.SceneMap.DrawScene(canvas);
            }

            public override bool Process()
            {
                Character character1 = GetCharacter(_code.Get2BytesUInt(_start));
                Character character2 = GetCharacter(_code.Get2BytesUInt(_start + 2));
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
                return true;
            }

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