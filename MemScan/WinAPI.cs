using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WinAPI
{
	class WinAPI
	{
		#region Constants
		public const int MEM_COMMIT = 0x00001000;
		public const int PAGE_READWRITE = 0x04;
		public const int WM_CLIPBOARDUPDATE = 0x031D;
		public static IntPtr HWND_MESSAGE = new IntPtr(-3);
		#endregion

		#region Imports
		[DllImport("kernel32.dll", SetLastError = false)]
		public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION64 lpBuffer, uint dwLength);

		//32 bit
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadProcessMemory(IntPtr hProcess, long lpBaseAddress, byte[] lpBuffer, long dwSize, ref int lpNumberOfBytesRead);

		[DllImport("kernel32.dll")]
		public static extern uint GetLastError();

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AddClipboardFormatListener(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();


		/**
				[DllImport("dbghelp.dll")]
				public static extern bool MiniDumpWriteDump(IntPtr hProcess, uint ProcessId, IntPtr hFile, MINI_DUMP_TYPE DumpType, [In] IntPtr ExceptionParam , [In] IntPtr UserStreamParam, [In] IntPtr CallbackParam);

				[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
				public static extern IntPtr CreateFileW(
					[MarshalAs(UnmanagedType.LPWStr)] string filename,
					[MarshalAs(UnmanagedType.U4)] FileAccess access,
					[MarshalAs(UnmanagedType.U4)] FileShare share,
					IntPtr securityAttributes,
					[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
					[MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
					IntPtr templateFile);

				[DllImport("kernel32.dll", SetLastError = true)]
				[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Bool)]
				public static extern bool CloseHandle(IntPtr hObject);
		**/

		/**
				[DllImport("clrdump.dll", CharSet = CharSet.Unicode, SetLastError = true)]
				public static extern Int32 CreateDump(Int32 ProcessId, string FileName,
				Int32 DumpType, Int32 ExcThreadId, IntPtr ExtPtrs);
		**/

		#endregion

		#region Enums
		/**
	public enum MINI_DUMP_TYPE : int
	{
		// From dbghelp.h:
		Normal = 0x00000000,
		WithDataSegs = 0x00000001,
		WithFullMemory = 2,
		WithHandleData = 0x00000004,
		FilterMemory = 0x00000008,
		ScanMemory = 0x00000010,
		WithUnloadedModules = 0x00000020,
		WithIndirectlyReferencedMemory = 0x00000040,
		FilterModulePaths = 0x00000080,
		WithProcessThreadData = 0x00000100,
		WithPrivateReadWriteMemory = 0x00000200,
		WithoutOptionalData = 0x00000400,
		WithFullMemoryInfo = 0x00000800,
		WithThreadInfo = 0x00001000,
		WithCodeSegs = 0x00002000,
		WithoutAuxiliaryState = 0x00004000,
		WithFullAuxiliaryState = 0x00008000,
		WithPrivateWriteCopyMemory = 0x00010000,
		IgnoreInaccessibleMemory = 0x00020000,
		[SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Justification = "")]
		ValidTypeFlags = 0x0003ffff,
	};
		**/
		[Flags]
		public enum ProcessAccessFlags : uint
		{
			All = 0x001F0FFF,
			Terminate = 0x00000001,
			CreateThread = 0x00000002,
			VirtualMemoryOperation = 0x00000008,
			VirtualMemoryRead = 0x00000010,
			VirtualMemoryWrite = 0x00000020,
			DuplicateHandle = 0x00000040,
			CreateProcess = 0x000000080,
			SetQuota = 0x00000100,
			SetInformation = 0x00000200,
			QueryInformation = 0x00000400,
			QueryLimitedInformation = 0x00001000,
			Synchronize = 0x00100000
		}

		public enum AllocationProtectEnum : uint
		{
			PAGE_EXECUTE = 0x00000010,
			PAGE_EXECUTE_READ = 0x00000020,
			PAGE_EXECUTE_READWRITE = 0x00000040,
			PAGE_EXECUTE_WRITECOPY = 0x00000080,
			PAGE_NOACCESS = 0x00000001,
			PAGE_READONLY = 0x00000002,
			PAGE_READWRITE = 0x00000004,
			PAGE_WRITECOPY = 0x00000008,
			PAGE_GUARD = 0x00000100,
			PAGE_NOCACHE = 0x00000200,
			PAGE_WRITECOMBINE = 0x00000400
		}

		public enum StateEnum : uint
		{
			MEM_COMMIT = 0x1000,
			MEM_FREE = 0x10000,
			MEM_RESERVE = 0x2000
		}

		public enum TypeEnum : uint
		{
			MEM_IMAGE = 0x1000000,
			MEM_MAPPED = 0x40000,
			MEM_PRIVATE = 0x20000
		}

		public enum ProcessorArchitecture
		{
			X86 = 0,
			X64 = 9,
			@Arm = -1,
			Itanium = 6,
			Unknown = 0xFFFF,
		}

		#endregion


		#region structs
		/**
				[StructLayout(LayoutKind.Sequential, Pack = 4)]
				public struct MINIDUMP_EXCEPTION_INFORMATION
				{
					public uint ThreadId;
					public IntPtr ExceptionPointers;
					public int ClientPointers;
				}
			**/
		//For if I add 32 bit compatibility
		[StructLayout(LayoutKind.Sequential)]
		public struct MEMORY_BASIC_INFORMATION
		{
			public IntPtr BaseAddress;
			public IntPtr AllocationBase;
			public uint AllocationProtect;
			public IntPtr RegionSize;
			public uint State;
			public uint Protect;
			public uint Type;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct MEMORY_BASIC_INFORMATION64
		{
			public long BaseAddress;
			public long AllocationBase;
			public int AllocationProtect;
			public int __alignment1;
			public long RegionSize;
			public int State;
			public int Protect;
			public int Type;
			public int __alignment2;
		}



		public struct SYSTEM_INFO
		{
			public ushort processorArchitecture;
			ushort reserved;
			public uint pageSize;
			public IntPtr minimumApplicationAddress;
			public IntPtr maximumApplicationAddress;
			public IntPtr activeProcessorMask;
			public uint numberOfProcessors;
			public uint processorType;
			public uint allocationGranularity;
			public ushort processorLevel;
			public ushort processorRevision;
		}


		#endregion


	}
}
