using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 物品菜单界面
    /// </summary>
    internal class ScreenMenuGoods : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 操作选项
        /// </summary>
        private static readonly string[] _operateItems = { "使用", "装备" };

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 背景图片
        /// </summary>
        private ImageBuilder _background = null;

        /// <summary>
        /// 当前选择的操作ID
        /// </summary>
        private int _selectedId = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 物品菜单界面
        /// </summary>
        /// <param name="context"></param>
        public ScreenMenuGoods(SimulatorContext context) : base(context)
        {
            _background = Context.Util.GetFrameBitmap(77 - 39 + 1, 77 - 39 + 1);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 39, 39);
            if (_selectedId == 0)
            {
                TextRender.DrawSelText(canvas, _operateItems[0], 39 + 3, 39 + 3);
                TextRender.DrawText(canvas, _operateItems[1], 39 + 3, 39 + 3 + 16);
            }
            else if (_selectedId == 1)
            {
                TextRender.DrawText(canvas, _operateItems[0], 39 + 3, 39 + 3);
                TextRender.DrawSelText(canvas, _operateItems[1], 39 + 3, 39 + 3 + 16);
            }
        }

        public override bool IsPopup()
        {
            return true;
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP || key == SimulatorKeys.KEY_DOWN)
            {
                _selectedId = 1 - _selectedId;
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
                Context.PopScreen();
                Context.PushScreen(new ScreenGoodsList(Context, _selectedId == 0 ? Context.GoodsManage.GoodsList : Context.GoodsManage.EquipList,
                        (goods) =>
                        {
                            if (_selectedId == 0)
                            { // 使用
                                GoodsSelected(goods);
                            }
                            else if (_selectedId == 1)
                            { // 装备
                                EquipSelected(goods);
                            }
                        }, GoodsOperateMode.Use));
            }
        }

        public override void Update(long delta)
        {
        }

        /// <summary>
        /// 选择装备
        /// </summary>
        /// <param name="goods"></param>
        private void EquipSelected(BaseGoods goods)
        {
            List<PlayerCharacter> characters = new List<PlayerCharacter>();
            for (int i = 0; i < Context.PlayContext.PlayerCharacters.Count; i++)
            {
                PlayerCharacter character = Context.PlayContext.PlayerCharacters[i];
                if (goods.CanPlayerUse(character.Index))
                {
                    characters.Add(character);
                }
            }

            if (characters.Count == 0)
            {
                // 没人能装备
                Context.ShowMessage("不能装备!", 1000);
            }
            else if (characters.Count == 1)
            {
                // 一个人能装备
                if (characters[0].HasEquipt(goods.Type, goods.Index))
                {
                    Context.ShowMessage("已装备!", 1000);
                }
                else
                {
                    Context.PushScreen(new ScreenChangeEquipment(Context, characters[0], (GoodsEquipment)goods));
                }
            }
            else
            {
                // 多人可装备
                Context.PushScreen(new MutilCharacterEquipScreen(Context, characters, goods));
            }
        }

        /// <summary>
        /// 选择使用
        /// </summary>
        /// <param name="goods"></param>
        private void GoodsSelected(BaseGoods goods)
        {
            switch (goods.Type)
            {
                case 8: // 暗器
                case 12: // 兴奋剂
                    Context.ShowMessage("战斗中才能使用!", 1000);
                    break;

                case 13: // 土遁
                         // TODO 迷宫中的用法，调用脚本
                    Context.ScriptProcess.TriggerEvent(255);
                    while (!(Context.GetCurrentScreen() is ScreenMainGame))
                    {
                        Context.PopScreen();
                    }
                    break;

                case 14: // 剧情类
                         // TODO 剧情类物品用法
                    Context.ShowMessage("当前无法使用!", 1000);
                    break;

                case 9: // 药物
                case 10: // 灵药
                case 11: // 仙药
                    Context.PushScreen(new ScreenTakeMedicine(Context, goods));
                    break;
            }
        }

        #endregion 方法
    }
}