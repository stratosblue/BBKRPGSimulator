using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 单体辅助魔法
    /// </summary>
    internal class ActionMagicHelpOne : ActionSingleTarget
    {
        #region 字段

        /// <summary>
        /// 使用的魔法
        /// </summary>
        private BaseMagic _magic;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 单体辅助魔法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="magic"></param>
        public ActionMagicHelpOne(SimulatorContext context, FightingCharacter attacker, FightingCharacter target, BaseMagic magic) : base(context, attacker, target)
        {
            _magic = magic;
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
            Animation = _magic.Animation;
            Animation.StartAni();
            Animation.SetIteratorNum(2);
            TargetX = Target.GetCombatX();
            TargetY = Target.GetCombatY();

            var oldHp = Target.HP;
            var magicRestore = (MagicRestore)_magic;
            magicRestore.Use(Executor, Target);
            var value = Target.HP - oldHp;

            RaiseAnimation = new RaiseAnimation(Context, Target.GetCombatX(), Target.GetCombatTop(), value, 0);
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】对【{Target.Name}】使用辅助魔法【{_magic.Name}】";
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
                    }
                    break;

                case CombatAnimationState.End:
                    return RaiseAnimation.Update(delta);
                    //			break;
            }
            return true;
        }

        #endregion 方法
    }
}