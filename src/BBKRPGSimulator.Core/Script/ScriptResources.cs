using System.IO;

using BBKRPGSimulator.Interface;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 脚本资源
    /// </summary>
    internal class ScriptResources : ICustomSerializeable
    {
        #region 属性

        /// <summary>
        /// 全局事件标志1-2400
        /// </summary>
        public bool[] GlobalEvents { get; private set; } = new bool[2401];

        /// <summary>
        /// 全局变量0-199,局部变量200-239
        /// </summary>
        public int[] Variables { get; private set; } = new int[240];

        #endregion 属性

        #region 方法

        /// <summary>
        ///  将全局事件index标志设置为false
        /// </summary>
        /// <param name="index">1-2400</param>
        public void ClearEvent(int index)
        {
            GlobalEvents[index] = false;
        }

        public void Deserialize(BinaryReader binaryReader)
        {
            // 读全局事件
            for (int i = 1; i <= 2400; ++i)
            {
                GlobalEvents[i] = binaryReader.ReadBoolean();
            }

            // 读全局变量&局部变量
            for (int i = 0; i < 240; ++i)
            {
                Variables[i] = binaryReader.ReadInt32();
            }
        }

        /// <summary>
        /// 初始化全局事件
        /// </summary>
        public void InitGlobalEvents()
        {
            for (int i = 1; i <= 2400; i++)
            {
                GlobalEvents[i] = false;
            }
        }

        /// <summary>
        /// 初始化全局变量
        /// </summary>
        public void InitGlobalVar()
        {
            for (int i = 0; i < 200; i++)
            {
                Variables[i] = 0;
            }
        }

        /// <summary>
        /// 初始化局部变量
        /// </summary>
        public void InitLocalVar()
        {
            for (int i = 200; i < 240; i++)
            {
                Variables[i] = 0;
            }
        }

        public void Serialize(BinaryWriter binaryWriter)
        {
            // 写全局事件
            for (int i = 1; i <= 2400; ++i)
            {
                binaryWriter.Write(GlobalEvents[i]);
            }
            // 写全局变量&局部变量
            for (int i = 0; i < 240; ++i)
            {
                binaryWriter.Write(Variables[i]);
            }
        }

        /// <summary>
        ///  将全局事件index标志设置为true
        /// </summary>
        /// <param name="index">1-2400</param>
        public void SetEvent(int index)
        {
            GlobalEvents[index] = true;
        }

        #endregion 方法
    }
}