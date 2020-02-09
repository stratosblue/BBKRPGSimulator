using System;
using System.Collections.Generic;
using System.Diagnostics;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 多人装备选择界面
    /// </summary>
    internal class MutilCharacterEquipScreen : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 背景图片
        /// </summary>
        private ImageBuilder _background;

        /// <summary>
        /// 角色列表
        /// </summary>
        private List<PlayerCharacter> _characters;

        /// <summary>
        /// 当前选择进行装备的装备
        /// </summary>
        private BaseGoods _goods;

        /// <summary>
        /// 角色名字列表
        /// </summary>
        private byte[][] _names;

        /// <summary>
        /// 当前选择的索引
        /// </summary>
        private int _selectedIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 多人装备选择界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="characters">角色列表</param>
        /// <param name="goods">当前选择进行装备的装备</param>
        public MutilCharacterEquipScreen(SimulatorContext context, List<PlayerCharacter> characters, BaseGoods goods) : base(context)
        {
            //HACK 此处需要测试是否正常运行
            _characters = characters;
            _goods = goods;

            _background = Context.Util.GetFrameBitmap(16 * 5 + 6, 6 + 16 * _characters.Count);
            _names = ExtensionFunction.DyadicArrayByte(_characters.Count, 11);//new byte[list.Count][11];
            for (int i = 0; i < _names.Length; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _names[i][j] = (byte)' ';
                }
                try
                {
                    byte[] tmp = _characters[i].Name.GetBytes();
                    Array.Copy(tmp, 0, _names[i], 0, tmp.Length);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 50, 14);
            for (int i = 0; i < _names.Length; i++)
            {
                if (i != _selectedIndex)
                {
                    TextRender.DrawText(canvas, _names[i], 50 + 3, 14 + 3 + 16 * i);
                }
                else
                {
                    TextRender.DrawSelText(canvas, _names[i], 50 + 3, 14 + 3 + 16 * i);
                }
            }
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_DOWN && _selectedIndex < _names.Length - 1)
            {
                ++_selectedIndex;
            }
            else if (key == SimulatorKeys.KEY_UP && _selectedIndex > 0)
            {
                --_selectedIndex;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                if (_characters[_selectedIndex].HasEquipt(_goods.Type, _goods.Index))
                {
                    Context.ShowMessage("已装备!", 1000);
                }
                else
                {
                    Context.PopScreen();
                    Context.PushScreen(new ScreenChangeEquipment(Context, _characters[_selectedIndex], (GoodsEquipment)_goods));
                }
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                Context.PopScreen();
            }
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}