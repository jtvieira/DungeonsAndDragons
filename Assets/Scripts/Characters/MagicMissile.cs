namespace Characters
{
    public class MagicMissile : Spell
    {
        public MagicMissile()
        {
            base.name = "MagicMissile";
            base.level = "1";
            base.diceType = 4;  //(1d4+1)
            base.range = 24;
        }

        //This function ATTACKS the enemy only if the D20+3 roll > enemy AC level 
        //And then checks to see if the enemy is within the attack RANGE
        public static void fireSpell()
        {
            //Roll for damage, 1d4+1
            float totalRollResult = rollDice(4) + 1;

            //add code to attack the enemys HP stat
            //Skeleton.HP -= totalRollResult;
            //something like that^
        }

        public override string ToString()
        {
            return string.Format("MagicMissile: \n name: {0} \n level: {1} \n diceType: {2} \n range: {3} \n", name, level, diceType, range);
        }
    }
}