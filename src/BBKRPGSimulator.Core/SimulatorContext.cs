using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using BBKRPGSimulator.Characters;
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
        public void ShowMessage(string msg, long delay)
        {
            PushScreen(new ScreenShowMessage(this, msg, delay));
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        /// <param name="keyCode"></param>
        public void KeyPressed(int keyCode)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (now - _lastKeyPressedTime >= KeyInterval)
            {
                ScreenStack.Peek().OnKeyDown(keyCode);
                _lastKeyPressedTime = now;
            }
        }

        /// <summary>
        /// 按键放开
        /// </summary>
        /// <param name="keyCode"></param>
        public void KeyReleased(int keyCode)
        {
            ScreenStack.Peek().OnKeyUp(keyCode);
        }

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
        internal BaseScreen GetCurScreen()
        {
            return ScreenStack.Peek();
        }

        /// <summary>
        /// 弹出最后一个页面
        /// </summary>
        internal void PopScreen()
        {
            ScreenStack.Pop();
        }

        /// <summary>
        /// 添加新的界面
        /// </summary>
        /// <param name="screen"></param>
        internal void PushScreen(BaseScreen screen)
        {
            ScreenStack.Push(screen);
        }

        #region 序列化

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        public void Deserialize(BinaryReader binaryReader)
        {
            SceneMap.SceneName = binaryReader.ReadString();

            int actorCount = binaryReader.ReadInt32();

            while (actorCount-- > 0)
            {
                //HACK 读取出来不保存？
                int t = binaryReader.ReadInt32();
            }

            SceneMap.MapType = binaryReader.ReadInt32();
            SceneMap.MapIndex = binaryReader.ReadInt32();
            SceneMap.MapScreenX = binaryReader.ReadInt32();
            SceneMap.MapScreenY = binaryReader.ReadInt32();
            SceneMap.ScriptType = binaryReader.ReadInt32();
            SceneMap.ScriptIndex = binaryReader.ReadInt32();

            actorCount = binaryReader.ReadInt32();

            PlayContext.PlayerCharacters.Clear();
            for (int i = 0; i < actorCount; i++)
            {
                PlayerCharacter p = new PlayerCharacter(this);
                p.Deserialize(binaryReader);
                PlayContext.PlayerCharacters.Add(p);
            }

            PlayContext.Money = binaryReader.ReadInt32();

            GoodsManage.Deserialize(binaryReader);

            int npcCount = binaryReader.ReadInt32();

            SceneMap.SceneNPCs = new NPC[41];

            for (int i = 0; i < npcCount; i++)
            {
                int npcId = binaryReader.ReadInt32();
                SceneMap.SceneNPCs[npcId] = new NPC(this);
                SceneMap.SceneNPCs[npcId].Deserialize(binaryReader);
            }

            CombatManage.Deserialize(binaryReader);
            ScriptProcess.ScriptState.Deserialize(binaryReader);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="stream"></param>
        public void Serialize(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(SceneMap.SceneName);

            int actorCount = PlayContext.PlayerCharacters.Count;

            binaryWriter.Write(actorCount);

            for (int i = 0; i < actorCount; i++)
            {
                binaryWriter.Write(PlayContext.PlayerCharacters[i].Index);
            }

            binaryWriter.Write(SceneMap.MapType);
            binaryWriter.Write(SceneMap.MapIndex);
            binaryWriter.Write(SceneMap.MapScreenX);
            binaryWriter.Write(SceneMap.MapScreenY);
            binaryWriter.Write(SceneMap.ScriptType);
            binaryWriter.Write(SceneMap.ScriptIndex);
            binaryWriter.Write(PlayContext.PlayerCharacters.Count);

            for (int i = 0; i < PlayContext.PlayerCharacters.Count; i++)
            {
                PlayContext.PlayerCharacters[i].Serialize(binaryWriter);
            }

            binaryWriter.Write(PlayContext.Money);

            GoodsManage.Serialize(binaryWriter);

            var npcCount = SceneMap.SceneNPCs.Where(m => m != null).Count();

            binaryWriter.Write(npcCount);

            for (int i = 0; i < SceneMap.SceneNPCs.Length; i++)
            {
                if (SceneMap.SceneNPCs[i] != null)
                {
                    binaryWriter.Write(i);
                    SceneMap.SceneNPCs[i].Serialize(binaryWriter);
                }
            }

            CombatManage.Serialize(binaryWriter);
            ScriptProcess.ScriptState.Serialize(binaryWriter);
        }

        #endregion 序列化

        #endregion 方法
    }
}