using System;

namespace BBKRPGSimulator.Lib
{
    /// <summary>
    /// 脚本资源
    /// </summary>
    internal class ResGut : ResBase
    {
        #region 属性

        /// <summary>
        /// 脚本说明
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 脚本长度，字节总数
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// 场景事件个数
        /// </summary>
        public int NumSceneEvent { get; private set; }

        /// <summary>
        /// 场景事件，255个(1-255)。分为NPC事件、地图事件和其他事件。 NPC事件由1到40，与其资源操作号对应；地图事件由41到255，
        /// 即地图编辑器中设置的事件为1，在场景中的事件为1+40=41； 其他事件可用1到255。
        /// </summary>
        public int[] SceneEvent { get; private set; }

        /// <summary>
        /// 脚本，格式为 指令号+数据
        /// </summary>
        public byte[] ScriptData { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 脚本资源
        /// </summary>
        /// <param name="context"></param>
        public ResGut(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void SetData(byte[] buf, int offset)
        {
            Type = buf[offset];
            Index = buf[offset + 1];
            Description = buf.GetString(offset + 2);
            Length = (((int)buf[offset + 0x19] & 0xFF) << 8)
                    | ((int)buf[offset + 0x18] & 0xFF);
            NumSceneEvent = (int)buf[offset + 0x1a] & 0xFF;
            SceneEvent = new int[NumSceneEvent];
            for (int i = 0; i < NumSceneEvent; i++)
            {
                SceneEvent[i] = ((int)buf[offset + (i << 1) + 0x1c] & 0xFF) << 8
                        | ((int)buf[offset + (i << 1) + 0x1b] & 0xFF);
            }
            int len = Length - NumSceneEvent * 2 - 3;
            ScriptData = new byte[len];

            Array.Copy(buf, offset + 0x1b + (NumSceneEvent * 2), ScriptData, 0, len);
        }

        #endregion 方法
    }
}