namespace Characters
{
    public class Spell
    {
        //Spell stats
        public string name;
        public string level;
        public float diceType;
        public float range;

        //Other variables
        static System.Random random = new System.Random(); //This is for the random dice roller

        //Function that rolls specific diceType for spell
        public static float rollDice(float diceType)
        {
            return (float)random.Next(1, (int)diceType + 1);
        }

    }
}