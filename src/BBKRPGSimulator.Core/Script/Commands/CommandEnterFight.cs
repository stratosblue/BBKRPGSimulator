using System;

namespace BBKRPGSimulator.Script.Commands
{
    /// <summary>
    /// 进入战斗命令
    /// </summary>
    internal class CommandEnterFight : BaseCommand
    {
        #region 构造函数

        /// <summary>
        /// 进入战斗命令
        /// </summary>
        /// <param name="context"></param>
        public CommandEnterFight(ArraySegment<byte> data, SimulatorContext context) : base(data, 30, context)
        {
        }

        #endregion 构造函数

        #region 方法

        protected override Operate ProcessAndGetOperate()
        {
            var _start = Data.Offset;
            var _code = Data.Array;

            var monstersType = new int[] {
                _code.Get2BytesUInt(_start + 2),
                _code.Get2BytesUInt(_start + 4),
                _code.Get2BytesUInt(_start + 6)};
            var scr = new int[]{
                _code.Get2BytesUInt(_start + 8),
                _code.Get2BytesUInt(_start + 10),
                _code.Get2BytesUInt(_start + 12)};
            var evtRnds = new int[]{
                _code.Get2BytesUInt(_start + 14),
                _code.Get2BytesUInt(_start + 16),
                _code.Get2BytesUInt(_start + 18)};
            var evts = new int[]{
                _code.Get2BytesUInt(_start + 20),
                _code.Get2BytesUInt(_start + 22),
                _code.Get2BytesUInt(_start + 24)};
            var lossto = _code.Get2BytesUInt(_start + 26);
            var winto = _code.Get2BytesUInt(_start + 28);
            var maxround = _code.Get2BytesUInt(_start);

            Context.CombatManage.EnterStoryCombat(maxround, monstersType, scr, evtRnds, evts, lossto, winto);
            Context.ScriptProcess.ExitScript();

            return null;
        }

        #endregion 方法
    }
}