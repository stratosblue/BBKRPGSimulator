using System;

using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 显示界面命令？
    /// </summary>
    internal class CommandShowScreen : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 显示界面命令？
        /// </summary>
        /// <param name="context"></param>
        public CommandShowScreen(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandShowScreenOperate(Context);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 显示界面命令操作？
        /// </summary>
        private class CommandShowScreenOperate : OperateDrawOnce
        {
            #region 构造函数

            /// <summary>
            /// 显示界面命令操作？
            /// </summary>
            /// <param name="context"></param>
            public CommandShowScreenOperate(SimulatorContext context) : base(context)
            {
            }

            #endregion 构造函数

            #region 方法

            public override void DrawOnce(ICanvas canvas)
            {
                Context.SceneMap.DrawScene(canvas);
            }

            public override bool Process()
            {
                throw new NotImplementedException();
                //return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}