using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 群体物理攻击
    /// </summary>
    internal class ActionPhysicalAttackAll : ActionMultiTarget
    {
        #region 字段

        private int buffRound;
        private float dx, dy;
        private int ox, oy;
        private int TOTAL_FRAME = 5;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 群体物理攻击
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="targets"></param>
        public ActionPhysicalAttackAll(SimulatorContext context, FightingCharacter attacker, List<FightingCharacter> targets) : base(context, attacker, targets)
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
            dx = (44.0f - Executor.GetCombatX()) / TOTAL_FRAME;
            dy = (14.0f - Executor.GetCombatY()) / TOTAL_FRAME;
            for (int i = 0; i < Targets.Count; i++)
            {
                FightingCharacter fc = Targets[i];
                if (!fc.IsAlive)
                {
                    continue;
                }
                damage = Executor.Attack - fc.Defend;
                if (damage <= 0)
                {
                    damage = 1;
                }
                damage += (int)(Context.Random.NextDouble() * 3);
                fc.HP -= damage;
                RaiseAnimations.Add(new RaiseAnimation(Context, Targets[i].GetCombatX(), Targets[i].GetCombatY(), -damage, 0));
            }
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】的群体物理攻击";
        }

        public override bool Update(long delta)
        {
            base.Update(delta);
            if (_currentFrame < TOTAL_FRAME)    //发起动作
            {
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
            else if (_currentFrame > TOTAL_FRAME)   //扣血、异常状态的动画
            {
                return UpdateRaiseAnimation(delta);
            }
            else
            {
                Executor.SetCombatPos(ox, oy);
                if (Executor is Monster)
                {
                    FightingSprite fs = Executor.FightingSprite;
                    fs.CurrentFrame = 1;
                }
                else if (Executor is PlayerCharacter)
                {
                    FightingSprite fs = Executor.FightingSprite;
                    fs.CurrentFrame = 1;
                    // TODO the old state 眠
                }
            }
            return true;
        }

        #endregion 方法
    }
}