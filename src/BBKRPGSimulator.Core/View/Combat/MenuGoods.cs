using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Actions;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.View.GameMenu;

namespace BBKRPGSimulator.View.Combat
{
    /// <summary>
    /// 道具子菜单，装备、投掷、使用
    /// </summary>
    internal class MenuGoods : BaseCombatScreen
    {
        #region 静态定义

        /// <summary>
        /// 菜单选项文本数据
        /// </summary>
        private static readonly byte[] _menuItems = "装备投掷使用".GetBytes();

        /// <summary>
        /// 菜单选项当前选择文本数据
        /// </summary>
        private static readonly byte[][] _menuSelectItems = new byte[][] {
            "装备".GetBytes(),
            "投掷".GetBytes(),
            "使用".GetBytes() };

        /// <summary>
        /// 展示的矩形位置
        /// </summary>
        private Rectangle _showRectangle = new Rectangle(29 + 3, 14 + 3, 16 * 2 + 6, 16 * 3 + 6); //new Rectangle(29 + 3, 14 + 3, 29 + 3 + mBg.Width, 14 + 3 + mBg.Height);

        #endregion 静态定义

        #region 字段

        /// <summary>
        /// 背景图
        /// </summary>
        private readonly ImageBuilder _background;

        /// <summary>
        /// 当前选项索引
        /// </summary>
        private int _curSelectedIndex = 0;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 道具子菜单，装备、投掷、使用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="combatUI"></param>
        public MenuGoods(SimulatorContext context, CombatUI combatUI) : base(context, combatUI)
        {
            _background = Context.Util.GetFrameBitmap(_showRectangle.Width, _showRectangle.Height);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 29, 14);
            TextRender.DrawText(canvas, _menuItems, 0, _showRectangle);
            TextRender.DrawSelText(canvas, _menuSelectItems[_curSelectedIndex], _showRectangle.Left, _showRectangle.Top + 16 * _curSelectedIndex);
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_DOWN)
            {
                ++_curSelectedIndex;
                _curSelectedIndex %= _menuSelectItems.Length;
            }
            else if (key == SimulatorKeys.KEY_UP)
            {
                --_curSelectedIndex;
                _curSelectedIndex = (_curSelectedIndex + _menuSelectItems.Length) % _menuSelectItems.Length;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_ENTER)
            {
                List<FightingCharacter> listMonsters = new List<FightingCharacter>();
                listMonsters.AddRange(_combatUI.Monsters);

                List<FightingCharacter> listPlayers = new List<FightingCharacter>();
                listPlayers.AddRange(_combatUI.PlayerCharacters);

                _combatUI.ScreenStack.Pop(); // 弹出子菜单
                switch (_curSelectedIndex)
                {
                    case 0:// 装备
                        Context.PushScreen(new ScreenGoodsList(Context, Context.GoodsManage.EquipList,
                                (goods) =>
                                {
                                    EquipSelected(goods);
                                }, GoodsOperateMode.Use));
                        break;

                    case 1:// 投掷
                        Context.PushScreen(new ScreenGoodsList(Context, GetThrowableGoodsList(),
                             (goods) =>
                             {
                                 Context.PopScreen(); // pop goods list
                                 _combatUI.ScreenStack.Pop(); // pop misc menu
                                 if (goods.IsEffectAll())
                                 {
                                     // 投掷伤害全体敌人
                                     _combatUI.OnActionSelected(new ActionThrowItemAll(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], listMonsters, (GoodsHiddenWeapon)goods));
                                 }
                                 else
                                 {
                                     // 选一个敌人
                                     _combatUI.ScreenStack.Push(new MenuCharacterSelect(Context, _combatUI.MonsterIndicator, CombatUI.MonsterIndicatorPos, listMonsters,
                                         (fc) =>
                                            {
                                                _combatUI.OnActionSelected(new ActionThrowItemOne(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], fc, (GoodsHiddenWeapon)goods));
                                            }, false, _combatUI));
                                 }
                             }, GoodsOperateMode.Use));
                        break;

                    case 2:// 使用
                        Context.PushScreen(new ScreenGoodsList(Context, GetUseableGoodsList(),
                            (goods) =>
                            {
                                Context.PopScreen(); // pop goods list
                                _combatUI.ScreenStack.Pop(); // pop misc menu
                                if (goods.IsEffectAll())
                                {
                                    _combatUI.OnActionSelected(new ActionUseItemAll(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], listMonsters, goods));
                                }
                                else
                                {
                                    // 选一个敌人
                                    _combatUI.ScreenStack.Push(new MenuCharacterSelect(Context, _combatUI.MonsterIndicator, CombatUI.MonsterIndicatorPos, listMonsters,
                                        (fc) =>
                                        {
                                            _combatUI.OnActionSelected(new ActionUseItemOne(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], fc, goods));
                                        }, false, _combatUI));
                                }
                            }, GoodsOperateMode.Use));
                        break;
                }
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                _combatUI.ScreenStack.Pop();
            }
        }

        public override void Update(long delta)
        {
        }

        /// <summary>
        /// 选择装备物品
        /// </summary>
        /// <param name="goods"></param>
        private void EquipSelected(BaseGoods goods)
        {
            var canUseCharacters = _combatUI.PlayerCharacters.Where(m => goods.CanPlayerUse(m.Index)).ToArray();
            if (canUseCharacters.Length == 0)
            {
                // 没人能装备
                Context.ShowMessage("不能装备!", 1000);
            }
            else if (canUseCharacters.Length == 1)
            {
                // 一个人能装备
                if (canUseCharacters[0].HasEquipt(goods.Type, goods.Index))
                {
                    Context.ShowMessage("已装备!", 1000);
                }
                else
                {
                    Context.PushScreen(new ScreenChangeEquipment(Context, canUseCharacters[0], (GoodsEquipment)goods));
                }
            }
            else
            {
                // 多人可装备
                Context.PushScreen(new EquipSelectedScreen(Context, canUseCharacters, goods));
            }
        }

        /// <summary>
        /// 当前物品链表中，可用于投掷敌人的物品
        /// </summary>
        private List<BaseGoods> GetThrowableGoodsList()
        {
            List<BaseGoods> result = new List<BaseGoods>();

            foreach (var goods in Context.GoodsManage.GoodsList)
            {
                if (goods.Type == 8)
                {
                    result.Add(goods);
                }
            }
            return result;
        }

        /// <summary>
        /// 当前物品链表中，可用物品
        /// </summary>
        private List<BaseGoods> GetUseableGoodsList()
        {
            List<BaseGoods> result = new List<BaseGoods>();

            foreach (var goods in Context.GoodsManage.GoodsList)
            {
                switch (goods.Type)
                {
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                        result.Add(goods);
                        break;
                }
            }
            return result;
        }

        #endregion 方法

        // end of equipSelected
    }
}