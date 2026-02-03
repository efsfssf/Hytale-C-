using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x020000E1 RID: 225
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ActiveSessionInfoInternal : IGettable<ActiveSessionInfo>, ISettable<ActiveSessionInfo>, IDisposable
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0000B868 File Offset: 0x00009A68
		// (set) Token: 0x060007F3 RID: 2035 RVA: 0x0000B889 File Offset: 0x00009A89
		public Utf8String SessionName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_SessionName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SessionName);
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0000B89C File Offset: 0x00009A9C
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x0000B8BD File Offset: 0x00009ABD
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

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0000B8D0 File Offset: 0x00009AD0
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x0000B8E8 File Offset: 0x00009AE8
		public OnlineSessionState State
		{
			get
			{
				return this.m_State;
			}
			set
			{
				this.m_State = value;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0000B8F4 File Offset: 0x00009AF4
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x0000B915 File Offset: 0x00009B15
		public SessionDetailsInfo? SessionDetails
		{
			get
			{
				SessionDetailsInfo? result;
				Helper.Get<SessionDetailsInfoInternal, SessionDetailsInfo>(this.m_SessionDetails, out result);
				return result;
			}
			set
			{
				Helper.Set<SessionDetailsInfo, SessionDetailsInfoInternal>(ref value, ref this.m_SessionDetails);
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0000B926 File Offset: 0x00009B26
		public void Set(ref ActiveSessionInfo other)
		{
			this.m_ApiVersion = 1;
			this.SessionName = other.SessionName;
			this.LocalUserId = other.LocalUserId;
			this.State = other.State;
			this.SessionDetails = other.SessionDetails;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0000B964 File Offset: 0x00009B64
		public void Set(ref ActiveSessionInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.SessionName = other.Value.SessionName;
				this.LocalUserId = other.Value.LocalUserId;
				this.State = other.Value.State;
				this.SessionDetails = other.Value.SessionDetails;
			}
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0000B9D9 File Offset: 0x00009BD9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_SessionName);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SessionDetails);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0000BA00 File Offset: 0x00009C00
		public void Get(out ActiveSessionInfo output)
		{
			output = default(ActiveSessionInfo);
			output.Set(ref this);
		}

		// Token: 0x040003D1 RID: 977
		private int m_ApiVersion;

		// Token: 0x040003D2 RID: 978
		private IntPtr m_SessionName;

		// Token: 0x040003D3 RID: 979
		private IntPtr m_LocalUserId;

		// Token: 0x040003D4 RID: 980
		private OnlineSessionState m_State;

		// Token: 0x040003D5 RID: 981
		private IntPtr m_SessionDetails;
	}
}
