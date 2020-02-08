using System.IO;

namespace BBKRPGSimulator
{
    /// <summary>
    /// 基础目录流提供器
    /// </summary>
    public class BaseDirectoryStreamProvider : IStreamProvider
    {
        #region 属性

        /// <summary>
        /// 基础路径
        /// </summary>
        public string BasePath { get; }

        #endregion 属性

        #region 构造函数

        public BaseDirectoryStreamProvider(string basePath)
        {
            BasePath = basePath;
        }

        #endregion 构造函数

        #region 方法

        public Stream GetOrCreateStream(string relativeFilePath)
        {
            var path = Path.Combine(BasePath, relativeFilePath);
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            return File.Open(path, FileMode.OpenOrCreate);
        }

        public Stream GetStream(string relativeFilePath)
        {
            var path = Path.Combine(BasePath, relativeFilePath);
            if (File.Exists(path))
            {
                return File.Open(path, FileMode.OpenOrCreate);
            }
            return null;
        }

        #endregion 方法
    }
}