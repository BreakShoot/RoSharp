using System;
using Binarysharp.MemoryManagement.Assembly.CallingConvention;

namespace RoSharp.Memory
{
    class Address
    {
        public IntPtr AddressValue;
        public CallingConventions CallingConvetion { get; private set; }
        public bool RemoveRetcheck { get; private set; }
        public bool IsLua { get; private set; }

        public Address(uint address, CallingConventions callingConvention, bool unprotect = true, bool isLua = true)
        {
            AddressValue = (IntPtr)address;
            CallingConvetion = callingConvention;
            RemoveRetcheck = unprotect;
            IsLua = isLua;
        }
    }
}
