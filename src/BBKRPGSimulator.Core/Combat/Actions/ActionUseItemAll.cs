using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 使用群体物品
    /// </summary>
    internal class ActionUseItemAll : ActionMultiTarget
    {
        #region 字段

        /// <summary>
        /// 物品
        /// </summary>
        private BaseGoods _goods;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 使用群体物品
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="targets"></param>
        /// <param name="goods"></param>
        public ActionUseItemAll(SimulatorContext context, FightingCharacter attacker, List<FightingCharacter> targets, BaseGoods goods) : base(context, attacker, targets)
        {
            _goods = goods;
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

        public override int GetPriority()
        {
            return base.GetPriority();
        }

        public override void PreProccess()
        {
            ExecutorX = Executor.GetCombatX();
            ExecutorY = Executor.GetCombatY();
            //		mAni = magic.getMagicAni(); TODO null fix
            Animation.StartAni();
            Animation.SetIteratorNum(2);
            RaiseAnimations.Add(new RaiseAnimation(Context, 10, 20, 10, 0));
            RaiseAnimations.Add(new RaiseAnimation(Context, 30, 10, 10, 0));
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】群体使用【{_goods.Name}】";
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
                    return UpdateRaiseAnimation(delta);
                    //			break;
            }
            return true;
        }

        #endregion 方法
    }
}