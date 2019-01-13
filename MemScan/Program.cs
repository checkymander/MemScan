 using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace MemScan
{
	class Program
	{
		static void Main(string[] args)
		{
			bool verbose = true;
			string file = "results.txt";
			if (!File.Exists(file))
			{
				using (StreamWriter sw = File.CreateText(file))
				{
					sw.WriteLine("Output");
				}
			}
			string searchString = args[0];
			string withSpaces = String.Join(" ", searchString.ToCharArray());
			string b64ss = System.Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(searchString));
			string b64spaces = String.Join(" ", b64ss.ToCharArray());
			Console.WriteLine(withSpaces);
			Console.ReadKey();
			Dictionary<string, string> searchKeys = new Dictionary<string, string>()
			{
				{"Search String",searchString },{"Base64 Encoded Search String",b64ss},{"Search String2",withSpaces},{"Base64 Search String2",b64spaces}
			};
			int i = 0;
			Process[] procs = Process.GetProcesses();
			if (verbose)
			{
				Console.WriteLine("[DEBUG] Number of Processes Found: {0}", procs.Length);
				Console.ReadKey();
			}
			foreach (var proc in procs)
			{
				i++;
				try
				{
					string strResult = MemoryScanner.dumpProcessMemory(proc).Replace("\0","").Replace("\n","").Replace("\r","");


					foreach (KeyValuePair<string, string> kvp in searchKeys)
					{
						if (strResult.Contains(kvp.Value))
						{
							using (StreamWriter sw = File.AppendText(file))
							{
								string outLine = String.Format("[{0}]: Found {0} in {1} ProcID: {2}", kvp.Key, proc.ProcessName, proc.Id);
								Console.WriteLine(outLine);
								sw.WriteLine(outLine);

							}
						}
						else
						{
							if(verbose)
								Console.WriteLine("No String found in {0}:{1}", proc.ProcessName, proc.Id);
						}
					}
				}
				catch (Exception e)
				{
					if(verbose)
						Console.WriteLine("[ERROR] {0} - {1}", e.Message, proc.ProcessName);
				}

			}
			Console.WriteLine("[Finished]: {0}", i);
			Console.ReadKey();
		}
	}
}
