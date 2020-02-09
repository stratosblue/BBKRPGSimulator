using System;
using System.Collections.Generic;
using System.IO;

using BBKRPGSimulator.Combat;
using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Interface;
using BBKRPGSimulator.Lib;
using BBKRPGSimulator.Script;
using BBKRPGSimulator.View;

namespace BBKRPGSimulator
{
    /// <summary>
    /// 上下文状态
    /// </summary>
    internal class SimulatorContext : ICustomSerializeable
    {
        #region 字段

        /// <summary>
        /// 静态随机对象
        /// </summary>
        private static readonly Random _random = new Random();

        private long _lastKeyPressedTime = 0;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 战斗管理
        /// </summary>
        public CombatManage CombatManage { get; private set; }

        /// <summary>
        /// 物品管理
        /// </summary>
        public GoodsManage GoodsManage { get; private set; }

        /// <summary>
        /// 图片构建工厂
        /// </summary>
        public IGraphicsFactory GraphicsFactory { get; private set; }

        /// <summary>
        /// 按键处理的间隔（毫秒）
        /// 默认50
        /// </summary>
        public int KeyInterval { get; set; } = 50;

        /// <summary>
        /// Lib数据
        /// </summary>
        public DatLib LibData { get; private set; }

        /// <summary>
        /// 处理的循环间隔（毫秒）
        /// 45
        /// </summary>
        public int LoopInterval { get; set; } = 45;

        /// <summary>
        /// 游戏信息
        /// </summary>
        public PlayContext PlayContext { get; private set; }

        /// <summary>
        /// 随机对象
        /// </summary>
        public Random Random { get => _random; }

        /// <summary>
        /// 场景地图
        /// </summary>
        public SceneMap SceneMap { get; private set; }

        /// <summary>
        /// 屏幕显示栈
        /// (表面上是个栈)
        /// </summary>
        public List<BaseScreen> ScreenStack { get; private set; } = new List<BaseScreen>();

        /// <summary>
        /// 脚本处理器
        /// </summary>
        public ScriptProcess ScriptProcess { get; private set; }

        public RPGSimulator Simulator { get; }

        /// <summary>
        /// 流提供器，用于保存存档文件
        /// </summary>
        public IStreamProvider StreamProvider { get; set; }

        /// <summary>
        /// 文本呈现器
        /// </summary>
        public TextRender TextRender { get; private set; }

