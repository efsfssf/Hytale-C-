using System;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Machinima
{
	// Token: 0x02000910 RID: 2320
	internal struct SerializedSceneObject
	{
		// Token: 0x0400238E RID: 9102
		public string Name;

		// Token: 0x0400238F RID: 9103
		public int Type;

		// Token: 0x04002390 RID: 9104
		public SceneTrack Track;

		// Token: 0x04002391 RID: 9105
		public Model Model;

		// Token: 0x04002392 RID: 9106
		public string ModelId;

		// Token: 0x04002393 RID: 9107
		public string ItemId;

		// Token: 0x04002394 RID: 9108
		public float Scale;
	}
}
