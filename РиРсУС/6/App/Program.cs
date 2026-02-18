namespace RLCExamples01
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fileName = "bill.yaml";
            using (var reader = new StreamReader(fileName))
            {
                Bill bill = CreateBill(reader);
                var generator = new BillGenerator(bill, new TxtView());
                Console.WriteLine(generator.GetBill());
            }
        }

        public static Bill CreateBill(TextReader reader)
        {
            var factory = new BillFactory();
            return factory.CreateBill(reader);
        }
    }
}
