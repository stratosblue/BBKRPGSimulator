namespace BBKRPGSimulator
{
    /// <summary>
    /// 角色动作状态
    /// </summary>
    internal enum CharacterActionState
    {
        /// <summary>
        /// 停止状态，不作运动驱动
        /// </summary>
        STATE_STOP = 0,

        /// <summary>
        /// 强制移动状态，效果同2
        /// </summary>
        STATE_FORCE_MOVE = 1,

        /// <summary>
        /// 巡逻状态，自由行走
        /// </summary>
        STATE_WALKING = 2,

        /// <summary>
        /// 暂停状态，等到延时到了后转变为巡逻状态
        /// </summary>
        STATE_PAUSE = 3,

        /// <summary>
        /// 激活状态，只换图片，不改变位置（适合动态的场景对象，比如：伏魔灯）
        /// </summary>
        STATE_ACTIVE = 4,
    }
}