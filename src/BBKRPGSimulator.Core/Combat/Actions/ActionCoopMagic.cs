using System.Collections.Generic;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Combat.Anim;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Combat.Actions
{
    /// <summary>
    /// 合击动作
    /// </summary>
    internal class ActionCoopMagic : ActionBase
    {
        #region 字段

        /// <summary>
        /// 移位帧数
        /// </summary>
        public const int MOV_FRAME = 5;

        private List<PlayerCharacter> _actors;

        private MagicAttack _magic;
        private FightingCharacter _monster;
        private List<FightingCharacter> _monsters;
        private bool _onlyOneMonster;
        private RaiseAnimation _raiseAnimation;

        private List<RaiseAnimation> _raiseAnimations;

        private float[][] dxy;
        private int mAniX, mAniY;
        private int[][] oxy;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 合击动作
        /// </summary>
        /// <param name="actors"></param>
        /// <param name="monster"></param>
        public ActionCoopMagic(SimulatorContext context, List<PlayerCharacter> actors, FightingCharacter monster) : base(context)
        {
            State = CombatAnimationState.Move;

            _actors = actors;
            _monster = monster;
            _onlyOneMonster = true;

            _magic = GetCoopMagic();
        }

        public ActionCoopMagic(SimulatorContext context, List<PlayerCharacter> actors, List<FightingCharacter> monsters) : base(context)
        {
            _actors = actors;
            _monsters = new List<FightingCharacter>();
            _monsters.AddRange(monsters);
            _onlyOneMonster = false;

            _magic = GetCoopMagic();
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            if (State == CombatAnimationState.Magic)
            {
                Animation.DrawAbsolutely(canvas, mAniX, mAniY);
            }
        }

        public override int GetPriority()
        {
            return _actors[0].Speed;
        }

        public override bool IsAttackerAlive()
        {
            return _actors[0].IsAlive;
        }

        public override bool IsTargetAlive()
        {
            if (_onlyOneMonster)
            {
                return _monster.IsAlive;
            }
            else
            {
                foreach (var item in _monsters)
                {
                    if (item.IsAlive)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool IsTargetsMoreThanOne()
        {
            return _onlyOneMonster;
        }

        public override void PostExecute()
        {
        }

        public override void PreProccess()
        {
            // TODO 记下伤害值、异常状态
            int[][] midpos = new int[][] { new int[] { 92, 52 }, new int[] { 109, 63 }, new int[] { 126, 74 } };
            dxy = ExtensionFunction.DyadicArrayFloat(_actors.Count, 2);// new float[mActors.Count, 2];
            oxy = ExtensionFunction.DyadicArrayInt(_actors.Count, 2);// new int[mActors.Count, 2];
            for (int i = 0; i < _actors.Count; i++)
            {
                oxy[i][0] = _actors[i].GetCombatX();
                oxy[i][1] = _actors[i].GetCombatY();
            }
            for (int i = 0; i < dxy.Length; i++)
            {
                dxy[i][0] = midpos[i][0] - oxy[i][0];
                dxy[i][0] /= MOV_FRAME;
                dxy[i][1] = midpos[i][1] - oxy[i][1];
                dxy[i][1] /= MOV_FRAME;
            }

            if (_onlyOneMonster)
            {
                mAniX = _monster.GetCombatX();
                mAniY = _monster.GetCombatY() - _monster.FightingSprite.Height / 2;
            }
            else
            {
                mAniX = mAniY = 0;
            }

            if (_magic == null)
            {
                Animation = Context.LibData.GetSrs(2, 240);
            }
            else
            {
                Animation = _magic.Animation;
            }
            Animation.StartAni();
        }

        public override bool TargetIsMonster()
        {
            return true;
        }

        public override string ToString()
        {
            return $"【{_actors[0].Name}】释放的合击魔法";
        }

        public override bool Update(long delta)
        {
            base.Update(delta);
            switch (State)
            {
                case CombatAnimationState.Move:
                    if (_currentFrame < MOV_FRAME)
                    {
                        for (int i = 0; i < _actors.Count; i++)
                        {
                            _actors[i].SetCombatPos((int)(oxy[i][0] + dxy[i][0] * _currentFrame),
                                    (int)(oxy[i][1] + dxy[i][1] * _currentFrame));
                        }
                    }
                    else
                    {
                        State = CombatAnimationState.Start;
                    }
                    break;

                case CombatAnimationState.Start:
                    if (_currentFrame < 10 + MOV_FRAME)
                    {
                        for (int i = 0; i < _actors.Count; i++)
                        {
                            _actors[i].FightingSprite.CurrentFrame = (_currentFrame - MOV_FRAME) * 3 / 10 + 6;
                        }
                    }
                    else
                    {
                        State = CombatAnimationState.Magic;
                    }
                    break;

                case CombatAnimationState.Magic:
                    if (!Animation.Update(delta))
                    {
                        State = CombatAnimationState.End;
                        for (int i = 0; i < _actors.Count; i++)
                        {
                            _actors[i].SetFrameByState();
                            _actors[i].SetCombatPos(oxy[i][0], oxy[i][1]);
                        }
                    }
                    break;

                case CombatAnimationState.End:
                    if (_onlyOneMonster)
                    {
                        //				return m
                    }
                    //HACK if (true)?
                    //if (true)
                    //    return false;
                    return false;
            }
            return true;
        }

        protected override void DrawRaiseAnimation(ICanvas canvas)
        {
            if (_onlyOneMonster)
            {
                if (_raiseAnimation != null)
                {
                    _raiseAnimation.Draw(canvas);
                }
            }
            else
            {
                if (_raiseAnimations != null)
                {
                    foreach (var item in _raiseAnimations)
                    {
                        item.Draw(canvas);
                    }
                }
            }
        }

        protected override bool UpdateRaiseAnimation(long delta)
        {
            if (_onlyOneMonster)
            {
                return _raiseAnimation != null && _raiseAnimation.Update(delta);
            }

            if (_raiseAnimations != null)
            { // 全体
                if (_raiseAnimations.Count == 0)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < _raiseAnimations.Count; i++)
                    {
                        if (!_raiseAnimations[i].Update(delta))
                        {
                            _raiseAnimations.RemoveAt(i);
                            if (_raiseAnimations.Count <= 0)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取合击魔法
        /// </summary>
        /// <returns></returns>
        private MagicAttack GetCoopMagic()
        {
            PlayerCharacter firstPlayer = _actors[0];
            GoodsDecorations dc = (GoodsDecorations)firstPlayer.Equipments[0];
            return dc?.GetCoopMagic();
        }

        #endregion 方法
    }
}