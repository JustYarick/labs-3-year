namespace RLCExamples01.Tests
{
    public class YamlFileSourceTests
    {
        [Fact]
        public void ParseYaml_ShouldReadCustomerAndItems()
        {
            string yaml =
                @"customer: John
bonus: 10
- type: book
title: Hobbit
qty: 2
price: 15.5
- type: food
title: Apple
qty: 5
price: 1.2";

            var reader = new StringReader(yaml);

            IFileSource source = new YamlFileSource();
            source.SetSource(reader);

            var customer = source.GetCustomer();

            Assert.Equal("John", customer.getName());
            Assert.Equal(10, customer.getBonus());
            Assert.Equal(2, source.GetItemsCount());

            var item1 = source.GetNextItem();

            Assert.Equal("book", item1.type);
            Assert.Equal("Hobbit", item1.title);
            Assert.Equal(2, item1.qty);
            Assert.Equal(15.5, item1.price);
        }
    }
}
