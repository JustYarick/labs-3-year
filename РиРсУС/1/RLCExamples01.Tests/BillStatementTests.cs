using RLCExamples01;
using Xunit;

namespace RLCExamples01.Tests
{
    public class BillStatementTests
    {
        // ─────────────────────────────────────────────
        // Вспомогательные методы
        // ─────────────────────────────────────────────
        private Customer MakeCustomer(string name = "Иван", int bonus = 0) =>
            new Customer(name, bonus);

        private Bill MakeBill(Customer customer) => new Bill(customer);

        // ─────────────────────────────────────────────
        // TC-01: REGULAR, кол-во ≤ 2, скидки нет, бонус = floor(qty*price*0.05)
        // ─────────────────────────────────────────────
        [Fact]
        public void Regular_Quantity2_NoDiscount_BonusCalculated()
        {
            // qty=2, price=100 → скидка=0, бонус=floor(200*0.05)=10
            var customer = MakeCustomer(bonus: 0);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Товар", Goods.REGULAR), 2, 100.0));

            string result = bill.statement();

            Assert.Contains("200\t0\t200\t10", result); // стоимость/скидка/сумма/бонус
            Assert.Contains("Сумма счета составляет 200", result);
            Assert.Contains("Вы заработали 10 бонусных балов", result);
        }

        // ─────────────────────────────────────────────
        // TC-02: REGULAR, кол-во > 2 и ≤ 5, скидка 3 %, бонус начисляется
        // ─────────────────────────────────────────────
        [Fact]
        public void Regular_Quantity3_Discount3Percent_BonusCalculated()
        {
            // qty=3, price=100 → скидка=300*0.03=9, бонус=floor(300*0.05)=15
            var customer = MakeCustomer(bonus: 0);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Товар", Goods.REGULAR), 3, 100.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 291", result);
            Assert.Contains("Вы заработали 15 бонусных балов", result);
        }

