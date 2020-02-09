using System;

using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 定时消息？
    /// </summary>
    internal class CommandTimeMsg : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 定时消息？
        /// </summary>
        /// <param name="context"></param>
        public CommandTimeMsg(ArraySegment<byte> data, SimulatorContext context) : base(data, -1, context)
        {
            //TODO 这里还需要验证
            Length = data.GetStringLength(2) + 2;
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandTimeMsgOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandTimeMsgOperate : Operate
        {
            #region 字段

            private readonly string _message;
            private readonly int _time;

            private long _countDown;
            private int _downKey;
            private bool _isAnyKeyDown = false;

            #endregion 字段

            #region 构造函数

            public CommandTimeMsgOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                _time = data.Get2BytesUInt(0);
                _message = data.GetString(2);

                _downKey = SimulatorKeys.KEY_INVALID;
                _isAnyKeyDown = false;
                _countDown = _time * 10;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.Util.ShowMessage(canvas, _message);
            }

            public override void OnKeyDown(int key)
            {
                _downKey = key;
            }

            public override void OnKeyUp(int key)
            {
                if (_downKey == key)
                {
                    _isAnyKeyDown = true;
                }
            }

            public override bool Update(long delta)
            {
                if (_countDown != 0)
                {
                    _countDown -= delta;
                    if (_countDown <= 0)
                    {
                        return false;
                    }
                }
                return !_isAnyKeyDown;
            }

            #endregion 方法
        }

        #endregion 类
    }
}