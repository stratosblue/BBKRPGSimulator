using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Actions;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.View.Combat
{
    internal class CombatContext
    {
    }

    /// <summary>
    /// 战斗界面
    /// </summary>
    internal class CombatUI : BaseScreen
    {
        #region 静态定义

        /// <summary>
        /// 怪物显示位置
        /// </summary>
        public static readonly Point[] MonsterIndicatorPos = new Point[] { new Point(16, 14), new Point(48, 3), new Point(86, 0) };

        /// <summary>
        /// 玩家角色显示位置
        /// </summary>
        public static readonly Point[] PlayerCharacterIndicatorPos = new Point[] { new Point(69, 45), new Point(101, 41), new Point(133, 33) };

        #endregion 静态定义

        #region 字段

        public ICallBack mCallBack;

        /// <summary>
        /// 当前角色索引
        /// </summary>
        public int CurCharacterIndex { get; set; }

        /// <summary>
        /// 角色头像
        /// </summary>
        public ResImage[] HeadImgs { get; set; }

        /// <summary>
        /// 标记action作用的敌人角色
        /// </summary>
        public FrameAnimation MonsterIndicator { get; }

        public List<Monster> Monsters { get; set; }

        public List<PlayerCharacter> PlayerCharacters { get; set; }

        /// <summary>
        /// 标记发出的action的玩家角色
        /// </summary>
        public FrameAnimation PlayerIndicator { get; set; }

        public List<BaseScreen> ScreenStack { get; } = new List<BaseScreen>();

        /// <summary>
        /// 标记action作用的玩家角色
        /// </summary>
        public FrameAnimation TargetIndicator { get; }

        #endregion 字段

        #region 构造函数

        public CombatUI(SimulatorContext context, ICallBack callBack, int curPlayerIndex) : base(context)
        {
            mCallBack = callBack;
            CurCharacterIndex = curPlayerIndex;
            ScreenStack.Push(new MainMenu(Context, this));

            ResImage tmpImg;
            tmpImg = Context.LibData.GetImage(2, 4);
            PlayerIndicator = new FrameAnimation(tmpImg, 1, 2);
            TargetIndicator = new FrameAnimation(tmpImg, 3, 4);
            tmpImg = Context.LibData.GetImage(2, 3);
            MonsterIndicator = new FrameAnimation(tmpImg);

            HeadImgs = new ResImage[]{
            Context.LibData.GetImage(1, 1),
            Context.LibData.GetImage(1, 2),
            Context.LibData.GetImage(1, 3)};
        }

        #endregion 构造函数

        #region 接口

        public interface ICallBack
        {
            #region 方法

            /// <summary>
            /// 当一个Action被选择后，会调用此方法
            /// </summary>
            /// <param name="action"></param>
            void OnActionSelected(ActionBase action);

            /// <summary>
            /// 选择围攻时，调用改方法
            /// </summary>
            void OnAutoAttack();

            /// <summary>
            /// 取消选择当前角色的Action，应该返回选择上一个角色的Action
            /// </summary>
            void OnCancel();

            /// <summary>
            /// 选择逃跑时，调用该方法。对于已经做出决策的角色，其决策不变；之后的角色动作皆为逃跑
            /// </summary>
            void OnFlee();

            #endregion 方法
        }

        #endregion 接口

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            for (int i = 0; i < ScreenStack.Count; i++)
            {
                try
                {
                    ScreenStack[i].Draw(canvas);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// helper for the callback interface
        /// </summary>
        public void OnActionSelected(ActionBase action)
        {
            if (mCallBack != null)
            {
                mCallBack.OnActionSelected(action);
            }
        }

        /// <summary>
        /// helper for the callback interface
        /// </summary>
        public void OnCancel()
        {
            if (mCallBack != null)
            {
                mCallBack.OnCancel();
            }
        }

        public override void OnKeyDown(int key)
        {
            BaseScreen bs = ScreenStack.Peek();
            if (bs != null)
            {
                bs.OnKeyDown(key);
            }
        }

        public override void OnKeyUp(int key)
        {
            BaseScreen bs = ScreenStack.Peek();
            if (bs != null)
            {
                bs.OnKeyUp(key);
            }
        }

        public void Reset()
        {
            ScreenStack.Clear();
            ScreenStack.Push(new MainMenu(Context, this));
        }

        public void SetCurrentPlayerIndex(int i)
        {
            CurCharacterIndex = i;
        }

        public void SetMonsterList(List<Monster> list)
        {
            Monsters = list;
        }

        public void SetPlayerList(List<PlayerCharacter> list)
        {
            PlayerCharacters = list;
        }

        public override void Update(long delta)
        {
            for (int i = 0; i < ScreenStack.Count; i++)
            {
                try
                {
                    ScreenStack[i].Update(delta);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        #endregion 方法
    }
}