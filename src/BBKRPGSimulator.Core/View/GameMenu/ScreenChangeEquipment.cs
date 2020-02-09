using BBKRPGSimulator.Characters;
using BBKRPGSimulator.Goods;
using BBKRPGSimulator.Graphics;

namespace BBKRPGSimulator.View.GameMenu
{
    /// <summary>
    /// 装备更换界面
    /// </summary>
    internal class ScreenChangeEquipment : BaseScreen
    {
        #region 字段

        /// <summary>
        /// 当前角色
        /// </summary>
        private PlayerCharacter _character;

        /// <summary>
        /// 当前显示的页面
        /// </summary>
        private int _curPage = 0;

        /// <summary>
        /// 装备切换列表
        /// </summary>
        private GoodsEquipment[] _equipments;

        /// <summary>
        /// 选择的索引
        /// </summary>
        private int _selectedIndex;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 装备更换界面
        /// </summary>
        /// <param name="context"></param>
        /// <param name="character">角色</param>
        /// <param name="equipment">要装备的装备</param>
        public ScreenChangeEquipment(SimulatorContext context, PlayerCharacter character, GoodsEquipment equipment) : base(context)
        {
            _character = character;
            if (character.HasSpace(equipment.Type))
            {
                _equipments = new GoodsEquipment[1];
                _equipments[0] = equipment;
                _selectedIndex = 0;
            }
            else
            {
                _equipments = new GoodsEquipment[2];
                _equipments[0] = character.GetCurrentEquipment(equipment.Type);
                _equipments[1] = equipment;
                _selectedIndex = 1;
                // 没有空间，脱掉当前装备的
                character.TakeOff(equipment.Type);
            }
            character.PutOn(equipment);
        }

        #endregion 构造函数

        #region 方法

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawColor(Constants.COLOR_WHITE);
            if (_character != null)
            {
                _character.DrawState(canvas, _curPage);
                _character.DrawHead(canvas, 5, 60);
            }
            for (int i = 0; i < _equipments.Length; i++)
            {
                if (_equipments[i] != null)
                {
                    _equipments[i].Draw(canvas, 8, 2 + 32 * i);
                }
            }
            Context.Util.DrawTriangleCursor(canvas, 1, 10 + 32 * _selectedIndex);
        }

        public override void OnKeyDown(int key)
        {
            if (key == SimulatorKeys.KEY_UP && _selectedIndex > 0)
            {
                _character.TakeOff(_equipments[_selectedIndex].Type);
                --_selectedIndex;
                _character.PutOn(_equipments[_selectedIndex]);
            }
            else if (key == SimulatorKeys.KEY_DOWN && _selectedIndex < _equipments.Length - 1)
            {
                _character.TakeOff(_equipments[_selectedIndex].Type);
                ++_selectedIndex;
                _character.PutOn(_equipments[_selectedIndex]);
            }
            else if (key == SimulatorKeys.KEY_PAGEDOWN || key == SimulatorKeys.KEY_PAGEUP)
            {
                _curPage = 1 - _curPage;
            }
        }

        public override void OnKeyUp(int key)
        {
            if (key == SimulatorKeys.KEY_CANCEL)
            {
                // 换上原来的装备
                _character.TakeOff(_equipments[0].Type);
                if (_equipments.Length > 1)
                {
                    _character.PutOn(_equipments[0]);
                }
                Context.PopScreen();
            }
            else if (key == SimulatorKeys.KEY_ENTER)
            {
                if (_selectedIndex == _equipments.Length - 1)
                {
                    // 换了新装备
                    // 物品链中删除该装备
                    Context.GoodsManage.DropGoods(_equipments[_equipments.Length - 1].Type, _equipments[_equipments.Length - 1].Index);
                    // 物品链中加入老装备
                    if (_equipments.Length > 1)
                    {
                        Context.GoodsManage.AddGoods(_equipments[0].Type, _equipments[0].Index);
                    }
                }
                Context.PopScreen();
            }
        }

        public override void Update(long delta)
        {
        }

        #endregion 方法
    }
}