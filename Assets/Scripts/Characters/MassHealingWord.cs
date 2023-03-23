namespace Characters
{
public class MassHealingWord : Spell
{
    public MassHealingWord()
    {
        base.name = "MassHealingWord";
        base.level = "3";
        base.diceType = 4;  //(1d4)
        base.range = 12;
    }

    //This function ATTACKS the enemy only if the D20+3 roll > enemy AC level 
    //And then checks to see if the enemy is within the attack RANGE
    public static void fireSpell()
    {
        //Roll for damage, 1d4
        float totalRollResult = rollDice(4);

        //ASK WHAT THIS SPELL DOES

        //add code to increase self and teams HP stat that are within the range
        //Wizard.HP += totalRollResult;
        //something like that^
    }

    public override string ToString()
    {
        return string.Format("MassHealingWord: \n name: {0} \n level: {1} \n diceType: {2} \n range: {3} \n", name, level, diceType, range);
    }
}
}