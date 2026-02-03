using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Hypixel.ProtoPlus;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.ClientInteraction.Client;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Gizmos;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x02000937 RID: 2359
	internal class InteractionModule : Module
	{
		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x0600480B RID: 18443 RVA: 0x00111E14 File Offset: 0x00110014
		// (set) Token: 0x0600480C RID: 18444 RVA: 0x00111E1C File Offset: 0x0011001C
		public bool HasFoundTargetBlock { get; private set; }

		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x0600480D RID: 18445 RVA: 0x00111E25 File Offset: 0x00110025
		// (set) Token: 0x0600480E RID: 18446 RVA: 0x00111E2D File Offset: 0x0011002D
		public HitDetection.RaycastHit TargetBlockHit { get; private set; }

		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x0600480F RID: 18447 RVA: 0x00111E36 File Offset: 0x00110036
		// (set) Token: 0x06004810 RID: 18448 RVA: 0x00111E3E File Offset: 0x0011003E
		public Entity TargetEntityHit { get; private set; }

		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x06004811 RID: 18449 RVA: 0x00111E48 File Offset: 0x00110048
		public bool PlacingAtRange
		{
			get
			{
				return this._placingAtRange;
			}
		}

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x06004812 RID: 18450 RVA: 0x00111E60 File Offset: 0x00110060
		// (set) Token: 0x06004813 RID: 18451 RVA: 0x00111E78 File Offset: 0x00110078
		public bool FluidityActive
		{
			get
			{
				return this._fluidityActive;
			}
			set
			{
				this._fluidityActive = value;
			}
		}

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x06004814 RID: 18452 RVA: 0x00111E82 File Offset: 0x00110082
		private bool IsTargetPositionValid
		{
			get
			{
				return this._targetBlockInfo != null && this._targetBlockInfo.GetValueOrDefault().Valid;
			}
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x06004815 RID: 18453 RVA: 0x00111EA0 File Offset: 0x001100A0
		public bool HeldBlockCanBePlaced
		{
			get
			{
				return this.IsTargetPositionValid && (!this.BlockPreview.IsEnabled || !this._gameInstance.App.Settings.BlockPlacementSupportValidation || this.BlockPreview.HasSupport);
			}
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x00111EF0 File Offset: 0x001100F0
		private void UpdateInteractionTarget()
		{
			this._placingAtRange = this._gameInstance.Input.IsAltHeld();
			ClientItemBase primaryItem = this._gameInstance.LocalPlayer.PrimaryItem;
			ItemBase.InteractionConfiguration interactionConfiguration = (primaryItem != null) ? primaryItem.InteractionConfiguration : null;
			bool flag = this._gameInstance.GameMode.Equals(0);
			float num;
			if (flag)
			{
				bool placingAtRange = this._placingAtRange;
				if (placingAtRange)
				{
					num = (float)this._gameInstance.App.Settings.CurrentAdventureInteractionDistance;
				}
				else
				{
					num = (float)this._gameInstance.App.Settings.adventureInteractionDistance;
				}
			}
			else
			{
				bool placingAtRange2 = this._placingAtRange;
				if (placingAtRange2)
				{
					num = (float)this._gameInstance.App.Settings.CurrentCreativeInteractionDistance;
				}
				else
				{
					num = (float)this._gameInstance.App.Settings.creativeInteractionDistance;
				}
			}
			bool flag2 = !this._placingAtRange;
			if (flag2)
			{
				if (interactionConfiguration != null)
				{
					Dictionary<GameMode, float> useDistance = interactionConfiguration.UseDistance;
					if (useDistance != null)
					{
						useDistance.TryGetValue(this._gameInstance.GameMode, out num);
					}
				}
			}
			bool interactFromEntity = this._gameInstance.CameraModule.Controller.InteractFromEntity;
			this._targetBlockRaycastOptions.Distance = (interactFromEntity ? (num + this._gameInstance.CameraModule.Controller.PositionOffset.Length()) : num);
			this._targetBlockRaycastOptions.CheckOnlyTangibleEntities = (interactionConfiguration == null || !interactionConfiguration.AllEntities);
			this._targetBlockRaycastOptions.ReturnEndpointBlock = this._placingAtRange;
			Ray lookRay = this._gameInstance.CameraModule.GetLookRay();
			bool flag3;
			HitDetection.RaycastHit targetBlockHit;
			bool flag4;
			HitDetection.EntityHitData entityHitData;
			this._gameInstance.HitDetection.Raycast(lookRay.Position, lookRay.Direction, this._targetBlockRaycastOptions, out flag3, out targetBlockHit, out flag4, out entityHitData);
			bool flag5 = this._placingAtRange && flag3 && this._gameInstance.App.Settings.InteractionDistanceIsMinimum(this._gameInstance.GameMode);
			if (flag5)
			{
				Vector3 vector = Vector3.Floor(this._gameInstance.LocalPlayer.Position);
				Vector3 vector2 = new Vector3(targetBlockHit.BlockPosition.X, vector.Y - 1f, targetBlockHit.BlockPosition.Z);
				int block = this._gameInstance.MapModule.GetBlock((int)vector2.X, (int)vector2.Y, (int)vector2.Z, 1);
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				int boxId = 0;
				targetBlockHit = new HitDetection.RaycastHit(vector2, vector2, vector2, vector2, targetBlockHit.HitPosition, Vector3.UnitY, Vector3.UnitY, targetBlockHit.TextureCoord, targetBlockHit.Distance, block, clientBlockType.HitboxType, boxId);
			}
			this.HasFoundTargetBlock = flag3;
			HitDetection.RaycastHit targetBlockHit2 = this.TargetBlockHit;
			this.TargetBlockHit = targetBlockHit;
			this.TargetEntityHit = entityHitData.Entity;
			this.InteractionTarget = InteractionModule.InteractionTargetType.None;
			InteractionModule.InteractionHintData none = InteractionModule.InteractionHintData.None;
			bool flag6 = this.InteractionTarget == InteractionModule.InteractionTargetType.None && this.TargetEntityHit != null;
			if (flag6)
			{
				bool flag7 = true;
				bool flag8 = interactFromEntity;
				if (flag8)
				{
					float num2 = (this._gameInstance.CameraModule.Controller.AttachmentPosition - entityHitData.RayBoxCollision.Position).Length();
					flag7 = (num2 < num);
				}
				bool flag9 = flag7;
				if (flag9)
				{
					this.InteractionTarget = InteractionModule.InteractionTargetType.Entity;
					bool flag10 = this.TargetEntityHit.IsUsable();
					if (flag10)
					{
						none = new InteractionModule.InteractionHintData(InteractionModule.InteractionTargetType.Entity, this.TargetEntityHit.Name, "interactionHints.generic");
					}
				}
			}
			else
			{
				bool flag11 = this.InteractionTarget == InteractionModule.InteractionTargetType.None && this.HasFoundTargetBlock;
				if (flag11)
				{
					bool flag12 = true;
					bool flag13 = interactFromEntity;
					if (flag13)
					{
						float num3 = (this._gameInstance.CameraModule.Controller.AttachmentPosition - this.TargetBlockHit.HitPosition).Length();
						flag12 = (this._placingAtRange || num3 < num);
					}
					bool flag14 = flag12;
					if (flag14)
					{
						this.InteractionTarget = InteractionModule.InteractionTargetType.Block;
						bool isUsable = this._gameInstance.MapModule.ClientBlockTypes[this.TargetBlockHit.BlockId].IsUsable;
						if (isUsable)
						{
							none = new InteractionModule.InteractionHintData(InteractionModule.InteractionTargetType.Block, this._gameInstance.MapModule.ClientBlockTypes[this.TargetBlockHit.BlockId].Item, this._gameInstance.MapModule.ClientBlockTypes[this.TargetBlockHit.BlockId].InteractionHint ?? "interactionHints.generic");
						}
					}
				}
			}
			bool flag15 = !none.Equals(this._interactionHint);
			if (flag15)
			{
				this._gameInstance.App.Interface.TriggerEvent("crosshair.setInteractionHint", none, null, null, null, null, null);
				this._interactionHint = none;
			}
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x001123E0 File Offset: 0x001105E0
		private InteractionModule.BlockTargetInfo GetHeldBlockTargetInfo(out int heldBlockId, out int targetX, out int targetY, out int targetZ)
		{
			ClientItemBase primaryItem = this._gameInstance.LocalPlayer.PrimaryItem;
			heldBlockId = ((primaryItem != null) ? primaryItem.BlockId : 0);
			bool flag = this.CurrentRotationMode != InteractionModule.RotationMode.None && this._currentRotatedBlockId != -1;
			if (flag)
			{
				heldBlockId = this._currentRotatedBlockId;
			}
			ClientItemBase primaryItem2 = this._gameInstance.LocalPlayer.PrimaryItem;
			int num = (primaryItem2 != null) ? primaryItem2.BlockId : 0;
			bool flag2 = this._currentBlockId != num;
			if (flag2)
			{
				this._currentBlockId = num;
				bool flag3 = this.CurrentRotationMode != InteractionModule.RotationMode.None;
				if (flag3)
				{
					ClientBlockType blockType = this._gameInstance.MapModule.ClientBlockTypes[this._currentBlockId];
					ClientBlockType clientBlockType = this.TryGetRotatedVariant(blockType, this._currentBlockRotationAxis, this._rotationMatrix[this._currentBlockRotationAxis]);
					heldBlockId = clientBlockType.Id;
					this._currentRotatedBlockId = heldBlockId;
				}
			}
			int num2 = heldBlockId;
			ClientBlockType clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[heldBlockId];
			ValueTuple<bool, IntVector3?> valueTuple = PlaceBlockInteraction.TryGetPlacementPosition(this._gameInstance, clientBlockType2, out targetX, out targetY, out targetZ);
			bool flag4 = !valueTuple.Item1;
			InteractionModule.BlockTargetInfo result;
			if (flag4)
			{
				result = InteractionModule.BlockTargetInfo.FromFailedBlocks(this._gameInstance, new IntVector3(targetX, targetY, targetZ), valueTuple.Item2);
			}
			else
			{
				int block = this._gameInstance.MapModule.GetBlock(targetX, targetY, targetZ, 1);
				ClientBlockType targetBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				bool flag5 = this.CurrentRotationMode == InteractionModule.RotationMode.None;
				if (flag5)
				{
					heldBlockId = PlaceBlockInteraction.GetPlacedBlockVariant(this._gameInstance, clientBlockType2, targetBlockType, targetX, targetY, targetZ);
					clientBlockType2 = this._gameInstance.MapModule.ClientBlockTypes[heldBlockId];
					this._rotationMatrix[0] = clientBlockType2.RotationYaw;
					this._rotationMatrix[1] = clientBlockType2.RotationPitch;
					this._rotationMatrix[2] = clientBlockType2.RotationRoll;
				}
				valueTuple = PlaceBlockInteraction.TryGetPlacementPosition(this._gameInstance, clientBlockType2, out targetX, out targetY, out targetZ);
				bool flag6 = !valueTuple.Item1;
				if (flag6)
				{
					heldBlockId = num2;
					result = InteractionModule.BlockTargetInfo.FromFailedBlocks(this._gameInstance, new IntVector3(targetX, targetY, targetZ), valueTuple.Item2);
				}
				else
				{
					BoundingBox? box;
					bool flag7 = PlaceBlockInteraction.IsEntityBlockingPlacement(this._gameInstance, clientBlockType2, targetX, targetY, targetZ, out box);
					if (flag7)
					{
						result = new InteractionModule.BlockTargetInfo(new IntVector3(targetX, targetY, targetZ), box, false);
					}
					else
					{
						BoundingBox? box2;
						bool flag8 = !PlaceBlockInteraction.CanPlaceBlock(this._gameInstance, clientBlockType2, targetX, targetY, targetZ, out box2);
						if (flag8)
						{
							result = new InteractionModule.BlockTargetInfo(new IntVector3(targetX, targetY, targetZ), box2, false);
						}
						else
						{
							result = new InteractionModule.BlockTargetInfo(new IntVector3(targetX, targetY, targetZ), true);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x00112698 File Offset: 0x00110898
		private void UpdateBlockPreview()
		{
			int num;
			int x;
			int y;
			int z;
			this._targetBlockInfo = new InteractionModule.BlockTargetInfo?(this.GetHeldBlockTargetInfo(out num, out x, out y, out z));
			bool flag = !this.BlockPreview.IsEnabled;
			if (flag)
			{
				this.BlockPreview.IsVisible = false;
			}
			else
			{
				bool flag2 = !this.IsTargetPositionValid;
				if (flag2)
				{
					bool flag3 = this.BlockPreview.BlockId != num;
					if (flag3)
					{
						this.BlockPreview.IsVisible = false;
					}
				}
				else
				{
					bool flag4;
					if (this.InteractionTarget == InteractionModule.InteractionTargetType.Block)
					{
						ClientItemBase primaryItem = this._gameInstance.LocalPlayer.PrimaryItem;
						flag4 = (((primaryItem != null) ? primaryItem.BlockId : 0) == 0);
					}
					else
					{
						flag4 = true;
					}
					bool flag5 = flag4;
					if (flag5)
					{
						this.BlockPreview.IsVisible = false;
					}
					else
					{
						bool flag6 = this._currentLockedBlockPosition != null;
						if (flag6)
						{
							IntVector3? currentLockedBlockPosition = this._currentLockedBlockPosition;
							if (currentLockedBlockPosition == null)
							{
								throw new InvalidOperationException();
							}
							IntVector3 valueOrDefault = currentLockedBlockPosition.GetValueOrDefault();
							x = valueOrDefault.X;
							y = valueOrDefault.Y;
							z = valueOrDefault.Z;
						}
						this.BlockPreview.UpdatePreview(num, x, y, z);
					}
				}
			}
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x001127BC File Offset: 0x001109BC
		public bool TargetBlockOutineNeedsDrawing()
		{
			bool flag3;
			if (this._gameInstance.App.InGame.IsHudVisible && this.InteractionTarget == InteractionModule.InteractionTargetType.Block)
			{
				ClientItemBase primaryItem = this._gameInstance.LocalPlayer.PrimaryItem;
				bool? flag;
				if (primaryItem == null)
				{
					flag = null;
				}
				else
				{
					ItemBase.InteractionConfiguration interactionConfiguration = primaryItem.InteractionConfiguration;
					flag = ((interactionConfiguration != null) ? new bool?(interactionConfiguration.DisplayOutlines) : null);
				}
				bool? flag2 = flag;
				flag3 = !flag2.GetValueOrDefault(true);
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			bool result;
			if (flag4)
			{
				result = false;
			}
			else
			{
				int num = (int)Math.Floor((double)this.TargetBlockHit.BlockPosition.X);
				int num2 = (int)Math.Floor((double)this.TargetBlockHit.BlockPosition.Y);
				int num3 = (int)Math.Floor((double)this.TargetBlockHit.BlockPosition.Z);
				int worldChunkX = num >> 5;
				int worldChunkY = num2 >> 5;
				int worldChunkZ = num3 >> 5;
				Chunk chunk = this._gameInstance.MapModule.GetChunk(worldChunkX, worldChunkY, worldChunkZ);
				bool flag5 = chunk == null;
				if (flag5)
				{
					result = false;
				}
				else
				{
					int blockIndex = ChunkHelper.IndexOfWorldBlockInChunk(num, num2, num3);
					int num4;
					float num5;
					bool flag6 = chunk.Data.TryGetBlockHitTimer(blockIndex, out num4, out num5);
					result = !flag6;
				}
			}
			return result;
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x001128FC File Offset: 0x00110AFC
		public void DrawTargetBlockOutline(ref Vector3 cameraPosition, ref Matrix viewRotationProjectionMatrix)
		{
			bool flag = !this.TargetBlockOutineNeedsDrawing();
			if (flag)
			{
				throw new Exception("DrawTargetBlockOutline called when it was not required. Please check with RequestsDrawing() first before calling this.");
			}
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[this.TargetBlockHit.BlockId];
			BlockHitbox hitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
			ClientItemBase primaryItem = this._gameInstance.LocalPlayer.PrimaryItem;
			bool? flag2;
			if (primaryItem == null)
			{
				flag2 = null;
			}
			else
			{
				ItemBase.InteractionConfiguration interactionConfiguration = primaryItem.InteractionConfiguration;
				flag2 = ((interactionConfiguration != null) ? new bool?(interactionConfiguration.DebugOutlines) : null);
			}
			bool? flag3 = flag2;
			bool valueOrDefault = flag3.GetValueOrDefault();
			Vector3 vector = this.TargetBlockHit.BlockOrigin - cameraPosition;
			Vector3 position = vector;
			position.X += (float)((vector.X > 0f) ? -1 : 1) * 0.005f;
			position.Y += (float)((vector.Y > 0f) ? -1 : 1) * 0.005f;
			position.Z += (float)((vector.Z > 0f) ? -1 : 1) * 0.005f;
			this._blockOutlineRenderer.Draw(position, hitbox, viewRotationProjectionMatrix, valueOrDefault);
			bool flag4 = this._gameInstance.App.Settings.DisplayBlockBoundaries && this._targetBlockInfo != null;
			if (flag4)
			{
				InteractionModule.BlockTargetInfo value = this._targetBlockInfo.Value;
				List<ValueTuple<BoundingBox, float>> list = new List<ValueTuple<BoundingBox, float>>();
				foreach (BoundingBox boundingBox in value.CollidingBoxes)
				{
					BoundingBox item = boundingBox;
					item.Grow(new Vector3(0.005f));
					list.Add(new ValueTuple<BoundingBox, float>(item, 1f));
				}
				bool flag5 = !value.Valid;
				if (flag5)
				{
					BoundingBox item2 = new BoundingBox(value.Position, value.Position + Vector3.One);
					item2.Grow(new Vector3(-0.005f));
					list.Add(new ValueTuple<BoundingBox, float>(item2, 0.5f));
				}
				bool flag6 = list.Count > 0;
				if (flag6)
				{
					GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
					BoxRenderer boxRenderer = new BoxRenderer(graphics, graphics.GPUProgramStore.BasicProgram);
					BoundingBox hitbox2 = this._gameInstance.LocalPlayer.Hitbox;
					hitbox2.Translate(this._gameInstance.LocalPlayer.Position);
					foreach (ValueTuple<BoundingBox, float> valueTuple in list)
					{
						ValueTuple<BoundingBox, float> valueTuple2 = valueTuple;
						BoundingBox item3 = valueTuple2.Item1;
						float item4 = valueTuple2.Item2;
						bool flag7 = item3.Contains(hitbox2) == ContainmentType.Disjoint;
						if (flag7)
						{
							boxRenderer.Draw(-cameraPosition, item3, viewRotationProjectionMatrix, graphics.RedColor, 0.4f * item4, graphics.RedColor, 0.15f * item4);
						}
					}
					boxRenderer.Dispose();
				}
			}
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x00112C50 File Offset: 0x00110E50
		public void DrawTargetBlockSubface(ref Vector3 cameraPosition, ref Matrix viewRotationProjectionMatrix, ClientBlockType blockType)
		{
			BlockType.VariantRotation variantRotation = blockType.VariantRotation;
			BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[blockType.HitboxType].Clone();
			bool flag = blockHitbox.BoundingBox.Max.X - blockHitbox.BoundingBox.Min.X > 1f || blockHitbox.BoundingBox.Max.Y - blockHitbox.BoundingBox.Min.Y > 1f || blockHitbox.BoundingBox.Max.Z - blockHitbox.BoundingBox.Min.Z > 1f;
			Vector3 vector = this.TargetBlockHit.BlockPosition - cameraPosition;
			bool flag2 = this.TargetBlockHit.BlockNormal.X > 0f || this.TargetBlockHit.BlockNormal.Y > 0f || this.TargetBlockHit.BlockNormal.Z > 0f;
			if (flag2)
			{
				vector += this.TargetBlockHit.BlockNormal;
			}
			float x = (float)Math.Sign(this.TargetBlockHit.BlockNormal.X) * 0.001f;
			float y = (float)Math.Sign(this.TargetBlockHit.BlockNormal.Y) * 0.001f;
			float z = (float)Math.Sign(this.TargetBlockHit.BlockNormal.Z) * 0.001f;
			BoundingBox box = new BoundingBox(Vector3.Zero, Vector3.One);
			Vector3 vector2 = this.TargetBlockHit.HitPosition - this.TargetBlockHit.BlockOrigin;
			float num = Math.Abs(vector2.X - 0.5f);
			float num2 = Math.Abs(vector2.Y - 0.5f);
			float num3 = Math.Abs(vector2.Z - 0.5f);
			float num4 = 0.65f;
			bool flag3 = !flag && variantRotation == 6;
			if (flag3)
			{
				bool flag4 = this.TargetBlockHit.BlockNormal.X != 0f;
				if (flag4)
				{
					box.Max.X = x;
					box.Min.X = x;
					bool flag5 = vector2.Y > num4;
					if (flag5)
					{
						box.Min.Y = num4;
					}
				}
				bool flag6 = this.TargetBlockHit.BlockNormal.Y != 0f;
				if (flag6)
				{
					box.Max.Y = y;
					box.Min.Y = y;
				}
				bool flag7 = this.TargetBlockHit.BlockNormal.Z != 0f;
				if (flag7)
				{
					box.Max.Z = z;
					box.Min.Z = z;
					bool flag8 = vector2.Y > num4;
					if (flag8)
					{
						box.Min.Y = num4;
					}
				}
			}
			else
			{
				bool flag9 = !flag && (variantRotation == 3 || variantRotation == 4);
				if (flag9)
				{
					bool flag10 = this.TargetBlockHit.BlockNormal.X != 0f;
					if (flag10)
					{
						box.Max.X = x;
						box.Min.X = x;
						bool flag11 = num3 > num2;
						if (flag11)
						{
							bool flag12 = vector2.Z > num4;
							if (flag12)
							{
								box.Min.Z = num4;
							}
							else
							{
								bool flag13 = vector2.Z < 0.35f;
								if (flag13)
								{
									box.Max.Z = 0.35f;
								}
							}
						}
						else
						{
							bool flag14 = vector2.Y > num4;
							if (flag14)
							{
								box.Min.Y = num4;
							}
							else
							{
								bool flag15 = vector2.Y < 0.35f;
								if (flag15)
								{
									box.Max.Y = 0.35f;
								}
							}
						}
					}
					bool flag16 = this.TargetBlockHit.BlockNormal.Y != 0f;
					if (flag16)
					{
						box.Max.Y = y;
						box.Min.Y = y;
						bool flag17 = num > num3;
						if (flag17)
						{
							bool flag18 = vector2.X > num4;
							if (flag18)
							{
								box.Min.X = num4;
							}
							else
							{
								bool flag19 = vector2.X < 0.35f;
								if (flag19)
								{
									box.Max.X = 0.35f;
								}
							}
						}
						else
						{
							bool flag20 = vector2.Z > num4;
							if (flag20)
							{
								box.Min.Z = num4;
							}
							else
							{
								bool flag21 = vector2.Z < 0.35f;
								if (flag21)
								{
									box.Max.Z = 0.35f;
								}
							}
						}
					}
					bool flag22 = this.TargetBlockHit.BlockNormal.Z != 0f;
					if (flag22)
					{
						box.Max.Z = z;
						box.Min.Z = z;
						bool flag23 = num > num2;
						if (flag23)
						{
							bool flag24 = vector2.X > num4;
							if (flag24)
							{
								box.Min.X = num4;
							}
							else
							{
								bool flag25 = vector2.X < 0.35f;
								if (flag25)
								{
									box.Max.X = 0.35f;
								}
							}
						}
						else
						{
							bool flag26 = vector2.Y > num4;
							if (flag26)
							{
								box.Min.Y = num4;
							}
							else
							{
								bool flag27 = vector2.Y < 0.35f;
								if (flag27)
								{
									box.Max.Y = 0.35f;
								}
							}
						}
					}
				}
				else
				{
					bool flag28 = this.TargetBlockHit.BlockNormal.X != 0f;
					if (flag28)
					{
						box.Max.X = x;
						box.Min.X = x;
					}
					bool flag29 = this.TargetBlockHit.BlockNormal.Y != 0f;
					if (flag29)
					{
						box.Max.Y = y;
						box.Min.Y = y;
					}
					bool flag30 = this.TargetBlockHit.BlockNormal.Z != 0f;
					if (flag30)
					{
						box.Max.Z = z;
						box.Min.Z = z;
					}
				}
			}
			GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
			BoxRenderer boxRenderer = new BoxRenderer(graphics, graphics.GPUProgramStore.BasicProgram);
			boxRenderer.Draw(vector, box, viewRotationProjectionMatrix, graphics.WhiteColor, 0.25f, new Vector3(0.4f, 0.694f, 1f), 0.1f);
			boxRenderer.Dispose();
		}

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x0600481C RID: 18460 RVA: 0x0011337F File Offset: 0x0011157F
		public int RotatedBlockIdOverride
		{
			get
			{
				return (this.CurrentRotationMode == InteractionModule.RotationMode.PrePlacement) ? this._currentRotatedBlockId : -1;
			}
		}

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x0600481D RID: 18461 RVA: 0x00113394 File Offset: 0x00111594
		public bool ShouldForwardMouseWheelEvents
		{
			get
			{
				return this.CurrentRotationMode != InteractionModule.RotationMode.None || this._placingAtRange || this._gameInstance.BuilderToolsModule.ShouldSendMouseWheelEventToPlaySelectionTool();
			}
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x001133CC File Offset: 0x001115CC
		private void HandleBlockRotationInteractions()
		{
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			this.ValidateBlockRotationInteractions();
			bool flag = this._gameInstance.Input.IsShiftHeld();
			if (!flag)
			{
				this.ConsumeRotationInteraction(inputBindings.TogglePostRotationMode, new Action(this.OnTogglePostRotationMode));
				this.ConsumeRotationInteraction(inputBindings.TogglePreRotationMode, new Action(this.OnTogglePreRotationMode));
				bool flag2 = this.CurrentRotationMode != InteractionModule.RotationMode.None;
				if (flag2)
				{
					this.ConsumeRotationInteraction(inputBindings.NextRotationAxis, new Action(this.OnNextRotationAxis));
					this.ConsumeRotationInteraction(inputBindings.PreviousRotationAxis, new Action(this.OnPrevRotationAxis));
				}
			}
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x00113484 File Offset: 0x00111684
		private void ValidateBlockRotationInteractions()
		{
			bool flag = this.CurrentRotationMode != InteractionModule.RotationMode.None && this._currentBlockId != -1;
			if (flag)
			{
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[this._currentBlockId];
				string a = (clientBlockType != null) ? clientBlockType.Item : null;
				ClientItemStack activeHotbarItem = this._gameInstance.InventoryModule.GetActiveHotbarItem();
				bool flag2 = a != ((activeHotbarItem != null) ? activeHotbarItem.Id : null);
				if (flag2)
				{
					this.ToggleOrSwitchRotationMode(InteractionModule.RotationMode.None);
				}
			}
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x00113502 File Offset: 0x00111702
		private void OnTogglePostRotationMode()
		{
			this.ToggleOrSwitchRotationMode(InteractionModule.RotationMode.PostPlacement);
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x0011350C File Offset: 0x0011170C
		private void OnTogglePreRotationMode()
		{
			this.ToggleOrSwitchRotationMode(InteractionModule.RotationMode.PrePlacement);
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x00113516 File Offset: 0x00111716
		private void OnNextRotationAxis()
		{
			this.SwitchRotationAxis(1);
		}

		// Token: 0x06004823 RID: 18467 RVA: 0x00113520 File Offset: 0x00111720
		private void OnPrevRotationAxis()
		{
			this.SwitchRotationAxis(-1);
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x0011352C File Offset: 0x0011172C
		private void SwitchRotationAxis(int dir)
		{
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[this._currentBlockId];
			Axis axis = this._currentBlockRotationAxis;
			for (;;)
			{
				axis = InteractionModule.NextEnumerationValue<Axis>(InteractionModule.AxisValues, axis, dir);
				bool flag = !InteractionModule.HasRotation(clientBlockType.VariantRotation, axis);
				if (!flag)
				{
					break;
				}
				if (axis == this._currentBlockRotationAxis)
				{
					goto Block_2;
				}
			}
			this._gameInstance.Chat.Log(string.Format("Rotating {0} in {1}", InteractionModule.GetAxisName(this._currentBlockRotationAxis), this.CurrentRotationMode));
			this._currentBlockRotationAxis = axis;
			return;
			Block_2:
			this.PlayErrorSound();
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x001135D0 File Offset: 0x001117D0
		private static string GetAxisName(Axis axis)
		{
			string result;
			switch (axis)
			{
			case 0:
				result = "Yaw";
				break;
			case 1:
				result = "Pitch";
				break;
			case 2:
				result = "Roll";
				break;
			default:
				result = Enum.GetName(typeof(Axis), axis);
				break;
			}
			return result;
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x00113628 File Offset: 0x00111828
		private void ToggleOrSwitchRotationMode(InteractionModule.RotationMode mode)
		{
			bool flag = this.CurrentRotationMode == mode;
			if (flag)
			{
				mode = InteractionModule.RotationMode.None;
			}
			bool flag2 = mode != InteractionModule.RotationMode.None;
			if (flag2)
			{
				bool flag3 = this.InteractionTarget != InteractionModule.InteractionTargetType.Block;
				if (flag3)
				{
					return;
				}
				bool flag4 = false;
				bool flag5 = mode == InteractionModule.RotationMode.PostPlacement && !this.TryEnterPostPlacementMode();
				if (flag5)
				{
					flag4 = true;
				}
				bool flag6 = mode == InteractionModule.RotationMode.PrePlacement && !this.TryEnterPrePlacementMode();
				if (flag6)
				{
					flag4 = true;
				}
				bool flag7 = flag4;
				if (flag7)
				{
					this.PlayErrorSound();
					return;
				}
			}
			else
			{
				bool flag8 = this.CurrentRotationMode == InteractionModule.RotationMode.PostPlacement;
				if (flag8)
				{
					this.LeavePostPlacementMode();
				}
				bool flag9 = this.CurrentRotationMode == InteractionModule.RotationMode.PrePlacement;
				if (flag9)
				{
					this.LeavePrePlacementMode();
				}
				this._rotationMatrix[0] = 0;
				this._rotationMatrix[1] = 0;
				this._rotationMatrix[2] = 0;
			}
			this.CurrentRotationMode = mode;
			this.BlockPreview.UpdateEffect();
			this._gameInstance.Chat.Log(string.Format("Switched to rotation mode: {0}", this.CurrentRotationMode));
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x00113742 File Offset: 0x00111942
		private void LeavePrePlacementMode()
		{
			this._currentBlockId = -1;
			this._currentRotatedBlockId = -1;
			this.BlockPreview.UpdateEffect();
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x00113760 File Offset: 0x00111960
		private void LeavePostPlacementMode()
		{
			IntVector3? currentLockedBlockPosition = this._currentLockedBlockPosition;
			if (currentLockedBlockPosition == null)
			{
				throw new InvalidOperationException();
			}
			IntVector3 valueOrDefault = currentLockedBlockPosition.GetValueOrDefault();
			this._gameInstance.InjectPacket(new ServerSetBlock(valueOrDefault.X, valueOrDefault.Y, valueOrDefault.Z, this._currentRotatedBlockId, false));
			this._currentBlockId = -1;
			this._currentRotatedBlockId = -1;
			this._currentLockedBlockPosition = null;
			this.BlockPreview.UpdateEffect();
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x001137DC File Offset: 0x001119DC
		private bool TryEnterPostPlacementMode()
		{
			HitDetection.RaycastHit targetBlockHit = this._gameInstance.InteractionModule.TargetBlockHit;
			IntVector3 intVector = new IntVector3((int)Math.Floor((double)targetBlockHit.BlockPositionNoFiller.X), (int)Math.Floor((double)targetBlockHit.BlockPositionNoFiller.Y), (int)Math.Floor((double)targetBlockHit.BlockPositionNoFiller.Z));
			int block = this._gameInstance.MapModule.GetBlock(intVector.X, intVector.Y, intVector.Z, -1);
			bool flag = block <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				bool flag2 = !InteractionModule.DoesBlockSupportRotation(clientBlockType);
				if (flag2)
				{
					result = false;
				}
				else
				{
					BlockHitbox blockHitbox = this._gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
					bool flag3 = blockHitbox.IsOversized();
					if (flag3)
					{
						result = false;
					}
					else
					{
						this._currentRotatedBlockId = block;
						this._currentBlockId = ((clientBlockType.VariantOriginalId == -1) ? clientBlockType.Id : clientBlockType.VariantOriginalId);
						this._currentLockedBlockPosition = new IntVector3?(intVector);
						this._currentBlockRotationAxis = Enumerable.First<Axis>(InteractionModule.GetSupportedBlockRotations(clientBlockType));
						this._gameInstance.InjectPacket(new ServerSetBlock(intVector.X, intVector.Y, intVector.Z, 0, false));
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0011393C File Offset: 0x00111B3C
		private bool TryEnterPrePlacementMode()
		{
			bool flag = this._gameInstance.LocalPlayer.PrimaryItem == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int blockId = this._gameInstance.LocalPlayer.PrimaryItem.BlockId;
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[blockId];
				int num;
				int num2;
				int num3;
				PlaceBlockInteraction.TryGetPlacementPosition(this._gameInstance, clientBlockType, out num, out num2, out num3);
				int block = this._gameInstance.MapModule.GetBlock(num, num2, num3, 1);
				ClientBlockType targetBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				bool flag2 = !InteractionModule.DoesBlockSupportRotation(this._gameInstance.MapModule.ClientBlockTypes[blockId]);
				if (flag2)
				{
					result = false;
				}
				else
				{
					this._currentBlockId = blockId;
					this._currentRotatedBlockId = PlaceBlockInteraction.GetPlacedBlockVariant(this._gameInstance, clientBlockType, targetBlockType, num, num2, num3);
					this._currentBlockRotationAxis = Enumerable.First<Axis>(InteractionModule.GetSupportedBlockRotations(clientBlockType));
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x00113A34 File Offset: 0x00111C34
		private void ConsumeRotationInteraction(InputBinding binding, Action action)
		{
			bool flag = this._gameInstance.Input.CanConsumeBinding(binding, false);
			if (flag)
			{
				this._gameInstance.Input.ConsumeBinding(binding, false);
				action();
			}
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x00113A74 File Offset: 0x00111C74
		public void OnMouseWheelEvent(SDL.SDL_Event evt)
		{
			bool flag = evt.wheel.y == 0;
			if (!flag)
			{
				bool flag2 = this._gameInstance.BuilderToolsModule.ShouldSendMouseWheelEventToPlaySelectionTool();
				if (flag2)
				{
					this._gameInstance.BuilderToolsModule.SendMouseWheelEventToPlaySelectionTool(Math.Sign(evt.wheel.y));
				}
				else
				{
					bool placingAtRange = this._placingAtRange;
					if (placingAtRange)
					{
						bool flag3 = this._gameInstance.GameMode.Equals(0);
						if (flag3)
						{
							this._gameInstance.App.Settings.CurrentAdventureInteractionDistance += evt.wheel.y;
						}
						else
						{
							this._gameInstance.App.Settings.CurrentCreativeInteractionDistance += evt.wheel.y;
						}
					}
					else
					{
						bool flag4 = this._currentBlockId == -1;
						if (!flag4)
						{
							int direction = Math.Sign(evt.wheel.y);
							ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[this._currentBlockId];
							bool flag5 = !InteractionModule.HasRotation(clientBlockType.VariantRotation, this._currentBlockRotationAxis);
							if (flag5)
							{
								this.PlayErrorSound();
							}
							else
							{
								Rotation rotation = this._rotationMatrix[this._currentBlockRotationAxis];
								Rotation rotation2 = rotation;
								ClientBlockType clientBlockType2;
								for (;;)
								{
									clientBlockType2 = this.TryGetRotatedVariant(clientBlockType, this._currentBlockRotationAxis, rotation2 = InteractionModule.NextEnumerationValue<Rotation>(InteractionModule.RotationValues, rotation2, direction));
									bool flag6 = clientBlockType2.Id != this._currentRotatedBlockId;
									if (flag6)
									{
										break;
									}
									if (rotation2 == rotation)
									{
										goto IL_220;
									}
								}
								this._currentRotatedBlockId = clientBlockType2.Id;
								this._rotationMatrix[0] = clientBlockType2.RotationYaw;
								this._rotationMatrix[1] = clientBlockType2.RotationPitch;
								this._rotationMatrix[2] = clientBlockType2.RotationRoll;
								string audioEventForAxisRotation = InteractionModule.GetAudioEventForAxisRotation(this._currentBlockRotationAxis);
								bool flag7 = audioEventForAxisRotation != null;
								if (flag7)
								{
									this._gameInstance.AudioModule.PlayLocalSoundEvent(audioEventForAxisRotation);
								}
								IL_220:;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x00113CA2 File Offset: 0x00111EA2
		private void PlayErrorSound()
		{
			this._gameInstance.AudioModule.PlayLocalSoundEvent("CREATE_ERROR");
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x00113CBC File Offset: 0x00111EBC
		private ClientBlockType TryGetRotatedVariant(ClientBlockType blockType, Axis axis, Rotation rotation)
		{
			Rotation rotation2 = (axis == null) ? rotation : this._rotationMatrix[0];
			Rotation rotation3 = (axis == 1) ? rotation : this._rotationMatrix[1];
			Rotation rotation4 = (axis == 2) ? rotation : this._rotationMatrix[2];
			bool flag = rotation2 == null && rotation3 == null && rotation4 == 0;
			ClientBlockType result;
			if (flag)
			{
				result = blockType;
			}
			else
			{
				int num = blockType.TryGetRotatedVariant(rotation2, rotation3, rotation4);
				bool flag2 = num != blockType.Id;
				if (flag2)
				{
					result = this._gameInstance.MapModule.ClientBlockTypes[num];
				}
				else
				{
					result = blockType;
				}
			}
			return result;
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x00113D58 File Offset: 0x00111F58
		private static T NextEnumerationValue<T>(T[] array, T value, int direction)
		{
			int num = Array.IndexOf<T>(array, value);
			bool flag = num == -1;
			if (flag)
			{
				throw new InvalidOperationException(string.Format("Value {0} is not valid for enumeration", value));
			}
			int num2 = num + direction;
			bool flag2 = num2 < 0;
			T result;
			if (flag2)
			{
				result = array[array.Length - 1];
			}
			else
			{
				bool flag3 = num2 >= array.Length;
				if (flag3)
				{
					result = array[0];
				}
				else
				{
					result = array[num2];
				}
			}
			return result;
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x00113DD4 File Offset: 0x00111FD4
		private static bool DoesBlockSupportRotation(ClientBlockType blockType)
		{
			return Enumerable.Any<Axis>(InteractionModule.GetSupportedBlockRotations(blockType));
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x00113DF4 File Offset: 0x00111FF4
		private static IEnumerable<Axis> GetSupportedBlockRotations(ClientBlockType blockType)
		{
			return Enumerable.Where<Axis>(InteractionModule.AxisValues, (Axis axis) => InteractionModule.HasRotation(blockType.VariantRotation, axis));
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x00113E2C File Offset: 0x0011202C
		private static bool HasRotation(BlockType.VariantRotation rotation, Axis axis)
		{
			bool result;
			switch (rotation)
			{
			case 0:
				result = false;
				break;
			case 1:
				result = (axis == 0);
				break;
			case 2:
				result = (axis == 1);
				break;
			case 3:
				result = (axis == null || axis == 1);
				break;
			case 4:
				result = (axis == null || axis == 1);
				break;
			case 5:
				result = (axis == 0);
				break;
			case 6:
				result = (axis == null || axis == 1);
				break;
			case 7:
				result = true;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x00113EB4 File Offset: 0x001120B4
		private static string GetAudioEventForAxisRotation(Axis axis)
		{
			string result;
			switch (axis)
			{
			case 0:
				result = "CREATE_ROTATE_YAW";
				break;
			case 1:
				result = "CREATE_ROTATE_PITCH";
				break;
			case 2:
				result = "CREATE_ROTATE_ROLL";
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x00113EF8 File Offset: 0x001120F8
		public InteractionModule(GameInstance gameInstance) : base(gameInstance)
		{
			this.BlockPreview = new BlockPlacementPreview(this._gameInstance);
			this.BlockBreakHealth = new BlockBreakHealth(this._gameInstance);
			this._blockOutlineRenderer = new BlockOutlineRenderer(this._gameInstance.Engine.Graphics);
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x001140AF File Offset: 0x001122AF
		protected override void DoDispose()
		{
			this._blockOutlineRenderer.Dispose();
			this.BlockBreakHealth.Dispose();
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x001140CC File Offset: 0x001122CC
		public void PrepareInteractions(Interaction[] networkInteractions, out ClientInteraction[] upcomingInteractions)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingInteractions = new ClientInteraction[networkInteractions.Length];
			for (int i = 0; i < networkInteractions.Length; i++)
			{
				Interaction interaction = networkInteractions[i];
				bool flag = interaction == null;
				if (!flag)
				{
					upcomingInteractions[i] = ClientInteraction.Parse(i, interaction);
				}
			}
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x00114120 File Offset: 0x00112320
		public void PrepareRootInteractions(RootInteraction[] networkRootInteractions, out ClientRootInteraction[] upcomingRootInteractions)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingRootInteractions = new ClientRootInteraction[networkRootInteractions.Length];
			for (int i = 0; i < networkRootInteractions.Length; i++)
			{
				RootInteraction root = networkRootInteractions[i];
				upcomingRootInteractions[i] = new ClientRootInteraction(i, root);
			}
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x0011416C File Offset: 0x0011236C
		public void SetupInteractions(ClientInteraction[] interactions)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.Interactions = interactions;
			bool flag = this.RootInteractions == null;
			if (!flag)
			{
				foreach (ClientRootInteraction clientRootInteraction in this.RootInteractions)
				{
					clientRootInteraction.Build(this);
				}
			}
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x001141C0 File Offset: 0x001123C0
		public void SetupRootInteractions(ClientRootInteraction[] rootInteractions)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.RootInteractions = rootInteractions;
			foreach (ClientRootInteraction clientRootInteraction in this.RootInteractions)
			{
				clientRootInteraction.Build(this);
			}
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x00114204 File Offset: 0x00112404
		public void Update(float dt)
		{
			bool flag = true;
			bool flag2 = this._gameInstance.CameraModule.Controller.AttachedTo != this._gameInstance.LocalPlayer;
			if (flag2)
			{
				bool flag3 = !this._interactionHint.Equals(InteractionModule.InteractionHintData.None);
				if (flag3)
				{
					this._interactionHint = InteractionModule.InteractionHintData.None;
					this._gameInstance.App.Interface.TriggerEvent("crosshair.setInteractionHint", InteractionModule.InteractionHintData.None, null, null, null, null, null);
					this.InteractionTarget = InteractionModule.InteractionTargetType.None;
				}
				flag = false;
			}
			bool flag4 = flag;
			if (flag4)
			{
				this.UpdateInteractionTarget();
				this.UpdateBlockPreview();
			}
			this.HandleInteractions(dt, flag);
			this.DamageInfos.Clear();
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x001142C0 File Offset: 0x001124C0
		private void RequireNewClick(InteractionType type)
		{
			this._requireNewClick[type] = true;
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x001142CC File Offset: 0x001124CC
		public void DisableInput(InteractionType type)
		{
			this._disabledInputs[type] = true;
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x001142D8 File Offset: 0x001124D8
		private void QueueClick(InteractionContext context, InteractionType type, float clickQueueTimeout)
		{
			ref InteractionModule.ClickQueueData ptr = ref this._clickQueueData[type];
			bool flag = ptr.Timer == null;
			if (flag)
			{
				ptr.Timer = Stopwatch.StartNew();
				this._queuedClickTypes++;
			}
			else
			{
				float num = (float)ptr.Timer.ElapsedMilliseconds / 1000f;
				num *= this._gameInstance.TimeDilationModifier;
				bool flag2 = num + clickQueueTimeout < ptr.Timeout;
				if (flag2)
				{
					return;
				}
			}
			ptr.Timer.Restart();
			ptr.Timeout = clickQueueTimeout;
			ptr.TargetSlot = context.MetaStore.TargetSlot;
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x00114378 File Offset: 0x00112578
		public void UpdateCooldown(ClientRootInteraction root, bool click, out bool isOnCooldown)
		{
			InteractionCooldown cooldown = root.RootInteraction.Cooldown;
			string cooldownId = root.Id;
			float maxTime = 0.35f;
			float[] chargeTimes = InteractionModule.DefaultChargeTimes;
			bool interruptRecharge = false;
			bool flag = cooldown != null;
			if (flag)
			{
				maxTime = cooldown.Cooldown;
				bool flag2 = cooldown.ChargeTimes != null && cooldown.ChargeTimes.Length != 0;
				if (flag2)
				{
					chargeTimes = cooldown.ChargeTimes;
				}
				bool flag3 = cooldown.CooldownId != null;
				if (flag3)
				{
					cooldownId = cooldown.CooldownId;
				}
				bool interruptRecharge2 = cooldown.InterruptRecharge;
				if (interruptRecharge2)
				{
					interruptRecharge = true;
				}
				bool flag4 = cooldown.ClickBypass && click;
				if (flag4)
				{
					this.ResetCooldown(cooldownId, maxTime, chargeTimes, interruptRecharge);
					isOnCooldown = false;
					return;
				}
			}
			RootInteractionSettings rootInteractionSettings;
			bool flag5 = root.RootInteraction.Settings.TryGetValue(this._gameInstance.GameMode, out rootInteractionSettings);
			if (flag5)
			{
				bool flag6 = rootInteractionSettings.AllowSkipChainOnClick && click;
				if (flag6)
				{
					this.ResetCooldown(cooldownId, maxTime, chargeTimes, interruptRecharge);
					isOnCooldown = false;
					return;
				}
			}
			isOnCooldown = this.IsOnCooldown(root, cooldownId, maxTime, chargeTimes, interruptRecharge);
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x00114478 File Offset: 0x00112678
		public bool IsOnCooldown(ClientRootInteraction root, string cooldownId, float maxTime, float[] chargeTimes, bool interruptRecharge)
		{
			bool flag = maxTime <= 0f;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Cooldown cooldown = this.GetCooldown(cooldownId, maxTime, chargeTimes, root.RootInteraction.Cooldown == null || !root.RootInteraction.Cooldown.SkipCooldownReset, interruptRecharge);
				result = (cooldown != null && cooldown.HasCooldown(true));
			}
			return result;
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x001144DC File Offset: 0x001126DC
		public void ResetCooldown(string cooldownId, float maxTime, float[] chargeTimes, bool interruptRecharge)
		{
			Cooldown cooldown = this.GetCooldown(cooldownId, maxTime, chargeTimes, true, interruptRecharge);
			cooldown.ResetCooldown();
			cooldown.ResetCharges();
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x00114508 File Offset: 0x00112708
		public Cooldown GetCooldown(string cooldownId, float maxTime, float[] chargeTimes, bool force, bool interruptRecharge)
		{
			if (force)
			{
				bool flag = !this._cooldowns.ContainsKey(cooldownId);
				if (flag)
				{
					this._cooldowns.Add(cooldownId, new Cooldown(maxTime, chargeTimes, interruptRecharge));
				}
			}
			return this._cooldowns.ContainsKey(cooldownId) ? this._cooldowns[cooldownId] : null;
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x0011456C File Offset: 0x0011276C
		public Cooldown GetCooldown(string cooldownId)
		{
			Cooldown cooldown;
			bool flag = this._cooldowns.TryGetValue(cooldownId, out cooldown);
			Cooldown result;
			if (flag)
			{
				result = cooldown;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x00114598 File Offset: 0x00112798
		private void HandleInteractions(float dt, bool allInteractions)
		{
			for (int i = 0; i < this._disabledInputs.Length; i++)
			{
				this._disabledInputs[i] = false;
			}
			this.Tick(dt);
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			if (allInteractions)
			{
				bool flag = !this._gameInstance.LocalPlayer.IsMounting && this._gameInstance.InteractionModule.CurrentRotationMode != InteractionModule.RotationMode.PostPlacement;
				if (flag)
				{
					this.ConsumeInteractionType(inputBindings.PrimaryItemAction, 0, null);
					this.ConsumeInteractionType(inputBindings.SecondaryItemAction, 1, null);
					this.ConsumeInteractionType(inputBindings.TertiaryItemAction, 2, null);
					this.ConsumeInteractionType(inputBindings.Ability1ItemAction, 3, null);
					this.ConsumeInteractionType(inputBindings.Ability2ItemAction, 4, null);
					this.ConsumeInteractionType(inputBindings.Ability3ItemAction, 5, null);
					this.ConsumeInteractionType(inputBindings.BlockInteractAction, 7, null);
					this.ConsumeInteractionType(inputBindings.PickBlock, 8, null);
					this.ConsumeInteractionType(inputBindings.Sprint, 25, null);
					this.TryRunHeldInteraction(22, null);
					this.TryRunHeldInteraction(23, null);
					for (int j = 0; j < this._gameInstance.InventoryModule._armorInventory.Length; j++)
					{
						this.TryRunHeldInteraction(24, new int?(j));
					}
				}
				else
				{
					bool flag2 = this._gameInstance.Input.ConsumeBinding(inputBindings.DismountAction, false);
					if (flag2)
					{
						this._gameInstance.CharacterControllerModule.DismountNpc(true);
					}
				}
			}
			bool flag3 = false;
			for (int k = 0; k < 9; k++)
			{
				InputBinding hotbarSlot = inputBindings.GetHotbarSlot(k);
				bool flag4 = this._gameInstance.Input.CanConsumeBinding(hotbarSlot, false);
				if (flag4)
				{
					bool flag5 = !flag3;
					if (flag5)
					{
						this.ConsumeInteractionType(inputBindings.GetHotbarSlot(k), 15, new int?(k));
						flag3 = true;
					}
					this._gameInstance.Input.ConsumeBinding(hotbarSlot, false);
				}
			}
			this.HandleBlockRotationInteractions();
			this.ConsumeClickQueue();
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x0011481C File Offset: 0x00112A1C
		private bool CanPlayerUseInteraction(InteractionType type)
		{
			return this._gameInstance.GameMode == 1 || !this._gameInstance.LocalPlayer.IsInteractionDisabled(type);
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x00114854 File Offset: 0x00112A54
		private void TryRunHeldInteraction(InteractionType type, int? equipSlot = null)
		{
			bool flag = !this.CanPlayerUseInteraction(type);
			if (!flag)
			{
				InventoryModule inventoryModule = this._gameInstance.InventoryModule;
				ClientItemStack clientItemStack;
				switch (type)
				{
				case 22:
					clientItemStack = inventoryModule.GetHotbarItem(inventoryModule.HotbarActiveSlot);
					break;
				case 23:
					clientItemStack = inventoryModule.GetUtilityItem(inventoryModule.UtilityActiveSlot);
					break;
				case 24:
				{
					bool flag2 = equipSlot == null;
					if (flag2)
					{
						throw new ArgumentException();
					}
					clientItemStack = inventoryModule.GetArmorItem(equipSlot.Value);
					break;
				}
				default:
					throw new ArgumentException();
				}
				ClientItemBase item = this._gameInstance.ItemLibraryModule.GetItem((clientItemStack != null) ? clientItemStack.Id : null);
				int num = 0;
				bool? flag3;
				if (item == null)
				{
					flag3 = null;
				}
				else
				{
					Dictionary<InteractionType, int> interactions = item.Interactions;
					flag3 = ((interactions != null) ? new bool?(interactions.TryGetValue(type, out num)) : null);
				}
				bool? flag4 = flag3;
				bool flag5 = !flag4.GetValueOrDefault();
				if (!flag5)
				{
					ClientRootInteraction rootInteraction = this.RootInteractions[num];
					bool flag6 = !this.CanRun(type, equipSlot.GetValueOrDefault(-1), rootInteraction);
					if (!flag6)
					{
						InventoryModule inventoryModule2 = this._gameInstance.InventoryModule;
						InteractionContext context = InteractionContext.ForInteraction(this._gameInstance, inventoryModule2, type, equipSlot);
						this.StartChain(context, type, InteractionModule.ClickType.None, null, null, null, null);
					}
				}
			}
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x001149B8 File Offset: 0x00112BB8
		public void ConsumeInteractionType(InputBinding binding, InteractionType type, int? targetSlot = null)
		{
			bool flag = binding != null && this._gameInstance.LocalPlayer.GetRelativeMovementStates().IsMantling;
			if (!flag)
			{
				bool flag2 = (binding != null && !this._gameInstance.Input.IsBindingHeld(binding, false)) || this._disabledInputs[type];
				if (!flag2)
				{
					bool flag3 = !this.CanPlayerUseInteraction(type);
					if (!flag3)
					{
						InventoryModule inventoryModule = this._gameInstance.InventoryModule;
						InteractionContext interactionContext = InteractionContext.ForInteraction(this._gameInstance, inventoryModule, type, null);
						bool flag4 = targetSlot != null;
						if (flag4)
						{
							int? num = targetSlot;
							int hotbarActiveSlot = inventoryModule.HotbarActiveSlot;
							bool flag5 = (num.GetValueOrDefault() == hotbarActiveSlot & num != null) && !inventoryModule.UsingToolsItem();
							if (flag5)
							{
								return;
							}
							interactionContext.MetaStore.TargetSlot = targetSlot;
						}
						InteractionModule.ClickType clickType = InteractionModule.ClickType.Single;
						bool flag6 = binding == null || this._gameInstance.Input.CanConsumeBinding(binding, false);
						if (flag6)
						{
							this._requireNewClick[type] = false;
						}
						else
						{
							bool flag7 = this._requireNewClick[type];
							if (flag7)
							{
								return;
							}
							bool flag8 = this._itemOnClick[type] != null;
							if (flag8)
							{
								ClientItemStack heldItem = interactionContext.HeldItem;
								bool flag9 = ((heldItem != null) ? heldItem.Id : null) != this._itemOnClick[type] && this._activeSlot[type] == interactionContext.HeldItemSlot && this._activeInventory[type] == interactionContext.HeldItemSectionId;
								if (flag9)
								{
									this._requireNewClick[type] = true;
									return;
								}
							}
							clickType = InteractionModule.ClickType.Held;
						}
						bool flag10 = this.StartChain(interactionContext, type, clickType, null, null, null, null);
						if (flag10)
						{
							bool flag11 = binding != null;
							if (flag11)
							{
								this._gameInstance.Input.ConsumeBinding(binding, false);
							}
							bool flag12 = clickType == InteractionModule.ClickType.Single || this._itemOnClick[type] == null || this._activeSlot[type] != interactionContext.HeldItemSlot || this._activeInventory[type] != interactionContext.HeldItemSectionId;
							if (flag12)
							{
								string[] itemOnClick = this._itemOnClick;
								ClientItemStack heldItem2 = interactionContext.HeldItem;
								itemOnClick[type] = ((heldItem2 != null) ? heldItem2.Id : null);
								this._activeSlot[type] = interactionContext.HeldItemSlot;
								this._activeInventory[type] = interactionContext.HeldItemSectionId;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x00114C10 File Offset: 0x00112E10
		public InputBinding GetInputBindingForType(InteractionType type)
		{
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			switch (type)
			{
			case 0:
				return inputBindings.PrimaryItemAction;
			case 1:
				return inputBindings.SecondaryItemAction;
			case 2:
				return inputBindings.TertiaryItemAction;
			case 3:
				return inputBindings.Ability1ItemAction;
			case 4:
				return inputBindings.Ability2ItemAction;
			case 5:
				return inputBindings.Ability3ItemAction;
			case 7:
				return inputBindings.BlockInteractAction;
			case 8:
				return inputBindings.PickBlock;
			case 14:
			case 15:
				return inputBindings.HotbarSlot1;
			}
			throw new ArgumentOutOfRangeException("type", type, null);
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x00114CE4 File Offset: 0x00112EE4
		private void ConsumeClickQueue()
		{
			bool flag = this.Chains.Count > 0 || this._queuedClickTypes == 0;
			if (!flag)
			{
				for (int i = 0; i < this._clickQueueData.Length; i++)
				{
					ref InteractionModule.ClickQueueData ptr = ref this._clickQueueData[i];
					bool flag2 = ptr.Timer == null;
					if (!flag2)
					{
						float num = (float)ptr.Timer.ElapsedMilliseconds / 1000f;
						num *= this._gameInstance.TimeDilationModifier;
						bool flag3 = num > ptr.Timeout;
						if (!flag3)
						{
							InteractionType type = i;
							InputBinding inputBindingForType = this.GetInputBindingForType(type);
							InventoryModule inventoryModule = this._gameInstance.InventoryModule;
							InteractionContext interactionContext = InteractionContext.ForInteraction(this._gameInstance, inventoryModule, type, null);
							bool flag4 = ptr.TargetSlot != null;
							if (flag4)
							{
								interactionContext.MetaStore.TargetSlot = ptr.TargetSlot;
							}
							bool flag5 = this.StartChain(interactionContext, type, this._gameInstance.Input.CanConsumeBinding(inputBindingForType, false) ? InteractionModule.ClickType.Single : InteractionModule.ClickType.Held, null, null, null, null);
							if (flag5)
							{
								ptr.Timer = null;
								this._queuedClickTypes--;
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x00114E40 File Offset: 0x00113040
		private void Tick(float dt)
		{
			for (int i = 0; i < this._globalTimeShift.Length; i++)
			{
				this._globalTimeShift[i] = 0f;
			}
			bool flag = this._cooldowns.Count > 0;
			if (flag)
			{
				foreach (KeyValuePair<string, Cooldown> keyValuePair in Enumerable.ToArray<KeyValuePair<string, Cooldown>>(this._cooldowns))
				{
					Cooldown value = keyValuePair.Value;
					bool flag2 = value.Tick(dt);
					if (flag2)
					{
						this._cooldowns.Remove(keyValuePair.Key);
					}
				}
			}
			bool flag3 = this.Chains.Count == 0;
			if (!flag3)
			{
				foreach (KeyValuePair<int, InteractionChain> keyValuePair2 in Enumerable.ToArray<KeyValuePair<int, InteractionChain>>(this.Chains))
				{
					InteractionChain value2 = keyValuePair2.Value;
					bool flag4 = this.TickChain(value2);
					if (flag4)
					{
						this.Chains.Remove(keyValuePair2.Key);
					}
				}
			}
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x00114F50 File Offset: 0x00113150
		private bool TickChain(InteractionChain chain)
		{
			foreach (KeyValuePair<ulong, InteractionChain> keyValuePair in Enumerable.ToArray<KeyValuePair<ulong, InteractionChain>>(chain.ForkedChains))
			{
				InteractionChain value = keyValuePair.Value;
				bool flag = this.TickChain(value);
				if (flag)
				{
					chain.ForkedChains.Remove(keyValuePair.Key);
				}
			}
			bool flag2 = chain.ClientState != 4;
			bool result;
			if (flag2)
			{
				this.TryCancelAndRevert(chain);
				bool flag3 = chain.ServerState != 4;
				if (flag3)
				{
					bool flag4 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
					if (flag4)
					{
						InteractionModule.Logger.Trace(string.Format("Remove Chain: {0}, {1}", chain.ChainId, chain));
					}
					Action onCompletion = chain.OnCompletion;
					if (onCompletion != null)
					{
						onCompletion();
					}
					chain.OnCompletion = null;
					bool flag5 = chain.ForkedChains.Count == 0;
					if (flag5)
					{
						return true;
					}
				}
				else
				{
					long elapsedMilliseconds = chain.WaitingForServerFinished.ElapsedMilliseconds;
					bool flag6 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
					if (flag6)
					{
						InteractionModule.Logger.Trace(string.Format("Client finished chain but server hasn't! {0}, {1}, {2}", chain.ChainId, chain, TimeHelper.FormatMillis(elapsedMilliseconds)));
					}
					chain.WaitingForServerFinished.Start();
					bool flag7 = (float)elapsedMilliseconds > this._gameInstance.TimeModule.OperationTimeoutThreshold;
					if (flag7)
					{
						bool flag8 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
						if (flag8)
						{
							InteractionModule.Logger.Trace("Error: Server took too long to finish chain!");
						}
					}
				}
				result = false;
			}
			else
			{
				this.DoTickChain<object>(chain, (InteractionModule module, InteractionChain c, InteractionModule.ClickType clickType, bool hasAnyButtonClick, object p) => module.ClientTick(c, clickType, hasAnyButtonClick), null, false);
				this.TryCancelAndRevert(chain);
				bool flag9 = chain.ClientState != 4;
				if (flag9)
				{
					bool hasTempSyncData = chain.HasTempSyncData;
					if (hasTempSyncData)
					{
						throw new Exception("Finished yet server took a different route?");
					}
					bool flag10 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
					if (flag10)
					{
						InteractionModule.Logger.Trace(string.Format("Client finished chain: {0}, {1} in {2:f}s", chain.ChainId, chain, chain.Time.Elapsed.TotalSeconds));
					}
					bool flag11 = chain.ServerState != 4;
					if (flag11)
					{
						bool flag12 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
						if (flag12)
						{
							InteractionModule.Logger.Trace(string.Format("Remove Chain: {0}, {1}", chain.ChainId, chain));
						}
						Action onCompletion2 = chain.OnCompletion;
						if (onCompletion2 != null)
						{
							onCompletion2();
						}
						chain.OnCompletion = null;
						bool flag13 = chain.ForkedChains.Count == 0;
						if (flag13)
						{
							return true;
						}
					}
				}
				else
				{
					bool flag14 = chain.ServerState != 4;
					if (flag14)
					{
						long elapsedMilliseconds2 = chain.WaitingForClientFinished.ElapsedMilliseconds;
						bool flag15 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
						if (flag15)
						{
							InteractionModule.Logger.Trace(string.Format("Server finished chain but client hasn't! {0}, {1}, {2}", chain.ChainId, chain, TimeHelper.FormatMillis(elapsedMilliseconds2)));
						}
						chain.WaitingForClientFinished.Start();
						bool flag16 = (float)elapsedMilliseconds2 > this._gameInstance.TimeModule.OperationTimeoutThreshold;
						if (flag16)
						{
							bool flag17 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
							if (flag17)
							{
								InteractionModule.Logger.Trace("Error: Server finished chain earlier than client!");
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x001152DC File Offset: 0x001134DC
		private void TryCancelAndRevert(InteractionChain chain)
		{
			InteractionEntry previousInteractionEntry = chain.PreviousInteractionEntry;
			int num = (previousInteractionEntry != null) ? previousInteractionEntry.Index : 0;
			bool flag = !chain.ServerCancelled || chain.OperationIndex < num;
			if (!flag)
			{
				bool flag2 = chain.ClientState == 3 && chain.ServerState == 3;
				if (!flag2)
				{
					chain.ClearIncompleteSyncData();
					this.DoRollback(chain, num, null);
					chain.ClientState = 3;
					chain.ServerState = 3;
					InteractionEntry interactionEntry;
					bool interaction = chain.GetInteraction(chain.OperationIndex, out interactionEntry);
					if (interaction)
					{
						try
						{
							ClientRootInteraction rootInteraction = chain.RootInteraction;
							ClientRootInteraction.Operation operation = rootInteraction.Operations[chain.OperationCounter];
							interactionEntry.State.State = 3;
							chain.Context.InitEntry(chain, interactionEntry, this._gameInstance);
							operation.Handle(this._gameInstance, false, interactionEntry.TimeOffset, chain.Type, chain.Context);
						}
						finally
						{
							chain.Context.DeinitEntry(chain, interactionEntry, this._gameInstance);
						}
					}
					bool serverCancelled = chain.ServerCancelled;
					if (serverCancelled)
					{
						chain.ClearSyncData();
					}
					this.SendSyncPacket(chain, chain.OperationIndex, null, true);
				}
			}
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x00115420 File Offset: 0x00113620
		private void DoTickChain<T>(InteractionChain chain, Func<InteractionModule, InteractionChain, InteractionModule.ClickType, bool, T, InteractionSyncData> tickFunc, T param, bool force = false)
		{
			List<InteractionSyncData> tempSyncDataList = this._tempSyncDataList;
			tempSyncDataList.Clear();
			ClientRootInteraction rootInteraction = chain.RootInteraction;
			int num = rootInteraction.Operations.Length;
			int operationCounter = chain.OperationCounter;
			int operationIndex = chain.OperationIndex;
			int i = chain.GetCallDepth();
			InteractionModule.ClickType clickType;
			bool arg = this.HasAnyButtonClick(chain.Type, out clickType, chain.Context);
			bool flag = chain.ConsumeFirstRun();
			if (flag)
			{
				bool flag2 = chain.ForkedChainId == null;
				if (flag2)
				{
					chain.TimeShift = this.GetGlobalTimeShift(chain.Type);
				}
			}
			else
			{
				chain.TimeShift = 0f;
			}
			for (;;)
			{
				int serverCompleteIndex = chain.ServerCompleteIndex;
				bool flag3 = chain.ServerCancelled && chain.OperationIndex >= serverCompleteIndex;
				if (flag3)
				{
					break;
				}
				try
				{
					tempSyncDataList.Add(tickFunc(this, chain, clickType, arg, param));
				}
				catch (InteractionModule.RollbackException)
				{
					this.SendSyncPacket(chain, operationIndex, tempSyncDataList, false);
					this.DoRollback(chain, chain.OperationIndex - 1, null);
					return;
				}
				bool flag4 = i != chain.GetCallDepth();
				if (flag4)
				{
					i = chain.GetCallDepth();
					rootInteraction = chain.RootInteraction;
					num = rootInteraction.Operations.Length;
				}
				else
				{
					bool flag5 = operationCounter == chain.OperationCounter;
					if (flag5)
					{
						break;
					}
				}
				chain.NextOperationIndex();
				operationCounter = chain.OperationCounter;
				bool flag6 = operationCounter >= num;
				if (flag6)
				{
					while (i > 0)
					{
						chain.PopRoot();
						i = chain.GetCallDepth();
						operationCounter = chain.OperationCounter;
						rootInteraction = chain.RootInteraction;
						num = rootInteraction.Operations.Length;
						bool flag7 = operationCounter < num || i == 0;
						if (flag7)
						{
							break;
						}
					}
					bool flag8 = i == 0 && operationCounter >= num;
					if (flag8)
					{
						break;
					}
				}
			}
			chain.UpdateClientState(this._gameInstance, clickType);
			this.SendSyncPacket(chain, operationIndex, tempSyncDataList, force);
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x00115628 File Offset: 0x00113828
		private InteractionSyncData ClientTick(InteractionChain chain, InteractionModule.ClickType clickType, bool hasAnyButtonClick)
		{
			ClientRootInteraction rootInteraction = chain.RootInteraction;
			ClientRootInteraction.Operation operation = rootInteraction.Operations[chain.OperationCounter];
			InteractionEntry orCreateInteractionEntry = chain.GetOrCreateInteractionEntry(chain.OperationIndex);
			bool flag = orCreateInteractionEntry.ServerState == null;
			if (flag)
			{
				InteractionSyncData interactionSyncData = chain.GetInteractionSyncData(chain.OperationIndex);
				bool flag2 = interactionSyncData != null && (orCreateInteractionEntry.State.OperationCounter != interactionSyncData.OperationCounter || orCreateInteractionEntry.State.RootInteraction != interactionSyncData.RootInteraction);
				if (flag2)
				{
					throw new InteractionModule.RollbackException();
				}
				chain.RemoveInteractionSyncData(chain.OperationIndex);
				orCreateInteractionEntry.ServerState = interactionSyncData;
			}
			bool flag3 = operation.GetWaitForDataFrom(this._gameInstance) == 1 && orCreateInteractionEntry.ServerState == null;
			InteractionSyncData result;
			if (flag3)
			{
				long elapsedMilliseconds = orCreateInteractionEntry.WaitingForSyncData.ElapsedMilliseconds;
				bool flag4 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
				if (flag4)
				{
					InteractionModule.Logger.Trace(string.Format("Wait for interaction serverData: {0}, {1}, {2}", chain.OperationIndex, orCreateInteractionEntry, TimeHelper.FormatMillis(elapsedMilliseconds)));
				}
				orCreateInteractionEntry.WaitingForSyncData.Start();
				bool flag5 = (float)elapsedMilliseconds > this._gameInstance.TimeModule.OperationTimeoutThreshold;
				if (flag5)
				{
					bool flag6 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
					if (flag6)
					{
						InteractionModule.Logger.Trace("Error: Server took too long to send serverData!");
					}
				}
				result = null;
			}
			else
			{
				int clientDataHashCode = orCreateInteractionEntry.GetClientDataHashCode();
				InteractionContext context = chain.Context;
				float num = orCreateInteractionEntry.TimeOffset + (float)orCreateInteractionEntry.Time.Elapsed.TotalSeconds;
				bool flag7 = !orCreateInteractionEntry.Time.IsRunning;
				bool flag8 = flag7;
				if (flag8)
				{
					num = chain.TimeShift;
					orCreateInteractionEntry.SetTimestamp(num);
				}
				num *= this._gameInstance.TimeDilationModifier;
				try
				{
					context.InitEntry(chain, orCreateInteractionEntry, this._gameInstance);
					operation.Tick(this._gameInstance, clickType, hasAnyButtonClick, flag7, num, chain.Type, context);
				}
				finally
				{
					context.DeinitEntry(chain, orCreateInteractionEntry, this._gameInstance);
				}
				InteractionSyncData interactionSyncData2 = null;
				InteractionSyncData state = orCreateInteractionEntry.State;
				bool flag9 = flag7 || clientDataHashCode != orCreateInteractionEntry.GetClientDataHashCode();
				if (flag9)
				{
					interactionSyncData2 = state;
				}
				try
				{
					context.InitEntry(chain, orCreateInteractionEntry, this._gameInstance);
					operation.Handle(this._gameInstance, flag7, num, chain.Type, context);
				}
				finally
				{
					context.DeinitEntry(chain, orCreateInteractionEntry, this._gameInstance);
				}
				this.RemoveInteractionIfFinished(chain, orCreateInteractionEntry);
				result = ((interactionSyncData2 != null) ? new InteractionSyncData(interactionSyncData2) : null);
			}
			return result;
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x001158E0 File Offset: 0x00113AE0
		private void RemoveInteractionIfFinished(InteractionChain chain, InteractionEntry entry)
		{
			bool flag = chain.OperationIndex == entry.Index && entry.State.State != 4;
			if (flag)
			{
				chain.FinalState = entry.State.State;
			}
			bool flag2 = entry.State != null && entry.State.State != 4;
			if (flag2)
			{
				bool flag3 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
				if (flag3)
				{
					InteractionModule.Logger.Trace(string.Format("Client finished interaction: {0}, {1}", entry.Index, entry));
				}
				bool flag4 = entry.ServerState != null && entry.ServerState.State != 4;
				if (flag4)
				{
					bool flag5 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
					if (flag5)
					{
						InteractionModule.Logger.Trace(string.Format("Remove Interaction: {0}, {1}", entry.Index, entry));
					}
					chain.RemoveInteractionEntry(entry.Index);
				}
			}
			else
			{
				bool flag6 = entry.ServerState != null && entry.ServerState.State != 4;
				if (flag6)
				{
					long elapsedMilliseconds = entry.WaitingForClientFinished.ElapsedMilliseconds;
					bool flag7 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
					if (flag7)
					{
						InteractionModule.Logger.Trace(string.Format("Server finished interaction but client hasn't! {0}, {1}, {2}, {3}", new object[]
						{
							entry.ServerState.State,
							entry.Index,
							entry,
							TimeHelper.FormatMillis(elapsedMilliseconds)
						}));
					}
					entry.WaitingForClientFinished.Start();
					bool flag8 = (float)elapsedMilliseconds > this._gameInstance.TimeModule.OperationTimeoutThreshold;
					if (flag8)
					{
						bool flag9 = InteractionModule.Logger.IsEnabled(LogLevel.Warn);
						if (flag9)
						{
							InteractionModule.Logger.Warn("Error: Server finished interaction earlier than client!");
						}
					}
				}
			}
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x00115AD0 File Offset: 0x00113CD0
		public void RevertChain(InteractionChain chain, int index)
		{
			chain.ClientState = 4;
			bool flag = chain.OperationCounter >= chain.RootInteraction.Operations.Length;
			if (flag)
			{
				chain.OperationIndex--;
			}
			while (chain.OperationIndex >= index)
			{
				InteractionEntry previousInteractionEntry;
				bool flag2 = !chain.GetInteraction(chain.OperationIndex, out previousInteractionEntry) && chain.PreviousInteractionEntry != null && chain.OperationIndex == chain.PreviousInteractionEntry.Index;
				if (flag2)
				{
					previousInteractionEntry = chain.PreviousInteractionEntry;
				}
				bool flag3 = previousInteractionEntry == null;
				if (flag3)
				{
					bool flag4 = chain.OperationIndex == 0;
					if (flag4)
					{
						InteractionModule.Logger.Warn(string.Format("Tried to revert chain {0}-{1} that never started?", chain.ChainId, chain.ForkedChainId));
					}
					else
					{
						Logger logger = InteractionModule.Logger;
						string format = "Tried to revert chain {0}-{1} to {2} but couldn't due to a missing entry. Index={3}, Prev={4}";
						object[] array = new object[5];
						array[0] = chain.ChainId;
						array[1] = chain.ForkedChainId;
						array[2] = index;
						array[3] = chain.OperationIndex;
						int num = 4;
						InteractionEntry previousInteractionEntry2 = chain.PreviousInteractionEntry;
						array[num] = ((previousInteractionEntry2 != null) ? new int?(previousInteractionEntry2.Index) : null);
						logger.Error(string.Format(format, array));
						chain.OperationIndex = index;
					}
					return;
				}
				chain.OperationCounter = previousInteractionEntry.State.OperationCounter;
				chain.RootInteraction = this.RootInteractions[previousInteractionEntry.State.RootInteraction];
				ClientRootInteraction.Operation operation = chain.RootInteraction.Operations[chain.OperationCounter];
				try
				{
					chain.Context.InitEntry(chain, previousInteractionEntry, this._gameInstance);
					operation.Revert(this._gameInstance, chain.Type, chain.Context);
				}
				finally
				{
					chain.Context.DeinitEntry(chain, previousInteractionEntry, this._gameInstance);
				}
				chain.RemoveForksForEntry(this, previousInteractionEntry.Index);
				chain.OperationIndex--;
			}
			chain.OperationIndex++;
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x00115CEC File Offset: 0x00113EEC
		private void DoRollback(InteractionChain chain, int index, int? rewriteRoot = null)
		{
			bool flag = chain.OperationIndex == index;
			if (!flag)
			{
				chain.Desync = true;
				bool flag2 = true;
				bool flag3 = index < 0;
				if (flag3)
				{
					index = 0;
					flag2 = false;
				}
				this.RevertChain(chain, index);
				bool flag4 = rewriteRoot != null;
				if (flag4)
				{
					int value = rewriteRoot.Value;
					bool flag5 = chain.InitialRootInteraction.Index != value;
					if (flag5)
					{
						InteractionModule.Logger.Warn(string.Format("Incorrect root, swapping: Client: {0}, Server: {1}", chain.InitialRootInteraction.Index, value));
						chain.RootInteraction = (chain.InitialRootInteraction = this.RootInteractions[value]);
						chain.ClearInteractions();
					}
				}
				bool flag6 = flag2;
				if (flag6)
				{
					chain.ShiftInteractionEntryOffset(1);
				}
				bool flag7 = chain.GetInteractionSyncData(index) == null && chain.PreviousInteractionEntry != null;
				if (flag7)
				{
					InteractionEntry orCreateInteractionEntry = chain.GetOrCreateInteractionEntry(index);
					orCreateInteractionEntry.ServerState = chain.PreviousInteractionEntry.ServerState;
				}
				this.DoTickChain<int>(chain, delegate(InteractionModule module, InteractionChain c, InteractionModule.ClickType clickType, bool hasAnyButtonClick, int rootIndex)
				{
					ClientRootInteraction rootInteraction = c.RootInteraction;
					ClientRootInteraction.Operation operation = rootInteraction.Operations[c.OperationCounter];
					InteractionEntry orCreateInteractionEntry2 = c.GetOrCreateInteractionEntry(c.OperationIndex);
					InteractionSyncData interactionSyncData = c.RemoveInteractionSyncData(c.OperationIndex);
					bool flag8 = interactionSyncData != null;
					if (flag8)
					{
						orCreateInteractionEntry2.ServerState = interactionSyncData;
					}
					bool flag9 = orCreateInteractionEntry2.ServerState == null;
					InteractionSyncData result;
					if (flag9)
					{
						result = null;
					}
					else
					{
						orCreateInteractionEntry2.Time.Start();
						orCreateInteractionEntry2.TimeOffset = orCreateInteractionEntry2.ServerState.Progress;
						try
						{
							c.Context.InitEntry(c, orCreateInteractionEntry2, module._gameInstance);
							operation.MatchServer(module._gameInstance, clickType, hasAnyButtonClick, c.Type, c.Context);
							operation.Handle(module._gameInstance, true, orCreateInteractionEntry2.TimeOffset, c.Type, c.Context);
						}
						finally
						{
							c.Context.DeinitEntry(c, orCreateInteractionEntry2, module._gameInstance);
						}
						module.RemoveInteractionIfFinished(c, orCreateInteractionEntry2);
						result = ((orCreateInteractionEntry2.Index == rootIndex) ? null : orCreateInteractionEntry2.State);
					}
					return result;
				}, index, true);
			}
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x00115E1C File Offset: 0x0011401C
		private bool HasAnyButtonClick(InteractionType type, out InteractionModule.ClickType clickType, InteractionContext context)
		{
			InputBindings inputBindings = this._gameInstance.App.Settings.InputBindings;
			InputBinding binding;
			bool flag;
			switch (type)
			{
			case 0:
				binding = inputBindings.PrimaryItemAction;
				flag = (this._gameInstance.Input.CanConsumeBinding(inputBindings.SecondaryItemAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.BlockInteractAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.PickBlock, false));
				break;
			case 1:
				binding = inputBindings.SecondaryItemAction;
				flag = (this._gameInstance.Input.CanConsumeBinding(inputBindings.PrimaryItemAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.BlockInteractAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.PickBlock, false));
				break;
			case 2:
				binding = inputBindings.TertiaryItemAction;
				flag = this._gameInstance.Input.CanConsumeBinding(inputBindings.TertiaryItemAction, false);
				break;
			case 3:
				binding = inputBindings.Ability1ItemAction;
				flag = this._gameInstance.Input.CanConsumeBinding(inputBindings.Ability1ItemAction, false);
				break;
			case 4:
				binding = inputBindings.Ability2ItemAction;
				flag = this._gameInstance.Input.CanConsumeBinding(inputBindings.Ability2ItemAction, false);
				break;
			case 5:
				binding = inputBindings.Ability3ItemAction;
				flag = this._gameInstance.Input.CanConsumeBinding(inputBindings.Ability3ItemAction, false);
				break;
			case 6:
			case 10:
			case 11:
			case 12:
			case 13:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
				clickType = InteractionModule.ClickType.None;
				return false;
			case 7:
				binding = inputBindings.BlockInteractAction;
				flag = (this._gameInstance.Input.CanConsumeBinding(inputBindings.PrimaryItemAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.SecondaryItemAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.PickBlock, false));
				break;
			case 8:
				binding = inputBindings.PickBlock;
				flag = (this._gameInstance.Input.CanConsumeBinding(inputBindings.PrimaryItemAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.SecondaryItemAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.BlockInteractAction, false));
				break;
			case 9:
				clickType = InteractionModule.ClickType.None;
				return this._gameInstance.Input.CanConsumeBinding(inputBindings.PrimaryItemAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.SecondaryItemAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.BlockInteractAction, false) || this._gameInstance.Input.CanConsumeBinding(inputBindings.PickBlock, false);
			case 14:
			case 15:
				binding = inputBindings.GetHotbarSlot(context.MetaStore.TargetSlot.Value);
				flag = false;
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
			bool flag2 = this._gameInstance.Input.IsBindingHeld(binding, false);
			if (flag2)
			{
				bool flag3 = this._gameInstance.Input.CanConsumeBinding(binding, false);
				clickType = (flag3 ? InteractionModule.ClickType.Single : InteractionModule.ClickType.Held);
				flag = (flag || flag3);
			}
			else
			{
				clickType = InteractionModule.ClickType.None;
			}
			return flag;
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x0011619C File Offset: 0x0011439C
		private void PutChain(int chainId, ForkedChainId forkedChainId, InteractionChain val)
		{
			bool flag = forkedChainId == null;
			if (flag)
			{
				this.Chains.Add(chainId, val);
			}
			else
			{
				InteractionChain interactionChain;
				bool flag2 = this.Chains.TryGetValue(chainId, out interactionChain);
				if (flag2)
				{
					while (forkedChainId.ForkedId != null)
					{
						bool flag3 = !interactionChain.GetForkedChain(forkedChainId, out interactionChain);
						if (flag3)
						{
							InteractionModule.Logger.Warn("Missing middle chain for fork");
							return;
						}
						forkedChainId = forkedChainId.ForkedId;
					}
					interactionChain.PutForkedChain(forkedChainId, val);
				}
				else
				{
					InteractionModule.Logger.Warn("Missing primary chain for fork");
				}
			}
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x00116234 File Offset: 0x00114434
		private void CancelChains(int chainId, ForkedChainId forkedChainId)
		{
			bool flag = forkedChainId == null;
			if (flag)
			{
				InteractionChain chain;
				bool flag2 = this.Chains.TryGetValue(chainId, out chain);
				if (flag2)
				{
					this.CancelChains(chain);
				}
			}
			else
			{
				InteractionChain interactionChain;
				bool flag3 = this.Chains.TryGetValue(chainId, out interactionChain);
				if (flag3)
				{
					while (forkedChainId.ForkedId != null)
					{
						bool flag4 = !interactionChain.GetForkedChain(forkedChainId, out interactionChain);
						if (flag4)
						{
							InteractionModule.Logger.Warn("Missing middle chain for fork");
							return;
						}
						forkedChainId = forkedChainId.ForkedId;
					}
					InteractionChain chain2;
					bool forkedChain = interactionChain.GetForkedChain(forkedChainId, out chain2);
					if (forkedChain)
					{
						this.CancelChains(chain2);
					}
				}
				else
				{
					InteractionModule.Logger.Warn("Missing primary chain for fork");
				}
			}
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x001162F0 File Offset: 0x001144F0
		private void CancelChains(InteractionChain chain)
		{
			this.CancelChain(chain.ChainId, chain.ForkedChainId);
			foreach (InteractionChain chain2 in chain.ForkedChains.Values)
			{
				this.CancelChains(chain2);
			}
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x00116364 File Offset: 0x00114564
		private void CancelChain(int chainId, ForkedChainId forkedChainId)
		{
			bool flag = forkedChainId == null;
			if (flag)
			{
				InteractionChain interactionChain;
				bool flag2 = this.Chains.TryGetValue(chainId, out interactionChain);
				if (flag2)
				{
					interactionChain.ServerCancelled = true;
				}
			}
			else
			{
				InteractionChain interactionChain2;
				bool flag3 = this.Chains.TryGetValue(chainId, out interactionChain2);
				if (flag3)
				{
					while (forkedChainId.ForkedId != null)
					{
						bool flag4 = !interactionChain2.GetForkedChain(forkedChainId, out interactionChain2);
						if (flag4)
						{
							InteractionModule.Logger.Warn("Missing middle chain for fork");
							return;
						}
						forkedChainId = forkedChainId.ForkedId;
					}
					InteractionChain interactionChain3;
					bool forkedChain = interactionChain2.GetForkedChain(forkedChainId, out interactionChain3);
					if (forkedChain)
					{
						interactionChain3.ServerCancelled = true;
					}
				}
				else
				{
					InteractionModule.Logger.Warn("Missing primary chain for fork");
				}
			}
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x00116420 File Offset: 0x00114620
		private void InterruptChains(int chainId, ForkedChainId forkedChainId)
		{
			bool flag = forkedChainId == null;
			if (flag)
			{
				InteractionChain chain;
				bool flag2 = this.Chains.TryGetValue(chainId, out chain);
				if (flag2)
				{
					this.InterruptChains(chain);
				}
			}
			else
			{
				InteractionChain interactionChain;
				bool flag3 = this.Chains.TryGetValue(chainId, out interactionChain);
				if (flag3)
				{
					while (forkedChainId.ForkedId != null)
					{
						bool flag4 = !interactionChain.GetForkedChain(forkedChainId, out interactionChain);
						if (flag4)
						{
							InteractionModule.Logger.Warn("Missing middle chain for fork");
							return;
						}
						forkedChainId = forkedChainId.ForkedId;
					}
					InteractionChain chain2;
					bool forkedChain = interactionChain.GetForkedChain(forkedChainId, out chain2);
					if (forkedChain)
					{
						this.InterruptChains(chain2);
					}
				}
				else
				{
					InteractionModule.Logger.Warn("Missing primary chain for fork");
				}
			}
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x001164DC File Offset: 0x001146DC
		private void InterruptChains(InteractionChain chain)
		{
			this.InterruptChain(chain.ChainId, chain.ForkedChainId);
			foreach (InteractionChain chain2 in chain.ForkedChains.Values)
			{
				this.InterruptChains(chain2);
			}
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x00116550 File Offset: 0x00114750
		private void InterruptChain(int chainId, ForkedChainId forkedChainId)
		{
			bool flag = forkedChainId == null;
			if (flag)
			{
				InteractionChain interactionChain;
				bool flag2 = this.Chains.TryGetValue(chainId, out interactionChain);
				if (flag2)
				{
					bool flag3 = interactionChain.ClientState == 4;
					if (flag3)
					{
						interactionChain.ClientState = 1;
					}
					InteractionEntry interactionEntry;
					bool interaction = interactionChain.GetInteraction(interactionChain.OperationIndex, out interactionEntry);
					if (interaction)
					{
						bool flag4 = interactionEntry.State.State == 4;
						if (flag4)
						{
							interactionEntry.State.State = 1;
						}
					}
				}
			}
			else
			{
				InteractionChain interactionChain2;
				bool flag5 = this.Chains.TryGetValue(chainId, out interactionChain2);
				if (flag5)
				{
					while (forkedChainId.ForkedId != null)
					{
						bool flag6 = !interactionChain2.GetForkedChain(forkedChainId, out interactionChain2);
						if (flag6)
						{
							InteractionModule.Logger.Warn("Interrupt: Missing middle chain for fork");
							return;
						}
						forkedChainId = forkedChainId.ForkedId;
					}
					InteractionChain interactionChain3;
					bool forkedChain = interactionChain2.GetForkedChain(forkedChainId, out interactionChain3);
					if (forkedChain)
					{
						bool flag7 = interactionChain3.ClientState == 4;
						if (flag7)
						{
							interactionChain3.ClientState = 1;
						}
						InteractionEntry interactionEntry2;
						bool interaction2 = interactionChain3.GetInteraction(interactionChain3.OperationIndex, out interactionEntry2);
						if (interaction2)
						{
							bool flag8 = interactionEntry2.State.State == 4;
							if (flag8)
							{
								interactionEntry2.State.State = 1;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x0011668C File Offset: 0x0011488C
		public void Handle(SyncInteractionChain packet)
		{
			bool flag = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
			if (flag)
			{
				InteractionModule.Logger.Trace(string.Format("Receive Sync Packet: {0}", packet));
			}
			bool flag2 = packet.OverrideRootInteraction != int.MinValue && packet.ForkedId != null;
			InteractionChain interactionChain = null;
			bool flag3 = this.Chains.TryGetValue(packet.ChainId, out interactionChain);
			if (flag3)
			{
				ForkedChainId forkedId = packet.ForkedId;
				while (forkedId != null)
				{
					InteractionChain interactionChain2 = interactionChain;
					bool flag4 = !interactionChain.GetForkedChain(forkedId, out interactionChain);
					if (flag4)
					{
						bool flag5 = flag2;
						if (flag5)
						{
							interactionChain = null;
							break;
						}
						InteractionChain.TempChain tempForkedChain = interactionChain2.GetTempForkedChain(forkedId);
						for (forkedId = forkedId.ForkedId; forkedId != null; forkedId = forkedId.ForkedId)
						{
							tempForkedChain = tempForkedChain.GetTempForkedChain(forkedId);
						}
						this.Sync(tempForkedChain, packet);
						return;
					}
					else
					{
						forkedId = forkedId.ForkedId;
					}
				}
			}
			bool flag6 = interactionChain == null && packet.ForkedId != null && !flag2;
			if (flag6)
			{
				InteractionModule.Logger.Info("Ignoring incorrect fork. Assuming it was cancelled.");
			}
			else
			{
				bool flag7 = interactionChain == null;
				if (flag7)
				{
					this.SyncStart(packet);
				}
				else
				{
					this.Sync(interactionChain, packet);
				}
			}
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x001167D0 File Offset: 0x001149D0
		public void Handle(CancelInteractionChain packet)
		{
			bool flag = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
			if (flag)
			{
				InteractionModule.Logger.Trace(string.Format("Receive Cancel Packet: {0}", packet));
			}
			this.CancelChain(packet.ChainId, packet.ForkedId);
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x0011681C File Offset: 0x00114A1C
		private void SyncStart(SyncInteractionChain packet)
		{
			int chainId = packet.ChainId;
			InteractionType interactionType_ = packet.InteractionType_;
			bool flag = !packet.Initial;
			if (flag)
			{
				InteractionModule.Logger.Warn(string.Format("Got SyncStart for {0} but packet wasn't the first.", chainId));
			}
			else
			{
				bool flag2 = packet.ForkedId == null;
				if (flag2)
				{
					bool flag3 = chainId >= 0;
					if (flag3)
					{
						bool flag4 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
						if (flag4)
						{
							InteractionModule.Logger.Trace(string.Format("Invalid server chainId! Got {0} but server id's should be < 0", chainId));
						}
						return;
					}
					bool flag5 = chainId >= this._lastServerChainId;
					if (flag5)
					{
						bool flag6 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
						if (flag6)
						{
							InteractionModule.Logger.Trace(string.Format("Invalid server chainId! The last serverChainId was {0} but just got {1}", this._lastServerChainId, chainId));
						}
						return;
					}
					this._lastServerChainId = chainId;
				}
				InventoryModule inventoryModule = this._gameInstance.InventoryModule;
				InteractionType type = interactionType_;
				bool flag7 = packet.ForkedId != null;
				if (flag7)
				{
					InteractionChain interactionChain;
					bool flag8 = this.Chains.TryGetValue(packet.ChainId, out interactionChain);
					if (flag8)
					{
						type = interactionChain.Type;
					}
				}
				InteractionContext interactionContext = InteractionContext.ForInteraction(this._gameInstance, inventoryModule, type, null);
				int overrideRootInteraction = packet.OverrideRootInteraction;
				bool flag9 = packet.OverrideRootInteraction == int.MinValue;
				if (flag9)
				{
					bool flag10 = !interactionContext.GetRootInteractionId(this._gameInstance, interactionType_, out overrideRootInteraction);
					if (flag10)
					{
						throw new Exception("Missing root interaction");
					}
				}
				ClientRootInteraction rootInteraction = this.RootInteractions[overrideRootInteraction];
				BlockPosition targetBlock = null;
				bool flag11 = packet.Data.BlockPosition_ != null;
				if (flag11)
				{
					targetBlock = this._gameInstance.MapModule.GetBaseBlock(packet.Data.BlockPosition_);
				}
				bool flag12 = packet.Data.BlockPosition_ != null;
				if (flag12)
				{
					interactionContext.MetaStore.TargetBlockRaw = packet.Data.BlockPosition_;
					interactionContext.MetaStore.TargetBlock = targetBlock;
				}
				bool flag13 = packet.Data.EntityId != -1;
				if (flag13)
				{
					interactionContext.MetaStore.TargetEntity = this._gameInstance.EntityStoreModule.GetEntity(packet.Data.EntityId);
				}
				bool flag14 = packet.Data.HitLocation != null;
				if (flag14)
				{
					interactionContext.MetaStore.HitLocation = new Vector4?(new Vector4(packet.Data.HitLocation.X, packet.Data.HitLocation.Y, packet.Data.HitLocation.Z, 0f));
				}
				bool flag15 = packet.Data.TargetSlot != int.MinValue;
				if (flag15)
				{
					bool flag16 = packet.Data.TargetSlot < 0;
					if (flag16)
					{
						interactionContext.MetaStore.TargetSlot = new int?(-(packet.Data.TargetSlot + 1));
						interactionContext.MetaStore.DisableSlotFork = true;
					}
					else
					{
						interactionContext.MetaStore.TargetSlot = new int?(packet.Data.TargetSlot);
					}
				}
				ForkedChainId forkedId = packet.ForkedId;
				while (((forkedId != null) ? forkedId.ForkedId : null) != null)
				{
					forkedId = forkedId.ForkedId;
				}
				InteractionChain interactionChain2 = new InteractionChain(packet.ForkedId, forkedId, interactionType_, interactionContext, packet.Data, rootInteraction, 0, null, null);
				interactionChain2.ChainId = chainId;
				interactionChain2.Time.Start();
				this.Sync(interactionChain2, packet);
				bool flag17 = this.TickChain(interactionChain2);
				if (!flag17)
				{
					bool flag18 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
					if (flag18)
					{
						InteractionModule.Logger.Trace(string.Format("Add Chain: {0}, {1}", chainId, interactionChain2));
					}
					this.PutChain(chainId, packet.ForkedId, interactionChain2);
				}
			}
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x00116C0C File Offset: 0x00114E0C
		public void Sync(InteractionModule.ChainSyncStorage chain, SyncInteractionChain packet)
		{
			bool flag = chain.ServerState != 4 && packet.State == 4;
			if (flag)
			{
				throw new Exception(string.Format("Tried to change serverState on chain that is already Finished/Failed to NotFinished! {0}, {1}", chain, packet));
			}
			bool flag2 = packet.NewForks != null;
			if (flag2)
			{
				foreach (SyncInteractionChain packet2 in packet.NewForks)
				{
					chain.SyncFork(this._gameInstance, packet2);
				}
			}
			chain.ServerState = packet.State;
			bool flag3 = packet.InteractionData == null;
			if (!flag3)
			{
				InteractionChain interactionChain = chain as InteractionChain;
				bool flag4 = interactionChain != null;
				if (flag4)
				{
					interactionChain.ServerAck = true;
				}
				for (int j = 0; j < packet.InteractionData.Length; j++)
				{
					InteractionSyncData interactionSyncData = packet.InteractionData[j];
					bool flag5 = interactionSyncData == null;
					if (!flag5)
					{
						int num = packet.OperationBaseIndex + j;
						if (interactionSyncData.State == 4)
						{
							goto IL_FF;
						}
						InteractionChain interactionChain2 = chain as InteractionChain;
						if (interactionChain2 == null)
						{
							goto IL_FF;
						}
						bool flag6 = num > interactionChain2.ServerCompleteIndex;
						IL_100:
						bool flag7 = flag6;
						if (flag7)
						{
							interactionChain2.ServerCompleteIndex = num;
						}
						InteractionChain interactionChain3 = chain as InteractionChain;
						bool flag8 = interactionChain3 != null;
						if (!flag8)
						{
							chain.PutInteractionSyncData(num, interactionSyncData);
							goto IL_2D1;
						}
						InteractionEntry interactionEntry;
						bool flag9 = !interactionChain3.GetInteraction(num, out interactionEntry);
						if (flag9)
						{
							bool flag10 = interactionChain3.ClientState != 4;
							if (flag10)
							{
								InteractionModule.Logger.Warn(string.Format("Client finished while the server continued. Rolling back {0} - {1}", interactionChain3.ChainId, interactionChain3.ForkedChainId));
								this.OnSyncFailed(interactionChain3, packet, j, num, null);
								return;
							}
							chain.PutInteractionSyncData(num, interactionSyncData);
							goto IL_2D1;
						}
						else
						{
							bool flag11 = interactionEntry.ServerState != null && interactionEntry.ServerState.State != 4 && interactionSyncData.State == 4;
							if (flag11)
							{
								throw new Exception(string.Format("Tried to change syncData on interaction that is already Finished/Failed to NotFinished! {0}, {1}", interactionEntry, interactionSyncData));
							}
							bool flag12 = interactionEntry.State.OperationCounter != interactionSyncData.OperationCounter || interactionEntry.State.RootInteraction != interactionSyncData.RootInteraction;
							if (flag12)
							{
								this.OnSyncFailed(interactionChain3, packet, j, num, new int?(interactionSyncData.RootInteraction));
								return;
							}
							interactionChain3.UpdateSyncPosition(num);
							interactionEntry.ServerState = interactionSyncData;
							bool flag13 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
							if (flag13)
							{
								Logger logger = InteractionModule.Logger;
								string format = "{0}: Time (Sync) - Client: {1} vs Server: {2}";
								object arg = num;
								object arg2 = interactionEntry.Time.Elapsed.TotalSeconds;
								InteractionSyncData serverState = interactionEntry.ServerState;
								logger.Trace(string.Format(format, arg, arg2, (serverState != null) ? serverState.Progress.ToString(CultureInfo.InvariantCulture) : null));
							}
							this.RemoveInteractionIfFinished(interactionChain3, interactionEntry);
							goto IL_2D1;
						}
						IL_FF:
						flag6 = false;
						goto IL_100;
					}
					IL_2D1:;
				}
				InteractionChain interactionChain4 = chain as InteractionChain;
				bool flag14 = interactionChain4 == null;
				if (!flag14)
				{
					int serverCompleteIndex = interactionChain4.ServerCompleteIndex;
					bool flag15 = interactionChain4.ClientState != 4 && interactionChain4.ServerState != 4 && interactionChain4.OperationIndex != serverCompleteIndex + 1;
					if (flag15)
					{
						InteractionModule.Logger.Warn(string.Format("Client finished incorrectly. Rolling back to: {0} for {1} - {2}", serverCompleteIndex, interactionChain4.ChainId, interactionChain4.ForkedChainId));
						this.DoRollback(interactionChain4, serverCompleteIndex, null);
					}
					bool flag16 = interactionChain4.ClientState == 4 && interactionChain4.ServerState != 4 && interactionChain4.OperationIndex >= serverCompleteIndex + 1;
					if (flag16)
					{
						InteractionModule.Logger.Warn(string.Format("Client went down a different path. Rolling back to: {0} for {1} - {2}", serverCompleteIndex, interactionChain4.ChainId, interactionChain4.ForkedChainId));
						this.DoRollback(interactionChain4, serverCompleteIndex, null);
					}
				}
			}
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x00116FFC File Offset: 0x001151FC
		private void OnSyncFailed(InteractionChain interactionChain, SyncInteractionChain packet, int offset, int index, int? realRoot = null)
		{
			for (int i = offset; i < packet.InteractionData.Length; i++)
			{
				interactionChain.PutInteractionSyncData(packet.OperationBaseIndex + i, packet.InteractionData[i]);
			}
			int? rewriteRoot = null;
			bool flag = index == 0;
			if (flag)
			{
				bool flag2 = realRoot == null;
				if (flag2)
				{
					throw new Exception("Failed to start chain correctly");
				}
				rewriteRoot = new int?(realRoot.Value);
			}
			this.DoRollback(interactionChain, index - 1, rewriteRoot);
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00117084 File Offset: 0x00115284
		public bool CanRun(InteractionType type, int equipSlot, ClientRootInteraction rootInteraction)
		{
			List<InteractionChain> list = null;
			return this.ApplyRules<int>(null, type, equipSlot, rootInteraction, this.Chains, list);
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x001170AC File Offset: 0x001152AC
		public bool ApplyRules(InteractionContext context, InteractionChainData data, InteractionType type, ClientRootInteraction rootInteraction)
		{
			List<InteractionChain> list = new List<InteractionChain>();
			bool flag = !this.ApplyRules<int>(data, type, (context != null) ? context.HeldItemSlot : 0, rootInteraction, this.Chains, list);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				list.ForEach(delegate(InteractionChain chain)
				{
					this.InterruptChains(chain.ChainId, chain.ForkedChainId);
				});
				result = true;
			}
			return result;
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x00117104 File Offset: 0x00115304
		private bool ApplyRules<T>(InteractionChainData data, InteractionType type, int heldItemSlot, ClientRootInteraction rootInteraction, Dictionary<T, InteractionChain> chains, in List<InteractionChain> chainsToCancel)
		{
			bool flag = chains.Count == 0 || rootInteraction == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				foreach (InteractionChain interactionChain in chains.Values)
				{
					bool flag2 = interactionChain.ForkedChainId != null && !interactionChain.Predicted;
					if (!flag2)
					{
						bool flag3 = data != null && interactionChain.ChainData.ProxyId != data.ProxyId;
						if (!flag3)
						{
							bool flag4 = type == 24 && interactionChain.Type == 24 && interactionChain.Context.HeldItemSlot != heldItemSlot;
							if (!flag4)
							{
								bool flag5 = interactionChain.ClientState == 4;
								if (flag5)
								{
									ClientRootInteraction.Operation operation = interactionChain.RootInteraction.Operations[interactionChain.OperationCounter];
									ClientInteraction.ClientInteractionRules otherRules;
									HashSet<int> otherTags;
									bool flag6 = operation.TryGetRules(this._gameInstance, out otherRules, out otherTags);
									bool flag7 = rootInteraction.Rules.ValidateInterrupts(type, rootInteraction.Tags, interactionChain.Type, interactionChain.RootInteraction.Tags, interactionChain.RootInteraction.Rules);
									if (flag7)
									{
										List<InteractionChain> list = chainsToCancel;
										if (list != null)
										{
											list.Add(interactionChain);
										}
									}
									else
									{
										bool flag8 = flag6 && rootInteraction.Rules.ValidateInterrupts(type, rootInteraction.Tags, interactionChain.Type, otherTags, otherRules);
										if (flag8)
										{
											List<InteractionChain> list2 = chainsToCancel;
											if (list2 != null)
											{
												list2.Add(interactionChain);
											}
										}
										else
										{
											bool flag9 = rootInteraction.Rules.ValidateBlocked(type, rootInteraction.Tags, interactionChain.Type, interactionChain.RootInteraction.Tags, interactionChain.RootInteraction.Rules);
											if (flag9)
											{
												return false;
											}
											bool flag10 = flag6 && rootInteraction.Rules.ValidateBlocked(type, rootInteraction.Tags, interactionChain.Type, otherTags, otherRules);
											if (flag10)
											{
												return false;
											}
										}
									}
								}
								bool flag11 = (chainsToCancel == null || chainsToCancel.Count == 0) && !this.ApplyRules<ulong>(data, type, heldItemSlot, rootInteraction, interactionChain.ForkedChains, chainsToCancel);
								if (flag11)
								{
									return false;
								}
							}
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x00117368 File Offset: 0x00115568
		public bool StartChain(InteractionType type, InteractionModule.ClickType clickType, Action onCompletion)
		{
			InventoryModule inventoryModule = this._gameInstance.InventoryModule;
			InteractionContext context = InteractionContext.ForInteraction(this._gameInstance, inventoryModule, type, null);
			return this.StartChain(context, type, clickType, onCompletion, null, null, null);
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x001173BC File Offset: 0x001155BC
		public bool StartChain(InteractionContext context, InteractionType type, InteractionModule.ClickType clickType, Action onCompletion, int? targetEntityId = null, Vector4? hitPosition = null, string hitDetail = null)
		{
			int num;
			bool flag = !context.GetRootInteractionId(this._gameInstance, type, out num);
			bool result;
			if (flag)
			{
				bool flag2 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
				if (flag2)
				{
					Logger logger = InteractionModule.Logger;
					string format = "No interactions defined for {0} for item {1}";
					object arg = type;
					ClientItemStack heldItem = context.HeldItem;
					logger.Trace(string.Format(format, arg, (heldItem != null) ? heldItem.Id : null));
				}
				result = false;
			}
			else
			{
				ClientRootInteraction clientRootInteraction = this.RootInteractions[num];
				BlockPosition blockPosition = (this.InteractionTarget == InteractionModule.InteractionTargetType.Block) ? new BlockPosition((int)Math.Floor((double)this.TargetBlockHit.BlockPosition.X), (int)Math.Floor((double)this.TargetBlockHit.BlockPosition.Y), (int)Math.Floor((double)this.TargetBlockHit.BlockPosition.Z)) : null;
				BlockPosition targetBlock = null;
				bool flag3 = blockPosition != null;
				if (flag3)
				{
					targetBlock = this._gameInstance.MapModule.GetBaseBlock(blockPosition);
				}
				int num2 = targetEntityId ?? ((this.InteractionTarget == InteractionModule.InteractionTargetType.Entity) ? this.TargetEntityHit.NetworkId : -1);
				context.MetaStore.TargetBlock = targetBlock;
				context.MetaStore.TargetBlockRaw = blockPosition;
				context.MetaStore.TargetEntity = this._gameInstance.EntityStoreModule.GetEntity(num2);
				context.MetaStore.HitLocation = hitPosition;
				context.MetaStore.HitDetail = hitDetail;
				bool flag4 = context.Entity != this._gameInstance.LocalPlayer;
				Guid guid;
				if (flag4)
				{
					guid = context.Entity.PredictionId.Value;
				}
				else
				{
					guid = GuidSerializer.GetDefault();
				}
				Vector3f vector3f = null;
				bool flag5 = hitPosition != null;
				if (flag5)
				{
					Vector4 value = hitPosition.Value;
					vector3f = new Vector3f(value.X, value.Y, value.Z);
				}
				InteractionChainData interactionChainData = new InteractionChainData(num2, guid, vector3f, hitDetail, blockPosition, context.MetaStore.TargetSlot.GetValueOrDefault(int.MinValue));
				List<InteractionChain> list = new List<InteractionChain>();
				bool flag6 = !this.ApplyRules<int>(interactionChainData, type, context.HeldItemSlot, clientRootInteraction, this.Chains, list);
				if (flag6)
				{
					bool flag7 = clientRootInteraction.RootInteraction.ClickQueuingTimeout > 0f && clickType == InteractionModule.ClickType.Single;
					if (flag7)
					{
						this.QueueClick(context, type, clientRootInteraction.RootInteraction.ClickQueuingTimeout);
					}
					result = false;
				}
				else
				{
					bool flag8;
					this.UpdateCooldown(clientRootInteraction, clickType == InteractionModule.ClickType.Single, out flag8);
					bool flag9 = flag8;
					if (flag9)
					{
						bool flag10 = clickType == InteractionModule.ClickType.Single;
						if (flag10)
						{
							this._gameInstance.App.Interface.InGameView.AbilitiesHudComponent.CooldownError(clientRootInteraction);
							bool flag11 = clientRootInteraction.RootInteraction.ClickQueuingTimeout > 0f;
							if (flag11)
							{
								this.QueueClick(context, type, clientRootInteraction.RootInteraction.ClickQueuingTimeout);
							}
						}
						result = false;
					}
					else
					{
						int hotbarActiveSlot = this._gameInstance.InventoryModule.HotbarActiveSlot;
						ClientItemStack hotbarItem = this._gameInstance.InventoryModule.GetHotbarItem(this._gameInstance.InventoryModule.HotbarActiveSlot);
						int num3 = this._lastClientChainId + 1;
						this._lastClientChainId = num3;
						int num4 = num3;
						bool flag12 = num4 < 0;
						if (flag12)
						{
							num4 = (this._lastClientChainId = 1);
						}
						InteractionChain interactionChain = new InteractionChain(type, context, interactionChainData, clientRootInteraction, hotbarActiveSlot, hotbarItem, onCompletion);
						interactionChain.Predicted = true;
						interactionChain.Time.Start();
						interactionChain.ChainId = num4;
						bool requireNewClick = clientRootInteraction.RootInteraction.RequireNewClick;
						if (requireNewClick)
						{
							this.RequireNewClick(type);
						}
						bool flag13 = this.TickChain(interactionChain);
						list.ForEach(delegate(InteractionChain c)
						{
							this.InterruptChains(c.ChainId, c.ForkedChainId);
						});
						bool flag14 = flag13;
						if (flag14)
						{
							bool flag15 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
							if (flag15)
							{
								InteractionModule.Logger.Trace(string.Format("Finished Chain: {0}, {1}, {2}", num4, interactionChain, interactionChain.ClientState));
							}
						}
						else
						{
							bool flag16 = InteractionModule.Logger.IsEnabled(LogLevel.Trace);
							if (flag16)
							{
								InteractionModule.Logger.Trace(string.Format("Add Chain: {0}, {1}", num4, interactionChain));
							}
							this.Chains.Add(num4, interactionChain);
						}
						this._gameInstance.App.Interface.InGameView.AbilitiesHudComponent.OnStartChain(clientRootInteraction.Id);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x00117838 File Offset: 0x00115A38
		public T ForEachInteraction<T>(Func<InteractionChain, ClientInteraction, T, T> consumer, T val)
		{
			return this.ForEachInteraction<T, int>(this.Chains, consumer, val);
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x00117858 File Offset: 0x00115A58
		private T ForEachInteraction<T, TK>(Dictionary<TK, InteractionChain> chains, Func<InteractionChain, ClientInteraction, T, T> consumer, T val)
		{
			foreach (InteractionChain interactionChain in chains.Values)
			{
				bool flag = interactionChain.OperationCounter >= interactionChain.RootInteraction.Operations.Length;
				if (!flag)
				{
					ClientRootInteraction.Operation operation = interactionChain.RootInteraction.Operations[interactionChain.OperationCounter];
					ClientRootInteraction.InteractionWrapper interactionWrapper = operation as ClientRootInteraction.InteractionWrapper;
					bool flag2 = interactionWrapper != null;
					if (flag2)
					{
						val = consumer(interactionChain, interactionWrapper.GetInteraction(this), val);
					}
					val = this.ForEachInteraction<T, ulong>(interactionChain.ForkedChains, consumer, val);
				}
			}
			return val;
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x00117918 File Offset: 0x00115B18
		private void SendSyncPacket(InteractionChain chain, int operationBaseIndex, List<InteractionSyncData> interactionData, bool force = false)
		{
			bool flag;
			if (!force && chain.SentInitialState)
			{
				if (interactionData != null)
				{
					if (!Enumerable.All<InteractionSyncData>(interactionData, (InteractionSyncData v) => v == null))
					{
						goto IL_47;
					}
				}
				flag = (chain.NewForks.Count == 0);
				goto IL_48;
			}
			IL_47:
			flag = false;
			IL_48:
			bool flag2 = flag;
			if (!flag2)
			{
				SyncInteractionChain packet = this.MakeSyncPacket(chain, operationBaseIndex, interactionData);
				this._gameInstance.Connection.SendPacketImmediate(packet);
			}
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x00117990 File Offset: 0x00115B90
		private SyncInteractionChain MakeSyncPacket(InteractionChain chain, int operationBaseIndex, List<InteractionSyncData> interactionData)
		{
			SyncInteractionChain[] array = null;
			bool flag = chain.NewForks.Count > 0;
			if (flag)
			{
				array = new SyncInteractionChain[chain.NewForks.Count];
				for (int i = 0; i < chain.NewForks.Count; i++)
				{
					InteractionChain chain2 = chain.NewForks[i];
					array[i] = this.MakeSyncPacket(chain2, operationBaseIndex, null);
				}
				chain.NewForks.Clear();
			}
			int initialSlot = chain.InitialSlot;
			int utilityActiveSlot = this._gameInstance.InventoryModule.UtilityActiveSlot;
			int toolsActiveSlot = this._gameInstance.InventoryModule.ToolsActiveSlot;
			int consumableActiveSlot = this._gameInstance.InventoryModule.ConsumableActiveSlot;
			Item item;
			if (chain.SentInitialState)
			{
				item = null;
			}
			else
			{
				ClientItemStack initialItem = chain.InitialItem;
				item = ((initialItem != null) ? initialItem.ToItemPacket(true) : null);
			}
			Item item2;
			if (chain.SentInitialState)
			{
				item2 = null;
			}
			else
			{
				ClientItemStack utilityItem = this._gameInstance.InventoryModule.GetUtilityItem(this._gameInstance.InventoryModule.UtilityActiveSlot);
				item2 = ((utilityItem != null) ? utilityItem.ToItemPacket(true) : null);
			}
			Item item3;
			if (chain.SentInitialState)
			{
				item3 = null;
			}
			else
			{
				ClientItemStack toolsItem = this._gameInstance.InventoryModule.GetToolsItem(this._gameInstance.InventoryModule.ToolsActiveSlot);
				item3 = ((toolsItem != null) ? toolsItem.ToItemPacket(true) : null);
			}
			Item item4;
			if (chain.SentInitialState)
			{
				item4 = null;
			}
			else
			{
				ClientItemStack consumableItem = this._gameInstance.InventoryModule.GetConsumableItem(this._gameInstance.InventoryModule.ConsumableActiveSlot);
				item4 = ((consumableItem != null) ? consumableItem.ToItemPacket(true) : null);
			}
			SyncInteractionChain result = new SyncInteractionChain(initialSlot, utilityActiveSlot, toolsActiveSlot, consumableActiveSlot, item, item2, item3, item4, !chain.SentInitialState, chain.ConsumeDesync(), chain.Type, chain.Context.HeldItemSlot, chain.ChainId, chain.ForkedChainId, chain.ChainData, chain.ClientState, array, operationBaseIndex, (interactionData != null) ? interactionData.ToArray() : null);
			chain.SentInitialState = true;
			return result;
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x00117B60 File Offset: 0x00115D60
		public void SetGlobalTimeShift(InteractionType type, float shift)
		{
			bool flag = shift < 0f;
			if (flag)
			{
				throw new ArgumentException();
			}
			this._globalTimeShift[type] = shift;
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x00117B8C File Offset: 0x00115D8C
		public float GetGlobalTimeShift(InteractionType type)
		{
			return this._globalTimeShift[type];
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x00117BA8 File Offset: 0x00115DA8
		public void DrawDebugSelector(GraphicsDevice graphics, GLFunctions gl, ref Vector3 cameraPosition, ref Matrix viewProjectionMatrix)
		{
			foreach (InteractionModule.DebugSelectorMesh debugSelectorMesh in this.SelectorDebugMeshes)
			{
				Matrix matrix = debugSelectorMesh.Matrix * Matrix.CreateTranslation(-cameraPosition) * viewProjectionMatrix;
				graphics.GPUProgramStore.BasicProgram.MVPMatrix.SetValue(ref matrix);
				graphics.GPUProgramStore.BasicProgram.Opacity.SetValue(0.8f * (debugSelectorMesh.Time / debugSelectorMesh.InitialTime));
				graphics.GPUProgramStore.BasicProgram.Color.SetValue(debugSelectorMesh.DebugColor);
				gl.BindVertexArray(debugSelectorMesh.Mesh.VertexArray);
				gl.DrawElements(GL.TRIANGLES, debugSelectorMesh.Mesh.Count, GL.UNSIGNED_SHORT, IntPtr.Zero);
				gl.PolygonMode(GL.FRONT_AND_BACK, GL.LINE);
				graphics.GPUProgramStore.BasicProgram.Opacity.SetValue(1f);
				graphics.GPUProgramStore.BasicProgram.Color.SetValue(graphics.BlackColor);
				gl.DrawElements(GL.TRIANGLES, debugSelectorMesh.Mesh.Count, GL.UNSIGNED_SHORT, IntPtr.Zero);
				gl.PolygonMode(GL.FRONT_AND_BACK, GL.FILL);
				debugSelectorMesh.Time -= 0.016666668f;
			}
			this.SelectorDebugMeshes.RemoveAll(delegate(InteractionModule.DebugSelectorMesh s)
			{
				bool flag = s.Time <= 0f;
				bool result;
				if (flag)
				{
					s.Mesh.Dispose();
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			});
		}

		// Token: 0x0400245E RID: 9310
		public const float BlockSubfaceWidth = 0.35f;

		// Token: 0x0400245F RID: 9311
		private const string DefaultInteractionHint = "interactionHints.generic";

		// Token: 0x04002460 RID: 9312
		private InteractionModule.InteractionHintData _interactionHint;

		// Token: 0x04002461 RID: 9313
		public InteractionModule.InteractionTargetType InteractionTarget = InteractionModule.InteractionTargetType.None;

		// Token: 0x04002465 RID: 9317
		private const float BlockOutlineIntersectOffset = 0.005f;

		// Token: 0x04002466 RID: 9318
		public readonly BlockPlacementPreview BlockPreview;

		// Token: 0x04002467 RID: 9319
		public readonly BlockBreakHealth BlockBreakHealth;

		// Token: 0x04002468 RID: 9320
		private readonly BlockOutlineRenderer _blockOutlineRenderer;

		// Token: 0x04002469 RID: 9321
		private readonly HitDetection.RaycastOptions _targetBlockRaycastOptions = new HitDetection.RaycastOptions
		{
			IgnoreFluids = true,
			CheckOversizedBoxes = true
		};

		// Token: 0x0400246A RID: 9322
		private bool _placingAtRange = false;

		// Token: 0x0400246B RID: 9323
		private bool _fluidityActive = false;

		// Token: 0x0400246C RID: 9324
		private InteractionModule.BlockTargetInfo? _targetBlockInfo;

		// Token: 0x0400246D RID: 9325
		private static readonly Axis[] AxisValues = Enum.GetValues(typeof(Axis)) as Axis[];

		// Token: 0x0400246E RID: 9326
		private static readonly Rotation[] RotationValues = Enum.GetValues(typeof(Rotation)) as Rotation[];

		// Token: 0x0400246F RID: 9327
		private Axis _currentBlockRotationAxis = 0;

		// Token: 0x04002470 RID: 9328
		private IntVector3? _currentLockedBlockPosition = null;

		// Token: 0x04002471 RID: 9329
		private int _currentBlockId = -1;

		// Token: 0x04002472 RID: 9330
		private int _currentRotatedBlockId = -1;

		// Token: 0x04002473 RID: 9331
		private readonly Dictionary<Axis, Rotation> _rotationMatrix = Enumerable.ToDictionary<Axis, Axis, Rotation>(InteractionModule.AxisValues, (Axis axis) => axis, (Axis axis) => 0);

		// Token: 0x04002474 RID: 9332
		public InteractionModule.RotationMode CurrentRotationMode = InteractionModule.RotationMode.None;

		// Token: 0x04002475 RID: 9333
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002476 RID: 9334
		private static readonly int InteractionTypeLength = typeof(InteractionType).GetEnumNames().Length;

		// Token: 0x04002477 RID: 9335
		public const float DefaultCooldown = 0.35f;

		// Token: 0x04002478 RID: 9336
		public static readonly float[] DefaultChargeTimes = new float[1];

		// Token: 0x04002479 RID: 9337
		public bool ShowSelectorDebug = false;

		// Token: 0x0400247A RID: 9338
		public ClientRootInteraction[] RootInteractions;

		// Token: 0x0400247B RID: 9339
		public ClientInteraction[] Interactions;

		// Token: 0x0400247C RID: 9340
		private readonly Dictionary<string, Cooldown> _cooldowns = new Dictionary<string, Cooldown>();

		// Token: 0x0400247D RID: 9341
		public readonly Dictionary<int, InteractionChain> Chains = new Dictionary<int, InteractionChain>();

		// Token: 0x0400247E RID: 9342
		public readonly Random Random = new Random();

		// Token: 0x0400247F RID: 9343
		public List<DamageInfo> DamageInfos = new List<DamageInfo>();

		// Token: 0x04002480 RID: 9344
		private readonly List<InteractionSyncData> _tempSyncDataList = new List<InteractionSyncData>();

		// Token: 0x04002481 RID: 9345
		private int _lastServerChainId;

		// Token: 0x04002482 RID: 9346
		private int _lastClientChainId;

		// Token: 0x04002483 RID: 9347
		private readonly float[] _globalTimeShift = new float[InteractionModule.InteractionTypeLength];

		// Token: 0x04002484 RID: 9348
		private RotationAxis _currentRotationAxis = 0;

		// Token: 0x04002485 RID: 9349
		private int _queuedClickTypes;

		// Token: 0x04002486 RID: 9350
		private readonly InteractionModule.ClickQueueData[] _clickQueueData = new InteractionModule.ClickQueueData[InteractionModule.InteractionTypeLength];

		// Token: 0x04002487 RID: 9351
		private readonly bool[] _disabledInputs = new bool[InteractionModule.InteractionTypeLength];

		// Token: 0x04002488 RID: 9352
		private readonly bool[] _requireNewClick = new bool[InteractionModule.InteractionTypeLength];

		// Token: 0x04002489 RID: 9353
		private readonly int[] _activeSlot = new int[InteractionModule.InteractionTypeLength];

		// Token: 0x0400248A RID: 9354
		private readonly string[] _itemOnClick = new string[InteractionModule.InteractionTypeLength];

		// Token: 0x0400248B RID: 9355
		private readonly InventorySectionType[] _activeInventory = new InventorySectionType[InteractionModule.InteractionTypeLength];

		// Token: 0x0400248C RID: 9356
		public readonly List<InteractionModule.DebugSelectorMesh> SelectorDebugMeshes = new List<InteractionModule.DebugSelectorMesh>();

		// Token: 0x02000E0C RID: 3596
		public enum InteractionTargetType
		{
			// Token: 0x040044FF RID: 17663
			None,
			// Token: 0x04004500 RID: 17664
			Entity,
			// Token: 0x04004501 RID: 17665
			Block
		}

		// Token: 0x02000E0D RID: 3597
		internal readonly struct BlockTargetInfo
		{
			// Token: 0x060066C0 RID: 26304 RVA: 0x00215D32 File Offset: 0x00213F32
			public BlockTargetInfo(IntVector3 position, BoundingBox[] collidingBoxes, bool valid)
			{
				this.Position = position;
				this.CollidingBoxes = collidingBoxes;
				this.Valid = valid;
			}

			// Token: 0x060066C1 RID: 26305 RVA: 0x00215D4A File Offset: 0x00213F4A
			public BlockTargetInfo(IntVector3 position, BoundingBox? box, bool valid)
			{
				this.Position = position;
				BoundingBox[] collidingBoxes;
				if (box != null)
				{
					(collidingBoxes = new BoundingBox[1])[0] = box.Value;
				}
				else
				{
					collidingBoxes = Array.Empty<BoundingBox>();
				}
				this.CollidingBoxes = collidingBoxes;
				this.Valid = valid;
			}

			// Token: 0x060066C2 RID: 26306 RVA: 0x00215D85 File Offset: 0x00213F85
			public BlockTargetInfo(IntVector3 position, bool valid)
			{
				this.Position = position;
				this.CollidingBoxes = Array.Empty<BoundingBox>();
				this.Valid = valid;
			}

			// Token: 0x060066C3 RID: 26307 RVA: 0x00215DA4 File Offset: 0x00213FA4
			public static InteractionModule.BlockTargetInfo FromFailedBlocks(GameInstance gameInstance, IntVector3 position, IntVector3? conflict)
			{
				bool flag = conflict == null;
				InteractionModule.BlockTargetInfo result;
				if (flag)
				{
					result = new InteractionModule.BlockTargetInfo(position, false);
				}
				else
				{
					int block = gameInstance.MapModule.GetBlock(conflict.Value.X, conflict.Value.Y, conflict.Value.Z, 1);
					bool flag2 = block == 1;
					if (flag2)
					{
						result = new InteractionModule.BlockTargetInfo(position, false);
					}
					else
					{
						ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[block];
						BlockHitbox blockHitbox = gameInstance.ServerSettings.BlockHitboxes[clientBlockType.HitboxType];
						BoundingBox boundingBox = blockHitbox.BoundingBox;
						boundingBox.Translate(conflict.Value);
						result = new InteractionModule.BlockTargetInfo(position, new BoundingBox?(boundingBox), false);
					}
				}
				return result;
			}

			// Token: 0x04004502 RID: 17666
			internal readonly IntVector3 Position;

			// Token: 0x04004503 RID: 17667
			internal readonly BoundingBox[] CollidingBoxes;

			// Token: 0x04004504 RID: 17668
			internal readonly bool Valid;
		}

		// Token: 0x02000E0E RID: 3598
		public enum ClickType
		{
			// Token: 0x04004506 RID: 17670
			Single,
			// Token: 0x04004507 RID: 17671
			Held,
			// Token: 0x04004508 RID: 17672
			None
		}

		// Token: 0x02000E0F RID: 3599
		public struct InteractionHintData
		{
			// Token: 0x060066C4 RID: 26308 RVA: 0x00215E65 File Offset: 0x00214065
			public InteractionHintData(InteractionModule.InteractionTargetType target, string name, string hint)
			{
				this.Target = target;
				this.Name = name;
				this.Hint = hint;
			}

			// Token: 0x060066C5 RID: 26309 RVA: 0x00215E80 File Offset: 0x00214080
			public bool Equals(InteractionModule.InteractionHintData other)
			{
				return other.Target == this.Target && other.Name == this.Name && other.Hint == this.Hint;
			}

			// Token: 0x04004509 RID: 17673
			public static readonly InteractionModule.InteractionHintData None = new InteractionModule.InteractionHintData(InteractionModule.InteractionTargetType.None, null, null);

			// Token: 0x0400450A RID: 17674
			public InteractionModule.InteractionTargetType Target;

			// Token: 0x0400450B RID: 17675
			public string Name;

			// Token: 0x0400450C RID: 17676
			public string Hint;
		}

		// Token: 0x02000E10 RID: 3600
		internal enum RotationMode
		{
			// Token: 0x0400450E RID: 17678
			PrePlacement,
			// Token: 0x0400450F RID: 17679
			PostPlacement,
			// Token: 0x04004510 RID: 17680
			None
		}

		// Token: 0x02000E11 RID: 3601
		private struct ClickQueueData
		{
			// Token: 0x04004511 RID: 17681
			public Stopwatch Timer;

			// Token: 0x04004512 RID: 17682
			public float Timeout;

			// Token: 0x04004513 RID: 17683
			public int? TargetSlot;
		}

		// Token: 0x02000E12 RID: 3602
		public interface ChainSyncStorage
		{
			// Token: 0x1700145B RID: 5211
			// (get) Token: 0x060066C7 RID: 26311
			// (set) Token: 0x060066C8 RID: 26312
			InteractionState ServerState { get; set; }

			// Token: 0x060066C9 RID: 26313
			void PutInteractionSyncData(int index, InteractionSyncData data);

			// Token: 0x060066CA RID: 26314
			void SyncFork(GameInstance gameInstance, SyncInteractionChain packet);
		}

		// Token: 0x02000E13 RID: 3603
		private class RollbackException : Exception
		{
		}

		// Token: 0x02000E14 RID: 3604
		public class DebugSelectorMesh
		{
			// Token: 0x060066CC RID: 26316 RVA: 0x00215EE0 File Offset: 0x002140E0
			public DebugSelectorMesh(Matrix matrix, Mesh mesh, float time, Vector3 debugColor)
			{
				this.Matrix = matrix;
				this.Mesh = mesh;
				this.Time = time;
				this.InitialTime = time;
				this.DebugColor = debugColor;
			}

			// Token: 0x04004514 RID: 17684
			public readonly Matrix Matrix;

			// Token: 0x04004515 RID: 17685
			public Mesh Mesh;

			// Token: 0x04004516 RID: 17686
			public float Time;

			// Token: 0x04004517 RID: 17687
			public readonly float InitialTime;

			// Token: 0x04004518 RID: 17688
			public readonly Vector3 DebugColor;
		}
	}
}
