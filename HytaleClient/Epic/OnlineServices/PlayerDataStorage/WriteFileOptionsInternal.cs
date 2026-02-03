using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x0200032B RID: 811
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct WriteFileOptionsInternal : ISettable<WriteFileOptions>, IDisposable
	{
		// Token: 0x1700060A RID: 1546
		// (set) Token: 0x0600163D RID: 5693 RVA: 0x0002060C File Offset: 0x0001E80C
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x1700060B RID: 1547
		// (set) Token: 0x0600163E RID: 5694 RVA: 0x0002061C File Offset: 0x0001E81C
		public Utf8String Filename
		{
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x1700060C RID: 1548
		// (set) Token: 0x0600163F RID: 5695 RVA: 0x0002062C File Offset: 0x0001E82C
		public uint ChunkLengthBytes
		{
			set
			{
				this.m_ChunkLengthBytes = value;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001640 RID: 5696 RVA: 0x00020638 File Offset: 0x0001E838
		public static OnWriteFileDataCallbackInternal WriteFileDataCallback
		{
			get
			{
				bool flag = WriteFileOptionsInternal.s_WriteFileDataCallback == null;
				if (flag)
				{
					WriteFileOptionsInternal.s_WriteFileDataCallback = new OnWriteFileDataCallbackInternal(PlayerDataStorageInterface.OnWriteFileDataCallbackInternalImplementation);
				}
				return WriteFileOptionsInternal.s_WriteFileDataCallback;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x00020670 File Offset: 0x0001E870
		public static OnFileTransferProgressCallbackInternal FileTransferProgressCallback
		{
			get
			{
				bool flag = WriteFileOptionsInternal.s_FileTransferProgressCallback == null;
				if (flag)
				{
					WriteFileOptionsInternal.s_FileTransferProgressCallback = new OnFileTransferProgressCallbackInternal(PlayerDataStorageInterface.OnFileTransferProgressCallbackInternalImplementation);
				}
				return WriteFileOptionsInternal.s_FileTransferProgressCallback;
			}
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x000206A8 File Offset: 0x0001E8A8
		public void Set(ref WriteFileOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.ChunkLengthBytes = other.ChunkLengthBytes;
			this.m_WriteFileDataCallback = ((other.WriteFileDataCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnWriteFileDataCallbackInternal>(WriteFileOptionsInternal.WriteFileDataCallback) : IntPtr.Zero);
			this.m_FileTransferProgressCallback = ((other.FileTransferProgressCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnFileTransferProgressCallbackInternal>(WriteFileOptionsInternal.FileTransferProgressCallback) : IntPtr.Zero);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x00020724 File Offset: 0x0001E924
		public void Set(ref WriteFileOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
				this.ChunkLengthBytes = other.Value.ChunkLengthBytes;
				this.m_WriteFileDataCallback = ((other.Value.WriteFileDataCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnWriteFileDataCallbackInternal>(WriteFileOptionsInternal.WriteFileDataCallback) : IntPtr.Zero);
				this.m_FileTransferProgressCallback = ((other.Value.FileTransferProgressCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnFileTransferProgressCallbackInternal>(WriteFileOptionsInternal.FileTransferProgressCallback) : IntPtr.Zero);
			}
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x000207D5 File Offset: 0x0001E9D5
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
			Helper.Dispose(ref this.m_WriteFileDataCallback);
			Helper.Dispose(ref this.m_FileTransferProgressCallback);
		}

		// Token: 0x040009AB RID: 2475
		private int m_ApiVersion;

		// Token: 0x040009AC RID: 2476
		private IntPtr m_LocalUserId;

		// Token: 0x040009AD RID: 2477
		private IntPtr m_Filename;

		// Token: 0x040009AE RID: 2478
		private uint m_ChunkLengthBytes;

		// Token: 0x040009AF RID: 2479
		private IntPtr m_WriteFileDataCallback;

		// Token: 0x040009B0 RID: 2480
		private IntPtr m_FileTransferProgressCallback;

		// Token: 0x040009B1 RID: 2481
		private static OnWriteFileDataCallbackInternal s_WriteFileDataCallback;

		// Token: 0x040009B2 RID: 2482
		private static OnFileTransferProgressCallbackInternal s_FileTransferProgressCallback;
	}
}
