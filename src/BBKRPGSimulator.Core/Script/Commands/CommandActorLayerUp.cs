using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 角色面板升级？
    /// </summary>
    internal class CommandActorLayerUp : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 角色面板升级？
        /// </summary>
        /// <param name="context"></param>
        public CommandActorLayerUp(SimulatorContext context) : base(4, context)
        { }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            return new CommandActorLayerUpOperate(Context);
        }

        #endregion 方法

        #region 类

        public class CommandActorLayerUpOperate : Operate
        {
            #region 字段

            private bool _exit = false;

            #endregion 字段

            #region 属性

            private TextRender TextRender => Context.TextRender;

            #endregion 属性

            #region 构造函数

            public CommandActorLayerUpOperate(SimulatorContext context) : base(context)
            {
                _exit = false;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                TextRender.DrawText(canvas, "cmd_actorlayerup", 10, 20);
                TextRender.DrawText(canvas, "press cancel to continue", 0, 40);
            }

            public override void OnKeyUp(int key)
            {
                if (key == SimulatorKeys.KEY_CANCEL)
                {
                    _exit = true;
                }
            }

            public override bool Update(long delta)
            {
                return !_exit;
            }

            #endregion 方法
        }

        #endregion 类
    }
}