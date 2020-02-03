using System.IO;

namespace BBKRPGSimulator
{
    /// <summary>
    /// 相关工具
    /// </summary>
    public static class Utilities
    {
        #region 方法

        /// <summary>
        /// 获取游戏名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetGameName(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return GetGameName(stream);
            }
        }

        /// <summary>
        /// 获取游戏名称
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetGameName(Stream stream)
        {
            var buffer = new byte[0x10];
            stream.Read(buffer, 0, 0x10);
            return GetGameName(buffer);
        }

        /// <summary>
        /// 获取游戏名称
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetGameName(byte[] data)
        {
            if (IsGame(data))
            {
                return data.GetString(3);
            }
            else
            {
                throw new InvalidDataException("数据并非游戏LIB");
            }
        }

        /// <summary>
        /// 是否是游戏文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsGame(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                return IsGame(stream);
            }
        }

        /// <summary>
        /// 是否是游戏文件
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool IsGame(Stream stream)
        {
            var buffer = new byte[3];
            stream.Read(buffer, 0, 3);
            return IsGame(buffer);
        }

        /// <summary>
        /// 是否是游戏文件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsGame(byte[] data)
        {
            return (data[0] == 76 || data[0] == 108) &&
                (data[1] == 73 || data[1] == 105) &&
                (data[2] == 66 || data[2] == 98);
        }

        #endregion 方法
    }
}