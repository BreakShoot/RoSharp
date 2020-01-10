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
            robloxLuaState = new RobloxLua(process, new IntPtr(0x300000));
            
        }

        public void Initialize()
        {
            Console.WriteLine("Getfield {0:X}", Offsets.lua_getfield_address.AddressValue.ToInt32());
            robloxLuaState.GetGlobal("print");
            robloxLuaState.PushString("test");
            robloxLuaState.Call(1, 0);
        }
    }
}
