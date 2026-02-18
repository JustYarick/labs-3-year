namespace RLCExamples01
{
    public class BillSummary
    {
        public string CustomerName { get; set; }
        public double TotalAmount { get; set; }
        public int TotalBonus { get; set; }
        public List<ItemSummary> Items { get; set; } = new List<ItemSummary>();
    }
}
