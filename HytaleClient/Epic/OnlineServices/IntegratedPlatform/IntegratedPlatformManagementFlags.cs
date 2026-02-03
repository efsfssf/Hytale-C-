using System;

namespace Epic.OnlineServices.IntegratedPlatform
{
	// Token: 0x020004BA RID: 1210
	[Flags]
	public enum IntegratedPlatformManagementFlags
	{
		// Token: 0x04000DB4 RID: 3508
		Disabled = 1,
		// Token: 0x04000DB5 RID: 3509
		LibraryManagedByApplication = 2,
		// Token: 0x04000DB6 RID: 3510
		LibraryManagedBySDK = 4,
		// Token: 0x04000DB7 RID: 3511
		DisablePresenceMirroring = 8,
		// Token: 0x04000DB8 RID: 3512
		DisableSDKManagedSessions = 16,
		// Token: 0x04000DB9 RID: 3513
		PreferEOSIdentity = 32,
		// Token: 0x04000DBA RID: 3514
		PreferIntegratedIdentity = 64,
		// Token: 0x04000DBB RID: 3515
		ApplicationManagedIdentityLogin = 128
	}
}
