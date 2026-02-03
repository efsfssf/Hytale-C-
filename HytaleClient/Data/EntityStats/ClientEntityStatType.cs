using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.EntityStats
{
	// Token: 0x02000B08 RID: 2824
	public struct ClientEntityStatType
	{
		// Token: 0x06005881 RID: 22657 RVA: 0x001B00F8 File Offset: 0x001AE2F8
		public ClientEntityStatType(EntityStatType entityStatType)
		{
			this.Id = entityStatType.Id;
			this.Value = entityStatType.Value;
			this.Min = entityStatType.Min;
			this.Max = entityStatType.Max;
			this.MinValueEffects = entityStatType.MinValueEffects;
			this.MaxValueEffects = entityStatType.MaxValueEffects;
		}

		// Token: 0x040036F5 RID: 14069
		public string Id;

		// Token: 0x040036F6 RID: 14070
		public float Value;

		// Token: 0x040036F7 RID: 14071
		public float Min;

		// Token: 0x040036F8 RID: 14072
		public float Max;

		// Token: 0x040036F9 RID: 14073
		public EntityStatEffects MinValueEffects;

		// Token: 0x040036FA RID: 14074
		public EntityStatEffects MaxValueEffects;
	}
}
