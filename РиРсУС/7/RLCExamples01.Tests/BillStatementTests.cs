namespace RLCExamples01.Tests
{
    public class BillStatementTests
    {
        private Customer MakeCustomer(string name = "Иван", int bonus = 0) =>
            new Customer(name, bonus);

        private (Bill bill, BillGenerator generator) MakeBill(Customer customer)
        {
            var bill = new Bill(customer);
            var generator = new BillGenerator(bill, new TxtView());
            return (bill, generator);
        }

        [Fact]
        public void Regular_Quantity2_NoDiscount_BonusCalculated()
        {
            var customer = MakeCustomer(bonus: 0);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new RegularGoods("Товар"), 2, 100.0));

            string result = generator.GetBill();

            Assert.Contains("200\t0\t200\t10", result);
            Assert.Contains("Сумма счета составляет 200", result);
            Assert.Contains("Вы заработали 10 бонусных балов", result);
        }

        [Fact]
        public void Regular_Quantity3_Discount3Percent_BonusCalculated()
        {
            var customer = MakeCustomer(bonus: 0);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new RegularGoods("Товар"), 3, 100.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 291", result);
            Assert.Contains("Вы заработали 15 бонусных балов", result);
        }

        [Fact]
        public void Regular_Quantity6_Discount3PctPlusBonusUsed()
        {
            var customer = MakeCustomer(bonus: 50);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new RegularGoods("Товар"), 6, 100.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 532", result);
        }

        [Fact]
        public void Regular_Quantity6_NoCustomerBonus_Only3PctDiscount()
        {
            var customer = MakeCustomer(bonus: 0);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new RegularGoods("Товар"), 6, 100.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 582", result);
        }

        [Fact]
        public void Sale_Quantity3_NoDiscount_BonusCalculated()
        {
            var customer = MakeCustomer(bonus: 0);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new SaleGoods("Акция"), 3, 200.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 600", result);
            Assert.Contains("Вы заработали 6 бонусных балов", result);
        }

        [Fact]
        public void Sale_Quantity4_Discount1Pct_Bonus1Pct()
        {
            var customer = MakeCustomer(bonus: 0);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new SaleGoods("Акция"), 4, 200.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 792", result);
            Assert.Contains("Вы заработали 8 бонусных балов", result);
        }

        [Fact]
        public void SpecialOffer_Quantity1_NoDiscountNoBonus()
        {
            var customer = MakeCustomer(bonus: 100);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new SpecialGoods("Спецпредложение"), 1, 150.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 150", result);
            Assert.Contains("Вы заработали 0 бонусных балов", result);
        }

        [Fact]
        public void SpecialOffer_Quantity5_BonusDiscountApplied()
        {
            var customer = MakeCustomer(bonus: 200);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new SpecialGoods("Спецпредложение"), 5, 100.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 300", result);
        }

        [Fact]
        public void SpecialOffer_Quantity11_BonusOverridesHalfPctDiscount()
        {
            var customer = MakeCustomer(bonus: 0);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new SpecialGoods("Спецпредложение"), 11, 10.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 110", result);
        }

        [Fact]
        public void MultiplItems_TotalAmountSummedCorrectly()
        {
            var customer = MakeCustomer(bonus: 0);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new RegularGoods("А"), 1, 100.0));
            bill.addGoods(new Item(new SaleGoods("Б"), 1, 50.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 150", result);
        }

        [Fact]
        public void MultipleItems_TotalBonusAccumulatedAndGrantedToCustomer()
        {
            var customer = MakeCustomer(bonus: 0);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new RegularGoods("А"), 1, 100.0));
            bill.addGoods(new Item(new SaleGoods("Б"), 4, 100.0));

            generator.GetBill();

            Assert.Equal(9, customer.getBonus());
        }

        [Fact]
        public void Statement_ContainsCustomerName()
        {
            var customer = MakeCustomer("Мария");
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new RegularGoods("Товар"), 1, 10.0));

            string result = generator.GetBill();

            Assert.Contains("Счет для Мария", result);
        }

        [Fact]
        public void Regular_Quantity6_CustomerBonusLessThanCost_OnlyAvailableBonusUsed()
        {
            var customer = MakeCustomer(bonus: 30);
            var (bill, generator) = MakeBill(customer);
            bill.addGoods(new Item(new RegularGoods("Товар"), 6, 100.0));

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 552", result);
            Assert.Equal(30, customer.getBonus());
        }

        [Fact]
        public void EmptyBill_TotalZero_BonusZero()
        {
            var customer = MakeCustomer(bonus: 100);
            var (bill, generator) = MakeBill(customer);

            string result = generator.GetBill();

            Assert.Contains("Сумма счета составляет 0", result);
            Assert.Contains("Вы заработали 0 бонусных балов", result);
        }
    }
}
