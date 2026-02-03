using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002C9 RID: 713
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct JoinGameAcceptedCallbackInfoInternal : ICallbackInfoInternal, IGettable<JoinGameAcceptedCallbackInfo>, ISettable<JoinGameAcceptedCallbackInfo>, IDisposable
	{
		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060013BA RID: 5050 RVA: 0x0001CBF8 File Offset: 0x0001ADF8
		// (set) Token: 0x060013BB RID: 5051 RVA: 0x0001CC19 File Offset: 0x0001AE19
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

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0001CC2C File Offset: 0x0001AE2C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x0001CC44 File Offset: 0x0001AE44
		// (set) Token: 0x060013BE RID: 5054 RVA: 0x0001CC65 File Offset: 0x0001AE65
		public Utf8String JoinInfo
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_JoinInfo, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_JoinInfo);
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x0001CC78 File Offset: 0x0001AE78
		// (set) Token: 0x060013C0 RID: 5056 RVA: 0x0001CC99 File Offset: 0x0001AE99
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

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x0001CCAC File Offset: 0x0001AEAC
		// (set) Token: 0x060013C2 RID: 5058 RVA: 0x0001CCCD File Offset: 0x0001AECD
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

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x0001CCE0 File Offset: 0x0001AEE0
		// (set) Token: 0x060013C4 RID: 5060 RVA: 0x0001CCF8 File Offset: 0x0001AEF8
		public ulong UiEventId
		{
			get
			{
				return this.m_UiEventId;
			}
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x0001CD04 File Offset: 0x0001AF04
		public void Set(ref JoinGameAcceptedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.JoinInfo = other.JoinInfo;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserId = other.TargetUserId;
			this.UiEventId = other.UiEventId;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0001CD54 File Offset: 0x0001AF54
		public void Set(ref JoinGameAcceptedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.JoinInfo = other.Value.JoinInfo;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserId = other.Value.TargetUserId;
				this.UiEventId = other.Value.UiEventId;
			}
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0001CDD7 File Offset: 0x0001AFD7
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_JoinInfo);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserId);
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x0001CE0A File Offset: 0x0001B00A
		public void Get(out JoinGameAcceptedCallbackInfo output)
		{
			output = default(JoinGameAcceptedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040008A9 RID: 2217
		private IntPtr m_ClientData;

		// Token: 0x040008AA RID: 2218
		private IntPtr m_JoinInfo;

		// Token: 0x040008AB RID: 2219
		private IntPtr m_LocalUserId;

		// Token: 0x040008AC RID: 2220
		private IntPtr m_TargetUserId;

		// Token: 0x040008AD RID: 2221
		private ulong m_UiEventId;
	}
}
