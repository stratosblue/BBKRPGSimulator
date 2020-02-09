using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat;
using BBKRPGSimulator.Combat.Actions;
using BBKRPGSimulator.Combat.Ui;
using BBKRPGSimulator.Definitions;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.View.Combat
{
    internal class CombatScreen : BaseScreen, CombatUI.ICallBack
    {
        #region 字段

        /// <summary>
        /// 玩家角色中心坐标
        /// </summary>
        public static readonly Point[] PlayerPos = new Point[] { new Point(64 + 12, 52 + 18), new Point(96 + 12, 48 + 18), new Point(128 + 12, 40 + 18) };

        /// <summary>
        /// 战斗管理
        /// </summary>
        private readonly CombatManage _combatManage;

        /// <summary>
        /// 动作队列的执行者
        /// </summary>
        private ActionExecutor _actionExecutor;

        /// <summary>
        /// 动作队列，一个回合中，双方的决策
        /// </summary>
        private List<ActionBase> _actionQueue = new List<ActionBase>();

        /// <summary>
        /// 战斗背景图
        /// </summary>
        private ImageBuilder _background;

        /// <summary>
        /// 战斗状态
        /// </summary>
        private CombatState _combatState = CombatState.SelectAction;

        /// <summary>
        /// 战斗胜利界面
        /// </summary>
        private CombatSuccess _combatSuccess;

        /// <summary>
        /// 战斗的UI
        /// </summary>
        private CombatUI _combatUI;

        /// <summary>
        /// 当前选择动作的角色在PlayerList中的序号
        /// </summary>
        private int _curSelActionPlayerIndex = 0;

        /// <summary>
        /// 随机战斗中，可能出现的敌人类型
        /// </summary>
        private int[] _enableMonsterType;

        /// <summary>
        /// 触发事件的回合，以及对应的事件号
        /// </summary>
        private int[] _eventRound, _eventNum;

        /// <summary>
        /// 事件是否已触发
        /// </summary>
        private bool _hasEventExed;

        /// <summary>
        /// 是否自动攻击，围攻状态
        /// </summary>
        private bool _isAutoAttack = false;

        /// <summary>
        /// 是否胜利
        /// </summary>
        private bool _isWin = false;

        /// <summary>
        /// 战斗失败跳转地址，战斗成功跳转地址
        /// </summary>
        private int _lossAddr, _winAddr;

        /// <summary>
        /// 最多回合数，0为无限
        /// </summary>
        private int _maxRound;

        /// <summary>
        /// 参加战斗的怪物队列
        /// </summary>
        private List<Monster> _monsterList = new List<Monster>();

        /// <summary>
        /// 参加战斗的玩家角色队列
        /// </summary>
        private List<PlayerCharacter> _playerList;

        /// <summary>
        /// 飞桃花-死亡动画
        /// </summary>
        private ResSrs _resFlyPeach;

        /// <summary>
        /// 当前回合
        /// </summary>
        private int _roundCount;

        /// <summary>
        /// _scrBottomIndex战斗背景，_scrLeftIndex左下角图，_scrRightIndex右上角图
        /// </summary>
        private int _scrBottomIndex, _scrLeftIndex, _scrRightIndex;

        /// <summary>
        /// 更新计时
        /// </summary>
        private long _timeCount = 0;

        /// <summary>
        /// 战斗胜利能获得的金钱和经验
        /// </summary>
        private int _winMoney, _winExp;

        public int[] EnableMonsterType { get => _enableMonsterType; }

        /// <summary>
        /// 是否正在战斗
        /// </summary>
        public bool IsCombating { get; private set; }

        public int ScrBottomIndex { get => _scrBottomIndex; }
        public int ScrLeftIndex { get => _scrLeftIndex; }
        public int ScrRightIndex { get => _scrRightIndex; }

        #endregion 字段

        #region 构造函数

        public CombatScreen(SimulatorContext context, CombatManage combatManage) : base(context)
        {
            _combatManage = combatManage ?? throw new ArgumentNullException("combatManage不可为空");

            //TODO 这里换成了直接用List，不知道会不会有问题。
            _actionExecutor = new ActionExecutor(_actionQueue, this);

            _resFlyPeach = Context.LibData.GetSrs(1, 249);

            _combatUI = new CombatUI(context, this, 0);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_background, 0, 0);

            // draw the monsters and players
            for (int i = 0; i < _monsterList.Count; i++)
            {
                FightingCharacter fc = _monsterList[i];
                if (fc.IsVisiable)
                {
                    fc.FightingSprite.Draw(canvas);
                }
            }

            for (int i = _playerList.Count - 1; i >= 0; i--)
            {
                FightingSprite f = _playerList[i].FightingSprite;
                f.Draw(canvas);
            }

            if (_combatState == CombatState.SelectAction && !_isAutoAttack)
            {
                _combatUI.Draw(canvas);
            }
            else if (_combatState == CombatState.PerformAction)
            {
                _actionExecutor.Draw(canvas);
            }
            else if (_combatState == CombatState.Win)
            {
                //			TextRender.drawText(canvas, "Win", 20, 40);
                _combatSuccess.Draw(canvas);
            }
            else if (_combatState == CombatState.Loss && _combatManage.IsRandomCombat)
            {
                //			TextRender.drawText(canvas, "Loss", 20, 40);
                _resFlyPeach.Draw(canvas, 0, 0);
            }
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
            _monsterList = new List<Monster>();
            for (int i = 0; i < monstersType.Length; ++i)
            {
                if (monstersType[i] > 0)
                {
                    Monster tmp = Context.LibData.GetCharacter(3, monstersType[i]) as Monster;
                    if (tmp != null)
                    {
                        _monsterList.Add(tmp);
                    }
                }
            }

            _maxRound = roundMax;
            _roundCount = 0;

            PrepareForNewCombat();

            CreateBackgroundBitmap(scr[0], scr[1], scr[2]);

            _eventRound = evtRnds;
            _eventNum = evts;

            _lossAddr = lossto;
            _winAddr = winto;

            IsCombating = true;
        }

        /// <summary>
        /// 第一个活着的怪物，
        /// </summary>
        /// <returns></returns>
        public Monster GetFirstAliveMonster()
        {
            foreach (var item in _monsterList)
            {
                if (item.IsAlive)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// 随机获取一个活着的玩家角色
        /// 全部死亡时返回null
        /// </summary>
        /// <returns>null全死了</returns>
        public PlayerCharacter GetRandomAlivePlayer()
        {
            if (_playerList.Count > 1)
            {
                return _playerList.Where(m => m.IsAlive).OrderBy(m => Context.Random.Next()).FirstOrDefault();
            }
            else
            {
                return _playerList.FirstOrDefault();
            }
        }

        /// <summary>
        /// 初始化随机战斗
        /// </summary>
        /// <param name="monstersType">0-7 可能出现的敌人种类</param>
        /// <param name="scrb">战斗背景</param>
        /// <param name="scrl">左下角图</param>
        /// <param name="scrr">右上角图</param>
        public void InitRandomCombat(int[] monstersType, int scrb, int scrl, int scrr)
        {
            IsCombating = false;
            _enableMonsterType = monstersType.Where(m => m > 0).ToArray();

            _roundCount = 0;
            _maxRound = 0; // 回合数无限制

            CreateBackgroundBitmap(scrb, scrl, scrr);
        }

        public void OnActionSelected(ActionBase action)
        {
            _actionQueue.Add(action);

            _combatUI.Reset(); // 重置战斗UI

            if (action is ActionCoopMagic)
            { // 只保留合击
                _actionQueue.Clear();
                _actionQueue.Add(action);
                GenerateMonstersActions();
                SortActionQueue();
                _combatState = CombatState.PerformAction;
            }
            else if (_curSelActionPlayerIndex < 0 || _curSelActionPlayerIndex >= _playerList.Count - 1 || IsPlayerBehindDead(_curSelActionPlayerIndex))
            { // 全部玩家角色的动作选择完成
                GenerateMonstersActions();
                SortActionQueue();
                _combatState = CombatState.PerformAction; // 开始执行动作队列
            }
            else
            { // 选择下一个玩家角色的动作
                _curSelActionPlayerIndex = GetNextAlivePlayerIndex();
                //TODO 乱眠死不能自己选择action
                if (_curSelActionPlayerIndex > 0 &&
                    (!_playerList[_curSelActionPlayerIndex].HasDebuff(CombatBuff.BUFF_MASK_MIAN) &&
                    !_playerList[_curSelActionPlayerIndex].HasDebuff(CombatBuff.BUFF_MASK_LUAN)))
                {
                    _combatUI.SetCurrentPlayerIndex(_curSelActionPlayerIndex);
                }
            }
        }

        public void OnAutoAttack()
        {
            // clear all the actions that has been selected, enter into auto fight mode
            _combatUI.Reset();
            _actionQueue.Clear();
            _isAutoAttack = true;
            _combatState = CombatState.SelectAction;
        }

        public void OnCancel()
        {
            int i = GetPreAlivePlayerIndex();
            if (i >= 0)
            { // 不是第一个角色
              // 重选上一个角色的动作
                _actionQueue.RemoveAt(_actionQueue.Count - 1);
                _curSelActionPlayerIndex = i;
                _combatUI.SetCurrentPlayerIndex(_curSelActionPlayerIndex);

                _combatUI.Reset();
            }
        }

        public void OnFlee()
        {
            // TODO add flee action to all the other actor

            _combatUI.Reset(); // 重置战斗UI

            for (int i = _curSelActionPlayerIndex; i < _playerList.Count; i++)
            {
                if (_playerList[i].IsAlive && Context.Random.Next(2) == 0 && _combatManage.IsRandomCombat)
                {
                    // 50% 逃走
                    _actionQueue.Add(new ActionFlee(Context, _playerList[i], true, () =>
                    {
                        // 逃跑成功后执行
                        _isWin = true;
                        _combatState = CombatState.Exit;
                    }));
                    break;
                }
                else
                { // 逃跑失败
                    _actionQueue.Add(new ActionFlee(Context, _playerList[i], false, null));
                }
            }
            GenerateMonstersActions();

            SortActionQueue();
            _combatState = CombatState.PerformAction;
        }

        public override void OnKeyDown(int key)
        {
            if (_combatState == CombatState.SelectAction)
            {
                if (!_isAutoAttack)
                {
                    _combatUI.OnKeyDown(key);
                }
            }
            else if (_combatState == CombatState.Win)
            {
                _combatSuccess.OnKeyDown(key);
            }
        }

        public override void OnKeyUp(int key)
        {
            if (_combatState == CombatState.SelectAction)
            {
                if (!_isAutoAttack)
                {
                    _combatUI.OnKeyUp(key);
                }
            }
            else if (_combatState == CombatState.Win)
            {
                _combatSuccess.OnKeyUp(key);
            }

            if (_isAutoAttack && key == SimulatorKeys.KEY_CANCEL)
            { // 退出“围攻”模式
                _isAutoAttack = false;
            }
        }

        /// <summary>
        /// 准备新的战斗？
        /// </summary>
        public void PrepareForNewCombat()
        {
            _actionQueue.Clear();

            _isAutoAttack = false;
            _combatState = CombatState.SelectAction;

            _curSelActionPlayerIndex = 0;
            _playerList = Context.PlayContext.PlayerCharacters;

            _combatUI.Reset();
            _combatUI.SetCurrentPlayerIndex(0);
            _combatUI.SetMonsterList(_monsterList);
            _combatUI.SetPlayerList(_playerList);

            SetOriginalPlayerPos();
            SetOriginalMonsterPos();

            _roundCount = 0;

            _hasEventExed = false;

            // 检查玩家血量
            foreach (var item in _playerList)
            {
                if (item.HP <= 0)
                {
                    // 确保血量大于0
                    item.HP = 1;
                }
                item.SetFrameByState();
            }

            // 怪物血量设置为其最大值
            foreach (var item in _monsterList)
            {
                item.HP = item.MaxHP;
            }

            // 计算战斗胜利能获得的金钱和经验
            _winMoney = 0;
            _winExp = 0;
            foreach (var monster in _monsterList)
            {
                _winMoney += monster.Money;
                _winExp += monster.EXP;
            }

            if (!_combatManage.IsRandomCombat && _monsterList.Count == 1)
            {
                // 剧情战斗，只有一个怪时，怪的位置在中间
                Monster m = _monsterList[0];
                Monster n = Context.LibData.GetCharacter(m.Type, m.Index) as Monster;
                n.HP = -1;
                n.IsVisiable = false;
                _monsterList.Insert(0, n); // 加入一个看不见的怪
                SetOriginalMonsterPos(); // 重置位置
            }

            _resFlyPeach.StartAni();
            _resFlyPeach.SetIteratorNum(5);
        }

        /// <summary>
        /// 进入一个随机战斗
        /// </summary>
        /// <returns><code>true</code>新战斗 <code>false</code>不开始战斗</returns>
        public bool StartNewRandomCombat()
        {
            // 打乱怪物类型
            for (int i = _enableMonsterType.Length - 1; i > 1; --i)
            {
                int r = Context.Random.Next(i);

                int t = _enableMonsterType[i];
                _enableMonsterType[i] = _enableMonsterType[r];
                _enableMonsterType[r] = t;
            }

            // 随机添加怪物
            _monsterList.Clear();
            for (int i = Context.Random.Next(3), j = 0; i >= 0; i--)
            {
                Monster m = Context.LibData.GetCharacter(3, _enableMonsterType[j++]) as Monster;
                if (m != null)
                {
                    _monsterList.Add(m);
                }
            }

            _roundCount = 0;
            _maxRound = 0; // 回合不限

            PrepareForNewCombat();

            IsCombating = true;
            return true;
        }

        public override void Update(long delta)
        {
            _timeCount += delta;
            switch (_combatState)
            {
                case CombatState.SelectAction:
                    if (!_hasEventExed && !_combatManage.IsRandomCombat)
                    {
                        _hasEventExed = true;
                        for (int i = 0; i < _eventRound.Length; i++)
                        {
                            if (_roundCount == _eventRound[i] && _eventNum[i] != 0)
                            {
                                Context.ScriptProcess.TriggerEvent(_eventNum[i]);
                            }
                        }
                    }
                    if (_isAutoAttack)
                    {
                        // 自动生成动作队列
                        GenerateAutoActionQueue();
                        _combatState = CombatState.PerformAction;
                    }
                    else
                    {
                        // 玩家决策
                        _combatUI.Update(delta);
                    }
                    break;

                case CombatState.PerformAction:
                    if (!_actionExecutor.Update(delta))
                    {
                        // 动作执行完毕
                        if (IsAllMonsterDead())
                        {
                            // 怪物全挂
                            _timeCount = 0; // 计时器清零
                            _combatState = CombatState.Win;

                            Context.PlayContext.Money += _winMoney; // 获得金钱
                            List<PlayerCharacter> lvuplist = new List<PlayerCharacter>();

                            foreach (var item in _playerList)
                            {
                                // 获得经验
                                if (item.IsAlive)
                                {
                                    //TODO 完成连续升级
                                    if (item.Level >= item.LevelupChain.MaxLevel) // 满级
                                        break;
                                    int nextExp = item.LevelupChain.GetNextLevelExp(item.Level);
                                    int exp = _winExp + item.CurrentExp;
                                    if (exp < nextExp)
                                    {
                                        item.GainExperience(_winExp);
                                    }
                                    else
                                    {
                                        // 升级
                                        int cl = item.Level; // 当前等级
                                        ResLevelupChain c = item.LevelupChain;
                                        item.GainExperience(_winExp);
                                        item.Level = cl + 1;
                                        item.MaxHP = item.MaxHP + c.GetMaxHP(cl + 1) - c.GetMaxHP(cl);
                                        //								p.HP = p.getMaxHP(); CombatSuccess 中设置
                                        item.MaxMP = item.MaxMP + c.GetMaxMP(cl + 1) - c.GetMaxMP(cl);
                                        //								p.MP = p.getMaxMP(); CombatSuccess 中设置
                                        item.Attack = item.Attack + c.GetAttack(cl + 1) - c.GetAttack(cl);
                                        item.Defend = item.Defend + c.GetDefend(cl + 1) - c.GetDefend(cl);

                                        if (item.MagicChain is ResMagicChain magicChain)
                                        {
                                            magicChain.LearnFromChain(c.GetLearnMagicNum(cl + 1));
                                        }

                                        item.Speed = item.Speed + c.GetSpeed(cl + 1) - c.GetSpeed(cl);
                                        item.Lingli = item.Lingli + c.GetLingli(cl + 1) - c.GetLingli(cl);
                                        item.Luck = item.Luck + c.GetLuck(cl + 1) - c.GetLuck(cl);
                                        lvuplist.Add(item);
                                    }
                                }
                            }

                            // 最大幸运值
                            int ppt = 10;
                            foreach (var item in _playerList)
                            {
                                if (item.Luck > ppt)
                                {
                                    ppt = item.Luck;
                                }
                            }

                            ppt -= 10;
                            if (ppt > 100)
                            {
                                ppt = 100;
                            }
                            else if (ppt < 0)
                            {
                                ppt = 10;
                            }

                            // 战利品链表
                            GoodsManage gm = new GoodsManage(Context);
                            List<BaseGoods> gl = new List<BaseGoods>();

                            foreach (var item in _monsterList)
                            {
                                BaseGoods g = item.GetDropGoods();
                                if (g != null && Context.Random.Next(101) < ppt)
                                {
                                    //  ppt%掉率
                                    gm.AddGoods(g.Type, g.Index, g.GoodsNum);
                                    Context.GoodsManage.AddGoods(g.Type, g.Index, g.GoodsNum); // 添加到物品链表
                                }
                            }

                            gl.AddRange(gm.GoodsList);
                            gl.AddRange(gm.EquipList);
                            _combatSuccess = new CombatSuccess(Context, _winExp, _winMoney, gl, lvuplist); // 显示玩家的收获
                        }
                        else
                        {
                            // 还有怪物存活
                            if (IsAnyPlayerAlive())
                            {
                                // 有玩家角色没挂，继续打怪
                                ++_roundCount;
                                UpdateFighterState();
                                _combatState = CombatState.SelectAction;
                                _curSelActionPlayerIndex = GetFirstAlivePlayerIndex();
                                _combatUI.SetCurrentPlayerIndex(_curSelActionPlayerIndex);
                                foreach (var item in _playerList)
                                {
                                    item.SetFrameByState();
                                }
                            }
                            else
                            {
                                // 玩家角色全挂，战斗失败
                                _timeCount = 0;
                                _combatState = CombatState.Loss;
                            }
                        }
                    }
                    break;

                case CombatState.Win:
                    // TODO if (winAddr...)
                    //			if (mTimeCnt > 1000) {
                    //				mCombatState = CombatState.Exit;
                    //			}
                    _isWin = true;
                    if (_combatSuccess.Update(delta))
                    {
                        _combatState = CombatState.Exit;
                    }
                    break;

                case CombatState.Loss:
                    // TODO if (lossAddr...)
                    if (_combatManage.IsRandomCombat && _resFlyPeach.Update(delta))
                    {
                    }
                    else
                    {
                        _isWin = false;
                        _combatState = CombatState.Exit;
                    }
                    break;

                case CombatState.Exit:
                    ExitCurrentCombat();
                    break;
            }
        }

        /// <summary>
        /// 创建背景图片
        /// </summary>
        /// <param name="scrb"></param>
        /// <param name="scrl"></param>
        /// <param name="scrr"></param>
        private void CreateBackgroundBitmap(int scrb, int scrl, int scrr)
        {
            _background = Context.GraphicsFactory.NewImageBuilder(160, 96);
            ICanvas canvas = Context.GraphicsFactory.NewCanvas(_background); ;

            ResImage img = Context.LibData.GetImage(4, scrb);
            if (img != null)    // 背景
            {
                img.Draw(canvas, 1, 0, 0);
            }

            img = Context.LibData.GetImage(4, scrl);
            if (img != null)    // 左下角
            {
                img.Draw(canvas, 1, 0, 96 - img.Height);
            }

            img = Context.LibData.GetImage(4, scrr);
            if (img != null)    // 右上角
            {
                img.Draw(canvas, 1, 160 - img.Width, 0);
            }

            _scrBottomIndex = scrb;
            _scrLeftIndex = scrl;
            _scrRightIndex = scrr;
        }

        /// <summary>
        /// 退出当前战斗
        /// </summary>
        private void ExitCurrentCombat()
        {
            if (!_combatManage.IsRandomCombat)
            {
                Context.ScriptProcess.GotoAddress(_isWin ? _winAddr : _lossAddr);
                Context.ScriptProcess.EnableExecuteScript = true;
            }
            else
            {
                if (!_isWin)
                {
                    // 死了，游戏结束
                    Context.ChangeScreen(ScreenEnum.SCREEN_MENU);
                }
            }

            _combatManage.ExitCurrentCombat(_isWin);

            IsCombating = false;

            _actionQueue.Clear();
            _actionExecutor.Reset();
            _combatUI.Reset();
            _isAutoAttack = false;

            // 恢复一定的血量
            foreach (var item in _playerList)
            {
                if (item.HP <= 0)
                {
                    item.HP = 1;
                }
                if (item.MP <= 0)
                {
                    item.MP = 1;
                }
                item.HP = item.HP + (item.MaxHP - item.HP) / 10;
                item.MP = item.MP + item.MaxMP / 5;
                if (item.MP > item.MaxMP)
                {
                    item.MP = item.MaxMP;
                }
            }
        }

        /// <summary>
        /// 自动攻击队列
        /// </summary>
        private void GenerateAutoActionQueue()
        {
            Monster monster = GetFirstAliveMonster();
            //TODO 测试是否改变
            List<FightingCharacter> listMonster = new List<FightingCharacter>();
            listMonster.AddRange(_monsterList);

            _actionQueue.Clear();

            // 玩家的Action
            foreach (var item in _playerList)
            {
                if (item.IsAlive)
                {
                    if (item.HasAtbuff(CombatBuff.BUFF_MASK_ALL))
                    {
                        _actionQueue.Add(new ActionPhysicalAttackAll(Context, item, listMonster));
                    }
                    else
                    {
                        _actionQueue.Add(new ActionPhysicalAttackOne(Context, item, monster));
                    }
                }
            }

            // 怪物的Action
            GenerateMonstersActions();

            SortActionQueue();
        }

        /// <summary>
        /// 怪物攻击队列
        /// </summary>
        private void GenerateMonstersActions()
        {
            //TODO 测试是否改变
            List<FightingCharacter> listPlay = new List<FightingCharacter>();
            listPlay.AddRange(_playerList);

            // TODO according to the monster's intelligence, add some magic attack
            foreach (var item in _monsterList)
            {
                if (item.IsAlive)
                {
                    PlayerCharacter p = GetRandomAlivePlayer();
                    if (p != null)
                    {
                        if (item.HasAtbuff(CombatBuff.BUFF_MASK_ALL))
                        {
                            _actionQueue.Add(new ActionPhysicalAttackAll(Context, item, listPlay));
                        }
                        else
                        {
                            _actionQueue.Add(new ActionPhysicalAttackOne(Context, item, p));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取第一个玩家的索引
        /// </summary>
        /// <returns></returns>
        private int GetFirstAlivePlayerIndex()
        {
            for (int i = 0; i < _playerList.Count; i++)
            {
                if (_playerList[i].IsAlive)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取下一个存活的主角序号
        /// </summary>
        /// <returns></returns>
        private int GetNextAlivePlayerIndex()
        {
            for (int i = _curSelActionPlayerIndex + 1; i < _playerList.Count; i++)
            {
                if (_playerList[i].IsAlive)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取下一个活着的角色的索引
        /// </summary>
        /// <returns></returns>
        private int GetPreAlivePlayerIndex()
        {
            for (int i = _curSelActionPlayerIndex - 1; i >= 0; i--)
            {
                if (_playerList[i].IsAlive)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 怪物是否都挂了
        /// </summary>
        /// <returns></returns>
        private bool IsAllMonsterDead()
        {
            return GetFirstAliveMonster() == null;
        }

        /// <summary>
        /// 是否有玩家角色存活
        /// </summary>
        /// <returns></returns>
        private bool IsAnyPlayerAlive()
        {
            foreach (var item in _playerList)
            {
                if (item.HP > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// index 之后的主角是否都挂
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool IsPlayerBehindDead(int index)
        {
            for (int i = index + 1; i < _playerList.Count; i++)
            {
                if (_playerList[i].IsAlive)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 设置怪物偏移
        /// </summary>
        private void SetOriginalMonsterPos()
        {
            for (int i = 0; i < _monsterList.Count; i++)
            {
                _monsterList[i].SetOriginalCombatPos(i);
            }
        }

        /// <summary>
        /// 设置角色偏移
        /// </summary>
        private void SetOriginalPlayerPos()
        {
            for (int i = 0; i < _playerList.Count; i++)
            {
                _playerList[i].SetCombatPos(PlayerPos[i].X, PlayerPos[i].Y);
            }
        }

        /// <summary>
        /// 按敏捷从大到小排列动作队列
        /// </summary>
        private void SortActionQueue()
        {
            _actionQueue.Sort((a, b) => { return a.GetPriority() - b.GetPriority(); });
        }

        /// <summary>
        /// 更新双方状态
        /// </summary>
        private void UpdateFighterState()
        {
            // TODO decrease the buff's round count
        }

        #endregion 方法
    }
}