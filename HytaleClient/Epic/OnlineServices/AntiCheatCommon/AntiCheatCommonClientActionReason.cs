using System;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x02000695 RID: 1685
	public enum AntiCheatCommonClientActionReason
	{
		// Token: 0x040012C3 RID: 4803
		Invalid,
		// Token: 0x040012C4 RID: 4804
		InternalError,
		// Token: 0x040012C5 RID: 4805
		InvalidMessage,
		// Token: 0x040012C6 RID: 4806
		AuthenticationFailed,
		// Token: 0x040012C7 RID: 4807
		NullClient,
		// Token: 0x040012C8 RID: 4808
		HeartbeatTimeout,
		// Token: 0x040012C9 RID: 4809
		ClientViolation,
		// Token: 0x040012CA RID: 4810
		BackendViolation,
		// Token: 0x040012CB RID: 4811
		TemporaryCooldown,
		// Token: 0x040012CC RID: 4812
		TemporaryBanned,
		// Token: 0x040012CD RID: 4813
		PermanentBanned
	}
}
