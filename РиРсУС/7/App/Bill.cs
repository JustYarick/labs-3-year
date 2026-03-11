namespace RLCExamples01
{
    public class Bill
    {
        private List<Item> _items;
        private Customer _customer;

        public Bill(Customer customer)
        {
            _customer = customer;
            _items = new List<Item>();
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

        public BillSummary Process()
        {
            var summary = new BillSummary();
            summary.CustomerName = _customer.getName();

            foreach (Item each in _items)
            {
                double itemSum = GetSum(each);
                double discount = each.GetDiscount();
                double usedBonus = GetUsedBonus(each, itemSum - discount);
                double thisAmount = itemSum - discount - usedBonus;
                int bonus = each.GetBonus();

                var itemSummary = new ItemSummary
                {
                    Title = each.getGoods().getTitle(),
                    Price = each.getPrice(),
                    Quantity = each.getQuantity(),
                    Sum = itemSum,
                    Discount = discount + usedBonus,
                    ThisAmount = thisAmount,
                    Bonus = bonus,
                };

                summary.Items.Add(itemSummary);
                summary.TotalAmount += thisAmount;
                summary.TotalBonus += bonus;
            }

            _customer.receiveBonus(summary.TotalBonus);
            return summary;
        }
    }
}
