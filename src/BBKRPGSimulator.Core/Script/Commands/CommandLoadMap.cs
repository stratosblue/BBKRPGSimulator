using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 加载地图命令
    /// </summary>
    internal class CommandLoadMap : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 加载地图命令
        /// </summary>
        /// <param name="context"></param>
        public CommandLoadMap(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 8;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandLoadMapOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 加载地图命令的操作
        /// </summary>
        private class CommandLoadMapOperate : OperateDrawOnce
        {
            #region 字段

            /// <summary>
            /// 地图index
            /// </summary>
            private readonly int _index;

            /// <summary>
            /// 地图type
            /// </summary>
            private readonly int _type;

            /// <summary>
            /// 载入的x坐标
            /// </summary>
            private readonly int _x;

            /// <summary>
            /// 载入的y坐标
            /// </summary>
            private readonly int _y;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 加载地图命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandLoadMapOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _type = ((int)code[start] & 0xFF) | ((int)code[start + 1] << 8 & 0xFF00);
                _index = ((int)code[start + 2] & 0xFF) | ((int)code[start + 3] << 8 & 0xFF00);
                _x = ((int)code[start + 4] & 0xFF) | ((int)code[start + 5] << 8 & 0xFF00);
                _y = ((int)code[start + 6] & 0xFF) | ((int)code[start + 7] << 8 & 0xFF00);
            }

            #endregion 构造函数

            #region 方法

            public override void DrawOnce(ICanvas canvas)
            {
                Context.SceneMap.DrawScene(canvas);
            }

            public override bool Process()
            {
                Context.SceneMap.LoadMap(_type, _index, _x, _y);
                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}