using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bozi
{
    internal class Program
    {
        static bool _continue;
        static SerialPort _serialPort;
        static void Main(string[] args)
        {

            string message;

            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            _serialPort = new SerialPort("COM7");

            _serialPort.BaudRate = 57600;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
            readThread.Start();

            Console.WriteLine("Type QUIT to exit");
            if(_continue)
                {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine(
                        String.Format("<{0}>", message));
                }
            }

            readThread.Join();
            _serialPort.Close();
        }
        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }
    }
}
