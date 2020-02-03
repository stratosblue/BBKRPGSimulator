namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 命令接口
    /// </summary>
    internal interface ICommand
    {
        #region 方法

        /// <summary>
        /// 得到下一条指令的位置
        /// 小于0结束，大于0为下一条指令的位置
        /// </summary>
        /// <param name="code">指令缓冲区</param>
        /// <param name="start">要执行的指令的数据开始位置</param>
        /// <returns>小于0结束，大于0为下一条指令的位置</returns>
        int GetNextPos(byte[] code, int start);

        /// <summary>
        /// 获取操作
        /// </summary>
        /// <param name="code">指令缓冲区</param>
        /// <param name="start">要执行的指令的数据开始位置</param>
        /// <returns>操作</returns>
        Operate GetOperate(byte[] code, int start);

        #endregion 方法
    }
}