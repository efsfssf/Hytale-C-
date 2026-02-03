using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace HytaleClient.Utils
{
	// Token: 0x020007C7 RID: 1991
	public static class OpenUtils
	{
		// Token: 0x06003394 RID: 13204
		[DllImport("shell32.dll", SetLastError = true)]
		public static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, uint cidl, [MarshalAs(UnmanagedType.LPArray)] [In] IntPtr[] apidl, uint dwFlags);

		// Token: 0x06003395 RID: 13205
		[DllImport("shell32.dll", SetLastError = true)]
		public static extern void SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] string name, IntPtr bindingContext, out IntPtr pidl, uint sfgaoIn, out uint psfgaoOut);

		// Token: 0x06003396 RID: 13206 RVA: 0x000518E4 File Offset: 0x0004FAE4
		private static bool TryRevealPathInDirectory_Windows(string filePath)
		{
			string directoryName = Path.GetDirectoryName(filePath);
			IntPtr intPtr;
			uint num;
			OpenUtils.SHParseDisplayName(directoryName, IntPtr.Zero, out intPtr, 0U, out num);
			bool flag = intPtr == IntPtr.Zero;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				IntPtr intPtr2;
				OpenUtils.SHParseDisplayName(filePath, IntPtr.Zero, out intPtr2, 0U, out num);
				bool flag2 = intPtr2 == IntPtr.Zero;
				IntPtr[] array;
				if (flag2)
				{
					array = new IntPtr[0];
				}
				else
				{
					array = new IntPtr[]
					{
						intPtr2
					};
				}
				OpenUtils.SHOpenFolderAndSelectItems(intPtr, (uint)array.Length, array, 0U);
				Marshal.FreeCoTaskMem(intPtr);
				bool flag3 = intPtr2 != IntPtr.Zero;
				if (flag3)
				{
					Marshal.FreeCoTaskMem(intPtr2);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x00051994 File Offset: 0x0004FB94
		private static bool TryRevealPathInDirectory_Mac(string filePath)
		{
			Process.Start("open", "--reveal " + filePath);
			return true;
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x000519C0 File Offset: 0x0004FBC0
		private static bool TryRevealPathInDirectory_Linux(string filePath)
		{
			Process.Start("nautilus", "--select " + filePath);
			return true;
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x000519EC File Offset: 0x0004FBEC
		public static bool TryOpenFileInContainingDirectory(string filePath, string rootPathForValidation)
		{
			filePath = Path.GetFullPath(filePath);
			rootPathForValidation = Path.GetFullPath(rootPathForValidation);
			bool flag = !File.Exists(filePath);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !filePath.StartsWith(rootPathForValidation + Path.DirectorySeparatorChar.ToString());
				if (flag2)
				{
					result = false;
				}
				else
				{
					switch (BuildInfo.Platform)
					{
					case Platform.Windows:
						result = OpenUtils.TryRevealPathInDirectory_Windows(filePath);
						break;
					case Platform.MacOS:
						result = OpenUtils.TryRevealPathInDirectory_Mac(filePath);
						break;
					case Platform.Linux:
						result = OpenUtils.TryRevealPathInDirectory_Linux(filePath);
						break;
					default:
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x00051A80 File Offset: 0x0004FC80
		public static bool TryOpenDirectoryInContainingDirectory(string filePath, string rootPathForValidation)
		{
			filePath = Path.GetFullPath(filePath);
			rootPathForValidation = Path.GetFullPath(rootPathForValidation);
			bool flag = !filePath.EndsWith(Path.DirectorySeparatorChar.ToString());
			if (flag)
			{
				filePath += Path.DirectorySeparatorChar.ToString();
			}
			bool flag2 = !Directory.Exists(filePath);
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool flag3 = !filePath.StartsWith(rootPathForValidation + Path.DirectorySeparatorChar.ToString());
				if (flag3)
				{
					result = false;
				}
				else
				{
					switch (BuildInfo.Platform)
					{
					case Platform.Windows:
						result = OpenUtils.TryRevealPathInDirectory_Windows(filePath);
						break;
					case Platform.MacOS:
						result = OpenUtils.TryRevealPathInDirectory_Mac(filePath);
						break;
					case Platform.Linux:
						result = OpenUtils.TryRevealPathInDirectory_Linux(filePath);
						break;
					default:
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x00051B44 File Offset: 0x0004FD44
		public static void OpenTrustedUrlInDefaultBrowser(string url)
		{
			switch (BuildInfo.Platform)
			{
			case Platform.Windows:
				Process.Start(new ProcessStartInfo
				{
					FileName = url,
					UseShellExecute = true
				});
				break;
			case Platform.MacOS:
				Process.Start("open", url);
				break;
			case Platform.Linux:
				Process.Start("xdg-open", url);
				break;
			}
		}
	}
}
