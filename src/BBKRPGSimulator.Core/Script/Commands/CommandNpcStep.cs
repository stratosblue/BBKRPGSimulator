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
        public CommandNpcStep(SimulatorContext context) : base(context)
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
            return new CommandNpcStepOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 角色脚步命令的操作
        /// </summary>
        private class CommandNpcStepOperate : Operate
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

            private byte[] _code;

            /// <summary>
            /// 更新间隔
            /// </summary>
            private long _interval = 0;

            /// <summary>
            /// 处理时间
            /// </summary>
            private long _processTime = 0;

            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 角色脚步命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandNpcStepOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;

                _id = code.Get2BytesUInt(start);
                _faceTo = (Direction)code.Get2BytesUInt(start + 2);
                _step = code.Get2BytesUInt(start + 4);
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
                return true;
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