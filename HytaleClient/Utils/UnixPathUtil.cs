using System;
using System.IO;

namespace HytaleClient.Utils
{
	// Token: 0x020007D5 RID: 2005
	public static class UnixPathUtil
	{
		// Token: 0x06003448 RID: 13384 RVA: 0x00054168 File Offset: 0x00052368
		public static int IndexOfExtension(string path)
		{
			bool flag = path == null;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int num = path.LastIndexOf('.');
				int num2 = path.LastIndexOf('/');
				result = ((num > num2) ? num : -1);
			}
			return result;
		}

		// Token: 0x06003449 RID: 13385 RVA: 0x000541A0 File Offset: 0x000523A0
		public static string GetExtension(string path)
		{
			int num = UnixPathUtil.IndexOfExtension(path);
			bool flag = num > -1 && num < path.Length - 1;
			string result;
			if (flag)
			{
				result = path.Substring(num);
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x0600344A RID: 13386 RVA: 0x000541E0 File Offset: 0x000523E0
		public static string GetFileNameWithoutExtension(string path)
		{
			int num = UnixPathUtil.IndexOfExtension(path);
			int num2 = path.LastIndexOf('/');
			bool flag = num > -1 && num < path.Length - 1;
			string result;
			if (flag)
			{
				num2 = ((num2 < 0) ? 0 : (num2 + 1));
				result = path.Substring(num2, num - num2);
			}
			else
			{
				result = ((num2 > -1) ? path.Substring(num2 + 1) : path);
			}
			return result;
		}

		// Token: 0x0600344B RID: 13387 RVA: 0x00054244 File Offset: 0x00052444
		public static string GetFileName(string path)
		{
			int num = path.LastIndexOf('/');
			return (num > -1) ? path.Substring(num + 1) : path;
		}

		// Token: 0x0600344C RID: 13388 RVA: 0x00054270 File Offset: 0x00052470
		public static string ConvertToUnixPath(string path)
		{
			bool flag = Path.DirectorySeparatorChar == '/';
			string result;
			if (flag)
			{
				result = path;
			}
			else
			{
				result = path.Replace(Path.DirectorySeparatorChar, '/');
			}
			return result;
		}
	}
}
