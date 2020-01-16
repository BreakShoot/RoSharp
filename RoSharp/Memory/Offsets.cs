using System.Collections.Generic;
using Binarysharp.MemoryManagement.Assembly.CallingConvention;

namespace RoSharp.Memory
{
    static class Offsets
    {
        public static Dictionary<string, Address> GlobalAddressTable = new Dictionary<string, Address>()
        {
            {"ScriptContext", new Address(0x22B1E8C, CallingConventions.Cdecl, false, false)},
            {"GetField", new Address(0xEDE9C0, CallingConventions.Cdecl)},
            {"PushString", new Address(0xEDFEB0, CallingConventions.Stdcall, false)},
            {"SetField", new Address(0xEE0980, CallingConventions.Cdecl)},
            {"Call", new Address(0xEDE410, CallingConventions.Cdecl)},
            {"Index2Adr", new Address(0xED6EE0, CallingConventions.Cdecl, false) },
            {"PushNumber", new Address(0xEDFE30, CallingConventions.Cdecl) }
        };
    }
}