        // ─────────────────────────────────────────────
        // TC-03: REGULAR, кол-во > 5, скидка 3 % + бонусная скидка клиента
        // ─────────────────────────────────────────────
        [Fact]
        public void Regular_Quantity6_Discount3PctPlusBonusUsed()
        {
            // qty=6, price=100 → базовая скидка=600*0.03=18
            // бонус клиента=50 → useBonus(600)=50 → итого скидка=68
            // итого сумма=600-68=532
            var customer = MakeCustomer(bonus: 50);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Товар", Goods.REGULAR), 6, 100.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 532", result);
        }

        // ─────────────────────────────────────────────
        // TC-04: REGULAR, кол-во > 5, клиент без бонусов — бонусная скидка = 0
        // ─────────────────────────────────────────────
        [Fact]
        public void Regular_Quantity6_NoCustomerBonus_Only3PctDiscount()
        {
            // скидка только 3 %: 600*0.03=18 → итого 582
            var customer = MakeCustomer(bonus: 0);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Товар", Goods.REGULAR), 6, 100.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 582", result);
        }

        // ─────────────────────────────────────────────
        // TC-05: SALE, кол-во ≤ 3, скидки нет, бонус = floor(qty*price*0.01)
        // ─────────────────────────────────────────────
        [Fact]
        public void Sale_Quantity3_NoDiscount_BonusCalculated()
        {
            // qty=3, price=200 → скидка=0, бонус=floor(600*0.01)=6
            var customer = MakeCustomer(bonus: 0);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Акция", Goods.SALE), 3, 200.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 600", result);
            Assert.Contains("Вы заработали 6 бонусных балов", result);
        }

        // ─────────────────────────────────────────────
        // TC-06: SALE, кол-во > 3, скидка 1 %, бонус 1 %
        // ─────────────────────────────────────────────
        [Fact]
        public void Sale_Quantity4_Discount1Pct_Bonus1Pct()
        {
            // qty=4, price=200 → стоимость=800, скидка=800*0.01=8, бонус=floor(800*0.01)=8
            var customer = MakeCustomer(bonus: 0);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Акция", Goods.SALE), 4, 200.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 792", result);
            Assert.Contains("Вы заработали 8 бонусных балов", result);
        }

        // ─────────────────────────────────────────────
        // TC-07: SPECIAL_OFFER, кол-во = 1, скидки нет, бонуса нет
        // ─────────────────────────────────────────────
        [Fact]
        public void SpecialOffer_Quantity1_NoDiscountNoBonus()
        {
            // qty=1 → ни скидка 0.5 %, ни бонусная скидка не применяются
            var customer = MakeCustomer(bonus: 100);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Спецпредложение", Goods.SPECIAL_OFFER), 1, 150.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 150", result);
            Assert.Contains("Вы заработали 0 бонусных балов", result);
        }

        // ─────────────────────────────────────────────
        // TC-08: SPECIAL_OFFER, 1 < кол-во ≤ 10, только бонусная скидка клиента
        // ─────────────────────────────────────────────
        [Fact]
        public void SpecialOffer_Quantity5_BonusDiscountApplied()
        {
            // qty=5, price=100 → стоимость=500
            // условие qty>1 → скидка=useBonus(500); клиент имеет 200 → скидка=200
            // итого сумма=300
            var customer = MakeCustomer(bonus: 200);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Спецпредложение", Goods.SPECIAL_OFFER), 5, 100.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 300", result);
        }

        // ─────────────────────────────────────────────
        // TC-09: SPECIAL_OFFER, кол-во > 10, скидка 0.5 % переопределяется бонусной скидкой
        // ─────────────────────────────────────────────
        [Fact]
        public void SpecialOffer_Quantity11_BonusOverridesHalfPctDiscount()
        {
            // qty=11, price=10 → стоимость=110
            // case: qty>10 → discount=110*0.005=0.55
            // затем qty>1 → discount=useBonus(110); клиент bonus=0 → discount=0 (override!)
            // итого сумма=110
            var customer = MakeCustomer(bonus: 0);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Спецпредложение", Goods.SPECIAL_OFFER), 11, 10.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 110", result);
        }

        // ─────────────────────────────────────────────
        // TC-10: Несколько товаров, итоговая сумма суммируется корректно
        // ─────────────────────────────────────────────
        [Fact]
        public void MultiplItems_TotalAmountSummedCorrectly()
        {
            // REGULAR qty=1 price=100 → 100; SALE qty=1 price=50 → 50; итого=150
            var customer = MakeCustomer(bonus: 0);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("А", Goods.REGULAR), 1, 100.0));
            bill.addGoods(new Item(new Goods("Б", Goods.SALE), 1, 50.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 150", result);
        }

        // ─────────────────────────────────────────────
        // TC-11: Бонусы нескольких товаров суммируются и зачисляются клиенту
        // ─────────────────────────────────────────────
        [Fact]
        public void MultipleItems_TotalBonusAccumulatedAndGrantedToCustomer()
        {
            // REGULAR qty=1 price=100 → бонус=floor(100*0.05)=5
            // SALE qty=4 price=100 → бонус=floor(400*0.01)=4
            // итого бонусов=9; после receiveBonus клиент имеет 9
            var customer = MakeCustomer(bonus: 0);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("А", Goods.REGULAR), 1, 100.0));
            bill.addGoods(new Item(new Goods("Б", Goods.SALE), 4, 100.0));

            bill.statement();

            Assert.Equal(9, customer.getBonus());
        }

        // ─────────────────────────────────────────────
        // TC-12: Имя покупателя отображается в заголовке счёта
        // ─────────────────────────────────────────────
        [Fact]
        public void Statement_ContainsCustomerName()
        {
            var customer = MakeCustomer("Мария");
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Товар", Goods.REGULAR), 1, 10.0));

            string result = bill.statement();

            Assert.Contains("Счет для Мария", result);
        }

        // ─────────────────────────────────────────────
        // TC-13: useBonus списывает не больше, чем есть у клиента (граничный случай)
        // ─────────────────────────────────────────────
        [Fact]
        public void Regular_Quantity6_CustomerBonusLessThanCost_OnlyAvailableBonusUsed()
        {
            // qty=6, price=100 → cost=600; клиент bonus=30
            // useBonus(600)=30 (не хватает) → скидка=18+30=48 → итого=552
            // После statement() вызывается receiveBonus(totalBonus):
            //   totalBonus = floor(600*0.05) = 30 → bonus становится равен 30
            var customer = MakeCustomer(bonus: 30);
            var bill = MakeBill(customer);
            bill.addGoods(new Item(new Goods("Товар", Goods.REGULAR), 6, 100.0));

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 552", result);
            // receiveBonus(30) перезаписывает бонус: клиент получает 30 новых бонусных баллов
            Assert.Equal(30, customer.getBonus());
        }

        // ─────────────────────────────────────────────
        // TC-14: Пустой счёт — итоговая сумма 0, бонусов 0
        // ─────────────────────────────────────────────
        [Fact]
        public void EmptyBill_TotalZero_BonusZero()
        {
            var customer = MakeCustomer(bonus: 100);
            var bill = MakeBill(customer);

            string result = bill.statement();

            Assert.Contains("Сумма счета составляет 0", result);
            Assert.Contains("Вы заработали 0 бонусных балов", result);
        }
    }
}
