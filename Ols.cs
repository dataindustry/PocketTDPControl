using System;
using System.Runtime.InteropServices;

namespace PocketTDPControl
{
    public class Ols : IDisposable
    {
        private const string dllNameX64 = "WinRing0x64.dll";
        private IntPtr module = IntPtr.Zero;
        private uint status;
        public Ols._GetDllStatus GetDllStatus;
        public Ols._GetDriverType GetDriverType;
        public Ols._GetDllVersion GetDllVersion;
        public Ols._GetDriverVersion GetDriverVersion;
        public Ols._InitializeOls InitializeOls;
        public Ols._DeinitializeOls DeinitializeOls;
        public Ols._IsCpuid IsCpuid;
        public Ols._IsMsr IsMsr;
        public Ols._IsTsc IsTsc;
        public Ols._Hlt Hlt;
        public Ols._HltTx HltTx;
        public Ols._HltPx HltPx;
        public Ols._Rdmsr Rdmsr;
        public Ols._RdmsrTx RdmsrTx;
        public Ols._RdmsrPx RdmsrPx;
        public Ols._Wrmsr Wrmsr;
        public Ols._WrmsrTx WrmsrTx;
        public Ols._WrmsrPx WrmsrPx;
        public Ols._Rdpmc Rdpmc;
        public Ols._RdpmcTx RdpmcTx;
        public Ols._RdpmcPx RdpmcPx;
        public Ols._Cpuid Cpuid;
        public Ols._CpuidTx CpuidTx;
        public Ols._CpuidPx CpuidPx;
        public Ols._Rdtsc Rdtsc;
        public Ols._RdtscTx RdtscTx;
        public Ols._RdtscPx RdtscPx;
        public Ols._ReadIoPortByte ReadIoPortByte;
        public Ols._ReadIoPortWord ReadIoPortWord;
        public Ols._ReadIoPortDword ReadIoPortDword;
        public Ols._ReadIoPortByteEx ReadIoPortByteEx;
        public Ols._ReadIoPortWordEx ReadIoPortWordEx;
        public Ols._ReadIoPortDwordEx ReadIoPortDwordEx;
        public Ols._WriteIoPortByte WriteIoPortByte;
        public Ols._WriteIoPortWord WriteIoPortWord;
        public Ols._WriteIoPortDword WriteIoPortDword;
        public Ols._WriteIoPortByteEx WriteIoPortByteEx;
        public Ols._WriteIoPortWordEx WriteIoPortWordEx;
        public Ols._WriteIoPortDwordEx WriteIoPortDwordEx;
        public Ols._SetPciMaxBusIndex SetPciMaxBusIndex;
        public Ols._ReadPciConfigByte ReadPciConfigByte;
        public Ols._ReadPciConfigWord ReadPciConfigWord;
        public Ols._ReadPciConfigDword ReadPciConfigDword;
        public Ols._ReadPciConfigByteEx ReadPciConfigByteEx;
        public Ols._ReadPciConfigWordEx ReadPciConfigWordEx;
        public Ols._ReadPciConfigDwordEx ReadPciConfigDwordEx;
        public Ols._WritePciConfigByte WritePciConfigByte;
        public Ols._WritePciConfigWord WritePciConfigWord;
        public Ols._WritePciConfigDword WritePciConfigDword;
        public Ols._WritePciConfigByteEx WritePciConfigByteEx;
        public Ols._WritePciConfigWordEx WritePciConfigWordEx;
        public Ols._WritePciConfigDwordEx WritePciConfigDwordEx;
        public Ols._FindPciDeviceById FindPciDeviceById;
        public Ols._FindPciDeviceByClass FindPciDeviceByClass;

        public uint PciBusDevFunc(uint bus, uint dev, uint func) => (uint)(((int)bus & (int)byte.MaxValue) << 8 | ((int)dev & 31) << 3 | (int)func & 7);

        public uint PciGetBus(uint address) => address >> 8 & (uint)byte.MaxValue;

        public uint PciGetDev(uint address) => address >> 3 & 31U;

        public uint PciGetFunc(uint address) => address & 7U;

