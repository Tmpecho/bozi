using System;
using System.IO.Ports;
using System.Threading;

namespace bozi
{
    internal class Program
    {
        static bool _continue;
        static SerialPort _serialPort;

        static void Main(string[] args)
        {
            var readThread = new Thread(Read);

            InitializeSerialPort();

            _continue = true;
            readThread.Start();

            Console.WriteLine("Type QUIT to exit");

            ReadUserInput();

            readThread.Join();
            _serialPort.Close();
        }

        private static void InitializeSerialPort()
        {
            _serialPort = new SerialPort("COM7")
            {
                BaudRate = 57600,
                DataBits = 8,
                StopBits = StopBits.One,
                ReadTimeout = 500,
                WriteTimeout = 500
            };

            _serialPort.Open();
        }

        private static void ReadUserInput()
        {
            while (_continue)
            {
                var message = Console.ReadLine();

                if (string.Equals(nameof(quit), message, StringComparison.OrdinalIgnoreCase))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine($"<{message}>");
                }
            }
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    var message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }
    }
}
