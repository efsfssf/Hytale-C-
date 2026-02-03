using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Stats
{
	// Token: 0x020000D3 RID: 211
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnQueryStatsCompleteCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnQueryStatsCompleteCallbackInfo>, ISettable<OnQueryStatsCompleteCallbackInfo>, IDisposable
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0000AE90 File Offset: 0x00009090
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x0000AEA8 File Offset: 0x000090A8
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

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0000AEB4 File Offset: 0x000090B4
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0000AED5 File Offset: 0x000090D5
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

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0000AEE8 File Offset: 0x000090E8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x0000AF00 File Offset: 0x00009100
		// (set) Token: 0x060007A0 RID: 1952 RVA: 0x0000AF21 File Offset: 0x00009121
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

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0000AF34 File Offset: 0x00009134
		// (set) Token: 0x060007A2 RID: 1954 RVA: 0x0000AF55 File Offset: 0x00009155
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

		// Token: 0x060007A3 RID: 1955 RVA: 0x0000AF65 File Offset: 0x00009165
		public void Set(ref OnQueryStatsCompleteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0000AF9C File Offset: 0x0000919C
		public void Set(ref OnQueryStatsCompleteCallbackInfo? other)
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

		// Token: 0x060007A5 RID: 1957 RVA: 0x0000B00A File Offset: 0x0000920A
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0000B031 File Offset: 0x00009231
		public void Get(out OnQueryStatsCompleteCallbackInfo output)
		{
			output = default(OnQueryStatsCompleteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040003A0 RID: 928
		private Result m_ResultCode;

		// Token: 0x040003A1 RID: 929
		private IntPtr m_ClientData;

		// Token: 0x040003A2 RID: 930
		private IntPtr m_LocalUserId;

		// Token: 0x040003A3 RID: 931
		private IntPtr m_TargetUserId;
	}
}
