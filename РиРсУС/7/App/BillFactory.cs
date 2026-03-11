namespace RLCExamples01
{
    public class BillFactory
    {
        private readonly GoodsFactory _goodsFactory = new GoodsFactory();
        private readonly IFileSource _fileSource;

        public BillFactory(IFileSource fileSource)
        {
            _fileSource = fileSource;
        }

        public Bill CreateBill(TextReader reader)
        {
            _fileSource.SetSource(reader);

            Customer customer = _fileSource.GetCustomer();
            Bill bill = new Bill(customer);

            int count = _fileSource.GetItemsCount();

            for (int i = 0; i < count; i++)
            {
                var (type, title, qty, price) = _fileSource.GetNextItem();

                Goods goods = _goodsFactory.Create(type, title);

                bill.addGoods(new Item(goods, qty, price));
            }

            return bill;
        }
    }
}
