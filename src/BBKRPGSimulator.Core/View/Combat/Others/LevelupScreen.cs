using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.View.Combat
{
    /// <summary>
    /// 等级提升界面
    /// </summary>
    internal class LevelupScreen : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 信息图片
        /// </summary>
        private ImageBuilder _infoImg;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 等级提升界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="character"></param>
        public LevelupScreen(SimulatorContext context, PlayerCharacter character) : base(context)
        {
            _infoImg = Context.LibData.GetImage(2, 9)[0];

            ICanvas canvas = Context.GraphicsFactory.NewCanvas(_infoImg); ;
            ResLevelupChain levelupChain = character.LevelupChain;
            int curl = character.Level;
            Context.Util.DrawSmallNum(canvas, character.HP, 37, 9); character.HP = character.MaxHP;
            Context.Util.DrawSmallNum(canvas, character.MaxHP - (levelupChain.GetMaxHP(curl) - levelupChain.GetMaxHP(curl - 1)), 56, 9);
            Context.Util.DrawSmallNum(canvas, character.MaxHP, 86, 9);
            Context.Util.DrawSmallNum(canvas, character.MaxHP, 105, 9);
            Context.Util.DrawSmallNum(canvas, character.MP, 37, 21); character.MP = character.MaxMP;
            Context.Util.DrawSmallNum(canvas, character.MaxMP - (levelupChain.GetMaxMP(curl) - levelupChain.GetMaxMP(curl - 1)), 56, 21);
            Context.Util.DrawSmallNum(canvas, character.MaxMP, 86, 21);
            Context.Util.DrawSmallNum(canvas, character.MaxMP, 105, 21);
            Context.Util.DrawSmallNum(canvas, character.Attack - (levelupChain.GetAttack(curl) - levelupChain.GetAttack(curl - 1)), 47, 33);
            Context.Util.DrawSmallNum(canvas, character.Attack, 96, 33);
            Context.Util.DrawSmallNum(canvas, character.Defend - (levelupChain.GetDefend(curl) - levelupChain.GetDefend(curl - 1)), 47, 45);
            Context.Util.DrawSmallNum(canvas, character.Defend, 96, 45);
            Context.Util.DrawSmallNum(canvas, character.Speed - (levelupChain.GetSpeed(curl) - levelupChain.GetSpeed(curl - 1)), 47, 57);
            Context.Util.DrawSmallNum(canvas, character.Speed, 96, 57);
            Context.Util.DrawSmallNum(canvas, character.Lingli - (levelupChain.GetLingli(curl) - levelupChain.GetLingli(curl - 1)), 47, 69);
            Context.Util.DrawSmallNum(canvas, character.Lingli, 96, 69);
            Context.Util.DrawSmallNum(canvas, character.Luck - (levelupChain.GetLuck(curl) - levelupChain.GetLuck(curl - 1)), 47, 81);
            Context.Util.DrawSmallNum(canvas, character.Luck, 96, 81);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_infoImg, (160 - _infoImg.Width) / 2, (96 - _infoImg.Height) / 2);
        }

        public override void OnKeyDown(int key)
        {
        }

        public override void OnKeyUp(int key)
        {
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}