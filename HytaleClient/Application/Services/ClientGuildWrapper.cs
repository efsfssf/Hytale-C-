using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF0 RID: 3056
	public class ClientGuildWrapper
	{
		// Token: 0x06006158 RID: 24920 RVA: 0x00200074 File Offset: 0x001FE274
		public ClientGuildWrapper(ClientGuild guild)
		{
			this.Name = guild.Name;
			this.GuildId = guild.GuildId;
			this.Leader = new ClientGuildMemberWrapper(guild.Leader);
			this.Officers = Enumerable.ToList<ClientGuildMemberWrapper>(Enumerable.Select<ClientGuildMember, ClientGuildMemberWrapper>(guild.Officers, (ClientGuildMember member) => new ClientGuildMemberWrapper(member)));
			this.Members = Enumerable.ToList<ClientGuildMemberWrapper>(Enumerable.Select<ClientGuildMember, ClientGuildMemberWrapper>(guild.Members, (ClientGuildMember member) => new ClientGuildMemberWrapper(member)));
			this.Permissions = guild.Permissions.Permissions;
		}

		// Token: 0x04003C7D RID: 15485
		public string Name;

		// Token: 0x04003C7E RID: 15486
		public string GuildId;

		// Token: 0x04003C7F RID: 15487
		public ClientGuildMemberWrapper Leader;

		// Token: 0x04003C80 RID: 15488
		public List<ClientGuildMemberWrapper> Officers;

		// Token: 0x04003C81 RID: 15489
		public List<ClientGuildMemberWrapper> Members;

		// Token: 0x04003C82 RID: 15490
		public Dictionary<ClientGuildPermission, ClientGuildRank> Permissions;
	}
}
