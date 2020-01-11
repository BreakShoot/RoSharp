using System.Collections.Generic;
using Binarysharp.MemoryManagement.Assembly.CallingConvention;

namespace RoSharp.Memory
{
    static class Offsets
    {
        public static Dictionary<string, Address> GlobalAddressTable = new Dictionary<string, Address>()
        {
            {"ScriptContext", new Address(0x188B89C, CallingConventions.Cdecl, false, false)},
            {"GetField", new Address(0x4BCFA0, CallingConventions.Cdecl)},
            {"PushString", new Address(0x4BE580, CallingConventions.Fastcall, false)},
            {"Call", new Address(0x4BC9E0, CallingConventions.Cdecl)}
        };
    }
}
