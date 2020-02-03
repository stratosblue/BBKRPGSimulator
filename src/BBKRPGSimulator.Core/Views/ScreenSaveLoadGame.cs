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

        /// <summary>
        /// 空档案名称
        /// </summary>
        private const string _empty = "空档案    ";

        /// <summary>
        /// 存档文件名称列表
        /// </summary>
        private static readonly string[] _fileNames = { "fmjsave0", "fmjsave1", "fmjsave2" };

        /// <summary>
        /// 文本的位置
        /// </summary>
        private static readonly int[][] _textPos = {
           new int[] {68, 28},
           new int[] {68, 51},
            new int[] {68, 74}};

        /// <summary>
        /// 背景图片
        /// </summary>
        private ResImage _backgroudImage;

        /// <summary>
        /// 当前执行的操作
        /// </summary>
        private SaveLoadOperate _curOperate;

        /// <summary>
        /// 存档图片列表
        /// </summary>
        private List<List<ResImage>> _headImgs = new List<List<ResImage>>();

        /// <summary>
        /// 当前选择索引
        /// </summary>
        private int _index = 0;

        /// <summary>
        /// 存档名称列表
        /// </summary>
        private string[] _texts = { _empty, _empty, _empty };

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

            string fileName = "./assets/" + _fileNames[0];

            FileStream fileStream = null;

            try
            {
                if (File.Exists(fileName))
                {
                    fileStream = File.OpenRead(fileName);
                    _texts[0] = FormatSaveName(GetSceneNameAndHeads(fileStream, _headImgs[0]));
                }

                fileName = "./assets/" + _fileNames[1];

                if (File.Exists(fileName))
                {
                    fileStream = File.OpenRead(fileName);
                    _texts[1] = FormatSaveName(GetSceneNameAndHeads(fileStream, _headImgs[1]));
                }

                if (File.Exists(fileName))
                {
                    fileStream = File.OpenRead(fileName);
                    _texts[2] = FormatSaveName(GetSceneNameAndHeads(fileStream, _headImgs[2]));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                fileStream?.Dispose();
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
                        img.Draw(canvas, 7, 8 + 20 * j, _textPos[i][1] - 6);
                    }
                }
            }
            TextRender.DrawText(canvas, _texts[0], _textPos[0][0], _textPos[0][1]);
            TextRender.DrawText(canvas, _texts[1], _textPos[1][0], _textPos[1][1]);
            TextRender.DrawText(canvas, _texts[2], _textPos[2][0], _textPos[2][1]);
            TextRender.DrawSelText(canvas, _texts[_index], _textPos[_index][0], _textPos[_index][1]);
        }

        public bool LoadGame(FileStream file)
        {
            try
            {
                using (BinaryReader binaryReader = new BinaryReader(file, Encoding.UTF8))
                {
                    Context.Deserialize(binaryReader);
                }
                file.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
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
                string fileName = "./assets/" + _fileNames[_index];
                FileStream file = null;
                try
                {
                    if (_curOperate == SaveLoadOperate.LOAD)
                    {
                        // 加载存档
                        if (!File.Exists(fileName))
                        {
                            return;
                        }
                        file = new FileStream(fileName, FileMode.Open);
                        if (LoadGame(file))
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
                        if (!File.Exists(fileName))
                        {
                            file = new FileStream(fileName, FileMode.OpenOrCreate);

                            SaveGame(file);
                            Context.PopScreen();
                            Context.PopScreen();
                            Context.PopScreen();
                        }
                        else
                        {
                            // 询问是否覆盖存档
                            Context.PushScreen(new ScreenMessageBox(Context, "覆盖原进度?",
                                () =>
                                {
                                    file = new FileStream(fileName, FileMode.Open);
                                    SaveGame(file);
                                    Context.PopScreen();
                                    Context.PopScreen();
                                    Context.PopScreen();
                                }));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    file?.Dispose();
                }
            }
        }

        /// <summary>
        /// 保存游戏
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool SaveGame(FileStream file)
        {
            try
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(file, Encoding.UTF8))
                {
                    Context.Serialize(binaryWriter);
                }
                file.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
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
            }
            return name;
        }

        /// <summary>
        /// 获取存档场景名称和头像
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="heads"></param>
        /// <returns></returns>
        private string GetSceneNameAndHeads(FileStream fileStream, List<ResImage> heads)
        {
            string name = string.Empty;
            try
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    name = binaryReader.ReadString();
                    int actorNum = binaryReader.ReadInt32();

                    for (int i = 0; i < actorNum; i++)
                    {
                        heads.Add(Context.LibData.GetImage(1, binaryReader.ReadInt32()));
                    }
                }
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return name;
        }

        #endregion 方法
    }
}