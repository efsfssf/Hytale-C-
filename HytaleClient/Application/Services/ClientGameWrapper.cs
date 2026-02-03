using System;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BED RID: 3053
	public class ClientGameWrapper
	{
		// Token: 0x06006154 RID: 24916 RVA: 0x001FFF80 File Offset: 0x001FE180
		public ClientGameWrapper(ClientGame game)
		{
			this.NameKey = game.NameKey;
			this.DefaultName = game.DefaultName;
			this.PlayerMin = game.PlayerMin;
			this.PlayerMax = game.PlayerMax;
			this.JoinKey = game.JoinKey;
			this.ImageUrl = game.ImageUrl;
			this.Featured = game.Featured;
			this.Display = game.Display;
		}

		// Token: 0x04003C6F RID: 15471
		public readonly string NameKey;

		// Token: 0x04003C70 RID: 15472
		public readonly string DefaultName;

		// Token: 0x04003C71 RID: 15473
		public readonly int PlayerMin;

		// Token: 0x04003C72 RID: 15474
		public readonly int PlayerMax;

		// Token: 0x04003C73 RID: 15475
		public readonly string JoinKey;

		// Token: 0x04003C74 RID: 15476
		public readonly string ImageUrl;

		// Token: 0x04003C75 RID: 15477
		public readonly bool Featured;

		// Token: 0x04003C76 RID: 15478
		public readonly bool Display;
	}
}
