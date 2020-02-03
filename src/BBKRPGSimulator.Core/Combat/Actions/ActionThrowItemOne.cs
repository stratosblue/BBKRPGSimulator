using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 单体投掷
    /// </summary>
    internal class ActionThrowItemOne : ActionSingleTarget
    {
        #region 字段

        /// <summary>
        /// 暗器
        /// </summary>
        private GoodsHiddenWeapon _hiddenWeapon;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 单体投掷
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="hiddenWeapon"></param>
        public ActionThrowItemOne(SimulatorContext context, FightingCharacter attacker, FightingCharacter target, GoodsHiddenWeapon hiddenWeapon) : base(context, attacker, target)
        {
            _hiddenWeapon = hiddenWeapon;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            if (State == CombatAnimationState.Magic)
            {
                Animation.DrawAbsolutely(canvas, TargetX, TargetY);
            }
            else if (State == CombatAnimationState.End)
            {
                RaiseAnimation.Draw(canvas);
            }
        }

        public override void PreProccess()
        {
            // TODO 记下伤害值、异常状态 there is null pointer
            ExecutorX = Executor.GetCombatX();
            ExecutorY = Executor.GetCombatY();
            Animation = _hiddenWeapon.Animation;
            Animation.StartAni();
            Animation.SetIteratorNum(2);
            // TODO effect it
            TargetX = Target.GetCombatX();
            TargetY = Target.GetCombatY();
            RaiseAnimation = new RaiseAnimation(Context, TargetX, Target.GetCombatTop(), 10, 0);
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】对【{Target.Name}】投掷【{_hiddenWeapon.Name}】";
        }

        public override bool Update(long delta)
        {
            base.Update(delta);
            switch (State)
            {
                case CombatAnimationState.Start:
                    if (_currentFrame < 10)
                    {
                        if (Executor is PlayerCharacter)
                        {
                            Executor.FightingSprite.CurrentFrame = _currentFrame * 3 / 10 + 6;
                        }
                        else
                        {
                            Executor.SetCombatPos(ExecutorX + 2, ExecutorY + 2);
                        }
                    }
                    else
                    {
                        State = CombatAnimationState.Magic;
                    }
                    break;

                case CombatAnimationState.Magic:
                    if (!Animation.Update(delta))
                    {
                        State = CombatAnimationState.End;
                        if (Executor is PlayerCharacter)
                        {
                            ((PlayerCharacter)Executor).SetFrameByState();
                        }
                        else
                        {
                            Executor.FightingSprite.Move(-2, -2);
                        }
                        if (Target is PlayerCharacter)
                        {
                            Target.FightingSprite.CurrentFrame = 10;
                        }
                        else
                        {
                            Target.FightingSprite.Move(2, 2);
                        }
                    }
                    break;

                case CombatAnimationState.End:
                    if (!RaiseAnimation.Update(delta))
                    {
                        if (Target is PlayerCharacter)
                        {
                            ((PlayerCharacter)Target).SetFrameByState();
                        }
                        else
                        {
                            Target.FightingSprite.Move(-2, -2);
                        }
                        return false;
                    }
                    break;
            }
            return true;
        }

        #endregion 方法
    }
}