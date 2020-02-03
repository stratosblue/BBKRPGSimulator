namespace BBKRPGSimulator
{
    /// <summary>
    /// 战斗状态
    /// </summary>
    internal enum CombatState
    {
        /// <summary>
        /// 玩家操作阶段，制定攻击策略
        /// </summary>
        SelectAction,

        /// <summary>
        /// 执行动作队列，播放攻击动画
        /// </summary>
        PerformAction,

        /// 赢得战斗
        /// </summary>
        Win,

        /// <summary>
        /// 战斗失败
        /// </summary>
        Loss,

        /// <summary>
        /// 逃跑
        /// </summary>
        Exit
    }
}