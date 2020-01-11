using System;
using System.Diagnostics;
using RoSharp.Memory;
using RoSharp.Roblox;

namespace RoSharp
{
    class CheatEntry
    {
        private readonly RobloxLua robloxLuaState;

        public CheatEntry(Process process) 
        {
            robloxLuaState = new RobloxLua(process, new IntPtr(0x110000));
            
        }

        public void Initialize()
        {
            

            while (true)
            {
                string input = Console.ReadLine();
                robloxLuaState.GetGlobal("print");
                robloxLuaState.PushString(input);
                robloxLuaState.Call(1, 0);
            }
        }
    }
}
