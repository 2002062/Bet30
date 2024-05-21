using System;
using System.IO;

namespace Lotaria.Logging
{
    public class ConsoleAndJsonLogger : IMessage
    {
        private string _logFilePath;

        // Construtor que aceita o caminho do arquivo de log
        public ConsoleAndJsonLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Debug(string message)
        {
            Log("DEBUG", message);
        }

        public void Warning(string message)
        {
            Log("WARNING", message);
        }

        public void Error(string message)
        {
            Log("ERROR", message);
        }

        private void Log(string level, string message)
        {
            // Escreve a mensagem na console
            Console.WriteLine($"{level}: {message}");

            // Adiciona a mensagem ao arquivo de log
            File.AppendAllText(_logFilePath, $"{DateTime.Now} - {level}: {message}\n");
        }
    }
}
