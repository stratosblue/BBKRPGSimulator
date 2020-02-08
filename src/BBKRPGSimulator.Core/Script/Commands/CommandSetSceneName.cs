using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 设置场景名称命令
    /// </summary>
    internal class CommandSetSceneName : BaseCommand
    {
        #region 字段

        private readonly string _name;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 设置场景名称命令
        /// </summary>
        /// <param name="context"></param>
        public CommandSetSceneName(ArraySegment<byte> data, SimulatorContext context) : base(-1, context)
        {
            var start = data.Offset;
            var code = data.Array;

            int i = 0;
            while (code[start + i] != 0)
            {
                ++i;
            }
            Length = i + 1;

            _name = code.GetString(start);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            Context.SceneMap.SceneName = _name;
            return null;
        }

        #endregion 方法
    }
}