using System;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 逃跑动作
    /// </summary>
    internal class ActionFlee : ActionBase
    {
        #region 字段

        /// <summary>
        /// 逃跑成功的callback
        /// </summary>
        private readonly Action _callback;

        /// <summary>
        /// 是否逃跑成功
        /// </summary>
        private readonly bool _fleeSucceed;

        /// <summary>
        /// 执行操作的角色
        /// </summary>
        private PlayerCharacter _character;

        /// <summary>
        /// 每帧的Y轴移动距离
        /// </summary>
        private int _frameOffsetY;

        private int FRAME_CNT = 5;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 逃跑动作
        /// </summary>
        /// <param name="context"></param>
        /// <param name="character">角色</param>
        /// <param name="fleeSuc">是否逃跑成功</param>
        /// <param name="callback">逃跑成功后的CallBack</param>
        public ActionFlee(SimulatorContext context, PlayerCharacter character, bool fleeSuc, Action callback) : base(context)
        {
            _character = character;
            _fleeSucceed = fleeSuc;
            _callback = callback;
        }

        #endregion 构造函数

        #region 方法

        public override int GetPriority()
        {
            return _character.Speed;
        }

        public override bool IsAttackerAlive()
        {
            return true;
        }

        public override bool IsTargetAlive()
        {
            return false;
        }

        public override bool IsTargetsMoreThanOne()
        {
            return false;
        }

        public override void PostExecute()
        {
            if (_fleeSucceed)
            {
                _callback?.Invoke();
            }
            else
            {
                _character.SetCombatPos(ExecutorX, ExecutorY);
            }
        }

        public override void PreProccess()
        {
            //HACK TODO calc the pos
            ExecutorX = _character.GetCombatX();
            ExecutorY = _character.GetCombatY();
            _frameOffsetY = (96 - ExecutorY) / FRAME_CNT;
            _character.FightingSprite.CurrentFrame = 1;
        }

        public override bool TargetIsMonster()
        {
            return false;
        }

        public override string ToString()
        {
            return $"【{_character.Name}】的逃跑动作";
        }

        public override bool Update(long delta)
        {
            base.Update(delta);
            if (_currentFrame < FRAME_CNT)
            {
                _character.SetCombatPos(ExecutorX, ExecutorY + _frameOffsetY * _currentFrame);
                return true;
            }
            else if (!_fleeSucceed && _currentFrame < FRAME_CNT + 2)
            {
                _character.SetCombatPos(ExecutorX, ExecutorY);
                _character.FightingSprite.CurrentFrame = 11;
            }
            else if (!_fleeSucceed)
            {
                _character.SetFrameByState();
            }
            return false;
        }

        protected override void DrawRaiseAnimation(ICanvas canvas)
        {
        }

        protected override bool UpdateRaiseAnimation(long delta)
        {
            return false;
        }

        #endregion 方法
    }
}