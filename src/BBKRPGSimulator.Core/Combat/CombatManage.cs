using System.IO;

using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Interface;
using BBKRPGSimulator.View.Combat;

namespace BBKRPGSimulator.Combat
{
    /// <summary>
    /// 战斗管理
    /// </summary>
    internal class CombatManage : ContextDependent, ICustomSerializeable
    {
        #region 字段

        /// <summary>
        /// 随机战斗
        /// </summary>
        private CombatScreen _randomCombat;

        /// <summary>
        /// 剧情战斗
        /// </summary>
        private CombatScreen _storyCombat;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 是否启用随机战斗
        /// </summary>
        public bool EnableRandomCombat { get; set; }

        /// <summary>
        /// 当前是否在战斗
        /// </summary>
        //public bool IsActive => _enableRandomCombat && (_randomCombat != null) && _isCombating;
        public bool IsActive => CurCombat?.IsCombating == true;

        /// <summary>
        /// 当前是否随机战斗
        /// </summary>
        public bool IsRandomCombat { get; private set; }

        /// <summary>
        /// 当前战斗
        /// </summary>
        private CombatScreen CurCombat
        {
            get
            {
                return IsRandomCombat ? _randomCombat : _storyCombat;
            }
        }

        #endregion 属性

        #region 方法

        /// <summary>
        /// 战斗管理
        /// </summary>
        public CombatManage(SimulatorContext context) : base(context)
        {
            _randomCombat = new CombatScreen(Context, this);
        }

        /// <summary>
        /// 进入剧情战斗
        /// </summary>
        /// <param name="roundMax">最多回合数，0为无限</param>
        /// <param name="monstersType">0-3 敌人</param>
        /// <param name="scr">0战斗背景，1左下角图，2右上角图</param>
        /// <param name="evtRnds">0-3 战斗中，触发事件的回合</param>
        /// <param name="evts">0-3 对应的事件号</param>
        /// <param name="lossto">战斗失败跳转的地址</param>
        /// <param name="winto">战斗成功跳转的地址</param>
        public void EnterStoryCombat(int roundMax, int[] monstersType, int[] scr, int[] evtRnds, int[] evts, int lossto, int winto)
        {
            IsRandomCombat = false;

            EnableRandomCombat = true;

            _storyCombat = new CombatScreen(Context, this);

            _storyCombat.EnterStoryCombat(roundMax, monstersType, scr, evtRnds, evts, lossto, winto);
        }

        /// <summary>
        /// 退出当前战斗
        /// </summary>
        public void ExitCurrentCombat(bool isWin)
        {
            if (IsRandomCombat)
            {
                if (!isWin)
                {
                    // 死了，游戏结束
                    Context.ChangeScreen(ScreenEnum.SCREEN_MENU);
                }
            }
            else
            {
                //剧情战斗结束，设置成为随机战斗
                IsRandomCombat = true;
                _storyCombat = null;
            }
        }

        /// <summary>
        /// 初始化并开启随即战斗
        /// </summary>
        /// <param name="monstersType">0-7 可能出现的敌人种类</param>
        /// <param name="scrb">战斗背景</param>
        /// <param name="scrl">左下角图</param>
        /// <param name="scrr">右上角图</param>
        public void InitRandomCombat(int[] monstersType, int scrb, int scrl, int scrr)
        {
            EnableRandomCombat = true;

            IsRandomCombat = true;

            _randomCombat = new CombatScreen(Context, this);

            _randomCombat.InitRandomCombat(monstersType, scrb, scrl, scrr);
        }

        /// <summary>
        /// 进入一个随机战斗
        /// </summary>
        /// <returns><code>true</code>新战斗 <code>false</code>不开始战斗</returns>
        public bool StartNewRandomCombat()
        {
            if (!EnableRandomCombat || _randomCombat == null || Context.Random.Next(Context.PlayContext.CombatProbability) != 0)
            {
                return false;
            }
            if (_randomCombat.EnableMonsterType == null || _randomCombat.EnableMonsterType.Length == 0)
            {
                return false;
            }

            EnableRandomCombat = true;

            _randomCombat.StartNewRandomCombat();

            return true;
        }

        #region 常规操作

        public void Deserialize(BinaryReader binaryReader)
        {
            EnableRandomCombat = binaryReader.ReadBoolean();

            if (EnableRandomCombat)
            {
                var enableMonsterTypeCount = binaryReader.ReadInt32();

                var enableMonsterType = new int[enableMonsterTypeCount];

                if (enableMonsterTypeCount > 0)
                {
                    for (int i = 0; i < enableMonsterTypeCount; i++)
                    {
                        enableMonsterType[i] = binaryReader.ReadInt32();
                    }
                }

                int scrb = binaryReader.ReadInt32();
                int scrl = binaryReader.ReadInt32();
                int scrr = binaryReader.ReadInt32();

                InitRandomCombat(enableMonsterType, scrb, scrl, scrr);
            }
        }

        public void Draw(ICanvas canvas)
        {
            CurCombat?.Draw(canvas);
        }

        public void KeyDown(int key)
        {
            CurCombat?.OnKeyDown(key);
        }

        public void KeyUp(int key)
        {
            CurCombat?.OnKeyUp(key);
        }

        public void Serialize(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(EnableRandomCombat);

            if (EnableRandomCombat)
            {
                var enableMonsterTypeCount = (_randomCombat.EnableMonsterType?.Length).GetValueOrDefault(0);

                binaryWriter.Write(enableMonsterTypeCount);

                if (enableMonsterTypeCount > 0)
                {
                    for (int i = 0; i < enableMonsterTypeCount; i++)
                    {
                        binaryWriter.Write(_randomCombat.EnableMonsterType[i]);
                    }
                }

                binaryWriter.Write(_randomCombat.ScrBottomIndex);
                binaryWriter.Write(_randomCombat.ScrLeftIndex);
                binaryWriter.Write(_randomCombat.ScrRightIndex);
            }
        }

        public void Update(long delta)
        {
            CurCombat?.Update(delta);
        }

        #endregion 常规操作

        #endregion 方法
    }
}