using System;

namespace HytaleClient.Audio
{
	// Token: 0x02000B81 RID: 2945
	internal struct AudioCategoryState
	{
		// Token: 0x06005AA0 RID: 23200 RVA: 0x001C3547 File Offset: 0x001C1747
		public AudioCategoryState(int category, string rtpcName, float volume)
		{
			this.Category = category;
			this.RtpcName = rtpcName;
			this.Volume = volume;
		}

		// Token: 0x0400389C RID: 14492
		public readonly int Category;

		// Token: 0x0400389D RID: 14493
		public readonly string RtpcName;

		// Token: 0x0400389E RID: 14494
		public float Volume;
	}
}
