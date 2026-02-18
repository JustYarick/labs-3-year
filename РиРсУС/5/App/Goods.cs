namespace RLCExamples01
{
    public class Goods
    {
        protected String _title;

        public Goods(String title)
        {
            _title = title;
        }

        public String getTitle()
        {
            return _title;
        }

        // Методы с возвращением значений по умолчанию
        public virtual int GetBonus(int qty, double price)
        {
            return 0;
        }

        public virtual double GetDiscount(int qty, double price)
        {
            return 0;
        }
    }
}
