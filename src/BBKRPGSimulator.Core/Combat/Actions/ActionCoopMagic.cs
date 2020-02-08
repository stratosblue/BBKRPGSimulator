using System.Collections.Generic;
using System.Linq;

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
    internal class ActionCoopMagic : ActionMultiTarget
    {
        #region 字段

        /// <summary>
        /// 移位帧数
        /// </summary>
        public const int MOV_FRAME = 5;

        private readonly MagicAttack _magic;

        private readonly bool _onlyOneMonster;

        private float[,] dxy;
        private int mAniX, mAniY;
        private int[,] oxy;

        private IReadOnlyList<PlayerCharacter> Attackers { get; set; }

        private FightingCharacter FirstTarget => Targets[0];

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 合击动作
        /// </summary>
        /// <param name="actors"></param>
        /// <param name="monster"></param>
        public ActionCoopMagic(SimulatorContext context,
                               List<PlayerCharacter> actors,
                               FightingCharacter monster)
            : base(context,
                   actors.First(),
                   new List<FightingCharacter>() { monster })
        {
            State = CombatAnimationState.Move;

            Attackers = actors;
            _onlyOneMonster = true;

            _magic = GetCoopMagic();
        }

        public ActionCoopMagic(SimulatorContext context,
                               List<PlayerCharacter> actors,
                               List<FightingCharacter> monsters)
            : base(context,
                   actors.First(),
                   monsters)
        {
            Attackers = actors;
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
            else if (State == CombatAnimationState.End)
            {
                DrawRaiseAnimation(canvas);
            }
        }

        public override bool IsAttackerAlive() => Executor.IsAlive;

        public override bool IsTargetAlive() => Targets.Any(m => m.IsAlive);

        public override bool IsTargetsMoreThanOne() => _onlyOneMonster;

        public override void PreProccess()
        {
            // TODO 记下伤害值、异常状态
            int[,] midpos = new int[,] { { 92, 52 }, { 109, 63 }, { 126, 74 } };
            dxy = new float[Attackers.Count, 2];
            oxy = new int[Attackers.Count, 2];
            for (int i = 0; i < Attackers.Count; i++)
            {
                oxy[i, 0] = Attackers[i].GetCombatX();
                oxy[i, 1] = Attackers[i].GetCombatY();
            }
            for (int i = 0; i < Attackers.Count; i++)
            {
                dxy[i, 0] = midpos[i, 0] - oxy[i, 0];
                dxy[i, 0] /= MOV_FRAME;
                dxy[i, 1] = midpos[i, 1] - oxy[i, 1];
                dxy[i, 1] /= MOV_FRAME;
            }

            if (_onlyOneMonster)
            {
                mAniX = FirstTarget.GetCombatX();
                mAniY = FirstTarget.GetCombatY() - FirstTarget.FightingSprite.Height / 2;
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

                Targets.ForEach(m => UseMagic(m));
            }
            Animation.StartAni();
        }

        public override bool TargetIsMonster() => true;

        public override string ToString()
        {
            return $"【{Executor.Name}】释放的合击魔法";
        }

        public override bool Update(long delta)
        {
            base.Update(delta);
            switch (State)
            {
                case CombatAnimationState.Move:
                    if (_currentFrame < MOV_FRAME)
                    {
                        for (int i = 0; i < Attackers.Count; i++)
                        {
                            Attackers[i].SetCombatPos((int)(oxy[i, 0] + dxy[i, 0] * _currentFrame),
                                    (int)(oxy[i, 1] + dxy[i, 1] * _currentFrame));
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
                        for (int i = 0; i < Attackers.Count; i++)
                        {
                            Attackers[i].FightingSprite.CurrentFrame = (_currentFrame - MOV_FRAME) * 3 / 10 + 6;
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
                        for (int i = 0; i < Attackers.Count; i++)
                        {
                            Attackers[i].SetFrameByState();
                            Attackers[i].SetCombatPos(oxy[i, 0], oxy[i, 1]);
                        }
                    }
                    break;

                case CombatAnimationState.End:
                    return UpdateRaiseAnimation(delta);
            }
            return true;
        }

        /// <summary>
        /// 获取合击魔法
        /// </summary>
        /// <returns></returns>
        private MagicAttack GetCoopMagic()
        {
            GoodsDecorations dc = (GoodsDecorations)(Executor as PlayerCharacter).Equipments[0];
            return dc?.GetCoopMagic();
        }

        private void UseMagic(FightingCharacter target)
        {
            //TODO 处理异常状态
            var debuffs = target.DeBuffRound.Clone() as int[];
            var oldhp = target.HP;
            _magic.Use(Attackers[0], target);
            var damage = oldhp - target.HP;

            RaiseAnimations.Add(new RaiseAnimation(Context, target.GetCombatX(), target.GetCombatY(), -damage, 0));
        }

        #endregion 方法
    }
}