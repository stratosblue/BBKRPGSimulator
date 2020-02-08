namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 命令接口
    /// </summary>
    internal interface ICommand
    {
        #region 属性

        /// <summary>
        /// 命令的数据长度
        /// </summary>
        int Length { get; }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 处理一条指令
        /// true继续执行Update&Draw;false指令执行完毕
        /// </summary>
        /// <returns></returns>
        Operate Process();

        #endregion 方法
    }
}