using System;
using System.Collections.Generic;
using HytaleClient.Audio;
using HytaleClient.Data.ClientInteraction.Selector;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.Entities.Projectile;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x02000936 RID: 2358
	internal class InteractionMetaStore
	{
		// Token: 0x04002449 RID: 9289
		public float? TimeShift;

		// Token: 0x0400244A RID: 9290
		public AudioDevice.SoundEventReference SoundEventReference = AudioDevice.SoundEventReference.None;

		// Token: 0x0400244B RID: 9291
		public float LastFirstPersonCameraTime = 0f;

		// Token: 0x0400244C RID: 9292
		public float LastThirdPersonCameraTime = 0f;

		// Token: 0x0400244D RID: 9293
		public int OldBlockId = int.MaxValue;

		// Token: 0x0400244E RID: 9294
		public int ExpectedBlockId = int.MaxValue;

		// Token: 0x0400244F RID: 9295
		public int PrimaryChargingLastProgress;

		// Token: 0x04002450 RID: 9296
		public bool ChargingVisible = false;

		// Token: 0x04002451 RID: 9297
		public float TotalDelay = 0f;

		// Token: 0x04002452 RID: 9298
		public Selector EntitySelector;

		// Token: 0x04002453 RID: 9299
		public HashSet<int> HitEntities;

		// Token: 0x04002454 RID: 9300
		public List<SelectedHitEntity> RecordedHits;

		// Token: 0x04002455 RID: 9301
		public InteractionChain ForkedChain;

		// Token: 0x04002456 RID: 9302
		public int RemainingRepeats;

		// Token: 0x04002457 RID: 9303
		public int? OriginalSlot;

		// Token: 0x04002458 RID: 9304
		public EntityEffectUpdate PredictedEffect;

		// Token: 0x04002459 RID: 9305
		public int Sequence;

		// Token: 0x0400245A RID: 9306
		public EntityStatUpdate[] PredictedStats;

		// Token: 0x0400245B RID: 9307
		public AudioDevice.SoundEventReference DamageSoundEventReference = AudioDevice.SoundEventReference.None;

		// Token: 0x0400245C RID: 9308
		public ParticleSystemProxy[] DamageParticles;

		// Token: 0x0400245D RID: 9309
		public PredictedProjectile Projectile;
	}
}
