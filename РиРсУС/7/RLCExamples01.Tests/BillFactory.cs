namespace RLCExamples01.Tests
{
    public class BillFactoryTests
    {
        private readonly string _regularFile =
            "customer: Иван\n"
            + "bonus: 0\n"
            + "items:\n"
            + "  - type: regular\n"
            + "    title: Молоко\n"
            + "    qty: 3\n"
            + "    price: 100.0\n";

        private readonly string _multipleItemsFile =
            "customer: Мария\n"
            + "bonus: 50\n"
            + "items:\n"
            + "  - type: regular\n"
            + "    title: Хлеб\n"
            + "    qty: 2\n"
            + "    price: 50.0\n"
            + "  - type: sale\n"
            + "    title: Молоко\n"
            + "    qty: 4\n"
            + "    price: 80.0\n";

        [Fact]
        public void CreateBill_Regular_Quantity3_CorrectTotal()
        {
            using var reader = new StringReader(_regularFile);

            IFileSource source = new YamlFileSource();
            var factory = new BillFactory(source);

            var bill = factory.CreateBill(reader);

            var generator = new BillGenerator(bill, new TxtView());

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 291", result);
            Assert.Contains("Вы заработали 15 бонусных балов", result);
        }

        [Fact]
        public void CreateBill_ContainsCustomerName()
        {
            using var reader = new StringReader(_regularFile);

            IFileSource source = new YamlFileSource();
            var factory = new BillFactory(source);

            var bill = factory.CreateBill(reader);

            var generator = new BillGenerator(bill, new TxtView());

            string result = generator.GetBill();

            Assert.Contains("Счет для Иван", result);
        }

        [Fact]
        public void CreateBill_MultipleItems_CorrectTotal()
        {
            using var reader = new StringReader(_multipleItemsFile);
            IFileSource source = new YamlFileSource();
            var factory = new BillFactory(source);

            var bill = factory.CreateBill(reader);
            var generator = new BillGenerator(bill, new TxtView());

            string result = generator.GetBill();

            Assert.Contains("Счет для Мария", result);
        }

        [Fact]
        public void GoodsFactory_CreatesCorrectTypes()
        {
            var factory = new GoodsFactory();

            Assert.IsType<RegularGoods>(factory.Create("regular", "Товар"));
            Assert.IsType<SaleGoods>(factory.Create("sale", "Товар"));
            Assert.IsType<SpecialGoods>(factory.Create("special", "Товар"));
        }

        [Fact]
        public void GoodsFactory_UnknownType_ThrowsException()
        {
            var factory = new GoodsFactory();

            Assert.Throws<ArgumentException>(() => factory.Create("unknown", "Товар"));
        }
    }
}
