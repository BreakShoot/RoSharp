#define DEBUG_ROBLOX

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using KeksV5;
using RoSharp.Memory;
using NativeObjects = Binarysharp.MemoryManagement.Native;

namespace RoSharp.Roblox
{
    class RobloxSharp : MemorySharp
    {
        private readonly IntPtr _referencedBaseAddress;

        public RobloxSharp(Process process, IntPtr baseAddress) : base(process)
        {
            _referencedBaseAddress = baseAddress;
        }

        public RobloxSharp(int processId, IntPtr baseAddress) : base(processId)
        {
            _referencedBaseAddress = baseAddress;
        }

        public int FixAddress(int Address)
            => Address - _referencedBaseAddress.ToInt32() + this.Native.MainModule.BaseAddress.ToInt32();
        public RemotePointer this[int address] => new RemotePointer(this, (IntPtr)FixAddress(address));
        public RemotePointer this[IntPtr address] => new RemotePointer(this, (IntPtr)FixAddress(address.ToInt32()));

        private IntPtr GetScriptContext()
        {
#if DEBUG_ROBLOX
            Logger.Log(Logger.LogType.WORK, "Attempting to scan for ScriptContext...");
#endif

            NativeObjects.MemoryBasicInformation mbi;

            IntPtr baseAdr = this.Native.MainModule.BaseAddress;

            SigScan sigScan = new SigScan()
            {
                Process = this.Native
            };

            for (IntPtr offset = baseAdr; offset.ToInt32() < 0x7FFFFFFF; offset += baseAdr.ToInt32())
            {
                NativeObjects.NativeMethods.VirtualQueryEx(this.Handle, offset, out mbi, Marshal.SizeOf<NativeObjects.MemoryBasicInformation>());

                if (mbi.Protect == NativeObjects.MemoryProtectionFlags.ReadWrite)
                {
                    sigScan.Address = offset;
                    sigScan.Size = mbi.RegionSize;
                    IntPtr result = sigScan.FindPattern(BitConverter.ToString(BitConverter.GetBytes(FixAddress(Offsets.GlobalAddressTable["ScriptContext"].AddressValue.ToInt32()))).Replace("-", " "));
                    sigScan.ResetRegion();
                    if (result != IntPtr.Zero)
                    {
#if DEBUG_ROBLOX
                        Logger.Log(Logger.LogType.SUCCESS, "Successfully scanned for ScriptContext! SC: 0x{0:X}", result.ToInt32());
#endif

                        return result;
                    }
                    
                }

                baseAdr = (IntPtr)mbi.RegionSize;
            }

#if DEBUG_ROBLOX
            Logger.Log(Logger.LogType.ERROR, "Failed to scan for ScriptContext");
#endif
            return IntPtr.Zero;
        }

        private int GetFunctionSize(IntPtr address)
        {
            IntPtr offset = address;
            byte[] epilogueBytes = {0x55, 0x8B, 0xEC};

            do
            {
                offset += 16;
            } while (!this.ReadBytes(offset, 3, false).SequenceEqual(epilogueBytes));

            return offset.ToInt32() - address.ToInt32();
        }

        private IntPtr RemoveReturnCheck(IntPtr address)
        {
            address = (IntPtr)FixAddress(address.ToInt32());
            int allocationSize = GetFunctionSize(address);

            IntPtr allocatedMemory = NativeObjects.NativeMethods.VirtualAllocEx(this.Handle, IntPtr.Zero, allocationSize,
                NativeObjects.MemoryAllocationFlags.Commit | NativeObjects.MemoryAllocationFlags.Reserve,
                NativeObjects.MemoryProtectionFlags.ExecuteReadWrite);

          

            this[allocatedMemory, false].Write(this.ReadBytes(address, allocationSize, false));


            IntPtr position = allocatedMemory;

            bool valid = false;

            do
            {
                if (this[position, false].Read<byte>() == 0x72 && this[position + 2, false].Read<byte>() == 0xA1 && this[position + 7, false].Read<byte>() == 0x8B)
                {
                    this[position, false].Write<byte>(0xEB);

                    IntPtr callByte = allocatedMemory;

                    do
                    {
                        if (this[callByte, false].Read<byte>() == 0xE8)
                        {
                            
                            IntPtr oFuncPos = address + (callByte - allocatedMemory.ToInt32()).ToInt32();
                            
                            IntPtr oFuncAddr = (oFuncPos + this[(oFuncPos + 1), false].Read<int>()) + 5;

                            if (oFuncAddr.ToInt32() % 16 == 0)
                            {
                                IntPtr relativeAddr = oFuncAddr - callByte.ToInt32() - 5;
                                this[(callByte + 1), false].Write(relativeAddr.ToInt32());
                                callByte += 4;
                            }
                        }

                        callByte = IntPtr.Add(callByte, 1);
                    } while (callByte.ToInt32() - allocatedMemory.ToInt32() < allocationSize);

                    valid = true;
                }

                position = IntPtr.Add(position, 1);
            } while (position.ToInt32() < allocatedMemory.ToInt32() + allocationSize);


            if (!valid)
            {
                NativeObjects.NativeMethods.VirtualFreeEx(this.Handle, allocatedMemory, 0, NativeObjects.MemoryReleaseFlags.Release);
                return allocatedMemory;
            }

            return allocatedMemory;
        }

        public IntPtr GetLuaState()
        {
#if DEBUG_ROBLOX
            Logger.Log(Logger.LogType.WORK, "Attempting to grab LuaState...");
#endif
            IntPtr scriptContextPtr = GetScriptContext();


            if (scriptContextPtr != IntPtr.Zero)
            {
                IntPtr LuaState = (IntPtr)(IntPtr.Add(scriptContextPtr, 172).ToInt32() ^ this[scriptContextPtr + 172, false].Read<int>());
#if DEBUG_ROBLOX
                Logger.Log(Logger.LogType.SUCCESS, "Successfully grabbed LuaState! LS: 0x{0:X}", LuaState.ToInt32());
#endif
                return LuaState;
            }
            else
            {
#if DEBUG_ROBLOX
                Logger.Log(Logger.LogType.ERROR, "Failed to grab LuaState");
#endif
                return (IntPtr)0;
            }            
        }

        public void CleanAddresses()
        {
#if DEBUG_ROBLOX
            Logger.Log(Logger.LogType.WORK, "Cleaning addresses! This may take a while!");
#endif

            foreach (Address address in Offsets.GlobalAddressTable.Values)
            {
                if (address.RemoveRetcheck)
                    address.AddressValue = this.RemoveReturnCheck(address.AddressValue);
            }

#if DEBUG_ROBLOX
            Logger.Log(Logger.LogType.WORK, "Finished cleaning addresses!");
#endif
        }
    }
}
