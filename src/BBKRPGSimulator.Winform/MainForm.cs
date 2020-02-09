using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Winform
{
    public partial class MainForm : Form
    {
        #region 字段

        private readonly RPGSimulator _simulator = new RPGSimulator();
        private System.Drawing.Graphics _graphics = null;
        private int _renderWidth, _renderHeight;

        #endregion 字段

        #region 构造函数

        public MainForm()
        {
            InitializeComponent();

            _renderWidth = renderPanel.Width;
            _renderHeight = renderPanel.Height;

            renderPanel.SizeChanged += (s, e) =>
            {
                _renderWidth = renderPanel.Width;
                _renderHeight = renderPanel.Height;
                GetNewGraphics();
            };

            //体验不好。。
            //_simulator.ExitRequested += (s, e) =>
            //{
            //    if (MessageBox.Show(this, "确定退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //    {
            //        Application.Exit();
            //    }
            //};
        }

        #endregion 构造函数

        #region 方法

        [DebuggerStepThrough]
        private void GameViewRenderFrame(ImageBuilder frameData)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    if (frameData is NewImageBuilder newImageBuilder)
                    {
                        _graphics.DrawImage(newImageBuilder.Instance, 0, 0);
                    }
                    else
                    {
                        using (var image = PlatformExtensionFunction.GetImageFromBuffer(frameData))
                        {
                            _graphics.DrawImage(image, new Rectangle(0, 0, _renderWidth, _renderHeight), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                        }
                    }
                }));
            }
            catch (ObjectDisposedException)
            {
            }
        }

        private void GetNewGraphics()
        {
            _graphics = renderPanel.CreateGraphics();
            _graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            _graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _simulator.Stop();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            _simulator.KeyPressed(e.KeyCode.GetHashCode());
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            _simulator.KeyReleased(e.KeyCode.GetHashCode());
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            _simulator.RenderFrame += GameViewRenderFrame;

            var libPath = "./assets/fmj.lib";

            Text = $"{nameof(RPGSimulator)} - {Utilities.GetGameName(libPath)}";

            var options = new SimulatorOptions()
            {
                LibPath = libPath,
                KeyMap = new Dictionary<int, int>()
                {
                    { (int)Keys.Enter, SimulatorKeys.KEY_ENTER },
                    { (int)Keys.Space, SimulatorKeys.KEY_ENTER },
                    { (int)Keys.Escape, SimulatorKeys.KEY_CANCEL },
                    { (int)Keys.Q, SimulatorKeys.KEY_PAGEUP },
                    { (int)Keys.PageUp, SimulatorKeys.KEY_PAGEUP },
                    { (int)Keys.W, SimulatorKeys.KEY_PAGEDOWN },
                    { (int)Keys.PageDown, SimulatorKeys.KEY_PAGEDOWN },
                },
            };

            _simulator.Start(options);
            GetNewGraphics();
        }

        #endregion 方法
    }
}