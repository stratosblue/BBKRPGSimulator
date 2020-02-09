using System;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.View;
using BBKRPGSimulator.View.Combat;

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

            private readonly BaseScreen _learnShowScreen;

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

                var magic = Context.LibData.GetMagic(type, index) ?? throw new ArgumentException();
                var playerCharacter = Context.PlayContext.GetPlayer(actorId);
                playerCharacter.MagicChain.LearnMagic(type, index);

                _learnShowScreen = new LearnMagicScreen(Context, playerCharacter.Name, magic.Name);
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas) => _learnShowScreen.Draw(canvas);

            public override void OnKeyUp(int key) => _isAnyKeyDown = true;

            public override bool Update(long delta)
            {
                _showTime += delta;
                return _showTime < 1500 && !_isAnyKeyDown;
            }

            #endregion 方法
        }

        #endregion 类
    }
}