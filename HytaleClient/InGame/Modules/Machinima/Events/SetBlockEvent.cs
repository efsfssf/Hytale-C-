using System;
using HytaleClient.InGame.Modules.Machinima.Track;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Events
{
	// Token: 0x02000924 RID: 2340
	internal class SetBlockEvent : KeyframeEvent
	{
		// Token: 0x06004745 RID: 18245 RVA: 0x0010D611 File Offset: 0x0010B811
		public SetBlockEvent(int x, int y, int z, ushort blockid)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.BlockId = blockid;
			base.AllowDuplicates = true;
			base.Initialized = true;
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x0010D648 File Offset: 0x0010B848
		public override void Execute(GameInstance gameInstance, SceneTrack track)
		{
			gameInstance.MapModule.SetClientBlock(this.X, this.Y, this.Z, (int)this.BlockId);
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x0010D670 File Offset: 0x0010B870
		public override string ToString()
		{
			return string.Format("#{0} - SetBlockEvent: Block {1} @ [{2}, {3}, {4}]", new object[]
			{
				this.Id,
				this.BlockId,
				this.X,
				this.Y,
				this.Z
			});
		}

		// Token: 0x040023D5 RID: 9173
		[JsonProperty("SetBlock")]
		public readonly int X;

		// Token: 0x040023D6 RID: 9174
		public readonly int Y;

		// Token: 0x040023D7 RID: 9175
		public readonly int Z;

		// Token: 0x040023D8 RID: 9176
		public readonly ushort BlockId;
	}
}
