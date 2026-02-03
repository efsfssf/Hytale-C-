using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.FX;
using HytaleClient.Data.Map;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;
using SDL2;

namespace HytaleClient.InGame.Modules.Particles
{
	// Token: 0x02000904 RID: 2308
	internal class ParticleSystemStoreModule : Module
	{
		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x0600454E RID: 17742 RVA: 0x000F229B File Offset: 0x000F049B
		public int SystemCount
		{
			get
			{
				return this._gameInstance.Engine.FXSystem.Particles.ParticleSystemCount;
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x0600454F RID: 17743 RVA: 0x000F22B7 File Offset: 0x000F04B7
		public int MaxSpawnedSystems
		{
			get
			{
				return this._gameInstance.Engine.FXSystem.Particles.MaxParticleSystemSpawned;
			}
		}

		// Token: 0x06004550 RID: 17744 RVA: 0x000F22D3 File Offset: 0x000F04D3
		public void SetMaxSpawnedSystems(int max)
		{
			this._gameInstance.Engine.FXSystem.Particles.SetMaxParticleSystemSpawned(max);
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x000F22F4 File Offset: 0x000F04F4
		public ParticleSystemStoreModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._gameInstance.Engine.FXSystem.Particles.InitializeFunctions(new UpdateSpawnerLightingFunc(this.UpdateParticleSpawnerLighting), new UpdateParticleCollisionFunc(this.UpdateParticleCollision), new InitParticleFunc(this.InitParticle));
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x000F2386 File Offset: 0x000F0586
		protected override void DoDispose()
		{
			this._gameInstance.Engine.FXSystem.Particles.DisposeFunctions();
			this.Clear();
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x000F23AC File Offset: 0x000F05AC
		private void UpdateParticleSpawnerLighting(ParticleSpawner particleSpawnerInstance)
		{
			bool flag = particleSpawnerInstance.LightInfluence > 0f && !this._gameInstance.MapModule.Disposed;
			if (flag)
			{
				Vector4 vector = Vector4.One;
				bool isOvergroundOnly = particleSpawnerInstance.IsOvergroundOnly;
				if (isOvergroundOnly)
				{
					vector = new Vector4(this._gameInstance.WeatherModule.SunlightColor * this._gameInstance.WeatherModule.SunLight, 1f);
				}
				else
				{
					vector = this._gameInstance.MapModule.GetLightColorAtBlockPosition((int)Math.Floor((double)particleSpawnerInstance.Position.X), (int)Math.Floor((double)particleSpawnerInstance.Position.Y), (int)Math.Floor((double)particleSpawnerInstance.Position.Z));
				}
				vector = Vector4.Lerp(Vector4.One, vector, particleSpawnerInstance.LightInfluence);
				particleSpawnerInstance.UpdateLight(vector);
			}
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x000F248C File Offset: 0x000F068C
		private void UpdateParticleCollision(ParticleSpawner particleSpawner, ref ParticleBuffers.ParticleSimulationData particleData0, ref ParticleBuffers.ParticleRenderData particleData1, ref Vector2 particleScale, ref ParticleBuffers.ParticleLifeData particleLife, Vector3 previousPosition, Quaternion inverseRotation)
		{
			Vector3 vector = particleSpawner.Position + Vector3.Transform(particleData1.Position, particleSpawner.Rotation);
			int worldX = (int)Math.Floor((double)vector.X);
			int worldY = (int)Math.Floor((double)vector.Y);
			int worldZ = (int)Math.Floor((double)vector.Z);
			int block = this._gameInstance.MapModule.GetBlock(worldX, worldY, worldZ, 0);
			ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
			bool flag = (float)clientBlockType.VerticalFill * (1f / (float)clientBlockType.MaxFillLevel) > vector.Y % 1f;
			bool flag2 = (flag && ((particleSpawner.ParticleCollisionBlockType == ParticleFXSystem.ParticleCollisionBlockType.All && this._gameInstance.MapModule.ClientBlockTypes[block].CollisionMaterial != null) || (particleSpawner.ParticleCollisionBlockType == ParticleFXSystem.ParticleCollisionBlockType.Solid && this._gameInstance.MapModule.ClientBlockTypes[block].CollisionMaterial == 1))) || ((!flag || (flag && (this._gameInstance.MapModule.ClientBlockTypes[block].CollisionMaterial == null || this._gameInstance.MapModule.ClientBlockTypes[block].CollisionMaterial == 1))) && particleSpawner.ParticleCollisionBlockType == ParticleFXSystem.ParticleCollisionBlockType.Air);
			if (flag2)
			{
				BitUtils.SwitchOnBit(2, ref particleData1.BoolData);
				bool flag3 = particleSpawner.ParticleCollisionAction == ParticleFXSystem.ParticleCollisionAction.Expire || particleLife.LifeSpanTimer == particleLife.LifeSpan;
				if (flag3)
				{
					particleLife.LifeSpanTimer = 0.0001f;
					particleScale = Vector2.Zero;
				}
				else
				{
					ParticleSystemStoreModule.RaycastOptions.Distance = Vector3.Distance(particleData1.Position, previousPosition);
					Vector3 vector2 = Vector3.Transform(particleData1.Position, particleSpawner.Rotation) - Vector3.Transform(previousPosition, particleSpawner.Rotation);
					HitDetection.RaycastHit raycastHit;
					bool flag4 = ParticleSystemStoreModule.RaycastOptions.Distance != 0f && this._gameInstance.HitDetection.RaycastBlock(particleSpawner.Position + Vector3.Transform(previousPosition, particleSpawner.Rotation), vector2, ParticleSystemStoreModule.RaycastOptions, out raycastHit);
					if (flag4)
					{
						particleData1.Position = Vector3.Transform(raycastHit.HitPosition - particleSpawner.Position, inverseRotation) - Vector3.Normalize(vector2) * 0.05f;
					}
				}
			}
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x000F26E0 File Offset: 0x000F08E0
		private bool InitParticle(ParticleSpawner particleSpawner, ref Vector3 particlePosition)
		{
			bool isOvergroundOnly = particleSpawner.IsOvergroundOnly;
			if (isOvergroundOnly)
			{
				Vector3 vector = particleSpawner.Position + particlePosition;
				int num = (int)Math.Floor((double)vector.X);
				int num2 = (int)Math.Floor((double)vector.Z);
				int num3 = num >> 5;
				int num4 = num2 >> 5;
				ChunkColumn chunkColumn = this._gameInstance.MapModule.GetChunkColumn(num3, num4);
				bool flag = chunkColumn != null;
				if (flag)
				{
					int num5 = num - num3 * 32;
					int num6 = num2 - num4 * 32;
					int num7 = (num6 << 5) + num5;
					bool flag2 = vector.Y < (float)(chunkColumn.Heights[num7] + 1);
					if (flag2)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x000F279C File Offset: 0x000F099C
		public void PrepareParticles(Dictionary<string, ParticleSpawner> networkParticleSpawners, out Dictionary<string, ParticleSettings> upcomingParticles, out Dictionary<string, PacketHandler.TextureInfo> upcomingTextureInfo, out List<string> upcomingUVMotionTexturePaths, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingParticles = new Dictionary<string, ParticleSettings>();
			upcomingTextureInfo = new Dictionary<string, PacketHandler.TextureInfo>();
			upcomingUVMotionTexturePaths = new List<string>(32);
			int num = 0;
			int num2 = 0;
			foreach (ParticleSpawner particleSpawner in networkParticleSpawners.Values)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					break;
				}
				bool flag = particleSpawner.Particle_ == null;
				if (!flag)
				{
					ParticleSettings particleSettings = new ParticleSettings();
					ParticleCollision particleCollision_ = particleSpawner.ParticleCollision_;
					ParticleRotationInfluence collisionRotationInfluence = (particleCollision_ != null) ? particleCollision_.ParticleRotationInfluence_ : 0;
					ParticleProtocolInitializer.Initialize(particleSpawner.Particle_, particleSpawner.ParticleRotationInfluence_, collisionRotationInfluence, ref particleSettings);
					string text;
					bool flag2 = particleSettings.TexturePath == null || !this._gameInstance.HashesByServerAssetPath.TryGetValue(particleSettings.TexturePath, ref text);
					if (flag2)
					{
						this._gameInstance.App.DevTools.Error("Missing particle texture: " + particleSettings.TexturePath + " for particle " + particleSpawner.Id);
					}
					else
					{
						PacketHandler.TextureInfo textureInfo;
						bool flag3 = !upcomingTextureInfo.TryGetValue(text, out textureInfo);
						if (flag3)
						{
							textureInfo = new PacketHandler.TextureInfo
							{
								Checksum = text
							};
							string assetLocalPathUsingHash = AssetManager.GetAssetLocalPathUsingHash(text);
							bool flag4 = Image.TryGetPngDimensions(assetLocalPathUsingHash, out textureInfo.Width, out textureInfo.Height);
							if (flag4)
							{
								upcomingTextureInfo[text] = textureInfo;
								bool flag5 = textureInfo.Width % 32 != 0 || textureInfo.Height % 32 != 0 || textureInfo.Width < 32 || textureInfo.Height < 32;
								if (flag5)
								{
									this._gameInstance.App.DevTools.Warn(string.Format("Texture width/height must be a multiple of 32 and at least 32x32: {0} ({1}x{2})", particleSettings.TexturePath, textureInfo.Width, textureInfo.Height));
								}
							}
							else
							{
								this._gameInstance.App.DevTools.Error(string.Concat(new string[]
								{
									"Failed to get PNG dimensions for: ",
									particleSettings.TexturePath,
									", ",
									assetLocalPathUsingHash,
									" (",
									text,
									")"
								}));
							}
						}
						bool flag6 = particleSpawner.UvMotion_ != null && particleSpawner.UvMotion_.Texture != null;
						if (flag6)
						{
							bool flag7 = !Image.TryGetPngDimensions(Path.Combine(Paths.BuiltInAssets, "Common", particleSpawner.UvMotion_.Texture), out num, out num2);
							if (flag7)
							{
								this._gameInstance.App.DevTools.Error("Missing particle UV motion texture: " + Path.Combine(Paths.BuiltInAssets, "Common", particleSpawner.UvMotion_.Texture) + " for particle " + particleSpawner.Id);
							}
							else
							{
								bool flag8 = num != 64 || num2 != 64;
								if (flag8)
								{
									this._gameInstance.App.DevTools.Error(string.Format("UV motion exture width/height must be 64x64: {0} ({1}x{2})", particleSpawner.UvMotion_.Texture, num, num2));
								}
								else
								{
									bool flag9 = !upcomingUVMotionTexturePaths.Contains(particleSpawner.UvMotion_.Texture);
									if (flag9)
									{
										upcomingUVMotionTexturePaths.Add(particleSpawner.UvMotion_.Texture);
									}
								}
							}
						}
						upcomingParticles[particleSpawner.Id] = particleSettings;
					}
				}
			}
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x000F2B48 File Offset: 0x000F0D48
		public void SetupParticleSystems(Dictionary<string, ParticleSystem> networkParticleSystems)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, ParticleSystem> keyValuePair in networkParticleSystems)
			{
				try
				{
					ParticleSystemSettings particleSystemSettings = new ParticleSystemSettings();
					ParticleProtocolInitializer.Initialize(keyValuePair.Value, ref particleSystemSettings);
					int i = 0;
					while (i < (int)particleSystemSettings.SystemSpawnerCount)
					{
						ParticleSystemSettings.SystemSpawnerSettings systemSpawnerSettings = particleSystemSettings.SystemSpawnerSettingsList[i];
						ParticleSpawnerSettings particleSpawnerSettings;
						bool flag = this.LoadParticleSpawnerSettings(systemSpawnerSettings.ParticleSpawnerId, out particleSpawnerSettings);
						if (flag)
						{
							systemSpawnerSettings.ParticleSpawnerSettings = particleSpawnerSettings;
							i++;
						}
						else
						{
							particleSystemSettings.DeleteSpawnerSettings((byte)i);
						}
					}
					bool flag2 = particleSystemSettings.SystemSpawnerCount == 0;
					if (flag2)
					{
						this._gameInstance.App.DevTools.Error("No valid spawner settings listed in: " + keyValuePair.Key);
						list.Add(keyValuePair.Key);
					}
					this._systemSettingsById[keyValuePair.Key] = particleSystemSettings;
				}
				catch (Exception exception)
				{
					ParticleSystemStoreModule.Logger.Error(exception, "Failed to setup particle system! {0}:", new object[]
					{
						keyValuePair.Key
					});
					this._gameInstance.App.DevTools.Error("Error failed to setup particle system: " + keyValuePair.Key);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				this._systemSettingsById.Remove(list[j]);
			}
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x000F2D18 File Offset: 0x000F0F18
		public void RemoveParticleSystems(Dictionary<string, ParticleSystem> networkParticleSystems)
		{
			foreach (string key in networkParticleSystems.Keys)
			{
				this._systemSettingsById.Remove(key);
			}
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x000F2D78 File Offset: 0x000F0F78
		public void SetupParticleSpawners(Dictionary<string, ParticleSystem> networkParticleSystems, Dictionary<string, ParticleSpawner> networkParticleSpawners, Dictionary<string, ParticleSettings> upcomingParticles, List<string> upcomingUVMotionTexturePaths)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._particlesByFile = upcomingParticles;
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, ParticleSpawner> keyValuePair in networkParticleSpawners)
			{
				try
				{
					ParticleSpawnerSettings particleSpawnerSettings = new ParticleSpawnerSettings();
					ParticleProtocolInitializer.Initialize(keyValuePair.Value, ref particleSpawnerSettings);
					ParticleSettings particleSettings;
					bool flag = !this._particlesByFile.TryGetValue(keyValuePair.Value.Id, out particleSettings);
					if (flag)
					{
						this._gameInstance.App.DevTools.Error("Could not load particle settings for spawner " + keyValuePair.Key);
						list.Add(keyValuePair.Key);
					}
					else
					{
						bool flag2 = particleSettings.TexturePath == null || !this._gameInstance.HashesByServerAssetPath.ContainsKey(particleSettings.TexturePath);
						if (flag2)
						{
							this._gameInstance.App.DevTools.Error("Failed to find particle texture: " + particleSettings.TexturePath + " for spawner " + keyValuePair.Key);
							list.Add(keyValuePair.Key);
						}
						particleSpawnerSettings.ParticleSettings = particleSettings;
						bool flag3 = particleSpawnerSettings.UVMotion.TexturePath != null && upcomingUVMotionTexturePaths.Contains(particleSpawnerSettings.UVMotion.TexturePath);
						if (flag3)
						{
							particleSpawnerSettings.UVMotion.TextureId = upcomingUVMotionTexturePaths.IndexOf(particleSpawnerSettings.UVMotion.TexturePath);
						}
						else
						{
							particleSpawnerSettings.UVMotion.TextureId = -1;
						}
					}
					this._spawnerSettingsById[keyValuePair.Key] = particleSpawnerSettings;
				}
				catch (Exception exception)
				{
					ParticleSystemStoreModule.Logger.Error(exception, "Failed to setup particle spawner! {0}:", new object[]
					{
						keyValuePair.Key
					});
					this._gameInstance.App.DevTools.Error("Error failed to setup particle spawner: " + keyValuePair.Key);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				this._spawnerSettingsById.Remove(list[i]);
			}
			this.SetupParticleSystems(networkParticleSystems);
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x000F2FEC File Offset: 0x000F11EC
		public void RemoveParticleSpawners(Dictionary<string, ParticleSystem> networkParticleSystems, Dictionary<string, ParticleSpawner> networkParticleSpawners, Dictionary<string, ParticleSettings> upcomingParticles)
		{
			this._particlesByFile = upcomingParticles;
			foreach (string key in networkParticleSpawners.Keys)
			{
				this._spawnerSettingsById.Remove(key);
			}
			this.SetupParticleSystems(networkParticleSystems);
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x000F305C File Offset: 0x000F125C
		public void UpdateTextures()
		{
			foreach (ParticleSettings particleSettings in this._particlesByFile.Values)
			{
				string key;
				Rectangle imageLocation;
				bool flag = !this._gameInstance.HashesByServerAssetPath.TryGetValue(particleSettings.TexturePath, ref key) || !this._gameInstance.FXModule.ImageLocations.TryGetValue(key, out imageLocation);
				if (flag)
				{
					this._gameInstance.App.DevTools.Error("Missing particle texture: " + particleSettings.TexturePath + " for particle " + particleSettings.Id);
				}
				else
				{
					particleSettings.ImageLocation = imageLocation;
				}
			}
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x000F3134 File Offset: 0x000F1334
		public void ResetParticleSystems(bool skipEntities = false)
		{
			this.ResetDebugParticleSystems();
			bool flag = !skipEntities;
			if (flag)
			{
				this._gameInstance.EntityStoreModule.RebuildRenderers(false);
			}
			this._gameInstance.EntityStoreModule.ResetMovementParticleSystems();
			this._gameInstance.WeatherModule.ResetParticleSystems();
			this._gameInstance.MapModule.ResetParticleSystems();
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x000F3198 File Offset: 0x000F1398
		public bool CheckSettingsExist(string systemId)
		{
			bool flag = this._systemSettingsById.ContainsKey(systemId);
			bool flag2 = !flag;
			if (flag2)
			{
				this._gameInstance.App.DevTools.Error("Could not find particle system settings: " + systemId);
			}
			return flag;
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x000F31E4 File Offset: 0x000F13E4
		private bool LoadParticleSpawnerSettings(string spawnerId, out ParticleSpawnerSettings spawnerSettings)
		{
			bool flag = !this._spawnerSettingsById.TryGetValue(spawnerId, out spawnerSettings);
			bool result;
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Could not find particle spawner settings " + spawnerId);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x000F3231 File Offset: 0x000F1431
		public void ClearDebug()
		{
			this._gameInstance.Engine.FXSystem.Particles.ClearParticleSystemDebugs();
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x000F324E File Offset: 0x000F144E
		public void Clear()
		{
			this._gameInstance.Engine.FXSystem.Particles.ClearParticleSystems();
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x000F326C File Offset: 0x000F146C
		public bool TrySpawnBlockSystem(Vector3 position, ClientBlockType blockType, ClientBlockParticleEvent particleEvent, out ParticleSystemProxy particleSystemProxy, bool faceCameraYaw = false, bool isTracked = false)
		{
			particleSystemProxy = null;
			bool flag = blockType.BlockParticleSetId == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ClientBlockParticleSet clientBlockParticleSet;
				string systemId;
				bool flag2 = this._gameInstance.ServerSettings.BlockParticleSets.TryGetValue(blockType.BlockParticleSetId, out clientBlockParticleSet) && clientBlockParticleSet.ParticleSystemIds.TryGetValue(particleEvent, out systemId) && this.TrySpawnSystem(systemId, out particleSystemProxy, false, isTracked);
				if (flag2)
				{
					bool flag3 = !blockType.ParticleColor.IsTransparent;
					if (flag3)
					{
						particleSystemProxy.DefaultColor = blockType.ParticleColor;
					}
					else
					{
						bool flag4 = blockType.BiomeTintMultipliersBySide[0] != 0f;
						if (flag4)
						{
							Vector3 blockEnvironmentTint = this._gameInstance.MapModule.GetBlockEnvironmentTint((int)Math.Floor((double)position.X), (int)Math.Floor((double)position.Y), (int)Math.Floor((double)position.Z), blockType);
							particleSystemProxy.DefaultColor = UInt32Color.FromRGBA((byte)blockEnvironmentTint.X, (byte)blockEnvironmentTint.Y, (byte)blockEnvironmentTint.Z, byte.MaxValue);
						}
						else
						{
							bool flag5 = blockType.FluidFXIndex != 0;
							if (flag5)
							{
								Vector3 blockFluidTint = this._gameInstance.MapModule.GetBlockFluidTint((int)Math.Floor((double)position.X), (int)Math.Floor((double)position.Y), (int)Math.Floor((double)position.Z), blockType);
								particleSystemProxy.DefaultColor = UInt32Color.FromRGBA((byte)blockFluidTint.X, (byte)blockFluidTint.Y, (byte)blockFluidTint.Z, byte.MaxValue);
							}
							else
							{
								particleSystemProxy.DefaultColor = clientBlockParticleSet.Color;
							}
						}
					}
					particleSystemProxy.Scale = clientBlockParticleSet.Scale;
					particleSystemProxy.Position = clientBlockParticleSet.PositionOffset;
					if (faceCameraYaw)
					{
						particleSystemProxy.Rotation = Quaternion.CreateFromYawPitchRoll(this._gameInstance.CameraModule.Controller.Rotation.Yaw, 0f, 0f);
					}
					else
					{
						particleSystemProxy.Rotation = clientBlockParticleSet.RotationOffset;
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x000F3480 File Offset: 0x000F1680
		public bool TrySpawnSystem(string systemId, out ParticleSystemProxy particleSystemProxy, bool isLocalPlayer = false, bool isTracked = false)
		{
			particleSystemProxy = null;
			ParticleSystemSettings settings;
			bool flag = !this._systemSettingsById.TryGetValue(systemId, out settings);
			bool result;
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Could not find particle system settings: " + systemId);
				result = false;
			}
			else
			{
				Vector2 textureAltasInverseSize = new Vector2(1f / (float)this._gameInstance.FXModule.TextureAtlas.Width, 1f / (float)this._gameInstance.FXModule.TextureAtlas.Height);
				bool flag2 = !this._gameInstance.Engine.FXSystem.Particles.TrySpawnParticleSystemProxy(settings, textureAltasInverseSize, out particleSystemProxy, isLocalPlayer, isTracked);
				if (flag2)
				{
					ParticleSystemStoreModule.Logger.Warn("Particle system proxy limit reached");
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x000F3554 File Offset: 0x000F1754
		public void PreUpdate(Vector3 cameraPosition)
		{
			SceneRenderer sceneRenderer = this._gameInstance.SceneRenderer;
			FXSystem fxsystem = this._gameInstance.Engine.FXSystem;
			fxsystem.Particles.CleanDeadProxies();
			ParticleSystemProxy[] particleSystemProxies = fxsystem.Particles.ParticleSystemProxies;
			int particleSystemProxyCount = fxsystem.Particles.ParticleSystemProxyCount;
			sceneRenderer.PrepareForIncomingOccludeeParticle(particleSystemProxyCount);
			for (int i = 0; i < particleSystemProxyCount; i++)
			{
				Vector3 value = particleSystemProxies[i].Position - cameraPosition;
				Vector3 value2 = new Vector3(particleSystemProxies[i].Settings.BoundingRadius);
				BoundingBox boundingBox;
				boundingBox.Min = value - value2;
				boundingBox.Max = value + value2;
				sceneRenderer.RegisterOccludeeParticle(ref boundingBox);
			}
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x000F3614 File Offset: 0x000F1814
		public void Update(Vector3 cameraPosition)
		{
			FXSystem fxsystem = this._gameInstance.Engine.FXSystem;
			fxsystem.Particles.UpdateAnimatedBlockParticles();
			fxsystem.Particles.UpdateProxies(cameraPosition, this.ProxyCheck);
			this.UpdateDebugInfo();
			bool flag = this._gameInstance.Input.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_F5, false);
			if (flag)
			{
				bool flag2 = !this._resettingParticles;
				if (flag2)
				{
					bool flag3 = this._gameInstance.Input.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT, false);
					if (flag3)
					{
						this._gameInstance.ParticleSystemStoreModule.ResetParticleSystems(false);
					}
					else
					{
						this._gameInstance.ParticleSystemStoreModule.ResetParticleSystems(true);
					}
				}
				this._resettingParticles = true;
			}
			else
			{
				this._resettingParticles = false;
			}
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x000F36D8 File Offset: 0x000F18D8
		public void UpdateVisibilityPrediction(int[] occludeesVisibility, int particleResultsOffset, int particleResultsCount, bool updateParticles)
		{
			FXSystem fxsystem = this._gameInstance.Engine.FXSystem;
			ParticleSystemProxy[] particleSystemProxies = fxsystem.Particles.ParticleSystemProxies;
			bool flag = !updateParticles;
			for (int i = 0; i < particleResultsCount; i++)
			{
				bool visibilityPrediction = flag || occludeesVisibility[particleResultsOffset + i] == 1;
				particleSystemProxies[i].VisibilityPrediction = visibilityPrediction;
			}
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x000F3738 File Offset: 0x000F1938
		public void GatherRenderableSpawners(Vector3 cameraPosition, BoundingFrustum viewFrustum)
		{
			FXSystem fxsystem = this._gameInstance.Engine.FXSystem;
			foreach (ParticleSystem particleSystem in fxsystem.Particles.ParticleSystems.Values)
			{
				BoundingSphere sphere = new BoundingSphere(particleSystem.Position, particleSystem.BoundingRadius);
				bool flag = !this.FrustumCheck || particleSystem.IsFirstPerson || viewFrustum.Intersects(sphere);
				float num = particleSystem.IsFirstPerson ? 0.01f : Vector3.DistanceSquared(cameraPosition, particleSystem.Position);
				bool flag2 = !this.DistanceCheck || num < particleSystem.CullDistanceSquared;
				bool flag3 = particleSystem.IsFirstPerson || particleSystem.Proxy.VisibilityPrediction;
				bool isVisible = particleSystem.IsExpiring || ((particleSystem.IsImportant || (flag2 && flag && flag3)) && particleSystem.Proxy.Visible);
				fxsystem.Particles.RegisterTask(particleSystem, isVisible, num);
			}
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x000F3864 File Offset: 0x000F1A64
		public void DespawnDebugSystem(int id)
		{
			ParticleSystemStoreModule.DebugParticleSystemProxy debugParticleSystemProxy;
			bool flag = this.DebugParticleSytemProxiesById.TryGetValue(id, out debugParticleSystemProxy);
			if (flag)
			{
				debugParticleSystemProxy.ParticleSystemProxy.Expire(false);
				this.DebugParticleSytemProxiesById.Remove(id);
			}
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x000F38A0 File Offset: 0x000F1AA0
		public string GetSystemsList()
		{
			string text = "";
			foreach (string str in this._systemSettingsById.Keys)
			{
				text = text + str + "\n";
			}
			return text;
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x000F3910 File Offset: 0x000F1B10
		public void GetSettingsStats(out int particleSystemSettingsCount, out int particleSettingsCount, out int keyframeArrayCount, out int keyframeArrayMaxSize, out int keyframeCount)
		{
			particleSystemSettingsCount = this._systemSettingsById.Count;
			particleSettingsCount = this._particlesByFile.Count;
			keyframeArrayCount = 0;
			keyframeArrayMaxSize = 0;
			keyframeCount = 0;
			foreach (ParticleSettings particleSettings in this._particlesByFile.Values)
			{
				keyframeArrayCount += ((particleSettings.ColorKeyFrameCount != 0) ? 1 : 0);
				keyframeArrayCount += ((particleSettings.OpacityKeyFrameCount != 0) ? 1 : 0);
				keyframeArrayCount += ((particleSettings.RotationKeyFrameCount != 0) ? 1 : 0);
				keyframeArrayCount += ((particleSettings.ScaleKeyFrameCount != 0) ? 1 : 0);
				keyframeArrayCount += ((particleSettings.TextureKeyFrameCount != 0) ? 1 : 0);
				keyframeArrayMaxSize = Math.Max(keyframeArrayMaxSize, (int)particleSettings.ColorKeyFrameCount);
				keyframeArrayMaxSize = Math.Max(keyframeArrayMaxSize, (int)particleSettings.OpacityKeyFrameCount);
				keyframeArrayMaxSize = Math.Max(keyframeArrayMaxSize, (int)particleSettings.RotationKeyFrameCount);
				keyframeArrayMaxSize = Math.Max(keyframeArrayMaxSize, (int)particleSettings.ScaleKeyFrameCount);
				keyframeArrayMaxSize = Math.Max(keyframeArrayMaxSize, (int)particleSettings.TextureKeyFrameCount);
				keyframeCount += (int)particleSettings.ColorKeyFrameCount;
				keyframeCount += (int)particleSettings.OpacityKeyFrameCount;
				keyframeCount += (int)particleSettings.RotationKeyFrameCount;
				keyframeCount += (int)particleSettings.ScaleKeyFrameCount;
				keyframeCount += (int)particleSettings.TextureKeyFrameCount;
			}
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x000F3A88 File Offset: 0x000F1C88
		public void AddDebug(ParticleSystem particleSystem)
		{
			this._gameInstance.Engine.FXSystem.Particles.AddParticleSystemDebug(particleSystem);
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x000F3AA8 File Offset: 0x000F1CA8
		public void TrySpawnDebugSystem(string systemId, Vector3 startPosition, bool useDebug, int quantity)
		{
			bool flag = quantity > 1;
			if (flag)
			{
				startPosition.X -= 2f * (float)Math.Floor((double)((float)(quantity / 10) * 0.5f));
				startPosition.Z -= 2f * (float)Math.Floor(5.0);
			}
			int nextDebugId = this._nextDebugId;
			int nextDebugId2 = this._nextDebugId;
			for (int i = 0; i < quantity; i++)
			{
				Vector3 position = startPosition;
				bool flag2 = quantity > 1;
				if (flag2)
				{
					position.X += (float)Math.Floor((double)((float)i / 10f)) * 2f;
					position.Z += (float)(2 * (i % 10));
				}
				ParticleSystemProxy particleSystemProxy;
				bool flag3 = this.TrySpawnSystem(systemId, out particleSystemProxy, false, true);
				if (flag3)
				{
					ParticleSystemStoreModule.DebugParticleSystemProxy debugParticleSystemProxy = new ParticleSystemStoreModule.DebugParticleSystemProxy();
					debugParticleSystemProxy.SystemId = systemId;
					debugParticleSystemProxy.UseDebug = useDebug;
					debugParticleSystemProxy.NeedDebugRefreshing = debugParticleSystemProxy.UseDebug;
					debugParticleSystemProxy.Position = position;
					debugParticleSystemProxy.ParticleSystemProxy = particleSystemProxy;
					particleSystemProxy.Position = debugParticleSystemProxy.Position;
					particleSystemProxy.Rotation = Quaternion.Identity;
					this.DebugParticleSytemProxiesById[this._nextDebugId] = debugParticleSystemProxy;
					nextDebugId2 = this._nextDebugId;
					this._nextDebugId++;
				}
				else
				{
					this._gameInstance.Chat.Log("Particle System (" + systemId + ") could not be created!");
				}
			}
			bool flag4 = quantity > 1;
			if (flag4)
			{
				this._gameInstance.Chat.Log(string.Format("Particle systems created! (Ids: {0} - {1})", nextDebugId, nextDebugId2));
			}
			else
			{
				this._gameInstance.Chat.Log(string.Format("Particle system created! (Id: {0})", nextDebugId));
			}
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x000F3C78 File Offset: 0x000F1E78
		public void UpdateDebugInfo()
		{
			foreach (ParticleSystemStoreModule.DebugParticleSystemProxy debugParticleSystemProxy in this.DebugParticleSytemProxiesById.Values)
			{
				bool flag = debugParticleSystemProxy.ParticleSystemProxy == null;
				if (!flag)
				{
					bool flag2 = debugParticleSystemProxy.UseDebug && debugParticleSystemProxy.ParticleSystemProxy.ParticleSystem != null && debugParticleSystemProxy.NeedDebugRefreshing;
					if (flag2)
					{
						this.AddDebug(debugParticleSystemProxy.ParticleSystemProxy.ParticleSystem);
					}
					debugParticleSystemProxy.NeedDebugRefreshing = (debugParticleSystemProxy.ParticleSystemProxy.ParticleSystem == null);
				}
			}
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x000F3D28 File Offset: 0x000F1F28
		public void ResetDebugParticleSystems()
		{
			foreach (ParticleSystemStoreModule.DebugParticleSystemProxy debugParticleSystemProxy in this.DebugParticleSytemProxiesById.Values)
			{
				bool flag = debugParticleSystemProxy.ParticleSystemProxy != null;
				if (flag)
				{
					debugParticleSystemProxy.ParticleSystemProxy.Expire(true);
					debugParticleSystemProxy.ParticleSystemProxy = null;
				}
				ParticleSystemProxy particleSystemProxy;
				bool flag2 = this.TrySpawnSystem(debugParticleSystemProxy.SystemId, out particleSystemProxy, false, true);
				if (flag2)
				{
					particleSystemProxy.Position = debugParticleSystemProxy.Position;
					particleSystemProxy.Rotation = Quaternion.Identity;
					debugParticleSystemProxy.ParticleSystemProxy = particleSystemProxy;
				}
			}
		}

		// Token: 0x040022C5 RID: 8901
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040022C6 RID: 8902
		public bool FrustumCheck = true;

		// Token: 0x040022C7 RID: 8903
		public bool DistanceCheck = true;

		// Token: 0x040022C8 RID: 8904
		public bool ProxyCheck = true;

		// Token: 0x040022C9 RID: 8905
		private Dictionary<string, ParticleSystemSettings> _systemSettingsById = new Dictionary<string, ParticleSystemSettings>();

		// Token: 0x040022CA RID: 8906
		private Dictionary<string, ParticleSpawnerSettings> _spawnerSettingsById = new Dictionary<string, ParticleSpawnerSettings>();

		// Token: 0x040022CB RID: 8907
		private Dictionary<string, ParticleSettings> _particlesByFile;

		// Token: 0x040022CC RID: 8908
		private bool _resettingParticles;

		// Token: 0x040022CD RID: 8909
		private static readonly HitDetection.RaycastOptions RaycastOptions = new HitDetection.RaycastOptions
		{
			IgnoreEmptyCollisionMaterial = true
		};

		// Token: 0x040022CE RID: 8910
		public readonly Dictionary<int, ParticleSystemStoreModule.DebugParticleSystemProxy> DebugParticleSytemProxiesById = new Dictionary<int, ParticleSystemStoreModule.DebugParticleSystemProxy>();

		// Token: 0x040022CF RID: 8911
		private int _nextDebugId = 0;

		// Token: 0x02000DD7 RID: 3543
		public class DebugParticleSystemProxy
		{
			// Token: 0x04004433 RID: 17459
			public string SystemId;

			// Token: 0x04004434 RID: 17460
			public Vector3 Position;

			// Token: 0x04004435 RID: 17461
			public bool UseDebug;

			// Token: 0x04004436 RID: 17462
			public ParticleSystemProxy ParticleSystemProxy;

			// Token: 0x04004437 RID: 17463
			public bool NeedDebugRefreshing;
		}
	}
}
