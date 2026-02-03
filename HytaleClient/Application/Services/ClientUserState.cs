using System;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF4 RID: 3060
	public class ClientUserState
	{
		// Token: 0x0600615E RID: 24926 RVA: 0x0020024E File Offset: 0x001FE44E
		public ClientUserState(ClientPlayerState state)
		{
			this.Online = state.Online;
			this.InParty = state.InParty;
		}

		// Token: 0x04003C8D RID: 15501
		public bool Online;

		// Token: 0x04003C8E RID: 15502
		public bool InParty;
	}
}
