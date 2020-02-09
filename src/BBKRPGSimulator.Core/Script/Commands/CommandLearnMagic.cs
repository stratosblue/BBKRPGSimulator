using System;

using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Magic;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 学习魔法命令
    /// </summary>
    internal class CommandLearnMagic : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 学习魔法命令
        /// </summary>
        /// <param name="context"></param>
        public CommandLearnMagic(ArraySegment<byte> data, SimulatorContext context) : base(data, 6, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate() => new CommandLearnMagicOperate(Data, Context);

        #endregion 方法

        #region 类

        public class CommandLearnMagicOperate : Operate
        {
            #region 字段

            private readonly PlayerCharacter _playerCharacter;

            private readonly BaseMagic _magic;

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyDown;

            /// <summary>
            /// 显示时间
            /// </summary>
            private long _showTime;

            #endregion 字段

            #region 属性

            private TextRender TextRender => Context.TextRender;

            #endregion 属性

            #region 构造函数

            public CommandLearnMagicOperate(ArraySegment<byte> data, SimulatorContext context) : base(context)
            {
                _isAnyKeyDown = false;
                _showTime = 0;

                var actorId = data.Get2BytesUInt(0);
                var type = data.Get2BytesUInt(2);
                var index = data.Get2BytesUInt(4);

                _magic = Context.LibData.GetMagic(type, index) ?? throw new ArgumentException();
                _playerCharacter = Context.PlayContext.GetPlayer(actorId);
                _playerCharacter.MagicChain.LearnMagic(type, index);
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                //TODO 修正显示
                TextRender.DrawText(canvas, $"{_playerCharacter.Name} 学会了魔法:{_magic.Name}", 0, 0);
            }

            public override void OnKeyUp(int key)
            {
                _isAnyKeyDown = true;
            }

            public override bool Update(long delta)
            {
                _showTime += delta;
                return _showTime < 1000 && !_isAnyKeyDown;
            }

            #endregion 方法
        }

        #endregion 类
    }
}