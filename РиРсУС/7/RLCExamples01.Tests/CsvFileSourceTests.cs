namespace RLCExamples01.Tests
{
    public class CsvFileSourceTests
    {
        [Fact]
        public void ParseCsv_ShouldReadCustomerAndItems()
        {
            string csv =
                @"customer,John
bonus,10
book,Hobbit,2,15.5
food,Apple,5,1.2";

            var reader = new StringReader(csv);

            IFileSource source = new CsvFileSource();
            source.SetSource(reader);

            var customer = source.GetCustomer();

            Assert.Equal("John", customer.getName());
            Assert.Equal(10, customer.getBonus());
            Assert.Equal(2, source.GetItemsCount());

            var item = source.GetNextItem();

            Assert.Equal("book", item.type);
            Assert.Equal("Hobbit", item.title);
            Assert.Equal(2, item.qty);
            Assert.Equal(15.5, item.price);
        }
    }
}
