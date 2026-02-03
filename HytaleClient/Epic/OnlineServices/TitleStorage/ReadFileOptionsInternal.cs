using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000BE RID: 190
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReadFileOptionsInternal : ISettable<ReadFileOptions>, IDisposable
	{
		// Token: 0x17000153 RID: 339
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0000A0FD File Offset: 0x000082FD
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000154 RID: 340
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0000A10D File Offset: 0x0000830D
		public Utf8String Filename
		{
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x17000155 RID: 341
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0000A11D File Offset: 0x0000831D
		public uint ReadChunkLengthBytes
		{
			set
			{
				this.m_ReadChunkLengthBytes = value;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0000A128 File Offset: 0x00008328
		public static OnReadFileDataCallbackInternal ReadFileDataCallback
		{
			get
			{
				bool flag = ReadFileOptionsInternal.s_ReadFileDataCallback == null;
				if (flag)
				{
					ReadFileOptionsInternal.s_ReadFileDataCallback = new OnReadFileDataCallbackInternal(TitleStorageInterface.OnReadFileDataCallbackInternalImplementation);
				}
				return ReadFileOptionsInternal.s_ReadFileDataCallback;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0000A160 File Offset: 0x00008360
		public static OnFileTransferProgressCallbackInternal FileTransferProgressCallback
		{
			get
			{
				bool flag = ReadFileOptionsInternal.s_FileTransferProgressCallback == null;
				if (flag)
				{
					ReadFileOptionsInternal.s_FileTransferProgressCallback = new OnFileTransferProgressCallbackInternal(TitleStorageInterface.OnFileTransferProgressCallbackInternalImplementation);
				}
				return ReadFileOptionsInternal.s_FileTransferProgressCallback;
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0000A198 File Offset: 0x00008398
		public void Set(ref ReadFileOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.ReadChunkLengthBytes = other.ReadChunkLengthBytes;
			this.m_ReadFileDataCallback = ((other.ReadFileDataCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnReadFileDataCallbackInternal>(ReadFileOptionsInternal.ReadFileDataCallback) : IntPtr.Zero);
			this.m_FileTransferProgressCallback = ((other.FileTransferProgressCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnFileTransferProgressCallbackInternal>(ReadFileOptionsInternal.FileTransferProgressCallback) : IntPtr.Zero);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0000A214 File Offset: 0x00008414
		public void Set(ref ReadFileOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
				this.ReadChunkLengthBytes = other.Value.ReadChunkLengthBytes;
				this.m_ReadFileDataCallback = ((other.Value.ReadFileDataCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnReadFileDataCallbackInternal>(ReadFileOptionsInternal.ReadFileDataCallback) : IntPtr.Zero);
				this.m_FileTransferProgressCallback = ((other.Value.FileTransferProgressCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnFileTransferProgressCallbackInternal>(ReadFileOptionsInternal.FileTransferProgressCallback) : IntPtr.Zero);
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0000A2C5 File Offset: 0x000084C5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
			Helper.Dispose(ref this.m_ReadFileDataCallback);
			Helper.Dispose(ref this.m_FileTransferProgressCallback);
		}

		// Token: 0x0400035E RID: 862
		private int m_ApiVersion;

		// Token: 0x0400035F RID: 863
		private IntPtr m_LocalUserId;

		// Token: 0x04000360 RID: 864
		private IntPtr m_Filename;

		// Token: 0x04000361 RID: 865
		private uint m_ReadChunkLengthBytes;

		// Token: 0x04000362 RID: 866
		private IntPtr m_ReadFileDataCallback;

		// Token: 0x04000363 RID: 867
		private IntPtr m_FileTransferProgressCallback;

		// Token: 0x04000364 RID: 868
		private static OnReadFileDataCallbackInternal s_ReadFileDataCallback;

		// Token: 0x04000365 RID: 869
		private static OnFileTransferProgressCallbackInternal s_FileTransferProgressCallback;
	}
}
