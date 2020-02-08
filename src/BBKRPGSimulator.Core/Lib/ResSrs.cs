using System.Collections.Generic;

using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Lib
{
    /// <summary>
    /// 特效资源
    /// </summary>
    internal class ResSrs : ResBase
    {
        #region 字段

        /// <summary>
        /// 结束帧
        /// </summary>
        private int _endFrame;

        /// <summary>
        /// 帧数
        /// </summary>
        private int _frameCount;

        /// <summary>
        /// 帧的头定义
        /// mFrameHeader = new int[mFrameNum,5]
        /// </summary>
        private int[,] _frameHeader;

        /// <summary>
        /// 图片总数
        /// </summary>
        private int _imageCount;

        /// <summary>
        /// 图片资源数组
        /// </summary>
        private ResImage[] _images;

        /// <summary>
        /// update 迭代次数
        /// </summary>
        private int _iterator = 1;

        /// <summary>
        /// 显示列表
        /// </summary>
        private List<FrameInfo> _showList = new List<FrameInfo>();

        /// <summary>
        /// 起始帧
        /// </summary>
        private int _startFrame;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 特效资源
        /// </summary>
        /// <param name="context"></param>
        public ResSrs(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas, int dx, int dy)
        {
            foreach (var item in _showList)
            {
                _images[_frameHeader[item.Index, 4]].Draw(canvas, 1, _frameHeader[item.Index, 0] + dx, _frameHeader[item.Index, 1] + dy);
            }
        }

        public void DrawAbsolutely(ICanvas canvas, int x, int y)
        {
            foreach (var item in _showList)
            {
                _images[_frameHeader[item.Index, 4]].Draw(canvas, 1,
                    _frameHeader[item.Index, 0] - _frameHeader[0, 0] + x,
                    _frameHeader[item.Index, 1] - _frameHeader[0, 1] + y);
            }
        }

        public override void SetData(byte[] buf, int offset)
        {
            Type = buf[offset];
            Index = buf[offset + 1] & 0xFF;
            _frameCount = buf[offset + 2] & 0xFF;
            _imageCount = buf[offset + 3] & 0xFF;
            _startFrame = buf[offset + 4] & 0xFF;
            _endFrame = buf[offset + 5] & 0xFF;

            int ptr = offset + 6;
            _frameHeader = new int[_frameCount, 5];

            for (int i = 0; i < _frameCount; i++)
            {
                _frameHeader[i, 0] = buf[ptr++] & 0xFF; // x
                _frameHeader[i, 1] = buf[ptr++] & 0xFF; // y
                _frameHeader[i, 2] = buf[ptr++] & 0xFF; // Show
                _frameHeader[i, 3] = buf[ptr++] & 0xFF; // nShow
                _frameHeader[i, 4] = buf[ptr++] & 0xFF; // 图号
            }

            // 读入_imageCount个ResImage
            _images = new ResImage[_imageCount];
            for (int i = 0; i < _imageCount; i++)
            {
                _images[i] = new ResImage(Context);
                _images[i].SetData(buf, ptr);
                ptr += _images[i].GetBytesCount();
            }
        }

        /// <summary>
        /// 设置update迭代次数
        /// </summary>
        /// <param name="n"></param>
        public void SetIteratorNum(int n)
        {
            _iterator = n;
            if (_iterator < 1)
            {
                _iterator = 1;
            }
        }

        /// <summary>
        /// 开始特效动画
        /// </summary>
        public void StartAni()
        {
            _showList.Clear();
            _showList.Add(new FrameInfo(_frameHeader, 0));
        }

        /// <summary>
        /// 返回false动画播放完毕
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool Update(long delta)
        {
            for (int j = 0; j < _iterator; j++)
            {
                for (int i = 0; i < _showList.Count; i++)
                {
                    FrameInfo current = _showList[i];
                    --current.Show;
                    --current.NShow;
                    if (current.NShow == 0 && current.Index + 1 < _frameCount)
                    {
                        _showList.Add(new FrameInfo(_frameHeader, current.Index + 1)); // 下一帧开始显示
                    }
                }

                for (int i = 0; i < _showList.Count; i++)
                {
                    FrameInfo current = _showList[i];

                    if (current.Show <= 0)
                    {
                        // 该帧的图片显示完成
                        _showList.RemoveAt(i);
                        i--;
                    }
                }

                if (_showList.Count <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 资源帧信息
        /// </summary>
        private class FrameInfo
        {
            //TODO 特效资源呈现模式需要了解逻辑

            #region 字段

            /// <summary>
            /// 在帧的头信息中的索引
            /// </summary>
            public int Index { get; set; }

            /// <summary>
            /// 这是啥？
            /// </summary>
            public int NShow { get; set; }

            /// <summary>
            /// 这又是啥？
            /// </summary>
            public int Show { get; set; }

            #endregion 字段

            #region 构造函数

            public FrameInfo(int[,] frameHeader, int index)
            {
                Index = index;
                Show = frameHeader[index, 2];
                NShow = frameHeader[index, 3];
            }

            #endregion 构造函数
        }

        #endregion 类
    }
}