        [DllImport("kernel32")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        public Ols()
        {
            if (IntPtr.Size == 8)
            {
                this.module = Ols.LoadLibrary(dllNameX64);
                if (this.module == IntPtr.Zero)
                {
                    this.status = 1U;
                }
                else
                {
                    this.GetDllStatus = (Ols._GetDllStatus)this.GetDelegate(nameof(GetDllStatus), typeof(Ols._GetDllStatus));
                    this.GetDllVersion = (Ols._GetDllVersion)this.GetDelegate(nameof(GetDllVersion), typeof(Ols._GetDllVersion));
                    this.GetDriverVersion = (Ols._GetDriverVersion)this.GetDelegate(nameof(GetDriverVersion), typeof(Ols._GetDriverVersion));
                    this.GetDriverType = (Ols._GetDriverType)this.GetDelegate(nameof(GetDriverType), typeof(Ols._GetDriverType));
                    this.InitializeOls = (Ols._InitializeOls)this.GetDelegate(nameof(InitializeOls), typeof(Ols._InitializeOls));
                    this.DeinitializeOls = (Ols._DeinitializeOls)this.GetDelegate(nameof(DeinitializeOls), typeof(Ols._DeinitializeOls));
                    this.IsCpuid = (Ols._IsCpuid)this.GetDelegate(nameof(IsCpuid), typeof(Ols._IsCpuid));
                    this.IsMsr = (Ols._IsMsr)this.GetDelegate(nameof(IsMsr), typeof(Ols._IsMsr));
                    this.IsTsc = (Ols._IsTsc)this.GetDelegate(nameof(IsTsc), typeof(Ols._IsTsc));
                    this.Hlt = (Ols._Hlt)this.GetDelegate(nameof(Hlt), typeof(Ols._Hlt));
                    this.HltTx = (Ols._HltTx)this.GetDelegate(nameof(HltTx), typeof(Ols._HltTx));
                    this.HltPx = (Ols._HltPx)this.GetDelegate(nameof(HltPx), typeof(Ols._HltPx));
                    this.Rdmsr = (Ols._Rdmsr)this.GetDelegate(nameof(Rdmsr), typeof(Ols._Rdmsr));
                    this.RdmsrTx = (Ols._RdmsrTx)this.GetDelegate(nameof(RdmsrTx), typeof(Ols._RdmsrTx));
                    this.RdmsrPx = (Ols._RdmsrPx)this.GetDelegate(nameof(RdmsrPx), typeof(Ols._RdmsrPx));
                    this.Wrmsr = (Ols._Wrmsr)this.GetDelegate(nameof(Wrmsr), typeof(Ols._Wrmsr));
                    this.WrmsrTx = (Ols._WrmsrTx)this.GetDelegate(nameof(WrmsrTx), typeof(Ols._WrmsrTx));
                    this.WrmsrPx = (Ols._WrmsrPx)this.GetDelegate(nameof(WrmsrPx), typeof(Ols._WrmsrPx));
                    this.Rdpmc = (Ols._Rdpmc)this.GetDelegate(nameof(Rdpmc), typeof(Ols._Rdpmc));
                    this.RdpmcTx = (Ols._RdpmcTx)this.GetDelegate(nameof(RdpmcTx), typeof(Ols._RdpmcTx));
                    this.RdpmcPx = (Ols._RdpmcPx)this.GetDelegate(nameof(RdpmcPx), typeof(Ols._RdpmcPx));
                    this.Cpuid = (Ols._Cpuid)this.GetDelegate(nameof(Cpuid), typeof(Ols._Cpuid));
                    this.CpuidTx = (Ols._CpuidTx)this.GetDelegate(nameof(CpuidTx), typeof(Ols._CpuidTx));
                    this.CpuidPx = (Ols._CpuidPx)this.GetDelegate(nameof(CpuidPx), typeof(Ols._CpuidPx));
                    this.Rdtsc = (Ols._Rdtsc)this.GetDelegate(nameof(Rdtsc), typeof(Ols._Rdtsc));
                    this.RdtscTx = (Ols._RdtscTx)this.GetDelegate(nameof(RdtscTx), typeof(Ols._RdtscTx));
                    this.RdtscPx = (Ols._RdtscPx)this.GetDelegate(nameof(RdtscPx), typeof(Ols._RdtscPx));
                    this.ReadIoPortByte = (Ols._ReadIoPortByte)this.GetDelegate(nameof(ReadIoPortByte), typeof(Ols._ReadIoPortByte));
                    this.ReadIoPortWord = (Ols._ReadIoPortWord)this.GetDelegate(nameof(ReadIoPortWord), typeof(Ols._ReadIoPortWord));
                    this.ReadIoPortDword = (Ols._ReadIoPortDword)this.GetDelegate(nameof(ReadIoPortDword), typeof(Ols._ReadIoPortDword));
                    this.ReadIoPortByteEx = (Ols._ReadIoPortByteEx)this.GetDelegate(nameof(ReadIoPortByteEx), typeof(Ols._ReadIoPortByteEx));
                    this.ReadIoPortWordEx = (Ols._ReadIoPortWordEx)this.GetDelegate(nameof(ReadIoPortWordEx), typeof(Ols._ReadIoPortWordEx));
                    this.ReadIoPortDwordEx = (Ols._ReadIoPortDwordEx)this.GetDelegate(nameof(ReadIoPortDwordEx), typeof(Ols._ReadIoPortDwordEx));
                    this.WriteIoPortByte = (Ols._WriteIoPortByte)this.GetDelegate(nameof(WriteIoPortByte), typeof(Ols._WriteIoPortByte));
                    this.WriteIoPortWord = (Ols._WriteIoPortWord)this.GetDelegate(nameof(WriteIoPortWord), typeof(Ols._WriteIoPortWord));
                    this.WriteIoPortDword = (Ols._WriteIoPortDword)this.GetDelegate(nameof(WriteIoPortDword), typeof(Ols._WriteIoPortDword));
                    this.WriteIoPortByteEx = (Ols._WriteIoPortByteEx)this.GetDelegate(nameof(WriteIoPortByteEx), typeof(Ols._WriteIoPortByteEx));
                    this.WriteIoPortWordEx = (Ols._WriteIoPortWordEx)this.GetDelegate(nameof(WriteIoPortWordEx), typeof(Ols._WriteIoPortWordEx));
                    this.WriteIoPortDwordEx = (Ols._WriteIoPortDwordEx)this.GetDelegate(nameof(WriteIoPortDwordEx), typeof(Ols._WriteIoPortDwordEx));
                    this.SetPciMaxBusIndex = (Ols._SetPciMaxBusIndex)this.GetDelegate(nameof(SetPciMaxBusIndex), typeof(Ols._SetPciMaxBusIndex));
                    this.ReadPciConfigByte = (Ols._ReadPciConfigByte)this.GetDelegate(nameof(ReadPciConfigByte), typeof(Ols._ReadPciConfigByte));
                    this.ReadPciConfigWord = (Ols._ReadPciConfigWord)this.GetDelegate(nameof(ReadPciConfigWord), typeof(Ols._ReadPciConfigWord));
                    this.ReadPciConfigDword = (Ols._ReadPciConfigDword)this.GetDelegate(nameof(ReadPciConfigDword), typeof(Ols._ReadPciConfigDword));
                    this.ReadPciConfigByteEx = (Ols._ReadPciConfigByteEx)this.GetDelegate(nameof(ReadPciConfigByteEx), typeof(Ols._ReadPciConfigByteEx));
                    this.ReadPciConfigWordEx = (Ols._ReadPciConfigWordEx)this.GetDelegate(nameof(ReadPciConfigWordEx), typeof(Ols._ReadPciConfigWordEx));
                    this.ReadPciConfigDwordEx = (Ols._ReadPciConfigDwordEx)this.GetDelegate(nameof(ReadPciConfigDwordEx), typeof(Ols._ReadPciConfigDwordEx));
                    this.WritePciConfigByte = (Ols._WritePciConfigByte)this.GetDelegate(nameof(WritePciConfigByte), typeof(Ols._WritePciConfigByte));
                    this.WritePciConfigWord = (Ols._WritePciConfigWord)this.GetDelegate(nameof(WritePciConfigWord), typeof(Ols._WritePciConfigWord));
                    this.WritePciConfigDword = (Ols._WritePciConfigDword)this.GetDelegate(nameof(WritePciConfigDword), typeof(Ols._WritePciConfigDword));
                    this.WritePciConfigByteEx = (Ols._WritePciConfigByteEx)this.GetDelegate(nameof(WritePciConfigByteEx), typeof(Ols._WritePciConfigByteEx));
                    this.WritePciConfigWordEx = (Ols._WritePciConfigWordEx)this.GetDelegate(nameof(WritePciConfigWordEx), typeof(Ols._WritePciConfigWordEx));
                    this.WritePciConfigDwordEx = (Ols._WritePciConfigDwordEx)this.GetDelegate(nameof(WritePciConfigDwordEx), typeof(Ols._WritePciConfigDwordEx));
                    this.FindPciDeviceById = (Ols._FindPciDeviceById)this.GetDelegate(nameof(FindPciDeviceById), typeof(Ols._FindPciDeviceById));
                    this.FindPciDeviceByClass = (Ols._FindPciDeviceByClass)this.GetDelegate(nameof(FindPciDeviceByClass), typeof(Ols._FindPciDeviceByClass));
                    if (this.GetDllStatus == null || this.GetDllVersion == null || this.GetDriverVersion == null || this.GetDriverType == null || this.InitializeOls == null || this.DeinitializeOls == null || this.IsCpuid == null || this.IsMsr == null || this.IsTsc == null || this.Hlt == null || this.HltTx == null || this.HltPx == null || this.Rdmsr == null || this.RdmsrTx == null || this.RdmsrPx == null || this.Wrmsr == null || this.WrmsrTx == null || this.WrmsrPx == null || this.Rdpmc == null || this.RdpmcTx == null || this.RdpmcPx == null || this.Cpuid == null || this.CpuidTx == null || this.CpuidPx == null || this.Rdtsc == null || this.RdtscTx == null || this.RdtscPx == null || this.ReadIoPortByte == null || this.ReadIoPortWord == null || this.ReadIoPortDword == null || this.ReadIoPortByteEx == null || this.ReadIoPortWordEx == null || this.ReadIoPortDwordEx == null || this.WriteIoPortByte == null || this.WriteIoPortWord == null || this.WriteIoPortDword == null || this.WriteIoPortByteEx == null || this.WriteIoPortWordEx == null || this.WriteIoPortDwordEx == null || this.SetPciMaxBusIndex == null || this.ReadPciConfigByte == null || this.ReadPciConfigWord == null || this.ReadPciConfigDword == null || this.ReadPciConfigByteEx == null || this.ReadPciConfigWordEx == null || this.ReadPciConfigDwordEx == null || this.WritePciConfigByte == null || this.WritePciConfigWord == null || this.WritePciConfigDword == null || this.WritePciConfigByteEx == null || this.WritePciConfigWordEx == null || this.WritePciConfigDwordEx == null || this.FindPciDeviceById == null || this.FindPciDeviceByClass == null)
                        this.status = 2U;
                    if (this.InitializeOls() != 0)
                        return;
                    this.status = 3U;
                }
            }
            else
                this.status = 1U;
        }

        public uint GetStatus() => this.status;

        public void Dispose()
        {
            if (!(this.module != IntPtr.Zero))
                return;
            this.DeinitializeOls();
            Ols.FreeLibrary(this.module);
            this.module = IntPtr.Zero;
        }

        public Delegate GetDelegate(string procName, Type delegateType)
        {
            IntPtr procAddress = Ols.GetProcAddress(this.module, procName);
            return procAddress != IntPtr.Zero ? Marshal.GetDelegateForFunctionPointer(procAddress, delegateType) : throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        public enum Status
        {
            NO_ERROR,
            DLL_NOT_FOUND,
            DLL_INCORRECT_VERSION,
            DLL_INITIALIZE_ERROR,
        }

        public enum OlsDllStatus
        {
            OLS_DLL_NO_ERROR = 0,
            OLS_DLL_UNSUPPORTED_PLATFORM = 1,
            OLS_DLL_DRIVER_NOT_LOADED = 2,
            OLS_DLL_DRIVER_NOT_FOUND = 3,
            OLS_DLL_DRIVER_UNLOADED = 4,
            OLS_DLL_DRIVER_NOT_LOADED_ON_NETWORK = 5,
            OLS_DLL_UNKNOWN_ERROR = 9,
        }

        public enum OlsDriverType
        {
            OLS_DRIVER_TYPE_UNKNOWN,
            OLS_DRIVER_TYPE_WIN_9X,
            OLS_DRIVER_TYPE_WIN_NT,
            OLS_DRIVER_TYPE_WIN_NT4,
            OLS_DRIVER_TYPE_WIN_NT_X64,
            OLS_DRIVER_TYPE_WIN_NT_IA64,
        }

        public enum OlsErrorPci : uint
        {
            OLS_ERROR_PCI_BUS_NOT_EXIST = 3758096385, // 0xE0000001
            OLS_ERROR_PCI_NO_DEVICE = 3758096386, // 0xE0000002
            OLS_ERROR_PCI_WRITE_CONFIG = 3758096387, // 0xE0000003
            OLS_ERROR_PCI_READ_CONFIG = 3758096388, // 0xE0000004
        }

        public delegate uint _GetDllStatus();

        public delegate uint _GetDllVersion(
          ref byte major,
          ref byte minor,
          ref byte revision,
          ref byte release);

        public delegate uint _GetDriverVersion(
          ref byte major,
          ref byte minor,
          ref byte revision,
          ref byte release);

        public delegate uint _GetDriverType();

        public delegate int _InitializeOls();

        public delegate void _DeinitializeOls();

        public delegate int _IsCpuid();

        public delegate int _IsMsr();

        public delegate int _IsTsc();

        public delegate int _Hlt();

        public delegate int _HltTx(UIntPtr threadAffinityMask);

        public delegate int _HltPx(UIntPtr processAffinityMask);

        public delegate int _Rdmsr(uint index, ref uint eax, ref uint edx);

        public delegate int _RdmsrTx(
          uint index,
          ref uint eax,
          ref uint edx,
          UIntPtr threadAffinityMask);

        public delegate int _RdmsrPx(
          uint index,
          ref uint eax,
          ref uint edx,
          UIntPtr processAffinityMask);

        public delegate int _Wrmsr(uint index, uint eax, uint edx);

        public delegate int _WrmsrTx(uint index, uint eax, uint edx, UIntPtr threadAffinityMask);

        public delegate int _WrmsrPx(uint index, uint eax, uint edx, UIntPtr processAffinityMask);

        public delegate int _Rdpmc(uint index, ref uint eax, ref uint edx);

        public delegate int _RdpmcTx(
          uint index,
          ref uint eax,
          ref uint edx,
          UIntPtr threadAffinityMask);

        public delegate int _RdpmcPx(
          uint index,
          ref uint eax,
          ref uint edx,
          UIntPtr processAffinityMask);

        public delegate int _Cpuid(
          uint index,
          ref uint eax,
          ref uint ebx,
          ref uint ecx,
          ref uint edx);

        public delegate int _CpuidTx(
          uint index,
          ref uint eax,
          ref uint ebx,
          ref uint ecx,
          ref uint edx,
          UIntPtr threadAffinityMask);

        public delegate int _CpuidPx(
          uint index,
          ref uint eax,
          ref uint ebx,
          ref uint ecx,
          ref uint edx,
          UIntPtr processAffinityMask);

        public delegate int _Rdtsc(ref uint eax, ref uint edx);

        public delegate int _RdtscTx(ref uint eax, ref uint edx, UIntPtr threadAffinityMask);

        public delegate int _RdtscPx(ref uint eax, ref uint edx, UIntPtr processAffinityMask);

        public delegate byte _ReadIoPortByte(ushort port);

        public delegate ushort _ReadIoPortWord(ushort port);

        public delegate uint _ReadIoPortDword(ushort port);

        public delegate int _ReadIoPortByteEx(ushort port, ref byte value);

        public delegate int _ReadIoPortWordEx(ushort port, ref ushort value);

        public delegate int _ReadIoPortDwordEx(ushort port, ref uint value);

        public delegate void _WriteIoPortByte(ushort port, byte value);

        public delegate void _WriteIoPortWord(ushort port, ushort value);

        public delegate void _WriteIoPortDword(ushort port, uint value);

        public delegate int _WriteIoPortByteEx(ushort port, byte value);

        public delegate int _WriteIoPortWordEx(ushort port, ushort value);

        public delegate int _WriteIoPortDwordEx(ushort port, uint value);

        public delegate void _SetPciMaxBusIndex(byte max);

        public delegate byte _ReadPciConfigByte(uint pciAddress, byte regAddress);

        public delegate ushort _ReadPciConfigWord(uint pciAddress, byte regAddress);

        public delegate uint _ReadPciConfigDword(uint pciAddress, byte regAddress);

        public delegate int _ReadPciConfigByteEx(uint pciAddress, uint regAddress, ref byte value);

        public delegate int _ReadPciConfigWordEx(uint pciAddress, uint regAddress, ref ushort value);

        public delegate int _ReadPciConfigDwordEx(uint pciAddress, uint regAddress, ref uint value);

        public delegate void _WritePciConfigByte(uint pciAddress, byte regAddress, byte value);

        public delegate void _WritePciConfigWord(uint pciAddress, byte regAddress, ushort value);

        public delegate void _WritePciConfigDword(uint pciAddress, byte regAddress, uint value);

        public delegate int _WritePciConfigByteEx(uint pciAddress, uint regAddress, byte value);

        public delegate int _WritePciConfigWordEx(uint pciAddress, uint regAddress, ushort value);

        public delegate int _WritePciConfigDwordEx(uint pciAddress, uint regAddress, uint value);

        public delegate uint _FindPciDeviceById(ushort vendorId, ushort deviceId, byte index);

        public delegate uint _FindPciDeviceByClass(
          byte baseClass,
          byte subClass,
          byte programIf,
          byte index);
    }
    internal class OlsInitFailedEventArgs : EventArgs
    {

    }
}
