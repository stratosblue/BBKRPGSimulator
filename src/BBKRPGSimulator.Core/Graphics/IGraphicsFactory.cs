namespace BBKRPGSimulator.Graphics
{
    public interface IGraphicsFactory
    {
        #region 方法

        /// <summary>
        /// 新画布
        /// </summary>
        /// <returns></returns>
        ICanvas NewCanvas();

        /// <summary>
        /// 有背景的新画布
        /// </summary>
        /// <param name="image">背景</param>
        /// <returns></returns>
        ICanvas NewCanvas(ImageBuilder image);

        /// <summary>
        /// 创建一个图像构建器
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        ImageBuilder NewImageBuilder(int width, int height);

        /// <summary>
        /// 创建一个图像构建器
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        ImageBuilder NewImageBuilder(byte[] data, int width, int height);

        #endregion 方法
    }
}