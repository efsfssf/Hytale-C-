using System;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF3 RID: 3059
	public class ClientSharedSinglePlayerJoinableWorldWrapper
	{
		// Token: 0x0600615D RID: 24925 RVA: 0x00200219 File Offset: 0x001FE419
		public ClientSharedSinglePlayerJoinableWorldWrapper(ClientSSPJoinableWorld world)
		{
			this.WorldId = world.WorldId;
			this.Owner = world.Owner;
			this.Name = world.Name;
		}

		// Token: 0x04003C89 RID: 15497
		public readonly Guid WorldId;

		// Token: 0x04003C8A RID: 15498
		public readonly Guid Owner;

		// Token: 0x04003C8B RID: 15499
		public readonly bool Hidden = false;

		// Token: 0x04003C8C RID: 15500
		public readonly string Name;
	}
}
