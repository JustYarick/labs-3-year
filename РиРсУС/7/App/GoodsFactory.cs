namespace RLCExamples01
{
    public class GoodsFactory
    {
        public Goods Create(string type, string title)
        {
            switch (type.ToLower())
            {
                case "regular":
                    return new RegularGoods(title);
                case "sale":
                    return new SaleGoods(title);
                case "special":
                    return new SpecialGoods(title);
                default:
                    throw new ArgumentException($"Неизвестный тип товара: {type}");
            }
        }
    }
}
