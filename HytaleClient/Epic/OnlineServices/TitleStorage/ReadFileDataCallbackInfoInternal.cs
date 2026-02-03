using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.TitleStorage
{
	// Token: 0x020000BC RID: 188
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ReadFileDataCallbackInfoInternal : ICallbackInfoInternal, IGettable<ReadFileDataCallbackInfo>, ISettable<ReadFileDataCallbackInfo>, IDisposable
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00009E20 File Offset: 0x00008020
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x00009E41 File Offset: 0x00008041
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

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00009E54 File Offset: 0x00008054
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00009E6C File Offset: 0x0000806C
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x00009E8D File Offset: 0x0000808D
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

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00009EA0 File Offset: 0x000080A0
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x00009EC1 File Offset: 0x000080C1
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

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000707 RID: 1799 RVA: 0x00009ED4 File Offset: 0x000080D4
		// (set) Token: 0x06000708 RID: 1800 RVA: 0x00009EEC File Offset: 0x000080EC
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x00009EF8 File Offset: 0x000080F8
		// (set) Token: 0x0600070A RID: 1802 RVA: 0x00009F19 File Offset: 0x00008119
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

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00009F2C File Offset: 0x0000812C
		// (set) Token: 0x0600070C RID: 1804 RVA: 0x00009F53 File Offset: 0x00008153
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

		// Token: 0x0600070D RID: 1805 RVA: 0x00009F6C File Offset: 0x0000816C
		public void Set(ref ReadFileDataCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
			this.TotalFileSizeBytes = other.TotalFileSizeBytes;
			this.IsLastChunk = other.IsLastChunk;
			this.DataChunk = other.DataChunk;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00009FC8 File Offset: 0x000081C8
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

		// Token: 0x0600070F RID: 1807 RVA: 0x0000A063 File Offset: 0x00008263
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
			Helper.Dispose(ref this.m_DataChunk);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0000A096 File Offset: 0x00008296
		public void Get(out ReadFileDataCallbackInfo output)
		{
			output = default(ReadFileDataCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000352 RID: 850
		private IntPtr m_ClientData;

		// Token: 0x04000353 RID: 851
		private IntPtr m_LocalUserId;

		// Token: 0x04000354 RID: 852
		private IntPtr m_Filename;

		// Token: 0x04000355 RID: 853
		private uint m_TotalFileSizeBytes;

		// Token: 0x04000356 RID: 854
		private int m_IsLastChunk;

		// Token: 0x04000357 RID: 855
		private uint m_DataChunkLengthBytes;

		// Token: 0x04000358 RID: 856
		private IntPtr m_DataChunk;
	}
}
