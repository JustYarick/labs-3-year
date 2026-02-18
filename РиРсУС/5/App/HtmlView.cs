namespace RLCExamples01
{
    public class HtmlView : IView
    {
        public string GetHeader(string customerName)
        {
            return "<h1>Счет для "
                + customerName
                + "</h1>\n"
                + "<table>\n"
                + "<tr><th>Название</th><th>Цена</th><th>Кол-во</th>"
                + "<th>Стоимость</th><th>Скидка</th><th>Сумма</th><th>Бонус</th></tr>\n";
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
            return "<tr>"
                + "<td>"
                + title
                + "</td>"
                + "<td>"
                + price
                + "</td>"
                + "<td>"
                + quantity
                + "</td>"
                + "<td>"
                + sum
                + "</td>"
                + "<td>"
                + discount
                + "</td>"
                + "<td>"
                + thisAmount
                + "</td>"
                + "<td>"
                + bonus
                + "</td>"
                + "</tr>\n";
        }

        public string GetFooter(double totalAmount, int totalBonus)
        {
            return "</table>\n"
                + "<p>Сумма счета составляет "
                + totalAmount
                + "</p>\n"
                + "<p>Вы заработали "
                + totalBonus
                + " бонусных балов</p>";
        }
    }
}
