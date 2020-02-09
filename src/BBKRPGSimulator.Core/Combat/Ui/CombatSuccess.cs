using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.View;
using BBKRPGSimulator.View.Combat;

namespace BBKRPGSimulator.Combat.Ui
{
    /// <summary>
    /// 战斗胜利
    /// </summary>
    internal class CombatSuccess : ContextDependent
    {
        #region 字段

        /// <summary>
        /// 获取的物品列表
        /// </summary>
        private List<BaseGoods> _gainGoods;

        /// <summary>
        /// 是否有任意键按下
        /// </summary>
        private bool _isAnyKeyPressed = false;

        /// <summary>
        /// 等级提升界面列表
        /// </summary>
        private List<BaseScreen> _levelUpScreens;

        /// <summary>
        /// 消息界面列表
        /// </summary>
        private List<BaseScreen> _msgScreens;

        /// <summary>
        /// 显示计时
        /// </summary>
        private long _timeCount;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 战斗胜利
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exp"></param>
        /// <param name="money"></param>
        /// <param name="goodslist"></param>
        /// <param name="characters">等级提升的角色列表</param>
        public CombatSuccess(SimulatorContext context, int exp, int money, List<BaseGoods> goodslist, List<PlayerCharacter> characters) : base(context)
        {
            _gainGoods = goodslist;
            _msgScreens = new List<BaseScreen>();
            string estr = exp.ToString();
            _msgScreens.Add(new MsgScreen(Context, 18, "获得经验     ".Substring(0, 9 - estr.Length) + estr));

            string mstr = money.ToString();
            _msgScreens.Add(new MsgScreen(Context, 46, "战斗获得        ".Substring(0, 10 - mstr.Length) + mstr + "钱"));

            _levelUpScreens = new List<BaseScreen>();

            foreach (PlayerCharacter p in characters)
            {
                _levelUpScreens.Add(new MsgScreen(Context, p.Name + "修行提升"));
                _levelUpScreens.Add(new LevelupScreen(Context, p));
                if (p.LevelupChain.GetLearnMagicNum(p.Level) >
                p.LevelupChain.GetLearnMagicNum(p.Level - 1))
                {
                    _levelUpScreens.Add(new LearnMagicScreen(Context, p.Name, p.MagicChain[p.LevelupChain.GetLearnMagicNum(p.Level) - 1].Name));
                }
            }
        }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas)
        {
            foreach (BaseScreen msgScreen in _msgScreens)
            {
                msgScreen.Draw(canvas);
            }
        }

        public void OnKeyDown(int key)
        {
        }

        public void OnKeyUp(int key)
        {
            _isAnyKeyPressed = true;
        }

        /// <summary>
        /// 返回true所有内容显示完毕
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool Update(long delta)
        {
            _timeCount += delta;
            if (_timeCount > 1000 || _isAnyKeyPressed)
            {
                _timeCount = 0;
                _isAnyKeyPressed = false;
                if (_gainGoods.Count == 0)
                {
                    if (_levelUpScreens.Count == 0)
                    {
                        return true;
                    }
                    else
                    {
                        _msgScreens.Add(_levelUpScreens[0]);
                        _levelUpScreens.RemoveAt(0);
                    }
                }
                else
                {
                    BaseGoods goods = _gainGoods[0];
                    _gainGoods.RemoveAt(0);
                    _msgScreens.Add(new MsgScreen(Context, "得到 " + goods.Name + " x" + goods.GoodsNum));
                }
            }
            return false;
        }

        #endregion 方法
    }
}