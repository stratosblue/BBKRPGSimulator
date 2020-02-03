using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.View;

namespace BBKRPGSimulator
{
    /// <summary>
    /// RPG模拟器
    /// </summary>
    public class RPGSimulator
    {
        #region 事件

        /// <summary>
        /// 帧所有画面绘制完成，输出画面
        /// </summary>
        public event Action<ImageBuilder> RenderFrame;

        #endregion 事件

        #region 字段

        private SimulatorContext _context;

        /// <summary>
        /// 键映射
        /// </summary>
        private Dictionary<int, int> _keyMap = new Dictionary<int, int>();

        /// <summary>
        /// 主画布
        /// </summary>
        private ICanvas _mainCanvas = null;

        /// <summary>
        /// 运行的TokenSource
        /// </summary>
        private CancellationTokenSource _tokenSource = null;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 运行的TokenSource
        /// </summary>
        private CancellationTokenSource TokenSource
        {
            get => _tokenSource;
            set
            {
                if (_tokenSource != null)
                {
                    _tokenSource.Cancel(true);
                    _tokenSource.Dispose();
                }

                _tokenSource = value;
            }
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// RPG模拟器
        /// </summary>
        public RPGSimulator()
        { }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="keyCode"></param>
        public void KeyPressed(int keyCode)
        {
            int key = GetKey(keyCode);
            _context.KeyPressed(key);
        }

        /// <summary>
        /// 按键放开
        /// </summary>
        /// <param name="keyCode"></param>
        public void KeyReleased(int keyCode)
        {
            int key = GetKey(keyCode);
            //_context.PlayInfo?.PlayerCharacter?.GainExperience(1000);
            _context.KeyReleased(key);
        }

        /// <summary>
        /// 启动游戏
        /// </summary>
        public void Start(SimulatorOptions options)
        {
            _context = new SimulatorContext(options);

            if (options.KeyMap?.Count > 0)
            {
                foreach (var item in options.KeyMap)
                {
                    if (!_keyMap.ContainsKey(item.Key))
                    {
                        _keyMap.Add(item.Key, item.Value);
                    }
                }
            }
            _mainCanvas = _context.GraphicsFactory.NewCanvas();

            _context.PushScreen(new ScreenAnimation(_context, 247));

            TokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                InternalRun(TokenSource.Token);
            }, TokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        /// <summary>
        /// 停止运行
        /// </summary>
        public void Stop()
        {
            TokenSource = null;
        }

        #region Internal

        /// <summary>
        /// 游戏运行的主线程
        /// </summary>
        /// <param name="token"></param>
        internal void InternalRun(CancellationToken token)
        {
            long curTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long lastTime = curTime;
            while (!token.IsCancellationRequested)
            {
                try
                {
                    lock (_context.ScreenStack)
                    {
                        curTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        _context.ScreenStack.Peek().Update(curTime - lastTime);
                        lastTime = curTime;

                        // 刷新
                        for (int i = 0; i < _context.ScreenStack.Count; i++)
                        {
                            _context.ScreenStack[i].Draw(_mainCanvas);
                        }

                        RenderFrame?.Invoke(_mainCanvas.Background);
                    }

                    Thread.Sleep(_context.LoopInterval);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取按键值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int GetKey(int input)
        {
            if (_keyMap.ContainsKey(input))
            {
                return _keyMap[input];
            }
            int key = -1;
            switch (input)
            {
                case 37:
                    key = SimulatorKeys.KEY_LEFT;
                    break;

                case 39:
                    key = SimulatorKeys.KEY_RIGHT;
                    break;

                case 38:
                    key = SimulatorKeys.KEY_UP;
                    break;

                case 40:
                    key = SimulatorKeys.KEY_DOWN;
                    break;

                case 99:
                    key = SimulatorKeys.KEY_PAGEDOWN;
                    break;

                case 102:
                    key = SimulatorKeys.KEY_PAGEUP;
                    break;

                case 97:
                    key = SimulatorKeys.KEY_ENTER;
                    break;

                case 98:
                    key = SimulatorKeys.KEY_CANCEL;
                    break;
            }
            return key;
        }

        #endregion Internal

        #endregion 方法
    }
}