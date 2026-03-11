using System.IO;

namespace RLCExamples01
{
    public interface IFileSource
    {
        void SetSource(TextReader reader);

        Customer GetCustomer();

        int GetItemsCount();

        (string type, string title, int qty, double price) GetNextItem();
    }
}
