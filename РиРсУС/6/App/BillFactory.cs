namespace RLCExamples01
{
    public class BillFactory
    {
        private readonly GoodsFactory _goodsFactory = new GoodsFactory();

        public Bill CreateBill(TextReader reader)
        {
            var contentFile = new ContentFile();
            contentFile.SetSource(reader);

            Customer customer = contentFile.GetCustomer();
            Bill bill = new Bill(customer);

            int count = contentFile.GetItemsCount();
            for (int i = 0; i < count; i++)
            {
                var (type, title, qty, price) = contentFile.GetNextItem();
                Goods goods = _goodsFactory.Create(type, title);
                bill.addGoods(new Item(goods, qty, price));
            }

            return bill;
        }
    }
}
