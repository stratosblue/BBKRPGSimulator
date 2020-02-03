using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 动画命令
    /// </summary>
    internal class CommandMovie : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 动画命令
        /// </summary>
        /// <param name="context"></param>
        public CommandMovie(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 10;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandMovieOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 动画命令的操作
        /// </summary>
        private class CommandMovieOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 控制？
            /// </summary>
            private readonly int _control;

            /// <summary>
            /// 动画的类型、索引、显示位置
            /// </summary>
            private readonly int _type, _index, _showX, _showY;

            private byte[] _code;

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyPressed = false;

            /// <summary>
            /// 要播放的动画
            /// </summary>
            private ResSrs _movie;

            private int _start;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 动画命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandMovieOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;

                _type = code.Get2BytesUInt(start);
                _index = code.Get2BytesUInt(start + 2);
                _showX = code.Get2BytesUInt(start + 4);
                _showY = code.Get2BytesUInt(start + 6);
                _control = code.Get2BytesUInt(start + 8);
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                if (_control == 2 || _control == 3)
                {
                    Context.SceneMap.DrawScene(canvas);
                }
                _movie.Draw(canvas, _showX, _showY);
            }

            public override void OnKeyDown(int key)
            {
            }

            public override void OnKeyUp(int key)
            {
                _isAnyKeyPressed = true;
            }

            public override bool Process()
            {
                _movie = Context.LibData.GetSrs(_type, _index);
                _movie.SetIteratorNum(5);
                _movie.StartAni();
                return true;
            }

            public override bool Update(long delta)
            {
                if ((_control == 1 || _control == 3) && _isAnyKeyPressed)
                {
                    return false;
                }
                return _movie.Update(delta);
            }

            #endregion 方法
        }

        #endregion 类
    }
}