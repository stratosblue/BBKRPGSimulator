using System;
using System.Collections.Generic;
using System.Drawing;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.View.Combat
{
    /// <summary>
    /// 角色标识，用于标记当前选择的角色
    /// </summary>
    internal class MenuCharacterSelect : BaseCombatScreen
    {
        #region 字段

        private int mCurSel;
        private bool mIgnoreDead;
        private FrameAnimation mIndicator;
        private Point[] mIndicatorPos;
        private List<FightingCharacter> mList;
        private Action<FightingCharacter> OnCharacterSelected;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 选择操作对象
        /// </summary>
        /// <param name="indicator">标记符的帧动画</param>
        /// <param name="pos">标记符的位置</param>
        /// <param name="list">角色链表</param>
        /// <param name="selectAction"></param>
        /// <param name="ignoreDead">跳过死亡角色</param>
        /// <param name="combatUI"></param>
        public MenuCharacterSelect(SimulatorContext context, FrameAnimation indicator, Point[] pos, List<FightingCharacter> list, Action<FightingCharacter> selectAction, bool ignoreDead, CombatUI combatUI) : base(context, combatUI)
        {
            _combatUI = combatUI;
            mIndicator = indicator;
            mIndicatorPos = pos;
            mList = list;
            OnCharacterSelected = selectAction;
            mIgnoreDead = ignoreDead;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsAlive)
                {
                    mCurSel = i;
                    break;
                }
            }
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            mIndicator.Draw(canvas, mIndicatorPos[mCurSel].X, mIndicatorPos[mCurSel].Y);
            if (mIndicator == _combatUI.TargetIndicator)
            { // 当前选择角色
                PlayerCharacter p = _combatUI.PlayerCharacters[mCurSel];
                _combatUI.HeadImgs[p.Index - 1].Draw(canvas, 1, 50, 63); // 角色头像
                if (p != null)
                {
                    Context.Util.DrawSmallNum(canvas, p.HP, 79, 72); // hp
                    Context.Util.DrawSmallNum(canvas, p.MaxHP, 104, 72); // maxhp
                    Context.Util.DrawSmallNum(canvas, p.MP, 79, 83); // mp
                    Context.Util.DrawSmallNum(canvas, p.MaxMP, 104, 83); // maxmp
                }
            }
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_RIGHT)
            {
                SelectNextTarget();
            }
            else if (key == SimulatorKeys.KEY_LEFT)
            {
                SelectPreTarget();
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_CANCEL)
            {
                _combatUI.ScreenStack.Pop();
            }
            else if (key == SimulatorKeys.KEY_ENTER)
            {
                _combatUI.ScreenStack.Pop();
                if (OnCharacterSelected != null)
                {
                    //mOnCharacterSelectedListener.onCharacterSelected(mList.get(mCurSel));
                    OnCharacterSelected(mList[mCurSel]);
                }
            }
        }

        public override void Update(long delta)
        {
            mIndicator.Update(delta);
        }

        private void SelectNextTarget()
        {
            do
            {
                ++mCurSel;
                mCurSel %= mList.Count;
            } while (mIgnoreDead && !mList[mCurSel].IsAlive);
        }

        private void SelectPreTarget()
        {
            do
            {
                --mCurSel;
                mCurSel = (mCurSel + mList.Count) % mList.Count;
            } while (mIgnoreDead && !mList[mCurSel].IsAlive);
        }

        #endregion 方法
    }
}