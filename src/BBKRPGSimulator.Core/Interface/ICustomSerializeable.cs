using System.IO;

namespace BBKRPGSimulator.Interface
{
    /// <summary>
    /// 可自定义序列化对象接口
    /// </summary>
    internal interface ICustomSerializeable
    {
        #region 方法

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="binaryReader"></param>
        void Deserialize(BinaryReader binaryReader);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        void Serialize(BinaryWriter binaryWriter);

        #endregion 方法
    }
}