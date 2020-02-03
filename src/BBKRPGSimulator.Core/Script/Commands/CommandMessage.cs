using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 消息命令
    /// </summary>
    internal class CommandMessage : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 消息命令
        /// </summary>
        /// <param name="context"></param>
        public CommandMessage(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            int i = 0;
            while (code[start + i] != 0) ++i;
            return start + i + 1;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandMessageOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 消息命令的操作
        /// </summary>
        private class CommandMessageOperate : Operate
        {
            #region 字段

            private byte[] _code;

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyDown;

            /// <summary>
            /// 显示的消息
            /// </summary>
            private byte[] _message;

            private int _start;

            #endregion 字段

            #region 构造函数

            public CommandMessageOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
                _message = code.GetStringBytes(start);
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.Util.ShowMessage(canvas, _message);
            }

            public override void OnKeyDown(int key)
            {
            }

            public override void OnKeyUp(int key)
            {
                _isAnyKeyDown = true;
            }

            public override bool Process()
            {
                _isAnyKeyDown = false;
                return true;
            }

            public override bool Update(long delta)
            {
                return !_isAnyKeyDown;
            }

            #endregion 方法
        }

        #endregion 类
    }
}