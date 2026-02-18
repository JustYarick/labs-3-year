namespace RLCExamples01
{
    public class Customer
    {
        private int bonus;
        private String name;

        public Customer(String name, int bonus)
        {
            this.name = name;
            this.bonus = bonus;
        }

        public String getName()
        {
            return name;
        }

        public int getBonus()
        {
            return bonus;
        }

        public void receiveBonus(int bonus)
        {
            this.bonus = bonus;
        }

        public int useBonus(int needBonus)
        {
            int bonusTaken;
            if (needBonus > bonus)
            {
                bonusTaken = bonus;
                bonus = 0;
            }
            else
            {
                bonusTaken = needBonus;
                bonus = bonus - needBonus;
            }
            return bonusTaken;
        }
    }
}
