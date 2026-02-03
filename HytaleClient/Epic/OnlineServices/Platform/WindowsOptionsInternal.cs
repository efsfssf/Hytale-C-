using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.IntegratedPlatform;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x0200071E RID: 1822
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct WindowsOptionsInternal : ISettable<WindowsOptions>, IDisposable
	{
		// Token: 0x17000E2F RID: 3631
		// (set) Token: 0x06002F46 RID: 12102 RVA: 0x00045DF0 File Offset: 0x00043FF0
		public IntPtr Reserved
		{
			set
			{
				this.m_Reserved = value;
			}
		}

		// Token: 0x17000E30 RID: 3632
		// (set) Token: 0x06002F47 RID: 12103 RVA: 0x00045DFA File Offset: 0x00043FFA
		public Utf8String ProductId
		{
			set
			{
				Helper.Set(value, ref this.m_ProductId);
			}
		}

		// Token: 0x17000E31 RID: 3633
		// (set) Token: 0x06002F48 RID: 12104 RVA: 0x00045E0A File Offset: 0x0004400A
		public Utf8String SandboxId
		{
			set
			{
				Helper.Set(value, ref this.m_SandboxId);
			}
		}

		// Token: 0x17000E32 RID: 3634
		// (set) Token: 0x06002F49 RID: 12105 RVA: 0x00045E1A File Offset: 0x0004401A
		public ClientCredentials ClientCredentials
		{
			set
			{
				Helper.Set<ClientCredentials, ClientCredentialsInternal>(ref value, ref this.m_ClientCredentials);
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (set) Token: 0x06002F4A RID: 12106 RVA: 0x00045E2B File Offset: 0x0004402B
		public bool IsServer
		{
			set
			{
				Helper.Set(value, ref this.m_IsServer);
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (set) Token: 0x06002F4B RID: 12107 RVA: 0x00045E3B File Offset: 0x0004403B
		public Utf8String EncryptionKey
		{
			set
			{
				Helper.Set(value, ref this.m_EncryptionKey);
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (set) Token: 0x06002F4C RID: 12108 RVA: 0x00045E4B File Offset: 0x0004404B
		public Utf8String OverrideCountryCode
		{
			set
			{
				Helper.Set(value, ref this.m_OverrideCountryCode);
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (set) Token: 0x06002F4D RID: 12109 RVA: 0x00045E5B File Offset: 0x0004405B
		public Utf8String OverrideLocaleCode
		{
			set
			{
				Helper.Set(value, ref this.m_OverrideLocaleCode);
			}
		}

		// Token: 0x17000E37 RID: 3639
		// (set) Token: 0x06002F4E RID: 12110 RVA: 0x00045E6B File Offset: 0x0004406B
		public Utf8String DeploymentId
		{
			set
			{
				Helper.Set(value, ref this.m_DeploymentId);
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (set) Token: 0x06002F4F RID: 12111 RVA: 0x00045E7B File Offset: 0x0004407B
		public PlatformFlags Flags
		{
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (set) Token: 0x06002F50 RID: 12112 RVA: 0x00045E85 File Offset: 0x00044085
		public Utf8String CacheDirectory
		{
			set
			{
				Helper.Set(value, ref this.m_CacheDirectory);
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (set) Token: 0x06002F51 RID: 12113 RVA: 0x00045E95 File Offset: 0x00044095
		public uint TickBudgetInMilliseconds
		{
			set
			{
				this.m_TickBudgetInMilliseconds = value;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (set) Token: 0x06002F52 RID: 12114 RVA: 0x00045E9F File Offset: 0x0004409F
		public WindowsRTCOptions? RTCOptions
		{
			set
			{
				Helper.Set<WindowsRTCOptions, WindowsRTCOptionsInternal>(ref value, ref this.m_RTCOptions);
			}
		}

		// Token: 0x17000E3C RID: 3644
		// (set) Token: 0x06002F53 RID: 12115 RVA: 0x00045EB0 File Offset: 0x000440B0
		public IntegratedPlatformOptionsContainer IntegratedPlatformOptionsContainerHandle
		{
			set
			{
				Helper.Set(value, ref this.m_IntegratedPlatformOptionsContainerHandle);
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (set) Token: 0x06002F54 RID: 12116 RVA: 0x00045EC0 File Offset: 0x000440C0
		public IntPtr SystemSpecificOptions
		{
			set
			{
				this.m_SystemSpecificOptions = value;
			}
		}

		// Token: 0x17000E3E RID: 3646
		// (set) Token: 0x06002F55 RID: 12117 RVA: 0x00045ECA File Offset: 0x000440CA
		public double? TaskNetworkTimeoutSeconds
		{
			set
			{
				Helper.Set<double>(value, ref this.m_TaskNetworkTimeoutSeconds);
			}
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x00045EDC File Offset: 0x000440DC
		public void Set(ref WindowsOptions other)
		{
			this.m_ApiVersion = 14;
			this.Reserved = other.Reserved;
			this.ProductId = other.ProductId;
			this.SandboxId = other.SandboxId;
			this.ClientCredentials = other.ClientCredentials;
			this.IsServer = other.IsServer;
			this.EncryptionKey = other.EncryptionKey;
			this.OverrideCountryCode = other.OverrideCountryCode;
			this.OverrideLocaleCode = other.OverrideLocaleCode;
			this.DeploymentId = other.DeploymentId;
			this.Flags = other.Flags;
			this.CacheDirectory = other.CacheDirectory;
			this.TickBudgetInMilliseconds = other.TickBudgetInMilliseconds;
			this.RTCOptions = other.RTCOptions;
			this.IntegratedPlatformOptionsContainerHandle = other.IntegratedPlatformOptionsContainerHandle;
			this.SystemSpecificOptions = other.SystemSpecificOptions;
			this.TaskNetworkTimeoutSeconds = other.TaskNetworkTimeoutSeconds;
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x00045FC4 File Offset: 0x000441C4
		public void Set(ref WindowsOptions? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 14;
				this.Reserved = other.Value.Reserved;
				this.ProductId = other.Value.ProductId;
				this.SandboxId = other.Value.SandboxId;
				this.ClientCredentials = other.Value.ClientCredentials;
				this.IsServer = other.Value.IsServer;
				this.EncryptionKey = other.Value.EncryptionKey;
				this.OverrideCountryCode = other.Value.OverrideCountryCode;
				this.OverrideLocaleCode = other.Value.OverrideLocaleCode;
				this.DeploymentId = other.Value.DeploymentId;
				this.Flags = other.Value.Flags;
				this.CacheDirectory = other.Value.CacheDirectory;
				this.TickBudgetInMilliseconds = other.Value.TickBudgetInMilliseconds;
				this.RTCOptions = other.Value.RTCOptions;
				this.IntegratedPlatformOptionsContainerHandle = other.Value.IntegratedPlatformOptionsContainerHandle;
				this.SystemSpecificOptions = other.Value.SystemSpecificOptions;
				this.TaskNetworkTimeoutSeconds = other.Value.TaskNetworkTimeoutSeconds;
			}
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x0004613C File Offset: 0x0004433C
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Reserved);
			Helper.Dispose(ref this.m_ProductId);
			Helper.Dispose(ref this.m_SandboxId);
			Helper.Dispose<ClientCredentialsInternal>(ref this.m_ClientCredentials);
			Helper.Dispose(ref this.m_EncryptionKey);
			Helper.Dispose(ref this.m_OverrideCountryCode);
			Helper.Dispose(ref this.m_OverrideLocaleCode);
			Helper.Dispose(ref this.m_DeploymentId);
			Helper.Dispose(ref this.m_CacheDirectory);
			Helper.Dispose(ref this.m_RTCOptions);
			Helper.Dispose(ref this.m_IntegratedPlatformOptionsContainerHandle);
			Helper.Dispose(ref this.m_SystemSpecificOptions);
			Helper.Dispose(ref this.m_TaskNetworkTimeoutSeconds);
		}

		// Token: 0x0400150D RID: 5389
		private int m_ApiVersion;

		// Token: 0x0400150E RID: 5390
		private IntPtr m_Reserved;

		// Token: 0x0400150F RID: 5391
		private IntPtr m_ProductId;

		// Token: 0x04001510 RID: 5392
		private IntPtr m_SandboxId;

		// Token: 0x04001511 RID: 5393
		private ClientCredentialsInternal m_ClientCredentials;

		// Token: 0x04001512 RID: 5394
		private int m_IsServer;

		// Token: 0x04001513 RID: 5395
		private IntPtr m_EncryptionKey;

		// Token: 0x04001514 RID: 5396
		private IntPtr m_OverrideCountryCode;

		// Token: 0x04001515 RID: 5397
		private IntPtr m_OverrideLocaleCode;

		// Token: 0x04001516 RID: 5398
		private IntPtr m_DeploymentId;

		// Token: 0x04001517 RID: 5399
		private PlatformFlags m_Flags;

		// Token: 0x04001518 RID: 5400
		private IntPtr m_CacheDirectory;

		// Token: 0x04001519 RID: 5401
		private uint m_TickBudgetInMilliseconds;

		// Token: 0x0400151A RID: 5402
		private IntPtr m_RTCOptions;

		// Token: 0x0400151B RID: 5403
		private IntPtr m_IntegratedPlatformOptionsContainerHandle;

		// Token: 0x0400151C RID: 5404
		private IntPtr m_SystemSpecificOptions;

		// Token: 0x0400151D RID: 5405
		private IntPtr m_TaskNetworkTimeoutSeconds;
	}
}
