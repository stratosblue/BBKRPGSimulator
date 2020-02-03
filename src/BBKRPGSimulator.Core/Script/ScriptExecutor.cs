using System.Collections.Generic;

using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script
{
    /// <summary>
    /// 脚本执行器
    /// </summary>
    internal class ScriptExecutor : ContextDependent
    {
        #region 字段

        /// <summary>
        /// code数据前的长度
        /// </summary>
        private readonly int _headerLength;

        /// <summary>
        /// address offset --- operate's index of mOperateList
        /// </summary>
        private readonly Dictionary<int, int> _mapAddrOffsetIndex;

        /// <summary>
        /// 当前正在执行的操作在操作列表中的索引
        /// </summary>
        private int _curExeOperateIndex;

        /// <summary>
        /// mEventIndex[i]等于触发事件i+1时，要执行的Operate在list中的序号，
        /// -1表示不存在
        /// </summary>
        private int[] _eventIndex;

        /// <summary>
        /// 当前是否正在执行 update() draw()
        /// </summary>
        private bool _isExeUpdateDraw;

        /// <summary>
        /// 操作列表
        /// </summary>
        private List<Operate> _operateList;

        #endregion 字段

        #region 构造函数

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="list">一个脚本文件对应的操作表</param>
        /// <param name="eventIndex">eventIndex[i]等于触发事件i+1时，要执行的Operate在list中的序号</param>
        /// <param name="map">地址偏移-序号</param>
        /// <param name="headerLength">数据长度</param>
        public ScriptExecutor(SimulatorContext context, List<Operate> list, int[] eventIndex, Dictionary<int, int> map, int headerLength) : base(context)
        {
            _operateList = list;
            _eventIndex = eventIndex;
            _curExeOperateIndex = 0;
            _isExeUpdateDraw = false;
            _mapAddrOffsetIndex = map;
            _headerLength = headerLength;
        }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas)
        {
            if (_isExeUpdateDraw)
            {
                _operateList[_curExeOperateIndex].Draw(canvas);
            }
            else
            {
                //			mOperateList.get(mLastIndex).draw(canvas);
            }
        }

        public void GotoAddress(int address)
        {
            _curExeOperateIndex = _mapAddrOffsetIndex[address - _headerLength];
            if (_isExeUpdateDraw)
            {
                // 不在Operate.process()中调用的gotoAddress
                _isExeUpdateDraw = false;
                --_curExeOperateIndex;
            }
            else
            {
                // 在Operate.process()中调用的gotoAddress
                Context.ScriptProcess.EnableExecuteScript = false; // mark 下次调用process再执行
            }
        }

        public void KeyDown(int key)
        {
            if (_isExeUpdateDraw)
            {
                _operateList[_curExeOperateIndex].OnKeyDown(key);
            }
        }

        public void KeyUp(int key)
        {
            if (_isExeUpdateDraw)
            {
                _operateList[_curExeOperateIndex].OnKeyUp(key);
            }
        }

        public void Process()
        {
            if (!_isExeUpdateDraw)
            {
                for (; _curExeOperateIndex < _operateList.Count && Context.ScriptProcess.EnableExecuteScript;)
                {
                    Operate oper = _operateList[_curExeOperateIndex];
                    if (oper != null && oper.Process())
                    {
                        // 执行 update draw
                        _isExeUpdateDraw = true;
                        return;
                    }
                    if (!Context.ScriptProcess.EnableExecuteScript)
                    {
                        Context.ScriptProcess.EnableExecuteScript = true;
                        return;
                    }
                    ++_curExeOperateIndex;
                }
                // 正常情况不回执行到这里，脚本最后一句是callback
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
            if (eventId > _eventIndex.Length)
            {
                return false;
            }

            int index = _eventIndex[eventId - 1];
            if (index != -1)
            {
                _curExeOperateIndex = index;
                _isExeUpdateDraw = false;
                return true;
            }
            return false;
        }

        public void Update(long delta)
        {
            if (_isExeUpdateDraw)
            {
                if (!_operateList[_curExeOperateIndex].Update(delta))
                {
                    // 退出当前操作
                    _isExeUpdateDraw = false;
                    ++_curExeOperateIndex;
                }
            }
        }

        #endregion 方法
    }
}