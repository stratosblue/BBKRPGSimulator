using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// 战斗精灵
    /// </summary>
    internal class FightingSprite : ContextDependent
    {
        #region 字段

        /// <summary>
        /// 图像资源
        /// </summary>
        private ResImage _image;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 在战斗场景中的屏幕X坐标
        /// </summary>
        public int CombatX { get; private set; }

        /// <summary>
        /// 在战斗场景中的屏幕Y坐标
        /// </summary>
        public int CombatY { get; private set; }

        /// <summary>
        /// 当前帧
        /// </summary>
        public int CurrentFrame { get; set; } = 1;

        /// <summary>
        /// 总帧数
        /// </summary>
        public int FrameCnt => _image.Number;

        /// <summary>
        /// 高度
        /// </summary>
        public int Height => _image.Height;

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width => _image.Width;

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 战斗精灵
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isMonster">是否为怪物</param>
        /// <param name="index"></param>
        public FightingSprite(SimulatorContext context, bool isMonster, int index) : base(context)
        {
            if (isMonster)  //怪物
            {
                _image = Context.LibData.GetCharacterImage(3, index);
            }
            else    //玩家角色
            {
                _image = Context.LibData.GetImage(3, index);
            }
        }

        #endregion 构造函数

        #region 方法

        public void Draw(ICanvas canvas)
        {
            _image.Draw(canvas, CurrentFrame, CombatX - _image.Width / 2,
                    CombatY - _image.Height / 2);
        }

        public void Draw(ICanvas canvas, int x, int y)
        {
            _image.Draw(canvas, CurrentFrame, x, y);
        }

        public void Move(int dx, int dy)
        {
            CombatX += dx;
            CombatY += dy;
        }

        public void SetCombatPos(int x, int y)
        {
            CombatX = x;
            CombatY = y;
        }

        #endregion 方法
    }
}