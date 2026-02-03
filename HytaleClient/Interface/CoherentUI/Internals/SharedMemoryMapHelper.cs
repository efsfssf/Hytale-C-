using System;
using System.Runtime.InteropServices;

namespace HytaleClient.Interface.CoherentUI.Internals
{
	// Token: 0x020008E2 RID: 2274
	internal static class SharedMemoryMapHelper
	{
		// Token: 0x0600425A RID: 16986
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, SharedMemoryMapHelper.WindowsFileMapAccessType dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

		// Token: 0x0600425B RID: 16987
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool UnmapViewOfFile(IntPtr hFileMappingObject);

		// Token: 0x0600425C RID: 16988 RVA: 0x000C8B4C File Offset: 0x000C6D4C
		public static IntPtr DoMapSharedMemoryWindows(int handleValue, int sizeInBytes)
		{
			return SharedMemoryMapHelper.MapViewOfFile((IntPtr)handleValue, SharedMemoryMapHelper.WindowsFileMapAccessType.Read, 0U, 0U, (UIntPtr)((ulong)((long)sizeInBytes)));
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x000C8B73 File Offset: 0x000C6D73
		public static void FreeMapSharedMemoryWindows(IntPtr addr)
		{
			SharedMemoryMapHelper.UnmapViewOfFile(addr);
		}

		// Token: 0x0600425E RID: 16990
		[DllImport("/usr/lib/system/libsystem_kernel.dylib")]
		private static extern IntPtr mmap(IntPtr addr, int length, int prot, int flags, int fd, ulong offset);

		// Token: 0x0600425F RID: 16991
		[DllImport("/usr/lib/system/libsystem_kernel.dylib")]
		private static extern void munmap(IntPtr addr, int length);

		// Token: 0x06004260 RID: 16992 RVA: 0x000C8B80 File Offset: 0x000C6D80
		public static IntPtr DoMapSharedMemoryMacOS(int handleValue, int sizeInBytes)
		{
			IntPtr intPtr = SharedMemoryMapHelper.mmap(IntPtr.Zero, sizeInBytes, 3, 1, handleValue, 0UL);
			return ((int)intPtr == -1) ? IntPtr.Zero : intPtr;
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x000C8BB4 File Offset: 0x000C6DB4
		public static void FreeMapSharedMemoryMacOS(IntPtr addr, int length)
		{
			SharedMemoryMapHelper.munmap(addr, length);
		}

		// Token: 0x06004262 RID: 16994
		[DllImport("libc.so.6")]
		private static extern IntPtr shmat(int shmid, IntPtr shmaddr, int shmflg);

		// Token: 0x06004263 RID: 16995
		[DllImport("libc.so.6")]
		private static extern int shmdt(IntPtr pointer);

		// Token: 0x06004264 RID: 16996 RVA: 0x000C8BC0 File Offset: 0x000C6DC0
		public static IntPtr DoMapSharedMemoryLinux(int handleValue)
		{
			return SharedMemoryMapHelper.shmat(handleValue, IntPtr.Zero, SharedMemoryMapHelper.SHM_RDONLY);
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x000C8BE2 File Offset: 0x000C6DE2
		public static void FreeMapSharedMemoryLinux(IntPtr addr)
		{
			SharedMemoryMapHelper.shmdt(addr);
		}

		// Token: 0x04002070 RID: 8304
		private static int SHM_RDONLY = 10000;

		// Token: 0x02000D9F RID: 3487
		[Flags]
		private enum WindowsFileMapAccessType : uint
		{
			// Token: 0x040042C6 RID: 17094
			Copy = 1U,
			// Token: 0x040042C7 RID: 17095
			Write = 2U,
			// Token: 0x040042C8 RID: 17096
			Read = 4U,
			// Token: 0x040042C9 RID: 17097
			AllAccess = 8U,
			// Token: 0x040042CA RID: 17098
			Execute = 32U
		}
	}
}
