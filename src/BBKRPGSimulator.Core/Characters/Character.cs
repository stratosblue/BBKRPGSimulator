using System.Drawing;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.Characters
{
    /// <summary>
    /// 人物
    /// </summary>
    internal abstract class Character : ResBase
    {
        #region 字段

        /// <summary>
        /// 角色在地图中的面向
        /// </summary>
        private Direction _direction = Direction.South;

        /// <summary>
        /// 角色在地图中的位置
        /// </summary>
        private Point _posInMap = new Point();

        /// <summary>
        /// 人物行走图
        /// </summary>
        private WalkingSprite _walkingSprite;

        #endregion 字段

        #region 属性

        /// <summary>
        /// 角色在地图中的面向
        /// </summary>
        public Direction Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                _walkingSprite.SetDirection(value);
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 角色在地图中的位置
        /// </summary>
        public Point PosInMap { get => _posInMap; set => _posInMap = value; }

        /// <summary>
        /// 人物动作状态
        /// </summary>
        public CharacterActionState State { get; set; } = CharacterActionState.STATE_STOP;

        /// <summary>
        /// 行走显示状态
        /// </summary>
        public WalkingSprite WalkingSprite
        {
            get
            {
                return _walkingSprite;
            }
            set
            {
                _walkingSprite = value;
                _walkingSprite.RefreshData();
            }
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 人物
        /// </summary>
        /// <param name="context"></param>
        public Character(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        /// <summary>
        /// 绘制行走精灵？
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="posMapScreen"></param>
        public void DrawWalkingSprite(ICanvas canvas, Point posMapScreen)
        {
            Point p = GetPosOnScreen(posMapScreen);
            _walkingSprite.Draw(canvas, p.X * 16, p.Y * 16);
            //		if (p.X >= 0 && p.X <= 9 && p.Y >= 0 && p.Y <= 5) {
            //			mWalkingSprite.draw(canvas, p.X * 16, p.Y * 16);
            //		}
        }

        /// <summary>
        /// 获取脚步
        /// </summary>
        /// <returns></returns>
        public int GetStep()
        {
            return _walkingSprite.Step;
        }

        /// <summary>
        /// 获取行走精灵ID
        /// </summary>
        /// <returns></returns>
        public int GetWalkingSpriteId()
        {
            return _walkingSprite.Id;
        }

        #region 位置相关

        /// <summary>
        /// 获取在屏幕中的位置
        /// </summary>
        /// <param name="posMapScreen"></param>
        /// <returns></returns>
        public Point GetPosOnScreen(Point posMapScreen)
        {
            var result = new Point(PosInMap.X - posMapScreen.X, PosInMap.Y - posMapScreen.Y);
            return result;
        }

        /// <summary>
        /// 设置角色在地图上的位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPosInMap(int x, int y)
        {
            _posInMap.X = x;
            _posInMap.Y = y;
        }

        /// <summary>
        /// 设置在屏幕中的位置
        /// </summary>
        /// <param name="point"></param>
        /// <param name="posMapScreen"></param>
        public void SetPosOnScreen(Point point, Point posMapScreen)
        {
            _posInMap.X = point.X + posMapScreen.X;
            _posInMap.Y = point.Y + posMapScreen.Y;
        }

        /// <summary>
        /// 设置在屏幕中的位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="posMapScreen"></param>
        public void SetPosOnScreen(int x, int y, Point posMapScreen)
        {
            _posInMap.X = x + posMapScreen.X;
            _posInMap.Y = y + posMapScreen.Y;
        }

        /// <summary>
        /// 更新在地图中的位置
        /// </summary>
        /// <param name="direction"></param>
        private void UpdatePosInMap(Direction direction)
        {
            switch (direction)
            {
                case Direction.East: _posInMap.X++; break;
                case Direction.West: _posInMap.X--; break;
                case Direction.North: _posInMap.Y--; break;
                case Direction.South: _posInMap.Y++; break;
            }
        }

        #endregion 位置相关

        /// <summary>
        /// 设置脚步
        /// </summary>
        /// <param name="step">0—迈左脚；1—立正；2—迈右脚</param>
        public void SetStep(int step)
        {
            _walkingSprite.Step = step;
        }

        /// <summary>
        /// 设置行走精灵
        /// </summary>
        /// <param name="sprite"></param>
        public void SetWalkingSprite(WalkingSprite sprite)
        {
            _walkingSprite = sprite;
            _walkingSprite.SetDirection(Direction);
        }

        /// <summary>
        /// 行走
        /// </summary>
        public virtual void Walk()
        {
            _walkingSprite.Walk();
            UpdatePosInMap(Direction);
        }

        /// <summary>
        /// 往指定方向行走
        /// </summary>
        /// <param name="direction"></param>
        public virtual void Walk(Direction direction)
        {
            if (direction == Direction)
            {
                _walkingSprite.Walk();
            }
            else
            {
                _walkingSprite.Walk(direction);
                Direction = direction;
            }
            UpdatePosInMap(direction);
        }

        /// <summary>
        /// 原地踏步
        /// </summary>
        /// <param name="direction"></param>
        public virtual void WalkStay(Direction direction)
        {
            if (direction == Direction)
            {
                _walkingSprite.Walk();
            }
            else
            {
                _walkingSprite.Walk(direction);
                Direction = direction;
            }
        }

        /// <summary>
        /// 原地踏步，面向不变
        /// </summary>
        public void WalkStay()
        {
            _walkingSprite.Walk();
        }

        #endregion 方法
    }
}