﻿namespace Characters
{
    public class RayOfFrost : Spell
    {
        public RayOfFrost()
        {
            base.name = "RayOfFrost";
            base.level = "C";
            base.diceType = 8;
            base.range = 12;
        }

        //This function ATTACKS the enemy only if the D20+3 roll > enemy AC level 
        //And then checks to see if the enemy is within the attack RANGE
        public static void fireSpell()
        {
            //Roll for damage, 2d8
            float rollResult1 = rollDice(8);
            float rollResult2 = rollDice(8);
            float totalRollResult = rollResult1 + rollResult2;

            //add code to attack the enemys HP stat
            //Skeleton.HP -= totalRollResult;
            //something like that^
        }

        public override string ToString()
        {
            return string.Format("RayOfFrost: \n name: {0} \n level: {1} \n diceType: {2} \n range: {3} \n", name, level, diceType, range);
        }
    }
}