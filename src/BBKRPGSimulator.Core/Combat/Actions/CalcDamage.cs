namespace BBKRPGSimulator.Combat.Actions
{
    internal class CalcDamage
    {
        #region 方法

        public static int CalcBaseDamage(int attack, int defense)
        {
            int damage;

            if (attack > defense)
            {
                damage = (int)(attack * 2 - defense * 1.6 + 0.5);
            }
            else if (attack > defense * 0.6)
            {
                damage = (int)(attack - defense * 0.6 + 0.5);
            }
            else
            {
                damage = 0;
            }

            return damage;
        }

        #endregion 方法

        //	public static int calcMagicDamage(int )
    }
}