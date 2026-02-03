using System;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF1 RID: 3057
	public class ClientPartyInvitationWrapper
	{
		// Token: 0x06006159 RID: 24921 RVA: 0x0020012D File Offset: 0x001FE32D
		public ClientPartyInvitationWrapper(Guid by, string partyIdHex, long expiresAt)
		{
			this.By = by;
			this.PartyIdHex = partyIdHex;
			this.ExpiresAt = expiresAt;
		}

		// Token: 0x0600615A RID: 24922 RVA: 0x0020014C File Offset: 0x001FE34C
		public ClientPartyInvitationWrapper(ClientPartyInvitation invitation)
		{
			this.By = invitation.InvitedBy.Uuid_;
			this.PartyIdHex = invitation.PartyIdHex;
			this.ExpiresAt = invitation.ExpiresAt;
		}

		// Token: 0x04003C83 RID: 15491
		public readonly Guid By;

		// Token: 0x04003C84 RID: 15492
		public readonly string PartyIdHex;

		// Token: 0x04003C85 RID: 15493
		public readonly long ExpiresAt;
	}
}
