using LogStandartization.Models;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("THE LOG STANDARTIZATION IS RUNNING...");

        try
        {
            var controller = new LogFileStandartization();
            controller.GetLogFile();

            Console.WriteLine("THE LOG STANDARTIZATION SUCCESSFULLY COMPLETED.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}