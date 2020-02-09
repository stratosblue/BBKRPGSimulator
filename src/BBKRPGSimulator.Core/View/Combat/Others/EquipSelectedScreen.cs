using System;
using System.Collections.Generic;
using System.Diagnostics;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.View.GameMenu;

namespace BBKRPGSimulator.View.Combat
{
    internal class EquipSelectedScreen : BaseScreen
    {
        #region 字段

        private BaseGoods _goods;
        private ImageBuilder bg;
        private int curSel = 0;
        private byte[][] itemsText;
        private IList<PlayerCharacter> list;

        #endregion 字段

        #region 构造函数

        public EquipSelectedScreen(SimulatorContext context, IList<PlayerCharacter> _list, BaseGoods goods) : base(context)
        {
            _goods = goods;
            list = _list;
            bg = Context.Util.GetFrameBitmap(16 * 5 + 6, 6 + 16 * list.Count);
            itemsText = ExtensionFunction.DyadicArrayByte(list.Count, 11);//new byte[list.Count][11];

            for (int i = 0; i < itemsText.Length; i++)
            {
                itemsText[i] = new byte[11];
            }

            for (int i = 0; i < itemsText.Length; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    itemsText[i][j] = (byte)' ';
                }
                try
                {
                    byte[] tmp = list[i].Name.GetBytes();
                    Array.Copy(tmp, 0, itemsText[i], 0, tmp.Length);
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
            canvas.DrawBitmap(bg, 50, 14);
            for (int i = 0; i < itemsText.Length; i++)
            {
                if (i != curSel)
                {
                    TextRender.DrawText(canvas, itemsText[i], 50 + 3, 14 + 3 + 16 * i);
                }
                else
                {
                    TextRender.DrawSelText(canvas, itemsText[i], 50 + 3, 14 + 3 + 16 * i);
                }
            }
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_DOWN)
            {
                ++curSel;
                curSel %= itemsText.Length;
            }
            else if (key == SimulatorKeys.KEY_UP)
            {
                --curSel;
                curSel = (curSel + itemsText.Length) % itemsText.Length;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                if (list[curSel].HasEquipt(_goods.Type, _goods.Index))
                {
                    Context.ShowMessage("已装备!", 1000);
                }
                else
                {
                    Context.PopScreen();
                    Context.PushScreen(new ScreenChangeEquipment(Context, list[curSel], (GoodsEquipment)_goods));
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