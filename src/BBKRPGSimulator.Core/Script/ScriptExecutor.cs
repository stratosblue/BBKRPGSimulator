using System.Collections.Generic;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Script.Commands;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 脚本执行器
    /// </summary>
    internal class ScriptExecutor : ContextDependent
    {
        #region 字段

        /// <summary>
        /// 命令列表
        /// </summary>
        private readonly IReadOnlyList<ICommand> _commands;

        /// <summary>
        /// mEventIndex[i]等于触发事件i+1时，要执行的Operate在list中的序号，
        /// -1表示不存在
        /// </summary>
        private readonly IReadOnlyList<int> _eventIndex;

        /// <summary>
        /// code数据前的长度
        /// </summary>
        private readonly int _headerLength;

        /// <summary>
        /// address offset --- operate's index of mOperateList
        /// </summary>
        private readonly IReadOnlyDictionary<int, int> _mapAddrOffsetIndex;

        /// <summary>
        /// 当前正在执行的操作在操作列表中的索引
        /// </summary>
        private int _curExeOperateIndex;

        /// <summary>
        /// 当前命令的操作
        /// </summary>
        private Operate _currentOperate = null;

        private int _timer = 0;

        private long _timerCounter = 0;

        private int _timerEventId = 0;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 脚本处理器
        /// </summary>
        private ScriptProcess ScriptProcess => Context.ScriptProcess;

        #endregion 属性

        #region 构造函数

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="list">一个脚本文件对应的操作表</param>
        /// <param name="eventIndex">eventIndex[i]等于触发事件i+1时，要执行的Operate在list中的序号</param>
        /// <param name="map">地址偏移-序号</param>
        /// <param name="headerLength">数据长度</param>
        public ScriptExecutor(SimulatorContext context,
                              IReadOnlyList<ICommand> list,
                              IReadOnlyList<int> eventIndex,
                              IReadOnlyDictionary<int, int> map,
                              int headerLength) : base(context)
        {
            _commands = list;
            _eventIndex = eventIndex;
            _curExeOperateIndex = 0;
            _mapAddrOffsetIndex = map;
            _headerLength = headerLength;
        }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas) => _currentOperate?.Draw(canvas);

        public void GotoAddress(int address)
        {
            _curExeOperateIndex = _mapAddrOffsetIndex[address - _headerLength];

            if (_currentOperate != null)
            { // 不在Operate.process()中调用的gotoAddress
                _currentOperate = null;
                --_curExeOperateIndex;
            }
            else
            { // 在Operate.process()中调用的gotoAddress
              // loong TODO: why?
                ScriptProcess.EnableExecuteScript = false; // mark 下次调用process再执行
            }
            ScriptProcess.ScriptRunning = true;
        }

        public void KeyDown(int key) => _currentOperate?.OnKeyDown(key);

        public void KeyUp(int key) => _currentOperate?.OnKeyUp(key);

        public void Process()
        {
            if (_currentOperate == null)
            {
                while (_curExeOperateIndex < _commands.Count && ScriptProcess.EnableExecuteScript)
                {
                    var cmd = _commands[_curExeOperateIndex];
                    _currentOperate = cmd.Process();
                    if (_currentOperate != null)
                    {
                        // 执行 update draw
                        return;
                    }
                    if (!ScriptProcess.EnableExecuteScript)
                    {
                        ScriptProcess.EnableExecuteScript = true;
                        return;
                    }
                    ++_curExeOperateIndex;
                }
                // 正常情况不回执行到这里，脚本最后一句是callback
            }
        }

        public void SetTimer(int timer, int eventId)
        {
            _timer = timer * 500;
            _timerCounter = _timer;
            _timerEventId = eventId;
        }

        public void TimerStep(long delta)
        {
            if (_timer > 0 && _timerEventId > 0)
            {
                _timerCounter -= delta;
                if (_timerCounter <= 0)
                {
                    _timerCounter += _timer;
                    TriggerEvent(_timerEventId);
                }
            }
        }

        /// <summary>
        /// 触发地图事件,场景切换，NPC对话，开宝箱等
        /// 返回是否成功触发
        /// </summary>
        /// <param name="eventId">事件ID</param>
        /// <returns></returns>
        public bool TriggerEvent(int eventId)
        {
            if (eventId > _eventIndex.Count)
            {
                Stop();
                return false;
            }

            int index = _eventIndex[eventId - 1];
            if (index != -1)
            {
                _curExeOperateIndex = index;
                _currentOperate = null;
                Start();
                return true;
            }
            Stop();
            return false;
        }

        public void Update(long delta)
        {
            if (_currentOperate?.Update(delta) == false)
            {
                _currentOperate = null;
                ++_curExeOperateIndex;
            }
        }

        private void Start()
        {
            ScriptProcess.ScriptRunning = true;
            ScriptProcess.EnableExecuteScript = true;
        }

        private void Stop()
        {
            ScriptProcess.ScriptRunning = false;
            ScriptProcess.EnableExecuteScript = false;
        }

        #endregion 方法
    }
}