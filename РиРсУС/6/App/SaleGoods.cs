namespace RLCExamples01
{
    public class SaleGoods : Goods
    {
        public SaleGoods(String title)
            : base(title) { }

        public override int GetBonus(int qty, double price)
        {
            return (int)(qty * price * 0.01);
        }

        public override double GetDiscount(int qty, double price)
        {
            if (qty > 3)
                return qty * price * 0.01;
            return 0;
        }
    }
}
