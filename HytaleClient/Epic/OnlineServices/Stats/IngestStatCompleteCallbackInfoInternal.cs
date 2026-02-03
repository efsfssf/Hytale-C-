using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000CB RID: 203
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct IngestStatCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<IngestStatCompleteCallbackInfo>, ISettable<IngestStatCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0000AB20 File Offset: 0x00008D20
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x0000AB38 File Offset: 0x00008D38
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0000AB44 File Offset: 0x00008D44
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x0000AB65 File Offset: 0x00008D65
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

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x0000AB78 File Offset: 0x00008D78
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0000AB90 File Offset: 0x00008D90
		// (set) Token: 0x0600076D RID: 1901 RVA: 0x0000ABB1 File Offset: 0x00008DB1
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

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0000ABC4 File Offset: 0x00008DC4
		// (set) Token: 0x0600076F RID: 1903 RVA: 0x0000ABE5 File Offset: 0x00008DE5
		public ProductUserId TargetUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0000ABF5 File Offset: 0x00008DF5
		public void Set(ref IngestStatCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public void Set(ref IngestStatCompleteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0000AC9A File Offset: 0x00008E9A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0000ACC1 File Offset: 0x00008EC1
		public void Get(out IngestStatCompleteCallbackInfo output)
		{
			output = default(IngestStatCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000390 RID: 912
		private Result m_ResultCode;

		// Token: 0x04000391 RID: 913
		private IntPtr m_ClientData;

		// Token: 0x04000392 RID: 914
		private IntPtr m_LocalUserId;

		// Token: 0x04000393 RID: 915
		private IntPtr m_TargetUserId;
	}
}
