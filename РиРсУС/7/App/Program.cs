namespace RLCExamples01
{
    public class Program
    {
        static void Main(string[] args)
        {
            string fileName = "bill.yaml";

            IFileSource source = FileSourceFactory.Create(fileName);

            using (var reader = new StreamReader(fileName))
            {
                var factory = new BillFactory(source);
                Bill bill = factory.CreateBill(reader);

                var generator = new BillGenerator(bill, new TxtView());
                Console.WriteLine(generator.GetBill());
            }
        }
    }
}
