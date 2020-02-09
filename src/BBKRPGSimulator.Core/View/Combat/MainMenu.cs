using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Actions;
using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.View.Combat
{
    /// <summary>
    /// 显示主菜单、角色信息
    /// </summary>
    internal class MainMenu : BaseCombatScreen
    {
        #region 字段

        /// <summary>
        /// 显示角色HP MP的背景图
        /// </summary>
        private readonly ResImage _playerInfoBackground;

        /// <summary>
        /// 菜单图标
        /// 1↑、2←、3↓、4→
        /// </summary>
        private readonly ResImage _menuIcon;

        /// <summary>
        /// 当前选项索引
        /// 1↑、2←、3↓、4→
        /// </summary>
        private int _selectedIndex = 1;

        #endregion 字段

        #region 构造函数

        public MainMenu(SimulatorContext context, CombatUI combatUI) : base(context, combatUI)
        {
            _combatUI = combatUI;
            _menuIcon = Context.LibData.GetImage(2, 1);
            _playerInfoBackground = Context.LibData.GetImage(2, 2);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            _menuIcon.Draw(canvas, _selectedIndex, 7, 96 - _menuIcon.Height);
            _playerInfoBackground.Draw(canvas, 1, 49, 66);
            PlayerCharacter p = _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex];
            _combatUI.HeadImgs[p.Index - 1].Draw(canvas, 1, 50, 63); // 角色头像
            if (p != null)
            {
                Context.Util.DrawSmallNum(canvas, p.HP, 79, 72); // hp
                Context.Util.DrawSmallNum(canvas, p.MaxHP, 104, 72); // maxhp
                Context.Util.DrawSmallNum(canvas, p.MP, 79, 83); // mp
                Context.Util.DrawSmallNum(canvas, p.MaxMP, 104, 83); // maxmp
            }
            _combatUI.PlayerIndicator.Draw(canvas, CombatUI.PlayerCharacterIndicatorPos[_combatUI.CurCharacterIndex].X, CombatUI.PlayerCharacterIndicatorPos[_combatUI.CurCharacterIndex].Y);
        }

        public override void OnKeyDown(int key)
        {
            switch (key)
            {
                case SimulatorKeys.KEY_LEFT:
                    if (_combatUI.PlayerCharacters[_combatUI.CurCharacterIndex].HasDebuff(CombatBuff.BUFF_MASK_FENG))
                    {
                        break; // 被封，不能用魔法
                    }
                    _selectedIndex = 2;
                    break;

                case SimulatorKeys.KEY_DOWN:
                    _selectedIndex = 3;
                    break;

                case SimulatorKeys.KEY_RIGHT:
                    if (_combatUI.PlayerCharacters.Count <= 1)
                    { // 只有一人不能合击
                        break;
                    }
                    _selectedIndex = 4;
                    break;

                case SimulatorKeys.KEY_UP:
                    _selectedIndex = 1;
                    break;
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

                switch (_selectedIndex)
                {
                    case 1://物理攻击
                           // 攻击全体敌人
                        if (_combatUI.PlayerCharacters[_combatUI.CurCharacterIndex].HasAtbuff(CombatBuff.BUFF_MASK_ALL))
                        {
                            //TODO 测试值是否改变？
                            _combatUI.OnActionSelected(new ActionPhysicalAttackAll(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], listMonsters));
                            break;
                        }

                        //TODO 这里肯定有问题！
                        // 攻击单个敌人
                        _combatUI.ScreenStack.Push(new MenuCharacterSelect(Context, _combatUI.MonsterIndicator, CombatUI.MonsterIndicatorPos, listMonsters,
                            (fc) =>
                            {
                                _combatUI.OnActionSelected(new ActionPhysicalAttackOne(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], fc));
                            }, true, _combatUI));
                        break;

                    case 2://魔法技能

                        Context.PushScreen(new ScreenMagic(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex].MagicChain,
                                (magic) =>
                                {
                                    Context.PopScreen();
                                    if ((magic is MagicAttack) || (magic is MagicSpecial))
                                    {
                                        // 选一个敌人
                                        if (magic.IsEffectAll)
                                        {
                                            _combatUI.OnActionSelected(new ActionMagicAttackAll(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex],
                                                        listMonsters, (MagicAttack)magic));
                                        }
                                        else
                                        {
                                            // 选一个敌人
                                            _combatUI.ScreenStack.Push(new MenuCharacterSelect(Context, _combatUI.MonsterIndicator, CombatUI.MonsterIndicatorPos, listMonsters,
                                                (fc) =>
                                                {
                                                    _combatUI.OnActionSelected(new ActionMagicAttackOne(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], fc, magic));
                                                }, true, _combatUI));
                                        }
                                    }
                                    else
                                    {
                                        // 选队友或自己
                                        if (magic.IsEffectAll)
                                        {
                                            _combatUI.OnActionSelected(new ActionMagicHelpAll(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], listPlayers, magic));
                                        }
                                        else
                                        {
                                            // 选一个Player
                                            _combatUI.ScreenStack.Push(new MenuCharacterSelect(Context, _combatUI.TargetIndicator, CombatUI.PlayerCharacterIndicatorPos, listPlayers,
                                                (fc) =>
                                                {
                                                    _combatUI.OnActionSelected(new ActionMagicHelpOne(Context, _combatUI.PlayerCharacters[_combatUI.CurCharacterIndex], fc, magic));
                                                }, !(magic is MagicAuxiliary), _combatUI));
                                        }
                                    }
                                }));
                        break;

                    case 3://杂项
                        _combatUI.ScreenStack.Push(new MenuMisc(Context, _combatUI));
                        break;

                    case 4://合击
                        _combatUI.ScreenStack.Push(new MenuCharacterSelect(Context, _combatUI.MonsterIndicator, CombatUI.MonsterIndicatorPos, listMonsters,
                                (fc) =>
                                {
                                    _combatUI.OnActionSelected(new ActionCoopMagic(Context, _combatUI.PlayerCharacters, fc));
                                }, true, _combatUI));
                        break;
                }
            }
            else if (key == SimulatorKeys.KEY_CANCEL)
            {
                _combatUI.OnCancel();
            }
        }

        public override void Update(long delta)
        {
            _combatUI.PlayerIndicator.Update(delta);
        }

        #endregion 方法
    }
}