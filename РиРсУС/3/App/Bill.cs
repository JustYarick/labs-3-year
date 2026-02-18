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

        private string GetHeader()
        {
            return "Счет для "
                + _customer.getName()
                + "\n"
                + "\t"
                + "Название"
                + "\t"
                + "Цена"
                + "\t"
                + "Кол-во"
                + "Стоимость"
                + "\t"
                + "Скидка"
                + "\t"
                + "Сумма"
                + "\t"
                + "Бонус"
                + "\n";
        }

        private string GetItemString(Item item, double discount, double thisAmount, int bonus)
        {
            return "\t"
                + item.getGoods().getTitle()
                + "\t"
                + "\t"
                + item.getPrice()
                + "\t"
                + item.getQuantity()
                + "\t"
                + GetSum(item).ToString()
                + "\t"
                + discount.ToString()
                + "\t"
                + thisAmount.ToString()
                + "\t"
                + bonus.ToString()
                + "\n";
        }

        private string GetFooter(double totalAmount, int totalBonus)
        {
            return "Сумма счета составляет "
                + totalAmount.ToString()
                + "\n"
                + "Вы заработали "
                + totalBonus.ToString()
                + " бонусных балов";
        }

        public string statement()
        {
            double totalAmount = 0;
            int totalBonus = 0;

            string result = GetHeader();

            foreach (Item each in _items)
            {
                double itemSum = GetSum(each);
                double discount = each.GetDiscount();
                double usedBonus = GetUsedBonus(each, itemSum - discount);
                double thisAmount = itemSum - discount - usedBonus;
                int bonus = each.GetBonus();

                result += GetItemString(each, discount + usedBonus, thisAmount, bonus);

                totalAmount += thisAmount;
                totalBonus += bonus;
            }

            result += GetFooter(totalAmount, totalBonus);

            _customer.receiveBonus(totalBonus);
            return result;
        }
    }
}
