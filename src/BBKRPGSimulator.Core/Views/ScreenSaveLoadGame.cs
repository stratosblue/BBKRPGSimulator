using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Lib;
using BBKRPGSimulator.View;

namespace BBKRPGSimulator.GameMenu
{
    /// <summary>
    /// 读取保存Screen
    /// </summary>
    internal class ScreenSaveLoadGame : BaseScreen
    {
        //TODO 保存还需要优化；

        #region 字段

        public Action CallBack;

        /// <summary>
        /// 空档案名称
        /// </summary>
        private const string _empty = "空档案    ";

        /// <summary>
        /// 文本的位置
        /// </summary>
        private static readonly int[,] _textPos = {
            {68, 28},
            {68, 51},
            {68, 74}};

        /// <summary>
        /// 背景图片
        /// </summary>
        private readonly ResImage _backgroudImage;

        /// <summary>
        /// 当前执行的操作
        /// </summary>
        private readonly SaveLoadOperate _curOperate;

        /// <summary>
        /// 存档图片列表
        /// </summary>
        private readonly List<List<ResImage>> _headImgs = new List<List<ResImage>>();

        /// <summary>
        /// 存档名称列表
        /// </summary>
        private readonly string[] _texts = { _empty, _empty, _empty };

        /// <summary>
        /// 当前选择索引
        /// </summary>
        private int _index = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 读取保存Screen
        /// </summary>
        /// <param name="operate"></param>
        public ScreenSaveLoadGame(SimulatorContext context, SaveLoadOperate operate) : base(context)
        {
            _curOperate = operate;
            _backgroudImage = Context.LibData.GetImage(2, operate == SaveLoadOperate.LOAD ? 16 : 15);
            _headImgs.Add(new List<ResImage>());
            _headImgs.Add(new List<ResImage>());
            _headImgs.Add(new List<ResImage>());

            for (int i = 0; i < 3; i++)
            {
                string fileName = $"./assets/saves/{context.LibData.Hash}_{i}";

                Stream stream = Context.StreamProvider.GetStream(fileName);
                if (stream != null)
                {
                    try
                    {
                        _texts[i] = FormatSaveName(GetSceneNameAndHeads(stream, _headImgs[i]));
                    }
                    finally
                    {
                        stream.Dispose();
                    }
                }
            }
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            _backgroudImage.Draw(canvas, 1, 0, 0);
            for (int i = 0; i < _headImgs.Count; i++)
            {
                for (int j = 0; j < _headImgs[i].Count; j++)
                {
                    ResImage img = _headImgs[i][j];
                    if (img != null)
                    {
                        img.Draw(canvas, 7, 8 + 20 * j, _textPos[i, 1] - 6);
                    }
                }
            }
            TextRender.DrawText(canvas, _texts[0], _textPos[0, 0], _textPos[0, 1]);
            TextRender.DrawText(canvas, _texts[1], _textPos[1, 0], _textPos[1, 1]);
            TextRender.DrawText(canvas, _texts[2], _textPos[2, 0], _textPos[2, 1]);
            TextRender.DrawSelText(canvas, _texts[_index], _textPos[_index, 0], _textPos[_index, 1]);
        }

        public bool LoadGame(Stream stream)
        {
            try
            {
                using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
                {
                    Context.Deserialize(binaryReader);
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            finally
            {
                stream.Dispose();
            }
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP)
            {
                if (--_index < 0)
                {
                    _index = 2;
                }
            }
            else if (key == SimulatorKeys.KEY_DOWN)
            {
                if (++_index > 2)
                {
                    _index = 0;
                }
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.PopScreen();
            }
            else if (key == SimulatorKeys.KEY_ENTER)
            {
                var fileName = $"./assets/saves/{Context.LibData.Hash}_{_index}";
                Stream stream = Context.StreamProvider.GetOrCreateStream(fileName);

                if (_curOperate == SaveLoadOperate.LOAD)
                {
                    // 加载存档
                    if (stream is null)
                    {
                        return;
                    }
                    if (LoadGame(stream))
                    {
                        // 读档成功
                        Context.ChangeScreen(ScreenEnum.SCREEN_MAIN_GAME, false);
                    }
                    else
                    {
                        // 读档失败
                        Context.ChangeScreen(ScreenEnum.SCREEN_MENU, true);
                    }
                }
                else
                {
                    // 保存存档
                    if (_headImgs[_index].Count == 0)
                    {
                        SaveGame(stream);
                        Context.PopScreen();
                        Context.PopScreen();
                        Context.PopScreen();
                        CallBack?.Invoke();
                    }
                    else
                    {
                        // 询问是否覆盖存档
                        Context.PushScreen(new ScreenMessageBox(Context, "覆盖原进度?",
                            () =>
                            {
                                SaveGame(stream);
                                Context.PopScreen();
                                Context.PopScreen();
                                Context.PopScreen();
                                CallBack?.Invoke();
                            }));
                    }
                }
            }
        }

        /// <summary>
        /// 保存游戏
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool SaveGame(Stream stream)
        {
            try
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(stream, Encoding.UTF8))
                {
                    Context.Serialize(binaryWriter);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            finally
            {
                stream.Dispose();
            }
            return true;
        }

        public override void Update(long delta)
        {
        }

        /// <summary>
        /// 格式化存档名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string FormatSaveName(string name)
        {
            try
            {
                while (name.GetBytes().Length < _empty.GetBytes().Length) name += " ";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            return name;
        }

        /// <summary>
        /// 获取存档场景名称和头像
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="heads"></param>
        /// <returns></returns>
        private string GetSceneNameAndHeads(Stream stream, List<ResImage> heads)
        {
            string name = string.Empty;

            using (BinaryReader binaryReader = new BinaryReader(stream))
            {
                name = binaryReader.ReadString();
                int actorNum = binaryReader.ReadInt32();

                for (int i = 0; i < actorNum; i++)
                {
                    heads.Add(Context.LibData.GetImage(1, binaryReader.ReadInt32()));
                }
            }
            stream.Close();

            return name;
        }

        #endregion 方法
    }
}