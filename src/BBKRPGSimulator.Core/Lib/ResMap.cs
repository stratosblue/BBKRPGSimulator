using System;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.Lib
{
    /// <summary>
    /// 地图资源
    /// </summary>
    internal class ResMap : ResBase
    {
        #region 字段

        /// <summary>
        /// 纵向渲染的地图块总数
        /// </summary>
        public const int HEIGHT = Constants.SCREEN_HEIGHT / 16;

        /// <summary>
        /// 横向渲染的地图块总数
        /// </summary>
        public const int WIDTH = Constants.SCREEN_WIDTH / 16 - 1;

        /// <summary>
        /// 地图数据 两个字节表示一个地图快（从左到右，从上到下）
        /// （低字节：最高位1表示可行走，0不可行走。高字节：事件号）
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// 地图使用的地图块
        /// </summary>
        private Tiles _tiles;

        /// <summary>
        /// 该地图所用的til图块资源的索引号
        /// </summary>
        private int _tilIndex;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 地图高
        /// </summary>
        public int MapHeight { get; private set; }

        /// <summary>
        /// 地图名称
        /// </summary>
        public string MapName { get; private set; }

        /// <summary>
        /// 地图宽
        /// </summary>
        public int MapWidth { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 地图资源
        /// </summary>
        /// <param name="context"></param>
        public ResMap(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public bool CanPlayerWalk(int x, int y)
        {
            return CanWalk(x, y) && (x >= 4) && (x < MapWidth - 4)
                    && (y >= 3) && (y < MapHeight - 2);
        }

        /// <summary>
        /// 判断地图(x,y)是否可行走
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool CanWalk(int x, int y)
        {
            if (x < 0 || x >= MapWidth || y < 0 || y >= MapHeight)
            {
                return false;
            }

            int i = y * MapWidth + x;
            return (_data[i * 2] & 0x80) != 0;
        }

        /// <summary>
        /// 将地图绘制到指定的画板
        /// 水平方向 left --- left+WIDTH
        /// 竖直方向 top --- top + HEIGHT
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="left">地图的最左边</param>
        /// <param name="top">地图的最上边</param>
        public void DrawMap(ICanvas canvas, int left, int top)
        {
            if (_tiles == null)
            {
                _tiles = new Tiles(Context, _tilIndex);
            }

            int minY = Math.Min(HEIGHT, MapHeight - top);
            int minX = Math.Min(WIDTH, MapWidth - left);
            for (int y = 0; y < minY; y++)
            {
                for (int x = 0; x < minX; x++)
                {
                    _tiles.Draw(canvas, x * Tiles.WIDTH + Constants.MAP_LEFT_OFFSET,
                            y * Tiles.HEIGHT, GetTileIndex(left + x, top + y));
                }
            }
        }

        public void DrawWholeMap(ICanvas canvas, int x, int y)
        {
            if (_tiles == null)
            {
                _tiles = new Tiles(Context, _tilIndex);
            }

            for (int ty = 0; ty < MapHeight; ty++)
            {
                for (int tx = 0; tx < MapWidth; tx++)
                {
                    int sx = tx * Tiles.WIDTH + x;
                    int sy = ty * Tiles.HEIGHT + y;
                    _tiles.Draw(canvas, sx, sy, GetTileIndex(tx, ty));
                    int events = GetEventNum(tx, ty);
                    if (events != 0)
                    {
                        //TODO 这里改变了Global.COLOR_WHITE的颜色，已注释，暂时还不知道会不会有什么问题
                        //int color = Global.COLOR_WHITE;
                        //Global.COLOR_WHITE = 0xFFFF00;
                        TextRender.DrawText(canvas, events.ToString(), sx, sy);
                        //Global.COLOR_WHITE = color;
                    }
                }
            }
        }

        public int GetEventNum(int x, int y)
        {
            if (x < 0 || x >= MapWidth || y < 0 || y >= MapHeight)
            {
                return -1;
            }

            int i = y * MapWidth + x;
            return (int)_data[i * 2 + 1] & 0xFF;
        }

        public override void SetData(byte[] buf, int offset)
        {
            Type = buf[offset];
            Index = buf[offset + 1];
            _tilIndex = buf[offset + 2];
            MapName = buf.GetString(offset + 3);

            MapWidth = buf[offset + 0x10];
            MapHeight = buf[offset + 0x11];

            int len = MapWidth * MapHeight * 2;
            _data = new byte[len];
            Array.Copy(buf, offset + 0x12, _data, 0, len);
        }

        /// <summary>
        /// 获取指定位置的图块序号
        /// </summary>
        /// <param name="x">图块的x坐标</param>
        /// <param name="y">图块的y坐标</param>
        /// <returns>位置的图块在til中的序号</returns>

        private int GetTileIndex(int x, int y)
        {
            int i = y * MapWidth + x;
            return (int)_data[i * 2] & 0x7F;
        }

        #endregion 方法
    }
}