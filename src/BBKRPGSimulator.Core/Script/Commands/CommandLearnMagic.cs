using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 学习魔法命令
    /// </summary>
    internal class CommandLearnMagic : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 学习魔法命令
        /// </summary>
        /// <param name="context"></param>
        public CommandLearnMagic(SimulatorContext context) : base(context)
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
            return new CommandLearnMagicOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 学习魔法命令的操作
        /// </summary>
        private class CommandLearnMagicOperate : Operate
        {
            #region 字段

            private byte[] _code;

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyDown;

            /// <summary>
            /// 显示时间
            /// </summary>
            private long _showTime;

            private int _start;

            #endregion 字段

            #region 属性

            private TextRender TextRender => Context.TextRender;

            #endregion 属性

            #region 构造函数

            /// <summary>
            /// 学习魔法命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandLearnMagicOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                //TODO 修正显示
                TextRender.DrawText(canvas, "学会了魔法:", 0, 0);
                TextRender.DrawText(canvas, "actorId:" + _code.Get2BytesUInt(_start)
                        + "t" + _code.Get2BytesUInt(_start + 2)
                        + "i" + _code.Get2BytesUInt(_start + 4), 0, 16);
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
                _showTime = 0;
                return true;
            }

            public override bool Update(long delta)
            {
                _showTime += delta;
                return _showTime < 1000 && !_isAnyKeyDown;
            }

            #endregion 方法
        }

        #endregion 类
    }
}