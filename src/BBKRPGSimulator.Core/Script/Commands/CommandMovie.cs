using System;

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
        public CommandMovie(ArraySegment<byte> data, SimulatorContext context) : base(data, 10, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandMovieOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandMovieOperate : Operate
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

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyPressed = false;

            /// <summary>
            /// 要播放的动画
            /// </summary>
            private ResSrs _movie;

            #endregion 字段

            #region 构造函数

            public CommandMovieOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                _type = data.Get2BytesUInt(0);
                _index = data.Get2BytesUInt(2);
                _showX = data.Get2BytesUInt(4);
                _showY = data.Get2BytesUInt(6);
                _control = data.Get2BytesUInt(8);

                _movie = Context.LibData.GetSrs(_type, _index);
                _movie.SetIteratorNum(5);
                _movie.StartAni();
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

            public override void OnKeyUp(int key)
            {
                _isAnyKeyPressed = true;
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