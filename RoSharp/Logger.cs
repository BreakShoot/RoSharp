using System;


namespace KeksV5
{
    class Logger
    {
        public enum LogType
        {
            ERROR,
            WORK,
            SUCCESS,
            QUESTION
        }

        public static void Log(LogType type, string input, params object[] obj)
        {
            if (type == LogType.ERROR)
                Console.ForegroundColor = ConsoleColor.Red;
            if (type == LogType.SUCCESS)
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            if (type == LogType.QUESTION)
                Console.ForegroundColor = ConsoleColor.DarkRed;
            if (type == LogType.WORK)
                Console.ForegroundColor = ConsoleColor.White;

            if (obj.Length == 0)
                Console.WriteLine(input);
            else
                Console.WriteLine(input, obj);

            Console.ResetColor();
        }
    }
}
