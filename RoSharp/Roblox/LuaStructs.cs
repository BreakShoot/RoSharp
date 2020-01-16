using System;
using System.Runtime.InteropServices;

namespace RoSharp.Roblox
{
    public static class LuaStructs
    {
        public enum LuaTypes : int
        {
            LUA_TNIL = 0,
            LUA_TBOOLEAN = 1,
            LUA_TLIGHTUSERDATA = 2,
            LUA_TNUMBER = 3,
            LUA_TSTRING = 4 
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct TValue
        {
            [FieldOffset(0)]
            public IntPtr value_p;
            [FieldOffset(0)]
            public int value_i;
            [FieldOffset(0)]
            public double value_d;
            [FieldOffset(8)]
            public int tt;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct LuaState
        {
            [FieldOffset(12)]
            public IntPtr base_;

            [FieldOffset(32)]
            public IntPtr top;
        }
    }
}
