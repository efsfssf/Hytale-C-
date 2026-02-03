using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Entities;
using HytaleClient.Graphics;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Fonts;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.InGame.Modules.Entities
{
	// Token: 0x02000947 RID: 2375
	internal class EntityStoreModule : Module
	{
		// Token: 0x060049D8 RID: 18904 RVA: 0x00127154 File Offset: 0x00125354
		public void SetupModelsAndAnimations()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.SuspendBackgroundLoadingThread();
			this._models.Clear();
			this._animations.Clear();
			this._modelsAndAnimationsToLoad.Clear();
			foreach (KeyValuePair<string, string> keyValuePair in this._gameInstance.HashesByServerAssetPath)
			{
				bool flag = !EntityStoreModule.IsAssetPathCharacterRelated(keyValuePair.Key);
				if (!flag)
				{
					bool flag2 = keyValuePair.Key.EndsWith(".blockymodel") || keyValuePair.Key.EndsWith(".blockyanim");
					if (flag2)
					{
						this._modelsAndAnimationsToLoad.Enqueue(new Tuple<string, string>(keyValuePair.Key, keyValuePair.Value));
					}
				}
			}
			this.ResumeBackgroundLoadingThread();
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x00127240 File Offset: 0x00125440
		private void SuspendBackgroundLoadingThread()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			bool flag = this._thread == null;
			if (!flag)
			{
				this._threadCancellationTokenSource.Cancel();
				this._thread.Join();
				this._thread = null;
				this._threadCancellationTokenSource = null;
			}
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x00127290 File Offset: 0x00125490
		public void ResumeBackgroundLoadingThread()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._threadCancellationTokenSource = new CancellationTokenSource();
			this._threadCancellationToken = this._threadCancellationTokenSource.Token;
			this._thread = new Thread(new ThreadStart(this.BackgroundLoadingThreadStart))
			{
				Name = "EntityStoreModuleBackgroundLoading",
				IsBackground = true
			};
			this._thread.Start();
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x001272FC File Offset: 0x001254FC
		private void BackgroundLoadingThreadStart()
		{
			while (!this._threadCancellationToken.IsCancellationRequested && this._modelsAndAnimationsToLoad.Count > 0)
			{
				Tuple<string, string> tuple = this._modelsAndAnimationsToLoad.Dequeue();
				string item = tuple.Item1;
				string item2 = tuple.Item2;
				bool flag = item.EndsWith(".blockymodel");
				if (flag)
				{
					bool flag2 = this._models.ContainsKey(item2);
					if (!flag2)
					{
						BlockyModel blockyModel = new BlockyModel(BlockyModel.MaxNodeCount);
						try
						{
							BlockyModelInitializer.Parse(AssetManager.GetAssetUsingHash(item2, false), this.NodeNameManager, ref blockyModel);
						}
						catch (Exception value)
						{
							this._gameInstance.App.DevTools.Error("Failed to parse blocky model: " + item + ". See log for details.");
							EntityStoreModule.Logger.Error<Exception>(value);
							blockyModel = new BlockyModel(0);
						}
						this._models[item2] = blockyModel;
					}
				}
				else
				{
					bool flag3 = this._animations.ContainsKey(item2);
					if (!flag3)
					{
						BlockyAnimation blockyAnimation = new BlockyAnimation();
						try
						{
							BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(item2, false), this.NodeNameManager, ref blockyAnimation);
						}
						catch (Exception value2)
						{
							this._gameInstance.App.DevTools.Error("Failed to parse blocky animation: " + item + ". See log for details.");
							EntityStoreModule.Logger.Error<Exception>(value2);
							blockyAnimation = new BlockyAnimation();
						}
						this._animations[item2] = blockyAnimation;
					}
				}
			}
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x00127498 File Offset: 0x00125698
		public bool GetModel(string hash, out BlockyModel model)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			string assetLocalPathUsingHash = AssetManager.GetAssetLocalPathUsingHash(hash);
			try
			{
				model = this._models.GetOrAdd(hash, delegate(string x)
				{
					BlockyModel result = new BlockyModel(BlockyModel.MaxNodeCount);
					BlockyModelInitializer.Parse(AssetManager.GetAssetUsingHash(x, true), this.NodeNameManager, ref result);
					return result;
				});
			}
			catch (Exception value)
			{
				this._gameInstance.App.DevTools.Error("Failed to parse blocky model: " + assetLocalPathUsingHash + ". See log for details.");
				EntityStoreModule.Logger.Error<Exception>(value);
				model = null;
				return false;
			}
			return true;
		}

		// Token: 0x060049DD RID: 18909 RVA: 0x00127528 File Offset: 0x00125728
		public bool GetAnimation(string hash, out BlockyAnimation animation)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			string assetLocalPathUsingHash = AssetManager.GetAssetLocalPathUsingHash(hash);
			try
			{
				animation = this._animations.GetOrAdd(hash, delegate(string x)
				{
					BlockyAnimation result = new BlockyAnimation();
					BlockyAnimationInitializer.Parse(AssetManager.GetAssetUsingHash(x, true), this.NodeNameManager, ref result);
					return result;
				});
			}
			catch (Exception value)
			{
				this._gameInstance.App.DevTools.Error("Failed to parse blocky animation: " + assetLocalPathUsingHash + ". See log for details.");
				EntityStoreModule.Logger.Error<Exception>(value);
				animation = null;
				return false;
			}
			return true;
		}

		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x001275B8 File Offset: 0x001257B8
		public int PlayerEntityLocalId
		{
			get
			{
				return this._playerEntityLocalId;
			}
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x001275C0 File Offset: 0x001257C0
		public Entity[] GetAllEntities()
		{
			return this._entities;
		}

		// Token: 0x060049E0 RID: 18912 RVA: 0x001275C8 File Offset: 0x001257C8
		public int GetEntitiesCount()
		{
			return this._entitiesCount;
		}

		// Token: 0x060049E1 RID: 18913 RVA: 0x001275D0 File Offset: 0x001257D0
		public BoundingSphere[] GetBoundingVolumes()
		{
			return this._boundingVolumes;
		}

		// Token: 0x060049E2 RID: 18914 RVA: 0x001275D8 File Offset: 0x001257D8
		public Vector3[] GetOrientations()
		{
			return this._orientations;
		}

		// Token: 0x060049E3 RID: 18915 RVA: 0x001275E0 File Offset: 0x001257E0
		public AudioDevice.SoundObjectReference[] GetSoundObjectReferences()
		{
			return this._soundObjectReferences;
		}

		// Token: 0x170011E7 RID: 4583
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x001275E8 File Offset: 0x001257E8
		// (set) Token: 0x060049E5 RID: 18917 RVA: 0x001275F0 File Offset: 0x001257F0
		public Vector3[] ClosestEntityPositions { get; private set; } = new Vector3[20];

		// Token: 0x060049E6 RID: 18918 RVA: 0x001275FC File Offset: 0x001257FC
		public void ExtractClosestEntityPositions(SceneView cameraSceneView, bool insertFirstPersonCamera, Vector3 firstPersonCameraPosition)
		{
			int num = 0;
			if (insertFirstPersonCamera)
			{
				this.ClosestEntityPositions[0] = firstPersonCameraPosition - cameraSceneView.Position;
				num = 1;
			}
			int num2 = Math.Min(20, cameraSceneView.EntitiesCount);
			for (int i = num; i < num2; i++)
			{
				int sortedEntityId = cameraSceneView.GetSortedEntityId(i - num);
				this.ClosestEntityPositions[i] = this._boundingVolumes[sortedEntityId].Center - cameraSceneView.Position;
			}
			for (int j = num2; j < 20; j++)
			{
				this.ClosestEntityPositions[j] = new Vector3(1000f);
			}
		}

		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x060049E7 RID: 18919 RVA: 0x001276B3 File Offset: 0x001258B3
		// (set) Token: 0x060049E8 RID: 18920 RVA: 0x001276BB File Offset: 0x001258BB
		public Dictionary<string, Point> ImageLocations { get; private set; }

		// Token: 0x060049E9 RID: 18921 RVA: 0x001276C4 File Offset: 0x001258C4
		public EntityStoreModule(GameInstance gameInstance) : base(gameInstance)
		{
			this.TextureAtlas = new Texture(Texture.TextureTypes.Texture2D);
			this.TextureAtlas.CreateTexture2D(4096, 32, null, 5, GL.NEAREST_MIPMAP_NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
			this.NodeNameManager = NodeNameManager.Copy(gameInstance.App.CharacterPartStore.CharacterNodeNameManager);
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x00127874 File Offset: 0x00125A74
		protected override void DoDispose()
		{
			this.SuspendBackgroundLoadingThread();
			foreach (Entity entity in this._entitiesByNetworkId.Values)
			{
				entity.Dispose();
			}
			this.TextureAtlas.Dispose();
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x001278E4 File Offset: 0x00125AE4
		public void BeginFrame()
		{
			this.ResetFXUpdateTaskCounters();
			this.ProcessDeferredDespawn();
			this._needDeferredDespawn = false;
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x001278FC File Offset: 0x00125AFC
		public void RegisterEntity(Entity entity)
		{
			this._entitiesByNetworkId.Add(entity.NetworkId, entity);
			bool flag = entity.PredictionId != null;
			if (flag)
			{
				this._entitiesByPredictionId.Add(entity.PredictionId.Value, entity);
			}
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x00127948 File Offset: 0x00125B48
		public void UnregisterEntity(int networkId)
		{
			Entity entity = this._entitiesByNetworkId[networkId];
			this._entitiesByNetworkId.Remove(networkId);
			bool flag = entity.PredictionId != null;
			if (flag)
			{
				this._entitiesByPredictionId.Remove(entity.PredictionId.Value);
			}
			bool flag2 = networkId >= 0;
			if (!flag2)
			{
				bool flag3 = networkId >= this._nextLocalEntityId;
				if (flag3)
				{
					this._nextLocalEntityId = networkId + 1;
				}
			}
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x001279C0 File Offset: 0x00125BC0
		public void MapPrediction(Guid predictionId, Entity entity)
		{
			Entity entity2;
			bool flag = !this._entitiesByPredictionId.TryGetValue(predictionId, out entity2);
			if (!flag)
			{
				entity2.ServerEntity = entity;
				entity.BeenPredicted = true;
			}
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x001279F4 File Offset: 0x00125BF4
		public Entity GetEntity(int networkId)
		{
			Entity result;
			this._entitiesByNetworkId.TryGetValue(networkId, out result);
			return result;
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x00127A18 File Offset: 0x00125C18
		public int NextLocalEntityId()
		{
			int num = this._nextLocalEntityId - 1;
			this._nextLocalEntityId = num;
			int num2 = num;
			while (this._entitiesByNetworkId.ContainsKey(num2))
			{
				num = this._nextLocalEntityId - 1;
				this._nextLocalEntityId = num;
				num2 = num;
			}
			return num2;
		}

		// Token: 0x060049F1 RID: 18929 RVA: 0x00127A64 File Offset: 0x00125C64
		public bool Spawn(int networkId, out Entity entity)
		{
			bool flag = networkId == -1;
			if (flag)
			{
				networkId = this.NextLocalEntityId();
			}
			entity = this.GetEntity(networkId);
			bool flag2 = entity != null;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool flag3 = networkId == this._gameInstance.LocalPlayerNetworkId;
				if (flag3)
				{
					entity = new PlayerEntity(this._gameInstance, networkId);
					this._gameInstance.SetLocalPlayer(entity as PlayerEntity);
				}
				else
				{
					entity = new Entity(this._gameInstance, networkId);
				}
				this.RegisterEntity(entity);
				result = true;
			}
			return result;
		}

		// Token: 0x060049F2 RID: 18930 RVA: 0x00127AF0 File Offset: 0x00125CF0
		public List<Entity> GetEntitiesInBox(BoundingBox box)
		{
			List<Entity> list = new List<Entity>(32);
			for (int i = 0; i < this._entitiesCount; i++)
			{
				Entity entity = this._entities[i];
				bool flag = box.Contains(entity.Position) == ContainmentType.Contains;
				if (flag)
				{
					list.Add(entity);
				}
			}
			return list;
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x00127B4C File Offset: 0x00125D4C
		public List<Entity> GetEntitiesInSphere(Vector3 position, float radius)
		{
			List<Entity> list = new List<Entity>(32);
			float num = radius * radius;
			for (int i = 0; i < this._entitiesCount; i++)
			{
				Entity entity = this._entities[i];
				bool flag = this.Intersects(position, radius, entity.Position, entity.Hitbox);
				if (flag)
				{
					list.Add(entity);
				}
			}
			return list;
		}

		// Token: 0x060049F4 RID: 18932 RVA: 0x00127BB4 File Offset: 0x00125DB4
		public bool Intersects(Vector3 center, float radius, Vector3 pos, BoundingBox box)
		{
			box = new BoundingBox(box.Min, box.Max);
			box.Translate(pos);
			float num = Math.Max(box.Min.X, Math.Min(center.X, box.Max.X));
			float num2 = Math.Max(box.Min.Y, Math.Min(center.Y, box.Max.Y));
			float num3 = Math.Max(box.Min.Z, Math.Min(center.Z, box.Max.Z));
			double num4 = Math.Sqrt((double)((num - center.X) * (num - center.X) + (num2 - center.Y) * (num2 - center.Y) + (num3 - center.Z) * (num3 - center.Z)));
			return num4 < (double)radius;
		}

		// Token: 0x060049F5 RID: 18933 RVA: 0x00127CA4 File Offset: 0x00125EA4
		public void Despawn(int networkId)
		{
			bool needDeferredDespawn = this._needDeferredDespawn;
			if (needDeferredDespawn)
			{
				this._deferredDespawnIds.Add(networkId);
			}
			else
			{
				this.RemoveEntity(networkId);
			}
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x00127CD8 File Offset: 0x00125ED8
		public void DespawnAll()
		{
			foreach (int networkId in Enumerable.ToArray<int>(this._entitiesByNetworkId.Keys))
			{
				this.Despawn(networkId);
			}
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x00127D14 File Offset: 0x00125F14
		private void RemoveEntity(int networkId)
		{
			Entity entity = this.GetEntity(networkId);
			bool flag = entity != null;
			if (flag)
			{
				this.UnregisterEntity(networkId);
				entity.Dispose();
				this.UnregisterEntityLight(networkId);
			}
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x00127D4C File Offset: 0x00125F4C
		private void ProcessDeferredDespawn()
		{
			foreach (int networkId in this._deferredDespawnIds)
			{
				this.RemoveEntity(networkId);
			}
			this._deferredDespawnIds.Clear();
		}

		// Token: 0x060049F9 RID: 18937 RVA: 0x00127DB0 File Offset: 0x00125FB0
		public void UpdateEffects(int entityNetworkId, EntityEffectUpdate[] networkEffectIndexes)
		{
			Entity entity = this.GetEntity(entityNetworkId);
			if (entity != null)
			{
				entity.UpdateEffectsFromServerPacket(networkEffectIndexes);
			}
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x00127DD4 File Offset: 0x00125FD4
		public void SetEntityLight(int entityNetworkId, Vector3? lightColor)
		{
			bool flag = lightColor != null;
			if (flag)
			{
				this.RegisterEntityLight(entityNetworkId, lightColor.Value);
			}
			else
			{
				this.UnregisterEntityLight(entityNetworkId);
			}
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x00127E0C File Offset: 0x0012600C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void RegisterEntityLight(int entityNetworkId, Vector3 lightColor)
		{
			int num;
			bool flag = this.TryGetEntityLightIndex(entityNetworkId, out num);
			if (flag)
			{
				this.EntityLights[num].LightData.Color = lightColor;
			}
			else
			{
				bool flag2 = this.EntityLightCount == this.EntityLights.Length;
				if (flag2)
				{
					int newSize = this.EntityLightCount + 100;
					Array.Resize<EntityStoreModule.EntityLight>(ref this.EntityLights, newSize);
					Array.Resize<ushort>(ref this.SortedEntityLightIds, newSize);
					Array.Resize<float>(ref this.SortedEntityLightCameraSquaredDistances, newSize);
				}
				this.EntityLights[this.EntityLightCount] = new EntityStoreModule.EntityLight(entityNetworkId, lightColor);
				this.EntityLightCount++;
			}
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x00127EB4 File Offset: 0x001260B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void UnregisterEntityLight(int entityNetworkId)
		{
			int num;
			bool flag = this.TryGetEntityLightIndex(entityNetworkId, out num);
			if (flag)
			{
				this.EntityLights[num] = this.EntityLights[this.EntityLightCount - 1];
				this.EntityLightCount--;
			}
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x00127F00 File Offset: 0x00126100
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryGetEntityLightIndex(int entityNetworkId, out int entityLightIndex)
		{
			entityLightIndex = -1;
			for (int i = 0; i < this.EntityLightCount; i++)
			{
				bool flag = this.EntityLights[i].EntityNetworkId == entityNetworkId;
				if (flag)
				{
					entityLightIndex = i;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060049FE RID: 18942 RVA: 0x00127F50 File Offset: 0x00126150
		public void PrepareAtlas(out Dictionary<string, Point> upcomingImageLocations, out byte[][] upcomingAtlasPixelsPerLevel, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingImageLocations = new Dictionary<string, Point>();
			Dictionary<string, PacketHandler.TextureInfo> dictionary = new Dictionary<string, PacketHandler.TextureInfo>();
			foreach (KeyValuePair<string, string> keyValuePair in this._gameInstance.HashesByServerAssetPath)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				string key = keyValuePair.Key;
				bool flag = !key.EndsWith(".png") || key.EndsWith("-Icon.png");
				if (!flag)
				{
					bool flag2 = !EntityStoreModule.IsAssetPathCharacterRelated(key);
					if (!flag2)
					{
						bool flag3 = this._gameInstance.App.CharacterPartStore.ImageLocations.ContainsKey(key);
						if (!flag3)
						{
							string value = keyValuePair.Value;
							PacketHandler.TextureInfo textureInfo;
							bool flag4 = !dictionary.TryGetValue(value, out textureInfo);
							if (flag4)
							{
								textureInfo = new PacketHandler.TextureInfo
								{
									Checksum = value
								};
								string assetLocalPathUsingHash = AssetManager.GetAssetLocalPathUsingHash(value);
								bool flag5 = Image.TryGetPngDimensions(assetLocalPathUsingHash, out textureInfo.Width, out textureInfo.Height);
								if (flag5)
								{
									dictionary[value] = textureInfo;
									bool flag6 = textureInfo.Width % 32 != 0 || textureInfo.Height % 32 != 0 || textureInfo.Width < 32 || textureInfo.Height < 32;
									if (flag6)
									{
										this._gameInstance.App.DevTools.Warn(string.Format("Texture width/height must be a multiple of 32 and at least 32x32: {0} ({1}x{2})", key, textureInfo.Width, textureInfo.Height));
									}
								}
								else
								{
									this._gameInstance.App.DevTools.Error(string.Concat(new string[]
									{
										"Failed to get PNG dimensions for: ",
										key,
										", ",
										assetLocalPathUsingHash,
										" (",
										value,
										")"
									}));
								}
							}
						}
					}
				}
			}
			List<PacketHandler.TextureInfo> list = new List<PacketHandler.TextureInfo>(dictionary.Values);
			list.Sort((PacketHandler.TextureInfo a, PacketHandler.TextureInfo b) => b.Height.CompareTo(a.Height));
			Point zero = Point.Zero;
			int num = 0;
			int num2 = 512;
			foreach (PacketHandler.TextureInfo textureInfo2 in list)
			{
				bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested2)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				bool flag7 = zero.X + textureInfo2.Width > this.TextureAtlas.Width;
				if (flag7)
				{
					zero.X = 0;
					zero.Y = num;
				}
				while (zero.Y + textureInfo2.Height > num2)
				{
					num2 <<= 1;
				}
				upcomingImageLocations.Add(textureInfo2.Checksum, zero);
				num = Math.Max(num, zero.Y + textureInfo2.Height);
				zero.X += textureInfo2.Width;
			}
			byte[] array = new byte[this.TextureAtlas.Width * num2 * 4];
			zero = Point.Zero;
			foreach (PacketHandler.TextureInfo textureInfo3 in list)
			{
				bool isCancellationRequested3 = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested3)
				{
					upcomingAtlasPixelsPerLevel = null;
					return;
				}
				try
				{
					Image image = new Image(AssetManager.GetAssetUsingHash(textureInfo3.Checksum, false));
					for (int i = 0; i < image.Height; i++)
					{
						Point point = upcomingImageLocations[textureInfo3.Checksum];
						int dstOffset = ((point.Y + i) * this.TextureAtlas.Width + point.X) * 4;
						Buffer.BlockCopy(image.Pixels, i * image.Width * 4, array, dstOffset, image.Width * 4);
					}
				}
				catch (Exception exception)
				{
					EntityStoreModule.Logger.Error(exception, "Failed to load model texture: " + AssetManager.GetAssetLocalPathUsingHash(textureInfo3.Checksum));
				}
			}
			upcomingAtlasPixelsPerLevel = Texture.BuildMipmapPixels(array, this.TextureAtlas.Width, this.TextureAtlas.MipmapLevelCount);
		}

		// Token: 0x060049FF RID: 18943 RVA: 0x00128420 File Offset: 0x00126620
		public void CreateAtlasTexture(Dictionary<string, Point> imageLocations, byte[][] atlasPixelsPerLevel)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.ImageLocations = imageLocations;
			this.TextureAtlas.UpdateTexture2DMipMaps(atlasPixelsPerLevel);
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x00128444 File Offset: 0x00126644
		public static bool IsAssetPathCharacterRelated(string assetPath)
		{
			return assetPath.StartsWith("Characters/") || assetPath.StartsWith("NPC/") || assetPath.StartsWith("NPCs/") || assetPath.StartsWith("Items/") || assetPath.StartsWith("Consumable/") || assetPath.StartsWith("Resources/") || assetPath.StartsWith("VFX/") || assetPath.StartsWith("Trailer/");
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x001284C0 File Offset: 0x001266C0
		public void RebuildRenderers(bool itemOnly)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			for (int i = 0; i < this._entitiesCount; i++)
			{
				this._entities[i].RebuildRenderers(itemOnly);
			}
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x00128500 File Offset: 0x00126700
		public void ResetMovementParticleSystems()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			for (int i = 0; i < this._entitiesCount; i++)
			{
				this._entities[i].ResetMovementParticleSystems();
			}
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x0012853C File Offset: 0x0012673C
		public void PrepareEntityEffects(EntityEffect[] entityEffects, out EntityEffect[] preparedEntityEffects)
		{
			preparedEntityEffects = new EntityEffect[entityEffects.Length];
			for (int i = 0; i < entityEffects.Length; i++)
			{
				preparedEntityEffects[i] = new EntityEffect(entityEffects[i]);
			}
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x00128574 File Offset: 0x00126774
		public void SetupEntityEffects(EntityEffect[] entityEffects)
		{
			this.EntityEffects = entityEffects;
			this.EntityEffectIndicesByIds.Clear();
			for (int i = 0; i < entityEffects.Length; i++)
			{
				this.EntityEffectIndicesByIds[entityEffects[i].Id] = i;
			}
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x001285C0 File Offset: 0x001267C0
		public void PrepareModelVFXs(ModelVFX[] modelVFXs, out ModelVFX[] preparedModelVFXs)
		{
			preparedModelVFXs = new ModelVFX[modelVFXs.Length];
			for (int i = 0; i < modelVFXs.Length; i++)
			{
				preparedModelVFXs[i] = new ModelVFX(modelVFXs[i]);
			}
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x001285F8 File Offset: 0x001267F8
		public void SetupModelVFXs(ModelVFX[] modelVFXs)
		{
			this.ModelVFXs = modelVFXs;
			this.ModelVFXByIds.Clear();
			for (int i = 0; i < modelVFXs.Length; i++)
			{
				this.ModelVFXByIds[modelVFXs[i].Id] = i;
			}
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x00128644 File Offset: 0x00126844
		public void PrepareEntities()
		{
			this._entitiesCount = this._entitiesByNetworkId.Count;
			ArrayUtils.GrowArrayIfNecessary<Entity>(ref this._entities, this._entitiesCount, 500);
			int i = 0;
			foreach (Entity entity in this._entitiesByNetworkId.Values)
			{
				this._entities[i] = entity;
				i++;
			}
			this._playerEntityLocalId = -1;
			for (i = 0; i < this._entitiesCount; i++)
			{
				Entity entity2 = this._entities[i];
				bool flag = entity2.NetworkId == this._gameInstance.LocalPlayerNetworkId;
				if (flag)
				{
					Entity entity3 = this._entities[i];
					this._entities[i] = this._entities[0];
					this._entities[0] = entity3;
					this._playerEntityLocalId = 0;
					break;
				}
			}
			ArrayUtils.GrowArrayIfNecessary<BoundingSphere>(ref this._boundingVolumes, this._entitiesCount, 500);
			ArrayUtils.GrowArrayIfNecessary<float>(ref this._distancesToCamera, this._entitiesCount, 500);
			ArrayUtils.GrowArrayIfNecessary<Vector3>(ref this._orientations, this._entitiesCount, 500);
			ArrayUtils.GrowArrayIfNecessary<AudioDevice.SoundObjectReference>(ref this._soundObjectReferences, this._entitiesCount, 500);
			this._needDeferredDespawn = true;
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x001287A4 File Offset: 0x001269A4
		public void PrepareLights(Vector3 cameraPosition)
		{
			SceneRenderer sceneRenderer = this._gameInstance.SceneRenderer;
			int entityLightCount = this.EntityLightCount;
			bool flag = entityLightCount > 0;
			if (flag)
			{
				ushort num = 0;
				while ((int)num < entityLightCount)
				{
					ref EntityStoreModule.EntityLight ptr = ref this.EntityLights[(int)num];
					Vector3 position = this.GetEntity(ptr.EntityNetworkId).Position;
					ptr.LightData.Sphere.Center = position;
					float radius = LightData.ComputeRadiusFromColor(ptr.LightData.Color);
					ptr.LightData.Sphere.Radius = radius;
					this.SortedEntityLightIds[(int)num] = num;
					this.SortedEntityLightCameraSquaredDistances[(int)num] = Vector3.DistanceSquared(position, cameraPosition);
					num += 1;
				}
				Array.Sort<float, ushort>(this.SortedEntityLightCameraSquaredDistances, this.SortedEntityLightIds, 0, entityLightCount);
				sceneRenderer.PrepareForIncomingOccludeeLight(entityLightCount);
				for (int i = 0; i < entityLightCount; i++)
				{
					ushort num2 = this.SortedEntityLightIds[i];
					ref BoundingSphere ptr2 = ref this.EntityLights[(int)num2].LightData.Sphere;
					Vector3 value = ptr2.Center - cameraPosition;
					float value2 = ptr2.Radius * 0.8f;
					BoundingBox boundingBox;
					boundingBox.Min = value - new Vector3(value2);
					boundingBox.Max = value + new Vector3(value2);
					sceneRenderer.RegisterOccludeeLight(ref boundingBox);
				}
			}
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x00128918 File Offset: 0x00126B18
		public void GatherLights(ref BoundingFrustum viewFrustum, bool useOcclusionCullingForLights, int maxLightCount, ref LightData[] visibleLightData, out int visibleLightCount)
		{
			int num = 0;
			int num2 = Math.Min(this.EntityLightCount, maxLightCount);
			for (int i = 0; i < num2; i++)
			{
				ushort num3 = this.SortedEntityLightIds[i];
				ref EntityStoreModule.EntityLight ptr = ref this.EntityLights[(int)num3];
				bool flag = viewFrustum.Contains(ptr.LightData.Sphere) > ContainmentType.Disjoint;
				bool flag2 = !useOcclusionCullingForLights || ptr.VisibilityPrediction;
				bool flag3;
				if (flag && flag2)
				{
					Entity entity = this.GetEntity(ptr.EntityNetworkId);
					flag3 = (entity != null && entity.ShouldRender);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				if (flag4)
				{
					visibleLightData[num] = ptr.LightData;
					num++;
				}
			}
			visibleLightCount = num;
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x001289D0 File Offset: 0x00126BD0
		public void PreUpdate(float deltaTime)
		{
			Vector3 position = this._gameInstance.CameraModule.Controller.Position;
			SceneRenderer sceneRenderer = this._gameInstance.SceneRenderer;
			sceneRenderer.PrepareForIncomingOccludeeEntity(this._entitiesCount);
			int serverUpdatesPerSecond = this._gameInstance.ServerUpdatesPerSecond;
			for (int i = this._queuedSoundEvents.Count - 1; i >= 0; i--)
			{
				EntityStoreModule.QueuedSoundEvent queuedSoundEvent = this._queuedSoundEvents[i];
				Entity entity = this.GetEntity(queuedSoundEvent.NetworkId);
				bool flag;
				if (entity == null)
				{
					float time = queuedSoundEvent.Time;
					queuedSoundEvent.Time = time - 1f;
					flag = (time <= 0f);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				if (flag2)
				{
					bool flag3 = entity != null && entity.ShouldRender && !entity.ActiveSounds.Contains(queuedSoundEvent.EventIndex);
					if (flag3)
					{
						entity.ActiveSounds.Add(queuedSoundEvent.EventIndex);
						this._gameInstance.AudioModule.PlaySoundEvent(queuedSoundEvent.EventIndex, entity.SoundObjectReference, ref entity.SoundEventReference);
					}
					this._queuedSoundEvents.RemoveAt(i);
				}
				else
				{
					this._queuedSoundEvents[i] = queuedSoundEvent;
				}
			}
			for (int j = 0; j < this._entitiesCount; j++)
			{
				Entity entity2 = this._entities[j];
				bool flag4 = j != this._playerEntityLocalId && j != this.MountEntityLocalId;
				if (flag4)
				{
					entity2.UpdatePosition(deltaTime, serverUpdatesPerSecond);
				}
				this._distancesToCamera[j] = Vector3.Distance(entity2.RenderPosition, position);
				float val = Math.Max(Math.Abs(entity2.Hitbox.Min.X), entity2.Hitbox.Max.X);
				float val2 = Math.Max(Math.Abs(entity2.Hitbox.Min.Z), entity2.Hitbox.Max.Z);
				float num = Math.Max(val, val2);
				float num2 = (entity2.Hitbox.Max.Y - entity2.Hitbox.Min.Y) * 0.5f;
				float num3 = num2 * 2.5f;
				num3 = Math.Max(num, num3);
				this._boundingVolumes[j].Radius = num3;
				this._boundingVolumes[j].Center = entity2.Position + new Vector3(0f, num2, 0f);
				Vector3 vector = entity2.Position - position;
				BoundingBox boundingBox;
				boundingBox.Min.X = vector.X - num;
				boundingBox.Min.Y = vector.Y - entity2.Hitbox.Min.Y;
				boundingBox.Min.Z = vector.Z - num;
				boundingBox.Max.X = vector.X + num;
				boundingBox.Max.Y = vector.Y + entity2.Hitbox.Max.Y;
				boundingBox.Max.Z = vector.Z + num;
				sceneRenderer.RegisterOccludeeEntity(ref boundingBox);
			}
			bool flag5 = this._playerEntityLocalId != -1;
			if (flag5)
			{
				this.UpdateLocalPlayerEntity(deltaTime);
			}
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x00128D48 File Offset: 0x00126F48
		public void Update(float deltaTime)
		{
			bool flag = this._entitiesCount > 0;
			if (flag)
			{
				this.UpdateLocalEntities(deltaTime, (uint)(this._playerEntityLocalId + 1), (uint)(this._entitiesCount - 1));
			}
		}

		// Token: 0x06004A0C RID: 18956 RVA: 0x00128D80 File Offset: 0x00126F80
		private void UpdateLocalEntities(float deltaTime, uint startLocalId, uint endLocalId)
		{
			uint frameCounter = this._gameInstance.SceneRenderer.Data.FrameCounter;
			bool logicalLoDUpdate = this.CurrentSetup.LogicalLoDUpdate;
			Vector3 position = this._gameInstance.CameraModule.Controller.Position;
			float num = this.CurrentSetup.DistanceToCameraBeforeRotation * this.CurrentSetup.DistanceToCameraBeforeRotation;
			for (uint num2 = startLocalId; num2 <= endLocalId; num2 += 1U)
			{
				Entity entity = this._entities[(int)num2];
				ref BoundingSphere ptr = ref this._boundingVolumes[(int)num2];
				float distanceToCamera = this._distancesToCamera[(int)num2];
				int lod = this.ComputeLevelOfDetail(distanceToCamera, ptr.Radius);
				bool flag = logicalLoDUpdate && !this.ShouldUpdateBasedOnLodValue(lod, entity.LastLogicUpdateFrameId, frameCounter);
				bool flag2 = logicalLoDUpdate && !flag;
				if (flag2)
				{
					entity.LastLogicUpdateFrameId = frameCounter;
				}
				entity.UpdateWithoutPosition(deltaTime, distanceToCamera, flag);
				this._soundObjectReferences[(int)num2] = entity.SoundObjectReference;
				this._orientations[(int)num2] = entity.BodyOrientation;
				bool flag3 = entity.Type == Entity.EntityType.Character;
				if (flag3)
				{
					float num3 = Vector3.DistanceSquared(entity.Position, position);
					bool flag4 = num3 < num;
					if (flag4)
					{
						Vector3 bodyOrientation = entity.BodyOrientation;
						bodyOrientation.Yaw -= 3.1415927f;
						Quaternion.CreateFromYawPitchRoll(bodyOrientation.Yaw, -bodyOrientation.Pitch, -bodyOrientation.Roll, out entity.RenderOrientation);
						Vector3 lookOrientation = entity.LookOrientation;
						lookOrientation.Yaw -= 3.1415927f;
						Quaternion quaternion;
						Quaternion.CreateFromYawPitchRoll(lookOrientation.Yaw, -lookOrientation.Pitch, -lookOrientation.Roll, out quaternion);
						entity.ModelRenderer.SetCameraOrientation(Quaternion.Inverse(entity.RenderOrientation) * quaternion);
					}
					entity.RenderPosition = entity.Position;
				}
				else
				{
					bool flag5 = entity.Type == Entity.EntityType.Item;
					if (flag5)
					{
						float num4 = 0f;
						float num5 = 0f;
						bool flag6 = !entity.IsUsable() && !entity.IsLocalEntity && entity.ItemBase.DroppedItemAnimation == null;
						if (flag6)
						{
							num4 = ((float)Math.Cos((double)(entity.ItemAnimationTime * 2.5f)) + 1f) * 0.1f;
							num5 = MathHelper.WrapAngle(entity.ItemAnimationTime * 2f);
						}
						Vector3 bodyOrientation2 = entity.BodyOrientation;
						bodyOrientation2.Yaw -= -3.1415927f + num5;
						Quaternion.CreateFromYawPitchRoll(bodyOrientation2.Yaw, -bodyOrientation2.Pitch, -bodyOrientation2.Roll, out entity.RenderOrientation);
						entity.RenderPosition = entity.Position;
						Entity entity2 = entity;
						entity2.RenderPosition.Y = entity2.RenderPosition.Y + num4;
					}
					else
					{
						bool flag7 = entity.Type == Entity.EntityType.Block;
						if (flag7)
						{
							Vector3 lookOrientation2 = entity.LookOrientation;
							lookOrientation2.Yaw -= 3.1415927f;
							Quaternion.CreateFromYawPitchRoll(lookOrientation2.Yaw, -lookOrientation2.Pitch, -lookOrientation2.Roll, out entity.RenderOrientation);
							entity.RenderPosition = entity.Position;
						}
					}
				}
			}
		}

		// Token: 0x06004A0D RID: 18957 RVA: 0x001290D0 File Offset: 0x001272D0
		private void UpdateLocalPlayerEntity(float deltaTime)
		{
			uint frameCounter = this._gameInstance.SceneRenderer.Data.FrameCounter;
			bool logicalLoDUpdate = this.CurrentSetup.LogicalLoDUpdate;
			Vector3 position = this._gameInstance.CameraModule.Controller.Position;
			uint playerEntityLocalId = (uint)this._playerEntityLocalId;
			Entity entity = this._entities[(int)playerEntityLocalId];
			ref BoundingSphere ptr = ref this._boundingVolumes[(int)playerEntityLocalId];
			float distanceToCamera = this._distancesToCamera[(int)playerEntityLocalId];
			int lod = this.ComputeLevelOfDetail(distanceToCamera, ptr.Radius);
			bool flag = logicalLoDUpdate && !this.ShouldUpdateBasedOnLodValue(lod, entity.LastLogicUpdateFrameId, frameCounter);
			bool flag2 = logicalLoDUpdate && !flag;
			if (flag2)
			{
				entity.LastLogicUpdateFrameId = frameCounter;
			}
			entity.UpdateWithoutPosition(deltaTime, distanceToCamera, flag);
			this._soundObjectReferences[(int)playerEntityLocalId] = entity.SoundObjectReference;
			this._orientations[(int)playerEntityLocalId] = entity.BodyOrientation;
			bool isFirstPerson = this._gameInstance.CameraModule.Controller.IsFirstPerson;
			if (!isFirstPerson)
			{
				float num = Vector3.DistanceSquared(entity.Position, position);
				bool flag3 = num < 4096f;
				if (flag3)
				{
					Vector3 bodyOrientation = entity.BodyOrientation;
					bodyOrientation.Yaw -= 3.1415927f;
					Quaternion.CreateFromYawPitchRoll(bodyOrientation.Yaw, -bodyOrientation.Pitch, -bodyOrientation.Roll, out entity.RenderOrientation);
					Vector3 lookOrientation = entity.LookOrientation;
					lookOrientation.Yaw -= 3.1415927f;
					Quaternion quaternion;
					Quaternion.CreateFromYawPitchRoll(lookOrientation.Yaw, -lookOrientation.Pitch, -lookOrientation.Roll, out quaternion);
					entity.ModelRenderer.SetCameraOrientation(Quaternion.Inverse(entity.RenderOrientation) * quaternion);
				}
			}
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x00129298 File Offset: 0x00127498
		public void ProcessFrustumCulling(SceneView cameraSceneView, SceneView sunSceneView)
		{
			bool flag = sunSceneView != null;
			if (flag)
			{
				ArrayUtils.GrowArrayIfNecessary<bool>(ref cameraSceneView.EntitiesFrustumCullingResults, this._entitiesCount, 500);
				ArrayUtils.GrowArrayIfNecessary<bool>(ref sunSceneView.EntitiesFrustumCullingResults, this._entitiesCount, 500);
				bool useKDopForCulling = sunSceneView.UseKDopForCulling;
				if (useKDopForCulling)
				{
					for (int i = 0; i < this._entitiesCount; i++)
					{
						cameraSceneView.Frustum.Intersects(ref this._boundingVolumes[i], out cameraSceneView.EntitiesFrustumCullingResults[i]);
						BoundingSphere volume = this._boundingVolumes[i];
						volume.Center -= cameraSceneView.Position;
						sunSceneView.EntitiesFrustumCullingResults[i] = sunSceneView.KDopFrustum.Intersects(volume);
					}
				}
				else
				{
					for (int j = 0; j < this._entitiesCount; j++)
					{
						cameraSceneView.Frustum.Intersects(ref this._boundingVolumes[j], out cameraSceneView.EntitiesFrustumCullingResults[j]);
						BoundingSphere boundingSphere = this._boundingVolumes[j];
						boundingSphere.Center -= cameraSceneView.Position;
						sunSceneView.Frustum.Intersects(ref boundingSphere, out sunSceneView.EntitiesFrustumCullingResults[j]);
					}
				}
			}
			else
			{
				ArrayUtils.GrowArrayIfNecessary<bool>(ref cameraSceneView.EntitiesFrustumCullingResults, this._entitiesCount, 500);
				for (int k = 0; k < this._entitiesCount; k++)
				{
					cameraSceneView.Frustum.Intersects(ref this._boundingVolumes[k], out cameraSceneView.EntitiesFrustumCullingResults[k]);
				}
			}
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x00129460 File Offset: 0x00127660
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int ComputeLevelOfDetail(float distanceToCamera, float boundingRadius)
		{
			float num = MathHelper.Clamp(2.6f / boundingRadius, 0.1f, 2f);
			float num2 = (distanceToCamera - boundingRadius) * num;
			bool flag = num2 < 48f;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = num2 < 96f;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					bool flag3 = num2 < 160f;
					if (flag3)
					{
						result = 2;
					}
					else
					{
						result = 3;
					}
				}
			}
			return result;
		}

		// Token: 0x06004A10 RID: 18960 RVA: 0x001294C8 File Offset: 0x001276C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool ShouldUpdateBasedOnLodValue(int lod, uint lastUpdateFrameId, uint currentFrameId)
		{
			int num = 1 << lod;
			return (lod != 3 && (ulong)(currentFrameId - lastUpdateFrameId) % (ulong)((long)num) == 0UL) || currentFrameId < lastUpdateFrameId;
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x00129500 File Offset: 0x00127700
		public void GatherRenderableEntities(SceneView cameraSceneView, SceneView sunSceneView, Vector3 sunLightDirection, bool useAnimLOD, bool cullUndergroundShadowCasters, bool cullSmallShadowCasters)
		{
			AnimationSystem animationSystem = this._gameInstance.Engine.AnimationSystem;
			int num = 0;
			int num2 = 0;
			cameraSceneView.PrepareForIncomingEntities(this._entitiesCount);
			bool flag = sunSceneView != null;
			if (flag)
			{
				sunSceneView.PrepareForIncomingEntities(this._entitiesCount);
			}
			int num3 = 0;
			bool flag2 = this._playerEntityLocalId != -1;
			if (flag2)
			{
				int playerEntityLocalId = this._playerEntityLocalId;
				bool flag3 = cameraSceneView.EntitiesFrustumCullingResults[playerEntityLocalId];
				bool flag4 = sunSceneView != null && sunSceneView.EntitiesFrustumCullingResults[playerEntityLocalId];
				bool flag5 = flag3 || flag4;
				bool flag6 = flag5 && !this._gameInstance.LocalPlayer.FirstPersonViewNeedsDrawing();
				if (flag6)
				{
					Entity entity = this._entities[playerEntityLocalId];
					bool flag7 = entity.ModelRenderer != null && entity.ShouldRender;
					if (flag7)
					{
						bool flag8 = entity.HasFX();
						for (int i = 0; i < entity.EntityItems.Count; i++)
						{
							flag8 = (flag8 || entity.EntityItems[i].HasFX());
						}
						bool flag9 = flag3 && flag8;
						if (flag9)
						{
							ArrayUtils.GrowArrayIfNecessary<EntityStoreModule.FXUpdateTask>(ref this._fxUpdateTasks, this._fxUpdateTaskCount + 1, 100);
							this._fxUpdateTasks[this._fxUpdateTaskCount].Entity = entity;
							this._fxUpdateTaskCount++;
						}
						bool flag10 = !this._gameInstance.CameraModule.Controller.IsFirstPerson;
						if (flag10)
						{
							bool flag11 = flag3;
							if (flag11)
							{
								cameraSceneView.RegisterEntity(playerEntityLocalId, entity.Position);
								num += 1 + entity.EntityEffects.Length + entity.EntityItems.Count;
							}
							bool flag12 = flag4;
							if (flag12)
							{
								sunSceneView.RegisterEntity(playerEntityLocalId, entity.Position);
								num2 += 1 + entity.EntityItems.Count;
							}
						}
					}
				}
				num3 = 1;
			}
			uint frameCounter = this._gameInstance.FrameCounter;
			for (int j = num3; j < this._entitiesCount; j++)
			{
				bool flag13 = cameraSceneView.EntitiesFrustumCullingResults[j];
				bool flag14 = sunSceneView != null && sunSceneView.EntitiesFrustumCullingResults[j];
				bool flag15 = flag13 || flag14;
				Entity entity2 = this._entities[j];
				bool flag16 = !this._gameInstance.RenderPlayers && entity2.PlayerSkin != null;
				if (!flag16)
				{
					ref BoundingSphere ptr = ref this._boundingVolumes[j];
					float distanceToCamera = this._distancesToCamera[j];
					int num4 = this.ComputeLevelOfDetail(distanceToCamera, ptr.Radius);
					bool flag17 = useAnimLOD && !this.ShouldUpdateBasedOnLodValue(num4, entity2.LastAnimationUpdateFrameId, frameCounter);
					bool flag18 = useAnimLOD && !flag17;
					if (flag18)
					{
						entity2.LastAnimationUpdateFrameId = frameCounter;
					}
					bool flag19 = entity2.HasFX();
					animationSystem.PrepareForIncomingTasks(1 + entity2.EntityItems.Count + entity2.EntityEffects.Length);
					for (int k = 0; k < entity2.EntityItems.Count; k++)
					{
						animationSystem.RegisterAnimationTask(entity2.EntityItems[k].ModelRenderer, flag17);
						flag19 = (flag19 || entity2.EntityItems[k].HasFX());
					}
					bool flag20 = flag19;
					if (flag20)
					{
						ArrayUtils.GrowArrayIfNecessary<EntityStoreModule.FXUpdateTask>(ref this._fxUpdateTasks, this._fxUpdateTaskCount + 1, 100);
						this._fxUpdateTasks[this._fxUpdateTaskCount].Entity = entity2;
						this._fxUpdateTaskCount++;
					}
					bool flag21 = !flag15;
					if (!flag21)
					{
						bool flag22 = entity2.ModelRenderer == null || !entity2.ShouldRender;
						if (!flag22)
						{
							Vector3 position = entity2.Position;
							int chunkX = (int)Math.Floor((double)position.X) >> 5;
							int chunkY = (int)Math.Floor((double)position.Y) >> 5;
							int chunkZ = (int)Math.Floor((double)position.Z) >> 5;
							bool flag23 = !this._gameInstance.MapModule.IsChunkReadyForDraw(chunkX, chunkY, chunkZ);
							if (!flag23)
							{
								Vector3 vector;
								bool flag24 = CascadedShadowMapping.ComputeIntersection(new Plane(Vector3.Up, 0f), ptr.Center - cameraSceneView.Position, sunLightDirection, out vector);
								float num5 = vector.Length() - ptr.Radius;
								bool flag25 = flag13 || (flag24 && num5 > 32f);
								flag17 = (flag17 || (flag25 && !entity2.VisibilityPrediction));
								animationSystem.RegisterAnimationTask(entity2.ModelRenderer, flag17);
								bool flag26 = cullUndergroundShadowCasters && entity2.BlockLightColor.W * 15f <= 2f;
								bool flag27 = cullSmallShadowCasters && num4 >= 2;
								flag14 = (flag14 && !flag26 && !flag27);
								for (int l = 0; l < entity2.EntityEffects.Length; l++)
								{
									ref Entity.UniqueEntityEffect ptr2 = ref entity2.EntityEffects[l];
									bool flag28 = ptr2.ModelRenderer != null;
									if (flag28)
									{
										animationSystem.RegisterAnimationTask(ptr2.ModelRenderer, flag17);
									}
								}
								bool flag29 = flag13;
								if (flag29)
								{
									cameraSceneView.RegisterEntity(j, entity2.Position);
									num += 1 + entity2.EntityEffects.Length + entity2.EntityItems.Count;
								}
								bool flag30 = flag14;
								if (flag30)
								{
									sunSceneView.RegisterEntity(j, entity2.Position);
									num2 += 1 + entity2.EntityItems.Count;
								}
							}
						}
					}
				}
			}
			cameraSceneView.IncomingEntityDrawTaskCount = num;
			bool flag31 = sunSceneView != null;
			if (flag31)
			{
				sunSceneView.IncomingEntityDrawTaskCount = num2;
			}
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x00129AC0 File Offset: 0x00127CC0
		public void PrepareForDraw(SceneView cameraSceneView, ref Matrix viewMatrix, ref Matrix projectionMatrix, ref Matrix viewProjectionMatrix)
		{
			bool flag = !this._gameInstance.Engine.AnimationSystem.HasProcessed;
			if (flag)
			{
				throw new InvalidOperationException();
			}
			AnimationSystem animationSystem = this._gameInstance.Engine.AnimationSystem;
			float scale = 0.175f / (float)this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont.BaseSize;
			int spread = this._gameInstance.App.Fonts.DefaultFontFamily.RegularFont.Spread;
			float num = 1f / (float)spread;
			this._gameInstance.SceneRenderer.PrepareForIncomingEntityDrawTasks(cameraSceneView.IncomingEntityDrawTaskCount);
			bool isHudVisible = this._gameInstance.App.InGame.IsHudVisible;
			int i = 0;
			while (i < cameraSceneView.EntitiesCount)
			{
				int sortedEntityId = cameraSceneView.GetSortedEntityId(i);
				ref Entity ptr = ref this._entities[sortedEntityId];
				float num2 = this._distancesToCamera[sortedEntityId];
				bool flag2 = ptr.ModelRenderer == null || !ptr.ShouldRender;
				if (flag2)
				{
					throw new Exception("Entity with no ModelRenderer added to Render list!");
				}
				float scale2 = 0.015625f * ptr.Scale;
				Vector3 vector = ptr.RenderPosition - cameraSceneView.Position;
				Matrix matrix;
				Matrix.Compose(scale2, ptr.RenderOrientation, vector, out matrix);
				float modelHeight = ptr.Hitbox.Max.Y * 64f;
				ptr.ModelVFX.IdInTBO = this._gameInstance.SceneRenderer.RegisterModelVFXTask(ptr.ModelVFX.AnimationProgress, ptr.ModelVFX.HighlightColor, ptr.ModelVFX.HighlightThickness, ptr.ModelVFX.NoiseScale, ptr.ModelVFX.NoiseScrollSpeed, ptr.ModelVFX.PackedModelVFXParams, ptr.ModelVFX.PostColor, 0, 0, 0);
				ModelRenderer modelRenderer = ptr.ModelRenderer;
				Entity.EntityType type = ptr.Type;
				Entity.EntityType entityType = type;
				if (entityType != Entity.EntityType.Character)
				{
					if (entityType - Entity.EntityType.Item > 1)
					{
						throw new NotImplementedException("Unknown entity type");
					}
					bool flag3 = modelRenderer.NodeCount > 0;
					if (flag3)
					{
						this._gameInstance.SceneRenderer.RegisterEntityDrawTasks(sortedEntityId, ref matrix, modelRenderer.VertexArray, modelRenderer.IndicesCount, animationSystem.NodeBuffer, modelRenderer.NodeBufferOffset, modelRenderer.NodeCount, ptr.BlockLightColor, ptr.BottomTint, ptr.TopTint, modelHeight, ptr.UseDithering, ptr.ModelVFX.AnimationProgress, ptr.ModelVFX.PackedModelVFXParams, ptr.ModelVFX.IdInTBO);
					}
				}
				else
				{
					bool flag4 = modelRenderer.NodeCount > 0;
					if (flag4)
					{
						this._gameInstance.SceneRenderer.RegisterEntityDrawTasks(sortedEntityId, ref matrix, modelRenderer.VertexArray, modelRenderer.IndicesCount, animationSystem.NodeBuffer, modelRenderer.NodeBufferOffset, modelRenderer.NodeCount, ptr.BlockLightColor, ptr.BottomTint, ptr.TopTint, modelHeight, ptr.UseDithering, ptr.ModelVFX.AnimationProgress, ptr.ModelVFX.PackedModelVFXParams, ptr.ModelVFX.IdInTBO);
					}
					for (int j = 0; j < ptr.EntityEffects.Length; j++)
					{
						ref Entity.UniqueEntityEffect ptr2 = ref ptr.EntityEffects[j];
						bool flag5 = ptr2.ModelRenderer == null || ptr2.ModelRenderer.NodeCount == 0;
						if (!flag5)
						{
							this._gameInstance.SceneRenderer.RegisterEntityDrawTasks(sortedEntityId, ref matrix, ptr2.ModelRenderer.VertexArray, ptr2.ModelRenderer.IndicesCount, animationSystem.NodeBuffer, ptr2.ModelRenderer.NodeBufferOffset, ptr2.ModelRenderer.NodeCount, ptr.BlockLightColor, Vector3.Zero, Vector3.Zero, modelHeight, ptr.UseDithering, ptr.ModelVFX.AnimationProgress, ptr.ModelVFX.PackedModelVFXParams, ptr.ModelVFX.IdInTBO);
						}
					}
					for (int k = 0; k < ptr.EntityItems.Count; k++)
					{
						Entity.EntityItem entityItem = ptr.EntityItems[k];
						bool flag6 = entityItem.ModelRenderer == null || entityItem.ModelRenderer.NodeCount == 0;
						if (!flag6)
						{
							ClientModelVFX clientModelVFX = (entityItem.ModelVFX.Id != null) ? entityItem.ModelVFX : ptr.ModelVFX;
							ref AnimatedRenderer.NodeTransform ptr3 = ref modelRenderer.NodeTransforms[entityItem.TargetNodeIndex];
							Matrix matrix2;
							Matrix.Compose(ptr3.Orientation, ptr3.Position, out matrix2);
							Matrix.Multiply(entityItem.RootOffsetMatrix, ref matrix2, out matrix2);
							Matrix.Multiply(ref matrix2, ref matrix, out matrix2);
							Matrix.ApplyScale(ref matrix2, entityItem.Scale);
							this._gameInstance.SceneRenderer.RegisterEntityDrawTasks(sortedEntityId, ref matrix2, entityItem.ModelRenderer.VertexArray, entityItem.ModelRenderer.IndicesCount, animationSystem.NodeBuffer, entityItem.ModelRenderer.NodeBufferOffset, entityItem.ModelRenderer.NodeCount, ptr.BlockLightColor, Vector3.Zero, Vector3.Zero, modelHeight, ptr.UseDithering, clientModelVFX.AnimationProgress, clientModelVFX.PackedModelVFXParams, clientModelVFX.IdInTBO);
							bool flag7 = entityItem.ModelVFX.Id != null;
							if (flag7)
							{
								entityItem.ModelVFX.IdInTBO = this._gameInstance.SceneRenderer.RegisterModelVFXTask(clientModelVFX.AnimationProgress, clientModelVFX.HighlightColor, clientModelVFX.HighlightThickness, clientModelVFX.NoiseScale, clientModelVFX.NoiseScrollSpeed, clientModelVFX.PackedModelVFXParams, clientModelVFX.PostColor, 0, 0, 0);
							}
						}
					}
					bool flag8;
					if (isHudVisible && ptr.NetworkId != this._gameInstance.LocalPlayerNetworkId)
					{
						int[] uicomponents = ptr.UIComponents;
						flag8 = (uicomponents != null && uicomponents.Length != 0);
					}
					else
					{
						flag8 = false;
					}
					bool flag9 = flag8;
					if (flag9)
					{
						SceneRenderer.SceneData data = this._gameInstance.SceneRenderer.Data;
						float x = data.ViewportSize.X;
						float y = data.ViewportSize.Y;
						Vector3 position = ptr.Position;
						position.Y += ptr.Hitbox.Max.Y;
						Vector2 vector2 = Vector3.WorldToScreenPos(ref data.ViewProjectionMatrix, x, y, position);
						Matrix matrix3;
						Matrix.CreateTranslation(vector2.X - x / 2f, -(vector2.Y - y / 2f), 0f, out matrix3);
						this._gameInstance.App.Interface.InGameView.RegisterEntityUIDrawTasks(ref matrix3, ptr, num2);
					}
				}
				bool flag10 = isHudVisible && ptr.NameplateTextRenderer != null && num2 < 64f;
				if (flag10)
				{
					bool flag11 = !this.CurrentSetup.DrawLocalPlayerName && ptr.NetworkId == this._gameInstance.LocalPlayerNetworkId;
					if (!flag11)
					{
						float fillBlurThreshold = MathHelper.Clamp(2f * num2 * 0.1f, 1f, (float)spread) * num;
						Matrix.CreateTranslation(-ptr.NameplateTextRenderer.GetHorizontalOffset(TextRenderer.TextAlignment.Center), -ptr.NameplateTextRenderer.GetVerticalOffset(TextRenderer.TextVerticalAlignment.Middle), 0f, out matrix);
						Matrix matrix2;
						Matrix.CreateScale(scale, out matrix2);
						Matrix.Multiply(ref matrix, ref matrix2, out matrix);
						Vector3 rotation = this._gameInstance.CameraModule.Controller.Rotation;
						Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, 0f, out matrix2);
						Matrix.Multiply(ref matrix, ref matrix2, out matrix);
						vector = ptr.RenderPosition;
						vector.Y += ptr.Hitbox.Max.Y + 0.5f;
						Matrix.AddTranslation(ref matrix, vector.X, vector.Y, vector.Z);
						Matrix.Multiply(ref matrix, ref viewProjectionMatrix, out matrix);
						this._gameInstance.SceneRenderer.RegisterEntityNameplateDrawTask(sortedEntityId, ref matrix, vector - cameraSceneView.Position, fillBlurThreshold, ptr.NameplateTextRenderer.VertexArray, ptr.NameplateTextRenderer.IndicesCount);
					}
				}
				IL_863:
				i++;
				continue;
				goto IL_863;
			}
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x0012A34C File Offset: 0x0012854C
		public void PrepareForShadowMapDraw(SceneView sunSceneView)
		{
			bool flag = !this._gameInstance.Engine.AnimationSystem.HasProcessed;
			if (flag)
			{
				throw new InvalidOperationException();
			}
			AnimationSystem animationSystem = this._gameInstance.Engine.AnimationSystem;
			Vector3 cameraPosition = this._gameInstance.SceneRenderer.Data.CameraPosition;
			this._gameInstance.SceneRenderer.PrepareForIncomingEntitySunShadowCasterDrawTasks(sunSceneView.IncomingEntityDrawTaskCount);
			for (int i = 0; i < sunSceneView.EntitiesCount; i++)
			{
				int sortedEntityId = sunSceneView.GetSortedEntityId(i);
				ref Entity ptr = ref this._entities[sortedEntityId];
				bool flag2 = ptr.ModelRenderer == null;
				if (flag2)
				{
					throw new Exception("Entity with no ModelRenderer added to Render list!");
				}
				bool flag3 = ptr.ModelRenderer != null && ptr.ModelRenderer.NodeCount == 0;
				if (!flag3)
				{
					BoundingSphere boundingSphere = this._boundingVolumes[sortedEntityId];
					boundingSphere.Center -= cameraPosition;
					float num = 0.98f;
					float scale = 0.015625f * ptr.Scale * num;
					Vector3 translation = ptr.RenderPosition - cameraPosition;
					Matrix matrix;
					Matrix.Compose(scale, ptr.RenderOrientation, translation, out matrix);
					float modelHeight = ptr.Hitbox.Max.Y * 64f;
					ModelRenderer modelRenderer = ptr.ModelRenderer;
					Entity.EntityType type = ptr.Type;
					Entity.EntityType entityType = type;
					if (entityType != Entity.EntityType.Character)
					{
						if (entityType - Entity.EntityType.Item > 1)
						{
							throw new NotImplementedException("Unknown entity type");
						}
						this._gameInstance.SceneRenderer.RegisterEntitySunShadowCasterDrawTask(ref boundingSphere, ref matrix, modelRenderer.VertexArray, modelRenderer.IndicesCount, animationSystem.NodeBuffer, modelRenderer.NodeBufferOffset, (uint)modelRenderer.NodeCount, modelHeight, ptr.ModelVFX.AnimationProgress, ptr.ModelVFX.IdInTBO);
					}
					else
					{
						this._gameInstance.SceneRenderer.RegisterEntitySunShadowCasterDrawTask(ref boundingSphere, ref matrix, modelRenderer.VertexArray, modelRenderer.IndicesCount, animationSystem.NodeBuffer, modelRenderer.NodeBufferOffset, (uint)modelRenderer.NodeCount, modelHeight, ptr.ModelVFX.AnimationProgress, ptr.ModelVFX.IdInTBO);
						for (int j = 0; j < ptr.EntityItems.Count; j++)
						{
							Entity.EntityItem entityItem = ptr.EntityItems[j];
							ref AnimatedRenderer.NodeTransform ptr2 = ref modelRenderer.NodeTransforms[entityItem.TargetNodeIndex];
							Matrix matrix2;
							Matrix.Compose(ptr2.Orientation, ptr2.Position, out matrix2);
							Matrix.Multiply(entityItem.RootOffsetMatrix, ref matrix2, out matrix2);
							Matrix.Multiply(ref matrix2, ref matrix, out matrix2);
							Matrix.ApplyScale(ref matrix2, entityItem.Scale);
							this._gameInstance.SceneRenderer.RegisterEntitySunShadowCasterDrawTask(ref boundingSphere, ref matrix2, entityItem.ModelRenderer.VertexArray, entityItem.ModelRenderer.IndicesCount, animationSystem.NodeBuffer, entityItem.ModelRenderer.NodeBufferOffset, (uint)entityItem.ModelRenderer.NodeCount, modelHeight, ptr.ModelVFX.AnimationProgress, ptr.ModelVFX.IdInTBO);
						}
					}
				}
			}
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x0012A694 File Offset: 0x00128894
		public void PrepareDebugInfoForDraw(SceneView sceneView, ref Matrix viewProjectionMatrix)
		{
			CharacterControllerModule characterControllerModule = this._gameInstance.CharacterControllerModule;
			Matrix matrix = default(Matrix);
			Matrix matrix2 = default(Matrix);
			Matrix matrix3 = default(Matrix);
			for (int i = 0; i < sceneView.EntitiesCount; i++)
			{
				int sortedEntityId = sceneView.GetSortedEntityId(i);
				ref Entity ptr = ref this._entities[sortedEntityId];
				ref BoundingSphere ptr2 = ref this._boundingVolumes[sortedEntityId];
				float distanceToCamera = this._distancesToCamera[sortedEntityId];
				bool flag = ptr.NetworkId == this._gameInstance.LocalPlayerNetworkId;
				bool flag2 = flag && this._gameInstance.CameraModule.Controller.IsFirstPerson;
				if (!flag2)
				{
					bool hit = this._gameInstance.InteractionModule.TargetEntityHit == ptr;
					bool flag3 = ptr.LastPush != Vector2.Zero;
					Matrix matrix4;
					Matrix matrix5;
					if (flag3)
					{
						Vector3 position = ptr.Position;
						position.Y += ptr.Hitbox.GetSize().Y / 2f;
						float scale = ptr.LastPush.Length() * 2f;
						Matrix.CreateScale(scale, out matrix4);
						Vector3 zero = Vector3.Zero;
						Vector3 vector = new Vector3(ptr.LastPush.X, 0f, ptr.LastPush.Y);
						Quaternion quaternion;
						Quaternion.CreateFromVectors(ref zero, ref vector, out quaternion);
						Matrix.CreateFromYawPitchRoll((float)Math.Atan2((double)vector.X, (double)vector.Z) - 1.5707964f, 0f, 0f, out matrix5);
						Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
						Matrix.CreateTranslation(ref position, out matrix5);
						Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
						Matrix.Multiply(ref matrix4, ref viewProjectionMatrix, out matrix);
					}
					Vector3 position2 = ptr.Position;
					position2.Y += ptr.EyeOffset - 0.001f;
					bool flag4 = !flag;
					if (flag4)
					{
						position2.Y += (ptr.ServerMovementStates.IsCrouching ? ptr.CrouchOffset : 0f);
					}
					else
					{
						position2.Y += characterControllerModule.MovementController.AutoJumpHeightShift + characterControllerModule.MovementController.CrouchHeightShift;
					}
					float scale2 = MathHelper.Max(ptr.Hitbox.GetSize().X, ptr.Hitbox.GetSize().Z) / 2f + 0.75f;
					Matrix.CreateScale(scale2, out matrix4);
					Matrix.CreateFromYawPitchRoll(ptr.LookOrientation.Yaw + 1.5707964f, 0f, ptr.LookOrientation.Pitch, out matrix5);
					Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
					Matrix.CreateTranslation(ref position2, out matrix5);
					Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
					Matrix matrix6;
					Matrix.Multiply(ref matrix4, ref viewProjectionMatrix, out matrix6);
					BoundingBox hitbox = ptr.Hitbox;
					hitbox.Max.Y = 0.002f;
					Vector3 value = hitbox.Min + position2;
					Vector3 vector2 = hitbox.GetSize() / Vector3.One;
					Matrix.CreateScale(ref vector2, out matrix4);
					Matrix.CreateTranslation(ref value, out matrix5);
					Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
					Matrix matrix7;
					Matrix.Multiply(ref matrix4, ref viewProjectionMatrix, out matrix7);
					value = ptr.Hitbox.Min + ptr.Position;
					value.Y += 0.001f;
					vector2 = ptr.Hitbox.GetSize() / Vector3.One;
					Matrix.CreateScale(ref vector2, out matrix4);
					Matrix.CreateTranslation(ref value, out matrix5);
					Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
					Matrix matrix8;
					Matrix.Multiply(ref matrix4, ref viewProjectionMatrix, out matrix8);
					bool debugInfoBounds = this.DebugInfoBounds;
					Matrix matrix9;
					if (debugInfoBounds)
					{
						vector2 = new Vector3(ptr2.Radius);
						value = ptr2.Center;
						Matrix.CreateScale(ref vector2, out matrix4);
						Matrix.CreateTranslation(ref value, out matrix5);
						Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
						Matrix.Multiply(ref matrix4, ref viewProjectionMatrix, out matrix9);
					}
					else
					{
						matrix9 = default(Matrix);
					}
					HashSet<int> collidedEntities = this._gameInstance.CharacterControllerModule.MovementController.CollidedEntities;
					bool flag5 = collidedEntities.Contains(ptr.NetworkId);
					bool flag6 = (!this._gameInstance.DebugCollisionOnlyCollided || flag5) && ptr.HitboxCollisionConfigIndex != -1;
					bool flag7 = flag6;
					if (flag7)
					{
						Vector3 entityHitboxExpand = this._gameInstance.CharacterControllerModule.MovementController.EntityHitboxExpand;
						BoundingBox hitbox2 = ptr.Hitbox;
						hitbox2.Grow(entityHitboxExpand * 2f);
						value = hitbox2.Min + ptr.Position;
						vector2 = hitbox2.GetSize() / Vector3.One;
						Matrix.CreateScale(ref vector2, out matrix4);
						Matrix.CreateTranslation(ref value, out matrix5);
						Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
						Matrix.Multiply(ref matrix4, ref viewProjectionMatrix, out matrix3);
						bool flag8 = ptr.RepulsionConfigIndex != -1;
						if (flag8)
						{
							value = ptr.Position + new Vector3(0f, ptr.Hitbox.GetSize().Y / 2f, 0f);
							ClientRepulsionConfig clientRepulsionConfig = this._gameInstance.ServerSettings.RepulsionConfigs[ptr.RepulsionConfigIndex];
							vector2 = new Vector3(clientRepulsionConfig.Radius, 0f, clientRepulsionConfig.Radius);
							vector2.Y = ptr.Hitbox.GetSize().Y / 2f;
							Matrix.CreateScale(ref vector2, out matrix4);
							Matrix.CreateTranslation(ref value, out matrix5);
							Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
							Matrix.Multiply(ref matrix4, ref viewProjectionMatrix, out matrix2);
						}
					}
					int levelOfDetail = this.ComputeLevelOfDetail(distanceToCamera, ptr2.Radius);
					SceneRenderer.DebugInfoDetailTask[] array = null;
					bool flag9 = ptr.DetailBoundingBoxes.Count > 0;
					if (flag9)
					{
						int num = 0;
						foreach (Entity.DetailBoundingBox[] array2 in ptr.DetailBoundingBoxes.Values)
						{
							num += array2.Length;
						}
						array = new SceneRenderer.DebugInfoDetailTask[num];
						Quaternion quaternion2 = Quaternion.CreateFromAxisAngle(Vector3.Up, ptr.BodyOrientation.Y);
						int num2 = 0;
						foreach (KeyValuePair<string, Entity.DetailBoundingBox[]> keyValuePair in ptr.DetailBoundingBoxes)
						{
							float h = Math.Abs((float)keyValuePair.Key.GetHashCode() / 2.1474836E+09f);
							ColorHsla colorHsla = new ColorHsla(h, 0.83f, 0.77f, 1f);
							Vector3 color = default(Vector3);
							colorHsla.ToRgb(out color.X, out color.Y, out color.Z);
							foreach (Entity.DetailBoundingBox detailBoundingBox in keyValuePair.Value)
							{
								value = detailBoundingBox.Offset;
								Vector3.Transform(ref value, ref quaternion2, out value);
								value += ptr.Position + detailBoundingBox.Box.Min;
								value.Y += 0.001f;
								BoundingBox box = detailBoundingBox.Box;
								vector2 = box.GetSize() / Vector3.One;
								Matrix.CreateScale(ref vector2, out matrix4);
								Matrix.CreateTranslation(ref value, out matrix5);
								Matrix.Multiply(ref matrix4, ref matrix5, out matrix4);
								Matrix matrix10;
								Matrix.Multiply(ref matrix4, ref viewProjectionMatrix, out matrix10);
								array[num2++] = new SceneRenderer.DebugInfoDetailTask
								{
									Color = color,
									Matrix = matrix10
								};
							}
						}
					}
					this._gameInstance.SceneRenderer.RegisterEntityDebugDrawTask(hit, flag6, flag5, levelOfDetail, ref matrix6, ref matrix7, ref matrix8, ref matrix9, ref matrix3, ref matrix2, ref matrix, array);
				}
			}
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x0012AEF4 File Offset: 0x001290F4
		public void UpdateVisibilityPrediction(int[] occludeesVisibility, int entityResultsOffset, int entityResultsCount, int lightResultsOffset, int lightResultsCount, bool updateEntities, bool updateLights)
		{
			bool flag = !updateEntities;
			bool flag2 = !updateLights;
			for (int i = 0; i < entityResultsCount; i++)
			{
				bool visibilityPrediction = flag || occludeesVisibility[entityResultsOffset + i] == 1;
				this._entities[i].VisibilityPrediction = visibilityPrediction;
			}
			for (int j = 0; j < lightResultsCount; j++)
			{
				bool visibilityPrediction2 = flag2 || occludeesVisibility[lightResultsOffset + j] == 1;
				ushort num = this.SortedEntityLightIds[j];
				this.EntityLights[(int)num].VisibilityPrediction = visibilityPrediction2;
			}
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x0012AF8C File Offset: 0x0012918C
		public void ProcessFXUpdateTasks()
		{
			bool flag = !this._gameInstance.Engine.AnimationSystem.HasProcessed;
			if (flag)
			{
				throw new InvalidOperationException();
			}
			for (int i = 0; i < this._fxUpdateTaskCount; i++)
			{
				this._fxUpdateTasks[i].Entity.UpdateFX();
			}
		}

		// Token: 0x06004A17 RID: 18967 RVA: 0x0012AFEA File Offset: 0x001291EA
		private void ResetFXUpdateTaskCounters()
		{
			this._incomingFXUpdateTaskCount = 0;
			this._fxUpdateTaskCount = 0;
		}

		// Token: 0x06004A18 RID: 18968 RVA: 0x0012AFFB File Offset: 0x001291FB
		public void QueueSoundEvent(uint soundEventIndex, int networkId)
		{
			this._queuedSoundEvents.Add(new EntityStoreModule.QueuedSoundEvent(soundEventIndex, networkId));
		}

		// Token: 0x040025B2 RID: 9650
		private readonly ConcurrentDictionary<string, BlockyModel> _models = new ConcurrentDictionary<string, BlockyModel>();

		// Token: 0x040025B3 RID: 9651
		private readonly ConcurrentDictionary<string, BlockyAnimation> _animations = new ConcurrentDictionary<string, BlockyAnimation>();

		// Token: 0x040025B4 RID: 9652
		private readonly Queue<Tuple<string, string>> _modelsAndAnimationsToLoad = new Queue<Tuple<string, string>>();

		// Token: 0x040025B5 RID: 9653
		private Thread _thread;

		// Token: 0x040025B6 RID: 9654
		private CancellationTokenSource _threadCancellationTokenSource;

		// Token: 0x040025B7 RID: 9655
		private CancellationToken _threadCancellationToken;

		// Token: 0x040025B8 RID: 9656
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040025B9 RID: 9657
		private readonly List<EntityStoreModule.QueuedSoundEvent> _queuedSoundEvents = new List<EntityStoreModule.QueuedSoundEvent>();

		// Token: 0x040025BA RID: 9658
		private readonly Dictionary<int, Entity> _entitiesByNetworkId = new Dictionary<int, Entity>();

		// Token: 0x040025BB RID: 9659
		private readonly Dictionary<Guid, Entity> _entitiesByPredictionId = new Dictionary<Guid, Entity>();

		// Token: 0x040025BC RID: 9660
		private const int EntitiesDefaultSize = 1000;

		// Token: 0x040025BD RID: 9661
		private const int EntitiesGrowth = 500;

		// Token: 0x040025BE RID: 9662
		private int _entitiesCount;

		// Token: 0x040025BF RID: 9663
		private Entity[] _entities = new Entity[1000];

		// Token: 0x040025C0 RID: 9664
		private bool _needDeferredDespawn = false;

		// Token: 0x040025C1 RID: 9665
		private List<int> _deferredDespawnIds = new List<int>(10);

		// Token: 0x040025C2 RID: 9666
		private int _playerEntityLocalId;

		// Token: 0x040025C3 RID: 9667
		public int MountEntityLocalId = -1;

		// Token: 0x040025C4 RID: 9668
		private const int BoundingVolumesDefaultSize = 1000;

		// Token: 0x040025C5 RID: 9669
		private const int BoundingVolumesGrowth = 500;

		// Token: 0x040025C6 RID: 9670
		private BoundingSphere[] _boundingVolumes = new BoundingSphere[1000];

		// Token: 0x040025C7 RID: 9671
		private float[] _distancesToCamera = new float[1000];

		// Token: 0x040025C8 RID: 9672
		private Vector3[] _orientations = new Vector3[1000];

		// Token: 0x040025C9 RID: 9673
		private AudioDevice.SoundObjectReference[] _soundObjectReferences = new AudioDevice.SoundObjectReference[1000];

		// Token: 0x040025CA RID: 9674
		public const int MaxClosestEntities = 20;

		// Token: 0x040025CC RID: 9676
		private const int FXUpdateTasksDefaultSize = 200;

		// Token: 0x040025CD RID: 9677
		private const int FXUpdateTasksGrowth = 100;

		// Token: 0x040025CE RID: 9678
		private int _incomingFXUpdateTaskCount;

		// Token: 0x040025CF RID: 9679
		private int _fxUpdateTaskCount;

		// Token: 0x040025D0 RID: 9680
		private EntityStoreModule.FXUpdateTask[] _fxUpdateTasks = new EntityStoreModule.FXUpdateTask[200];

		// Token: 0x040025D1 RID: 9681
		public readonly Dictionary<string, int> ModelVFXByIds = new Dictionary<string, int>();

		// Token: 0x040025D2 RID: 9682
		public ModelVFX[] ModelVFXs;

		// Token: 0x040025D3 RID: 9683
		public readonly Dictionary<string, int> EntityEffectIndicesByIds = new Dictionary<string, int>();

		// Token: 0x040025D4 RID: 9684
		public EntityEffect[] EntityEffects;

		// Token: 0x040025D5 RID: 9685
		private const int EntityLightsDefaultSize = 200;

		// Token: 0x040025D6 RID: 9686
		private const int EntityLightsGrowth = 100;

		// Token: 0x040025D7 RID: 9687
		public int EntityLightCount = 0;

		// Token: 0x040025D8 RID: 9688
		public EntityStoreModule.EntityLight[] EntityLights = new EntityStoreModule.EntityLight[200];

		// Token: 0x040025D9 RID: 9689
		public ushort[] SortedEntityLightIds = new ushort[200];

		// Token: 0x040025DA RID: 9690
		public float[] SortedEntityLightCameraSquaredDistances = new float[200];

		// Token: 0x040025DB RID: 9691
		public EntityStoreModule.Setup CurrentSetup = EntityStoreModule.Setup.CreateDefault();

		// Token: 0x040025DC RID: 9692
		public readonly Texture TextureAtlas;

		// Token: 0x040025DE RID: 9694
		public bool DebugInfoNeedsDrawing = false;

		// Token: 0x040025DF RID: 9695
		public bool DebugInfoBounds = false;

		// Token: 0x040025E0 RID: 9696
		private int _nextLocalEntityId = -1;

		// Token: 0x040025E1 RID: 9697
		public NodeNameManager NodeNameManager;

		// Token: 0x02000E3C RID: 3644
		public struct Setup
		{
			// Token: 0x06006732 RID: 26418 RVA: 0x0021726C File Offset: 0x0021546C
			public static EntityStoreModule.Setup CreateDefault()
			{
				return new EntityStoreModule.Setup
				{
					LogicalLoDUpdate = true,
					DrawLocalPlayerName = false,
					DisplayDebugCommandsOnEntityEffect = false,
					DistanceToCameraBeforeRotation = 64f,
					DebugUI = false
				};
			}

			// Token: 0x040045B3 RID: 17843
			public bool LogicalLoDUpdate;

			// Token: 0x040045B4 RID: 17844
			public bool DrawLocalPlayerName;

			// Token: 0x040045B5 RID: 17845
			public bool DisplayDebugCommandsOnEntityEffect;

			// Token: 0x040045B6 RID: 17846
			public float DistanceToCameraBeforeRotation;

			// Token: 0x040045B7 RID: 17847
			public bool DebugUI;
		}

		// Token: 0x02000E3D RID: 3645
		public struct QueuedSoundEvent
		{
			// Token: 0x06006733 RID: 26419 RVA: 0x002172B3 File Offset: 0x002154B3
			public QueuedSoundEvent(uint eventIndex, int networkId)
			{
				this.Time = 1f;
				this.EventIndex = eventIndex;
				this.NetworkId = networkId;
			}

			// Token: 0x040045B8 RID: 17848
			public float Time;

			// Token: 0x040045B9 RID: 17849
			public uint EventIndex;

			// Token: 0x040045BA RID: 17850
			public int NetworkId;
		}

		// Token: 0x02000E3E RID: 3646
		private struct FXUpdateTask
		{
			// Token: 0x040045BB RID: 17851
			public Entity Entity;
		}

		// Token: 0x02000E3F RID: 3647
		public struct EntityLight
		{
			// Token: 0x06006734 RID: 26420 RVA: 0x002172D0 File Offset: 0x002154D0
			public EntityLight(int entityNetworkId, Vector3 color)
			{
				this.EntityNetworkId = entityNetworkId;
				this.LightData.Color = color;
				this.LightData.Sphere.Center = Vector3.Zero;
				this.LightData.Sphere.Radius = 0f;
				this.VisibilityPrediction = true;
			}

			// Token: 0x040045BC RID: 17852
			public readonly int EntityNetworkId;

			// Token: 0x040045BD RID: 17853
			public LightData LightData;

			// Token: 0x040045BE RID: 17854
			public bool VisibilityPrediction;
		}
	}
}
