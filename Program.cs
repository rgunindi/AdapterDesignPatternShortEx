interface IBankApi
{
    bool ExecuteTransaction(TransferTransaction transaction);
}

class XmlBankApi : IBankApi
{
    public bool ExecuteTransaction(TransferTransaction transaction)
    {
        var xml = $"""
                   <TransferTransaction>
                   <FromIBAN>{transaction.FromIBAN}</FromIBAN>
                    <ToIBAN>
                    {transaction.ToIBAN}
                    </ToIBAN>
                    <Amount>
                    {transaction.Amount:C2}
                    </Amount>
                   </TransferTransaction> 
                   """;
        //Calling bank API with xml
        Console.WriteLine($"{GetType().Name} working...\n{xml}");
        return true;
    }
}

class JsonBankApi : IBankApi
{
    public bool ExecuteTransaction(TransferTransaction transaction)
    {
        var json = $$"""
                     {
                     ""FromIBAN"":""{{transaction.FromIBAN}}"",
                     ""ToIBAN"":""{{transaction.ToIBAN}}"",
                     ""Amount"":""{{transaction.Amount:C2}}""
                     }
                     """;
        //Calling bank API with json
        Console.WriteLine($"{GetType().Name} working...\n{json}");
        return true;
    }
}

class XmlBankApiAdapter : IBankApi
{
    private readonly XmlBankApi _xmlBankApi = new();

    public bool ExecuteTransaction(TransferTransaction transaction)
    {
        return _xmlBankApi.ExecuteTransaction(transaction);
    }
}

class JsonBankApiAdapter : IBankApi
{
    private readonly JsonBankApi _jsonBankApi = new();

    public bool ExecuteTransaction(TransferTransaction transaction)
    {
        return _jsonBankApi.ExecuteTransaction(transaction);
    }
}

class TransferTransaction
{
    public string FromIBAN { get; set; }
    public string ToIBAN { get; set; }
    public decimal Amount { get; set; }
}

internal class Program
{
    private static Action<string?> _writeLine;

    public static void Main(string[] args)
    {
        var transaction = new TransferTransaction
        {
            FromIBAN = "TR123456789",
            ToIBAN = "TR987654321",
            Amount = 1000
        };
        /*
        var xmlBankApiAdapter = new XmlBankApiAdapter();
        var result = xmlBankApiAdapter.ExecuteTransaction(transaction);
        Console.WriteLine("xmlBankApiAdapter result: " + result);
        var jsonBankApiAdapter = new JsonBankApiAdapter();
        result = jsonBankApiAdapter.ExecuteTransaction(transaction);
        Console.WriteLine("jsonBankApiAdapter result: " + result);
        */
        var adapters = new IBankApi[] { new XmlBankApiAdapter(), new JsonBankApiAdapter() };
        foreach (var adapter in adapters)
        {
            var result = adapter.ExecuteTransaction(transaction);
            _writeLine = Console.WriteLine;
            _writeLine($"{adapter.GetType().Name} result: " + result);
        }
    }
}