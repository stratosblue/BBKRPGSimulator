namespace BBKRPGSimulator.Graphics
{
    internal class DefaultGraphicsFactory : IGraphicsFactory
    {
        #region 方法

        public ICanvas NewCanvas() => new Canvas(NewImageBuilder(Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT));

        public ICanvas NewCanvas(ImageBuilder image) => new Canvas(image);

        public ImageBuilder NewImageBuilder(int width, int height) => new InternalImageBuilder(width, height);

        public ImageBuilder NewImageBuilder(byte[] data, int width, int height) => new InternalImageBuilder(data, width, height);

        #endregion 方法
    }
}