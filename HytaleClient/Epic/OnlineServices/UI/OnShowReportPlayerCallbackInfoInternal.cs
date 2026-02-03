using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000079 RID: 121
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnShowReportPlayerCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnShowReportPlayerCallbackInfo>, ISettable<OnShowReportPlayerCallbackInfo>, IDisposable
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x000074BC File Offset: 0x000056BC
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x000074D4 File Offset: 0x000056D4
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

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x000074E0 File Offset: 0x000056E0
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x00007501 File Offset: 0x00005701
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

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00007514 File Offset: 0x00005714
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x0000752C File Offset: 0x0000572C
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x0000754D File Offset: 0x0000574D
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00007560 File Offset: 0x00005760
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x00007581 File Offset: 0x00005781
		public EpicAccountId TargetUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_TargetUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserId);
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00007591 File Offset: 0x00005791
		public void Set(ref OnShowReportPlayerCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000075C8 File Offset: 0x000057C8
		public void Set(ref OnShowReportPlayerCallbackInfo? other)
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

		// Token: 0x06000543 RID: 1347 RVA: 0x00007636 File Offset: 0x00005836
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0000765D File Offset: 0x0000585D
		public void Get(out OnShowReportPlayerCallbackInfo output)
		{
			output = default(OnShowReportPlayerCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400028D RID: 653
		private Result m_ResultCode;

		// Token: 0x0400028E RID: 654
		private IntPtr m_ClientData;

		// Token: 0x0400028F RID: 655
		private IntPtr m_LocalUserId;

		// Token: 0x04000290 RID: 656
		private IntPtr m_TargetUserId;
	}
}
