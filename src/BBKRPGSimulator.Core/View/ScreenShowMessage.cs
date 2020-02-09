using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.View
{
    internal class ScreenShowMessage : BaseScreen
    {
        #region 字段

        private long cnt = 0;
        private long delay;
        private string msg;

        #endregion 字段

        #region 构造函数

        public ScreenShowMessage(SimulatorContext context, string _msg, long _delay) : base(context)
        {
            msg = _msg;
            delay = _delay;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            Context.Util.ShowMessage(canvas, msg);
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            Context.PopScreen();
        }

        public override void OnKeyUp(int key)
        {
        }

        public override void Update(long delta)
        {
            cnt += delta;
            if (cnt > delay)
            {
                Context.PopScreen();
            }
        }

        #endregion 方法
    }
}