using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 单体魔法攻击
    /// </summary>
    internal class ActionMagicAttackOne : ActionSingleTarget
    {
        #region 字段

        /// <summary>
        /// 使用的魔法
        /// </summary>
        private BaseMagic _magic;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 单体魔法攻击
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="magic"></param>
        public ActionMagicAttackOne(SimulatorContext context, FightingCharacter attacker, FightingCharacter target, BaseMagic magic) : base(context, attacker, target)
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
            ExecutorX = Executor.GetCombatX();
            ExecutorY = Executor.GetCombatY();
            Animation = _magic.Animation;
            Animation.StartAni();
            Animation.SetIteratorNum(2);
            int ohp = Target.HP;
            if (_magic is MagicAttack magicAttack)
            {
                magicAttack.Use(Executor, Target);
            }
            TargetX = Target.GetCombatX();
            TargetY = Target.GetCombatY() - Target.FightingSprite.Height / 2;
            RaiseAnimation = new RaiseAnimation(Context, Target.GetCombatX(), Target.GetCombatY(), Target.HP - ohp, 0/*FightingCharacter.BUFF_MASK_DU*/);

            if (_magic is MagicSpecial)
            {
                //TODO 完成特殊魔法
            }
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】对【{Target.Name}】使用魔法【{_magic.Name}】";
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
                        if (Target is PlayerCharacter playerCharacter)
                        {
                            playerCharacter.SetFrameByState();
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