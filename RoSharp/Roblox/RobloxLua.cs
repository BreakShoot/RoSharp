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
            _rsharp.CleanAddresses();
            _luaState = _rsharp.GetLuaState();
        }

        public void GetGlobal(string input)
            => GetField(-10002, input);

        public void GetField(int index, string input)
            => _rsharp[Offsets.GlobalAddressTable["GetField"].AddressValue, false].Execute(Offsets.GlobalAddressTable["GetField"].CallingConvetion, _luaState, index, input);

        public void SetField(int index, string input)
            => _rsharp[Offsets.GlobalAddressTable["SetField"].AddressValue, false].Execute(Offsets.GlobalAddressTable["SetField"].CallingConvetion, _luaState, index, input);

        public void PushString(string input)
            => _rsharp[Offsets.GlobalAddressTable["PushString"].AddressValue].Execute(Offsets.GlobalAddressTable["PushString"].CallingConvetion, _luaState, input);

        public void PushNil()
            => _rsharp[Offsets.GlobalAddressTable["PushString"].AddressValue].Execute(Offsets.GlobalAddressTable["PushString"].CallingConvetion, _luaState, 0);

        public void Call(int a, int b)
            => _rsharp[Offsets.GlobalAddressTable["Call"].AddressValue, false].Execute(Offsets.GlobalAddressTable["Call"].CallingConvetion, _luaState, a, b);
    }
}
