using BBKRPGSimulator.View;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 存档命令
    /// </summary>
    internal class CommandGameSave : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 存档命令
        /// </summary>
        /// <param name="context"></param>
        public CommandGameSave(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandGameSaveOperate(Context);

        #endregion 方法

        #region 类

        public class CommandGameSaveOperate : Operate
        {
            #region 字段

            private bool _end = false;

            #endregion 字段

            #region 构造函数

            public CommandGameSaveOperate(SimulatorContext context) : base(context)
            {
                _end = false;
                Context.PushScreen(new ScreenSaveLoadGame(Context, SaveLoadOperate.SAVE)
                {
                    CallBack = () => _end = true
                });
            }

            #endregion 构造函数

            #region 方法

            public override bool Update(long delta)
            {
                return !_end;
            }

            #endregion 方法
        }

        #endregion 类
    }
}