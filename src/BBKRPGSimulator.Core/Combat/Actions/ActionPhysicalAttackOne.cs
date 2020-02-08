using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 单体物理攻击
    /// </summary>
    internal class ActionPhysicalAttackOne : ActionSingleTarget
    {
        #region 字段

        private int buffRound;
        private float dx, dy;
        private bool mTotalMark = true;
        private int ox, oy;
        private int TOTAL_FRAME = 5;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 单体物理攻击
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="target"></param>
        public ActionPhysicalAttackOne(SimulatorContext context, FightingCharacter attacker, FightingCharacter target) : base(context, attacker, target)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            if (_currentFrame >= TOTAL_FRAME)
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
            // TODO 记下伤害值、异常状态
            int damage;
            ox = Executor.GetCombatX();
            oy = Executor.GetCombatY();
            dx = (float)(Target.GetCombatX() - Executor.GetCombatX()) / TOTAL_FRAME;
            dy = (float)(Target.GetCombatY() - Executor.GetCombatY()) / TOTAL_FRAME;
            damage = Executor.Attack - Target.Defend;
            if (damage <= 0)
            {
                damage = 1;
            }
            //if (_attacker is PlayerCharacter)
            //{
            //    damage *= 10;
            //}
            damage += (int)(Context.Random.NextDouble() * 10);
            Target.HP = Target.HP - damage;
            RaiseAnimation = new RaiseAnimation(Context, Target.GetCombatLeft(), Target.GetCombatTop(), -damage, 0);
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】的物理单体攻击";
        }

        public override bool Update(long delta)
        {
            base.Update(delta);
            if (_currentFrame < TOTAL_FRAME)
            { // 发起动作
                Executor.SetCombatPos((int)(ox + dx * _currentFrame), (int)(oy + dy * _currentFrame));
                if (Executor is Monster)
                {
                    FightingSprite fs = Executor.FightingSprite;
                    fs.CurrentFrame = fs.FrameCnt * _currentFrame / TOTAL_FRAME + 1;
                }
                else if (Executor is PlayerCharacter)
                {
                    FightingSprite fs = Executor.FightingSprite;
                    fs.CurrentFrame = 5 * _currentFrame / TOTAL_FRAME + 1;
                }
            }
            else if (_currentFrame > TOTAL_FRAME)
            { // 扣血、异常状态的动画
                if (!UpdateRaiseAnimation(delta))
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
            }
            else if (mTotalMark)
            {
                mTotalMark = false;
                Executor.SetCombatPos(ox, oy);
                if (Executor is Monster)
                {
                    FightingSprite fs = Executor.FightingSprite;
                    fs.CurrentFrame = 1;
                }
                else if (Executor is PlayerCharacter)
                {
                    ((PlayerCharacter)Executor).SetFrameByState();
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
            return true;
        }

        #endregion 方法
    }
}