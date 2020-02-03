using System.Collections.Generic;

using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Combat.Anim
{
    /// <summary>
    /// 数字飘起动画
    /// </summary>
    internal class RaiseAnimation : ContextDependent
    {
        #region 字段

        private bool bShowNum;
        private long cnt = 0;
        private int dy = 0, dt = 0;
        private ImageBuilder raiseNum;
        private List<ResSrs> srsList;

        #endregion 字段

        #region 属性

        public int X { get; set; }

        public int Y { get; set; }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 数字飘起动画
        /// </summary>
        /// <param name="context"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="hitpoint"></param>
        /// <param name="buff"></param>
        public RaiseAnimation(SimulatorContext context, int x, int y, int hitpoint, CombatBuff buff) : base(context)
        {
            this.X = x;
            this.Y = y;
            raiseNum = Context.Util.GetSmallSignedNumBitmap(hitpoint);
            this.bShowNum = hitpoint != 0;
            srsList = new List<ResSrs>();
            if (buff.HasFlag(CombatBuff.BUFF_MASK_DU))
            {
                srsList.Add(Context.LibData.GetSrs(1, 243));
                srsList[srsList.Count - 1].StartAni();
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_LUAN))
            {
                srsList.Add(Context.LibData.GetSrs(1, 244));
                srsList[srsList.Count - 1].StartAni();
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_FENG))
            {
                srsList.Add(Context.LibData.GetSrs(1, 245));
                srsList[srsList.Count - 1].StartAni();
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_MIAN))
            {
                srsList.Add(Context.LibData.GetSrs(1, 246));
                srsList[srsList.Count - 1].StartAni();
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_GONG))
            {
                srsList.Add(Context.LibData.GetSrs(1, 240));
                srsList[srsList.Count - 1].StartAni();
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_FANG))
            {
                srsList.Add(Context.LibData.GetSrs(1, 241));
                srsList[srsList.Count - 1].StartAni();
            }
            if (buff.HasFlag(CombatBuff.BUFF_MASK_SU))
            {
                srsList.Add(Context.LibData.GetSrs(1, 242));
                srsList[srsList.Count - 1].StartAni();
            }
        }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas)
        {
            if (bShowNum)
            {
                canvas.DrawBitmap(raiseNum, X, Y + dy);
            }
            else
            {
                if (srsList.Count > 0)
                {
                    srsList[0].DrawAbsolutely(canvas, X, Y);
                }
            }
        }

        /// <summary>
        /// 播放完成返回false，否则返回true
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool Update(long delta)
        {
            if (bShowNum)
            {
                cnt += delta;
                if (cnt > 50)
                {
                    cnt = 0;
                    ++dt;
                    dy -= dt;
                    if (dt > 4)
                    {
                        bShowNum = false;
                    }
                }
            }
            else
            {
                if (srsList.Count <= 0)
                {
                    return false;
                }
                else
                {
                    if (!srsList[0].Update(delta))
                    {
                        srsList.RemoveAt(0);
                        return !(srsList.Count <= 0);
                    }
                }
            }
            return true;
        }

        #endregion 方法
    }
}