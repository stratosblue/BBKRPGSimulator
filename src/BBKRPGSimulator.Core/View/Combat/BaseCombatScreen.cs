using System;

using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.View.Combat
{
    internal class BaseCombatScreen : BaseScreen
    {
        #region 属性

        protected CombatUI _combatUI { get; set; }

        #endregion 属性

        #region 构造函数

        public BaseCombatScreen(SimulatorContext context, CombatUI combatUI) : base(context)
        {
            _combatUI = combatUI;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            throw new NotImplementedException();
        }

        public override void OnKeyDown(int key)
        {
            throw new NotImplementedException();
        }

        public override void OnKeyUp(int key)
        {
            throw new NotImplementedException();
        }

        public override void Update(long delta)
        {
            throw new NotImplementedException();
        }

        #endregion 方法
    }
}