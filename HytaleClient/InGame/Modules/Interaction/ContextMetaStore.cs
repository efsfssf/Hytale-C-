using System;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x02000934 RID: 2356
	internal class ContextMetaStore
	{
		// Token: 0x060047F5 RID: 18421 RVA: 0x001119D4 File Offset: 0x0010FBD4
		public void CopyFrom(ContextMetaStore other)
		{
			this.TargetEntity = other.TargetEntity;
			this.HitLocation = other.HitLocation;
			this.HitDetail = other.HitDetail;
			this.TargetBlock = other.TargetBlock;
			this.TargetBlockRaw = other.TargetBlockRaw;
			this.TargetSlot = other.TargetSlot;
			this.DisableSlotFork = other.DisableSlotFork;
			this.PlaceBlockPrediction = other.PlaceBlockPrediction;
		}

		// Token: 0x0400243A RID: 9274
		public Entity TargetEntity;

		// Token: 0x0400243B RID: 9275
		public Vector4? HitLocation;

		// Token: 0x0400243C RID: 9276
		public string HitDetail;

		// Token: 0x0400243D RID: 9277
		public BlockPosition TargetBlock;

		// Token: 0x0400243E RID: 9278
		public BlockPosition TargetBlockRaw;

		// Token: 0x0400243F RID: 9279
		public int? TargetSlot;

		// Token: 0x04002440 RID: 9280
		public bool DisableSlotFork;

		// Token: 0x04002441 RID: 9281
		public bool? PlaceBlockPrediction;

		// Token: 0x04002442 RID: 9282
		public InteractionMetaStore SelectMetaStore;
	}
}
