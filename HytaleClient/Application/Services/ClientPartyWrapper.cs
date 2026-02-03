using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF2 RID: 3058
	public class ClientPartyWrapper
	{
		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x0600615B RID: 24923 RVA: 0x0020017F File Offset: 0x001FE37F
		public string PartyId { get; }

		// Token: 0x0600615C RID: 24924 RVA: 0x00200188 File Offset: 0x001FE388
		public ClientPartyWrapper(ClientParty party)
		{
			this.PartyId = party.PartyId;
			this.Leader = party.Leader.Uuid_;
			this.Members = Enumerable.ToList<Guid>(Enumerable.Select<ClientUser, Guid>(party.Members, (ClientUser member) => member.Uuid_));
			bool flag = !this.Members.Contains(this.Leader);
			if (flag)
			{
				this.Members.Add(this.Leader);
			}
		}

		// Token: 0x04003C87 RID: 15495
		public Guid Leader;

		// Token: 0x04003C88 RID: 15496
		public List<Guid> Members;
	}
}
