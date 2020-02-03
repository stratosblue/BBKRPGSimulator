using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 投掷群体暗器
    /// </summary>
    internal class ActionThrowItemAll : ActionMultiTarget
    {
        #region 字段

        /// <summary>
        /// 暗器
        /// </summary>
        private GoodsHiddenWeapon _hiddenWeapon;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 投掷群体暗器
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="targets"></param>
        /// <param name="goods"></param>
        public ActionThrowItemAll(SimulatorContext context, FightingCharacter attacker, List<FightingCharacter> targets, GoodsHiddenWeapon goods) : base(context, attacker, targets)
        {
            _hiddenWeapon = goods;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            if (State == CombatAnimationState.Magic)
            {
                Animation.Draw(canvas, 0, 0);
            }
            else if (State == CombatAnimationState.End)
            {
                DrawRaiseAnimation(canvas);
            }
        }

        public override void PreProccess()
        {
            // TODO 记下伤害值、异常状态
            ExecutorX = Executor.GetCombatX();
            ExecutorX = Executor.GetCombatY();
            Animation = _hiddenWeapon.Animation;
            Animation.StartAni();
            Animation.SetIteratorNum(2);
            // TODO effect it
            RaiseAnimations.Add(new RaiseAnimation(Context, 10, 20, 10, 0));
            RaiseAnimations.Add(new RaiseAnimation(Context, 30, 10, 10, 0));
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】群体投掷【{_hiddenWeapon?.Name}】";
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
                            Executor.SetCombatPos(ExecutorX + 2, ExecutorX + 2);
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
                        if (!TargetIsMonster())
                        {
                            foreach (var fc in Targets)
                            {
                                fc.FightingSprite.CurrentFrame = 10;
                            }
                        }
                        else
                        {
                            foreach (var fc in Targets)
                            {
                                fc.FightingSprite.Move(2, 2);
                            }
                        }
                    }
                    break;

                case CombatAnimationState.End:
                    if (!UpdateRaiseAnimation(delta))
                    {
                        if (TargetIsMonster())
                        {
                            foreach (var fc in Targets)
                            {
                                fc.FightingSprite.Move(-2, -2);
                            }
                        }
                        else
                        {
                            foreach (var fc in Targets)
                            {
                                ((PlayerCharacter)fc).SetFrameByState();
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