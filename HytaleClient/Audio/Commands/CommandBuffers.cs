using System;
using System.Runtime.InteropServices;
using HytaleClient.Math;

namespace HytaleClient.Audio.Commands
{
	// Token: 0x02000B87 RID: 2951
	internal struct CommandBuffers
	{
		// Token: 0x06005AD6 RID: 23254 RVA: 0x001C504E File Offset: 0x001C324E
		public CommandBuffers(int maxCommands)
		{
			this.Count = maxCommands;
			this.Data = new CommandBuffers.CommandData[maxCommands];
		}

		// Token: 0x040038E6 RID: 14566
		public const int HasSingleEventBitId = 0;

		// Token: 0x040038E7 RID: 14567
		public const int HasUniqueEventBitId = 1;

		// Token: 0x040038E8 RID: 14568
		public int Count;

		// Token: 0x040038E9 RID: 14569
		public CommandBuffers.CommandData[] Data;

		// Token: 0x02000F6F RID: 3951
		[StructLayout(LayoutKind.Explicit)]
		public struct CommandData
		{
			// Token: 0x04004AF9 RID: 19193
			[FieldOffset(0)]
			public CommandType Type;

			// Token: 0x04004AFA RID: 19194
			[FieldOffset(1)]
			public AudioDevice.SoundObjectReference SoundObjectReference;

			// Token: 0x04004AFB RID: 19195
			[FieldOffset(9)]
			public uint EventId;

			// Token: 0x04004AFC RID: 19196
			[FieldOffset(13)]
			public int PlaybackId;

			// Token: 0x04004AFD RID: 19197
			[FieldOffset(9)]
			public Vector3 WorldPosition;

			// Token: 0x04004AFE RID: 19198
			[FieldOffset(21)]
			public Vector3 WorldOrientation;

			// Token: 0x04004AFF RID: 19199
			[FieldOffset(33)]
			public byte BoolData;

			// Token: 0x04004B00 RID: 19200
			[FieldOffset(1)]
			public int TransitionDuration;

			// Token: 0x04004B01 RID: 19201
			[FieldOffset(5)]
			public byte FadeCurveType;

			// Token: 0x04004B02 RID: 19202
			[FieldOffset(6)]
			public byte ActionType;

			// Token: 0x04004B03 RID: 19203
			[FieldOffset(1)]
			public float Volume;

			// Token: 0x04004B04 RID: 19204
			[FieldOffset(5)]
			public uint RTPCId;
		}
	}
}
