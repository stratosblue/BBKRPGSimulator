using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Actions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.View.Combat;

namespace BBKRPGSimulator.Combat
{
    internal class ActionExecutor
    {
        #region 字段

        /// <summary>
        /// 被执行的动作队列
        /// </summary>
        private List<ActionBase> mActionQueue;

        private CombatScreen mCombat;

        /// <summary>
        /// 当前执行的动作
        /// </summary>
        private ActionBase mCurrentAction;

        private bool mIsNewAction = true;

        #endregion 字段

        #region 构造函数

        public ActionExecutor(List<ActionBase> actionQueue, CombatScreen combat)
        {
            mActionQueue = actionQueue;
            mCombat = combat;
        }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas)
        {
            if (mCurrentAction != null)
            {
                mCurrentAction.Draw(canvas);
            }
        }

        public void Reset()
        {
            mCurrentAction = null;
            mIsNewAction = true;
        }

        /// <summary>
        ///
        /// @param delta
        /// @return 执行完毕返回<code>false</code>，否则返回<code>true</code>
        /// </summary>
        public bool Update(long delta)
        {
            if (mCurrentAction == null)
            {
                mCurrentAction = mActionQueue.Dequeue();
                if (mCurrentAction == null)
                {
                    return false;
                }
                mCurrentAction.PreProccess();
                mIsNewAction = false;
            }

            if (mIsNewAction)
            { // 跳过死亡角色
                if (!FixAction())
                {
                    return false;
                }
                mCurrentAction.PreProccess();
                mIsNewAction = false;
            }

            if (!mCurrentAction.Update(delta))
            { // 当前动作执行完毕
                mCurrentAction.PostExecute();
                mCurrentAction = mActionQueue.Dequeue(); // 取下一个动作
                if (mCurrentAction == null)
                { // 所有动作执行完毕
                    return false;
                }
                mIsNewAction = true;
            }

            return true;
        }

        /// <summary>
        /// 执行完毕返回<code>false</code>
        /// </summary>
        private bool FixAction()
        {
            // attacker dead, goto next action
            while (!mCurrentAction.IsAttackerAlive())
            {
                mCurrentAction = mActionQueue.Dequeue();
                if (mCurrentAction == null)
                {
                    return false;
                }
            }

            // target dead, get an alive target
            if (!mCurrentAction.IsTargetAlive())
            {
                if (mCurrentAction.IsTargetsMoreThanOne())
                { // 敌人都死了
                    return false;
                }
                else
                { // try to find an alive target
                    FightingCharacter newTarget = null;
                    if (mCurrentAction.TargetIsMonster())
                    {
                        newTarget = mCombat.GetFirstAliveMonster();
                    }
                    else
                    {
                        newTarget = mCombat.GetRandomAlivePlayer();
                    }

                    if (newTarget == null)
                    {
                        return false;
                    }
                    else if (!(mCurrentAction is ActionFlee))
                    {
                        ((ActionSingleTarget)mCurrentAction).SetTarget(newTarget);
                    }
                }
            }

            return true;
        }

        #endregion 方法
    }
}