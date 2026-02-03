using System;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Items;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Entities
{
	// Token: 0x02000946 RID: 2374
	internal class EntityAnimation
	{
		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x060049C7 RID: 18887 RVA: 0x00126F4A File Offset: 0x0012514A
		public BlockyAnimation Data { get; }

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x00126F52 File Offset: 0x00125152
		public float Speed { get; }

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x060049C9 RID: 18889 RVA: 0x00126F5A File Offset: 0x0012515A
		public float BlendingDuration { get; }

		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x060049CA RID: 18890 RVA: 0x00126F62 File Offset: 0x00125162
		public bool Looping { get; }

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x060049CB RID: 18891 RVA: 0x00126F6A File Offset: 0x0012516A
		public bool KeepPreviousFirstPersonAnimation { get; }

		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x060049CC RID: 18892 RVA: 0x00126F72 File Offset: 0x00125172
		public uint SoundEventIndex { get; }

		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x060049CD RID: 18893 RVA: 0x00126F7A File Offset: 0x0012517A
		public BlockyAnimation MovingData { get; }

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x060049CE RID: 18894 RVA: 0x00126F82 File Offset: 0x00125182
		public BlockyAnimation FaceData { get; }

		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x060049CF RID: 18895 RVA: 0x00126F8A File Offset: 0x0012518A
		public BlockyAnimation FirstPersonData { get; }

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x060049D0 RID: 18896 RVA: 0x00126F92 File Offset: 0x00125192
		public BlockyAnimation FirstPersonOverrideData { get; }

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x060049D1 RID: 18897 RVA: 0x00126F9A File Offset: 0x0012519A
		public bool ClipsGeometry { get; }

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x060049D2 RID: 18898 RVA: 0x00126FA2 File Offset: 0x001251A2
		public float Weight { get; }

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x060049D3 RID: 18899 RVA: 0x00126FAA File Offset: 0x001251AA
		public int[] FootstepIntervals { get; }

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x060049D4 RID: 18900 RVA: 0x00126FB2 File Offset: 0x001251B2
		public int PassiveLoopCount { get; }

		// Token: 0x060049D5 RID: 18901 RVA: 0x00126FBC File Offset: 0x001251BC
		public EntityAnimation(BlockyAnimation data, float speed, float blendingDuration, bool looping, bool keepPreviousFirstPersonAnimation, uint soundEventIndex, float weight, int[] footstepIntervals, int passiveLoopCount, BlockyAnimation movingData = null, BlockyAnimation faceData = null, BlockyAnimation firstPersonData = null, BlockyAnimation firstPersonOverrideData = null, ItemPullbackConfiguration pullbackConfig = null, bool clipsGeometry = false)
		{
			this.Data = data;
			this.Speed = speed;
			this.BlendingDuration = blendingDuration;
			this.Looping = looping;
			this.KeepPreviousFirstPersonAnimation = keepPreviousFirstPersonAnimation;
			this.SoundEventIndex = soundEventIndex;
			this.MovingData = (movingData ?? data);
			this.FaceData = faceData;
			this.FirstPersonData = firstPersonData;
			this.FirstPersonOverrideData = firstPersonOverrideData;
			this.Weight = weight;
			this.FootstepIntervals = footstepIntervals;
			this.PassiveLoopCount = passiveLoopCount;
			bool flag = pullbackConfig != null;
			if (flag)
			{
				this.PullbackConfig = new ClientItemPullbackConfig(pullbackConfig);
			}
			this.ClipsGeometry = clipsGeometry;
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x0012705C File Offset: 0x0012525C
		public EntityAnimation(EntityAnimation animation)
		{
			this.Data = animation.Data;
			this.Speed = animation.Speed;
			this.BlendingDuration = animation.BlendingDuration;
			this.Looping = animation.Looping;
			this.KeepPreviousFirstPersonAnimation = animation.KeepPreviousFirstPersonAnimation;
			this.SoundEventIndex = animation.SoundEventIndex;
			this.MovingData = animation.MovingData;
			this.FaceData = animation.FaceData;
			this.FirstPersonData = animation.FirstPersonData;
			this.FirstPersonOverrideData = animation.FirstPersonOverrideData;
			this.Weight = animation.Weight;
			this.FootstepIntervals = animation.FootstepIntervals;
			this.PassiveLoopCount = animation.PassiveLoopCount;
			this.PullbackConfig = animation.PullbackConfig;
		}

		// Token: 0x040025A1 RID: 9633
		public static readonly EntityAnimation Empty = new EntityAnimation(null, 1f, 12f, false, false, 0U, 0f, Array.Empty<int>(), 0, null, null, null, null, null, false);

		// Token: 0x040025A2 RID: 9634
		public const float DefaultBlendingDuration = 12f;

		// Token: 0x040025AD RID: 9645
		public ClientItemPullbackConfig PullbackConfig;

		// Token: 0x02000E3B RID: 3643
		public static class AnimationSlot
		{
			// Token: 0x06006731 RID: 26417 RVA: 0x00217210 File Offset: 0x00215410
			public static bool GetSlot(HytaleClient.Protocol.AnimationSlot slot, out int slotId)
			{
				bool result;
				switch (slot)
				{
				case 0:
					slotId = 0;
					result = true;
					break;
				case 1:
					slotId = 4;
					result = true;
					break;
				case 2:
					slotId = 6;
					result = true;
					break;
				case 3:
					slotId = 7;
					result = true;
					break;
				case 4:
					slotId = 8;
					result = true;
					break;
				default:
					slotId = -1;
					result = false;
					break;
				}
				return result;
			}

			// Token: 0x040045A9 RID: 17833
			public const int Movement = 0;

			// Token: 0x040045AA RID: 17834
			public const int PrimaryItemMovement = 1;

			// Token: 0x040045AB RID: 17835
			public const int SecondaryItemMovement = 2;

			// Token: 0x040045AC RID: 17836
			public const int Passive = 3;

			// Token: 0x040045AD RID: 17837
			public const int Status = 4;

			// Token: 0x040045AE RID: 17838
			public const int Action = 5;

			// Token: 0x040045AF RID: 17839
			public const int ServerAction = 6;

			// Token: 0x040045B0 RID: 17840
			public const int Face = 7;

			// Token: 0x040045B1 RID: 17841
			public const int Emote = 8;

			// Token: 0x040045B2 RID: 17842
			public const int SlotCount = 9;
		}
	}
}
