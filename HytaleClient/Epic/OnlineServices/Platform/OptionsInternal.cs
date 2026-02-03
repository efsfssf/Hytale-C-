using System;
using System.Runtime.InteropServices;
using Epic.OnlineServices.IntegratedPlatform;

namespace Epic.OnlineServices.Platform
{
	// Token: 0x02000716 RID: 1814
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct OptionsInternal : ISettable<Options>, IDisposable
	{
		// Token: 0x17000E0B RID: 3595
		// (set) Token: 0x06002EFE RID: 12030 RVA: 0x000457D1 File Offset: 0x000439D1
		public IntPtr Reserved
		{
			set
			{
				this.m_Reserved = value;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (set) Token: 0x06002EFF RID: 12031 RVA: 0x000457DB File Offset: 0x000439DB
		public Utf8String ProductId
		{
			set
			{
				Helper.Set(value, ref this.m_ProductId);
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (set) Token: 0x06002F00 RID: 12032 RVA: 0x000457EB File Offset: 0x000439EB
		public Utf8String SandboxId
		{
			set
			{
				Helper.Set(value, ref this.m_SandboxId);
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (set) Token: 0x06002F01 RID: 12033 RVA: 0x000457FB File Offset: 0x000439FB
		public ClientCredentials ClientCredentials
		{
			set
			{
				Helper.Set<ClientCredentials, ClientCredentialsInternal>(ref value, ref this.m_ClientCredentials);
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (set) Token: 0x06002F02 RID: 12034 RVA: 0x0004580C File Offset: 0x00043A0C
		public bool IsServer
		{
			set
			{
				Helper.Set(value, ref this.m_IsServer);
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (set) Token: 0x06002F03 RID: 12035 RVA: 0x0004581C File Offset: 0x00043A1C
		public Utf8String EncryptionKey
		{
			set
			{
				Helper.Set(value, ref this.m_EncryptionKey);
			}
		}

		// Token: 0x17000E11 RID: 3601
		// (set) Token: 0x06002F04 RID: 12036 RVA: 0x0004582C File Offset: 0x00043A2C
		public Utf8String OverrideCountryCode
		{
			set
			{
				Helper.Set(value, ref this.m_OverrideCountryCode);
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (set) Token: 0x06002F05 RID: 12037 RVA: 0x0004583C File Offset: 0x00043A3C
		public Utf8String OverrideLocaleCode
		{
			set
			{
				Helper.Set(value, ref this.m_OverrideLocaleCode);
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (set) Token: 0x06002F06 RID: 12038 RVA: 0x0004584C File Offset: 0x00043A4C
		public Utf8String DeploymentId
		{
			set
			{
				Helper.Set(value, ref this.m_DeploymentId);
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (set) Token: 0x06002F07 RID: 12039 RVA: 0x0004585C File Offset: 0x00043A5C
		public PlatformFlags Flags
		{
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (set) Token: 0x06002F08 RID: 12040 RVA: 0x00045866 File Offset: 0x00043A66
		public Utf8String CacheDirectory
		{
			set
			{
				Helper.Set(value, ref this.m_CacheDirectory);
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (set) Token: 0x06002F09 RID: 12041 RVA: 0x00045876 File Offset: 0x00043A76
		public uint TickBudgetInMilliseconds
		{
			set
			{
				this.m_TickBudgetInMilliseconds = value;
			}
		}

		// Token: 0x17000E17 RID: 3607
		// (set) Token: 0x06002F0A RID: 12042 RVA: 0x00045880 File Offset: 0x00043A80
		public RTCOptions? RTCOptions
		{
			set
			{
				Helper.Set<RTCOptions, RTCOptionsInternal>(ref value, ref this.m_RTCOptions);
			}
		}

		// Token: 0x17000E18 RID: 3608
		// (set) Token: 0x06002F0B RID: 12043 RVA: 0x00045891 File Offset: 0x00043A91
		public IntegratedPlatformOptionsContainer IntegratedPlatformOptionsContainerHandle
		{
			set
			{
				Helper.Set(value, ref this.m_IntegratedPlatformOptionsContainerHandle);
			}
		}

		// Token: 0x17000E19 RID: 3609
		// (set) Token: 0x06002F0C RID: 12044 RVA: 0x000458A1 File Offset: 0x00043AA1
		public IntPtr SystemSpecificOptions
		{
			set
			{
				this.m_SystemSpecificOptions = value;
			}
		}

		// Token: 0x17000E1A RID: 3610
		// (set) Token: 0x06002F0D RID: 12045 RVA: 0x000458AB File Offset: 0x00043AAB
		public double? TaskNetworkTimeoutSeconds
		{
			set
			{
				Helper.Set<double>(value, ref this.m_TaskNetworkTimeoutSeconds);
			}
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000458BC File Offset: 0x00043ABC
		public void Set(ref Options other)
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

		// Token: 0x06002F0F RID: 12047 RVA: 0x000459A4 File Offset: 0x00043BA4
		public void Set(ref Options? other)
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

		// Token: 0x06002F10 RID: 12048 RVA: 0x00045B1C File Offset: 0x00043D1C
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

		// Token: 0x040014DA RID: 5338
		private int m_ApiVersion;

		// Token: 0x040014DB RID: 5339
		private IntPtr m_Reserved;

		// Token: 0x040014DC RID: 5340
		private IntPtr m_ProductId;

		// Token: 0x040014DD RID: 5341
		private IntPtr m_SandboxId;

		// Token: 0x040014DE RID: 5342
		private ClientCredentialsInternal m_ClientCredentials;

		// Token: 0x040014DF RID: 5343
		private int m_IsServer;

		// Token: 0x040014E0 RID: 5344
		private IntPtr m_EncryptionKey;

		// Token: 0x040014E1 RID: 5345
		private IntPtr m_OverrideCountryCode;

		// Token: 0x040014E2 RID: 5346
		private IntPtr m_OverrideLocaleCode;

		// Token: 0x040014E3 RID: 5347
		private IntPtr m_DeploymentId;

		// Token: 0x040014E4 RID: 5348
		private PlatformFlags m_Flags;

		// Token: 0x040014E5 RID: 5349
		private IntPtr m_CacheDirectory;

		// Token: 0x040014E6 RID: 5350
		private uint m_TickBudgetInMilliseconds;

		// Token: 0x040014E7 RID: 5351
		private IntPtr m_RTCOptions;

		// Token: 0x040014E8 RID: 5352
		private IntPtr m_IntegratedPlatformOptionsContainerHandle;

		// Token: 0x040014E9 RID: 5353
		private IntPtr m_SystemSpecificOptions;

		// Token: 0x040014EA RID: 5354
		private IntPtr m_TaskNetworkTimeoutSeconds;
	}
}
