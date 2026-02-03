using System;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF5 RID: 3061
	public class ClientUserWrapper
	{
		// Token: 0x0600615F RID: 24927 RVA: 0x00200270 File Offset: 0x001FE470
		public ClientUserWrapper(ClientUser user)
		{
			this.Uuid = user.Uuid_;
			this.Name = user.Name;
			this.AvatarUrl = user.AvatarUrl;
			this.CurrentState = user.CurrentState;
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x002002AA File Offset: 0x001FE4AA
		public ClientUserWrapper(Guid uuid, string name, string avatarUrl)
		{
			this.Uuid = uuid;
			this.Name = name;
			this.AvatarUrl = avatarUrl;
		}

		// Token: 0x04003C8F RID: 15503
		public Guid Uuid;

		// Token: 0x04003C90 RID: 15504
		public string Name;

		// Token: 0x04003C91 RID: 15505
		public string AvatarUrl;

		// Token: 0x04003C92 RID: 15506
		public string CurrentState;
	}
}
