using System;

namespace HytaleClient.Data.Audio
{
	// Token: 0x02000B76 RID: 2934
	internal struct WwiseResource
	{
		// Token: 0x06005A13 RID: 23059 RVA: 0x001BF9E5 File Offset: 0x001BDBE5
		public WwiseResource(WwiseResource.WwiseResourceType type, uint id)
		{
			this.Type = type;
			this.Id = id;
		}

		// Token: 0x04003851 RID: 14417
		public WwiseResource.WwiseResourceType Type;

		// Token: 0x04003852 RID: 14418
		public uint Id;

		// Token: 0x02000F5D RID: 3933
		public enum WwiseResourceType : byte
		{
			// Token: 0x04004AC2 RID: 19138
			Event,
			// Token: 0x04004AC3 RID: 19139
			GameParameter
		}
	}
}
