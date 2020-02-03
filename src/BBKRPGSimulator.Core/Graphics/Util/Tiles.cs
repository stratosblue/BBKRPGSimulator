using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Graphics.Util
{
    /// <summary>
    /// 地图块
    /// </summary>
    internal class Tiles : ContextDependent
    {
        #region 字段

        /// <summary>
        /// 地图块的高
        /// </summary>
        public const int HEIGHT = 16;

        /// <summary>
        /// 地图块的宽
        /// </summary>
        public const int WIDTH = 16;

        /// <summary>
        /// 地图块图片资源
        /// </summary>
        private ResImage _tileRes;

        #endregion 字段

        #region 构造函数

        public Tiles(SimulatorContext context, int index) : base(context)
        {
            _tileRes = Context.LibData.GetTileImage(1, index);
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 在指定位置绘制地图块
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="index">图块的序号</param>
        public void Draw(ICanvas canvas, int x, int y, int index)
        {
            _tileRes.Draw(canvas, index + 1, x, y);
        }

        #endregion 方法
    }
}