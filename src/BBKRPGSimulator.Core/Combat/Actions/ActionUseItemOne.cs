using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 使用单体物品
    /// </summary>
    internal class ActionUseItemOne : ActionSingleTarget
    {
        #region 字段

        /// <summary>
        /// 物品
        /// </summary>
        private BaseGoods _goods;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 使用单体物品
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        /// <param name="goods"></param>
        public ActionUseItemOne(SimulatorContext context, FightingCharacter attacker, FightingCharacter target, BaseGoods goods) : base(context, attacker, target)
        {
            _goods = goods;
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
            // TODO 记下伤害值、异常状态
            int hp = 0;
            if (_goods is GoodsMedicine goodsMedicine)
            {
                Animation = goodsMedicine.Animation;
                hp = Target.HP;
                Target.UseMedicine(_goods);
                hp = Target.HP - hp;
            }
            else
            {
                Animation = Context.LibData.GetSrs(2, 1);
            }
            Animation.StartAni();
            Animation.SetIteratorNum(2);
            TargetX = Target.GetCombatX();
            TargetY = Target.GetCombatY();
            RaiseAnimation = new RaiseAnimation(Context, Target.GetCombatX(), Target.GetCombatTop(), hp, 0);
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】对【{Target.Name}】使用物品【{_goods.Name}】";
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