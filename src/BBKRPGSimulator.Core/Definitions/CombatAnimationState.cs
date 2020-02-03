namespace BBKRPGSimulator
{
    /// <summary>
    /// 战斗动作动画状态
    /// </summary>
    internal enum CombatAnimationState
    {
        /// <summary>
        /// 移位动画
        /// </summary>
        Move = 0,

        /// <summary>
        /// 起手动画
        /// </summary>
        Start = 1,

        /// <summary>
        /// 魔法动画
        /// </summary>
        Magic = 2,

        /// <summary>
        /// 结束动画
        /// </summary>
        End = 3,
    }
}