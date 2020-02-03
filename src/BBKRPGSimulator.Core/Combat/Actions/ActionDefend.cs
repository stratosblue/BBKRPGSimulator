using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 防御动作
    /// </summary>
    internal class ActionDefend : ActionSingleTarget
    {
        #region 构造函数

        public ActionDefend(SimulatorContext context, FightingCharacter fc) : base(context, fc, null)
        {
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
        }

        public override int GetPriority()
        {
            return base.GetPriority();
        }

        public override bool IsTargetAlive()
        {
            return true;
        }

        public override bool IsTargetsMoreThanOne()
        {
            return false;
        }

        public override void PostExecute()
        {
        }

        public override void PreProccess()
        {
        }

        public override bool TargetIsMonster()
        {
            return true;
        }

        public override string ToString()
        {
            return $"【{Executor.Name}】的防御";
        }

        public override bool Update(long delta)
        {
            return false;
        }

        #endregion 方法
    }
}