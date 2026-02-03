using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Characters;
using HytaleClient.Data.FX;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Data.Map.Chunk;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Map;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.Audio;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.InGame.Modules.Map
{
	// Token: 0x0200090B RID: 2315
	internal class MapModule : Module
	{
		// Token: 0x060045A0 RID: 17824 RVA: 0x000F9B04 File Offset: 0x000F7D04
		private void SetBlockInteractionState(Chunk chunk, int blockId, int blockIndex, Vector3 position, bool isDone, bool playInteractionStateSound)
		{
			ClientBlockType blockType = this.ClientBlockTypes[blockId];
			ChunkData.InteractionStateInfo interactionStateInfo = new ChunkData.InteractionStateInfo
			{
				BlockId = blockId,
				BlockType = blockType,
				StateFrameTime = (float)(isDone ? -2 : -1),
				SoundEventReference = AudioDevice.SoundEventReference.None
			};
			ChunkData.InteractionStateInfo interactionStateInfo2;
			bool flag = chunk.Data.CurrentInteractionStates.TryGetValue(blockIndex, out interactionStateInfo2);
			if (flag)
			{
				interactionStateInfo.SoundEventReference.SoundObjectReference = interactionStateInfo2.SoundEventReference.SoundObjectReference;
				bool flag2 = interactionStateInfo2.BlockType.SoundEventIndex == interactionStateInfo.BlockType.SoundEventIndex;
				if (flag2)
				{
					interactionStateInfo.SoundEventReference.PlaybackId = interactionStateInfo2.SoundEventReference.PlaybackId;
				}
				else
				{
					bool flag3 = interactionStateInfo2.SoundEventReference.PlaybackId != -1;
					if (flag3)
					{
						this._gameInstance.AudioModule.ActionOnEvent(ref interactionStateInfo2.SoundEventReference, 0);
					}
				}
				bool flag4 = interactionStateInfo.BlockType.BlockyAnimation != null && interactionStateInfo.BlockType.BlockyAnimation == interactionStateInfo2.BlockType.BlockyAnimation && interactionStateInfo.BlockType.Looping && interactionStateInfo2.BlockType.Looping;
				if (flag4)
				{
					interactionStateInfo.StateFrameTime = interactionStateInfo2.StateFrameTime;
				}
			}
			position += new Vector3(0.5f);
			bool flag5 = playInteractionStateSound && interactionStateInfo.SoundEventReference.PlaybackId == -1;
			if (flag5)
			{
				this.PlayBlockInteractionStateSound(ref interactionStateInfo, position, isDone);
			}
			bool flag6 = interactionStateInfo.SoundEventReference.SoundObjectReference.SoundObjectId != 0U && interactionStateInfo.SoundEventReference.PlaybackId == -1;
			if (flag6)
			{
				this._gameInstance.AudioModule.UnregisterSoundObject(ref interactionStateInfo.SoundEventReference.SoundObjectReference);
			}
			chunk.Data.CurrentInteractionStates[blockIndex] = interactionStateInfo;
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x000F9CE0 File Offset: 0x000F7EE0
		private void PlayBlockInteractionStateSound(ref ChunkData.InteractionStateInfo interactionInfo, Vector3 position, bool isDone)
		{
			bool flag = isDone && !interactionInfo.BlockType.Looping;
			if (!flag)
			{
				uint soundEventIndex = interactionInfo.BlockType.SoundEventIndex;
				bool flag2 = soundEventIndex > 0U;
				if (flag2)
				{
					bool flag3 = interactionInfo.SoundEventReference.SoundObjectReference.SoundObjectId == 0U;
					if (flag3)
					{
						this._gameInstance.AudioModule.TryRegisterSoundObject(position, Vector3.Zero, ref interactionInfo.SoundEventReference.SoundObjectReference, true);
					}
					this._gameInstance.AudioModule.PlaySoundEvent(soundEventIndex, interactionInfo.SoundEventReference.SoundObjectReference, ref interactionInfo.SoundEventReference);
				}
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060045A2 RID: 17826 RVA: 0x000F9D7D File Offset: 0x000F7F7D
		// (set) Token: 0x060045A3 RID: 17827 RVA: 0x000F9D85 File Offset: 0x000F7F85
		public ClientBlockType[] ClientBlockTypes { get; private set; }

		// Token: 0x060045A4 RID: 17828 RVA: 0x000F9D90 File Offset: 0x000F7F90
		public void PrepareBlockTypes(Dictionary<int, BlockType> networkBlockTypes, int highestReceivedBlockId, bool atlasNeedsUpdate, ref ClientBlockType[] upcomingBlockTypes, ref Dictionary<string, MapModule.AtlasLocation> upcomingBlocksImageLocations, ref Point atlasSize, out byte[][] upcomingAtlasPixelsPerLevel, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			bool flag = highestReceivedBlockId >= upcomingBlockTypes.Length;
			if (flag)
			{
				Array.Resize<ClientBlockType>(ref upcomingBlockTypes, highestReceivedBlockId + 1);
			}
			Dictionary<string, BlockyModel> dictionary = new Dictionary<string, BlockyModel>();
			Dictionary<string, BlockyAnimation> dictionary2 = new Dictionary<string, BlockyAnimation>();
			List<int> list = Enumerable.ToList<int>(networkBlockTypes.Keys);
			list.Sort();
			BlockyModel blockyModel = new BlockyModel(1);
			BlockyModelNode blockyModelNode = BlockyModelNode.CreateMapBlockNode(CharacterPartStore.BlockNameId, 16f, 1f);
			blockyModel.AddNode(ref blockyModelNode, -1);
			Dictionary<int, List<int>> dictionary3 = new Dictionary<int, List<int>>();
			for (int i = 0; i < list.Count; i++)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					upcomingBlocksImageLocations = null;
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				int num = list[i];
				BlockType blockType = networkBlockTypes[num];
				bool flag2 = blockType == null;
				if (flag2)
				{
					bool flag3 = num <= highestReceivedBlockId;
					if (flag3)
					{
						this._gameInstance.App.DevTools.Error(string.Format("Didn't receive block type for id: {0}, {1}", num, highestReceivedBlockId));
					}
				}
				else
				{
					ClientBlockType clientBlockType = new ClientBlockType();
					clientBlockType.Id = num;
					clientBlockType.Item = blockType.Item;
					clientBlockType.Name = blockType.Name;
					clientBlockType.Unknown = blockType.Unknown;
					bool flag4 = num == 0 && clientBlockType.Name != "Empty";
					if (flag4)
					{
						throw new InvalidDataException("Block type with EmptyBlockId but has name " + clientBlockType.Name);
					}
					bool flag5 = num == 1 && clientBlockType.Name != "Unknown";
					if (flag5)
					{
						throw new InvalidDataException("Block type with UnknownBlockId but has name " + clientBlockType.Name);
					}
					string name = blockType.Name;
					int num2 = name.IndexOf("|Filler=", StringComparison.Ordinal);
					bool flag6 = num2 > 0;
					bool flag7 = !clientBlockType.Unknown && flag6;
					if (flag7)
					{
						num2 += 8;
						int num3 = name.IndexOf("|", num2, StringComparison.Ordinal);
						string text = (num3 != -1) ? name.Substring(num2, num3 - num2) : name.Substring(num2);
						string[] array = text.Split(new char[]
						{
							','
						});
						bool flag8 = int.TryParse(array[0], out clientBlockType.FillerX);
						flag8 &= int.TryParse(array[1], out clientBlockType.FillerY);
						flag8 &= int.TryParse(array[2], out clientBlockType.FillerZ);
						bool flag9 = !flag8;
						if (flag9)
						{
							this._gameInstance.App.DevTools.Error("Failed to parse filler offset: " + text + " for " + name);
						}
					}
					clientBlockType.DrawType = blockType.DrawType_;
					clientBlockType.RotationYaw = blockType.RotationYaw;
					clientBlockType.RotationPitch = blockType.RotationPitch;
					clientBlockType.RotationRoll = blockType.RotationRoll;
					clientBlockType.RandomRotation = blockType.RandomRotation_;
					clientBlockType.RotationYawPlacementOffset = blockType.RotationYawPlacementOffset;
					clientBlockType.ShouldRenderCube = (clientBlockType.DrawType == 2 || clientBlockType.DrawType == 4 || clientBlockType.DrawType == 1);
					clientBlockType.RequiresAlphaBlending = blockType.RequiresAlphaBlending;
					clientBlockType.VerticalFill = (byte)blockType.VerticalFill;
					clientBlockType.MaxFillLevel = (byte)blockType.MaxFillLevel;
					clientBlockType.IsOccluder = ((clientBlockType.DrawType == 2 || clientBlockType.DrawType == 4) && !clientBlockType.RequiresAlphaBlending);
					clientBlockType.HasModel = (clientBlockType.DrawType == 3 || clientBlockType.DrawType == 4);
					clientBlockType.Opacity = blockType.Opacity_;
					bool shouldRenderCube = clientBlockType.ShouldRenderCube;
					if (shouldRenderCube)
					{
						bool flag10 = clientBlockType.RotationYaw != null && clientBlockType.RotationYaw != 1 && clientBlockType.RotationYaw != 2 && clientBlockType.RotationYaw != 3;
						if (flag10)
						{
							throw new Exception("Only 0°, 90°, 180° or 270° rotations around Y are supported for cube blocks");
						}
						bool flag11 = clientBlockType.RotationPitch != null && clientBlockType.RotationPitch != 1 && clientBlockType.RotationPitch != 2 && clientBlockType.RotationPitch != 3;
						if (flag11)
						{
							throw new Exception("Only 0°, 90°, 180° or 270° rotations around Z are supported for cube blocks");
						}
						BlockType.BlockTextures[] cubeTextures = blockType.CubeTextures;
						int num4 = (cubeTextures != null) ? cubeTextures.Length : 1;
						clientBlockType.CubeTextureWeights = new float[num4];
						string[] array2 = new string[num4];
						string[] array3 = new string[num4];
						string[] array4 = new string[num4];
						string[] array5 = new string[num4];
						string[] array6 = new string[num4];
						string[] array7 = new string[num4];
						bool flag12 = blockType.CubeTextures != null && blockType.CubeTextures.Length != 0;
						if (flag12)
						{
							for (int j = 0; j < num4; j++)
							{
								BlockType.BlockTextures blockTextures = blockType.CubeTextures[j];
								array2[j] = blockTextures.Top;
								array3[j] = blockTextures.Bottom;
								array4[j] = blockTextures.Left;
								array5[j] = blockTextures.Right;
								array6[j] = blockTextures.Front;
								array7[j] = blockTextures.Back;
								clientBlockType.CubeTextureWeights[j] = blockTextures.Weight;
							}
						}
						else
						{
							array2[0] = "BlockTextures/Unknown.png";
							array3[0] = "BlockTextures/Unknown.png";
							array4[0] = "BlockTextures/Unknown.png";
							array5[0] = "BlockTextures/Unknown.png";
							array6[0] = "BlockTextures/Unknown.png";
							array7[0] = "BlockTextures/Unknown.png";
							clientBlockType.CubeTextureWeights[0] = 1f;
						}
						ClientBlockType.CubeTexture cubeTexture = this.CreateBlockTexture(clientBlockType, array2);
						ClientBlockType.CubeTexture cubeTexture2 = this.CreateBlockTexture(clientBlockType, array3);
						ClientBlockType.CubeTexture cubeTexture3 = this.CreateBlockTexture(clientBlockType, array4);
						ClientBlockType.CubeTexture cubeTexture4 = this.CreateBlockTexture(clientBlockType, array5);
						ClientBlockType.CubeTexture cubeTexture5 = this.CreateBlockTexture(clientBlockType, array6);
						ClientBlockType.CubeTexture cubeTexture6 = this.CreateBlockTexture(clientBlockType, array7);
						bool flag13 = clientBlockType.CollisionMaterial != 2;
						if (flag13)
						{
							int num5 = MathHelper.RotationToDegrees(clientBlockType.RotationPitch);
							for (int k = 0; k < num5 / 90; k++)
							{
								ClientBlockType.CubeTexture cubeTexture7 = cubeTexture5;
								ClientBlockType.CubeTexture cubeTexture8 = cubeTexture6;
								cubeTexture5 = cubeTexture;
								cubeTexture6 = cubeTexture2;
								cubeTexture = cubeTexture8;
								cubeTexture2 = cubeTexture7;
							}
							cubeTexture3.Rotation += num5;
							cubeTexture4.Rotation += num5;
							int num6 = MathHelper.RotationToDegrees(clientBlockType.RotationYaw);
							for (int l = 0; l < num6 / 90; l++)
							{
								ClientBlockType.CubeTexture cubeTexture9 = cubeTexture5;
								ClientBlockType.CubeTexture cubeTexture10 = cubeTexture6;
								cubeTexture5 = cubeTexture3;
								cubeTexture6 = cubeTexture4;
								cubeTexture3 = cubeTexture10;
								cubeTexture4 = cubeTexture9;
							}
							cubeTexture.Rotation -= num6;
							cubeTexture2.Rotation += num6;
							int num7 = MathHelper.RotationToDegrees(clientBlockType.RotationRoll);
							for (int m = 0; m < num7 / 90; m++)
							{
								ClientBlockType.CubeTexture cubeTexture11 = cubeTexture;
								ClientBlockType.CubeTexture cubeTexture12 = cubeTexture2;
								ClientBlockType.CubeTexture cubeTexture13 = cubeTexture3;
								ClientBlockType.CubeTexture cubeTexture14 = cubeTexture4;
								cubeTexture = cubeTexture13;
								cubeTexture2 = cubeTexture14;
								cubeTexture3 = cubeTexture12;
								cubeTexture4 = cubeTexture11;
							}
							cubeTexture5.Rotation += num7;
							cubeTexture6.Rotation += num7;
						}
						clientBlockType.CubeTextures = new ClientBlockType.CubeTexture[]
						{
							cubeTexture,
							cubeTexture2,
							cubeTexture3,
							cubeTexture4,
							cubeTexture5,
							cubeTexture6
						};
						foreach (ClientBlockType.CubeTexture cubeTexture15 in clientBlockType.CubeTextures)
						{
							cubeTexture15.Rotation = 180 + MathHelper.WrapAngleDegrees(cubeTexture15.Rotation - 180);
						}
						byte b2 = (clientBlockType.MaxFillLevel == 0) ? 8 : clientBlockType.MaxFillLevel;
						bool flag14 = clientBlockType.VerticalFill == b2;
						if (flag14)
						{
							clientBlockType.CubeSideMaskTexture = blockType.CubeSideMaskTexture;
							clientBlockType.TransitionTexture = blockType.TransitionTexture;
							clientBlockType.TransitionGroupId = blockType.Group;
							clientBlockType.TransitionToGroupIds = blockType.TransitionToGroups;
						}
						BlockTypeProtocolInitializer.ConvertShadingMode(blockType.CubeShadingMode, out clientBlockType.CubeShadingMode);
					}
					bool hasModel = clientBlockType.HasModel;
					if (hasModel)
					{
						bool flag15 = blockType.Model == null || !this._gameInstance.HashesByServerAssetPath.TryGetValue(blockType.Model, ref clientBlockType.BlockyModelHash);
						if (flag15)
						{
							this._gameInstance.App.DevTools.Error("Missing model asset: " + blockType.Model + " for block " + clientBlockType.Name);
							clientBlockType.BlockyModelHash = null;
						}
						BlockType.ModelTexture[] modelTexture_ = blockType.ModelTexture_;
						clientBlockType.BlockyTextures = new ClientBlockType.BlockyTexture[Math.Max(1, modelTexture_.Length)];
						clientBlockType.BlockyTextureWeights = new float[Math.Max(1, modelTexture_.Length)];
						int num8 = 0;
						foreach (BlockType.ModelTexture modelTexture in modelTexture_)
						{
							clientBlockType.BlockyTextures[num8] = new ClientBlockType.BlockyTexture
							{
								Name = modelTexture.Texture
							};
							clientBlockType.BlockyTextureWeights[num8] = modelTexture.Weight;
							bool flag16 = clientBlockType.BlockyTextures[num8].Name == null || !this._gameInstance.HashesByServerAssetPath.TryGetValue(clientBlockType.BlockyTextures[num8].Name, ref clientBlockType.BlockyTextures[num8].Hash);
							if (flag16)
							{
								this._gameInstance.App.DevTools.Error("Missing texture asset for custom model: " + clientBlockType.BlockyTextures[num8].Name);
								clientBlockType.BlockyTextures[num8].Hash = null;
							}
							num8++;
						}
						clientBlockType.BlockyModelScale = blockType.ModelScale;
					}
					Matrix.CreateFromYawPitchRoll(MathHelper.RotationToRadians(clientBlockType.RotationYaw), MathHelper.RotationToRadians(clientBlockType.RotationPitch), MathHelper.RotationToRadians(clientBlockType.RotationRoll), out clientBlockType.RotationMatrix);
					Matrix.CreateScale(clientBlockType.BlockyModelScale * 0.03125f, out clientBlockType.BlockyModelTranslatedScaleMatrix);
					Matrix.AddTranslation(ref clientBlockType.BlockyModelTranslatedScaleMatrix, 0.5f, 0f, 0.5f);
					Matrix.Multiply(ref clientBlockType.BlockyModelTranslatedScaleMatrix, ref ChunkGeometryBuilder.NegativeHalfBlockOffsetMatrix, out clientBlockType.WorldMatrix);
					Matrix.Multiply(ref clientBlockType.WorldMatrix, ref clientBlockType.RotationMatrix, out clientBlockType.WorldMatrix);
					Matrix.Multiply(ref clientBlockType.WorldMatrix, ref ChunkGeometryBuilder.PositiveHalfBlockOffsetMatrix, out clientBlockType.WorldMatrix);
					Matrix.Invert(ref clientBlockType.WorldMatrix, out clientBlockType.CubeBlockInvertMatrix);
					Matrix.AddTranslation(ref clientBlockType.CubeBlockInvertMatrix, 0f, -16f, 0f);
					bool flag17 = blockType.ModelAnimation != null;
					if (flag17)
					{
						string text2;
						bool flag18 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(blockType.ModelAnimation, ref text2);
						if (flag18)
						{
							this._gameInstance.App.DevTools.Error("Missing animation asset: " + blockType.ModelAnimation + " for block " + clientBlockType.Name);
						}
						else
						{
							bool flag19 = !dictionary2.TryGetValue(text2, out clientBlockType.BlockyAnimation);
							if (flag19)
							{
								BlockyAnimation value = new BlockyAnimation();
								BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(text2, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref value);
								clientBlockType.BlockyAnimation = (dictionary2[text2] = value);
							}
						}
					}
					clientBlockType.SelfTintColorsBySide = new int[]
					{
						blockType.Tint_.Top,
						blockType.Tint_.Bottom,
						blockType.Tint_.Left,
						blockType.Tint_.Right,
						blockType.Tint_.Front,
						blockType.Tint_.Back
					};
					clientBlockType.BiomeTintMultipliersBySide = new float[]
					{
						(float)blockType.BiomeTint.Top / 100f,
						(float)blockType.BiomeTint.Bottom / 100f,
						(float)blockType.BiomeTint.Left / 100f,
						(float)blockType.BiomeTint.Right / 100f,
						(float)blockType.BiomeTint.Front / 100f,
						(float)blockType.BiomeTint.Back / 100f
					};
					bool flag20 = blockType.Light != null;
					if (flag20)
					{
						ClientItemBaseProtocolInitializer.ParseLightColor(blockType.Light, ref clientBlockType.LightEmitted);
					}
					clientBlockType.CollisionMaterial = blockType.Material_;
					clientBlockType.HitboxType = blockType.Hitbox;
					clientBlockType.MovementSettings = blockType.MovementSettings;
					clientBlockType.IsUsable = blockType.Flags.IsUsable;
					clientBlockType.InteractionHint = blockType.InteractionHint;
					clientBlockType.Gathering = blockType.Gathering;
					bool flag21 = Enumerable.Contains<ShaderType>(blockType.ShaderEffect, 4);
					if (flag21)
					{
						clientBlockType.CubeShaderEffect = ClientBlockType.ClientShaderEffect.Ice;
					}
					else
					{
						bool flag22 = Enumerable.Contains<ShaderType>(blockType.ShaderEffect, 5);
						if (flag22)
						{
							clientBlockType.CubeShaderEffect = ClientBlockType.ClientShaderEffect.Water;
						}
						else
						{
							bool flag23 = Enumerable.Contains<ShaderType>(blockType.ShaderEffect, 6);
							if (flag23)
							{
								clientBlockType.CubeShaderEffect = ClientBlockType.ClientShaderEffect.Lava;
							}
							else
							{
								bool flag24 = Enumerable.Contains<ShaderType>(blockType.ShaderEffect, 7);
								if (flag24)
								{
									clientBlockType.CubeShaderEffect = ClientBlockType.ClientShaderEffect.Slime;
								}
								else
								{
									clientBlockType.CubeShaderEffect = ClientBlockType.ClientShaderEffect.None;
								}
							}
						}
					}
					bool flag25 = Enumerable.Contains<ShaderType>(blockType.ShaderEffect, 2);
					if (flag25)
					{
						clientBlockType.BlockyModelShaderEffect = ClientBlockType.ClientShaderEffect.WindAttached;
					}
					else
					{
						bool flag26 = Enumerable.Contains<ShaderType>(blockType.ShaderEffect, 1);
						if (flag26)
						{
							clientBlockType.BlockyModelShaderEffect = ClientBlockType.ClientShaderEffect.Wind;
						}
						else
						{
							bool flag27 = Enumerable.Contains<ShaderType>(blockType.ShaderEffect, 8);
							if (flag27)
							{
								clientBlockType.BlockyModelShaderEffect = ClientBlockType.ClientShaderEffect.Ripple;
							}
							else
							{
								clientBlockType.BlockyModelShaderEffect = ClientBlockType.ClientShaderEffect.None;
							}
						}
					}
					clientBlockType.FluidBlockId = (int)((ushort)blockType.Fluid);
					clientBlockType.FluidFXIndex = blockType.FluidFXIndex;
					bool flag28 = blockType.VariantOriginalId != num;
					if (flag28)
					{
						ClientBlockType clientBlockType2 = upcomingBlockTypes[blockType.VariantOriginalId];
						bool flag29 = clientBlockType2 == null;
						if (flag29)
						{
							bool flag30 = !networkBlockTypes.ContainsKey(blockType.VariantOriginalId);
							if (flag30)
							{
								throw new Exception("Missing original block type");
							}
							List<int> list2;
							bool flag31 = !dictionary3.TryGetValue(blockType.VariantOriginalId, out list2);
							if (flag31)
							{
								list2 = (dictionary3[blockType.VariantOriginalId] = new List<int>());
							}
							list2.Add(num);
						}
						else
						{
							int capacity = clientBlockType.Name.Length - clientBlockType2.Name.Length;
							StringBuilder stringBuilder = new StringBuilder(capacity);
							string[] array9 = clientBlockType2.Name.Split(new char[]
							{
								'|'
							});
							foreach (string text3 in clientBlockType.Name.Split(new char[]
							{
								'|'
							}))
							{
								bool flag32 = Enumerable.Contains<string>(array9, text3);
								if (!flag32)
								{
									bool flag33 = stringBuilder.Length > 0;
									if (flag33)
									{
										stringBuilder.Append('|');
									}
									stringBuilder.Append(text3);
								}
							}
							clientBlockType2.Variants[stringBuilder.ToString()] = num;
						}
					}
					List<int> list3;
					bool flag34 = dictionary3.TryGetValue(num, out list3);
					if (flag34)
					{
						foreach (int num11 in list3)
						{
							ClientBlockType clientBlockType3 = upcomingBlockTypes[num11];
							int capacity2 = clientBlockType3.Name.Length - clientBlockType.Name.Length;
							StringBuilder stringBuilder2 = new StringBuilder(capacity2);
							string[] array11 = clientBlockType.Name.Split(new char[]
							{
								'|'
							});
							foreach (string text4 in clientBlockType3.Name.Split(new char[]
							{
								'|'
							}))
							{
								bool flag35 = Enumerable.Contains<string>(array11, text4);
								if (!flag35)
								{
									bool flag36 = stringBuilder2.Length > 0;
									if (flag36)
									{
										stringBuilder2.Append('|');
									}
									stringBuilder2.Append(text4);
								}
							}
							clientBlockType.Variants[stringBuilder2.ToString()] = num;
						}
					}
					clientBlockType.BlockSoundSetIndex = blockType.BlockSoundSetIndex;
					clientBlockType.BlockParticleSetId = blockType.BlockParticleSetId;
					bool flag37 = blockType.ParticleColor != null;
					if (flag37)
					{
						clientBlockType.ParticleColor = UInt32Color.FromRGBA((byte)blockType.ParticleColor.Red, (byte)blockType.ParticleColor.Green, (byte)blockType.ParticleColor.Blue, byte.MaxValue);
					}
					bool flag38 = !flag6 && blockType.Particles != null;
					if (flag38)
					{
						clientBlockType.Particles = new ModelParticleSettings[blockType.Particles.Length];
						int num13 = 0;
						for (int num14 = 0; num14 < blockType.Particles.Length; num14++)
						{
							bool flag39 = blockType.Particles[num14].SystemId != null;
							if (flag39)
							{
								ModelParticleSettings modelParticleSettings = new ModelParticleSettings("");
								ParticleProtocolInitializer.Initialize(blockType.Particles[num14], ref modelParticleSettings, this._gameInstance.EntityStoreModule.NodeNameManager);
								bool flag40 = !clientBlockType.ParticleColor.IsTransparent && modelParticleSettings.Color.IsTransparent;
								if (flag40)
								{
									modelParticleSettings.Color = clientBlockType.ParticleColor;
								}
								clientBlockType.Particles[num13] = modelParticleSettings;
								num13++;
							}
						}
						bool flag41 = num13 != blockType.Particles.Length;
						if (flag41)
						{
							Array.Resize<ModelParticleSettings>(ref clientBlockType.Particles, num13);
						}
					}
					clientBlockType.Interactions = blockType.Interactions;
					bool flag42 = !flag6;
					if (flag42)
					{
						clientBlockType.SoundEventIndex = ResourceManager.GetNetworkWwiseId(blockType.SoundEventIndex);
					}
					clientBlockType.Looping = blockType.Looping;
					clientBlockType.VariantRotation = blockType.VariantRotation_;
					clientBlockType.VariantOriginalId = blockType.VariantOriginalId;
					clientBlockType.Connections = blockType.Connections;
					clientBlockType.States = blockType.States;
					ClientBlockType clientBlockType4 = clientBlockType;
					Dictionary<string, int> states = blockType.States;
					Dictionary<int, string> statesReverse;
					if (states == null)
					{
						statesReverse = null;
					}
					else
					{
						statesReverse = Enumerable.ToDictionary<KeyValuePair<string, int>, int, string>(states, (KeyValuePair<string, int> p) => p.Value, (KeyValuePair<string, int> p) => p.Key);
					}
					clientBlockType4.StatesReverse = statesReverse;
					clientBlockType.TagIndexes = blockType.TagIndexes;
					bool flag43 = clientBlockType.DrawType == null || (clientBlockType.ShouldRenderCube && clientBlockType.BlockyModelHash == null);
					if (flag43)
					{
						clientBlockType.OriginalBlockyModel = new BlockyModel(0);
					}
					bool hasModel2 = clientBlockType.HasModel;
					if (hasModel2)
					{
						bool flag44 = clientBlockType.VariantOriginalId == num;
						if (flag44)
						{
							bool flag45 = clientBlockType.BlockyModelHash == null;
							if (flag45)
							{
								clientBlockType.OriginalBlockyModel = blockyModel;
							}
							else
							{
								bool flag46 = !dictionary.TryGetValue(clientBlockType.BlockyModelHash, out clientBlockType.OriginalBlockyModel);
								if (flag46)
								{
									try
									{
										BlockyModel value2 = new BlockyModel(BlockyModel.MaxNodeCount);
										BlockyModelInitializer.Parse(AssetManager.GetAssetUsingHash(clientBlockType.BlockyModelHash, false), this._gameInstance.EntityStoreModule.NodeNameManager, ref value2);
										clientBlockType.OriginalBlockyModel = (dictionary[clientBlockType.BlockyModelHash] = value2);
									}
									catch (Exception exception)
									{
										MapModule.Logger.Error(exception, "Failed to parse model " + blockType.Model + " for block " + clientBlockType.Name);
										clientBlockType.OriginalBlockyModel = blockyModel;
									}
								}
							}
							clientBlockType.RenderedBlockyModel = new RenderedStaticBlockyModel(clientBlockType.OriginalBlockyModel);
						}
						else
						{
							ClientBlockType clientBlockType5 = upcomingBlockTypes[blockType.VariantOriginalId];
							clientBlockType.OriginalBlockyModel = clientBlockType5.OriginalBlockyModel;
							clientBlockType.RenderedBlockyModel = clientBlockType5.RenderedBlockyModel;
						}
					}
					bool flag47 = clientBlockType.Particles != null;
					if (flag47)
					{
						for (int num15 = 0; num15 < clientBlockType.Particles.Length; num15++)
						{
							bool flag48 = clientBlockType.RenderedBlockyModel != null;
							if (flag48)
							{
								int targetNodeIndex = 0;
								bool flag49 = clientBlockType.Particles[num15].TargetNodeNameId != -1;
								if (flag49)
								{
									clientBlockType.OriginalBlockyModel.NodeIndicesByNameId.TryGetValue(clientBlockType.Particles[num15].TargetNodeNameId, out targetNodeIndex);
								}
								else
								{
									clientBlockType.Particles[num15].TargetNodeNameId = clientBlockType.OriginalBlockyModel.AllNodes[0].NameId;
								}
								clientBlockType.Particles[num15].TargetNodeIndex = targetNodeIndex;
							}
						}
					}
					clientBlockType.VertexData = new ChunkGeometryData();
					bool flag50 = clientBlockType.RenderedBlockyModel != null;
					if (flag50)
					{
						clientBlockType.VertexData.VerticesCount += clientBlockType.RenderedBlockyModel.AnimatedVertices.Length;
						clientBlockType.VertexData.IndicesCount += clientBlockType.RenderedBlockyModel.AnimatedIndices.Length;
					}
					bool shouldRenderCube2 = clientBlockType.ShouldRenderCube;
					if (shouldRenderCube2)
					{
						clientBlockType.VertexData.VerticesCount += 24;
						clientBlockType.VertexData.IndicesCount += 36;
					}
					clientBlockType.VertexData.Vertices = new ChunkVertex[clientBlockType.VertexData.VerticesCount];
					clientBlockType.VertexData.Indices = new uint[clientBlockType.VertexData.IndicesCount];
					bool flag51 = clientBlockType.RenderedBlockyModel != null;
					if (flag51)
					{
						for (int num16 = 0; num16 < clientBlockType.RenderedBlockyModel.AnimatedIndices.Length; num16++)
						{
							clientBlockType.VertexData.Indices[clientBlockType.VertexData.IndicesOffset + num16] = clientBlockType.VertexData.VerticesOffset + (uint)clientBlockType.RenderedBlockyModel.AnimatedIndices[num16];
						}
						clientBlockType.VertexData.IndicesOffset += clientBlockType.RenderedBlockyModel.AnimatedIndices.Length;
					}
					upcomingBlockTypes[num] = clientBlockType;
				}
			}
			UShortVector2[] texCoordsByCorner = new UShortVector2[4];
			UShortVector2[] sideMaskTexCoordsByCorner = new UShortVector2[4];
			int[] cornerOcclusions = new int[4];
			ClientBlockType.ClientShaderEffect[] cornerShaderEffects = new ClientBlockType.ClientShaderEffect[4];
			uint[] array13 = new uint[1156];
			uint[] array14 = new uint[8];
			for (int num17 = 0; num17 < array14.Length; num17++)
			{
				array14[num17] = 301989887U;
			}
			int num18 = ChunkHelper.IndexOfBlockInBorderedChunk(1, 1, 1);
			int[] array15 = new int[39304];
			ushort[] array16 = new ushort[39304];
			for (int num19 = 0; num19 < 39304; num19++)
			{
				array16[num19] = 61440;
			}
			string text5;
			bool flag52 = !this._gameInstance.HashesByServerAssetPath.TryGetValue("BlockTextures/Unknown.png", ref text5);
			if (flag52)
			{
				text5 = "";
				this._gameInstance.App.DevTools.Error("Missing block texture: BlockTextures/Unknown.png");
			}
			MapModule.BlockyModelTextureInfo blockyModelTextureInfo = new MapModule.BlockyModelTextureInfo
			{
				AtlasLocation = new MapModule.AtlasLocation
				{
					Position = Point.Zero,
					Size = new Point(32, 32)
				},
				Hash = text5,
				ServerPath = "BlockTextures/Unknown.png"
			};
			if (atlasNeedsUpdate)
			{
				Dictionary<string, int> dictionary4 = new Dictionary<string, int>
				{
					{
						text5,
						0
					}
				};
				Dictionary<string, MapModule.BlockyModelTextureInfo> dictionary5 = new Dictionary<string, MapModule.BlockyModelTextureInfo>();
				upcomingBlocksImageLocations.Clear();
				for (int num20 = 0; num20 < upcomingBlockTypes.Length; num20++)
				{
					ClientBlockType clientBlockType6 = upcomingBlockTypes[num20];
					bool shouldRenderCube3 = clientBlockType6.ShouldRenderCube;
					if (shouldRenderCube3)
					{
						for (int num21 = 0; num21 < clientBlockType6.CubeTextures.Length; num21++)
						{
							ClientBlockType.CubeTexture cubeTexture16 = clientBlockType6.CubeTextures[num21];
							for (int num22 = 0; num22 < cubeTexture16.Names.Length; num22++)
							{
								string text6 = cubeTexture16.Names[num22];
								int textureIndex = this.GetTextureIndex(dictionary4, text6);
								upcomingBlocksImageLocations[text6] = new MapModule.AtlasLocation
								{
									TileIndex = textureIndex
								};
								bool flag53 = textureIndex == 0 && text6 != "BlockTextures/Unknown.png";
								if (flag53)
								{
									this._gameInstance.App.DevTools.Error("Missing texture asset: " + text6 + " for block " + clientBlockType6.Name);
								}
							}
						}
						bool flag54 = !string.IsNullOrEmpty(clientBlockType6.CubeSideMaskTexture);
						if (flag54)
						{
							int textureIndex2 = this.GetTextureIndex(dictionary4, clientBlockType6.CubeSideMaskTexture);
							upcomingBlocksImageLocations[clientBlockType6.CubeSideMaskTexture] = new MapModule.AtlasLocation
							{
								TileIndex = textureIndex2
							};
							bool flag55 = textureIndex2 == 0;
							if (flag55)
							{
								this._gameInstance.App.DevTools.Error("Missing texture asset: " + clientBlockType6.CubeSideMaskTexture + " for block " + clientBlockType6.Name);
							}
						}
						bool flag56 = !string.IsNullOrEmpty(clientBlockType6.TransitionTexture);
						if (flag56)
						{
							int textureIndex3 = this.GetTextureIndex(dictionary4, clientBlockType6.TransitionTexture);
							upcomingBlocksImageLocations[clientBlockType6.TransitionTexture] = new MapModule.AtlasLocation
							{
								TileIndex = textureIndex3
							};
							bool flag57 = textureIndex3 == 0;
							if (flag57)
							{
								this._gameInstance.App.DevTools.Error("Missing texture asset: " + clientBlockType6.TransitionTexture + " for block " + clientBlockType6.Name);
							}
						}
					}
					bool hasModel3 = clientBlockType6.HasModel;
					if (hasModel3)
					{
						for (int num23 = 0; num23 < clientBlockType6.BlockyTextures.Length; num23++)
						{
							string hash = clientBlockType6.BlockyTextures[num23].Hash;
							MapModule.BlockyModelTextureInfo blockyModelTextureInfo2;
							bool flag58 = !string.IsNullOrEmpty(hash) && !dictionary5.TryGetValue(hash, out blockyModelTextureInfo2);
							if (flag58)
							{
								blockyModelTextureInfo2 = new MapModule.BlockyModelTextureInfo
								{
									ServerPath = clientBlockType6.BlockyTextures[num23].Name,
									Hash = hash
								};
								string assetLocalPathUsingHash = AssetManager.GetAssetLocalPathUsingHash(hash);
								bool flag59 = Image.TryGetPngDimensions(assetLocalPathUsingHash, out blockyModelTextureInfo2.AtlasLocation.Size.X, out blockyModelTextureInfo2.AtlasLocation.Size.Y);
								if (flag59)
								{
									dictionary5[hash] = blockyModelTextureInfo2;
								}
								else
								{
									this._gameInstance.App.DevTools.Error(string.Concat(new string[]
									{
										"Failed to get PNG dimensions for: ",
										assetLocalPathUsingHash,
										" (",
										hash,
										")"
									}));
								}
							}
						}
					}
				}
				int num24 = (int)Math.Ceiling((double)((float)dictionary4.Count / (float)(this.TextureAtlas.Width / 32))) * 32;
				atlasSize.Y = 32;
				while (atlasSize.Y < num24)
				{
					atlasSize.Y <<= 1;
				}
				Point zero = new Point(0, num24);
				List<MapModule.BlockyModelTextureInfo> list4 = new List<MapModule.BlockyModelTextureInfo>(dictionary5.Values);
				list4.Sort((MapModule.BlockyModelTextureInfo a, MapModule.BlockyModelTextureInfo b) => b.AtlasLocation.Size.Y.CompareTo(a.AtlasLocation.Size.Y));
				foreach (MapModule.BlockyModelTextureInfo blockyModelTextureInfo3 in list4)
				{
					bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
					if (isCancellationRequested2)
					{
						upcomingAtlasPixelsPerLevel = null;
						return;
					}
					int x = blockyModelTextureInfo3.AtlasLocation.Size.X;
					int y = blockyModelTextureInfo3.AtlasLocation.Size.Y;
					bool flag60 = x % 32 != 0 || y % 32 != 0 || x < 32 || y < 32;
					if (flag60)
					{
						this._gameInstance.App.DevTools.Warn(string.Format("Texture width/height must be a multiple of 32 and at least 32x32: {0} ({1}x{2})", blockyModelTextureInfo3.ServerPath, x, y));
					}
					bool flag61 = zero.X + x > atlasSize.X;
					if (flag61)
					{
						zero.X = 0;
						zero.Y = num24;
					}
					while (zero.Y + y > atlasSize.Y)
					{
						atlasSize.Y <<= 1;
					}
					blockyModelTextureInfo3.AtlasLocation.Position = zero;
					num24 = Math.Max(num24, zero.Y + y);
					zero.X += x;
				}
				byte[] array17 = new byte[atlasSize.X * atlasSize.Y * 4];
				zero = Point.Zero;
				foreach (string text7 in Enumerable.ToArray<string>(dictionary4.Keys))
				{
					bool isCancellationRequested3 = cancellationToken.IsCancellationRequested;
					if (isCancellationRequested3)
					{
						upcomingAtlasPixelsPerLevel = null;
						return;
					}
					try
					{
						Image image = new Image(AssetManager.GetAssetUsingHash(text7, false));
						bool flag62 = image.Width != 32 || image.Height != 32;
						if (flag62)
						{
							this._gameInstance.App.DevTools.Warn(string.Format("Invalid block texture size, must be {0}: {1} ({2}x{3})", new object[]
							{
								32,
								AssetManager.GetAssetLocalPathUsingHash(text7),
								image.Width,
								image.Height
							}));
						}
						for (int num26 = 0; num26 < image.Height; num26++)
						{
							int dstOffset = ((zero.Y + num26) * atlasSize.X + zero.X) * 4;
							Buffer.BlockCopy(image.Pixels, num26 * image.Width * 4, array17, dstOffset, image.Width * 4);
						}
						upcomingBlocksImageLocations[text7] = new MapModule.AtlasLocation
						{
							Position = zero,
							Size = new Point(image.Width, image.Height)
						};
					}
					catch (Exception exception2)
					{
						MapModule.Logger.Error(exception2, "Block texture not found: " + AssetManager.GetAssetLocalPathUsingHash(text7));
					}
					zero.X += 32;
					bool flag63 = zero.X >= atlasSize.X;
					if (flag63)
					{
						zero.X = 0;
						zero.Y += 32;
					}
				}
				foreach (MapModule.BlockyModelTextureInfo blockyModelTextureInfo4 in list4)
				{
					bool isCancellationRequested4 = cancellationToken.IsCancellationRequested;
					if (isCancellationRequested4)
					{
						upcomingAtlasPixelsPerLevel = null;
						return;
					}
					MapModule.AtlasLocation atlasLocation;
					bool flag64 = !upcomingBlocksImageLocations.TryGetValue(blockyModelTextureInfo4.Hash, out atlasLocation);
					if (flag64)
					{
						upcomingBlocksImageLocations[blockyModelTextureInfo4.Hash] = blockyModelTextureInfo4.AtlasLocation;
						Image image2 = null;
						try
						{
							image2 = new Image(AssetManager.GetAssetUsingHash(blockyModelTextureInfo4.Hash, false));
							for (int num27 = 0; num27 < image2.Height; num27++)
							{
								int dstOffset2 = ((blockyModelTextureInfo4.AtlasLocation.Position.Y + num27) * atlasSize.X + blockyModelTextureInfo4.AtlasLocation.Position.X) * 4;
								Buffer.BlockCopy(image2.Pixels, num27 * image2.Width * 4, array17, dstOffset2, image2.Width * 4);
							}
						}
						catch (Exception ex)
						{
							bool flag65 = image2 != null;
							if (flag65)
							{
								throw new Exception(string.Concat(new string[]
								{
									"Invalid texture! ",
									string.Format("TextureInfo: {0}, {1}, ({2}, {3}), ", new object[]
									{
										blockyModelTextureInfo4.ServerPath,
										blockyModelTextureInfo4.Hash,
										blockyModelTextureInfo4.AtlasLocation.Size.X,
										blockyModelTextureInfo4.AtlasLocation.Size.Y
									}),
									string.Format("Image: ({0}, {1}), {2}, ", image2.Width, image2.Height, image2.Pixels.Length / 4),
									string.Format("AtlasPosition: ({0} {1}), ", blockyModelTextureInfo4.AtlasLocation.Position.X, blockyModelTextureInfo4.AtlasLocation.Position.Y),
									string.Format("TextureAtlas: ({0} {1})", atlasSize.X, atlasSize.Y)
								}), ex);
							}
							MapModule.Logger.Error(ex, "Failed to load block texture: " + AssetManager.GetAssetLocalPathUsingHash(blockyModelTextureInfo4.Hash));
						}
					}
					else
					{
						blockyModelTextureInfo4.AtlasLocation = atlasLocation;
					}
				}
				upcomingAtlasPixelsPerLevel = Texture.BuildMipmapPixels(array17, this.TextureAtlas.Width, this.TextureAtlas.MipmapLevelCount);
				for (int num28 = 0; num28 < upcomingBlockTypes.Length; num28++)
				{
					ClientBlockType clientBlockType7 = upcomingBlockTypes[num28];
					bool flag66 = clientBlockType7 == null || cancellationToken.IsCancellationRequested;
					if (flag66)
					{
						break;
					}
					this.FinishBlockTypeModelPreparation(clientBlockType7, upcomingBlocksImageLocations, blockyModelTextureInfo.AtlasLocation, atlasSize);
					int alphaTestedAnimatedLowLODIndicesStart = 0;
					int? num29 = new int?(0);
					int num30 = 32;
					uint num31 = (uint)clientBlockType7.SelfTintColorsBySide[0];
					bool flag67 = !ChunkGeometryBuilder.NoTint.Equals(ChunkGeometryBuilder.ForceTint);
					if (flag67)
					{
					}
					array13[0] = num31;
					array13[1] = num31;
					array13[34] = num31;
					array13[35] = num31;
					array15[num18] = clientBlockType7.Id;
					clientBlockType7.VertexData.VerticesOffset = 0U;
					clientBlockType7.VertexData.IndicesOffset = 0;
					ChunkGeometryBuilder.CreateBlockGeometry(upcomingBlockTypes, this.LightLevels, clientBlockType7, num18, (float)num30, Vector3.Zero, 0, 0, 0, ref num29, byte.MaxValue, Matrix.Identity, clientBlockType7.RotationMatrix, clientBlockType7.CubeBlockInvertMatrix, texCoordsByCorner, sideMaskTexCoordsByCorner, cornerOcclusions, cornerShaderEffects, num31, array15, array16, array13, array14, atlasSize.X, atlasSize.Y, clientBlockType7.VertexData, clientBlockType7.VertexData, alphaTestedAnimatedLowLODIndicesStart, ref alphaTestedAnimatedLowLODIndicesStart, true);
				}
				return;
			}
			upcomingAtlasPixelsPerLevel = null;
			foreach (int num32 in networkBlockTypes.Keys)
			{
				ClientBlockType clientBlockType8 = upcomingBlockTypes[num32];
				bool flag68 = clientBlockType8 == null || cancellationToken.IsCancellationRequested;
				if (flag68)
				{
					break;
				}
				this.FinishBlockTypeModelPreparation(clientBlockType8, upcomingBlocksImageLocations, blockyModelTextureInfo.AtlasLocation, atlasSize);
				int alphaTestedAnimatedLowLODIndicesStart2 = 0;
				int? num33 = new int?(0);
				int num34 = 32;
				uint num35 = (uint)clientBlockType8.SelfTintColorsBySide[0];
				array13[0] = num35;
				array13[1] = num35;
				array13[34] = num35;
				array13[35] = num35;
				array15[num18] = clientBlockType8.Id;
				clientBlockType8.VertexData.VerticesOffset = 0U;
				clientBlockType8.VertexData.IndicesOffset = 0;
				ChunkGeometryBuilder.CreateBlockGeometry(upcomingBlockTypes, this.LightLevels, clientBlockType8, num18, (float)num34, Vector3.Zero, 0, 0, 0, ref num33, byte.MaxValue, Matrix.Identity, clientBlockType8.RotationMatrix, clientBlockType8.CubeBlockInvertMatrix, texCoordsByCorner, sideMaskTexCoordsByCorner, cornerOcclusions, cornerShaderEffects, num35, array15, array16, array13, array14, atlasSize.X, atlasSize.Y, clientBlockType8.VertexData, clientBlockType8.VertexData, alphaTestedAnimatedLowLODIndicesStart2, ref alphaTestedAnimatedLowLODIndicesStart2, true);
			}
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x000FC1D0 File Offset: 0x000FA3D0
		private void FinishBlockTypeModelPreparation(ClientBlockType blockType, Dictionary<string, MapModule.AtlasLocation> upcomingBlocksImageLocations, MapModule.AtlasLocation unknownBlockAtlasLocation, Point atlasSize)
		{
			blockType.FinalBlockyModel = blockType.OriginalBlockyModel.Clone();
			bool hasModel = blockType.HasModel;
			if (hasModel)
			{
				MapModule.AtlasLocation atlasLocation;
				bool flag = blockType.BlockyTextures[0].Hash == null || !upcomingBlocksImageLocations.TryGetValue(blockType.BlockyTextures[0].Hash, out atlasLocation);
				if (flag)
				{
					atlasLocation = unknownBlockAtlasLocation;
				}
				blockType.FinalBlockyModel.OffsetUVs(atlasLocation.Position);
				blockType.RenderedBlockyModel.PrepareUVs(blockType.OriginalBlockyModel, atlasLocation.Size, atlasSize);
				blockType.RenderedBlockyModelTextureOrigins = new Vector2[blockType.BlockyTextures.Length];
				for (int i = 0; i < blockType.BlockyTextures.Length; i++)
				{
					MapModule.AtlasLocation atlasLocation2;
					bool flag2 = blockType.BlockyTextures[i].Hash == null || !upcomingBlocksImageLocations.TryGetValue(blockType.BlockyTextures[i].Hash, out atlasLocation2);
					if (flag2)
					{
						atlasLocation2 = unknownBlockAtlasLocation;
					}
					blockType.RenderedBlockyModelTextureOrigins[i] = new Vector2((float)atlasLocation2.Position.X / (float)atlasSize.X, (float)atlasLocation2.Position.Y / (float)atlasSize.Y);
				}
			}
			bool shouldRenderCube = blockType.ShouldRenderCube;
			if (shouldRenderCube)
			{
				for (int j = 0; j < blockType.CubeTextures.Length; j++)
				{
					ClientBlockType.CubeTexture cubeTexture = blockType.CubeTextures[j];
					for (int k = 0; k < cubeTexture.Names.Length; k++)
					{
						cubeTexture.TileLinearPositionsInAtlas[k] = upcomingBlocksImageLocations[cubeTexture.Names[k]].TileIndex;
					}
				}
				bool flag3 = !string.IsNullOrEmpty(blockType.CubeSideMaskTexture);
				if (flag3)
				{
					blockType.CubeSideMaskTextureAtlasIndex = upcomingBlocksImageLocations[blockType.CubeSideMaskTexture].TileIndex;
				}
				else
				{
					blockType.CubeSideMaskTextureAtlasIndex = -1;
				}
				bool flag4 = !string.IsNullOrEmpty(blockType.TransitionTexture);
				if (flag4)
				{
					blockType.TransitionTextureAtlasIndex = upcomingBlocksImageLocations[blockType.TransitionTexture].TileIndex;
				}
				else
				{
					blockType.TransitionTextureAtlasIndex = -1;
				}
				blockType.FinalBlockyModel.AddMapBlockNode(blockType, CharacterPartStore.BlockNameId, CharacterPartStore.SideMaskNameId, atlasSize.X);
			}
			blockType.FinalBlockyModel.SetAtlasIndex(0);
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x000FC414 File Offset: 0x000FA614
		public void SetupBlockTypes(ClientBlockType[] blockTypes, bool rebuildAllChunks = true)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.DoWithMapGeometryBuilderPaused(rebuildAllChunks, delegate
			{
				this.ClientBlockTypes = blockTypes;
			});
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x000FC458 File Offset: 0x000FA658
		private ClientBlockType.CubeTexture CreateBlockTexture(ClientBlockType blockType, string[] networkBlockTextures)
		{
			return new ClientBlockType.CubeTexture
			{
				Names = networkBlockTextures,
				TileLinearPositionsInAtlas = new int[Math.Max(1, networkBlockTextures.Length)]
			};
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x000FC48C File Offset: 0x000FA68C
		private int GetTextureIndex(Dictionary<string, int> textures, string texturePath)
		{
			string key;
			bool flag = texturePath == null || !this._gameInstance.HashesByServerAssetPath.TryGetValue(texturePath, ref key);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int count;
				bool flag2 = !textures.TryGetValue(key, out count);
				if (flag2)
				{
					count = textures.Count;
					textures.Add(key, count);
				}
				result = count;
			}
			return result;
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x000FC4E8 File Offset: 0x000FA6E8
		public int GetClientBlockIdFromName(string blockName)
		{
			blockName = blockName.ToLower();
			return Array.IndexOf<ClientBlockType>(this.ClientBlockTypes, Array.Find<ClientBlockType>(this.ClientBlockTypes, (ClientBlockType p) => p.Name.ToLower() == blockName));
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x000FC53C File Offset: 0x000FA73C
		public ClientBlockType GetClientBlockTypeFromName(string blockName)
		{
			blockName = blockName.ToLower();
			return Array.Find<ClientBlockType>(this.ClientBlockTypes, (ClientBlockType p) => p.Name.ToLower() == blockName);
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x000FC584 File Offset: 0x000FA784
		public Vector3 GetBlockEnvironmentTint(int blockX, int blockY, int blockZ, ClientBlockType blockType)
		{
			Vector3 vector = new Vector3(255f);
			int num = blockType.SelfTintColorsBySide[0];
			byte b = (byte)(num >> 16);
			byte b2 = (byte)(num >> 8);
			byte b3 = (byte)num;
			int num2 = blockX >> 5;
			int num3 = blockZ >> 5;
			int num4 = blockX - num2 * 32;
			int num5 = blockZ - num3 * 32;
			ChunkColumn chunkColumn = this.GetChunkColumn(num2, num3);
			bool flag = chunkColumn == null;
			Vector3 result;
			if (flag)
			{
				result = vector;
			}
			else
			{
				uint num6 = chunkColumn.Tints[(num5 << 5) + num4];
				byte b4 = (byte)(num6 >> 16);
				byte b5 = (byte)(num6 >> 8);
				byte b6 = (byte)num6;
				float num7 = blockType.BiomeTintMultipliersBySide[0];
				vector.X = (uint)((float)b + (float)(b4 - b) * num7);
				vector.Y = (uint)((float)b2 + (float)(b5 - b2) * num7);
				vector.Z = (uint)((float)b3 + (float)(b6 - b3) * num7);
				result = vector;
			}
			return result;
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x000FC668 File Offset: 0x000FA868
		public Vector3 GetBlockFluidTint(int blockX, int blockY, int blockZ, ClientBlockType blockType)
		{
			Vector3 vector = new Vector3(255f);
			bool flag = blockType.FluidFXIndex == 0;
			Vector3 result;
			if (flag)
			{
				result = vector;
			}
			else
			{
				FluidFX fluidFX = this._gameInstance.ServerSettings.FluidFXs[blockType.FluidFXIndex];
				switch (fluidFX.FogMode)
				{
				case 0:
					vector.X = (float)((byte)fluidFX.FogColor.Red);
					vector.Y = (float)((byte)fluidFX.FogColor.Green);
					vector.Z = (float)((byte)fluidFX.FogColor.Blue);
					break;
				case 1:
				{
					Vector4 lightColorAtBlockPosition = this.GetLightColorAtBlockPosition(blockX, blockY, blockZ);
					vector.X = (float)((byte)fluidFX.FogColor.Red) * lightColorAtBlockPosition.X;
					vector.Y = (float)((byte)fluidFX.FogColor.Green) * lightColorAtBlockPosition.Y;
					vector.Z = (float)((byte)fluidFX.FogColor.Blue) * lightColorAtBlockPosition.Z;
					break;
				}
				case 2:
				{
					int num = blockX >> 5;
					int num2 = blockZ >> 5;
					int num3 = blockX - num * 32;
					int num4 = blockZ - num2 * 32;
					ChunkColumn chunkColumn = this.GetChunkColumn(num, num2);
					bool flag2 = chunkColumn == null;
					if (flag2)
					{
						return vector;
					}
					ushort environmentId = ChunkHelper.GetEnvironmentId(chunkColumn.Environments, num3, num4, blockY);
					int waterTint = this._gameInstance.ServerSettings.Environments[(int)environmentId].WaterTint;
					bool flag3 = waterTint == -1;
					if (flag3)
					{
						Vector4 lightColorAtBlockPosition = this.GetLightColorAtBlockPosition(blockX, blockY, blockZ);
						vector.X = this._gameInstance.WeatherModule.WaterTintColor.X * 255f * lightColorAtBlockPosition.X;
						vector.Y = this._gameInstance.WeatherModule.WaterTintColor.Y * 255f * lightColorAtBlockPosition.Y;
						vector.Z = this._gameInstance.WeatherModule.WaterTintColor.Z * 255f * lightColorAtBlockPosition.Z;
					}
					else
					{
						float num5 = (float)((byte)(waterTint >> 16)) / 255f;
						float num6 = (float)((byte)(waterTint >> 8)) / 255f;
						float num7 = (float)((byte)waterTint) / 255f;
						uint num8 = chunkColumn.Tints[(num4 << 5) + num3];
						byte b = (byte)(num8 >> 16);
						byte b2 = (byte)(num8 >> 8);
						byte b3 = (byte)num8;
						int num9 = blockType.SelfTintColorsBySide[0];
						byte b4 = (byte)(num9 >> 16);
						byte b5 = (byte)(num9 >> 8);
						byte b6 = (byte)num9;
						float num10 = blockType.BiomeTintMultipliersBySide[0];
						uint num11 = (uint)((float)b4 + (float)(b - b4) * num10);
						uint num12 = (uint)((float)b5 + (float)(b2 - b5) * num10);
						uint num13 = (uint)((float)b6 + (float)(b3 - b6) * num10);
						vector.X = (uint)(num11 * num5);
						vector.Y = (uint)(num12 * num6);
						vector.Z = (uint)(num13 * num7);
					}
					break;
				}
				}
				result = vector;
			}
			return result;
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x000FC95C File Offset: 0x000FAB5C
		public ChunkColumn GetChunkColumn(long indexChunk)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(MapModule).FullName);
			}
			ChunkColumn chunkColumn;
			this._chunkColumns.TryGetValue(indexChunk, ref chunkColumn);
			bool flag = chunkColumn == null || chunkColumn.Tints == null || chunkColumn.Heights == null || chunkColumn.Environments == null;
			ChunkColumn result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = chunkColumn;
			}
			return result;
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x000FC9C4 File Offset: 0x000FABC4
		public ChunkColumn GetChunkColumn(int worldChunkX, int worldChunkZ)
		{
			return this.GetChunkColumn(ChunkHelper.IndexOfChunkColumn(worldChunkX, worldChunkZ));
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x000FC9E4 File Offset: 0x000FABE4
		public ChunkColumn GetOrCreateChunkColumn(int worldChunkX, int worldChunkZ)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(MapModule).FullName);
			}
			return this._chunkColumns.GetOrAdd(ChunkHelper.IndexOfChunkColumn(worldChunkX, worldChunkZ), (long key) => new ChunkColumn(worldChunkX, worldChunkZ));
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x000FCA54 File Offset: 0x000FAC54
		public Chunk GetChunk(int worldChunkX, int worldChunkY, int worldChunkZ)
		{
			ChunkColumn chunkColumn = this.GetChunkColumn(worldChunkX, worldChunkZ);
			return (chunkColumn != null) ? chunkColumn.GetChunk(worldChunkY) : null;
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x000FCA7C File Offset: 0x000FAC7C
		public HytaleClient.Protocol.BlockPosition GetBaseBlock(HytaleClient.Protocol.BlockPosition position)
		{
			int block = this.GetBlock(position.X, position.Y, position.Z, -1);
			bool flag = block == -1;
			HytaleClient.Protocol.BlockPosition result;
			if (flag)
			{
				result = position;
			}
			else
			{
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				bool flag2 = clientBlockType.FillerX != 0 || clientBlockType.FillerY != 0 || clientBlockType.FillerZ != 0;
				if (flag2)
				{
					result = new HytaleClient.Protocol.BlockPosition(position.X - clientBlockType.FillerX, position.Y - clientBlockType.FillerY, position.Z - clientBlockType.FillerZ);
				}
				else
				{
					result = position;
				}
			}
			return result;
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x000FCB1C File Offset: 0x000FAD1C
		public int GetBlock(Vector3 position, int undefinedBlockId)
		{
			return this.GetBlock((int)Math.Floor((double)position.X), (int)Math.Floor((double)position.Y), (int)Math.Floor((double)position.Z), undefinedBlockId);
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x000FCB5C File Offset: 0x000FAD5C
		public int GetBlock(int worldX, int worldY, int worldZ, int undefinedBlockId)
		{
			bool flag = worldY < 0 || worldY >= ChunkHelper.Height;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int worldChunkX = worldX >> 5;
				int worldChunkY = worldY >> 5;
				int worldChunkZ = worldZ >> 5;
				Chunk chunk = this.GetChunk(worldChunkX, worldChunkY, worldChunkZ);
				result = ((chunk != null) ? chunk.Data.GetBlock(worldX, worldY, worldZ) : undefinedBlockId);
			}
			return result;
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x000FCBB8 File Offset: 0x000FADB8
		public void SetBlock(int worldX, int worldY, int worldZ, int newBlockId, bool playInteractionStateSound)
		{
			Debug.Assert(this._gameInstance.IsOnPacketHandlerThread);
			int num = worldX >> 5;
			int num2 = worldY >> 5;
			int num3 = worldZ >> 5;
			ChunkColumn chunkColumn = this.GetChunkColumn(num, num3);
			bool flag = chunkColumn == null;
			if (!flag)
			{
				object disposeLock = chunkColumn.DisposeLock;
				bool flag2 = false;
				try
				{
					Monitor.Enter(disposeLock, ref flag2);
					bool disposed = chunkColumn.Disposed;
					if (!disposed)
					{
						Chunk chunk = chunkColumn.GetChunk(num2);
						bool flag3 = chunk == null;
						if (!flag3)
						{
							int blockId = newBlockId;
							bool flag4 = blockId >= this.ClientBlockTypes.Length;
							if (flag4)
							{
								blockId = 1;
								this._gameInstance.App.DevTools.Error(string.Format("Invalid block set in chunk ({0}, {1}, {2}) at ({3} {4} {5}) to {6} (max value {7})", new object[]
								{
									num,
									num2,
									num3,
									worldX,
									worldY,
									worldZ,
									newBlockId,
									this.ClientBlockTypes.Length - 1
								}));
							}
							chunk.Data.SetBlock(worldX, worldY, worldZ, blockId);
							bool flag5 = blockId == 0;
							if (flag5)
							{
								this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
								{
									this.RegisterDestroyedBlock(worldX, worldY, worldZ);
									int key = ChunkHelper.IndexOfWorldBlockInChunk(worldX, worldY, worldZ);
									bool flag14 = chunk.Data.CurrentInteractionStates.ContainsKey(key);
									if (flag14)
									{
										ChunkData.InteractionStateInfo interactionStateInfo = chunk.Data.CurrentInteractionStates[key];
										bool flag15 = interactionStateInfo.SoundEventReference.SoundObjectReference.SoundObjectId > 0U;
										if (flag15)
										{
											this._gameInstance.AudioModule.ActionOnEvent(ref interactionStateInfo.SoundEventReference, 0);
											this._gameInstance.AudioModule.UnregisterSoundObject(ref interactionStateInfo.SoundEventReference.SoundObjectReference);
										}
										chunk.Data.CurrentInteractionStates.Remove(key);
									}
								}, false, false);
							}
							ClientBlockType clientBlockType = this.ClientBlockTypes[blockId];
							bool flag6 = clientBlockType.BlockyAnimation != null;
							if (flag6)
							{
								this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
								{
									this.HandleBlockInteractionState(worldX, worldY, worldZ, blockId, playInteractionStateSound);
								}, false, false);
							}
							int num4 = worldX - num * 32;
							int num5 = worldZ - num3 * 32;
							int num6 = (num5 << 5) + num4;
							ushort num7 = chunkColumn.Heights[num6];
							int val = worldY;
							bool flag7 = worldY > (int)num7 && clientBlockType.Opacity != 3;
							if (flag7)
							{
								chunkColumn.Heights[num6] = (ushort)worldY;
							}
							else
							{
								bool flag8 = worldY == (int)num7 && clientBlockType.Opacity == 3;
								if (flag8)
								{
									int num8 = worldY >> 5;
									Chunk chunk4 = chunk;
									for (int i = worldY; i >= 0; i--)
									{
										int num9 = i >> 5;
										bool flag9 = num8 != num9;
										if (flag9)
										{
											num8 = num9;
											chunk4 = chunkColumn.GetChunk(num9);
										}
										int block = chunk4.Data.GetBlock(worldX, i, worldZ);
										bool flag10 = this.ClientBlockTypes[block].Opacity != 3;
										if (flag10)
										{
											val = (int)((ushort)i);
											chunkColumn.Heights[num6] = (ushort)i;
											break;
										}
									}
								}
							}
							int num10 = Math.Min((int)num7, val) >> 5;
							int num11 = Math.Max((int)num7, val) >> 5;
							for (int j = num10; j <= num11; j++)
							{
								Chunk chunk2 = chunkColumn.GetChunk(j);
								bool flag11 = chunk2 != null;
								if (flag11)
								{
									chunk2.Data.SelfLightNeedsUpdate = true;
								}
							}
							for (int k = -1; k <= 1; k++)
							{
								for (int l = -1; l <= 1; l++)
								{
									int worldChunkX = worldX + l * 16 >> 5;
									int worldChunkZ = worldZ + k * 16 >> 5;
									ChunkColumn chunkColumn2 = this.GetChunkColumn(worldChunkX, worldChunkZ);
									bool flag12 = chunkColumn2 == null;
									if (!flag12)
									{
										for (int m = num10 - 1; m <= num11 + 1; m++)
										{
											Chunk chunk3 = chunkColumn2.GetChunk(m);
											RenderedChunk renderedChunk = (chunk3 != null) ? chunk3.Rendered : null;
											bool flag13 = renderedChunk != null;
											if (flag13)
											{
												renderedChunk.GeometryNeedsUpdate = true;
											}
										}
									}
								}
							}
						}
					}
				}
				finally
				{
					if (flag2)
					{
						Monitor.Exit(disposeLock);
					}
				}
			}
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x000FD0EC File Offset: 0x000FB2EC
		private void HandleBlockInteractionState(int worldX, int worldY, int worldZ, int blockId, bool playInteractionStateSound)
		{
			int worldChunkX = worldX >> 5;
			int worldChunkY = worldY >> 5;
			int worldChunkZ = worldZ >> 5;
			Chunk chunk = this._gameInstance.MapModule.GetChunk(worldChunkX, worldChunkY, worldChunkZ);
			bool flag = chunk == null;
			if (!flag)
			{
				int blockIndex = ChunkHelper.IndexOfWorldBlockInChunk(worldX, worldY, worldZ);
				this.SetBlockInteractionState(chunk, blockId, blockIndex, new Vector3((float)worldX, (float)worldY, (float)worldZ), false, playInteractionStateSound);
			}
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x000FD14C File Offset: 0x000FB34C
		public void SetBlockHitTimer(int worldX, int worldY, int worldZ, float hitTimer)
		{
			int num = worldX >> 5;
			int num2 = worldY >> 5;
			int num3 = worldZ >> 5;
			ChunkColumn chunkColumn = this.GetChunkColumn(num, num3);
			bool flag = chunkColumn == null;
			if (!flag)
			{
				object disposeLock = chunkColumn.DisposeLock;
				lock (disposeLock)
				{
					bool disposed = chunkColumn.Disposed;
					if (disposed)
					{
						return;
					}
					Chunk chunk = chunkColumn.GetChunk(num2);
					bool flag3 = chunk == null;
					if (flag3)
					{
						return;
					}
					chunk.Data.SetBlockHitTimer(ChunkHelper.IndexOfWorldBlockInChunk(worldX, worldY, worldZ), hitTimer);
					bool flag4 = chunk.Rendered != null;
					if (flag4)
					{
						chunk.Rendered.GeometryNeedsUpdate = true;
					}
				}
				int num4 = worldX - num * 32;
				ChunkColumn chunkColumn2 = null;
				bool flag5 = num4 == 0;
				if (flag5)
				{
					chunkColumn2 = this.GetChunkColumn(num - 1, num3);
				}
				else
				{
					bool flag6 = num4 == 31;
					if (flag6)
					{
						chunkColumn2 = this.GetChunkColumn(num + 1, num3);
					}
				}
				bool flag7 = chunkColumn2 != null;
				if (flag7)
				{
					object disposeLock2 = chunkColumn2.DisposeLock;
					lock (disposeLock2)
					{
						bool flag9 = !chunkColumn2.Disposed;
						if (flag9)
						{
							Chunk chunk2 = chunkColumn2.GetChunk(num2);
							bool flag10 = ((chunk2 != null) ? chunk2.Rendered : null) != null;
							if (flag10)
							{
								chunk2.Rendered.GeometryNeedsUpdate = true;
							}
						}
					}
				}
				int num5 = worldY - num3 * 32;
				Chunk chunk3 = null;
				bool flag11 = num5 == 0;
				if (flag11)
				{
					chunk3 = chunkColumn.GetChunk(num2 - 1);
				}
				else
				{
					bool flag12 = num5 == 31;
					if (flag12)
					{
						chunk3 = chunkColumn.GetChunk(num2 + 1);
					}
				}
				bool flag13 = ((chunk3 != null) ? chunk3.Rendered : null) != null;
				if (flag13)
				{
					chunk3.Rendered.GeometryNeedsUpdate = true;
				}
				int num6 = worldZ - num3 * 32;
				ChunkColumn chunkColumn3 = null;
				bool flag14 = num6 == 0;
				if (flag14)
				{
					chunkColumn3 = this.GetChunkColumn(num, num3 - 1);
				}
				else
				{
					bool flag15 = num6 == 31;
					if (flag15)
					{
						chunkColumn3 = this.GetChunkColumn(num, num3 + 1);
					}
				}
				bool flag16 = chunkColumn3 != null;
				if (flag16)
				{
					object disposeLock3 = chunkColumn3.DisposeLock;
					lock (disposeLock3)
					{
						bool flag18 = !chunkColumn3.Disposed;
						if (flag18)
						{
							Chunk chunk4 = chunkColumn3.GetChunk(num2);
							bool flag19 = ((chunk4 != null) ? chunk4.Rendered : null) != null;
							if (flag19)
							{
								chunk4.Rendered.GeometryNeedsUpdate = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x000FD3F8 File Offset: 0x000FB5F8
		public Vector4 GetLightColorAtBlockPosition(int blockX, int blockY, int blockZ)
		{
			Vector4 vector = new Vector4(this._gameInstance.WeatherModule.SunlightColor * this._gameInstance.WeatherModule.SunLight, 1f);
			bool flag = blockY >= 0 && blockY < ChunkHelper.Height;
			if (flag)
			{
				int worldChunkX = blockX >> 5;
				int worldChunkY = blockY >> 5;
				int worldChunkZ = blockZ >> 5;
				Chunk chunk = this.GetChunk(worldChunkX, worldChunkY, worldChunkZ);
				bool flag2 = chunk != null && chunk.Data.BorderedLightAmounts != null;
				if (flag2)
				{
					int x = (blockX & 31) + 1;
					int y = (blockY & 31) + 1;
					int z = (blockZ & 31) + 1;
					int num = ChunkHelper.IndexOfBlockInBorderedChunk(x, y, z);
					ushort num2 = chunk.Data.BorderedLightAmounts[num];
					Vector3 vector2 = new Vector3(this.LightLevels[(int)(num2 & 15)], this.LightLevels[num2 >> 4 & 15], this.LightLevels[num2 >> 8 & 15]);
					int num3 = num2 >> 12 & 15;
					vector *= this.LightLevels[num3];
					vector.X = Math.Max(vector2.X * 2f, vector.X);
					vector.Y = Math.Max(vector2.Y * 2f, vector.Y);
					vector.Z = Math.Max(vector2.Z * 2f, vector.Z);
				}
			}
			return vector;
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x000FD56C File Offset: 0x000FB76C
		public void SetChunkBlocks(int worldChunkX, int worldChunkY, int worldChunkZ, byte[] data, int maxValidBlockTypeId, byte[] localLight, byte[] globalLight)
		{
			Debug.Assert(this._gameInstance.IsOnPacketHandlerThread);
			ChunkColumn orCreateChunkColumn = this.GetOrCreateChunkColumn(worldChunkX, worldChunkZ);
			Chunk chunk = orCreateChunkColumn.GetChunk(worldChunkY);
			bool flag = chunk == null;
			if (flag)
			{
				chunk = orCreateChunkColumn.CreateChunk(worldChunkY);
			}
			ushort[] array = (localLight != null && localLight.Length != 0) ? this.DecodeLocalLightArray(localLight) : null;
			bool flag2 = array != null;
			if (flag2)
			{
				chunk.Data.SelfLightNeedsUpdate = false;
				chunk.Data.SelfLightAmounts = array;
			}
			else
			{
				chunk.Data.SelfLightNeedsUpdate = true;
			}
			ushort[] array2 = (globalLight != null && globalLight.Length != 0) ? this.DecodeGlobalLightArray(globalLight) : null;
			bool flag3 = array2 != null;
			if (flag3)
			{
				chunk.Data.BorderedLightAmounts = array2;
			}
			IChunkData chunkData = EmptyPaletteChunkData.Instance;
			bool flag4 = data != null;
			if (flag4)
			{
				using (MemoryStream memoryStream = new MemoryStream(data, 0, data.Length, false, true))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						PaletteType paletteType = binaryReader.ReadByte();
						switch (paletteType)
						{
						case 1:
							chunkData = new HalfBytePaletteChunkData();
							break;
						case 2:
							chunkData = new BytePaletteChunkData();
							break;
						case 3:
							chunkData = new ShortPaletteChunkData();
							break;
						}
						chunkData.Deserialize(binaryReader, maxValidBlockTypeId, paletteType);
					}
				}
			}
			chunk.Data.Blocks.SetChunkSection(chunkData);
			this.MarkAdjacentChunksDirty(worldChunkX, worldChunkY, worldChunkZ);
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x000FD6FC File Offset: 0x000FB8FC
		private ushort[] DecodeLocalLightArray(byte[] arr)
		{
			ushort[] result;
			using (MemoryStream memoryStream = new MemoryStream(arr))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					bool flag = !binaryReader.ReadBoolean();
					if (flag)
					{
						result = null;
					}
					else
					{
						MapGeometryBuilder mapGeometryBuilder = this._mapGeometryBuilder;
						ushort[] array = ((mapGeometryBuilder != null) ? mapGeometryBuilder.DequeueSelfLightAmountArray() : null) ?? new ushort[32768];
						MapModule.DeserializeOctree(binaryReader, array, 0, 0);
						result = array;
					}
				}
			}
			return result;
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x000FD790 File Offset: 0x000FB990
		private static void DeserializeOctree(BinaryReader from, ushort[] selfLightAmount, int depth, int index)
		{
			int num = (int)from.ReadByte();
			for (int i = 0; i < 8; i++)
			{
				int num2 = 12 - depth;
				int num3 = index | i << num2;
				bool flag = (num >> i & 1) == 1;
				if (flag)
				{
					MapModule.DeserializeOctree(from, selfLightAmount, depth + 3, num3);
				}
				else
				{
					ushort num4 = from.ReadUInt16();
					int num5 = index + (i + 1 << num2);
					for (int j = num3; j < num5; j++)
					{
						selfLightAmount[j] = num4;
					}
				}
			}
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x000FD820 File Offset: 0x000FBA20
		private ushort[] DecodeGlobalLightArray(byte[] arr)
		{
			ushort[] result;
			using (MemoryStream memoryStream = new MemoryStream(arr))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					bool flag = !binaryReader.ReadBoolean();
					if (flag)
					{
						result = null;
					}
					else
					{
						MapGeometryBuilder mapGeometryBuilder = this._mapGeometryBuilder;
						ushort[] array = ((mapGeometryBuilder != null) ? mapGeometryBuilder.DequeueBorderedLightAmountArray() : null) ?? new ushort[39304];
						MapModule.DeserializeBorderedOctree(binaryReader, array, 0, 0);
						result = array;
					}
				}
			}
			return result;
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x000FD8B4 File Offset: 0x000FBAB4
		private static void DeserializeBorderedOctree(BinaryReader from, ushort[] borderedLightAmount, int depth, int index)
		{
			int num = (int)from.ReadByte();
			for (int i = 0; i < 8; i++)
			{
				int num2 = 12 - depth;
				int num3 = index | i << num2;
				bool flag = (num >> i & 1) == 1;
				if (flag)
				{
					MapModule.DeserializeBorderedOctree(from, borderedLightAmount, depth + 3, num3);
				}
				else
				{
					ushort num4 = from.ReadUInt16();
					int num5 = index + (i + 1 << num2);
					for (int j = num3; j < num5; j++)
					{
						borderedLightAmount[ChunkHelper.IndexOfBlockInBorderedChunk(j, 0, 0, 0)] = num4;
					}
				}
			}
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x000FD94C File Offset: 0x000FBB4C
		private void MarkAdjacentChunksDirty(int worldChunkX, int worldChunkY, int worldChunkZ)
		{
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					ChunkColumn chunkColumn = this.GetChunkColumn(worldChunkX + j, worldChunkZ + i);
					bool flag = chunkColumn == null;
					if (!flag)
					{
						object disposeLock = chunkColumn.DisposeLock;
						lock (disposeLock)
						{
							bool disposed = chunkColumn.Disposed;
							if (!disposed)
							{
								Chunk chunk = chunkColumn.GetChunk(worldChunkY - 1);
								RenderedChunk renderedChunk = (chunk != null) ? chunk.Rendered : null;
								bool flag3 = renderedChunk != null;
								if (flag3)
								{
									renderedChunk.GeometryNeedsUpdate = true;
								}
								Chunk chunk2 = chunkColumn.GetChunk(worldChunkY);
								RenderedChunk renderedChunk2 = (chunk2 != null) ? chunk2.Rendered : null;
								bool flag4 = renderedChunk2 != null;
								if (flag4)
								{
									renderedChunk2.GeometryNeedsUpdate = true;
								}
								Chunk chunk3 = chunkColumn.GetChunk(worldChunkY + 1);
								RenderedChunk renderedChunk3 = (chunk3 != null) ? chunk3.Rendered : null;
								bool flag5 = renderedChunk3 != null;
								if (flag5)
								{
									renderedChunk3.GeometryNeedsUpdate = true;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x000FDA74 File Offset: 0x000FBC74
		public void SetChunkColumnHeights(int worldChunkX, int worldChunkZ, ushort[] heightData)
		{
			Debug.Assert(this._gameInstance.IsOnPacketHandlerThread);
			ChunkColumn orCreateChunkColumn = this.GetOrCreateChunkColumn(worldChunkX, worldChunkZ);
			object disposeLock = orCreateChunkColumn.DisposeLock;
			lock (disposeLock)
			{
				bool disposed = orCreateChunkColumn.Disposed;
				if (disposed)
				{
					return;
				}
				orCreateChunkColumn.Heights = heightData;
				for (int i = 0; i < ChunkHelper.ChunksPerColumn; i++)
				{
					Chunk chunk = orCreateChunkColumn.GetChunk(i);
					bool flag2 = chunk != null;
					if (flag2)
					{
						chunk.Data.SelfLightNeedsUpdate = true;
					}
				}
			}
			for (int j = -1; j < 2; j++)
			{
				for (int k = -1; k < 2; k++)
				{
					ChunkColumn chunkColumn = this.GetChunkColumn(worldChunkX + k, worldChunkZ + j);
					bool flag3 = chunkColumn == null;
					if (!flag3)
					{
						object disposeLock2 = chunkColumn.DisposeLock;
						lock (disposeLock2)
						{
							bool disposed2 = chunkColumn.Disposed;
							if (!disposed2)
							{
								for (int l = 0; l < ChunkHelper.ChunksPerColumn; l++)
								{
									Chunk chunk2 = chunkColumn.GetChunk(l);
									RenderedChunk renderedChunk = (chunk2 != null) ? chunk2.Rendered : null;
									bool flag5 = renderedChunk != null;
									if (flag5)
									{
										renderedChunk.GeometryNeedsUpdate = true;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x000FDC08 File Offset: 0x000FBE08
		public void SetChunkColumnTints(int worldChunkX, int worldChunkZ, uint[] tintData)
		{
			ChunkColumn orCreateChunkColumn = this.GetOrCreateChunkColumn(worldChunkX, worldChunkZ);
			object disposeLock = orCreateChunkColumn.DisposeLock;
			lock (disposeLock)
			{
				bool disposed = orCreateChunkColumn.Disposed;
				if (disposed)
				{
					return;
				}
				orCreateChunkColumn.Tints = tintData;
				for (int i = 0; i < ChunkHelper.ChunksPerColumn; i++)
				{
					Chunk chunk = orCreateChunkColumn.GetChunk(i);
					bool flag2 = chunk != null;
					if (flag2)
					{
						chunk.Data.SelfLightNeedsUpdate = true;
					}
				}
			}
			for (int j = -1; j < 2; j++)
			{
				for (int k = -1; k < 2; k++)
				{
					ChunkColumn chunkColumn = this.GetChunkColumn(worldChunkX + k, worldChunkZ + j);
					bool flag3 = chunkColumn == null;
					if (!flag3)
					{
						object disposeLock2 = chunkColumn.DisposeLock;
						lock (disposeLock2)
						{
							bool disposed2 = chunkColumn.Disposed;
							if (!disposed2)
							{
								for (int l = 0; l < ChunkHelper.ChunksPerColumn; l++)
								{
									Chunk chunk2 = chunkColumn.GetChunk(l);
									RenderedChunk renderedChunk = (chunk2 != null) ? chunk2.Rendered : null;
									bool flag5 = renderedChunk != null;
									if (flag5)
									{
										renderedChunk.GeometryNeedsUpdate = true;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x000FDD8C File Offset: 0x000FBF8C
		public void SetChunkColumnEnvironments(int worldChunkX, int worldChunkZ, ushort[][] environmentData)
		{
			ChunkColumn orCreateChunkColumn = this.GetOrCreateChunkColumn(worldChunkX, worldChunkZ);
			object disposeLock = orCreateChunkColumn.DisposeLock;
			lock (disposeLock)
			{
				bool disposed = orCreateChunkColumn.Disposed;
				if (disposed)
				{
					return;
				}
				orCreateChunkColumn.Environments = environmentData;
				for (int i = 0; i < ChunkHelper.ChunksPerColumn; i++)
				{
					Chunk chunk = orCreateChunkColumn.GetChunk(i);
					bool flag2 = chunk != null;
					if (flag2)
					{
						chunk.Data.SelfLightNeedsUpdate = true;
					}
				}
			}
			for (int j = -1; j < 2; j++)
			{
				for (int k = -1; k < 2; k++)
				{
					ChunkColumn chunkColumn = this.GetChunkColumn(worldChunkX + k, worldChunkZ + j);
					bool flag3 = chunkColumn == null;
					if (!flag3)
					{
						object disposeLock2 = chunkColumn.DisposeLock;
						lock (disposeLock2)
						{
							bool disposed2 = chunkColumn.Disposed;
							if (!disposed2)
							{
								for (int l = 0; l < ChunkHelper.ChunksPerColumn; l++)
								{
									Chunk chunk2 = chunkColumn.GetChunk(l);
									RenderedChunk renderedChunk = (chunk2 != null) ? chunk2.Rendered : null;
									bool flag5 = renderedChunk != null;
									if (flag5)
									{
										renderedChunk.GeometryNeedsUpdate = true;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x000FDF10 File Offset: 0x000FC110
		public void UnloadChunkColumn(int worldChunkX, int worldChunkZ)
		{
			Debug.Assert(this._gameInstance.IsOnPacketHandlerThread);
			ChunkColumn chunkColumn;
			bool flag = this._chunkColumns.TryRemove(ChunkHelper.IndexOfChunkColumn(worldChunkX, worldChunkZ), ref chunkColumn);
			if (flag)
			{
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this.DisposeChunkColumn(chunkColumn);
				}, false, false);
			}
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x000FDF80 File Offset: 0x000FC180
		public void DisposeChunkColumn(ChunkColumn chunkColumn)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			object disposeLock = chunkColumn.DisposeLock;
			lock (disposeLock)
			{
				for (int i = 0; i < ChunkHelper.ChunksPerColumn; i++)
				{
					Chunk chunk = chunkColumn.GetChunk(i);
					bool flag2 = chunk != null;
					if (flag2)
					{
						object disposeLock2 = chunk.DisposeLock;
						lock (disposeLock2)
						{
							RenderedChunk rendered = chunk.Rendered;
							bool flag4 = ((rendered != null) ? rendered.UpdateTask : null) != null;
							if (flag4)
							{
								MapGeometryBuilder mapGeometryBuilder = this._mapGeometryBuilder;
								if (mapGeometryBuilder != null)
								{
									mapGeometryBuilder.EnqueueChunkUpdateTask(chunk.Rendered.UpdateTask);
								}
								chunk.Rendered.UpdateTask = null;
							}
							bool flag5 = chunk.Data.SelfLightAmounts != null;
							if (flag5)
							{
								MapGeometryBuilder mapGeometryBuilder2 = this._mapGeometryBuilder;
								if (mapGeometryBuilder2 != null)
								{
									mapGeometryBuilder2.EnqueueSelfLightAmountArray(chunk.Data.SelfLightAmounts);
								}
								chunk.Data.SelfLightAmounts = null;
							}
							bool flag6 = chunk.Data.BorderedLightAmounts != null;
							if (flag6)
							{
								MapGeometryBuilder mapGeometryBuilder3 = this._mapGeometryBuilder;
								if (mapGeometryBuilder3 != null)
								{
									mapGeometryBuilder3.EnqueueBorderedLightAmountArray(chunk.Data.BorderedLightAmounts);
								}
								chunk.Data.BorderedLightAmounts = null;
							}
							foreach (ChunkData.InteractionStateInfo interactionStateInfo in chunk.Data.CurrentInteractionStates.Values)
							{
								bool flag7 = interactionStateInfo.SoundEventReference.SoundObjectReference.SoundObjectId > 0U;
								if (flag7)
								{
									AudioDevice.SoundEventReference soundEventReference = interactionStateInfo.SoundEventReference;
									AudioDevice.SoundObjectReference soundObjectReference = interactionStateInfo.SoundEventReference.SoundObjectReference;
									this._gameInstance.AudioModule.ActionOnEvent(ref soundEventReference, 0);
									this._gameInstance.AudioModule.UnregisterSoundObject(ref soundObjectReference);
								}
							}
							chunk.Data.CurrentInteractionStates = null;
							RenderedChunk rendered2 = chunk.Rendered;
							bool flag8 = ((rendered2 != null) ? rendered2.SoundObjects : null) != null;
							if (flag8)
							{
								AudioModule audioModule = this._gameInstance.AudioModule;
								for (int j = 0; j < chunk.Rendered.SoundObjects.Length; j++)
								{
									ref RenderedChunk.MapSoundObject ptr = ref chunk.Rendered.SoundObjects[j];
									bool flag9 = ptr.SoundEventReference.SoundObjectReference.SlotId != -1;
									if (flag9)
									{
										audioModule.ActionOnEvent(ref ptr.SoundEventReference, 0);
										audioModule.UnregisterSoundObject(ref ptr.SoundEventReference.SoundObjectReference);
									}
								}
								chunk.Rendered.SoundObjects = null;
							}
							chunk.Dispose();
						}
					}
				}
				chunkColumn.Dispose();
			}
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x000FE28C File Offset: 0x000FC48C
		public int ChunkColumnCount()
		{
			return this._chunkColumns.Count;
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x000FE299 File Offset: 0x000FC499
		public List<long> GetAllChunkColumnKeys()
		{
			return Enumerable.ToList<long>(this._chunkColumns.Keys);
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x060045C5 RID: 17861 RVA: 0x000FE2AB File Offset: 0x000FC4AB
		public bool ShouldDrawAllChunksAsNear
		{
			get
			{
				return this._chunkColumnCount <= 9;
			}
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x060045C6 RID: 17862 RVA: 0x000FE2BA File Offset: 0x000FC4BA
		// (set) Token: 0x060045C7 RID: 17863 RVA: 0x000FE2C2 File Offset: 0x000FC4C2
		public bool AreNearbyChunksRendered { get; private set; }

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x060045C8 RID: 17864 RVA: 0x000FE2CB File Offset: 0x000FC4CB
		// (set) Token: 0x060045C9 RID: 17865 RVA: 0x000FE2D3 File Offset: 0x000FC4D3
		public float EffectiveViewDistance { get; private set; }

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x060045CA RID: 17866 RVA: 0x000FE2DC File Offset: 0x000FC4DC
		// (set) Token: 0x060045CB RID: 17867 RVA: 0x000FE2E4 File Offset: 0x000FC4E4
		public int StartChunkX { get; private set; } = int.MaxValue;

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x060045CC RID: 17868 RVA: 0x000FE2ED File Offset: 0x000FC4ED
		// (set) Token: 0x060045CD RID: 17869 RVA: 0x000FE2F5 File Offset: 0x000FC4F5
		public int StartChunkZ { get; private set; } = int.MaxValue;

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x060045CE RID: 17870 RVA: 0x000FE2FE File Offset: 0x000FC4FE
		// (set) Token: 0x060045CF RID: 17871 RVA: 0x000FE306 File Offset: 0x000FC506
		public int ViewRadius { get; private set; }

		// Token: 0x060045D0 RID: 17872 RVA: 0x000FE30F File Offset: 0x000FC50F
		public void SafeRegisterDestroyedBlock(int x, int y, int z)
		{
			this._destroyedBlocksInfo.SafeRegisterDestroyedBlock(x, y, z);
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x000FE320 File Offset: 0x000FC520
		public void RegisterDestroyedBlock(int x, int y, int z)
		{
			this._destroyedBlocksInfo.RegisterDestroyedBlock(x, y, z);
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x000FE331 File Offset: 0x000FC531
		public void GetBlocksRemovedThisFrame(Vector3 previousCameraPosition, Vector3 cameraPosition, BoundingFrustum cameraFrustum, float rejectNearCameraDistance, out int blocksCount, out Vector3[] blocksPositionFromCamera)
		{
			this._destroyedBlocksInfo.PrepareBlocksRemovedThisFrame((int)this._updatedChunksCount, this._updatedChunksPositions, previousCameraPosition, cameraPosition, cameraFrustum, rejectNearCameraDistance);
			blocksCount = this._destroyedBlocksInfo.Count;
			blocksPositionFromCamera = this._destroyedBlocksInfo.BlockPositionsFromCamera;
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x000FE370 File Offset: 0x000FC570
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsChunkReadyForDraw(int chunkX, int chunkY, int chunkZ)
		{
			return this._bitFieldChunksReadyForDraw.IsBitOn(chunkX, chunkY, chunkZ);
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x000FE390 File Offset: 0x000FC590
		private void SetupBitField3DForFrustum(BoundingFrustum viewFrustum, ref Vector3 cameraPosition)
		{
			Vector3[] array = new Vector3[4];
			viewFrustum.GetFarCorners(array);
			int num = (int)Math.Floor((double)cameraPosition.X);
			int num2 = (int)Math.Floor((double)cameraPosition.X);
			int num3 = (int)Math.Floor((double)cameraPosition.Z);
			int num4 = (int)Math.Floor((double)cameraPosition.Z);
			for (int i = 0; i < 4; i++)
			{
				num = (((int)array[i].X > num) ? num : ((int)array[i].X));
				num2 = (((int)array[i].X < num2) ? num2 : ((int)array[i].X));
				num3 = (((int)array[i].Z > num3) ? num3 : ((int)array[i].Z));
				num4 = (((int)array[i].Z < num4) ? num4 : ((int)array[i].Z));
			}
			num >>= 5;
			num2 >>= 5;
			num3 >>= 5;
			num4 >>= 5;
			int chunksPerColumn = ChunkHelper.ChunksPerColumn;
			int num5 = 0;
			int num6 = chunksPerColumn;
			this._bitFieldChunksReadyForDraw.Setup(num - 1, num5 - 1, num3 - 1, num2 + 1, num6 + 1, num4 + 1);
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x000FE4D4 File Offset: 0x000FC6D4
		private void SetupBitField3D(Vector3 playerPosition)
		{
			int num = (int)Math.Floor((double)playerPosition.X) >> 5;
			int num2 = (int)Math.Floor((double)playerPosition.Y) >> 5;
			int num3 = (int)Math.Floor((double)playerPosition.Z) >> 5;
			int minX = num - (this.ViewRadius + 1);
			int maxX = num + (this.ViewRadius + 1);
			int minZ = num3 - (this.ViewRadius + 1);
			int maxZ = num3 + (this.ViewRadius + 1);
			int chunksPerColumn = ChunkHelper.ChunksPerColumn;
			int minY = 0;
			int maxY = chunksPerColumn;
			this._bitFieldChunksReadyForDraw.Setup(minX, minY, minZ, maxX, maxY, maxZ);
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x000FE568 File Offset: 0x000FC768
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool IsNear(float distance)
		{
			return distance < 64f;
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x000FE584 File Offset: 0x000FC784
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool ShouldUseAnimatedBlocks(float distance)
		{
			return distance < this.LODSetup.StartDistance;
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x000FE5A4 File Offset: 0x000FC7A4
		public int GetMaxChunksLoaded()
		{
			int num = this.ViewRadius * 2 + 1;
			return num * num * ChunkHelper.ChunksPerColumn;
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x000FE5CC File Offset: 0x000FC7CC
		public MapModule(GameInstance gameInstance) : base(gameInstance)
		{
			this.ViewRadius = Math.Min(this.MaxServerViewRadius, this._gameInstance.App.Settings.ViewDistance) / 32;
			this.TextureAtlas = new Texture(Texture.TextureTypes.Texture2D);
			this.TextureAtlas.CreateTexture2D(2048, 32, null, 5, GL.NEAREST_MIPMAP_NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this._bitFieldChunksReadyForDraw.Initialize(8192);
			this.LODSetup.Enabled = true;
			this.LODSetup.InvRange = 0.03125f;
			this.LODSetup.StartDistance = 160f;
			this.LODSetup.ShadowStartDistance = 48f;
			this.LODSetup.ShadowInvRange = 0.03125f;
			this.ComputeLightLevels(0.05f);
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x000FE7A0 File Offset: 0x000FC9A0
		protected override void DoDispose()
		{
			this.ClearAllColumns();
			bool flag = this._mapGeometryBuilder != null;
			if (flag)
			{
				this._mapGeometryBuilder.Dispose();
				this._mapGeometryBuilder = null;
			}
			this.TextureAtlas.Dispose();
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x000FE7E3 File Offset: 0x000FC9E3
		public void BeginFrame()
		{
			this._visibleAnimatedChunksCount = 0;
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x000FE7F0 File Offset: 0x000FC9F0
		public void ClearAllColumns()
		{
			MapGeometryBuilder mapGeometryBuilder = this._mapGeometryBuilder;
			if (mapGeometryBuilder != null)
			{
				mapGeometryBuilder.Suspend();
			}
			foreach (ChunkColumn chunkColumn in this._chunkColumns.Values)
			{
				this.DisposeChunkColumn(chunkColumn);
			}
			this._chunkColumns.Clear();
			MapGeometryBuilder mapGeometryBuilder2 = this._mapGeometryBuilder;
			if (mapGeometryBuilder2 != null)
			{
				mapGeometryBuilder2.Resume();
			}
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x000FE878 File Offset: 0x000FCA78
		public override void Initialize()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._mapGeometryBuilder = new MapGeometryBuilder(this._gameInstance);
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x000FE898 File Offset: 0x000FCA98
		public void SetClientBlock(int x, int y, int z, int blockId)
		{
			int block = this._gameInstance.MapModule.GetBlock(x, y, z, 1);
			bool flag = block == blockId;
			if (!flag)
			{
				this._gameInstance.InjectPacket(new ServerSetBlock(x, y, z, blockId, true));
				BlockParticleEvent blockParticleEvent = (block != 0 && blockId == 0) ? 7 : 8;
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[(blockId != 0) ? blockId : block];
				bool flag2 = clientBlockType.BlockParticleSetId != null;
				if (flag2)
				{
					this._gameInstance.InjectPacket(new SpawnBlockParticleSystem(blockId, blockParticleEvent, new Position((double)((float)x + 0.5f), (double)((float)y + 0.5f), (double)((float)z + 0.5f))));
				}
			}
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x000FE94C File Offset: 0x000FCB4C
		private void ComputeLightLevels(float ambientLight)
		{
			for (int i = 0; i < 16; i++)
			{
				this.LightLevels[i] = ambientLight + (float)Math.Pow((double)((float)i / 15f), 1.5) * (1f - ambientLight);
			}
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x000FE998 File Offset: 0x000FCB98
		public void SetAmbientLight(float ambientLight)
		{
			this.DoWithMapGeometryBuilderPaused(true, delegate
			{
				this.ComputeLightLevels(ambientLight);
			});
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x000FE9D0 File Offset: 0x000FCBD0
		private void UpdateAnimatedBlocks(Chunk chunk, RenderedChunk.ChunkUpdateTask chunkUpdateTask, float frameTime)
		{
			bool flag = chunk.Rendered.AnimatedBlocks != null;
			if (flag)
			{
				for (int i = 0; i < chunk.Rendered.AnimatedBlocks.Length; i++)
				{
					chunk.Rendered.AnimatedBlocks[i].Renderer.Dispose();
				}
				chunk.Rendered.AnimatedBlocks = null;
			}
			bool flag2 = ((chunkUpdateTask != null) ? chunkUpdateTask.AnimatedBlocks : null) != null;
			if (flag2)
			{
				chunk.Rendered.AnimatedBlocks = chunkUpdateTask.AnimatedBlocks;
				chunkUpdateTask.AnimatedBlocks = null;
				for (int j = 0; j < chunk.Rendered.AnimatedBlocks.Length; j++)
				{
					ref RenderedChunk.AnimatedBlock ptr = ref chunk.Rendered.AnimatedBlocks[j];
					ptr.Renderer.CreateGPUData(this._gameInstance.Engine.Graphics);
					int num = chunk.Data.Blocks.Get(ptr.Index);
					ClientBlockType clientBlockType = this.ClientBlockTypes[num];
					bool flag3 = clientBlockType.BlockyAnimation != null;
					if (flag3)
					{
						bool flag4 = !chunk.Data.CurrentInteractionStates.ContainsKey(ptr.Index);
						if (flag4)
						{
							this.SetBlockInteractionState(chunk, num, ptr.Index, ptr.Position, true, true);
						}
						ChunkData.InteractionStateInfo interactionStateInfo = chunk.Data.CurrentInteractionStates[ptr.Index];
						bool flag5 = interactionStateInfo.StateFrameTime == -1f;
						if (flag5)
						{
							interactionStateInfo.StateFrameTime = frameTime;
							chunk.Data.CurrentInteractionStates[ptr.Index] = interactionStateInfo;
						}
						float startTime = (interactionStateInfo.StateFrameTime == -2f && !clientBlockType.Looping) ? ((float)clientBlockType.BlockyAnimation.Duration) : ((frameTime - interactionStateInfo.StateFrameTime) * 60f);
						ptr.Renderer.SetSlotAnimationNoBlending(0, clientBlockType.BlockyAnimation, clientBlockType.Looping, 1f, startTime);
					}
					else
					{
						ptr.Renderer.SetSlotAnimationNoBlending(0, ptr.Animation, true, 1f, frameTime * 60f + ptr.AnimationTimeOffset);
					}
				}
			}
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x000FEC10 File Offset: 0x000FCE10
		public void UpdateSounds(Chunk chunk, RenderedChunk.ChunkUpdateTask chunkUpdateTask)
		{
			int num = (((chunkUpdateTask != null) ? chunkUpdateTask.SoundObjects : null) != null) ? chunkUpdateTask.SoundObjects.Length : 0;
			int num2 = (chunk.Rendered.SoundObjects != null) ? chunk.Rendered.SoundObjects.Length : 0;
			int num3 = 0;
			Dictionary<int, ChunkData.InteractionStateInfo> currentInteractionStates = chunk.Data.CurrentInteractionStates;
			AudioModule audioModule = this._gameInstance.AudioModule;
			int i = 0;
			while (i < num)
			{
				ref RenderedChunk.MapSoundObject ptr = ref chunkUpdateTask.SoundObjects[i];
				bool flag = num3 < num2;
				if (flag)
				{
					ref RenderedChunk.MapSoundObject ptr2 = ref chunk.Rendered.SoundObjects[num3];
					while (ptr.BlockIndex > ptr2.BlockIndex && num3 < num2)
					{
						audioModule.ActionOnEvent(ref ptr2.SoundEventReference, 0);
						audioModule.UnregisterSoundObject(ref ptr2.SoundEventReference.SoundObjectReference);
						num3++;
						bool flag2 = num3 < num2;
						if (flag2)
						{
							ptr2 = chunk.Rendered.SoundObjects[num3];
						}
					}
					bool flag3 = num3 < num2 && ptr.BlockIndex == ptr2.BlockIndex;
					if (flag3)
					{
						bool flag4 = ptr.SoundEventIndex == ptr2.SoundEventIndex;
						if (flag4)
						{
							ptr.SoundEventReference = ptr2.SoundEventReference;
							num3++;
							goto IL_1D6;
						}
						ptr.SoundEventReference.SoundObjectReference = ptr2.SoundEventReference.SoundObjectReference;
						audioModule.ActionOnEvent(ref ptr2.SoundEventReference, 0);
						num3++;
					}
					goto IL_174;
				}
				goto IL_174;
				IL_1D6:
				i++;
				continue;
				IL_174:
				bool flag5 = ptr.SoundEventReference.SoundObjectReference.SlotId != -1 || audioModule.TryRegisterSoundObject(ptr.Position, Vector3.Zero, ref ptr.SoundEventReference.SoundObjectReference, true);
				if (flag5)
				{
					audioModule.PlaySoundEvent(ptr.SoundEventIndex, ptr.SoundEventReference.SoundObjectReference, ref ptr.SoundEventReference);
				}
				goto IL_1D6;
			}
			for (int j = num3; j < num2; j++)
			{
				ref RenderedChunk.MapSoundObject ptr3 = ref chunk.Rendered.SoundObjects[j];
				bool flag6 = ptr3.SoundEventReference.SoundObjectReference.SlotId != -1;
				if (flag6)
				{
					this._gameInstance.AudioModule.ActionOnEvent(ref ptr3.SoundEventReference, 0);
					this._gameInstance.AudioModule.UnregisterSoundObject(ref ptr3.SoundEventReference.SoundObjectReference);
				}
			}
			bool flag7 = chunkUpdateTask != null;
			if (flag7)
			{
				chunk.Rendered.SoundObjects = chunkUpdateTask.SoundObjects;
				chunkUpdateTask.SoundObjects = null;
			}
			else
			{
				chunk.Rendered.SoundObjects = null;
			}
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x000FEEBC File Offset: 0x000FD0BC
		public void UpdateParticles(Chunk chunk, RenderedChunk.ChunkUpdateTask chunkUpdateTask)
		{
			int num = (((chunkUpdateTask != null) ? chunkUpdateTask.MapParticles : null) != null) ? chunkUpdateTask.MapParticles.Length : 0;
			int num2 = (chunk.Rendered.MapParticles != null) ? chunk.Rendered.MapParticles.Length : 0;
			int num3 = 0;
			int i = 0;
			while (i < num)
			{
				ref RenderedChunk.MapParticle ptr = ref chunkUpdateTask.MapParticles[i];
				bool flag = num3 < num2;
				if (flag)
				{
					ref RenderedChunk.MapParticle ptr2 = ref chunk.Rendered.MapParticles[num3];
					while (ptr.BlockIndex > ptr2.BlockIndex && num3 < num2)
					{
						bool flag2 = ptr2.ParticleSystemProxy != null;
						if (flag2)
						{
							ptr2.ParticleSystemProxy.Expire(false);
							ptr2.ParticleSystemProxy = null;
						}
						num3++;
						bool flag3 = num3 < num2;
						if (flag3)
						{
							ptr2 = chunk.Rendered.MapParticles[num3];
						}
					}
					bool flag4 = num3 < num2;
					if (flag4)
					{
						bool flag5 = ptr.BlockIndex == ptr2.BlockIndex && ptr.ParticleSystemId == ptr2.ParticleSystemId;
						if (flag5)
						{
							ptr.ParticleSystemProxy = ptr2.ParticleSystemProxy;
							num3++;
							goto IL_215;
						}
						bool flag6 = ptr.BlockIndex == ptr2.BlockIndex && ptr.ParticleSystemId != ptr2.ParticleSystemId;
						if (flag6)
						{
							bool flag7 = ptr2.ParticleSystemProxy != null;
							if (flag7)
							{
								ptr2.ParticleSystemProxy.Expire(false);
								ptr2.ParticleSystemProxy = null;
							}
						}
					}
					goto IL_18B;
				}
				goto IL_18B;
				IL_215:
				i++;
				continue;
				IL_18B:
				bool flag8 = this._gameInstance.ParticleSystemStoreModule.TrySpawnSystem(ptr.ParticleSystemId, out ptr.ParticleSystemProxy, false, true);
				if (flag8)
				{
					ptr.ParticleSystemProxy.Position = ptr.Position;
					ptr.ParticleSystemProxy.Rotation = ptr.RotationOffset;
					bool flag9 = !ptr.Color.IsTransparent;
					if (flag9)
					{
						ptr.ParticleSystemProxy.DefaultColor = ptr.Color;
					}
					ptr.ParticleSystemProxy.Scale = ptr.Scale;
				}
				goto IL_215;
			}
			for (int j = num3; j < num2; j++)
			{
				bool flag10 = chunk.Rendered.MapParticles[j].ParticleSystemProxy != null;
				if (flag10)
				{
					chunk.Rendered.MapParticles[j].ParticleSystemProxy.Expire(false);
					chunk.Rendered.MapParticles[j].ParticleSystemProxy = null;
				}
			}
			bool flag11 = chunkUpdateTask != null;
			if (flag11)
			{
				chunk.Rendered.MapParticles = chunkUpdateTask.MapParticles;
				chunkUpdateTask.MapParticles = null;
			}
			else
			{
				chunk.Rendered.MapParticles = null;
			}
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x000FF194 File Offset: 0x000FD394
		public void ResetParticleSystems()
		{
			Vector3 position = this._gameInstance.LocalPlayer.Position;
			int chunkX = (int)position.X >> 5;
			int chunkZ = (int)position.Z >> 5;
			this._spiralIterator.Initialize(chunkX, chunkZ, this.ViewRadius);
			foreach (long num in this._spiralIterator)
			{
				int num2 = ChunkHelper.XOfChunkColumnIndex(num);
				int num3 = ChunkHelper.ZOfChunkColumnIndex(num);
				ChunkColumn chunkColumn = this.GetChunkColumn(num);
				bool flag = chunkColumn == null;
				if (!flag)
				{
					for (int i = 0; i < ChunkHelper.ChunksPerColumn; i++)
					{
						Chunk chunk = chunkColumn.GetChunk(i);
						bool flag2 = chunk == null || chunk.Rendered == null || chunk.Rendered.MapParticles == null;
						if (!flag2)
						{
							for (int j = 0; j < chunk.Rendered.MapParticles.Length; j++)
							{
								ref RenderedChunk.MapParticle ptr = ref chunk.Rendered.MapParticles[j];
								bool flag3 = ptr.ParticleSystemProxy != null;
								if (flag3)
								{
									ptr.ParticleSystemProxy.Expire(true);
									ptr.ParticleSystemProxy = null;
								}
								bool flag4 = this._gameInstance.ParticleSystemStoreModule.TrySpawnSystem(ptr.ParticleSystemId, out ptr.ParticleSystemProxy, false, true);
								if (flag4)
								{
									ptr.ParticleSystemProxy.Position = ptr.Position;
									ptr.ParticleSystemProxy.Rotation = ptr.RotationOffset;
									bool flag5 = !ptr.Color.IsTransparent;
									if (flag5)
									{
										ptr.ParticleSystemProxy.DefaultColor = ptr.Color;
									}
									ptr.ParticleSystemProxy.Scale = ptr.Scale;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x000FF3A0 File Offset: 0x000FD5A0
		public void PrepareChunks(float frameTime)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(MapModule).FullName);
			}
			this.AreNearbyChunksRendered = true;
			int num = Math.Min(this.MaxServerViewRadius, this._gameInstance.App.Settings.ViewDistance) / 32;
			bool flag = this.ViewRadius != num;
			this.ViewRadius = num;
			Vector3 position = this._gameInstance.LocalPlayer.Position;
			int num2 = (int)position.X >> 5;
			int num3 = (int)position.Z >> 5;
			bool flag2 = num2 != this.StartChunkX || num3 != this.StartChunkZ;
			if (flag2)
			{
				this.StartChunkX = num2;
				this.StartChunkZ = num3;
				flag = true;
			}
			Settings settings = this._gameInstance.App.Settings;
			float num4 = (float)(settings.ViewDistance * settings.ViewDistance);
			float num5 = 0f;
			Vector2 vector = default(Vector2);
			int num6 = 2;
			int num7 = 2;
			ushort num8 = 0;
			ushort num9 = 0;
			ushort num10 = 0;
			this._chunkColumnCount = 0;
			this._animatedChunksCount = 0;
			this.SetupBitField3D(position);
			this._updatedChunksCount = 0;
			this._spiralIterator.Initialize(this.StartChunkX, this.StartChunkZ, num);
			foreach (long num11 in this._spiralIterator)
			{
				int num12 = ChunkHelper.XOfChunkColumnIndex(num11);
				int num13 = ChunkHelper.ZOfChunkColumnIndex(num11);
				bool flag3 = Math.Abs(num12 - this.StartChunkX) <= 1 && Math.Abs(num13 - this.StartChunkZ) <= 1;
				ChunkColumn chunkColumn = this.GetChunkColumn(num11);
				bool flag4 = chunkColumn == null;
				if (flag4)
				{
					bool flag5 = flag3;
					if (flag5)
					{
						this.AreNearbyChunksRendered = false;
					}
				}
				else
				{
					this._chunkColumnCount++;
					Vector3 vector2;
					vector2.X = (float)(num12 * 32);
					vector2.Z = (float)(num13 * 32);
					vector.X = ((float)num12 + 0.5f) * 32f - this._gameInstance.LocalPlayer.Position.X;
					vector.Y = ((float)num13 + 0.5f) * 32f - this._gameInstance.LocalPlayer.Position.Z;
					float num14 = vector.LengthSquared();
					for (int i = 0; i < ChunkHelper.ChunksPerColumn; i++)
					{
						Chunk chunk = chunkColumn.GetChunk(i);
						bool flag6 = chunk == null;
						if (flag6)
						{
							bool flag7 = flag3;
							if (flag7)
							{
								this.AreNearbyChunksRendered = false;
							}
						}
						else
						{
							num8 += 1;
							bool flag8 = chunk.Rendered == null;
							if (flag8)
							{
								bool flag9 = num6 > 0;
								if (!flag9)
								{
									bool flag10 = flag3;
									if (flag10)
									{
										this.AreNearbyChunksRendered = false;
									}
									goto IL_74E;
								}
								num6--;
								chunk.Initialize(this._gameInstance.Engine.Graphics);
							}
							bool flag11 = chunk.Rendered.GeometryNeedsUpdate && (chunk.Rendered.RebuildState == RenderedChunk.ChunkRebuildState.Waiting || chunk.Rendered.RebuildState == RenderedChunk.ChunkRebuildState.ReadyForRebuild) && this.HasReceivedAllAdjacentChunks(num12, i, num13, this.StartChunkX, this.StartChunkZ);
							if (flag11)
							{
								chunk.Rendered.GeometryNeedsUpdate = false;
								chunk.Rendered.RebuildState = RenderedChunk.ChunkRebuildState.ReadyForRebuild;
								flag = true;
							}
							bool flag12 = chunk.Rendered.RebuildState == RenderedChunk.ChunkRebuildState.UpdateReady;
							if (flag12)
							{
								object disposeLock = chunk.DisposeLock;
								RenderedChunk.ChunkUpdateTask updateTask;
								lock (disposeLock)
								{
									updateTask = chunk.Rendered.UpdateTask;
									chunk.Rendered.UpdateTask = null;
								}
								bool flag14 = num7 > 0 || updateTask == null;
								if (flag14)
								{
									this.UpdateChunkBufferData(chunk, updateTask);
									this.UpdateAnimatedBlocks(chunk, updateTask, frameTime);
									bool flag15 = ((updateTask != null) ? updateTask.MapParticles : null) != null || chunk.Rendered.MapParticles != null;
									if (flag15)
									{
										this.UpdateParticles(chunk, updateTask);
									}
									bool flag16 = ((updateTask != null) ? updateTask.SoundObjects : null) != null || chunk.Rendered.SoundObjects != null;
									if (flag16)
									{
										this.UpdateSounds(chunk, updateTask);
									}
									chunk.Rendered.BufferUpdateCount++;
									chunk.Rendered.RebuildState = RenderedChunk.ChunkRebuildState.Waiting;
									bool flag17 = (int)this._updatedChunksCount < this._updatedChunksPositions.Length;
									if (flag17)
									{
										this._updatedChunksPositions[(int)this._updatedChunksCount].X = chunk.X;
										this._updatedChunksPositions[(int)this._updatedChunksCount].Y = chunk.Y;
										this._updatedChunksPositions[(int)this._updatedChunksCount].Z = chunk.Z;
										this._updatedChunksCount += 1;
									}
									bool flag18 = updateTask != null;
									if (flag18)
									{
										num7--;
										this._mapGeometryBuilder.EnqueueChunkUpdateTask(updateTask);
									}
								}
								else
								{
									object disposeLock2 = chunk.DisposeLock;
									lock (disposeLock2)
									{
										chunk.Rendered.UpdateTask = updateTask;
									}
								}
							}
							bool flag20 = chunk.Rendered.BufferUpdateCount == 0;
							if (flag20)
							{
								bool flag21 = flag3;
								if (flag21)
								{
									this.AreNearbyChunksRendered = false;
								}
								num4 = MathHelper.Min(num4, num14);
							}
							else
							{
								num5 = num14;
							}
							bool flag22 = chunk.Rendered.BufferUpdateCount > 0;
							if (flag22)
							{
								this._bitFieldChunksReadyForDraw.SwitchBitOn(chunk.X, chunk.Y, chunk.Z);
								num9 += 1;
							}
							byte b = 0;
							b |= ((chunk.Rendered.OpaqueIndicesCount > 0) ? this.ChunkDrawTagOpaque : 0);
							b |= ((chunk.Rendered.AlphaTestedIndicesCount > 0) ? this.ChunkDrawTagAlphaTested : 0);
							b |= ((chunk.Rendered.AlphaBlendedIndicesCount > 0) ? this.ChunkDrawTagAlphaBlended : 0);
							b |= ((chunk.Rendered.AnimatedBlocks != null) ? this.ChunkDrawTagAnimated : 0);
							bool flag23 = b == 0;
							if (!flag23)
							{
								ArrayUtils.GrowArrayIfNecessary<Chunk>(ref this._chunks, (int)(num10 + 1), 1000);
								ArrayUtils.GrowArrayIfNecessary<byte>(ref this._drawMasks, (int)(num10 + 1), 1000);
								ArrayUtils.GrowArrayIfNecessary<bool>(ref this._undergroundHints, (int)(num10 + 1), 1000);
								ArrayUtils.GrowArrayIfNecessary<BoundingBox>(ref this._boundingVolumes, (int)(num10 + 1), 1000);
								ushort num15 = num10;
								this._chunks[(int)num15] = chunk;
								num10 += 1;
								this._drawMasks[(int)num15] = b;
								this._undergroundHints[(int)num15] = chunk.IsUnderground;
								bool flag24 = chunk.Rendered.AnimatedBlocks != null;
								if (flag24)
								{
									ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._animatedChunksLocalIds, (int)(this._animatedChunksCount + 1), 1000);
									ushort animatedChunksCount = this._animatedChunksCount;
									this._animatedChunksLocalIds[(int)animatedChunksCount] = num15;
									this._animatedChunksCount += 1;
								}
								vector2.Y = (float)(i * 32);
								BoundingBox boundingBox;
								boundingBox.Min = vector2;
								boundingBox.Max = vector2 + this._chunkSize;
								this._boundingVolumes[(int)num15] = boundingBox;
							}
						}
						IL_74E:;
					}
				}
			}
			this._chunksCount = num10;
			this.LoadedChunksCount = num8;
			this.DrawableChunksCount = num9;
			this.EffectiveViewDistance = MathHelper.Min((float)Math.Sqrt((double)num4), (float)Math.Sqrt((double)num5));
			bool flag25 = flag;
			if (flag25)
			{
				int startChunkY = MathHelper.Clamp((int)this._gameInstance.LocalPlayer.Position.Y >> 5, 0, ChunkHelper.ChunksPerColumn - 1);
				this._mapGeometryBuilder.RestartSpiral(ChunkHelper.WorldToChunk(position), num2, startChunkY, num3, this.ViewRadius);
			}
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x000FFC00 File Offset: 0x000FDE00
		public void Update(float deltaTime)
		{
			this._mapGeometryBuilder.HandleDisposeRequests();
			for (int i = 0; i < (int)this._animatedChunksCount; i++)
			{
				ushort num = this._animatedChunksLocalIds[i];
				Chunk chunk = this._chunks[(int)num];
				for (int j = 0; j < chunk.Rendered.AnimatedBlocks.Length; j++)
				{
					int index = chunk.Rendered.AnimatedBlocks[j].Index;
					chunk.Rendered.AnimatedBlocks[j].Renderer.AdvancePlayback(deltaTime * 60f);
					int num2;
					float num3;
					bool flag = chunk.Rendered.AnimatedBlocks[j].IsBeingHit && chunk.Data.TryGetBlockHitTimer(index, out num2, out num3);
					if (flag)
					{
						bool flag2 = deltaTime >= num3;
						if (flag2)
						{
							int worldX = chunk.X * 32 + index % 32;
							int worldY = chunk.Y * 32 + index / 32 / 32;
							int worldZ = chunk.Z * 32 + index / 32 % 32;
							this.SetBlockHitTimer(worldX, worldY, worldZ, 0f);
						}
						else
						{
							chunk.Data.BlockHitTimers[num2].Timer = num3 - deltaTime;
						}
					}
				}
			}
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x000FFD5C File Offset: 0x000FDF5C
		public void DoWithMapGeometryBuilderPaused(bool discardAllRenderedChunks, Action actionBeforeResume)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = this._mapGeometryBuilder != null;
			if (flag)
			{
				this._mapGeometryBuilder.Suspend();
				if (discardAllRenderedChunks)
				{
					foreach (ChunkColumn chunkColumn in this._chunkColumns.Values)
					{
						object disposeLock = chunkColumn.DisposeLock;
						lock (disposeLock)
						{
							bool flag3 = !chunkColumn.Disposed;
							if (flag3)
							{
								chunkColumn.DiscardRenderedChunks();
							}
						}
					}
					this._mapGeometryBuilder.EnsureEnoughChunkUpdateTasks();
					MapModule.Logger.Warn("All rendered chunks were discarded.");
				}
			}
			if (actionBeforeResume != null)
			{
				actionBeforeResume();
			}
			MapGeometryBuilder mapGeometryBuilder = this._mapGeometryBuilder;
			if (mapGeometryBuilder != null)
			{
				mapGeometryBuilder.Resume();
			}
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x000FFE64 File Offset: 0x000FE064
		public int GetChunkUpdateTaskQueueCount()
		{
			return this._mapGeometryBuilder.GetChunkUpdateTaskQueueCount();
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x000FFE74 File Offset: 0x000FE074
		private bool HasReceivedAllAdjacentChunks(int chunkX, int chunkY, int chunkZ, int startChunkX, int startChunkZ)
		{
			bool flag = Math.Abs(chunkX - startChunkX) >= this.ViewRadius || Math.Abs(chunkZ - startChunkZ) >= this.ViewRadius;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						ChunkColumn chunkColumn = this.GetChunkColumn(chunkX + (i - 1), chunkZ + (j - 1));
						bool flag2 = chunkColumn == null;
						if (flag2)
						{
							return false;
						}
						for (int k = 0; k < 3; k++)
						{
							int num = chunkY + (k - 1);
							bool flag3 = num < 0 || num >= ChunkHelper.ChunksPerColumn;
							if (!flag3)
							{
								Chunk chunk = chunkColumn.GetChunk(num);
								bool flag4 = ((chunk != null) ? chunk.Rendered : null) == null;
								if (flag4)
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

		// Token: 0x060045EA RID: 17898 RVA: 0x000FFF74 File Offset: 0x000FE174
		private unsafe void UpdateChunkBufferData(Chunk chunk, RenderedChunk.ChunkUpdateTask chunkUpdateTask)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			RenderedChunk rendered = chunk.Rendered;
			gl.BindVertexArray(rendered.OpaqueVertexArray);
			gl.BindBuffer(rendered.OpaqueVertexArray, GL.ARRAY_BUFFER, rendered.OpaqueVerticesBuffer);
			gl.BindBuffer(rendered.OpaqueVertexArray, GL.ELEMENT_ARRAY_BUFFER, rendered.OpaqueIndicesBuffer);
			rendered.OpaqueIndicesCount = ((chunkUpdateTask != null) ? chunkUpdateTask.OpaqueData.IndicesCount : 0);
			bool flag = rendered.OpaqueIndicesCount > 0;
			if (flag)
			{
				ChunkVertex[] array;
				ChunkVertex* value;
				if ((array = chunkUpdateTask.OpaqueData.Vertices) == null || array.Length == 0)
				{
					value = null;
				}
				else
				{
					value = &array[0];
				}
				gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(chunkUpdateTask.OpaqueData.VerticesCount * ChunkVertex.Size), (IntPtr)((void*)value), GL.STATIC_DRAW);
				array = null;
				uint[] array2;
				uint* value2;
				if ((array2 = chunkUpdateTask.OpaqueData.Indices) == null || array2.Length == 0)
				{
					value2 = null;
				}
				else
				{
					value2 = &array2[0];
				}
				gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(rendered.OpaqueIndicesCount * 4), (IntPtr)((void*)value2), GL.STATIC_DRAW);
				array2 = null;
			}
			else
			{
				gl.BufferData(GL.ARRAY_BUFFER, IntPtr.Zero, IntPtr.Zero, GL.STATIC_DRAW);
				gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, IntPtr.Zero, IntPtr.Zero, GL.STATIC_DRAW);
			}
			gl.BindVertexArray(rendered.AlphaBlendedVertexArray);
			gl.BindBuffer(rendered.AlphaBlendedVertexArray, GL.ARRAY_BUFFER, rendered.AlphaBlendedVerticesBuffer);
			gl.BindBuffer(rendered.AlphaBlendedVertexArray, GL.ELEMENT_ARRAY_BUFFER, rendered.AlphaBlendedIndicesBuffer);
			rendered.AlphaBlendedIndicesCount = ((chunkUpdateTask != null) ? chunkUpdateTask.AlphaBlendedData.IndicesCount : 0);
			bool flag2 = rendered.AlphaBlendedIndicesCount > 0;
			if (flag2)
			{
				ChunkVertex[] array;
				ChunkVertex* value3;
				if ((array = chunkUpdateTask.AlphaBlendedData.Vertices) == null || array.Length == 0)
				{
					value3 = null;
				}
				else
				{
					value3 = &array[0];
				}
				gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(chunkUpdateTask.AlphaBlendedData.VerticesCount * ChunkVertex.Size), (IntPtr)((void*)value3), GL.STATIC_DRAW);
				array = null;
				uint[] array2;
				uint* value4;
				if ((array2 = chunkUpdateTask.AlphaBlendedData.Indices) == null || array2.Length == 0)
				{
					value4 = null;
				}
				else
				{
					value4 = &array2[0];
				}
				gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(rendered.AlphaBlendedIndicesCount * 4), (IntPtr)((void*)value4), GL.STATIC_DRAW);
				array2 = null;
			}
			else
			{
				gl.BufferData(GL.ARRAY_BUFFER, IntPtr.Zero, IntPtr.Zero, GL.STATIC_DRAW);
				gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, IntPtr.Zero, IntPtr.Zero, GL.STATIC_DRAW);
			}
			gl.BindVertexArray(rendered.AlphaTestedVertexArray);
			gl.BindBuffer(rendered.AlphaTestedVertexArray, GL.ARRAY_BUFFER, rendered.AlphaTestedVerticesBuffer);
			gl.BindBuffer(rendered.AlphaTestedVertexArray, GL.ELEMENT_ARRAY_BUFFER, rendered.AlphaTestedIndicesBuffer);
			rendered.AlphaTestedAnimatedLowLODIndicesCount = ((chunkUpdateTask != null) ? chunkUpdateTask.AlphaTestedAnimatedLowLODIndicesCount : 0);
			rendered.AlphaTestedLowLODIndicesCount = ((chunkUpdateTask != null) ? chunkUpdateTask.AlphaTestedLowLODIndicesCount : 0);
			rendered.AlphaTestedHighLODIndicesCount = ((chunkUpdateTask != null) ? chunkUpdateTask.AlphaTestedHighLODIndicesCount : 0);
			rendered.AlphaTestedAnimatedHighLODIndicesCount = ((chunkUpdateTask != null) ? chunkUpdateTask.AlphaTestedAnimatedHighLODIndicesCount : 0);
			bool flag3 = rendered.AlphaTestedIndicesCount > 0;
			if (flag3)
			{
				ChunkVertex[] array;
				ChunkVertex* value5;
				if ((array = chunkUpdateTask.AlphaTestedData.Vertices) == null || array.Length == 0)
				{
					value5 = null;
				}
				else
				{
					value5 = &array[0];
				}
				gl.BufferData(GL.ARRAY_BUFFER, (IntPtr)(chunkUpdateTask.AlphaTestedData.VerticesCount * ChunkVertex.Size), (IntPtr)((void*)value5), GL.STATIC_DRAW);
				array = null;
				uint[] array2;
				uint* value6;
				if ((array2 = chunkUpdateTask.AlphaTestedData.Indices) == null || array2.Length == 0)
				{
					value6 = null;
				}
				else
				{
					value6 = &array2[0];
				}
				gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, (IntPtr)(rendered.AlphaTestedIndicesCount * 4), (IntPtr)((void*)value6), GL.STATIC_DRAW);
				array2 = null;
			}
			else
			{
				gl.BufferData(GL.ARRAY_BUFFER, IntPtr.Zero, IntPtr.Zero, GL.STATIC_DRAW);
				gl.BufferData(GL.ELEMENT_ARRAY_BUFFER, IntPtr.Zero, IntPtr.Zero, GL.STATIC_DRAW);
			}
			chunk.SolidPlaneMinY = ((chunkUpdateTask != null) ? chunkUpdateTask.SolidPlaneMinY : 0);
			chunk.IsUnderground = (chunkUpdateTask != null && chunkUpdateTask.IsUnderground);
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x00100418 File Offset: 0x000FE618
		public void ProcessFrustumCulling(SceneView sceneView)
		{
			ArrayUtils.GrowArrayIfNecessary<bool>(ref sceneView.ChunksFrustumCullingResults, (int)this._chunksCount, 1000);
			Vector3 cameraPosition = this._gameInstance.SceneRenderer.Data.CameraPosition;
			bool useKDopForCulling = sceneView.UseKDopForCulling;
			if (useKDopForCulling)
			{
				for (int i = 0; i < (int)this._chunksCount; i++)
				{
					BoundingBox volume = this._boundingVolumes[i];
					volume.Max -= cameraPosition;
					volume.Min -= cameraPosition;
					sceneView.ChunksFrustumCullingResults[i] = sceneView.KDopFrustum.Intersects(volume);
				}
			}
			else
			{
				for (int j = 0; j < (int)this._chunksCount; j++)
				{
					sceneView.Frustum.Intersects(ref this._boundingVolumes[j], out sceneView.ChunksFrustumCullingResults[j]);
				}
			}
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x00100510 File Offset: 0x000FE710
		public void GatherRenderableChunks(SceneView cameraSceneView)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(MapModule).FullName);
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			cameraSceneView.PrepareForIncomingChunks((int)this._chunksCount);
			for (int i = 0; i < (int)this._chunksCount; i++)
			{
				bool flag = cameraSceneView.ChunksFrustumCullingResults[i];
				bool flag2 = !flag;
				if (!flag2)
				{
					byte b = this._drawMasks[i];
					num += (((b & this.ChunkDrawTagOpaque) > 0) ? 1 : 0);
					num2 += (((b & this.ChunkDrawTagAlphaTested) > 0) ? 1 : 0);
					num3 += (((b & this.ChunkDrawTagAlphaBlended) > 0) ? 1 : 0);
					Vector3 center = this._boundingVolumes[i].GetCenter();
					cameraSceneView.RegisterChunk(i, center);
					bool flag3 = (b & this.ChunkDrawTagAnimated) > 0;
					if (flag3)
					{
						float distance = (center - cameraSceneView.Position).Length();
						bool flag4 = this.ShouldUseAnimatedBlocks(distance);
						if (flag4)
						{
							ArrayUtils.GrowArrayIfNecessary<ushort>(ref this._visibleAnimatedChunkIds, (int)(this._visibleAnimatedChunksCount + 1), 1000);
							this._visibleAnimatedChunkIds[(int)this._visibleAnimatedChunksCount] = (ushort)i;
							this._visibleAnimatedChunksCount += 1;
						}
					}
				}
			}
			this._gameInstance.SceneRenderer.PrepareForIncomingMapChunkDrawTasks(num, num2, num3);
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x00100678 File Offset: 0x000FE878
		public void PrepareChunksForDraw(SceneView cameraSceneView)
		{
			SceneRenderer sceneRenderer = this._gameInstance.SceneRenderer;
			Vector3 value = new Vector3(16f);
			bool shouldDrawAllChunksAsNear = this.ShouldDrawAllChunksAsNear;
			for (int i = 0; i < cameraSceneView.ChunksCount; i++)
			{
				int sortedChunkId = cameraSceneView.GetSortedChunkId(i);
				Chunk chunk = this._chunks[sortedChunkId];
				byte b = this._drawMasks[sortedChunkId];
				Vector3 value2;
				value2.X = (float)chunk.X * 32f;
				value2.Y = (float)chunk.Y * 32f;
				value2.Z = (float)chunk.Z * 32f;
				Vector3 vector = value2 - cameraSceneView.Position;
				Matrix matrix;
				Matrix.CreateTranslation(ref vector, out matrix);
				bool flag = chunk.SolidPlaneMinY > 0 && vector.Y <= -32f;
				if (flag)
				{
					sceneRenderer.PrepareForIncomingChunkOccluderPlane(1);
					sceneRenderer.RegisterChunkOccluderPlane(vector, (float)chunk.SolidPlaneMinY);
				}
				Vector3 value3 = value2 + value;
				float num = (value3 - cameraSceneView.Position).Length();
				float num2 = (num - this.LODSetup.StartDistance) * this.LODSetup.InvRange;
				num2 = 1f - MathHelper.Clamp(num2, 0f, 1f);
				num2 = (this.LODSetup.Enabled ? num2 : 1f);
				bool flag2 = this.IsNear(num);
				RenderedChunk rendered = chunk.Rendered;
				bool isNear = flag2 || shouldDrawAllChunksAsNear;
				bool flag3 = (b & this.ChunkDrawTagOpaque) > 0;
				if (flag3)
				{
					sceneRenderer.RegisterMapChunkOpaqueDrawTask(ref matrix, rendered.OpaqueVertexArray, rendered.OpaqueIndicesCount, isNear);
				}
				bool flag4 = (b & this.ChunkDrawTagAlphaTested) > 0;
				if (flag4)
				{
					bool useAnimatedBlocks = this.ShouldUseAnimatedBlocks(num);
					int indicesCount;
					IntPtr offset;
					rendered.GetAlphaTestedData(useAnimatedBlocks, num2, out indicesCount, out offset);
					sceneRenderer.RegisterMapChunkAlphaTestedDrawTask(ref matrix, rendered.AlphaTestedVertexArray, offset, indicesCount, isNear);
				}
				bool flag5 = (b & this.ChunkDrawTagAlphaBlended) > 0;
				if (flag5)
				{
					sceneRenderer.RegisterMapChunkAlphaBlendedDrawTask(ref matrix, rendered.AlphaBlendedVertexArray, rendered.AlphaBlendedIndicesCount, isNear);
				}
			}
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x001008A8 File Offset: 0x000FEAA8
		public void GatherRenderableAnimatedBlocks(SceneView cameraSceneView, SceneView sunSceneView, bool cullUndergroundShadowCasters = true)
		{
			SceneRenderer sceneRenderer = this._gameInstance.SceneRenderer;
			AnimationSystem animationSystem = this._gameInstance.Engine.AnimationSystem;
			FXSystem fxsystem = this._gameInstance.Engine.FXSystem;
			for (int i = 0; i < (int)this._visibleAnimatedChunksCount; i++)
			{
				ushort num = this._visibleAnimatedChunkIds[i];
				Chunk chunk = this._chunks[(int)num];
				Vector3 value;
				value.X = (float)chunk.X * 32f;
				value.Y = (float)chunk.Y * 32f;
				value.Z = (float)chunk.Z * 32f;
				Vector3 vector = value - cameraSceneView.Position;
				Matrix matrix;
				Matrix.CreateTranslation(ref vector, out matrix);
				RenderedChunk rendered = chunk.Rendered;
				int num2 = rendered.AnimatedBlocks.Length;
				sceneRenderer.PrepareForIncomingMapBlockAnimatedDrawTasks(num2);
				sceneRenderer.PrepareForIncomingMapBlockAnimatedSunShadowCasterDrawTasks(num2);
				animationSystem.PrepareForIncomingTasks(num2);
				for (int j = 0; j < num2; j++)
				{
					ref RenderedChunk.AnimatedBlock ptr = ref rendered.AnimatedBlocks[j];
					BoundingBox boundingBox = ptr.BoundingBox;
					bool flag = cameraSceneView.Frustum.Intersects(boundingBox);
					boundingBox.Min -= cameraSceneView.Position;
					boundingBox.Max -= cameraSceneView.Position;
					bool flag2 = sunSceneView != null && (sunSceneView.UseKDopForCulling ? sunSceneView.KDopFrustum.Intersects(boundingBox) : sunSceneView.Frustum.Intersects(boundingBox));
					bool flag3 = flag || flag2;
					bool flag4 = !flag3;
					if (!flag4)
					{
						animationSystem.RegisterAnimationTask(ptr.Renderer, false);
						float hitTimer = 0f;
						bool isBeingHit = ptr.IsBeingHit;
						if (isBeingHit)
						{
							int num3;
							chunk.Data.TryGetBlockHitTimer(ptr.Index, out num3, out hitTimer);
						}
						Matrix matrix2;
						animationSystem.ProcessHitBlockAnimation(hitTimer, ref ptr.Matrix, out matrix2);
						Matrix matrix3;
						Matrix.Multiply(ref matrix2, ref matrix, out matrix3);
						ref AnimatedBlockRenderer ptr2 = ref ptr.Renderer;
						bool flag5 = flag;
						if (flag5)
						{
							sceneRenderer.RegisterMapBlockAnimatedDrawTask(ref matrix3, ptr2.VertexArray, ptr2.IndicesCount, animationSystem.NodeBuffer, ptr2.NodeBufferOffset, (uint)ptr2.NodeCount);
						}
						bool flag6 = this._undergroundHints[(int)num];
						bool flag7 = cullUndergroundShadowCasters && flag6;
						bool flag8 = flag2 && !flag7;
						if (flag8)
						{
							sceneRenderer.RegisterMapBlockAnimatedSunShadowCasterDrawTask(ref boundingBox, ref matrix3, ptr2.VertexArray, ptr2.IndicesCount, animationSystem.NodeBuffer, ptr2.NodeBufferOffset, (uint)ptr2.NodeCount);
						}
						bool flag9 = flag && ptr.MapParticleIndices != null;
						if (flag9)
						{
							int num4 = ptr.MapParticleIndices.Length;
							fxsystem.Particles.PrepareForIncomingAnimatedBlockParticlesTasks(num4);
							for (int k = 0; k < num4; k++)
							{
								RenderedChunk.MapParticle mapParticle = rendered.MapParticles[ptr.MapParticleIndices[k]];
								bool flag10 = mapParticle.ParticleSystemProxy == null;
								if (!flag10)
								{
									fxsystem.Particles.RegisterFXAnimatedBlockParticlesTask(ptr.Renderer, mapParticle);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x00100BE4 File Offset: 0x000FEDE4
		public void GatherRenderableChunksForShadowMap(SceneView sunSceneView, bool cullUndergroundShadowCasters, int maxChunks = 100)
		{
			bool disposed = base.Disposed;
			if (disposed)
			{
				throw new ObjectDisposedException(typeof(MapModule).FullName);
			}
			int num = Math.Min((int)this._chunksCount, maxChunks);
			int num2 = 0;
			sunSceneView.PrepareForIncomingChunks(num);
			int num3 = 0;
			int num4 = 0;
			while (num4 < (int)this._chunksCount && num3 < num)
			{
				bool flag = sunSceneView.ChunksFrustumCullingResults[num4];
				bool flag2 = !flag;
				if (!flag2)
				{
					bool flag3 = this._undergroundHints[num4];
					bool flag4 = cullUndergroundShadowCasters && flag3;
					bool flag5 = flag4;
					if (!flag5)
					{
						byte b = this._drawMasks[num4];
						num2 += (((b & this.ChunkDrawTagOpaque) > 0) ? 1 : 0);
						num2 += (((b & this.ChunkDrawTagAlphaTested) > 0) ? 1 : 0);
						Vector3 center = this._boundingVolumes[num4].GetCenter();
						sunSceneView.RegisterChunk(num4, center);
						num3++;
					}
				}
				num4++;
			}
			this._gameInstance.SceneRenderer.PrepareForIncomingMapChunkSunShadowCasterDrawTasks(num2);
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x00100CF0 File Offset: 0x000FEEF0
		public void PrepareForSunShadowMapDraw(SceneView sunSceneView, Vector3 cameraPosition)
		{
			SceneRenderer sceneRenderer = this._gameInstance.SceneRenderer;
			Vector3 value = new Vector3(16f);
			for (int i = 0; i < sunSceneView.ChunksCount; i++)
			{
				int sortedChunkId = sunSceneView.GetSortedChunkId(i);
				Chunk chunk = this._chunks[sortedChunkId];
				Vector3 value2;
				value2.X = (float)chunk.X * 32f;
				value2.Y = (float)chunk.Y * 32f;
				value2.Z = (float)chunk.Z * 32f;
				value2 -= cameraPosition;
				Matrix matrix;
				Matrix.CreateTranslation(ref value2, out matrix);
				byte b = this._drawMasks[sortedChunkId];
				RenderedChunk rendered = chunk.Rendered;
				bool flag = (b & this.ChunkDrawTagOpaque) > 0;
				if (flag)
				{
					sceneRenderer.RegisterMapChunkSunShadowCasterDrawTask(ref matrix, rendered.OpaqueVertexArray, rendered.OpaqueIndicesCount, IntPtr.Zero);
				}
				bool flag2 = (b & this.ChunkDrawTagAlphaTested) > 0;
				if (flag2)
				{
					float num = (value2 + value).Length();
					float num2 = (num - this.LODSetup.ShadowStartDistance) * this.LODSetup.ShadowInvRange;
					num2 = 1f - MathHelper.Clamp(num2, 0f, 1f);
					num2 *= num2;
					bool useAnimatedBlocks = this.ShouldUseAnimatedBlocks(num);
					int dataCount;
					IntPtr dataOffset;
					rendered.GetAlphaTestedData(useAnimatedBlocks, num2, out dataCount, out dataOffset);
					sceneRenderer.RegisterMapChunkSunShadowCasterDrawTask(ref matrix, rendered.AlphaTestedVertexArray, dataCount, dataOffset);
				}
			}
		}

		// Token: 0x0400231D RID: 8989
		private const int InteractionStateDone = -2;

		// Token: 0x0400231E RID: 8990
		private const int InteractionStateStarting = -1;

		// Token: 0x0400231F RID: 8991
		private const int UnknownTextureIndex = 0;

		// Token: 0x04002320 RID: 8992
		public const string UnknownTexturePath = "BlockTextures/Unknown.png";

		// Token: 0x04002322 RID: 8994
		public readonly Texture TextureAtlas;

		// Token: 0x04002323 RID: 8995
		private readonly ConcurrentDictionary<long, ChunkColumn> _chunkColumns = new ConcurrentDictionary<long, ChunkColumn>();

		// Token: 0x04002324 RID: 8996
		private const int LightOctTreeSize = 8;

		// Token: 0x04002325 RID: 8997
		private const int LightOctMaxDepth = 12;

		// Token: 0x04002326 RID: 8998
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002327 RID: 8999
		private const int NearbyChunksRadius = 1;

		// Token: 0x04002328 RID: 9000
		private const int MaxNearChunkProgramColumns = 9;

		// Token: 0x04002329 RID: 9001
		public const float BlockHitDuration = 0.3f;

		// Token: 0x0400232C RID: 9004
		public ushort LoadedChunksCount;

		// Token: 0x0400232D RID: 9005
		public ushort DrawableChunksCount;

		// Token: 0x0400232E RID: 9006
		private MapGeometryBuilder _mapGeometryBuilder;

		// Token: 0x0400232F RID: 9007
		private readonly SpiralIterator _spiralIterator = new SpiralIterator();

		// Token: 0x04002330 RID: 9008
		public readonly float[] LightLevels = new float[16];

		// Token: 0x04002331 RID: 9009
		public MapModule.LevelOfDetailSetup LODSetup;

		// Token: 0x04002332 RID: 9010
		public int MaxServerViewRadius;

		// Token: 0x04002333 RID: 9011
		public int ChunkYMin;

		// Token: 0x04002337 RID: 9015
		private int _chunkColumnCount;

		// Token: 0x04002338 RID: 9016
		private const int ChunksDefaultSize = 1000;

		// Token: 0x04002339 RID: 9017
		private const int ChunksGrowth = 1000;

		// Token: 0x0400233A RID: 9018
		private Chunk[] _chunks = new Chunk[1000];

		// Token: 0x0400233B RID: 9019
		private ushort _chunksCount;

		// Token: 0x0400233C RID: 9020
		private byte[] _drawMasks = new byte[1000];

		// Token: 0x0400233D RID: 9021
		private bool[] _undergroundHints = new bool[1000];

		// Token: 0x0400233E RID: 9022
		private const int AnimatedChunksDefaultSize = 1000;

		// Token: 0x0400233F RID: 9023
		private const int AnimatedChunksGrowth = 1000;

		// Token: 0x04002340 RID: 9024
		private ushort[] _animatedChunksLocalIds = new ushort[1000];

		// Token: 0x04002341 RID: 9025
		private ushort _animatedChunksCount;

		// Token: 0x04002342 RID: 9026
		private ushort[] _visibleAnimatedChunkIds = new ushort[1000];

		// Token: 0x04002343 RID: 9027
		private ushort _visibleAnimatedChunksCount;

		// Token: 0x04002344 RID: 9028
		private const int BoundingVolumesDefaultSize = 1000;

		// Token: 0x04002345 RID: 9029
		private const int BoundingVolumesGrowth = 1000;

		// Token: 0x04002346 RID: 9030
		private BoundingBox[] _boundingVolumes = new BoundingBox[1000];

		// Token: 0x04002347 RID: 9031
		private ushort _updatedChunksCount;

		// Token: 0x04002348 RID: 9032
		private MapModule.ChunkPosition[] _updatedChunksPositions = new MapModule.ChunkPosition[128];

		// Token: 0x04002349 RID: 9033
		private MapModule.DestroyedBlocksInfo _destroyedBlocksInfo = new MapModule.DestroyedBlocksInfo(128);

		// Token: 0x0400234A RID: 9034
		private BitField3D _bitFieldChunksReadyForDraw;

		// Token: 0x0400234B RID: 9035
		private readonly byte ChunkDrawTagOpaque = 1;

		// Token: 0x0400234C RID: 9036
		private readonly byte ChunkDrawTagAlphaTested = 2;

		// Token: 0x0400234D RID: 9037
		private readonly byte ChunkDrawTagAlphaBlended = 4;

		// Token: 0x0400234E RID: 9038
		private readonly byte ChunkDrawTagAnimated = 8;

		// Token: 0x0400234F RID: 9039
		private readonly Vector3 _chunkSize = new Vector3(32f);

		// Token: 0x02000DD8 RID: 3544
		public struct AtlasLocation
		{
			// Token: 0x04004438 RID: 17464
			public Point Position;

			// Token: 0x04004439 RID: 17465
			public Point Size;

			// Token: 0x0400443A RID: 17466
			public int TileIndex;
		}

		// Token: 0x02000DD9 RID: 3545
		private class BlockyModelTextureInfo
		{
			// Token: 0x0400443B RID: 17467
			public string ServerPath;

			// Token: 0x0400443C RID: 17468
			public string Hash;

			// Token: 0x0400443D RID: 17469
			public MapModule.AtlasLocation AtlasLocation;
		}

		// Token: 0x02000DDA RID: 3546
		public struct LevelOfDetailSetup
		{
			// Token: 0x0400443E RID: 17470
			public bool Enabled;

			// Token: 0x0400443F RID: 17471
			public float InvRange;

			// Token: 0x04004440 RID: 17472
			public float StartDistance;

			// Token: 0x04004441 RID: 17473
			public float ShadowStartDistance;

			// Token: 0x04004442 RID: 17474
			public float ShadowInvRange;
		}

		// Token: 0x02000DDB RID: 3547
		public struct BlockPosition
		{
			// Token: 0x04004443 RID: 17475
			public int X;

			// Token: 0x04004444 RID: 17476
			public int Y;

			// Token: 0x04004445 RID: 17477
			public int Z;
		}

		// Token: 0x02000DDC RID: 3548
		private struct ChunkPosition
		{
			// Token: 0x04004446 RID: 17478
			public int X;

			// Token: 0x04004447 RID: 17479
			public int Y;

			// Token: 0x04004448 RID: 17480
			public int Z;
		}

		// Token: 0x02000DDD RID: 3549
		private struct DestroyedBlocksInfo
		{
			// Token: 0x17001459 RID: 5209
			// (get) Token: 0x06006666 RID: 26214 RVA: 0x00214017 File Offset: 0x00212217
			public int Count
			{
				get
				{
					return this._commitedChangeCount;
				}
			}

			// Token: 0x1700145A RID: 5210
			// (get) Token: 0x06006667 RID: 26215 RVA: 0x0021401F File Offset: 0x0021221F
			public Vector3[] BlockPositionsFromCamera
			{
				get
				{
					return this._commitedBlockPositionsFromCamera;
				}
			}

			// Token: 0x06006668 RID: 26216 RVA: 0x00214028 File Offset: 0x00212228
			public DestroyedBlocksInfo(int capacity)
			{
				this._deletedBlockCount = 0;
				this._deletedBlockPositions = new MapModule.BlockPosition[capacity];
				this._deletedBlockAge = new byte[capacity];
				this._commitedChangeCount = 0;
				this._commitedChangeIds = new ushort[capacity];
				this._commitedBlockPositionsFromCamera = new Vector3[capacity];
				this._commitedBlockDistanceFromCamera = new float[capacity];
			}

			// Token: 0x06006669 RID: 26217 RVA: 0x00214080 File Offset: 0x00212280
			public void RegisterDestroyedBlock(int x, int y, int z)
			{
				bool flag = this._deletedBlockCount == this._deletedBlockPositions.Length;
				if (!flag)
				{
					int deletedBlockCount = this._deletedBlockCount;
					this._deletedBlockPositions[deletedBlockCount].X = x;
					this._deletedBlockPositions[deletedBlockCount].Y = y;
					this._deletedBlockPositions[deletedBlockCount].Z = z;
					this._deletedBlockAge[deletedBlockCount] = 0;
					this._deletedBlockCount++;
				}
			}

			// Token: 0x0600666A RID: 26218 RVA: 0x002140F8 File Offset: 0x002122F8
			public void SafeRegisterDestroyedBlock(int x, int y, int z)
			{
				int num = Interlocked.Increment(ref this._deletedBlockCount);
				bool flag = num <= this._deletedBlockPositions.Length;
				if (flag)
				{
					int num2 = num - 1;
					this._deletedBlockPositions[num2].X = x;
					this._deletedBlockPositions[num2].Y = y;
					this._deletedBlockPositions[num2].Z = z;
					this._deletedBlockAge[num2] = 0;
				}
				else
				{
					Interlocked.Decrement(ref this._deletedBlockCount);
				}
			}

			// Token: 0x0600666B RID: 26219 RVA: 0x0021417C File Offset: 0x0021237C
			public void PrepareBlocksRemovedThisFrame(int updatedChunksCount, MapModule.ChunkPosition[] updatedChunksPositions, Vector3 previousCameraPosition, Vector3 cameraPosition, BoundingFrustum cameraFrustum, float rejectNearCameraDistance = 16f)
			{
				for (int i = 0; i < this._deletedBlockCount; i++)
				{
					for (int j = i + 1; j < this._deletedBlockCount; j++)
					{
						bool flag = this._deletedBlockPositions[i].X == this._deletedBlockPositions[j].X && this._deletedBlockPositions[i].Y == this._deletedBlockPositions[j].Y && this._deletedBlockPositions[i].Z == this._deletedBlockPositions[j].Z;
						if (flag)
						{
							for (int k = j + 1; k < this._deletedBlockCount; k++)
							{
								this._deletedBlockPositions[k - 1] = this._deletedBlockPositions[k];
							}
							this._deletedBlockCount--;
						}
					}
				}
				this._commitedChangeCount = 0;
				for (int l = 0; l < updatedChunksCount; l++)
				{
					MapModule.ChunkPosition chunkPosition = updatedChunksPositions[l];
					chunkPosition.X *= 32;
					chunkPosition.Y *= 32;
					chunkPosition.Z *= 32;
					MapModule.ChunkPosition chunkPosition2 = chunkPosition;
					chunkPosition2.X += 32;
					chunkPosition2.Y += 32;
					chunkPosition2.Z += 32;
					ushort num = 0;
					while ((int)num < this._deletedBlockCount)
					{
						bool flag2 = chunkPosition.X <= this._deletedBlockPositions[(int)num].X && this._deletedBlockPositions[(int)num].X < chunkPosition2.X && chunkPosition.Y <= this._deletedBlockPositions[(int)num].Y && this._deletedBlockPositions[(int)num].Y < chunkPosition2.Y && chunkPosition.Z <= this._deletedBlockPositions[(int)num].Z && this._deletedBlockPositions[(int)num].Z < chunkPosition2.Z;
						if (flag2)
						{
							this._commitedChangeIds[this._commitedChangeCount] = num;
							this._commitedChangeCount++;
							this._deletedBlockAge[(int)num] = byte.MaxValue;
						}
						num += 1;
					}
				}
				bool flag3 = this._commitedChangeCount > 0;
				if (flag3)
				{
					int num2 = 0;
					float num3 = rejectNearCameraDistance * rejectNearCameraDistance;
					Vector3 value = new Vector3(0.5f);
					for (int m = 0; m < this._commitedChangeCount; m++)
					{
						ushort num4 = this._commitedChangeIds[m];
						Vector3 vector;
						vector.X = (float)this._deletedBlockPositions[(int)num4].X - cameraPosition.X;
						vector.Y = (float)this._deletedBlockPositions[(int)num4].Y - cameraPosition.Y;
						vector.Z = (float)this._deletedBlockPositions[(int)num4].Z - cameraPosition.Z;
						BoundingBox box;
						box.Min = vector;
						box.Max = vector + Vector3.One;
						bool flag4 = cameraFrustum.Intersects(box);
						float num5 = (value + vector).LengthSquared();
						bool flag5 = num5 < num3 || !flag4;
						this._commitedBlockDistanceFromCamera[m] = (flag5 ? 1000000f : num5);
						this._commitedBlockPositionsFromCamera[m] = vector + cameraPosition - previousCameraPosition;
						num2 += (flag5 ? 1 : 0);
					}
					this._commitedChangeCount -= num2;
					Array.Sort<float, Vector3>(this._commitedBlockDistanceFromCamera, this._commitedBlockPositionsFromCamera, 0, this._commitedChangeCount);
				}
				this.ClearProcessedBlocks();
			}

			// Token: 0x0600666C RID: 26220 RVA: 0x0021457C File Offset: 0x0021277C
			private void ClearProcessedBlocks()
			{
				for (int i = 0; i < this._deletedBlockCount; i++)
				{
					bool flag = this._deletedBlockAge[i] >= 35;
					if (flag)
					{
						for (int j = i + 1; j < this._deletedBlockCount; j++)
						{
							this._deletedBlockPositions[j - 1] = this._deletedBlockPositions[j];
							this._deletedBlockAge[j - 1] = this._deletedBlockAge[j];
						}
						this._deletedBlockCount--;
					}
					else
					{
						byte[] deletedBlockAge = this._deletedBlockAge;
						int num = i;
						deletedBlockAge[num] += 1;
					}
				}
			}

			// Token: 0x04004449 RID: 17481
			private int _deletedBlockCount;

			// Token: 0x0400444A RID: 17482
			private MapModule.BlockPosition[] _deletedBlockPositions;

			// Token: 0x0400444B RID: 17483
			private byte[] _deletedBlockAge;

			// Token: 0x0400444C RID: 17484
			private int _commitedChangeCount;

			// Token: 0x0400444D RID: 17485
			public ushort[] _commitedChangeIds;

			// Token: 0x0400444E RID: 17486
			private Vector3[] _commitedBlockPositionsFromCamera;

			// Token: 0x0400444F RID: 17487
			private float[] _commitedBlockDistanceFromCamera;
		}
	}
}
