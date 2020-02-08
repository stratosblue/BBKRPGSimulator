using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 属性加命令
    /// </summary>
    internal class CommandAttributeAdd : BaseCommand
    {
        #region 字段

        private readonly int _actorid, _type, _value;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 属性加命令
        /// </summary>
        /// <param name="context"></param>
        public CommandAttributeAdd(ArraySegment<byte> data, SimulatorContext context) : base(data, 6, context)
        {
            _actorid = data.Get2BytesUInt(0);
            _type = data.Get2BytesUInt(2);
            _value = data.Get2BytesUInt(4);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var player = Context.PlayContext.GetPlayer(_actorid);
            // 0-级别，1-攻击力，2-防御力，3-身法，4-生命，5-真气当前值，6-当前经验值
            // 7-灵力，8-幸运，9-攻击的异常回合数，10-对特殊状态的免疫，11-普通攻击可能产生异常状态
            // 12-合体法术，13-每回合变化生命，14-每回合变化真气，15-生命上限，16-真气上限
            switch (_type)
            {
                case 0: player.Level += _value; break;
                case 1: player.Attack += _value; break;
                case 2: player.Defend += _value; break;
                case 3: player.Speed += _value; break;
                case 4: player.HP += _value; break;
                case 5: player.MP += _value; break;
                case 6: player.GainExperience(_value); break;
                case 7: player.Lingli += _value; break;
                case 8: player.Luck += _value; break;
                case 15: player.MaxHP += _value; break;
                case 16: player.MaxMP += _value; break;
                default: throw new NotImplementedException("ATTRIBADD $type");
            };
            return null;
        }

        #endregion 方法
    }
}