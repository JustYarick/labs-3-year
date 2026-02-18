namespace RLCExamples01
{
    public class RegularGoods : Goods
    {
        public RegularGoods(String title)
            : base(title) { }

        public override int GetBonus(int qty, double price)
        {
            return (int)(qty * price * 0.05);
        }

        public override double GetDiscount(int qty, double price)
        {
            if (qty > 2)
                return qty * price * 0.03;
            return 0;
        }
    }
}
