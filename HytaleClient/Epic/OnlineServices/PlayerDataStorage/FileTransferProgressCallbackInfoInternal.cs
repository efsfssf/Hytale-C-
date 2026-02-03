using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x020002FE RID: 766
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct FileTransferProgressCallbackInfoInternal : ICallbackInfoInternal, IGettable<FileTransferProgressCallbackInfo>, ISettable<FileTransferProgressCallbackInfo>, IDisposable
	{
		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001503 RID: 5379 RVA: 0x0001EACC File Offset: 0x0001CCCC
		// (set) Token: 0x06001504 RID: 5380 RVA: 0x0001EAED File Offset: 0x0001CCED
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x0001EB00 File Offset: 0x0001CD00
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x0001EB18 File Offset: 0x0001CD18
		// (set) Token: 0x06001507 RID: 5383 RVA: 0x0001EB39 File Offset: 0x0001CD39
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0001EB4C File Offset: 0x0001CD4C
		// (set) Token: 0x06001509 RID: 5385 RVA: 0x0001EB6D File Offset: 0x0001CD6D
		public Utf8String Filename
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Filename, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x0001EB80 File Offset: 0x0001CD80
		// (set) Token: 0x0600150B RID: 5387 RVA: 0x0001EB98 File Offset: 0x0001CD98
		public uint BytesTransferred
		{
			get
			{
				return this.m_BytesTransferred;
			}
			set
			{
				this.m_BytesTransferred = value;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x0001EBA4 File Offset: 0x0001CDA4
		// (set) Token: 0x0600150D RID: 5389 RVA: 0x0001EBBC File Offset: 0x0001CDBC
		public uint TotalFileSizeBytes
		{
			get
			{
				return this.m_TotalFileSizeBytes;
			}
			set
			{
				this.m_TotalFileSizeBytes = value;
			}
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0001EBC8 File Offset: 0x0001CDC8
		public void Set(ref FileTransferProgressCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.BytesTransferred = other.BytesTransferred;
			this.TotalFileSizeBytes = other.TotalFileSizeBytes;
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0001EC18 File Offset: 0x0001CE18
		public void Set(ref FileTransferProgressCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
				this.BytesTransferred = other.Value.BytesTransferred;
				this.TotalFileSizeBytes = other.Value.TotalFileSizeBytes;
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0001EC9B File Offset: 0x0001CE9B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0001ECC2 File Offset: 0x0001CEC2
		public void Get(out FileTransferProgressCallbackInfo output)
		{
			output = default(FileTransferProgressCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400093B RID: 2363
		private IntPtr m_ClientData;

		// Token: 0x0400093C RID: 2364
		private IntPtr m_LocalUserId;

		// Token: 0x0400093D RID: 2365
		private IntPtr m_Filename;

		// Token: 0x0400093E RID: 2366
		private uint m_BytesTransferred;

		// Token: 0x0400093F RID: 2367
		private uint m_TotalFileSizeBytes;
	}
}
