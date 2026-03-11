namespace RLCExamples01
{
    public class CsvFileSource : IFileSource
    {
        private TextReader _reader;
        private string _customerName;
        private int _customerBonus;

        private List<(string type, string title, int qty, double price)> _items;
        private int _currentItem;

        public void SetSource(TextReader reader)
        {
            _reader = reader;
            _items = new List<(string, string, int, double)>();
            Parse();
        }

        private void Parse()
        {
            string line;

            while ((line = _reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                if (parts[0] == "customer")
                    _customerName = parts[1];
                else if (parts[0] == "bonus")
                    _customerBonus = int.Parse(parts[1]);
                else
                {
                    string type = parts[0];
                    string title = parts[1];
                    int qty = int.Parse(parts[2]);
                    double price = double.Parse(
                        parts[3],
                        System.Globalization.CultureInfo.InvariantCulture
                    );

                    _items.Add((type, title, qty, price));
                }
            }

            _currentItem = 0;
        }

        public Customer GetCustomer()
        {
            return new Customer(_customerName, _customerBonus);
        }

        public int GetItemsCount()
        {
            return _items.Count;
        }

        public (string type, string title, int qty, double price) GetNextItem()
        {
            return _items[_currentItem++];
        }
    }
}
