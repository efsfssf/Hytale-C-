using System;
using HytaleClient.Data.Entities;
using HytaleClient.Data.Map;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008F8 RID: 2296
	internal class MovementSoundModule : Module
	{
		// Token: 0x06004458 RID: 17496 RVA: 0x000E6D4B File Offset: 0x000E4F4B
		public MovementSoundModule(GameInstance gameInstance) : base(gameInstance)
		{
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x000E6D88 File Offset: 0x000E4F88
		public unsafe void Update(float deltaTime)
		{
			Vector3 position = this._gameInstance.LocalPlayer.Position;
			int num = this._gameInstance.MapModule.GetBlock(position, 1);
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[num];
			bool flag = clientBlockType.FluidBlockId != 0;
			if (flag)
			{
				num = clientBlockType.FluidBlockId;
				clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[num];
			}
			bool flag2 = num != this._previousBlockId;
			if (flag2)
			{
				this._gameInstance.AudioModule.TryPlayLocalBlockSoundEvent(clientBlockType.BlockSoundSetIndex, 1, ref this._moveInPlaybackId);
				ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[this._previousBlockId];
				this._gameInstance.AudioModule.TryPlayLocalBlockSoundEvent(clientBlockType2.BlockSoundSetIndex, 2, ref this._moveOutPlaybackId);
				this._previousBlockId = num;
			}
			ClientMovementStates clientMovementStates = *this._gameInstance.LocalPlayer.GetRelativeMovementStates();
			EntityAnimation currentMovementAnimation = this._gameInstance.LocalPlayer.CurrentMovementAnimation;
			bool flag3 = currentMovementAnimation != null;
			if (flag3)
			{
				bool flag4 = !this._wasFalling && clientMovementStates.IsFalling && !clientMovementStates.IsInFluid;
				bool flag5 = !this._wasFlying && clientMovementStates.IsFlying;
				bool flag6 = this._wasFalling && !clientMovementStates.IsFalling;
				bool flag7 = !this._wasJumping && !clientMovementStates.IsOnGround && clientMovementStates.IsJumping;
				bool flag8 = clientMovementStates.IsInFluid || clientMovementStates.IsSwimming;
				bool flag9 = !this._wasOnGround && clientMovementStates.IsOnGround;
				bool flag10 = this._wasFalling && flag8;
				bool flag11 = currentMovementAnimation.FootstepIntervals.Length != 0 && clientMovementStates.IsOnGround && !flag8 && !clientMovementStates.IsIdle;
				bool flag12 = flag5 || flag10 || flag6;
				if (flag12)
				{
					this.ClearPlayback();
				}
				else
				{
					bool flag13 = flag9;
					if (flag13)
					{
						this.ClearPlayback();
						uint soundEventIndex;
						bool flag14 = !flag8 && this._gameInstance.Engine.Audio.ResourceManager.WwiseEventIds.TryGetValue("SFX_PL_FS_LAND", out soundEventIndex);
						if (flag14)
						{
							this.PlayLocalSoundEventWithBlockType(position, soundEventIndex);
						}
					}
					else
					{
						bool flag15 = flag4 || flag7 || clientMovementStates.IsSwimJumping || (clientMovementStates.IsSwimming && !clientMovementStates.IsOnGround);
						if (flag15)
						{
							this.PlayLocalSoundEvent(currentMovementAnimation.SoundEventIndex);
						}
						else
						{
							bool flag16 = flag11;
							if (flag16)
							{
								this.TickFootsteps(position);
							}
						}
					}
				}
			}
			else
			{
				this.ClearPlayback();
			}
			this._wasFalling = clientMovementStates.IsFalling;
			this._wasFlying = clientMovementStates.IsFlying;
			this._wasJumping = clientMovementStates.IsJumping;
			this._wasOnGround = clientMovementStates.IsOnGround;
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x000E7054 File Offset: 0x000E5254
		private void ClearPlayback()
		{
			bool flag = this._walkPlaybackId == -1;
			if (!flag)
			{
				this._gameInstance.AudioModule.ActionOnEvent(this._walkPlaybackId, 0);
				this._walkPlaybackId = -1;
			}
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x000E7090 File Offset: 0x000E5290
		private void PlayLocalSoundEventWithBlockType(Vector3 blockPosition, uint soundEventIndex)
		{
			blockPosition.Y -= 0.01f;
			int block = this._gameInstance.MapModule.GetBlock(blockPosition, 1);
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
			bool flag = !this._gameInstance.AudioModule.TryPlayLocalBlockSoundEvent(clientBlockType.BlockSoundSetIndex, 0, ref this._walkPlaybackId);
			if (!flag)
			{
				this.PlayLocalSoundEvent(soundEventIndex);
			}
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x000E7104 File Offset: 0x000E5304
		private void PlayLocalSoundEvent(uint soundEventIndex)
		{
			bool flag = soundEventIndex == 0U;
			if (!flag)
			{
				bool flag2 = this._walkPlaybackId != -1;
				if (flag2)
				{
					this._gameInstance.AudioModule.ActionOnEvent(this._walkPlaybackId, 3);
				}
				this._walkPlaybackId = this._gameInstance.AudioModule.PlayLocalSoundEvent(soundEventIndex);
			}
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x000E715C File Offset: 0x000E535C
		private void TickFootsteps(Vector3 blockPosition)
		{
			EntityAnimation currentMovementAnimation = this._gameInstance.LocalPlayer.CurrentMovementAnimation;
			int[] footstepIntervals = currentMovementAnimation.FootstepIntervals;
			float slotAnimationTime = this._gameInstance.LocalPlayer.ModelRenderer.GetSlotAnimationTime(0);
			int num = (int)Math.Floor((double)(slotAnimationTime / (float)currentMovementAnimation.Data.Duration * 100f));
			int nextFootstepIntervalIndex = this._nextFootstepIntervalIndex;
			int num2 = footstepIntervals[nextFootstepIntervalIndex];
			int num3 = (nextFootstepIntervalIndex < footstepIntervals.Length - 1) ? (nextFootstepIntervalIndex + 1) : 0;
			int num4 = footstepIntervals[num3];
			bool flag = num <= num2 || (num >= num4 && num4 >= num2);
			if (!flag)
			{
				this.PlayLocalSoundEventWithBlockType(blockPosition, currentMovementAnimation.SoundEventIndex);
				this._nextFootstepIntervalIndex = num3;
			}
		}

		// Token: 0x040021B7 RID: 8631
		private int _previousBlockId;

		// Token: 0x040021B8 RID: 8632
		private bool _wasFalling = false;

		// Token: 0x040021B9 RID: 8633
		private bool _wasFlying = false;

		// Token: 0x040021BA RID: 8634
		private bool _wasJumping = false;

		// Token: 0x040021BB RID: 8635
		private bool _wasOnGround = true;

		// Token: 0x040021BC RID: 8636
		private int _moveInPlaybackId = -1;

		// Token: 0x040021BD RID: 8637
		private int _moveOutPlaybackId = -1;

		// Token: 0x040021BE RID: 8638
		private int _walkPlaybackId = -1;

		// Token: 0x040021BF RID: 8639
		private int _nextFootstepIntervalIndex;

		// Token: 0x040021C0 RID: 8640
		private const string GroundLandWwiseId = "SFX_PL_FS_LAND";
	}
}
