namespace RLCExamples01
{
    public class SpecialGoods : Goods
    {
        public SpecialGoods(String title)
            : base(title) { }

        // GetBonus и GetDiscount возвращают 0 — наследуется от базового класса,
        // переопределять не нужно
    }
}
