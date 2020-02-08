using System;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 角色脚步命令
    /// </summary>
    internal class CommandNpcStep : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 角色脚步命令
        /// </summary>
        /// <param name="context"></param>
        public CommandNpcStep(ArraySegment<byte> data, SimulatorContext context) : base(data, 6, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandNpcStepOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandNpcStepOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 面向
            /// </summary>
            private readonly Direction _faceTo;

            /// <summary>
            /// 角色ID
            /// 0为主角
            /// </summary>
            private readonly int _id;

            /// <summary>
            /// 脚步状态
            /// </summary>
            private readonly int _step;

            /// <summary>
            /// 更新间隔
            /// </summary>
            private long _interval = 0;

            /// <summary>
            /// 处理时间
            /// </summary>
            private long _processTime = 0;

            #endregion 字段

            #region 构造函数

            public CommandNpcStepOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                var start = data.Offset;
                var code = data.Array;

                _id = code.Get2BytesUInt(start);
                _faceTo = (Direction)code.Get2BytesUInt(start + 2);
                _step = code.Get2BytesUInt(start + 4);

                _processTime = 0;
                if (_id == 0)   //主角动作
                {
                    PlayerCharacter character = Context.PlayContext.PlayerCharacter;
                    character.Direction = _faceTo;
                    character.SetStep(_step);
                    _interval = 300;
                }
                else    //NPC
                {
                    NPC npc = Context.SceneMap.SceneNPCs[_id];
                    npc.Direction = _faceTo;
                    npc.SetStep(_step);
                    if (Context.SceneMap.IsNpcVisible(npc))
                    {
                        _interval = 300;
                    }
                    else
                    {
                        _interval = 0;
                    }
                }
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.SceneMap.DrawScene(canvas);
            }

            public override bool Update(long delta)
            {
                _processTime += delta;
                return _processTime < _interval;
            }

            #endregion 方法
        }

        #endregion 类
    }
}