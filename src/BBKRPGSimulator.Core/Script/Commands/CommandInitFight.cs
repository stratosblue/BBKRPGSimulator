using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 初始化战斗命令
    /// </summary>
    internal class CommandInitFight : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 初始化战斗命令
        /// </summary>
        /// <param name="context"></param>
        public CommandInitFight(ArraySegment<byte> data, SimulatorContext context) : base(data, 22, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            int[] _monsterType = new int[8];
            for (int i = 0; i < 8; i++)
            {
                _monsterType[i] = Data.Get2BytesUInt(i * 2);
            }

            var scrb = Data.Get2BytesUInt(16);
            var scrl = Data.Get2BytesUInt(18);
            var scrr = Data.Get2BytesUInt(20);
            Context.CombatManage.InitRandomCombat(_monsterType, scrb, scrl, scrr);
            return null;
        }

        #endregion 方法
    }
}