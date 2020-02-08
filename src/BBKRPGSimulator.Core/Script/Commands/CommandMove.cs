using System;
using System.Drawing;

using BBKRPGSimulator.Characters;

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
        public CommandMove(ArraySegment<byte> data, SimulatorContext context) : base(data, 6, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandMoveOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandMoveOperate : OperateDrawScene
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

            private readonly int _npcId;

            /// <summary>
            /// 移动间隔
            /// </summary>
            private long _interval = 400;

            /// <summary>
            /// 目标NPC
            /// </summary>
            private NPC _targetNPC;

            #endregion 字段

            #region 构造函数

            public CommandMoveOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                _npcId = data.Get2BytesUInt(0);
                _destinationX = data.Get2BytesUInt(2);
                _destinationY = data.Get2BytesUInt(4);

                _targetNPC = Context.SceneMap.SceneNPCs[_npcId];
                _interval = 400;
            }

            #endregion 构造函数

            #region 方法

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