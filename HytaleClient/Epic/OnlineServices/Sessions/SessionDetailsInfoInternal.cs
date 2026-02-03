using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000157 RID: 343
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionDetailsInfoInternal : IGettable<SessionDetailsInfo>, ISettable<SessionDetailsInfo>, IDisposable
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0000E464 File Offset: 0x0000C664
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x0000E485 File Offset: 0x0000C685
		public Utf8String SessionId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SessionId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SessionId);
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0000E498 File Offset: 0x0000C698
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x0000E4B9 File Offset: 0x0000C6B9
		public Utf8String HostAddress
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_HostAddress, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_HostAddress);
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0000E4CC File Offset: 0x0000C6CC
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x0000E4E4 File Offset: 0x0000C6E4
		public uint NumOpenPublicConnections
		{
			get
			{
				return this.m_NumOpenPublicConnections;
			}
			set
			{
				this.m_NumOpenPublicConnections = value;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0000E4F0 File Offset: 0x0000C6F0
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x0000E511 File Offset: 0x0000C711
		public SessionDetailsSettings? Settings
		{
			get
			{
				SessionDetailsSettings? result;
				Helper.Get<SessionDetailsSettingsInternal, SessionDetailsSettings>(this.m_Settings, out result);
				return result;
			}
			set
			{
				Helper.Set<SessionDetailsSettings, SessionDetailsSettingsInternal>(ref value, ref this.m_Settings);
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0000E524 File Offset: 0x0000C724
		// (set) Token: 0x06000A45 RID: 2629 RVA: 0x0000E545 File Offset: 0x0000C745
		public ProductUserId OwnerUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_OwnerUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_OwnerUserId);
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0000E558 File Offset: 0x0000C758
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x0000E579 File Offset: 0x0000C779
		public Utf8String OwnerServerClientId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_OwnerServerClientId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_OwnerServerClientId);
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0000E58C File Offset: 0x0000C78C
		public void Set(ref SessionDetailsInfo other)
		{
			this.m_ApiVersion = 2;
			this.SessionId = other.SessionId;
			this.HostAddress = other.HostAddress;
			this.NumOpenPublicConnections = other.NumOpenPublicConnections;
			this.Settings = other.Settings;
			this.OwnerUserId = other.OwnerUserId;
			this.OwnerServerClientId = other.OwnerServerClientId;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0000E5F0 File Offset: 0x0000C7F0
		public void Set(ref SessionDetailsInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.SessionId = other.Value.SessionId;
				this.HostAddress = other.Value.HostAddress;
				this.NumOpenPublicConnections = other.Value.NumOpenPublicConnections;
				this.Settings = other.Value.Settings;
				this.OwnerUserId = other.Value.OwnerUserId;
				this.OwnerServerClientId = other.Value.OwnerServerClientId;
			}
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0000E692 File Offset: 0x0000C892
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionId);
			Helper.Dispose(ref this.m_HostAddress);
			Helper.Dispose(ref this.m_Settings);
			Helper.Dispose(ref this.m_OwnerUserId);
			Helper.Dispose(ref this.m_OwnerServerClientId);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0000E6D1 File Offset: 0x0000C8D1
		public void Get(out SessionDetailsInfo output)
		{
			output = default(SessionDetailsInfo);
			output.Set(ref this);
		}

		// Token: 0x040004A6 RID: 1190
		private int m_ApiVersion;

		// Token: 0x040004A7 RID: 1191
		private IntPtr m_SessionId;

		// Token: 0x040004A8 RID: 1192
		private IntPtr m_HostAddress;

		// Token: 0x040004A9 RID: 1193
		private uint m_NumOpenPublicConnections;

		// Token: 0x040004AA RID: 1194
		private IntPtr m_Settings;

		// Token: 0x040004AB RID: 1195
		private IntPtr m_OwnerUserId;

		// Token: 0x040004AC RID: 1196
		private IntPtr m_OwnerServerClientId;
	}
}
