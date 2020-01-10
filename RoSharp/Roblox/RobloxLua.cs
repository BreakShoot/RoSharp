using System;
using System.Diagnostics;
using RoSharp.Memory;

namespace RoSharp.Roblox
{
    class RobloxLua
    {
        private readonly IntPtr _luaState;
        private readonly RobloxSharp _rsharp;

        public RobloxLua(Process process, IntPtr baseAddress)
        {
            _rsharp = new RobloxSharp(process, baseAddress);
            _luaState = _rsharp.GetLuaState();
        }

        public void GetGlobal(string input)
            => GetField(-10002, input);

        public void GetField(int index, string input)
            => _rsharp[Offsets.lua_getfield_address.AddressValue].Execute<int>(Offsets.lua_getfield_address.CallingConvetion, _luaState, index, input);

        public void PushString(string input)
            => _rsharp[Offsets.lua_pushstring_address.AddressValue].Execute(Offsets.lua_pushstring_address.CallingConvetion, _luaState, input);

        public void PushNil()
            => _rsharp[Offsets.lua_pushstring_address.AddressValue].Execute(Offsets.lua_pushstring_address.CallingConvetion, _luaState, 0);

        public void Call(int a, int b)
            => _rsharp[Offsets.lua_call_address.AddressValue].Execute(Offsets.lua_call_address.CallingConvetion, _luaState, a, b);
    }
}