        /// <summary>
        /// 显示工具
        /// </summary>
        public Util Util { get; private set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 上下文状态
        /// </summary>
        public SimulatorContext(RPGSimulator simulator, SimulatorOptions options)
        {
            Simulator = simulator;
            StreamProvider = options.StreamProvider ?? new BaseDirectoryStreamProvider(Environment.CurrentDirectory);

            if (options.LibData != null)
            {
                LibData = new DatLib(options.LibData, this);
            }
            else if (options.LibStream != null)
            {
                LibData = new DatLib(options.LibStream, this);
            }
            else if (!string.IsNullOrEmpty(options.LibPath)
                     && StreamProvider.GetStream(options.LibPath) is Stream stream)
            {
                LibData = new DatLib(stream, this);
            }
            else
            {
                throw new ArgumentException("必须配置有效的Lib源");
            }

            GraphicsFactory = options.GraphicsFactory ?? new DefaultGraphicsFactory();

            TextRender = new TextRender(this);

            CombatManage = new CombatManage(this);
            GoodsManage = new GoodsManage(this);
            SceneMap = new SceneMap(this);
            ScriptProcess = new ScriptProcess(this);
            Util = new Util(this);

            Util.Init();

            PlayContext = new PlayContext(this);

            LoopInterval = options.LoopInterval;
            KeyInterval = options.KeyInterval;
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="delay"></param>
        internal void ShowMessage(string msg, long delay) => PushScreen(new ScreenShowMessage(this, msg, delay));

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="keyCode"></param>
        public void KeyPressed(int keyCode)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (now - _lastKeyPressedTime >= KeyInterval)
            {
                GetCurrentScreen().OnKeyDown(keyCode);
                _lastKeyPressedTime = now;
            }
        }

        /// <summary>
        /// 按键放开
        /// </summary>
        /// <param name="keyCode"></param>
        public void KeyReleased(int keyCode) => GetCurrentScreen().OnKeyUp(keyCode);

        /// <summary>
        /// 屏幕转换
        /// </summary>
        /// <param name="screenType"></param>
        /// <param name="tag">SCREEN_MAIN_GAME时true为新游戏</param>
        internal void ChangeScreen(ScreenEnum screenType, bool tag = false)
        {
            BaseScreen screen = null;
            switch (screenType)
            {
                case ScreenEnum.SCREEN_DEV_LOGO:
                    screen = new ScreenAnimation(this, 247);
                    break;

                case ScreenEnum.SCREEN_GAME_LOGO:
                    screen = new ScreenAnimation(this, 248);
                    break;

                case ScreenEnum.SCREEN_MENU:
                    screen = new ScreenMenuStart(this);
                    break;

                case ScreenEnum.SCREEN_MAIN_GAME:
                    screen = new ScreenMainGame(this, tag);
                    break;

                case ScreenEnum.SCREEN_GAME_FAIL:
                    screen = new ScreenAnimation(this, 249);
                    break;

                case ScreenEnum.SCREEN_SAVE_GAME:
                    screen = new ScreenSaveLoadGame(this, SaveLoadOperate.SAVE);
                    break;

                case ScreenEnum.SCREEN_LOAD_GAME:
                    screen = new ScreenSaveLoadGame(this, SaveLoadOperate.LOAD);
                    break;
            }
            if (screen != null)
            {
                ScreenStack.Clear();
                ScreenStack.Push(screen);
            }
            GC.Collect();
        }

        /// <summary>
        /// 获取当前界面
        /// </summary>
        /// <returns></returns>
        internal BaseScreen GetCurrentScreen() => ScreenStack.Peek();

        /// <summary>
        /// 弹出最后一个页面
        /// </summary>
        internal void PopScreen() => ScreenStack.Pop();

        /// <summary>
        /// 添加新的界面
        /// </summary>
        /// <param name="screen"></param>
        internal void PushScreen(BaseScreen screen) => ScreenStack.Push(screen);

        /// <summary>
        /// 调用指定章节
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        internal void CallChapter(int type, int index)
        {
            var process = new ScriptProcess(this)
            {
                PreScriptProcess = ScriptProcess,
            };

            process.LoadScript(type, index);

            ScriptProcess = process;

            process.ScriptRunning = true;
            process.EnableExecuteScript = true;
        }

        /// <summary>
        /// 返回章节
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        internal void ReturnChapter()
        {
            if (ScriptProcess.PreScriptProcess is null)
            {
                throw new MethodAccessException();
            }
            ScriptProcess = ScriptProcess.PreScriptProcess;
        }

        #region 序列化

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        public void Deserialize(BinaryReader binaryReader)
        {
            #region 此段数据为读取存档页面的展示数据

            binaryReader.ReadString();

            int actorCount = binaryReader.ReadInt32();

            while (actorCount-- > 0)
            {
                binaryReader.ReadInt32();
            }

            #endregion 此段数据为读取存档页面的展示数据

            SceneMap.Deserialize(binaryReader);

            PlayContext.Deserialize(binaryReader);

            GoodsManage.Deserialize(binaryReader);

            CombatManage.Deserialize(binaryReader);

            ScriptProcess.ScriptState.Deserialize(binaryReader);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        public void Serialize(BinaryWriter binaryWriter)
        {
            #region 此段数据为读取存档页面的展示数据

            binaryWriter.Write(SceneMap.SceneName);

            int actorCount = PlayContext.PlayerCharacters.Count;

            binaryWriter.Write(actorCount);

            for (int i = 0; i < actorCount; i++)
            {
                binaryWriter.Write(PlayContext.PlayerCharacters[i].Index);
            }

            #endregion 此段数据为读取存档页面的展示数据

            SceneMap.Serialize(binaryWriter);

            PlayContext.Serialize(binaryWriter);

            GoodsManage.Serialize(binaryWriter);

            CombatManage.Serialize(binaryWriter);

            ScriptProcess.ScriptState.Serialize(binaryWriter);
        }

        #endregion 序列化

        #endregion 方法
    }
}