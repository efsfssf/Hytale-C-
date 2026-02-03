using System;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BEF RID: 3055
	public class ClientGuildMemberWrapper
	{
		// Token: 0x06006156 RID: 24918 RVA: 0x00200034 File Offset: 0x001FE234
		public ClientGuildMemberWrapper(ClientGuildMember guildMember)
		{
			this.Uuid = guildMember.User.Uuid_;
			this.Rank = guildMember.Rank;
		}

		// Token: 0x06006157 RID: 24919 RVA: 0x0020005B File Offset: 0x001FE25B
		public ClientGuildMemberWrapper(Guid uuid, ClientGuildRank rank)
		{
			this.Uuid = uuid;
			this.Rank = rank;
		}

		// Token: 0x04003C7B RID: 15483
		public Guid Uuid;

		// Token: 0x04003C7C RID: 15484
		public ClientGuildRank Rank;
	}
}
