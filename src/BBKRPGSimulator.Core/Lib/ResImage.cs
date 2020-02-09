using System;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.Lib
{
    /// <summary>
    /// 图片资源
    /// </summary>
    internal class ResImage : ResBase
    {
        #region 字段

        /// <summary>
        /// 图片数据数组
        /// </summary>
        protected ImageBuilder[] _bitmaps;

        /// <summary>
        /// 图像数据 不透明：一位一像素，0白，1黑。
        /// 透明：两位一像素，高位（0不透明，1透明），低位（0白，1黑）。
        /// 注意：有冗余数据。
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// 是否透明
        /// </summary>
        private bool _transparent;

        #endregion 字段

        #region 索引器

        /// <summary>
        /// 获取资源指定索引的图像
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ImageBuilder this[int index]
        {
            get
            {
                if (index >= Number)
                {
                    return null;
                }
                return _bitmaps[index];
            }
        }

        #endregion 索引器

        #region 属性

        /// <summary>
        /// 切片高
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// 切片数量
        /// </summary>
        public int Number { get; protected set; }

        /// <summary>
        /// 切片宽
        /// </summary>
        public int Width { get; protected set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 图片资源
        /// </summary>
        /// <param name="context"></param>
        public ResImage(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 绘制图片到画布
        /// </summary>
        /// <param name="canvas">目标画布</param>
        /// <param name="num">要画的切片编号,>0</param>
        /// <param name="left">画到画布的最左端位置</param>
        /// <param name="top">画到画布的最上端位置</param>
        public void Draw(ICanvas canvas, int num, int left, int top)
        {
            if (num <= Number)
            {
                canvas.DrawBitmap(_bitmaps[num - 1], left, top);
            }
            else
            {
                if (Number > 0)
                {
                    //TODO 这里需要与原版确认
                    canvas.DrawBitmap(_bitmaps[0], left, top);
                }
                else
                {
                    TextRender.DrawText(canvas, "烫", left, top);
                }
            }
        }

        /// <summary>
        /// 获取资源的大小
        /// </summary>
        public int GetBytesCount()
        {
            return _data.Length + 6;
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        public override void SetData(byte[] buf, int offset)
        {
            Type = buf[offset];
            Index = (int)buf[offset + 1] & 0xFF;
            Width = (int)buf[offset + 2] & 0xFF;
            Height = (int)buf[offset + 3] & 0xFF;
            Number = (int)buf[offset + 4] & 0xFF;
            _transparent = buf[offset + 5] == 2;

            int len = Number * (Width / 8 + (Width % 8 != 0 ? 1 : 0))
                   * Height * buf[offset + 5];
            _data = new byte[len];

            Array.Copy(buf, offset + 6, _data, 0, len);

            CreateBitmaps();
        }

        /// <summary>
        /// 根据当前数据创建位图数组
        /// </summary>
        private void CreateBitmaps()
        {
            _bitmaps = new ImageBuilder[Number];

            int[] tmp = new int[Width * Height];
            int iOfData = 0;

            if (_transparent)
            {
                for (int i = 0; i < Number; i++)
                {
                    int cnt = 0, iOfTmp = 0;
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            if (((_data[iOfData] << cnt) & 0x80) != 0)
                            {
                                tmp[iOfTmp] = Constants.COLOR_TRANSP;
                            }
                            else
                            {
                                tmp[iOfTmp] = ((_data[iOfData] << cnt << 1) & 0x80) != 0 ?
                                        Constants.COLOR_BLACK : Constants.COLOR_WHITE;
                            }
                            ++iOfTmp;
                            cnt += 2;
                            if (cnt >= 8)
                            {
                                cnt = 0;
                                ++iOfData;
                            }
                        }

                        if (cnt > 0 && cnt <= 7)
                        {
                            cnt = 0;
                            ++iOfData;
                        }
                        if (iOfData % 2 != 0)
                        {
                            ++iOfData;
                        }
                    }
                    _bitmaps[i] = Context.GraphicsFactory.NewImageBuilder(ImageBuilderUtil.IntegerArrayToImageBytes(tmp, Width, Height), Width, Height);
                } // for mNumber
            }
            else
            { // 不透明
                for (int i = 0; i < Number; i++)
                {
                    int cnt = 0, iOfTmp = 0;
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            tmp[iOfTmp++] = ((_data[iOfData] << cnt) & 0x80) != 0 ? Constants.COLOR_BLACK
                                    : Constants.COLOR_WHITE;
                            if (++cnt >= 8)
                            {
                                cnt = 0;
                                ++iOfData;
                            }
                        }
                        if (cnt != 0)
                        { // 不足一字节的舍去
                            cnt = 0;
                            ++iOfData;
                        }
                    } // end for (int y = ...
                    _bitmaps[i] = Context.GraphicsFactory.NewImageBuilder(ImageBuilderUtil.IntegerArrayToImageBytes(tmp, Width, Height), Width, Height);
                } // end for (int i = ...*/
            } // end if
        }

        #endregion 方法
    }
}