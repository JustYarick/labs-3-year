namespace RLCExamples01
{
    public class Goods
    {
        public const int REGULAR = 0;
        public const int SALE = 1;
        public const int SPECIAL_OFFER = 2;

        private String _title;
        private int _priceCode;

        public Goods(String title, int priceCode)
        {
            _title = title;
            _priceCode = priceCode;
        }

        public int getPriceCode()
        {
            return _priceCode;
        }

        public void setPriceCode(int arg)
        {
            _priceCode = arg;
        }

        public String getTitle()
        {
            return _title;
        }

        public int GetBonus(int qty, double price)
        {
            double sum = qty * price;
            switch (_priceCode)
            {
                case REGULAR:
                    return (int)(sum * 0.05);
                case SALE:
                    return (int)(sum * 0.01);
                default:
                    return 0;
            }
        }

        public double GetDiscount(int qty, double price)
        {
            double sum = qty * price;
            switch (_priceCode)
            {
                case REGULAR:
                    if (qty > 2)
                        return sum * 0.03;
                    return 0;
                case SALE:
                    if (qty > 3)
                        return sum * 0.01;
                    return 0;
                case SPECIAL_OFFER:
                default:
                    return 0;
            }
        }
    }
}
