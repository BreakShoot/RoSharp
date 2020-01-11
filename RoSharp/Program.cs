using System;
using System.Diagnostics;
using System.Threading;
using RoSharp;

namespace KeksV5
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "RoSharp Beta v0.01";
            Logger.Log(Logger.LogType.WORK, "Looking for ROBLOX process...");

            ProcessWatcher processWatcher = new ProcessWatcher("RobloxPlayerBeta");

            processWatcher.Created += (sender, process) =>
            {
                Thread.Sleep(4000);
                Logger.Log(Logger.LogType.SUCCESS, "Successfully located ROBLOX process!");
                new CheatEntry(process).Initialize();
            };

            Console.ReadKey();
        }
    }
}