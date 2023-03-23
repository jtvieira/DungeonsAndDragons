namespace Characters
{
public class Aid : Spell
{
    //This spell increases its targets Hit Point Maximum
    //and current Hit Points by 5 for each slot level above 2nd

    public Aid()
    {
        base.name = "Aid";
        base.level = "2";
        base.diceType = 0; //always hits
        base.range = 6;
    }

    //targets hit points increase by +5 for each slot adove 2nd

    public override string ToString()
    {
        return string.Format("Aid: \n name: {0} \n level: {1} \n diceType: {2} \n range: {3} \n", name, level, diceType, range);
    }
}
}