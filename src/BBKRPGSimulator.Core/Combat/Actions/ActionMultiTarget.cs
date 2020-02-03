using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 多目标动作
    /// </summary>
    internal class ActionMultiTarget : ActionBase
    {
        #region 字段

        /// <summary>
        /// 漂浮动画列表
        /// </summary>
        protected List<RaiseAnimation> RaiseAnimations { get; set; }

        /// <summary>
        /// 目标集合
        /// </summary>
        protected List<FightingCharacter> Targets { get; set; }

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 多目标动作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attacker"></param>
        /// <param name="targets"></param>
        public ActionMultiTarget(SimulatorContext context, FightingCharacter attacker, List<FightingCharacter> targets) : base(context)
        {
            Executor = attacker;
            Targets = new List<FightingCharacter>();
            Targets.AddRange(targets);
            RaiseAnimations = new List<RaiseAnimation>();
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
        }

        public override bool IsTargetAlive()
        {
            foreach (var item in Targets)
            {
                if (item.IsAlive)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool IsTargetsMoreThanOne()
        {
            return false;
        }

        public override void PostExecute()
        {
            if (Targets != null)
            {
                foreach (var item in Targets)
                {
                    item.IsVisiable = item.IsAlive;
                }
            }
        }

        public override bool TargetIsMonster()
        {
            return Targets[0] is Monster;
        }

        protected override void DrawRaiseAnimation(ICanvas canvas)
        {
            if (RaiseAnimations != null)
            {
                foreach (var item in RaiseAnimations)
                {
                    item.Draw(canvas);
                }
            }
        }

        protected override bool UpdateRaiseAnimation(long delta)
        {
            if (RaiseAnimations != null) //全体
            {
                if (RaiseAnimations.Count == 0)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < RaiseAnimations.Count; i++)
                    {
                        if (!RaiseAnimations[i].Update(delta))
                        {
                            RaiseAnimations.RemoveAt(i);
                            if (RaiseAnimations.Count <= 0)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        #endregion 方法
    }
}