using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000A2 RID: 162
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct FileTransferProgressCallbackInfoInternal : ICallbackInfoInternal, IGettable<FileTransferProgressCallbackInfo>, ISettable<FileTransferProgressCallbackInfo>, IDisposable
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x000092CC File Offset: 0x000074CC
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x000092ED File Offset: 0x000074ED
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

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00009300 File Offset: 0x00007500
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00009318 File Offset: 0x00007518
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x00009339 File Offset: 0x00007539
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

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0000934C File Offset: 0x0000754C
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x0000936D File Offset: 0x0000756D
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

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x00009380 File Offset: 0x00007580
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x00009398 File Offset: 0x00007598
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

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x000093A4 File Offset: 0x000075A4
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x000093BC File Offset: 0x000075BC
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

		// Token: 0x06000665 RID: 1637 RVA: 0x000093C8 File Offset: 0x000075C8
		public void Set(ref FileTransferProgressCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.BytesTransferred = other.BytesTransferred;
			this.TotalFileSizeBytes = other.TotalFileSizeBytes;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00009418 File Offset: 0x00007618
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

		// Token: 0x06000667 RID: 1639 RVA: 0x0000949B File Offset: 0x0000769B
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x000094C2 File Offset: 0x000076C2
		public void Get(out FileTransferProgressCallbackInfo output)
		{
			output = default(FileTransferProgressCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000323 RID: 803
		private IntPtr m_ClientData;

		// Token: 0x04000324 RID: 804
		private IntPtr m_LocalUserId;

		// Token: 0x04000325 RID: 805
		private IntPtr m_Filename;

		// Token: 0x04000326 RID: 806
		private uint m_BytesTransferred;

		// Token: 0x04000327 RID: 807
		private uint m_TotalFileSizeBytes;
	}
}
