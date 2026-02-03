using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002E1 RID: 737
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryPresenceCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryPresenceCallbackInfo>, ISettable<QueryPresenceCallbackInfo>, IDisposable
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x0001D8F8 File Offset: 0x0001BAF8
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x0001D910 File Offset: 0x0001BB10
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

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x0001D91C File Offset: 0x0001BB1C
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x0001D93D File Offset: 0x0001BB3D
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

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x0001D950 File Offset: 0x0001BB50
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x0001D968 File Offset: 0x0001BB68
		// (set) Token: 0x06001444 RID: 5188 RVA: 0x0001D989 File Offset: 0x0001BB89
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

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0001D99C File Offset: 0x0001BB9C
		// (set) Token: 0x06001446 RID: 5190 RVA: 0x0001D9BD File Offset: 0x0001BBBD
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

		// Token: 0x06001447 RID: 5191 RVA: 0x0001D9CD File Offset: 0x0001BBCD
		public void Set(ref QueryPresenceCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x0001DA04 File Offset: 0x0001BC04
		public void Set(ref QueryPresenceCallbackInfo? other)
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

		// Token: 0x06001449 RID: 5193 RVA: 0x0001DA72 File Offset: 0x0001BC72
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x0001DA99 File Offset: 0x0001BC99
		public void Get(out QueryPresenceCallbackInfo output)
		{
			output = default(QueryPresenceCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040008E6 RID: 2278
		private Result m_ResultCode;

		// Token: 0x040008E7 RID: 2279
		private IntPtr m_ClientData;

		// Token: 0x040008E8 RID: 2280
		private IntPtr m_LocalUserId;

		// Token: 0x040008E9 RID: 2281
		private IntPtr m_TargetUserId;
	}
}
