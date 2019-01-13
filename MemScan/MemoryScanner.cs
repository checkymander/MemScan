using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MemScan
{
	class MemoryScanner
	{
		public static string dumpProcessMemory(Process proc)
		{
			//Dumps Process Memory, Converts it to string array and returns. No writing to file needed.
			StringBuilder sb = new StringBuilder();
			IntPtr hProc = WinAPI.WinAPI.OpenProcess(WinAPI.WinAPI.ProcessAccessFlags.QueryInformation | WinAPI.WinAPI.ProcessAccessFlags.VirtualMemoryRead, false, proc.Id);
			WinAPI.WinAPI.MEMORY_BASIC_INFORMATION64 mbi = new WinAPI.WinAPI.MEMORY_BASIC_INFORMATION64();
			//32 bit
			//WinAPI.WinAPI.MEMORY_BASIC_INFORMATION mbi = new WinAPI.WinAPI.MEMORY_BASIC_INFORMATION()
			WinAPI.WinAPI.SYSTEM_INFO si = new WinAPI.WinAPI.SYSTEM_INFO();
			if (hProc == IntPtr.Zero)
			{
				//Failed.
				//Console.WriteLine("Unable to create a connection to the process! Error Code: {0}", WinAPI.WinAPI.GetLastError());
				//Environment.Exit(6);
				return null;
			}
			WinAPI.WinAPI.GetSystemInfo(out si);
			IntPtr hProc_min_addr = si.minimumApplicationAddress;
			IntPtr hProc_max_addr = si.maximumApplicationAddress;
			long hProc_long_min = (long)hProc_min_addr;
			long hProc_long_max = (long)hProc_max_addr;
			//string fileName = "dump-" + proc.Id + "-" + proc.ProcessName + "-2.txt";
			//StreamWriter sw = new StreamWriter(fileName);

			int bytesRead = 0;

			while (hProc_long_min < hProc_long_max)
			{
				bytesRead = WinAPI.WinAPI.VirtualQueryEx(hProc, hProc_min_addr, out mbi, (uint)Marshal.SizeOf(typeof(WinAPI.WinAPI.MEMORY_BASIC_INFORMATION64)));
				if (mbi.Protect == WinAPI.WinAPI.PAGE_READWRITE && mbi.State == WinAPI.WinAPI.MEM_COMMIT)
				{
					byte[] buffer = new byte[mbi.RegionSize];
					WinAPI.WinAPI.ReadProcessMemory(hProc, mbi.BaseAddress, buffer, mbi.RegionSize, ref bytesRead);
					for (long i = 0; i < mbi.RegionSize; i++)
					{
						//sw.WriteLine("0x{0} : {1}", mbi.BaseAddress + i.ToString("X"), (char)buffer[i]);
						//sw.Write((char)buffer[i]);
						sb.Append((char)buffer[i]);
					}
				}
				hProc_long_min += mbi.RegionSize;
				hProc_min_addr = new IntPtr(hProc_long_min);
			}
			//sw.Close();

			return sb.ToString();
		}
	}
}
