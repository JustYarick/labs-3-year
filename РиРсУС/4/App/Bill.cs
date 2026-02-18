namespace RLCExamples01
{
    public class Bill
    {
        private List<Item> _items;
        private Customer _customer;
        private IView _view;

        public IView View
        {
            get { return _view; }
            set { _view = value; }
        }

        public Bill(Customer customer, IView view)
        {
            _customer = customer;
            _items = new List<Item>();
            _view = view;
        }

        public void addGoods(Item arg)
        {
            _items.Add(arg);
        }

        private double GetSum(Item item)
        {
            return item.getQuantity() * item.getPrice();
        }

        private double GetUsedBonus(Item item, double sumWithDiscount)
        {
            if (item.getGoods().GetType() == typeof(RegularGoods))
            {
                if (item.getQuantity() > 5)
                    return _customer.useBonus((int)sumWithDiscount);
                return 0;
            }
            else if (item.getGoods().GetType() == typeof(SpecialGoods))
            {
                if (item.getQuantity() > 1)
                    return _customer.useBonus((int)sumWithDiscount);
                return 0;
            }
            return 0;
        }

        public string GetBill()
        {
            double totalAmount = 0;
            int totalBonus = 0;

            string result = _view.GetHeader(_customer.getName());

            foreach (Item each in _items)
            {
                double itemSum = GetSum(each);
                double discount = each.GetDiscount();
                double usedBonus = GetUsedBonus(each, itemSum - discount);
                double thisAmount = itemSum - discount - usedBonus;
                int bonus = each.GetBonus();

                result += _view.GetItemString(
                    each.getGoods().getTitle(),
                    each.getPrice(),
                    each.getQuantity(),
                    itemSum,
                    discount + usedBonus,
                    thisAmount,
                    bonus
                );

                totalAmount += thisAmount;
                totalBonus += bonus;
            }

            result += _view.GetFooter(totalAmount, totalBonus);

            _customer.receiveBonus(totalBonus);
            return result;
        }
    }
}
