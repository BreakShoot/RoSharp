using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using RoSharp.Memory;

namespace RoSharp.Roblox
{
    class RobloxLua
    {
        private readonly IntPtr _luaStateBaseAddress;
        private readonly RobloxSharp _rsharp;

        public RobloxLua(Process process, IntPtr baseAddress)
        {
            _rsharp = new RobloxSharp(process, baseAddress);
            _rsharp.CleanAddresses();
            _luaStateBaseAddress = _rsharp.GetLuaState();
        }

        private LuaStructs.TValue Index2Adr(int index)
            => _rsharp[Offsets.GlobalAddressTable["Index2Adr"].AddressValue].Execute<LuaStructs.TValue>(Offsets.GlobalAddressTable["Index2Adr"].CallingConvetion, _luaStateBaseAddress, index);
        public void SetIdentity(int identity) 
            =>_rsharp[(IntPtr)(_rsharp[_luaStateBaseAddress + 128, false].Read<int>() + 24), false].Write(6);
        public void GetGlobal(string input)
            => GetField(-10002, input);
        public void GetField(int index, string input)
            => _rsharp[Offsets.GlobalAddressTable["GetField"].AddressValue, false].Execute(Offsets.GlobalAddressTable["GetField"].CallingConvetion, _luaStateBaseAddress, index, input);
        public void SetField(int index, string input)
            => _rsharp[Offsets.GlobalAddressTable["SetField"].AddressValue, false].Execute(Offsets.GlobalAddressTable["SetField"].CallingConvetion, _luaStateBaseAddress, index, input);
        public void PushString(string input)
            => _rsharp[Offsets.GlobalAddressTable["PushString"].AddressValue].Execute(Offsets.GlobalAddressTable["PushString"].CallingConvetion, _luaStateBaseAddress, input);
        public void PushNil()
            => _rsharp[Offsets.GlobalAddressTable["PushString"].AddressValue].Execute(Offsets.GlobalAddressTable["PushString"].CallingConvetion, _luaStateBaseAddress, 0);
        public void Call(int a, int b)
            => _rsharp[Offsets.GlobalAddressTable["Call"].AddressValue, false].Execute(Offsets.GlobalAddressTable["Call"].CallingConvetion, _luaStateBaseAddress, a, b);
        public void PushNumber(int input)
        {

        }
        public string ToString(int index)
        {
            LuaStructs.TValue o = this.Index2Adr(index);

            if (o.tt == (int) LuaStructs.LuaTypes.LUA_TSTRING)
                return _rsharp[new IntPtr(o.value_i + 24), false].ReadString(0);
            return null;
        }
    }
}
