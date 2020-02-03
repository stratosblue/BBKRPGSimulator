using BBKRPGSimulator.Graphics;

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
        public CommandCreateCharacter(SimulatorContext context) : base(context)
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
            return new CommandCreateCharacterOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 创建角色命令的操作
        /// </summary>
        private class CommandCreateCharacterOperate : OperateDrawOnce
        {
            #region 字段

            private byte[] _code;
            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 创建角色命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandCreateCharacterOperate(SimulatorContext context, byte[] code, int start) : base(context)
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
                Context.PlayContext.CreateActor(_code.Get2BytesUInt(_start),
                            _code.Get2BytesUInt(_start + 2),
                            _code.Get2BytesUInt(_start + 4));
                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}