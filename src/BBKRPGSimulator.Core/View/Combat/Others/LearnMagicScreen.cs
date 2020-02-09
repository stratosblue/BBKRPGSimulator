using System;
using System.Diagnostics;

using BBKRPGSimulator.Graphics;
using BBKRPGSimulator.Graphics.Util;

namespace BBKRPGSimulator.View.Combat
{
    /// <summary>
    /// 习得魔法界面
    /// </summary>
    internal class LearnMagicScreen : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 信息图片
        /// </summary>
        private ImageBuilder _infoImg;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 习得魔法界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="characterName">角色名称</param>
        /// <param name="magicName">魔法名称</param>
        public LearnMagicScreen(SimulatorContext context, string characterName, string magicName) : base(context)
        {
            _infoImg = Context.LibData.GetImage(2, 10)[0];
            byte[] nameData;
            byte[] magicNameData;
            try
            {
                nameData = characterName.GetBytes();
                magicNameData = magicName.GetBytes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                nameData = new byte[0];
                magicNameData = new byte[0];
            }

            ICanvas canvas = Context.GraphicsFactory.NewCanvas(_infoImg); ;
            TextRender.DrawText(canvas, nameData, (_infoImg.Width - nameData.Length * 8) / 2, 8);
            TextRender.DrawText(canvas, magicNameData, (_infoImg.Width - magicNameData.Length * 8) / 2, 42);
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