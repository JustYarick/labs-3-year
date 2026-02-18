namespace RLCExamples01
{
    public class TxtView : IView
    {
        public string GetHeader(string customerName)
        {
            return "Счет для "
                + customerName
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

        public string GetItemString(
            string title,
            double price,
            int quantity,
            double sum,
            double discount,
            double thisAmount,
            int bonus
        )
        {
            return "\t"
                + title
                + "\t\t"
                + price
                + "\t"
                + quantity
                + "\t"
                + sum.ToString()
                + "\t"
                + discount.ToString()
                + "\t"
                + thisAmount.ToString()
                + "\t"
                + bonus.ToString()
                + "\n";
        }

        public string GetFooter(double totalAmount, int totalBonus)
        {
            return "Сумма счета составляет "
                + totalAmount.ToString()
                + "\n"
                + "Вы заработали "
                + totalBonus.ToString()
                + " бонусных балов";
        }
    }
}
