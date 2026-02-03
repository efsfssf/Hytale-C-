using System;
using System.Collections.Generic;
using HytaleClient.Audio;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B3E RID: 2878
	internal class ConditionalPlaceCropInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005949 RID: 22857 RVA: 0x001B6A3C File Offset: 0x001B4C3C
		public ConditionalPlaceCropInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			this.seedToCrop = interaction.SeedToCrop;
			this.tilledSoil = interaction.TilledSoilBlocks;
		}

		// Token: 0x0600594A RID: 22858 RVA: 0x001B6A60 File Offset: 0x001B4C60
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			bool flag = this.seedToCrop == null;
			if (!flag)
			{
				bool flag2 = context.MetaStore.TargetBlockRaw == null;
				if (!flag2)
				{
					ClientItemBase primaryItem = gameInstance.LocalPlayer.PrimaryItem;
					HitDetection.RaycastHit targetBlockHit = gameInstance.InteractionModule.TargetBlockHit;
					int num;
					bool flag3 = !this.seedToCrop.TryGetValue(primaryItem.Id, out num);
					if (flag3)
					{
						context.State.State = 3;
					}
					else
					{
						ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[num];
						int num2 = (int)Math.Floor((double)targetBlockHit.BlockPosition.X);
						int num3 = (int)Math.Floor((double)targetBlockHit.BlockPosition.Y);
						int num4 = (int)Math.Floor((double)targetBlockHit.BlockPosition.Z);
						int num5 = targetBlockHit.BlockId;
						ClientBlockType clientBlockType2 = gameInstance.MapModule.ClientBlockTypes[num5];
						bool flag4 = clientBlockType2.CollisionMaterial == null && clientBlockType.CollisionMaterial == 1;
						if (!flag4)
						{
							num2 = (int)Math.Floor((double)(targetBlockHit.HitPosition.X + targetBlockHit.Normal.X * 0.5f));
							num3 = (int)Math.Floor((double)(targetBlockHit.HitPosition.Y + targetBlockHit.Normal.Y * 0.5f));
							num4 = (int)Math.Floor((double)(targetBlockHit.HitPosition.Z + targetBlockHit.Normal.Z * 0.5f));
							num5 = gameInstance.MapModule.GetBlock(num2, num3, num4, int.MaxValue);
							bool flag5 = num5 == int.MaxValue;
							if (flag5)
							{
								context.State.State = 3;
								return;
							}
							clientBlockType2 = gameInstance.MapModule.ClientBlockTypes[num5];
							bool flag6 = num5 != 0 && (clientBlockType2.CollisionMaterial == 1 || (clientBlockType2.CollisionMaterial == null && clientBlockType.CollisionMaterial != 1));
							if (flag6)
							{
								context.State.State = 3;
								return;
							}
						}
						Vector3 rotation = gameInstance.CameraModule.Controller.Rotation;
						bool flag7 = clientBlockType.Variants.ContainsKey("Pitch=90");
						int num6;
						if (flag7)
						{
							bool flag8 = (float)num3 == targetBlockHit.BlockPosition.Y;
							if (flag8)
							{
								bool flag9 = (float)num4 == targetBlockHit.BlockPosition.Z;
								if (flag9)
								{
									bool flag10 = (float)num2 > targetBlockHit.BlockPosition.X || !clientBlockType.Variants.TryGetValue("Yaw=270|Pitch=90", out num6);
									if (flag10)
									{
										num6 = clientBlockType.Variants["Yaw=90|Pitch=90"];
									}
								}
								else
								{
									bool flag11 = (float)num4 > targetBlockHit.BlockPosition.Z || !clientBlockType.Variants.TryGetValue("Yaw=180|Pitch=90", out num6);
									if (flag11)
									{
										num6 = clientBlockType.Variants["Pitch=90"];
									}
								}
							}
							else
							{
								bool flag12 = (float)num3 > targetBlockHit.BlockPosition.Y || !clientBlockType.Variants.TryGetValue("Pitch=180", out num6);
								if (flag12)
								{
									num6 = (int)((ushort)primaryItem.BlockId);
								}
							}
						}
						else
						{
							bool flag13 = clientBlockType.Variants.ContainsKey("Yaw=90") && !clientBlockType.Variants.ContainsKey("Yaw=180");
							if (flag13)
							{
								bool flag14 = rotation.Y >= -0.7853982f && rotation.Y <= 0.7853982f;
								if (flag14)
								{
									num6 = (int)((ushort)primaryItem.BlockId);
								}
								else
								{
									bool flag15 = rotation.Y >= 2.3561945f || rotation.Y <= -2.3561945f;
									if (flag15)
									{
										num6 = (int)((ushort)primaryItem.BlockId);
									}
									else
									{
										num6 = clientBlockType.Variants["Yaw=90"];
									}
								}
							}
							else
							{
								string text = (rotation.Y >= -0.7853982f && rotation.Y <= 0.7853982f) ? "" : ((rotation.Y >= 0.7853982f && rotation.Y <= 2.3561945f) ? "Yaw=90" : ((rotation.Y >= 2.3561945f || rotation.Y <= -2.3561945f) ? "Yaw=180" : "Yaw=270"));
								string text2 = ((float)num3 < targetBlockHit.BlockPosition.Y) ? "Pitch=180" : "";
								string text3 = "";
								bool flag16 = text2 == "Pitch=180";
								if (flag16)
								{
									string text4 = text;
									string text5 = text4;
									if (text5 == null || text5.Length != 0)
									{
										if (!(text5 == "Yaw=180"))
										{
											if (!(text5 == "Yaw=90"))
											{
												if (text5 == "Yaw=270")
												{
													text = "Yaw=90";
												}
											}
											else
											{
												text = "Yaw=270";
											}
										}
										else
										{
											text = "";
										}
									}
									else
									{
										text = "Yaw=180";
									}
								}
								bool flag17 = text != "" && text2 != "" && text3 != "" && clientBlockType.Variants.TryGetValue(string.Concat(new string[]
								{
									text,
									"|",
									text2,
									"|",
									text3
								}), out num6);
								if (!flag17)
								{
									bool flag18 = text3 != "" && clientBlockType.Variants.TryGetValue(text3 ?? "", out num6);
									if (!flag18)
									{
										bool flag19 = text2 != "" && clientBlockType.Variants.TryGetValue(text2 ?? "", out num6);
										if (!flag19)
										{
											bool flag20 = text != "" && clientBlockType.Variants.TryGetValue(text ?? "", out num6);
											if (!flag20)
											{
												num6 = (int)((ushort)primaryItem.BlockId);
											}
										}
									}
								}
							}
						}
						ClientBlockType clientBlockType3 = gameInstance.MapModule.ClientBlockTypes[num6];
						bool flag21 = clientBlockType2.CollisionMaterial == 2;
						if (flag21)
						{
							int num7;
							bool flag22 = clientBlockType3.Variants.TryGetValue("Fluid=" + clientBlockType2.Name, out num7);
							if (flag22)
							{
								num6 = num7;
								clientBlockType3 = gameInstance.MapModule.ClientBlockTypes[num6];
							}
						}
						BlockHitbox blockHitbox = gameInstance.ServerSettings.BlockHitboxes[clientBlockType3.HitboxType];
						Entity[] allEntities = gameInstance.EntityStoreModule.GetAllEntities();
						int entitiesCount = gameInstance.EntityStoreModule.GetEntitiesCount();
						for (int i = 0; i < entitiesCount; i++)
						{
							Entity entity = allEntities[i];
							bool flag23 = entity.Type != Entity.EntityType.Character;
							if (!flag23)
							{
								BoundingBox hitbox = entity.Hitbox;
								hitbox.Translate(entity.Position);
								int num8 = num2 - clientBlockType3.FillerX;
								int num9 = num3 - clientBlockType3.FillerY;
								int num10 = num4 - clientBlockType3.FillerZ;
								for (int j = 0; j < blockHitbox.Boxes.Length; j++)
								{
									BoundingBox box = blockHitbox.Boxes[j];
									bool flag24 = hitbox.IntersectsExclusive(box, (float)num8, (float)num9, (float)num10);
									if (flag24)
									{
										context.State.State = 3;
										return;
									}
								}
							}
						}
						context.InstanceStore.OldBlockId = num5;
						int value;
						bool flag25 = gameInstance.ServerSettings.BlockSoundSets[clientBlockType.BlockSoundSetIndex].SoundEventIndices.TryGetValue(5, out value);
						if (flag25)
						{
							uint networkWwiseId = ResourceManager.GetNetworkWwiseId(value);
							bool flag26 = networkWwiseId > 0U;
							if (flag26)
							{
								Vector3 position = new Vector3((float)num2 + 0.5f, (float)num3 + 0.5f, (float)num4 + 0.5f);
								gameInstance.AudioModule.PlaySoundEvent(networkWwiseId, position, Vector3.Zero);
							}
						}
						gameInstance.MapModule.SetClientBlock(num2, num3, num4, num6);
						ClientItemStack hotbarItem = gameInstance.InventoryModule.GetHotbarItem(gameInstance.InventoryModule.HotbarActiveSlot);
						bool flag27 = gameInstance.GameMode == null && hotbarItem != null && hotbarItem.Quantity == 1;
						if (flag27)
						{
							context.HeldItem = null;
						}
						context.State.BlockPosition_ = new BlockPosition(num2, num3, num4);
						context.State.BlockRotation_ = new BlockRotation(clientBlockType3.RotationYaw, clientBlockType3.RotationPitch, clientBlockType3.RotationRoll);
					}
				}
			}
		}

		// Token: 0x0600594B RID: 22859 RVA: 0x001B72E4 File Offset: 0x001B54E4
		public override void Handle(GameInstance gameInstance, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			base.Handle(gameInstance, firstRun, time, type, context);
			InteractionSyncData state = context.State;
			bool flag = state.State == 3;
			if (flag)
			{
				int oldBlockId = context.InstanceStore.OldBlockId;
				bool flag2 = ((state != null) ? state.BlockPosition_ : null) == null || oldBlockId == int.MaxValue;
				if (!flag2)
				{
					gameInstance.MapModule.SetClientBlock(state.BlockPosition_.X, state.BlockPosition_.Y, state.BlockPosition_.Z, oldBlockId);
				}
			}
		}

		// Token: 0x04003767 RID: 14183
		private readonly Dictionary<string, int> seedToCrop;

		// Token: 0x04003768 RID: 14184
		private readonly int[] tilledSoil;
	}
}
