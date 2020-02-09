using System;
using System.Diagnostics;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;
using BBKRPGSimulator.Lib;

namespace BBKRPGSimulator.View.Combat
{
    /// <summary>
    /// 战斗胜利消息界面
    /// (显示战斗胜利后玩家获得的东西)
    /// </summary>
    internal class MsgScreen : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 显示位置left
        /// </summary>
        private readonly int _left;

        /// <summary>
        /// 显示位置top
        /// </summary>
        private readonly int _top;

        /// <summary>
        /// 消息内容图片
        /// </summary>
        private ImageBuilder _messageImg;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 战斗胜利消息界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        public MsgScreen(SimulatorContext context, string msg) : this(context, (96 - 24) / 2, msg)
        {
        }

        /// <summary>
        /// 战斗胜利消息界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="top">消息显示Y坐标</param>
        /// <param name="msg"></param>
        public MsgScreen(SimulatorContext context, int top, string msg) : base(context)
        {
            byte[] msgData;
            try
            {
                msgData = msg.GetBytes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                msgData = new byte[0];
            }

            ResImage side = Context.LibData.GetImage(2, 8);
            _messageImg = Context.GraphicsFactory.NewImageBuilder(msgData.Length * 8 + 8, 24);
            ICanvas canvas = Context.GraphicsFactory.NewCanvas(_messageImg); ;
            canvas.DrawColor(Constants.COLOR_WHITE);
            side.Draw(canvas, 1, 0, 0);
            side.Draw(canvas, 2, _messageImg.Width - 3, 0);

            Paint paint = new Paint(PaintStyle.FILL_AND_STROKE, Constants.COLOR_BLACK);
            canvas.DrawLine(0, 1, _messageImg.Width, 1, paint);
            canvas.DrawLine(0, 22, _messageImg.Width, 22, paint);
            TextRender.DrawText(canvas, msgData, 4, 4);

            _left = (160 - _messageImg.Width) / 2;
            _top = top;
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawBitmap(_messageImg, _left, _top);
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