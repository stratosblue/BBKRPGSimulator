using System.Drawing;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 移动命令
    /// </summary>
    internal class CommandMove : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 移动命令
        /// </summary>
        /// <param name="context"></param>
        public CommandMove(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 6;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandMoveOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 移动命令的操作
        /// </summary>
        private class CommandMoveOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 目标X坐标
            /// </summary>
            private readonly int _destinationX;

            /// <summary>
            /// 目标Y坐标
            /// </summary>
            private readonly int _destinationY;

            private byte[] _code;

            /// <summary>
            /// 移动间隔
            /// </summary>
            private long _interval = 400;

            private int _start;

            /// <summary>
            /// 目标NPC
            /// </summary>
            private NPC _targetNPC;

            #endregion 字段

            #region 构造函数

            public CommandMoveOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
                _destinationX = code.Get2BytesUInt(start + 2);
                _destinationY = code.Get2BytesUInt(start + 4);
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.SceneMap.DrawScene(canvas);
            }

            public override void OnKeyDown(int key)
            {
            }

            public override void OnKeyUp(int key)
            {
            }

            public override bool Process()
            {
                _targetNPC = Context.SceneMap.SceneNPCs[_code.Get2BytesUInt(_start)];
                return true;
            }

            public override bool Update(long delta)
            {
                _interval += delta;
                if (_interval > 100)
                {
                    Point point = _targetNPC.PosInMap;
                    if (_destinationX < point.X)
                    {
                        _targetNPC.Walk(Direction.West);
                    }
                    else if (_destinationX > point.X)
                    {
                        _targetNPC.Walk(Direction.East);
                    }
                    else if (_destinationY < point.Y)
                    {
                        _targetNPC.Walk(Direction.North);
                    }
                    else if (_destinationY > point.Y)
                    {
                        _targetNPC.Walk(Direction.South);
                    }
                    else
                    {
                        return false;
                    }
                    _interval = 0;
                }
                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}