using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 单目标动作
    /// </summary>
    internal abstract class ActionSingleTarget : ActionBase
    {
        #region 字段

        /// <summary>
        /// 漂浮动画
        /// </summary>
        protected RaiseAnimation RaiseAnimation { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        protected FightingCharacter Target { get; set; }

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 单目标动作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        public ActionSingleTarget(SimulatorContext context, FightingCharacter attacker, FightingCharacter target) : base(context)
        {
            Executor = attacker;
            Target = target;
        }

        #endregion 构造函数

        #region 方法

        public override bool IsTargetAlive()
        {
            return Target.IsAlive;
        }

        public override bool IsTargetsMoreThanOne()
        {
            return false;
        }

        public override void PostExecute()
        {
            Target.IsVisiable = Target.IsAlive;
        }

        public void SetTarget(FightingCharacter fc)
        {
            Target = fc;
        }

        public override bool TargetIsMonster()
        {
            return Target is Monster;
        }

        protected override void DrawRaiseAnimation(ICanvas canvas)
        {
            if (RaiseAnimation != null)
            {
                RaiseAnimation.Draw(canvas);
            }
        }

        protected override bool UpdateRaiseAnimation(long delta)
        {
            return RaiseAnimation != null && RaiseAnimation.Update(delta);
        }

        #endregion 方法
    }
}