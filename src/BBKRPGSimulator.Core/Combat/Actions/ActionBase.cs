using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 动作基类
    /// </summary>
    internal abstract class ActionBase : ContextDependent
    {
        #region 字段

        /// <summary>
        /// 时间总长度
        /// </summary>
        public const int DELTA = 1000 / 20;

        /// <summary>
        /// 当前帧
        /// </summary>
        protected int _currentFrame = 0;

        /// <summary>
        /// 动画状态
        /// </summary>
        protected CombatAnimationState State = CombatAnimationState.Start;

        /// <summary>
        /// 时间长度
        /// </summary>
        private long _timeCount = 0;

        /// <summary>
        /// 动画
        /// </summary>
        protected ResSrs Animation { get; set; }

        /// <summary>
        /// 动作的执行者
        /// </summary>
        protected FightingCharacter Executor { get; set; }

        /// <summary>
        /// 执行者X坐标
        /// </summary>
        protected int ExecutorX { get; set; }

        /// <summary>
        /// 执行者Y坐标
        /// </summary>
        protected int ExecutorY { get; set; }

        /// <summary>
        /// 目标X坐标
        /// </summary>
        protected int TargetX { get; set; }

        /// <summary>
        /// 目标Y坐标
        /// </summary>
        protected int TargetY { get; set; }

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 动作基类
        /// </summary>
        /// <param name="context"></param>
        public ActionBase(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public virtual void Draw(ICanvas canvas)
        {
        }

        /// <summary>
        /// 动作发起者的身法
        /// </summary>
        /// <returns></returns>
        public virtual int GetPriority()
        {
            return Executor.Speed;
        }

        public virtual bool IsAttackerAlive()
        {
            return Executor.IsAlive;
        }

        public abstract bool IsTargetAlive();

        public abstract bool IsTargetsMoreThanOne();

        /// <summary>
        /// 隐藏死亡角色
        /// </summary>
        public abstract void PostExecute();

        /// <summary>
        /// 动作产生的影响，播放动作动画之前执行一次。
        /// </summary>
        public virtual void PreProccess()
        {
        }

        public abstract bool TargetIsMonster();

        public override string ToString()
        {
            return $"{Executor.Name} 执行的动作";
        }

        /// <summary>
        /// 执行完毕返回false否则返回true
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public virtual bool Update(long delta)
        {
            _timeCount += delta;
            if (_timeCount >= DELTA)
            {
                ++_currentFrame;
                _timeCount = 0;
            }
            return true;
        }

        protected abstract void DrawRaiseAnimation(ICanvas canvas);

        protected abstract bool UpdateRaiseAnimation(long delta);

        #endregion 方法
    }
}