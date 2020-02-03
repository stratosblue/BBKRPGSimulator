using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 获取物品命令
    /// </summary>
    internal class CommandGainGoods : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 获取物品命令
        /// </summary>
        /// <param name="context"></param>
        public CommandGainGoods(SimulatorContext context) : base(context)
        {
        }

        #endregion 构造函数

        #region 方法

        public override int GetNextPos(byte[] code, int start)
        {
            return start + 4;
        }

        public override Operate GetOperate(byte[] code, int start)
        {
            return new CommandGainGoodsOperate(Context, code, start);
        }

        #endregion 方法

        #region 类

        /// <summary>
        /// 获取物品命令的操作
        /// </summary>
        private class CommandGainGoodsOperate : Operate
        {
            #region 字段

            private readonly byte[] _code;

            /// <summary>
            /// 消息
            /// </summary>
            private readonly string _message;

            private readonly int _start;

            /// <summary>
            /// 要获取的物品
            /// </summary>
            private BaseGoods _goods;

            /// <summary>
            /// 是否有键按下
            /// </summary>
            private bool _isAnyKeyPressed;

            /// <summary>
            /// 展示时间
            /// </summary>
            private long _showTime;

            #endregion 字段

            #region 构造函数

            /// <summary>
            /// 获取物品命令的操作
            /// </summary>
            /// <param name="context"></param>
            /// <param name="code"></param>
            /// <param name="start"></param>
            public CommandGainGoodsOperate(SimulatorContext context, byte[] code, int start) : base(context)
            {
                _code = code;
                _start = start;

                _goods = Context.LibData.GetGoods(code.Get2BytesUInt(start), code.Get2BytesUInt(start + 2));
                _message = $"获得:{_goods.Name}";
            }

            #endregion 构造函数

            #region 方法

            public override void Draw(ICanvas canvas)
            {
                Context.Util.ShowMessage(canvas, _message);
            }

            public override void OnKeyDown(int key)
            {
            }

            public override void OnKeyUp(int key)
            {
                _isAnyKeyPressed = true;
            }

            public override bool Process()
            {
                _goods.GoodsNum = 1;
                Context.GoodsManage.AddGoods(_goods.Type, _goods.Index);
                _showTime = 0;
                _isAnyKeyPressed = false;
                return true;
            }

            public override bool Update(long delta)
            {
                _showTime += delta;
                if (_showTime > 1000 || _isAnyKeyPressed)
                {
                    return false;
                }

                return true;
            }

            #endregion 方法
        }

        #endregion 类
    }
}