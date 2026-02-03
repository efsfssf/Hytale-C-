using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002D1 RID: 721
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PresenceChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<PresenceChangedCallbackInfo>, ISettable<PresenceChangedCallbackInfo>, IDisposable
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x0001CE98 File Offset: 0x0001B098
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x0001CEB9 File Offset: 0x0001B0B9
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

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x0001CECC File Offset: 0x0001B0CC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x0001CEE4 File Offset: 0x0001B0E4
		// (set) Token: 0x060013ED RID: 5101 RVA: 0x0001CF05 File Offset: 0x0001B105
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

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060013EE RID: 5102 RVA: 0x0001CF18 File Offset: 0x0001B118
		// (set) Token: 0x060013EF RID: 5103 RVA: 0x0001CF39 File Offset: 0x0001B139
		public EpicAccountId PresenceUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_PresenceUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_PresenceUserId);
			}
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x0001CF49 File Offset: 0x0001B149
		public void Set(ref PresenceChangedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.PresenceUserId = other.PresenceUserId;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0001CF74 File Offset: 0x0001B174
		public void Set(ref PresenceChangedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.PresenceUserId = other.Value.PresenceUserId;
			}
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0001CFCD File Offset: 0x0001B1CD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_PresenceUserId);
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x0001CFF4 File Offset: 0x0001B1F4
		public void Get(out PresenceChangedCallbackInfo output)
		{
			output = default(PresenceChangedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040008B1 RID: 2225
		private IntPtr m_ClientData;

		// Token: 0x040008B2 RID: 2226
		private IntPtr m_LocalUserId;

		// Token: 0x040008B3 RID: 2227
		private IntPtr m_PresenceUserId;
	}
}
