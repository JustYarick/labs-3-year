namespace RLCExamples01
{
    public class BillGenerator
    {
        private Bill _bill;
        private IView _view;

        public IView View
        {
            get { return _view; }
            set { _view = value; }
        }

        public BillGenerator(Bill bill, IView view)
        {
            _bill = bill;
            _view = view;
        }

        public string GetBill()
        {
            BillSummary summary = _bill.Process();

            string result = _view.GetHeader(summary.CustomerName);

            foreach (ItemSummary item in summary.Items)
            {
                result += _view.GetItemString(
                    item.Title,
                    item.Price,
                    item.Quantity,
                    item.Sum,
                    item.Discount,
                    item.ThisAmount,
                    item.Bonus
                );
            }

            result += _view.GetFooter(summary.TotalAmount, summary.TotalBonus);

            return result;
        }
    }
}
