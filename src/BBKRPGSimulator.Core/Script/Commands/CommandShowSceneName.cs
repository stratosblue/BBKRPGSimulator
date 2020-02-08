using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 显示场景名称命令
    /// </summary>
    internal class CommandShowSceneName : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 显示场景名称命令
        /// </summary>
        /// <param name="context"></param>
        public CommandShowSceneName(SimulatorContext context) : base(0, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandShowSceneNameOperate(Context);

        #endregion 方法

        #region 类

        public class CommandShowSceneNameOperate : Operate
        {
            #region 字段

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyDown = false;

            /// <summary>
            /// 场景名称
            /// </summary>
            private string _sceneName;

            /// <summary>
            /// 显示时长计数
            /// </summary>
            private long _timeCount = 0;

            #endregion 字段

            #region 构造函数

            public CommandShowSceneNameOperate(SimulatorContext context) : base(context)
            {
                _timeCount = 0;
                _isAnyKeyDown = false;
                _sceneName = Context.SceneMap.SceneName;
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.SceneMap.DrawScene(canvas);
                Context.Util.ShowInformation(canvas, _sceneName);
            }

            public override void OnKeyUp(int key)
            {
                _isAnyKeyDown = true;
            }

            public override bool Update(long delta)
            {
                _timeCount += delta;
                if (_timeCount > 100 && _isAnyKeyDown)
                {
                    _isAnyKeyDown = false;
                    return false;
                }
                return _timeCount < 1000;
            }

            #endregion 方法
        }

        #endregion 类
    }
}