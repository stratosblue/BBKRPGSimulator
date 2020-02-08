using System.Diagnostics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 未完成的命令
    /// </summary>
    internal class NotImplementedCommand : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 未完成的命令
        /// </summary>
        public NotImplementedCommand(int length) : base(length, null)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Debug.WriteLine($"{nameof(NotImplementedCommand)}");
            return null;
        }

        #endregion 方法
    }
}