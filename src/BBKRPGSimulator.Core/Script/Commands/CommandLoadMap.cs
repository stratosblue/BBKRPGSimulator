using System;

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
        /// <param name="data"></param>
        /// <param name="context"></param>
        public CommandLoadMap(ArraySegment<byte> data, SimulatorContext context) : base(data, 8, context)
        {
        }

        protected override Operate ProcessAndGetOperate() => new CommandLoadMapOperate(Data, Context);

        #endregion 构造函数

        #region 类

        public class CommandLoadMapOperate : OperateDrawScene
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

            public CommandLoadMapOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                var start = data.Offset;
                var code = data.Array;
                _type = (code[start] & 0xFF) | ((code[start + 1] << 8) & 0xFF00);
                _index = (code[start + 2] & 0xFF) | ((code[start + 3] << 8) & 0xFF00);
                _x = (code[start + 4] & 0xFF) | ((code[start + 5] << 8) & 0xFF00);
                _y = (code[start + 6] & 0xFF) | ((code[start + 7] << 8) & 0xFF00);
                Context.SceneMap.LoadMap(_type, _index, _x, _y);
            }

            #endregion 构造函数
        }

        #endregion 类
    }
}