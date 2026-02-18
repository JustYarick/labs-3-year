namespace RLCExamples01
{
    public class ContentFile
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

        private string GetNextLine()
        {
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;
                return line;
            }
            return null;
        }

        private void Parse()
        {
            string line;
            while ((line = GetNextLine()) != null)
            {
                if (line.StartsWith("customer:"))
                    _customerName = line.Split(':')[1].Trim();
                else if (line.StartsWith("bonus:"))
                    _customerBonus = int.Parse(line.Split(':')[1].Trim());
                else if (line.StartsWith("- type:"))
                {
                    string type = line.Split(':')[1].Trim();
                    string title = GetNextLine().Split(':')[1].Trim();
                    int qty = int.Parse(GetNextLine().Split(':')[1].Trim());
                    double price = double.Parse(
                        GetNextLine().Split(':')[1].Trim(),
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
