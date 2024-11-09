using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        Console.Write("Enter the server IP address: ");
        string serverIp = Console.ReadLine();

        int port = 5002;

        using (TcpClient client = new TcpClient(serverIp, port))
        {
            using (NetworkStream stream = client.GetStream())
            {
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                while (true)
                {
                    // دریافت دستور از کاربر و ارسال به سرور
                    Console.Write("Enter command: ");
                string command = Console.ReadLine();
                writer.WriteLine(command);


                if (command.ToLower() == "exit") break; // خروج از برنامه
                                                        // آماده‌سازی فایل برای نوشتن نتیجه
                    using (StreamWriter fileWriter = new StreamWriter("results.txt", true))
                    {
                        fileWriter.WriteLine($"Command: {command}");
                        fileWriter.WriteLine("Result:");
                        // خواندن و نمایش نتیجه از سرور
                        Console.WriteLine("Command sent. Waiting for response...");

                        string result;
                        while ((result = reader.ReadLine()) != null)
                        {
                            if (result == "END_OF_MESSAGE")
                            {
                                break; // پایان پیام از سرور
                            }
                            Console.WriteLine(result);
                            fileWriter.WriteLine(result); // ذخیره در فایل
                        }
                        fileWriter.WriteLine(new string('-', 50)); // جداکننده برای خوانایی بیشتر
                        fileWriter.WriteLine();
                    }
                    }
            }
        }
        Console.WriteLine("Response received. Client finished.");
        Console.ReadKey(); // منتظر می‌ماند تا کاربر یک کلید را فشار دهد
    }
}
