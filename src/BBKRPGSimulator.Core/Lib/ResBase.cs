using System;

using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.Lib
{
    /// <summary>
    /// 资源对象
    /// 每个资源对象都要使用SetData填充数据
    /// </summary>
    internal abstract class ResBase : ContextDependent, IEquatable<ResBase>
    {
        #region 属性

        /// <summary>
        /// 资源索引值
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// 文本呈现器
        /// </summary>
        public TextRender TextRender => Context.TextRender;

        /// <summary>
        /// 资源类型
        /// </summary>
        public int Type { get; protected set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 资源对象
        /// 每个资源对象都要使用SetData填充数据
        /// </summary>
        /// <param name="context"></param>
        public ResBase(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 每个资源对象都必须调用该方法填充各个字段
        /// </summary>
        /// <param name="buf">资源的数据缓冲区</param>
        /// <param name="offset">该资源在数组buf中的偏移位置，offset为该资源的首字节</param>
        public abstract void SetData(byte[] buf, int offset);

        #region 重写Equals

        /// <summary>
        /// 判断两个物品是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => Equals(obj as ResBase);

        /// <summary>
        /// 判断两个物品是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ResBase other)
        {
            return other != null
                   && Index == other.Index
                   && Type == other.Type;
        }

        /// <summary>
        /// 物品id与类型的Hash Code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            var hashCode = 1377214832;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }

        #endregion 重写Equals

        #endregion 方法
    }
}