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
        public CommandActorLayerUp(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 4;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandActorLayerUpOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 角色面板升级操作？
        /// </summary>
        private class CommandActorLayerUpOperate : Operate
        {
            #region 字段

            private readonly byte[] _code;
            private readonly int _start;
            private bool _exit = false;

            #endregion 字段

            #region 属性

            private TextRender TextRender => Context.TextRender;

            #endregion 属性

            #region 构造函数

            /// <summary>
            /// 角色面板升级操作？
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandActorLayerUpOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                TextRender.DrawText(canvas, "cmd_actorlayerup", 10, 20);
                TextRender.DrawText(canvas, "press cancel to continue", 0, 40);
            }

            public override void OnKeyDown(int key)
            {
            }

            public override void OnKeyUp(int key)
            {
                if (key == SimulatorKeys.KEY_CANCEL)
                {
                    _exit = true;
                }
            }

            public override bool Process()
            {
                return true;
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