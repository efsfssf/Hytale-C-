using System;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BEE RID: 3054
	public class ClientGuildInvitationWrapper
	{
		// Token: 0x06006155 RID: 24917 RVA: 0x001FFFF5 File Offset: 0x001FE1F5
		public ClientGuildInvitationWrapper(ClientGuildInvitation invitation)
		{
			this.Id = invitation.HexId;
			this.Name = invitation.Name;
			this.By = invitation.By.Uuid_;
			this.ExpiresAt = invitation.ExpiresAt;
		}

		// Token: 0x04003C77 RID: 15479
		public string Id;

		// Token: 0x04003C78 RID: 15480
		public string Name;

		// Token: 0x04003C79 RID: 15481
		public Guid By;

		// Token: 0x04003C7A RID: 15482
		public long ExpiresAt;
	}
}
