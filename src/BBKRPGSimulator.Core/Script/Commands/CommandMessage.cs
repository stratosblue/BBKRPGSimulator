using System;

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
        public CommandMessage(ArraySegment<byte> data, SimulatorContext context) : base(data, -1, context)
        {
            var start = data.Offset;
            var code = data.Array;

            int i = 0;
            while (code[start + i] != 0) ++i;
            Length = i + 1;
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandMessageOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandMessageOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 显示的消息
            /// </summary>
            private readonly byte[] _message;

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyDown;

            #endregion 字段

            #region 构造函数

            public CommandMessageOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                _message = data.Array.GetStringBytes(data.Offset);
                _isAnyKeyDown = false;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.Util.ShowMessage(canvas, _message);
            }

            public override void OnKeyUp(int key)
            {
                _isAnyKeyDown = true;
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