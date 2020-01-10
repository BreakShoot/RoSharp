using Binarysharp.MemoryManagement.Assembly.CallingConvention;

namespace RoSharp.Memory
{
    static class Offsets
    {
        public static Address scriptcontext_address = new Address(0x188B89C, CallingConventions.Cdecl, false, false);
        public static Address lua_getfield_address = new Address(0x4BCFA0, CallingConventions.Cdecl);
        public static Address lua_pushstring_address = new Address(0x4BE580, CallingConventions.Fastcall, false);
        public static Address lua_call_address = new Address(0x4BC9E0, CallingConventions.Cdecl);
    }
}
