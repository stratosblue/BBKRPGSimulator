using System.Collections.Generic;
using System.Drawing;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 角色穿戴界面
    /// </summary>
    internal class ScreenCharacterWearing : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 物品的部位名称列表
        /// </summary>
        private static readonly string[] _itemNames = { "装饰", "装饰", "护腕", "脚蹬", "手持", "身穿", "肩披", "头戴" };

        /// <summary>
        /// 展示的偏移
        /// </summary>
        private static readonly Point[] _pos = new Point[] {
                new Point(4, 3),
                new Point(4, 30),
                new Point(21, 59),
                new Point(51, 65),
                new Point(80, 61),
                new Point(109, 46),
                new Point(107, 9),
                new Point(79, 2)
        };

        /// <summary>
        /// 描述的矩形位置
        /// </summary>
        private static readonly Rectangle _rectDesc = new Rectangle(12, 31, 139, 34); //new Rectangle(9 + 3, 28 + 3, 151, 65);

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 当前角色编号
        /// </summary>
        private int _curCharacterIndex = -1;

        /// <summary>
        /// 当前角色的装备
        /// </summary>
        private GoodsEquipment[] _curEquipments;

        /// <summary>
        /// 当前物品编号
        /// </summary>
        private int _curItemIndex = 0;

        /// <summary>
        /// 描述的图像
        /// </summary>
        private ImageBuilder _descImg;

        /// <summary>
        /// 描述的文本数据
        /// </summary>
        private byte[] _descText;

        /// <summary>
        /// 名称的图像
        /// </summary>
        private ImageBuilder _nameImg;

        /// <summary>
        /// 名称的文本数据
        /// </summary>
        private byte[] _nameText;

        /// <summary>
        /// 下一个要画的描述中的字节
        /// </summary>
        private int _nextToDraw = 0;

        /// <summary>
        /// 当前是否显示描述
        /// </summary>
        private bool _showingDesc = false;

        /// <summary>
        /// 保存上次描述所画位置
        /// </summary>
        private Stack<int> _stackLastToDraw = new Stack<int>();

        /// <summary>
        /// 当前要画的描述中的字节
        /// </summary>
        private int _toDraw = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 角色穿戴界面
        /// </summary>
        /// <param name="context"></param>
        public ScreenCharacterWearing(SimulatorContext context) : base(context)
        {
            _nameImg = Context.Util.GetFrameBitmap(92 - 9 + 1, 29 - 10 + 1);//Bitmap.createBitmap(92 - 9 + 1, 29 - 10 + 1, Bitmap.Config.ARGB_8888);
            _descImg = Context.Util.GetFrameBitmap(151 - 9 + 1, 65 - 28 + 1);//Bitmap.createBitmap(151 - 9 + 1, 65 - 28 + 1, Bitmap.Config.ARGB_8888);

            if (Context.PlayContext.PlayerCharacters.Count > 0)
            {
                _curEquipments = Context.PlayContext.PlayerCharacters[0].Equipments;
                _curCharacterIndex = 0;
            }
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawColor(Constants.COLOR_WHITE);
            canvas.DrawBitmap(Context.Util.bmpChuandai, 160 - Context.Util.bmpChuandai.Width, 0);

            // 画装备
            for (int i = 0; i < 8; i++)
            {
                if (_curEquipments[i] != null)
                {
                    _curEquipments[i].Draw(canvas, _pos[i].X + 1, _pos[i].Y + 1);
                }
            }
            canvas.DrawRect(_pos[_curItemIndex].X, _pos[_curItemIndex].Y, _pos[_curItemIndex].X + 25, _pos[_curItemIndex].Y + 25, Context.Util.sBlackPaint);
            TextRender.DrawText(canvas, _itemNames[_curItemIndex], 120, 80);

            // 画人物头像、姓名
            if (_curCharacterIndex >= 0)
            {
                PlayerCharacter p = Context.PlayContext.PlayerCharacters[_curCharacterIndex];
                p.DrawHead(canvas, 44, 12);
                TextRender.DrawText(canvas, p.Name, 30, 40);
            }

            if (_showingDesc)
            {
                canvas.DrawBitmap(_nameImg, 9, 10);
                canvas.DrawBitmap(_descImg, 9, 28);
                TextRender.DrawText(canvas, _nameText, 9 + 3, 10 + 3);
                _nextToDraw = TextRender.DrawText(canvas, _descText, _toDraw, _rectDesc);
            }
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_DOWN && _curItemIndex < 8 - 1)
            {
                ++_curItemIndex;
                ResetDesc();
            }
            else if (key == SimulatorKeys.KEY_UP && _curItemIndex > 0)
            {
                --_curItemIndex;
                ResetDesc();
            }
            else if (key == SimulatorKeys.KEY_RIGHT && _curCharacterIndex < Context.PlayContext.PlayerCharacters.Count - 1)
            {
                ++_curCharacterIndex;
                _curEquipments = Context.PlayContext.PlayerCharacters[_curCharacterIndex].Equipments;
                ResetDesc();
            }
            else if (key == SimulatorKeys.KEY_LEFT && _curCharacterIndex > 0)
            {
                --_curCharacterIndex;
                _curEquipments = Context.PlayContext.PlayerCharacters[_curCharacterIndex].Equipments;
                ResetDesc();
            }
            else if (_showingDesc)
            {
                if (key == SimulatorKeys.KEY_PAGEDOWN)
                {
                    if (_nextToDraw < _descText.Length)
                    {
                        _stackLastToDraw.Push(_toDraw);
                        _toDraw = _nextToDraw;
                    }
                }
                else if (key == SimulatorKeys.KEY_PAGEUP && _toDraw != 0)
                {
                    if (_stackLastToDraw.Count > 0)
                    {
                        _toDraw = _stackLastToDraw.Pop();
                    }
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
                if (!_showingDesc && _curEquipments[_curItemIndex] != null)
                {
                    _showingDesc = true;
                    _nameText = _curEquipments[_curItemIndex].Name.GetBytes();
                    _descText = _curEquipments[_curItemIndex].Description.GetBytes();
                }
                else
                {
                    //切换到装备界面
                    ResetDesc();
                    Context.PushScreen(new ScreenGoodsList(Context, GetTheEquipList(PlayerCharacter.EquipTypes[_curItemIndex]),
                        (goods) =>
                        {
                            PlayerCharacter actor = Context.PlayContext.PlayerCharacters[_curCharacterIndex];
                            if (goods.CanPlayerUse(actor.Index))
                            {
                                Context.PopScreen();
                                Context.PushScreen(new ScreenChangeEquipment(Context, actor, (GoodsEquipment)goods));
                            }
                            else
                            {
                                Context.ShowMessage("不能装备!", 1000);
                            }
                        }, GoodsOperateMode.Use));
                }
            }
        }

        public override void Update(long delta)
        {
        }

        /// <summary>
        /// 获取装备列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<BaseGoods> GetTheEquipList(int type)
        {
            List<BaseGoods> result = new List<BaseGoods>();

            foreach (var item in Context.GoodsManage.EquipList)
            {
                if (item.Type == PlayerCharacter.EquipTypes[_curItemIndex])
                {
                    // 找到所有与当前选择类型相同的装备
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// 重置描述显示信息
        /// </summary>
        private void ResetDesc()
        {
            if (_showingDesc)
            {
                _showingDesc = false;
                _toDraw = 0;
                _nextToDraw = 0;
                _stackLastToDraw.Clear();
            }
        }

        #endregion 方法
    }
}