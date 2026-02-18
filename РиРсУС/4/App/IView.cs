namespace RLCExamples01
{
    public interface IView
    {
        string GetHeader(string customerName);
        string GetItemString(
            string title,
            double price,
            int quantity,
            double sum,
            double discount,
            double thisAmount,
            int bonus
        );
        string GetFooter(double totalAmount, int totalBonus);
    }
}
