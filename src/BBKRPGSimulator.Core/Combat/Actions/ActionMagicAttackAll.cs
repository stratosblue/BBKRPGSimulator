using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 魔法攻击全部
    /// </summary>
    internal class ActionMagicAttackAll : ActionMultiTarget
    {
        #region 字段

        /// <summary>
        /// 使用的魔法
        /// </summary>
        private MagicAttack _magic;

        #endregion 字段

        #region 构造函数

        public ActionMagicAttackAll(SimulatorContext context, FightingCharacter attacker, List<FightingCharacter> targets, MagicAttack magic) : base(context, attacker, targets)
        {
            _magic = magic;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            base.Draw(canvas);
            if (State == CombatAnimationState.Magic)
            {
                Animation.Draw(canvas, 0, 0);
            }
            else if (State == CombatAnimationState.End)
            {
                DrawRaiseAnimation(canvas);
            }
        }

        public override int GetPriority()
        {
            return base.GetPriority();
        }

        public override void PreProccess()
        {
            ExecutorX = Executor.GetCombatX();
            ExecutorY = Executor.GetCombatY();
            Animation = _magic.Animation;
            Animation.StartAni();
            Animation.SetIteratorNum(2);
            _magic.Use(Executor, Targets);
            RaiseAnimations.Add(new RaiseAnimation(Context, 10, 10, 10, 0));
            RaiseAnimations.Add(new RaiseAnimation(Context, 30, 10, 20, 0/*FightingCharacter.BUFF_MASK_DU*/));
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】的群体攻击魔法【{_magic?.Name}】";
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
                        if (Targets[0] is PlayerCharacter)
                        {
                            foreach (var item in Targets)
                            {
                                item.FightingSprite.CurrentFrame = 10;
                            }
                        }
                        else
                        {
                            foreach (var item in Targets)
                            {
                                item.FightingSprite.Move(2, 2);
                            }
                        }
                    }
                    break;

                case CombatAnimationState.End:
                    if (!UpdateRaiseAnimation(delta))
                    {
                        if (Targets[0] is PlayerCharacter)
                        {
                            foreach (var item in Targets)
                            {
                                ((PlayerCharacter)item).SetFrameByState();
                            }
                        }
                        else
                        {
                            foreach (var item in Targets)
                            {
                                item.FightingSprite.Move(-2, -2);
                            }
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