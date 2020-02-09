using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 属性测试命令
    /// </summary>
    internal class CommandAttributeTest : BaseCommand
    {
        #region 字段

        private readonly int _actorid, _type, _value, _addr1, _addr2;

        #endregion 字段

        #region 构造函数

        /// <summary>
        /// 属性测试命令
        /// </summary>
        /// <param name="context"></param>
        public CommandAttributeTest(ArraySegment<byte> data, SimulatorContext context) : base(data, 10, context)
        {
            _actorid = data.Get2BytesUInt(0);
            _type = data.Get2BytesUInt(2);
            _value = data.Get2BytesUInt(4);
            _addr1 = data.Get2BytesUInt(6);
            _addr2 = data.Get2BytesUInt(8);
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var player = Context.PlayContext.GetPlayer(_actorid);
            // 0-级别，1-攻击力，2-防御力，3-身法，4-生命，5-真气当前值，6-当前经验值
            // 7-灵力，8-幸运，9-攻击的异常回合数，10-对特殊状态的免疫，11-普通攻击可能产生异常状态
            // 12-合体法术，13-每回合变化生命，14-每回合变化真气，15-头戴，16-身穿
            // 17-肩披，18-护腕，19-手持，20-脚蹬，21-佩戴1，22-佩戴2，23-生命上限，24-真气上限

            var currentValue = _type switch
            {
                0 => player.Level,
                1 => player.Attack,
                2 => player.Defend,
                3 => player.Speed,
                4 => player.HP,
                5 => player.MP,
                6 => player.CurrentExp,
                7 => player.Lingli,
                8 => player.Luck,
                // * 0装饰 1装饰 2护腕 3脚蹬 4手持 5身穿 6肩披 7头戴
                15 => player.Equipments[7]?.Index ?? 0,
                16 => player.Equipments[5]?.Index ?? 0,
                17 => player.Equipments[6]?.Index ?? 0,
                18 => player.Equipments[2]?.Index ?? 0,
                19 => player.Equipments[4]?.Index ?? 0,
                20 => player.Equipments[3]?.Index ?? 0,
                21 => player.Equipments[0]?.Index ?? 0,
                22 => player.Equipments[1]?.Index ?? 0,
                23 => player.MaxHP,
                24 => player.MaxMP,
                _ => throw new NotImplementedException("ATTRIBTEST $type")
            };

            if (currentValue < _value)
            {
                Context.ScriptProcess.GotoAddress(_addr1);
            }
            else if (currentValue > _value)
            {
                Context.ScriptProcess.GotoAddress(_addr2);
            }

            return null;
        }

        #endregion 方法
    }
}