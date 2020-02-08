using System;
using System.Diagnostics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 命令基类
    /// </summary>
    internal abstract class BaseCommand : ContextDependent, ICommand
    {
        #region 构造函数

        /// <summary>
        /// 命令的数据长度
        /// </summary>
        public int Length { get; protected set; }

        protected ArraySegment<byte> Data { get; }

        /// <summary>
        /// 命令基类
        /// </summary>
        /// <param name="length">命令的数据长度</param>
        /// <param name="context"></param>
        public BaseCommand(int length, SimulatorContext context) : base(context)
        {
            Length = length;
        }

        /// <summary>
        /// 命令基类
        /// </summary>
        /// <param name="length">命令的数据长度</param>
        /// <param name="context"></param>
        public BaseCommand(ArraySegment<byte> data, int length, SimulatorContext context) : base(context)
        {
            Length = length;
            Data = data;
        }

        #endregion 构造函数

        #region 方法

        public Operate Process()
        {
            var operate = ProcessAndGetOperate();

            DebugLog(operate);

            return operate;
        }

        /// <summary>
        /// 处理一条指令
        /// </summary>
        /// <returns></returns>
        protected abstract Operate ProcessAndGetOperate();

        [Conditional("DEBUG")]
        private void DebugLog(Operate operate)
        {
            if (operate != null)
            {
                Debug.WriteLine($"{DateTime.Now}: Processed Command - {GetType().Name} - {operate.ToString()}");
            }
            else
            {
                Debug.WriteLine($"{DateTime.Now}: Processed Command - {GetType().Name}");
            }
        }

        #endregion 方法
    }
}