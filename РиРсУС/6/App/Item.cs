namespace RLCExamples01
{
    public class Item
    {
        private Goods _Goods;
        private int _quantity;
        private double _price;

        public Item(Goods Goods, int quantity, double price)
        {
            _Goods = Goods;
            _quantity = quantity;
            _price = price;
        }

        public int getQuantity()
        {
            return _quantity;
        }

        public double getPrice()
        {
            return _price;
        }

        public Goods getGoods()
        {
            return _Goods;
        }

        public int GetBonus()
        {
            return _Goods.GetBonus(_quantity, _price);
        }

        public double GetDiscount()
        {
            return _Goods.GetDiscount(_quantity, _price);
        }
    }
}
