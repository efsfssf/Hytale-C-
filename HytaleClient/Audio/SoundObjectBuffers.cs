using System;
using HytaleClient.Math;

namespace HytaleClient.Audio
{
	// Token: 0x02000B84 RID: 2948
	internal struct SoundObjectBuffers
	{
		// Token: 0x06005ACE RID: 23246 RVA: 0x001C4F58 File Offset: 0x001C3158
		public SoundObjectBuffers(int maxSoundObjects)
		{
			this.Count = maxSoundObjects;
			this.SoundObjectId = new uint[maxSoundObjects];
			this.Position = new Vector3[maxSoundObjects];
			this.FrontOrientation = new Vector3[maxSoundObjects];
			this.TopOrientation = new Vector3[maxSoundObjects];
			this.LastPlaybackId = new int[maxSoundObjects];
			this.BoolData = new byte[maxSoundObjects];
		}

		// Token: 0x040038CE RID: 14542
		public const int HasSingleEventBitId = 0;

		// Token: 0x040038CF RID: 14543
		public const int HasUniqueEventBitId = 1;

		// Token: 0x040038D0 RID: 14544
		public const int IsLiveBitId = 2;

		// Token: 0x040038D1 RID: 14545
		public int Count;

		// Token: 0x040038D2 RID: 14546
		public uint[] SoundObjectId;

		// Token: 0x040038D3 RID: 14547
		public Vector3[] Position;

		// Token: 0x040038D4 RID: 14548
		public Vector3[] FrontOrientation;

		// Token: 0x040038D5 RID: 14549
		public Vector3[] TopOrientation;

		// Token: 0x040038D6 RID: 14550
		public int[] LastPlaybackId;

		// Token: 0x040038D7 RID: 14551
		public byte[] BoolData;
	}
}
