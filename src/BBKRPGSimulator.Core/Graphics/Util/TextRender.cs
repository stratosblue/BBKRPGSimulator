using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace BBKRPGSimulator.Graphics.Util
{
    /// <summary>
    /// 文本呈现器
    /// </summary>
    internal class TextRender : ContextDependent
    {
        #region 字段

        /// <summary>
        /// ASC字库缓存
        /// </summary>
        private static byte[] _ascBuf;

        /// <summary>
        /// HZK字库缓存
        /// </summary>
        private static byte[] _hzkBuf;

        /// <summary>
        /// 字的像素信息
        /// </summary>
        private static int[] _wordPixels;

        /// <summary>
        /// ASC字库的位图构建器
        /// </summary>
        private ImageBuilder _ascBitmapBuilder;

        /// <summary>
        /// HZK字库的位图构建器
        /// </summary>
        private ImageBuilder _hzkBitmapBuilder;

        #endregion 字段

        #region 构造函数

        public TextRender(SimulatorContext context) : base(context)
        {
            LoadData();

            _hzkBitmapBuilder = context.GraphicsFactory.NewImageBuilder(16, 16);
            _ascBitmapBuilder = context.GraphicsFactory.NewImageBuilder(8, 16);
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 加载字库数据
        /// </summary>
        public static void LoadData()
        {
            try
            {
                //TODO 如果修改命名空间或者移动资源位置，此处需要修改
                var assembly = Assembly.GetExecutingAssembly();
                if (_hzkBuf == null)
                {
                    using (var stream = assembly.GetManifestResourceStream("BBKRPGSimulator.Assets.HZK16"))
                    {
                        var buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);
                        _hzkBuf = buffer;
                    }
                }

                if (_ascBuf == null)
                {
                    using (var stream = assembly.GetManifestResourceStream("BBKRPGSimulator.Assets.ASC16"))
                    {
                        var buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);
                        _ascBuf = buffer;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }

            _wordPixels = new int[16 * 16];
        }

        /// <summary>
        /// 在指定画板的指定位置绘制反色文本
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawSelText(ICanvas canvas, string text, int x, int y)
        {
            DrawText(canvas, text, x, y, true);
        }

        /// <summary>
        /// 在指定画板的指定位置绘制反色文本
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void DrawSelText(ICanvas canvas, byte[] text, int x, int y)
        {
            DrawText(canvas, text, x, y, true);
        }

        /// <summary>
        /// 在指定画板的指定位置绘制文本
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isInverse"></param>
        public void DrawText(ICanvas canvas, string text, int x, int y, bool isInverse = false)
        {
            DrawText(canvas, text.GetBytes(), x, y, isInverse);
        }

        /// <summary>
        /// 在指定画板的指定位置绘制文本
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isInverse"></param>
        public void DrawText(ICanvas canvas, byte[] text, int x, int y, bool isInverse = false)
        {
            for (int i = 0; i < text.Length && text[i] != 0; i++)
            {
                int t = text[i] & 0xFF;
                if (t >= 0xa1)
                {
                    ++i;
                    int offset = (94 * (t - 0xa1) + (text[i] & 0xFF) - 0xa1) * 32;
                    canvas.DrawBitmap(GetHzk(offset, isInverse), x, y);
                    x += 16;
                }
                else if (t < 128)
                {
                    int offset = t * 16;
                    canvas.DrawBitmap(GetAsc(offset, isInverse), x, y);
                    x += 8;
                }
                else
                {
                    x += 8;
                }
            }
        }

        /// <summary>
        /// 在指定画板的指定位置绘制文本
        /// 返回：0,文字都在textArea.top上方 1,文字在textArea中 2,文字都在textArea.bottom下方 -1,出错
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="text"></param>
        /// <param name="textArea"></param>
        /// <param name="y"></param>
        /// <returns>0,文字都在textArea.top上方 1,文字在textArea中 2,文字都在textArea.bottom下方 -1,出错</returns>
        public int DrawText(ICanvas canvas, string text, Rectangle textArea, int y)
        {
            byte[] buf = null;
            try
            {
                buf = text.GetBytes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return -1;
            }

            int i = 0;
            // 比r.top高的不画
            for (; y <= textArea.Top - 16 && i < buf.Length; y += 16)
            {
                for (int x = 0; x < 160 && i < buf.Length;)
                {
                    int t = (int)buf[i] & 0xFF;
                    if (t >= 0xa1)
                    {
                        i += 2;
                        x += 16;
                    }
                    else
                    {
                        ++i;
                        x += 8;
                    }
                }
            }

            if (i >= buf.Length)
            {
                return 0;
            }

            // 比r.bottom低的不画
            for (; y < textArea.Bottom && i < buf.Length; y += 16)
            {
                for (int x = 0; x < 160 && i < buf.Length;)
                {
                    int t = (int)buf[i] & 0xFF;
                    if (t >= 0xa1)
                    {
                        ++i;
                        int offset = (94 * (t - 0xa1) + ((int)buf[i] & 0xFF) - 0xa1) * 32;
                        canvas.DrawBitmap(GetHzk(offset), x, y);
                        x += 16;
                    }
                    else if (t < 128)
                    {
                        int offset = t * 16;
                        canvas.DrawBitmap(GetAsc(offset), x, y);
                        x += 8;
                    }
                    else
                    {
                        x += 8;
                    }
                    ++i;
                }
            }

            if (i == 0 && buf.Length > 0)
            {
                return 2;
            }

            return 1;
        }

        /// <summary>
        /// 在指定画板的指定位置绘制文本
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public int DrawText(ICanvas canvas, string text, int start, Rectangle r)
        {
            return DrawText(canvas, text.GetBytes(), start, r);
        }

        /// <summary>
        /// 在指定画板的指定位置绘制 buffer中指定开始位置的文本
        /// 并返回接下来显示的索引
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="buf"></param>
        /// <param name="start">buf中第一个要画的字节</param>
        /// <param name="r"></param>
        /// <returns>下一个要画的字节</returns>
        public int DrawText(ICanvas canvas, byte[] buf, int start, Rectangle r)
        {
            int i = start;
            int y = r.Top;
            // 比r.bottom低的不画
            for (; y <= r.Bottom - 16 && i < buf.Length; y += 16)
            {
                for (int x = r.Left; x <= r.Right - 16 && i < buf.Length;)
                {
                    int t = (int)buf[i] & 0xFF;
                    if (t >= 0xa1)
                    {
                        ++i;
                        int offset = (94 * (t - 0xa1) + ((int)buf[i] & 0xFF) - 0xa1) * 32;
                        canvas.DrawBitmap(GetHzk(offset), x, y);
                        x += 16;
                    }
                    else if (t < 128)
                    {
                        int offset = t * 16;
                        canvas.DrawBitmap(GetAsc(offset), x, y);
                        x += 8;
                    }
                    else
                    {
                        x += 8;
                    }
                    ++i;
                }
            }

            return i;
        }

        #region 字库文字获取

        /// <summary>
        /// 从字库文件获取文字
        /// </summary>
        /// <param name="offset">偏移</param>
        /// <param name="isInverse">是否反色</param>
        /// <returns></returns>
        private ImageBuilder GetAsc(int offset, bool isInverse = false)
        {
            int colorWhite = Constants.COLOR_WHITE;
            int colorBlack = Constants.COLOR_BLACK;

            if (isInverse)
            {
                colorWhite = Constants.COLOR_BLACK;
                colorBlack = Constants.COLOR_WHITE;
            }

            for (int i = 0; i < 16; i++)
            {
                int t = _ascBuf[offset + i];
                int k = i << 3;
                _wordPixels[k] = (t & 0x80) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 1] = (t & 0x40) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 2] = (t & 0x20) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 3] = (t & 0x10) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 4] = (t & 0x08) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 5] = (t & 0x04) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 6] = (t & 0x02) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 7] = (t & 0x01) != 0 ? colorBlack : colorWhite;
            }
            _ascBitmapBuilder.SetPixels(0, 0, 8, 16, _wordPixels, 0, 8);
            return _ascBitmapBuilder;
        }

        /// <summary>
        /// 从字库文件获取文字
        /// </summary>
        /// <param name="offset">偏移</param>
        /// <param name="isInverse">是否反色</param>
        /// <returns></returns>
        private ImageBuilder GetHzk(int offset, bool isInverse = false)
        {
            int colorWhite = Constants.COLOR_WHITE;
            int colorBlack = Constants.COLOR_BLACK;

            if (isInverse)
            {
                colorWhite = Constants.COLOR_BLACK;
                colorBlack = Constants.COLOR_WHITE;
            }
            for (int i = 0; i < 32; i++)
            {
                int t = _hzkBuf[offset + i];
                int k = i << 3;
                _wordPixels[k] = (t & 0x80) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 1] = (t & 0x40) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 2] = (t & 0x20) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 3] = (t & 0x10) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 4] = (t & 0x08) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 5] = (t & 0x04) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 6] = (t & 0x02) != 0 ? colorBlack : colorWhite;
                _wordPixels[k | 7] = (t & 0x01) != 0 ? colorBlack : colorWhite;
            }
            _hzkBitmapBuilder.SetPixels(0, 0, 16, 16, _wordPixels, 0, 16);
            return _hzkBitmapBuilder;
        }

        #endregion 字库文字获取

        #endregion 方法
    }
}