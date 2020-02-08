using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BBKRPGSimulator
{
    /// <summary>
    /// 拓展类
    /// </summary>
    internal static class ExtensionFunction
    {
        #region 字段

        /// <summary>
        /// GB2312的编码
        /// </summary>
        public readonly static Encoding GB2312Encoding = null;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 拓展类
        /// </summary>
        static ExtensionFunction()
        {
            GB2312Encoding = Encoding.GetEncoding("GB2312");
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 拓展的System.Drawing.Point类的比较方法
        /// </summary>
        /// <param name="point">对象</param>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool EqualsLocation(this Point point, int x, int y)
        {
            return point.X == x && point.Y == y;
        }

        /// <summary>
        /// 获取字符串GB2312编码的byte数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static byte[] GetBytes(this string str)
        {
            return GB2312Encoding.GetBytes(str);
        }

        /// <summary>
        /// 获取字符串指定编码的byte数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encodingName">编码名称字符串</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static byte[] GetBytes(this string str, string encodingName)
        {
            return Encoding.GetEncoding(encodingName).GetBytes(str);
        }

        #endregion 方法

        #region 为List<T>添加的栈、队列操作操作

        /// <summary>
        /// 获取并移除最后一个对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T Dequeue<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }
            T result = default(T);
            lock (list)
            {
                var index = list.Count - 1;
                result = list[index];
                list.RemoveAt(index);
            }
            return result;
        }

        /// <summary>
        /// 返回最后一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T Peek<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        /// <summary>
        /// 移除并返回最后一个对象
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T Pop<T>(this List<T> list)
        {
            //HACK 这里去除了list的同步锁
            T result = list[list.Count - 1];
            list.Remove(result);
            return result;
        }

        /// <summary>
        /// 在最后追加一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        [DebuggerStepThrough]
        public static void Push<T>(this List<T> list, T item)
        {
            list.Add(item);
        }

        #endregion 为List<T>添加的栈、队列操作操作

        #region 二维数组的创建

        /// <summary>
        /// 二维byte数组
        /// </summary>
        /// <param name="dimension1">维度1</param>
        /// <param name="dimension2">维度2</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static byte[][] DyadicArrayByte(int dimension1, int dimension2)
        {
            byte[][] returnByte = new byte[dimension1][];
            for (int i = 0; i < dimension1; i++)
            {
                returnByte[i] = new byte[dimension2];
            }
            return returnByte;
        }

        #endregion 二维数组的创建

        #region Bytes取值操作

        /// <summary>
        /// 检查当前数组长度，如果符合则返回当前数组
        /// 否则返回一个新的字符串字节数组，长度为指定长度
        /// 过长部分直接截断，不足则填充空格
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static byte[] FixStringLength(this byte[] data, int length)
        {
            byte[] result = null;

            if (data.Length > length)
            {
                result = new byte[length];
                Array.Copy(data, 0, result, 0, length);
            }
            else if (data.Length > length)
            {
                result = new byte[length];
                Array.Copy(data, result, data.Length);

                for (int i = data.Length; i < length; i++)  //填充空白
                {
                    result[i] = 32;
                }
            }
            else
            {
                result = data;
            }

            return result;
        }

        /// <summary>
        /// 读取一字节有符号整型
        /// </summary>
        /// <param name="data">资源缓冲区</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get1ByteInt(this byte[] data, int offset)
        {
            int i = data[offset] & 0x7f;
            if ((data[offset] & 0x80) != 0)
            {
                return -i;
            }
            return i;
        }

        /// <summary>
        /// 读取一字节有符号整型
        /// </summary>
        /// <param name="data">资源缓冲区</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get1ByteInt(this ArraySegment<byte> data, int offset) => data.Array.Get1ByteInt(data.Offset + offset);

        /// <summary>
        /// 读取两字节有符号整型
        /// </summary>
        /// <param name="data">资源缓冲区</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get2BytesInt(this byte[] data, int offset)
        {
            int i = data[offset] & 0xFF | data[offset + 1] << 8 & 0x7F00;
            if ((data[offset + 1] & 0x80) != 0)
            {
                return -i;
            }
            return i;
        }

        /// <summary>
        /// 读取两字节有符号整型
        /// </summary>
        /// <param name="data">资源缓冲区</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get2BytesInt(this ArraySegment<byte> data, int offset) => data.Array.Get2BytesInt(data.Offset + offset);

        /// <summary>
        /// 读取两字节无符号整型
        /// </summary>
        /// <param name="data">资源缓冲区</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get2BytesUInt(this byte[] data, int offset)
        {
            return data[offset] & 0xFF | data[offset + 1] << 8 & 0xFF00;
        }

        /// <summary>
        /// 读取两字节无符号整型
        /// </summary>
        /// <param name="data">资源缓冲区</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get2BytesUInt(this ArraySegment<byte> data, int offset) => data.Array.Get2BytesUInt(data.Offset + offset);

        /// <summary>
        /// 获取四字节Int
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get4BytesInt(this byte[] data, int offset)
        {
            return data[offset] & 0xFF |
                data[offset + 1] << 8 & 0xFF00 |
                data[offset + 2] << 16 & 0xFF0000 |
                data[offset + 3] << 24;
        }

        /// <summary>
        /// 获取四字节Int
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get4BytesInt(this ArraySegment<byte> data, int offset) => data.Array.Get4BytesInt(data.Offset + offset);

        /// <summary>
        /// 获得GBK编码的字符串
        /// </summary>
        /// <param name="data">资源缓冲区</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetString(this byte[] data, int offset)
        {
            byte[] strbyte = data.GetStringBytes(offset);

            return GB2312Encoding.GetString(strbyte).TrimEnd('\0');
        }

        /// <summary>
        /// 获得GBK编码的字符串
        /// </summary>
        /// <param name="data">资源缓冲区</param>
        /// <param name="offset">起始位置</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetString(this ArraySegment<byte> data, int offset) => data.Array.GetString(data.Offset + offset);

        /// <summary>
        /// 获取字符串字节信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetStringBytes(this byte[] data, int offset)
        {
            int i = 0;
            while (data[offset + i] != 0)
            {
                ++i;
            }

            byte[] result = new byte[++i];
            Array.Copy(data, offset, result, 0, i);
            return result;
        }

        /// <summary>
        /// 获取字符串字节信息
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetStringBytes(this ArraySegment<byte> data, int offset) => data.Array.GetStringBytes(data.Offset + offset);

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetStringLength(this byte[] data, int offset)
        {
            int i = 0;
            while (data[offset + i] != 0) ++i;

            return ++i;
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetStringLength(this ArraySegment<byte> data, int offset) => data.Array.GetStringLength(data.Offset + offset);

        #endregion Bytes取值操作

        #region 其它

        /// <summary>
        /// 构造动态构造委托
        /// 需要自己明确调用参数
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <returns></returns>
        public static Delegate BuildDynamicConstructorDelegate(this ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null)
            {
                throw new ArgumentNullException("constructorInfo");
            }

            var paramExpressions = constructorInfo.GetParameters().Select(param =>
            {
                return Expression.Parameter(param.ParameterType, param.Name);
            }).ToArray();

            var constructorExpression = Expression.New(constructorInfo, paramExpressions);

            var lambdaExpression = Expression.Lambda(constructorExpression, paramExpressions);
            return lambdaExpression.Compile();
        }

        #endregion 其它
    }
}