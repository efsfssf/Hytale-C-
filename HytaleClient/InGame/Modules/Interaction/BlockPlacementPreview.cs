using System;
using HytaleClient.Data.Map;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x02000933 RID: 2355
	internal class BlockPlacementPreview
	{
		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x060047E5 RID: 18405 RVA: 0x001112C0 File Offset: 0x0010F4C0
		public bool HasSupport
		{
			get
			{
				return this._hasSupport;
			}
		}

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x060047E6 RID: 18406 RVA: 0x001112D8 File Offset: 0x0010F4D8
		// (set) Token: 0x060047E7 RID: 18407 RVA: 0x001112E0 File Offset: 0x0010F4E0
		public bool HasValidPosition { get; private set; }

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x060047E8 RID: 18408 RVA: 0x001112E9 File Offset: 0x0010F4E9
		public bool UseDithering
		{
			get
			{
				return this._entity.UseDithering;
			}
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x060047E9 RID: 18409 RVA: 0x001112F8 File Offset: 0x0010F4F8
		public BlockPlacementPreview.DisplayMode _displayMode
		{
			get
			{
				bool flag = this._gameInstance.InteractionModule.CurrentRotationMode != InteractionModule.RotationMode.None || (this._gameInstance.Input.IsAltHeld() && !this._gameInstance.InteractionModule.FluidityActive);
				BlockPlacementPreview.DisplayMode result;
				if (flag)
				{
					result = BlockPlacementPreview.DisplayMode.All;
				}
				else
				{
					result = this._gameInstance.App.Settings.PlacementPreviewMode;
				}
				return result;
			}
		}

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x060047EA RID: 18410 RVA: 0x00111366 File Offset: 0x0010F566
		public bool IsEnabled
		{
			get
			{
				return this._displayMode > BlockPlacementPreview.DisplayMode.None;
			}
		}

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x060047EB RID: 18411 RVA: 0x00111374 File Offset: 0x0010F574
		// (set) Token: 0x060047EC RID: 18412 RVA: 0x00111398 File Offset: 0x0010F598
		public bool IsVisible
		{
			get
			{
				Entity entity = this._entity;
				return entity != null && entity.IsVisible;
			}
			set
			{
				bool flag = this._entity != null;
				if (flag)
				{
					this._entity.IsVisible = value;
				}
			}
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x001113C0 File Offset: 0x0010F5C0
		public BlockPlacementPreview(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			this._gameInstance.EntityStoreModule.Spawn(-1, out this._entity);
			this._entity.SetIsTangible(false);
			this._entity.UseDithering = true;
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x00111434 File Offset: 0x0010F634
		public void CheckSupportValidation()
		{
			this._gameInstance.Connection.SendPacket(new SupportValidationCheck(new BlockPosition(this.BlockPosition.X, this.BlockPosition.Y, this.BlockPosition.Z), this.BlockId));
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x00111484 File Offset: 0x0010F684
		public void HandleSupportValidationResponse(SupportValidationResponse response)
		{
			IntVector3 value = new IntVector3(response.BlockPosition_.X, response.BlockPosition_.Y, response.BlockPosition_.Z);
			bool flag = this.BlockId == response.BlockId && this.BlockPosition == value;
			if (flag)
			{
				this._hasSupport = response.Valid;
				this.UpdateEffect();
			}
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x001114F0 File Offset: 0x0010F6F0
		public void UpdateEffect()
		{
			int num = -1;
			bool flag = !this._gameInstance.InteractionModule.HeldBlockCanBePlaced;
			if (flag)
			{
				this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds.TryGetValue("PrototypeBlockPlaceFail", out num);
				this.HasValidPosition = false;
			}
			else
			{
				bool flag2 = this._gameInstance.InteractionModule.CurrentRotationMode != InteractionModule.RotationMode.None;
				if (flag2)
				{
					this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds.TryGetValue("PrototypeBlockPlaceRotated", out num);
					this.HasValidPosition = true;
				}
				else
				{
					this._gameInstance.EntityStoreModule.EntityEffectIndicesByIds.TryGetValue("PrototypeBlockPlaceSuccess", out num);
					this.HasValidPosition = true;
				}
			}
			bool flag3 = num != this._currentEntityEffect || !this._entity.HasEffect(num);
			if (flag3)
			{
				this._entity.ClearEffects();
				bool flag4 = num != -1;
				if (flag4)
				{
					this._entity.AddEffect(num, null, null, null);
				}
				this._currentEntityEffect = num;
			}
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x00111618 File Offset: 0x0010F818
		public void EnableDithering(bool enable)
		{
			this._entity.UseDithering = enable;
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x00111628 File Offset: 0x0010F828
		public void UpdatePreview(int blockId, int worldX, int worldY, int worldZ)
		{
			bool flag = false;
			bool blockPlacementSupportValidation = this._gameInstance.App.Settings.BlockPlacementSupportValidation;
			if (blockPlacementSupportValidation)
			{
				bool flag2 = this._previousBlockPosition != this.BlockPosition || blockId != this.BlockId;
				if (flag2)
				{
					flag = true;
					this.BlockId = blockId;
					this.CheckSupportValidation();
				}
			}
			else
			{
				this._hasSupport = true;
			}
			bool flag3 = !this.IsEnabled || blockId == -1;
			if (flag3)
			{
				this.IsVisible = false;
				this._currentEntityEffect = -1;
			}
			else
			{
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[blockId];
				BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
				this.BlockPosition = new IntVector3(worldX, worldY, worldZ);
				bool flag4 = this._displayMode == BlockPlacementPreview.DisplayMode.Multipart && !blockHitbox.IsOversized();
				if (flag4)
				{
					this.IsVisible = false;
					this._currentEntityEffect = -1;
				}
				else
				{
					bool flag5 = blockId != this.BlockId || flag;
					if (flag5)
					{
						this.BlockId = blockId;
						int num = (clientBlockType.RotationYaw == 1) ? 90 : ((clientBlockType.RotationYaw == 2) ? 180 : ((clientBlockType.RotationYaw == 3) ? 270 : 0));
						int num2 = (clientBlockType.RotationPitch == 1) ? 90 : ((clientBlockType.RotationPitch == 2) ? 180 : ((clientBlockType.RotationPitch == 3) ? 270 : 0));
						int num3 = 0;
						this._offset = BlockPlacementPreview._baseOffset;
						bool flag6 = num2 == 90;
						if (flag6)
						{
							int num4 = num;
							int num5 = num4;
							if (num5 <= 90)
							{
								if (num5 != 0)
								{
									if (num5 == 90)
									{
										this._offset.X = this._offset.X - 0.5f;
									}
								}
								else
								{
									this._offset.Z = this._offset.Z - 0.5f;
								}
							}
							else if (num5 != 180)
							{
								if (num5 == 270)
								{
									this._offset.X = this._offset.X + 0.5f;
								}
							}
							else
							{
								this._offset.Z = this._offset.Z + 0.5f;
							}
							this._offset.Y = this._offset.Y + 0.5f;
							num += 90;
							num3 = -90;
						}
						else
						{
							bool flag7 = num2 == 180;
							if (flag7)
							{
								this._offset += Vector3.Up;
							}
							else
							{
								num = (num + 180) % 360;
							}
						}
						Console.WriteLine(string.Format("X: {0}  Y: {1}  Z: {2}", num3, num, num2));
						this._entity.SetBlock(clientBlockType.Id);
						this._entity.LookOrientation = new Vector3(MathHelper.ToRadians((float)num3), MathHelper.ToRadians((float)num), MathHelper.ToRadians((float)num2));
					}
					this._entity.SetPosition(new Vector3((float)worldX + this._offset.X, (float)worldY + this._offset.Y, (float)worldZ + this._offset.Z));
					this._entity.PositionProgress = 1f;
					bool flag8 = this._previousBlockPosition != this.BlockPosition || flag;
					if (flag8)
					{
						this.UpdateEffect();
					}
					this._previousBlockPosition = this.BlockPosition;
					this.IsVisible = true;
				}
			}
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0011198C File Offset: 0x0010FB8C
		public void RegisterSoundObjectReference()
		{
			this._gameInstance.AudioModule.TryRegisterSoundObject(Vector3.Zero, Vector3.Zero, ref this._entity.SoundObjectReference, false);
		}

		// Token: 0x04002430 RID: 9264
		private readonly GameInstance _gameInstance;

		// Token: 0x04002431 RID: 9265
		private static readonly Vector3 _baseOffset = new Vector3(0.5f, 0f, 0.5f);

		// Token: 0x04002432 RID: 9266
		private readonly Entity _entity;

		// Token: 0x04002433 RID: 9267
		private Vector3 _offset;

		// Token: 0x04002434 RID: 9268
		private IntVector3 _previousBlockPosition = IntVector3.Zero;

		// Token: 0x04002435 RID: 9269
		private int _currentEntityEffect = -1;

		// Token: 0x04002436 RID: 9270
		private bool _hasSupport = true;

		// Token: 0x04002438 RID: 9272
		public int BlockId;

		// Token: 0x04002439 RID: 9273
		public IntVector3 BlockPosition = IntVector3.Zero;

		// Token: 0x02000E0B RID: 3595
		public enum DisplayMode
		{
			// Token: 0x040044FB RID: 17659
			None,
			// Token: 0x040044FC RID: 17660
			All,
			// Token: 0x040044FD RID: 17661
			Multipart
		}
	}
}
