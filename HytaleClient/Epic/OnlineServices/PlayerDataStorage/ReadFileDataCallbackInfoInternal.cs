using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000322 RID: 802
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReadFileDataCallbackInfoInternal : ICallbackInfoInternal, IGettable<ReadFileDataCallbackInfo>, ISettable<ReadFileDataCallbackInfo>, IDisposable
	{
		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x0001FC44 File Offset: 0x0001DE44
		// (set) Token: 0x060015E3 RID: 5603 RVA: 0x0001FC65 File Offset: 0x0001DE65
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

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x0001FC78 File Offset: 0x0001DE78
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x0001FC90 File Offset: 0x0001DE90
		// (set) Token: 0x060015E6 RID: 5606 RVA: 0x0001FCB1 File Offset: 0x0001DEB1
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

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x060015E7 RID: 5607 RVA: 0x0001FCC4 File Offset: 0x0001DEC4
		// (set) Token: 0x060015E8 RID: 5608 RVA: 0x0001FCE5 File Offset: 0x0001DEE5
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

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0001FCF8 File Offset: 0x0001DEF8
		// (set) Token: 0x060015EA RID: 5610 RVA: 0x0001FD10 File Offset: 0x0001DF10
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

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x060015EB RID: 5611 RVA: 0x0001FD1C File Offset: 0x0001DF1C
		// (set) Token: 0x060015EC RID: 5612 RVA: 0x0001FD3D File Offset: 0x0001DF3D
		public bool IsLastChunk
		{
			get
			{
				bool result;
				Helper.Get(this.m_IsLastChunk, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_IsLastChunk);
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060015ED RID: 5613 RVA: 0x0001FD50 File Offset: 0x0001DF50
		// (set) Token: 0x060015EE RID: 5614 RVA: 0x0001FD77 File Offset: 0x0001DF77
		public ArraySegment<byte> DataChunk
		{
			get
			{
				ArraySegment<byte> result;
				Helper.Get(this.m_DataChunk, out result, this.m_DataChunkLengthBytes);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_DataChunk, out this.m_DataChunkLengthBytes);
			}
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0001FD90 File Offset: 0x0001DF90
		public void Set(ref ReadFileDataCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.TotalFileSizeBytes = other.TotalFileSizeBytes;
			this.IsLastChunk = other.IsLastChunk;
			this.DataChunk = other.DataChunk;
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0001FDEC File Offset: 0x0001DFEC
		public void Set(ref ReadFileDataCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
				this.TotalFileSizeBytes = other.Value.TotalFileSizeBytes;
				this.IsLastChunk = other.Value.IsLastChunk;
				this.DataChunk = other.Value.DataChunk;
			}
		}

		// Token: 0x060015F1 RID: 5617 RVA: 0x0001FE87 File Offset: 0x0001E087
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
			Helper.Dispose(ref this.m_DataChunk);
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x0001FEBA File Offset: 0x0001E0BA
		public void Get(out ReadFileDataCallbackInfo output)
		{
			output = default(ReadFileDataCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400097E RID: 2430
		private IntPtr m_ClientData;

		// Token: 0x0400097F RID: 2431
		private IntPtr m_LocalUserId;

		// Token: 0x04000980 RID: 2432
		private IntPtr m_Filename;

		// Token: 0x04000981 RID: 2433
		private uint m_TotalFileSizeBytes;

		// Token: 0x04000982 RID: 2434
		private int m_IsLastChunk;

		// Token: 0x04000983 RID: 2435
		private uint m_DataChunkLengthBytes;

		// Token: 0x04000984 RID: 2436
		private IntPtr m_DataChunk;
	}
}
