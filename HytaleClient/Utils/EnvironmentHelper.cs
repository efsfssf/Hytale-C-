using System;
using System.IO;
using System.Linq;

namespace HytaleClient.Utils
{
	// Token: 0x020007C0 RID: 1984
	public static class EnvironmentHelper
	{
		// Token: 0x0600336D RID: 13165 RVA: 0x0004F6C4 File Offset: 0x0004D8C4
		public static string ResolvePathExecutable(string exec)
		{
			bool flag = File.Exists(exec);
			string result;
			if (flag)
			{
				result = Path.GetFullPath(exec);
			}
			else
			{
				string environmentVariable = Environment.GetEnvironmentVariable("PATH");
				bool flag2 = string.IsNullOrEmpty(environmentVariable);
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = Enumerable.FirstOrDefault<string>(Enumerable.Select<string, string>(environmentVariable.Split(new char[]
					{
						Path.PathSeparator
					}), (string path) => Path.Combine(path, exec)), new Func<string, bool>(File.Exists));
				}
			}
			return result;
		}
	}
}
