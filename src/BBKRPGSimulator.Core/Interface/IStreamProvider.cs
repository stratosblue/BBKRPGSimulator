using System.IO;

namespace BBKRPGSimulator
{
    /// <summary>
    /// 流提供器
    /// </summary>
    public interface IStreamProvider
    {
        #region 方法

        /// <summary>
        /// 获取流
        /// </summary>
        /// <param name="relativeFilePath">相对文件路径</param>
        /// <returns></returns>
        Stream GetOrCreateStream(string relativeFilePath);

        /// <summary>
        /// 获取流
        /// </summary>
        /// <param name="relativeFilePath">相对文件路径</param>
        /// <returns></returns>
        Stream GetStream(string relativeFilePath);

        #endregion 方法
    }
}