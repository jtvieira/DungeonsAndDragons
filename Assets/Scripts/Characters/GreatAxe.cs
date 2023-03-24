namespace Characters
{
    public class GreatAxe : Spell
    {
        public GreatAxe()
        {
            base.name = "GreatAxe";
            base.level = "M";
            base.diceType = 12; //(1d12)
            base.range = 1;
        }

        //This function ATTACKS the enemy only if the D20+3 roll > enemy AC level 
        //And then checks to see if the enemy is within the attack RANGE
        public static void fireSpell()
        {
            float rollResult1 = rollDice(12);
            float totalRollResult = rollResult1;

        }
    }

}