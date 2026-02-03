using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x02000071 RID: 113
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OnShowBlockPlayerCallbackInfoInternal : ICallbackInfoInternal, IGettable<OnShowBlockPlayerCallbackInfo>, ISettable<OnShowBlockPlayerCallbackInfo>, IDisposable
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x00007270 File Offset: 0x00005470
		// (set) Token: 0x0600050A RID: 1290 RVA: 0x00007288 File Offset: 0x00005488
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

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00007294 File Offset: 0x00005494
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x000072B5 File Offset: 0x000054B5
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

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600050D RID: 1293 RVA: 0x000072C8 File Offset: 0x000054C8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x000072E0 File Offset: 0x000054E0
		// (set) Token: 0x0600050F RID: 1295 RVA: 0x00007301 File Offset: 0x00005501
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x00007314 File Offset: 0x00005514
		// (set) Token: 0x06000511 RID: 1297 RVA: 0x00007335 File Offset: 0x00005535
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

		// Token: 0x06000512 RID: 1298 RVA: 0x00007345 File Offset: 0x00005545
		public void Set(ref OnShowBlockPlayerCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0000737C File Offset: 0x0000557C
		public void Set(ref OnShowBlockPlayerCallbackInfo? other)
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

		// Token: 0x06000514 RID: 1300 RVA: 0x000073EA File Offset: 0x000055EA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00007411 File Offset: 0x00005611
		public void Get(out OnShowBlockPlayerCallbackInfo output)
		{
			output = default(OnShowBlockPlayerCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000285 RID: 645
		private Result m_ResultCode;

		// Token: 0x04000286 RID: 646
		private IntPtr m_ClientData;

		// Token: 0x04000287 RID: 647
		private IntPtr m_LocalUserId;

		// Token: 0x04000288 RID: 648
		private IntPtr m_TargetUserId;
	}
}
