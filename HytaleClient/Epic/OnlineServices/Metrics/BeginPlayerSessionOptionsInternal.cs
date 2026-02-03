using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Metrics
{
	// Token: 0x0200034E RID: 846
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct BeginPlayerSessionOptionsInternal : ISettable<BeginPlayerSessionOptions>, IDisposable
	{
		// Token: 0x1700065C RID: 1628
		// (set) Token: 0x0600172C RID: 5932 RVA: 0x00021C3F File Offset: 0x0001FE3F
		public BeginPlayerSessionOptionsAccountId AccountId
		{
			set
			{
				Helper.Set<BeginPlayerSessionOptionsAccountId, BeginPlayerSessionOptionsAccountIdInternal>(ref value, ref this.m_AccountId);
			}
		}

		// Token: 0x1700065D RID: 1629
		// (set) Token: 0x0600172D RID: 5933 RVA: 0x00021C50 File Offset: 0x0001FE50
		public Utf8String DisplayName
		{
			set
			{
				Helper.Set(value, ref this.m_DisplayName);
			}
		}

		// Token: 0x1700065E RID: 1630
		// (set) Token: 0x0600172E RID: 5934 RVA: 0x00021C60 File Offset: 0x0001FE60
		public UserControllerType ControllerType
		{
			set
			{
				this.m_ControllerType = value;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (set) Token: 0x0600172F RID: 5935 RVA: 0x00021C6A File Offset: 0x0001FE6A
		public Utf8String ServerIp
		{
			set
			{
				Helper.Set(value, ref this.m_ServerIp);
			}
		}

		// Token: 0x17000660 RID: 1632
		// (set) Token: 0x06001730 RID: 5936 RVA: 0x00021C7A File Offset: 0x0001FE7A
		public Utf8String GameSessionId
		{
			set
			{
				Helper.Set(value, ref this.m_GameSessionId);
			}
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x00021C8C File Offset: 0x0001FE8C
		public void Set(ref BeginPlayerSessionOptions other)
		{
			this.m_ApiVersion = 1;
			this.AccountId = other.AccountId;
			this.DisplayName = other.DisplayName;
			this.ControllerType = other.ControllerType;
			this.ServerIp = other.ServerIp;
			this.GameSessionId = other.GameSessionId;
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x00021CE4 File Offset: 0x0001FEE4
		public void Set(ref BeginPlayerSessionOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.AccountId = other.Value.AccountId;
				this.DisplayName = other.Value.DisplayName;
				this.ControllerType = other.Value.ControllerType;
				this.ServerIp = other.Value.ServerIp;
				this.GameSessionId = other.Value.GameSessionId;
			}
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x00021D6E File Offset: 0x0001FF6E
		public void Dispose()
		{
			Helper.Dispose<BeginPlayerSessionOptionsAccountIdInternal>(ref this.m_AccountId);
			Helper.Dispose(ref this.m_DisplayName);
			Helper.Dispose(ref this.m_ServerIp);
			Helper.Dispose(ref this.m_GameSessionId);
		}

		// Token: 0x04000A13 RID: 2579
		private int m_ApiVersion;

		// Token: 0x04000A14 RID: 2580
		private BeginPlayerSessionOptionsAccountIdInternal m_AccountId;

		// Token: 0x04000A15 RID: 2581
		private IntPtr m_DisplayName;

		// Token: 0x04000A16 RID: 2582
		private UserControllerType m_ControllerType;

		// Token: 0x04000A17 RID: 2583
		private IntPtr m_ServerIp;

		// Token: 0x04000A18 RID: 2584
		private IntPtr m_GameSessionId;
	}
}
