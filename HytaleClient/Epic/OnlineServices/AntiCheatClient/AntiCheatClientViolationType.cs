using System;

namespace Epic.OnlineServices.AntiCheatClient
{
	// Token: 0x020006DD RID: 1757
	public enum AntiCheatClientViolationType
	{
		// Token: 0x04001410 RID: 5136
		Invalid,
		// Token: 0x04001411 RID: 5137
		IntegrityCatalogNotFound,
		// Token: 0x04001412 RID: 5138
		IntegrityCatalogError,
		// Token: 0x04001413 RID: 5139
		IntegrityCatalogCertificateRevoked,
		// Token: 0x04001414 RID: 5140
		IntegrityCatalogMissingMainExecutable,
		// Token: 0x04001415 RID: 5141
		GameFileMismatch,
		// Token: 0x04001416 RID: 5142
		RequiredGameFileNotFound,
		// Token: 0x04001417 RID: 5143
		UnknownGameFileForbidden,
		// Token: 0x04001418 RID: 5144
		SystemFileUntrusted,
		// Token: 0x04001419 RID: 5145
		ForbiddenModuleLoaded,
		// Token: 0x0400141A RID: 5146
		CorruptedMemory,
		// Token: 0x0400141B RID: 5147
		ForbiddenToolDetected,
		// Token: 0x0400141C RID: 5148
		InternalAntiCheatViolation,
		// Token: 0x0400141D RID: 5149
		CorruptedNetworkMessageFlow,
		// Token: 0x0400141E RID: 5150
		VirtualMachineNotAllowed,
		// Token: 0x0400141F RID: 5151
		ForbiddenSystemConfiguration
	}
}
