using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000324 RID: 804
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReadFileOptionsInternal : ISettable<ReadFileOptions>, IDisposable
	{
		// Token: 0x170005EE RID: 1518
		// (set) Token: 0x060015FD RID: 5629 RVA: 0x0001FF21 File Offset: 0x0001E121
		public ProductUserId LocalUserId
		{
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170005EF RID: 1519
		// (set) Token: 0x060015FE RID: 5630 RVA: 0x0001FF31 File Offset: 0x0001E131
		public Utf8String Filename
		{
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (set) Token: 0x060015FF RID: 5631 RVA: 0x0001FF41 File Offset: 0x0001E141
		public uint ReadChunkLengthBytes
		{
			set
			{
				this.m_ReadChunkLengthBytes = value;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06001600 RID: 5632 RVA: 0x0001FF4C File Offset: 0x0001E14C
		public static OnReadFileDataCallbackInternal ReadFileDataCallback
		{
			get
			{
				bool flag = ReadFileOptionsInternal.s_ReadFileDataCallback == null;
				if (flag)
				{
					ReadFileOptionsInternal.s_ReadFileDataCallback = new OnReadFileDataCallbackInternal(PlayerDataStorageInterface.OnReadFileDataCallbackInternalImplementation);
				}
				return ReadFileOptionsInternal.s_ReadFileDataCallback;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001601 RID: 5633 RVA: 0x0001FF84 File Offset: 0x0001E184
		public static OnFileTransferProgressCallbackInternal FileTransferProgressCallback
		{
			get
			{
				bool flag = ReadFileOptionsInternal.s_FileTransferProgressCallback == null;
				if (flag)
				{
					ReadFileOptionsInternal.s_FileTransferProgressCallback = new OnFileTransferProgressCallbackInternal(PlayerDataStorageInterface.OnFileTransferProgressCallbackInternalImplementation);
				}
				return ReadFileOptionsInternal.s_FileTransferProgressCallback;
			}
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0001FFBC File Offset: 0x0001E1BC
		public void Set(ref ReadFileOptions other)
		{
			this.m_ApiVersion = 2;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.ReadChunkLengthBytes = other.ReadChunkLengthBytes;
			this.m_ReadFileDataCallback = ((other.ReadFileDataCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnReadFileDataCallbackInternal>(ReadFileOptionsInternal.ReadFileDataCallback) : IntPtr.Zero);
			this.m_FileTransferProgressCallback = ((other.FileTransferProgressCallback != null) ? Marshal.GetFunctionPointerForDelegate<OnFileTransferProgressCallbackInternal>(ReadFileOptionsInternal.FileTransferProgressCallback) : IntPtr.Zero);
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00020038 File Offset: 0x0001E238
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

		// Token: 0x06001604 RID: 5636 RVA: 0x000200E9 File Offset: 0x0001E2E9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
			Helper.Dispose(ref this.m_ReadFileDataCallback);
			Helper.Dispose(ref this.m_FileTransferProgressCallback);
		}

		// Token: 0x0400098A RID: 2442
		private int m_ApiVersion;

		// Token: 0x0400098B RID: 2443
		private IntPtr m_LocalUserId;

		// Token: 0x0400098C RID: 2444
		private IntPtr m_Filename;

		// Token: 0x0400098D RID: 2445
		private uint m_ReadChunkLengthBytes;

		// Token: 0x0400098E RID: 2446
		private IntPtr m_ReadFileDataCallback;

		// Token: 0x0400098F RID: 2447
		private IntPtr m_FileTransferProgressCallback;

		// Token: 0x04000990 RID: 2448
		private static OnReadFileDataCallbackInternal s_ReadFileDataCallback;

		// Token: 0x04000991 RID: 2449
		private static OnFileTransferProgressCallbackInternal s_FileTransferProgressCallback;
	}
}
