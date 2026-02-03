using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Hypixel.ProtoPlus;
using HytaleClient.Application;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.Audio;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Data.Characters;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.Entities;
using HytaleClient.Data.Entities.Initializers;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.EntityUI;
using HytaleClient.Data.FX;
using HytaleClient.Data.Items;
using HytaleClient.Data.Map;
using HytaleClient.Data.Palette;
using HytaleClient.Data.Weather;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Particles;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules;
using HytaleClient.InGame.Modules.AmbienceFX;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.InGame.Modules.Machinima.Actors;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.InGame.Modules.WorldMap;
using HytaleClient.Interface;
using HytaleClient.Interface.InGame;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using HytaleClient.Net.Protocol;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;
using Org.BouncyCastle.Pkix;

namespace HytaleClient.Networking
{
	// Token: 0x020007DA RID: 2010
	internal class PacketHandler : Disposable
	{
		// Token: 0x0600346D RID: 13421 RVA: 0x00055248 File Offset: 0x00053448
		private void ProcessUpdateAmbienceFXPacket(UpdateAmbienceFX packet)
		{
			UpdateAmbienceFX updateAmbienceFX = new UpdateAmbienceFX(packet);
			UpdateType type = updateAmbienceFX.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateAmbienceFX.GetType().Name, this._stage, updateAmbienceFX.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] AmbienceFX: Starting {0}", updateAmbienceFX.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = updateAmbienceFX.Type == 1;
				if (flag)
				{
					bool flag2 = updateAmbienceFX.MaxId > this._networkAmbienceFXs.Length;
					if (flag2)
					{
						Array.Resize<AmbienceFX>(ref this._networkAmbienceFXs, updateAmbienceFX.MaxId);
					}
					foreach (KeyValuePair<int, AmbienceFX> keyValuePair in updateAmbienceFX.AmbienceFX_)
					{
						this._networkAmbienceFXs[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				AmbienceFX[] clonedAmbienceFXs = new AmbienceFX[this._networkAmbienceFXs.Length];
				for (int i = 0; i < this._networkAmbienceFXs.Length; i++)
				{
					clonedAmbienceFXs[i] = new AmbienceFX(this._networkAmbienceFXs[i]);
				}
				AmbienceFXSettings[] ambienceFXSettings;
				this._gameInstance.AmbienceFXModule.PrepareAmbienceFXs(clonedAmbienceFXs, out ambienceFXSettings);
				UpdateType updateType = updateAmbienceFX.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.AmbienceFXModule.SetupAmbienceFXs(clonedAmbienceFXs, ambienceFXSettings);
					this._gameInstance.AmbienceFXModule.OnAmbienceFXChanged();
					this._gameInstance.AudioModule.OnSoundEffectCollectionChanged();
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] AmbienceFX: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.AmbienceFX);
				bool flag3 = this._networkAmbienceFXs == null;
				if (flag3)
				{
					this._networkAmbienceFXs = new AmbienceFX[updateAmbienceFX.MaxId];
				}
				foreach (KeyValuePair<int, AmbienceFX> keyValuePair2 in updateAmbienceFX.AmbienceFX_)
				{
					this._networkAmbienceFXs[keyValuePair2.Key] = keyValuePair2.Value;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.AmbienceFX);
			}
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x000554C0 File Offset: 0x000536C0
		private static bool IsAssetPathItemIconRelated(string assetPath)
		{
			return assetPath.StartsWith("Icons/ItemsGenerated/") || assetPath.StartsWith("Icons/Items/");
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x000554DD File Offset: 0x000536DD
		private static bool IsAssetPathBlockRelated(string assetPath)
		{
			return assetPath.StartsWith("Blocks/") || assetPath.StartsWith("BlockTextures/") || assetPath.StartsWith("Resources/");
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x00055507 File Offset: 0x00053707
		private static bool IsAssetPathParticlesRelated(string assetPath)
		{
			return assetPath.StartsWith("Particles/");
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x00055514 File Offset: 0x00053714
		private static bool IsAssetPathTrailsRelated(string assetPath)
		{
			return assetPath.StartsWith("Trails/");
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x00055521 File Offset: 0x00053721
		private static bool IsAssetPathSkyRelated(string assetPath)
		{
			return assetPath.StartsWith("Sky/");
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x0005552E File Offset: 0x0005372E
		private static bool IsAssetPathWorldMapUIRelated(string assetPath)
		{
			return assetPath.StartsWith("UI/WorldMap/");
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x0005553B File Offset: 0x0005373B
		private static bool IsAssetPathScreenEffectRelated(string assetPath)
		{
			return assetPath.StartsWith("ScreenEffects/");
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x00055548 File Offset: 0x00053748
		private static bool IsAssetPathSoundEffectRelated(string assetPath)
		{
			return assetPath.StartsWith("SoundEffects/");
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x00055555 File Offset: 0x00053755
		private static bool IsAssetPathSoundBankRelated(string assetPath)
		{
			return assetPath.StartsWith("SoundBanks/");
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x00055562 File Offset: 0x00053762
		private static bool IsAssetPathItemAnimationRelated(string assetPath)
		{
			return assetPath.StartsWith("Characters/Animations/Items/") || assetPath.StartsWith("NPC/");
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x0005557F File Offset: 0x0005377F
		private static bool IsAssetPathCharacterAnimationRelated(string assetPath)
		{
			return assetPath.StartsWith("Characters/Animations/");
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x0005558C File Offset: 0x0005378C
		private static bool IsAssetPathItemRelated(string assetPath)
		{
			return assetPath.StartsWith("UI/Reticles/") || assetPath.StartsWith("Items/") || EntityStoreModule.IsAssetPathCharacterRelated(assetPath) || PacketHandler.IsAssetPathBlockRelated(assetPath);
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000555CC File Offset: 0x000537CC
		private void ProcessAssetInitializePacket(AssetInitialize packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			bool flag = this._blobAsset != null;
			if (flag)
			{
				throw new Exception("A blob download has already started! Name: " + this._blobAsset.Name + ", Hash: " + this._blobAsset.Hash);
			}
			bool flag2 = !PacketHandler.HashRegex.IsMatch(packet.Asset_.Hash);
			if (flag2)
			{
				throw new Exception("Invalid asset hash " + packet.Asset_.Hash + " for " + packet.Asset_.Name);
			}
			this._blobFileStream = File.Create(Paths.TempAssetDownload);
			this._blobAsset = packet.Asset_;
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x0005567E File Offset: 0x0005387E
		private void ProcessAssetPartPacket(AssetPart packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._blobFileStream.Write((byte[])packet.Part, 0, packet.Part.Length);
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000556AC File Offset: 0x000538AC
		private void ProcessAssetFinalizePacket(AssetFinalize packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._blobFileStream.Flush(true);
			this._blobFileStream.Close();
			this._blobFileStream = null;
			string assetName = this._blobAsset.Name;
			string assetHash = this._blobAsset.Hash;
			this._blobAsset = null;
			string text = Path.Combine(Paths.CachedAssets, assetHash.Substring(0, 2));
			string text2 = Path.Combine(text, assetHash.Substring(2));
			Directory.CreateDirectory(Paths.CachedAssets);
			Directory.CreateDirectory(text);
			bool flag = File.Exists(text2);
			if (flag)
			{
				File.Delete(text2);
			}
			File.Move(Paths.TempAssetDownload, text2);
			PacketHandler.ConnectionStage stage = this._stage;
			PacketHandler.ConnectionStage connectionStage = stage;
			if (connectionStage != PacketHandler.ConnectionStage.SettingUp)
			{
				if (connectionStage == PacketHandler.ConnectionStage.Playing)
				{
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						AssetManager.AddServerAssetToCache(assetName, assetHash);
						this._gameInstance.RegisterHashForServerAsset(assetName, assetHash);
						this._assetNamesToUpdate.Add(assetName);
						this._gameInstance.App.DevTools.Info("[AssetUpdate] Assets: Starting AddOrUpdate \"" + assetName + "\"");
					}, false, false);
				}
			}
			else
			{
				AssetManager.AddServerAssetToCache(assetName, assetHash);
			}
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000557C8 File Offset: 0x000539C8
		private void ProcessRemoveAssetsPacket(RemoveAssets packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			Asset[] assets = new Asset[packet.Asset_.Length];
			for (int i = 0; i < packet.Asset_.Length; i++)
			{
				bool flag = packet.Asset_[i] != null;
				if (flag)
				{
					assets[i] = new Asset(packet.Asset_[i]);
				}
			}
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				List<string> list = new List<string>();
				Asset[] assets;
				foreach (Asset asset in assets)
				{
					bool flag2 = !PacketHandler.HashRegex.IsMatch(asset.Hash);
					if (flag2)
					{
						throw new Exception("Invalid asset hash " + asset.Hash + " for " + asset.Name);
					}
					list.Add(asset.Name);
					this._assetNamesToUpdate.Add(asset.Name);
					this._gameInstance.RemoveHashForServerAsset(asset.Name);
					AssetManager.RemoveServerAssetFromCache(asset.Name, asset.Hash);
				}
				this._gameInstance.App.DevTools.Info("[AssetUpdate] Assets: Starting Remove [" + string.Join(",", list) + "]");
			}, false, false);
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x00055860 File Offset: 0x00053A60
		private void ProcessRequestCommonAssetsRebuildPacket(RequestCommonAssetsRebuild packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				string[] array = this._assetNamesToUpdate.ToArray();
				this._assetNamesToUpdate.Clear();
				this._gameInstance.App.Interface.TriggerEvent("assets.updated", array, null, null, null, null, null);
				this.QueueCommonAssetUpdate(array);
			}, false, false);
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x00055894 File Offset: 0x00053A94
		private void QueueCommonAssetUpdate(string[] assetNamesToUpdate)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				this._gameInstance.App.DevTools.Info("[AssetUpdate] Assets: Inside ThreadPool.QueueUserWorkItem " + TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds));
				byte[][] upcomingBlocksAtlasPixelsPerLevel = null;
				byte[][] upcomingEntitiesAtlasPixelsPerLevel = null;
				byte[][] upcomingWorldMapAtlasPixelsPerLevel = null;
				byte[][] upcomingFXAtlasPixelsPerLevel = null;
				byte[] upcomingIconAtlasPixels = null;
				int upcomingIconAtlasWidth = 0;
				int upcomingIconAtlasHeight = 0;
				ClientBlockType[] upcomingBlockTypes = null;
				Dictionary<string, ClientIcon> upcomingItemIcons = null;
				Dictionary<string, Point> upcomingEntitiesImageLocations = null;
				Dictionary<string, ClientItemPlayerAnimations> upcomingItemAnimations = null;
				Dictionary<string, ClientItemBase> upcomingItems = null;
				ClientInteraction[] upcomingInteractions = null;
				Dictionary<string, ParticleSystem> upcomingParticleSystems = null;
				Dictionary<string, ParticleSpawner> upcomingParticleSpawners = null;
				Dictionary<string, ParticleSettings> upcomingParticles = null;
				Dictionary<string, Trail> upcomingTrails = null;
				Dictionary<string, Rectangle> upcomingFXImageLocations = null;
				Dictionary<string, WorldMapModule.Texture> upcomingWorldMapImageLocations = null;
				AmbienceFXSettings[] ambienceFXSettings = null;
				Dictionary<string, WwiseResource> upcomingWwiseIds = null;
				AmbienceFX[] clonedAmbienceFXs = null;
				bool blockRelatedUpdate = false;
				bool flag = false;
				bool characterRelatedUpdate = false;
				bool flag2 = false;
				bool particlesRelatedUpdate = false;
				bool flag3 = false;
				bool trailsRelatedUpdate = false;
				bool skyRelatedUpdate = false;
				bool screenEffectsRelatedUpdate = false;
				bool flag4 = false;
				bool flag5 = false;
				bool soundEffectRelatedUpdate = false;
				bool soundBankRelatedUpdate = false;
				foreach (string assetPath in assetNamesToUpdate)
				{
					bool flag6 = PacketHandler.IsAssetPathBlockRelated(assetPath);
					if (flag6)
					{
						blockRelatedUpdate = true;
					}
					bool flag7 = PacketHandler.IsAssetPathItemIconRelated(assetPath);
					if (flag7)
					{
						flag = true;
					}
					bool flag8 = EntityStoreModule.IsAssetPathCharacterRelated(assetPath);
					if (flag8)
					{
						characterRelatedUpdate = true;
					}
					bool flag9 = PacketHandler.IsAssetPathItemRelated(assetPath);
					if (flag9)
					{
						flag2 = true;
					}
					bool flag10 = PacketHandler.IsAssetPathParticlesRelated(assetPath);
					if (flag10)
					{
						particlesRelatedUpdate = true;
					}
					bool flag11 = PacketHandler.IsAssetPathWorldMapUIRelated(assetPath);
					if (flag11)
					{
						flag3 = true;
					}
					bool flag12 = PacketHandler.IsAssetPathTrailsRelated(assetPath);
					if (flag12)
					{
						trailsRelatedUpdate = true;
					}
					bool flag13 = PacketHandler.IsAssetPathSkyRelated(assetPath);
					if (flag13)
					{
						skyRelatedUpdate = true;
					}
					bool flag14 = PacketHandler.IsAssetPathScreenEffectRelated(assetPath);
					if (flag14)
					{
						screenEffectsRelatedUpdate = true;
					}
					bool flag15 = PacketHandler.IsAssetPathItemAnimationRelated(assetPath);
					if (flag15)
					{
						flag4 = true;
					}
					bool flag16 = PacketHandler.IsAssetPathCharacterAnimationRelated(assetPath);
					if (flag16)
					{
						flag5 = true;
					}
					bool flag17 = PacketHandler.IsAssetPathSoundEffectRelated(assetPath);
					if (flag17)
					{
						soundEffectRelatedUpdate = true;
					}
					bool flag18 = PacketHandler.IsAssetPathSoundBankRelated(assetPath);
					if (flag18)
					{
						soundBankRelatedUpdate = true;
					}
				}
				long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
				object setupLock = this._setupLock;
				lock (setupLock)
				{
					PacketHandler.Logger.Info("[AssetUpdate] Assets: Took {0} to obtain lock", TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds - elapsedMilliseconds));
					Array.Clear(this._assetUpdatePrepareTimes, 0, this._assetUpdatePrepareTimes.Length);
					try
					{
						bool blockRelatedUpdate2 = blockRelatedUpdate;
						if (blockRelatedUpdate2)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._gameInstance.MapModule.PrepareBlockTypes(this._networkBlockTypes, this._highestReceivedBlockId, true, ref this._upcomingBlockTypes, ref this._upcomingBlocksImageLocations, ref this._upcomingBlocksAtlasSize, out upcomingBlocksAtlasPixelsPerLevel, this._threadCancellationToken);
							bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested)
							{
								return;
							}
							this._assetUpdatePrepareTimes[0] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
							this._assetUpdatePrepareTimer.Restart();
							upcomingBlockTypes = new ClientBlockType[this._upcomingBlockTypes.Length];
							Array.Copy(this._upcomingBlockTypes, upcomingBlockTypes, this._upcomingBlockTypes.Length);
							this._assetUpdatePrepareTimes[1] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool flag20 = flag;
						if (flag20)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._gameInstance.ItemLibraryModule.PrepareItemIconAtlas(this._networkItems, out upcomingItemIcons, out upcomingIconAtlasPixels, out upcomingIconAtlasWidth, out upcomingIconAtlasHeight, this._threadCancellationToken);
							bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested2)
							{
								return;
							}
							this._assetUpdatePrepareTimes[2] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool flag21 = characterRelatedUpdate || flag;
						if (flag21)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._gameInstance.EntityStoreModule.PrepareAtlas(out this._upcomingEntitiesImageLocations, out upcomingEntitiesAtlasPixelsPerLevel, this._threadCancellationToken);
							upcomingEntitiesImageLocations = this._upcomingEntitiesImageLocations;
							bool isCancellationRequested3 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested3)
							{
								return;
							}
							this._assetUpdatePrepareTimes[3] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool flag22 = flag2;
						if (flag22)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._upcomingItems.Clear();
							this._gameInstance.ItemLibraryModule.PrepareItems(this._networkItems, this._upcomingEntitiesImageLocations, ref this._upcomingItems, this._threadCancellationToken);
							upcomingItems = new Dictionary<string, ClientItemBase>(this._upcomingItems);
							bool isCancellationRequested4 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested4)
							{
								return;
							}
							this._assetUpdatePrepareTimes[4] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool particlesRelatedUpdate2 = particlesRelatedUpdate;
						if (particlesRelatedUpdate2)
						{
							this._assetUpdatePrepareTimer.Restart();
							upcomingParticleSystems = new Dictionary<string, ParticleSystem>(this._networkParticleSystems);
							upcomingParticleSpawners = new Dictionary<string, ParticleSpawner>(this._networkParticleSpawners);
							this._gameInstance.ParticleSystemStoreModule.PrepareParticles(upcomingParticleSpawners, out upcomingParticles, out this._upcomingParticleTextureInfo, out this._upcomingUVMotionTexturePaths, this._threadCancellationToken);
							bool isCancellationRequested5 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested5)
							{
								return;
							}
							this._assetUpdatePrepareTimes[5] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool trailsRelatedUpdate3 = trailsRelatedUpdate;
						if (trailsRelatedUpdate3)
						{
							this._assetUpdatePrepareTimer.Restart();
							upcomingTrails = new Dictionary<string, Trail>(this._networkTrails);
							this._gameInstance.TrailStoreModule.PrepareTrails(this._networkTrails, out this._upcomingTrailTextureInfo, this._threadCancellationToken);
							bool isCancellationRequested6 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested6)
							{
								return;
							}
							this._assetUpdatePrepareTimes[6] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool flag23 = flag4;
						if (flag23)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._gameInstance.ItemLibraryModule.PrepareItemPlayerAnimations(this._networkItemPlayerAnimations, out this._upcomingItemPlayerAnimations);
							upcomingItemAnimations = this._upcomingItemPlayerAnimations;
							bool isCancellationRequested7 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested7)
							{
								return;
							}
							this._assetUpdatePrepareTimes[7] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool flag24 = flag5;
						if (flag24)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._gameInstance.InteractionModule.PrepareInteractions(this._networkInteractions, out this._upcomingInteractions);
							upcomingInteractions = this._upcomingInteractions;
							bool isCancellationRequested8 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested8)
							{
								return;
							}
							this._assetUpdatePrepareTimes[8] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool flag25 = flag3;
						if (flag25)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._gameInstance.WorldMapModule.PrepareTextureAtlas(out this._upcomingWorldMapImageLocations, out upcomingWorldMapAtlasPixelsPerLevel, this._threadCancellationToken);
							bool isCancellationRequested9 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested9)
							{
								return;
							}
							upcomingWorldMapImageLocations = this._upcomingWorldMapImageLocations;
							this._assetUpdatePrepareTimes[9] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool flag26 = particlesRelatedUpdate | trailsRelatedUpdate;
						if (flag26)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._gameInstance.FXModule.PrepareAtlas(this._upcomingParticleTextureInfo, this._upcomingTrailTextureInfo, out upcomingFXImageLocations, out upcomingFXAtlasPixelsPerLevel, this._threadCancellationToken);
							bool isCancellationRequested10 = this._threadCancellationToken.IsCancellationRequested;
							if (isCancellationRequested10)
							{
								return;
							}
							this._assetUpdatePrepareTimes[10] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool soundEffectRelatedUpdate3 = soundEffectRelatedUpdate;
						if (soundEffectRelatedUpdate3)
						{
							this._assetUpdatePrepareTimer.Restart();
							clonedAmbienceFXs = new AmbienceFX[this._networkAmbienceFXs.Length];
							for (int j = 0; j < this._networkAmbienceFXs.Length; j++)
							{
								clonedAmbienceFXs[j] = new AmbienceFX(this._networkAmbienceFXs[j]);
							}
							this._gameInstance.AmbienceFXModule.PrepareAmbienceFXs(clonedAmbienceFXs, out ambienceFXSettings);
							this._assetUpdatePrepareTimes[11] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
						bool soundBankRelatedUpdate3 = soundBankRelatedUpdate;
						if (soundBankRelatedUpdate3)
						{
							this._assetUpdatePrepareTimer.Restart();
							this._gameInstance.AudioModule.PrepareSoundBanks(out upcomingWwiseIds);
							this._assetUpdatePrepareTimes[12] = this._assetUpdatePrepareTimer.ElapsedMilliseconds;
						}
					}
					catch (FileNotFoundException ex)
					{
						this._gameInstance.App.DevTools.Error("[AssetUpdate] Failed to update assets! File disappeared: " + ex.FileName);
						PacketHandler.Logger.Error(ex, "Failed to update assets! File disappeared:");
						return;
					}
					catch (IOException ex2)
					{
						this._gameInstance.App.DevTools.Error("[AssetUpdate] Failed to update assets! " + ex2.Message);
						PacketHandler.Logger.Error(ex2, "Failed to update assets:");
						return;
					}
					catch (KeyNotFoundException ex3)
					{
						this._gameInstance.App.DevTools.Error("[AssetUpdate] Failed to update assets! " + ex3.Message);
						PacketHandler.Logger.Error(ex3, "Failed to update assets:");
						return;
					}
					PacketHandler.Logger.Info("[AssetUpdate] Assets: Finished prepare in {0}.", TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds));
					for (int k = 0; k < this._assetUpdatePrepareTimes.Length; k++)
					{
						long num = this._assetUpdatePrepareTimes[k];
						bool flag27 = num > 0L;
						if (flag27)
						{
							PacketHandler.AssetUpdatePrepareSteps argument = (PacketHandler.AssetUpdatePrepareSteps)k;
							PacketHandler.Logger.Info<PacketHandler.AssetUpdatePrepareSteps, string>("[AssetUpdate] {0}: {1}", argument, TimeHelper.FormatMillis(num));
						}
					}
				}
				long beforeMainThread = stopwatch.ElapsedMilliseconds;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					PacketHandler.Logger.Info("[AssetUpdate] Assets: Took {0} to enter main thread!", TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds - beforeMainThread));
					Array.Clear(this._assetUpdateSetupTimes, 0, this._assetUpdateSetupTimes.Length);
					bool flag28 = upcomingBlockTypes != null;
					if (flag28)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.MapModule.SetupBlockTypes(upcomingBlockTypes, true);
						this._assetUpdateSetupTimes[0] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag29 = upcomingBlocksAtlasPixelsPerLevel != null;
					if (flag29)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.MapModule.TextureAtlas.UpdateTexture2DMipMaps(upcomingBlocksAtlasPixelsPerLevel);
						this._assetUpdateSetupTimes[1] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag30 = upcomingItemIcons != null;
					if (flag30)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.ItemLibraryModule.SetupItemIcons(upcomingItemIcons, upcomingIconAtlasPixels, upcomingIconAtlasWidth, upcomingIconAtlasHeight);
						this._assetUpdateSetupTimes[2] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag31 = upcomingItemAnimations != null;
					if (flag31)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.ItemLibraryModule.SetupItemPlayerAnimations(upcomingItemAnimations);
						this._assetUpdateSetupTimes[3] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag32 = upcomingItems != null;
					if (flag32)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.ItemLibraryModule.SetupItems(upcomingItems);
						this._assetUpdateSetupTimes[4] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag33 = upcomingInteractions != null;
					if (flag33)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.InteractionModule.SetupInteractions(upcomingInteractions);
						this._assetUpdateSetupTimes[5] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag34 = upcomingParticles != null;
					if (flag34)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.ParticleSystemStoreModule.SetupParticleSpawners(upcomingParticleSystems, upcomingParticleSpawners, upcomingParticles, this._upcomingUVMotionTexturePaths);
						this._assetUpdateSetupTimes[6] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool trailsRelatedUpdate2 = trailsRelatedUpdate;
					if (trailsRelatedUpdate2)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.TrailStoreModule.SetupTrailSettings(upcomingTrails);
						this._assetUpdateSetupTimes[7] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag35 = upcomingParticles != null | trailsRelatedUpdate;
					if (flag35)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.FXModule.CreateAtlasTextures(upcomingFXImageLocations, upcomingFXAtlasPixelsPerLevel);
						this._assetUpdateSetupTimes[8] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag36 = upcomingEntitiesImageLocations != null;
					if (flag36)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.EntityStoreModule.CreateAtlasTexture(upcomingEntitiesImageLocations, upcomingEntitiesAtlasPixelsPerLevel);
						this._assetUpdateSetupTimes[9] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool characterRelatedUpdate = characterRelatedUpdate;
					if (characterRelatedUpdate)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.EntityStoreModule.SetupModelsAndAnimations();
						this._assetUpdateSetupTimes[10] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					this._assetUpdateSetupTimer.Restart();
					this._gameInstance.UpdateAtlasSizes();
					this._assetUpdateSetupTimes[11] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					bool flag37 = characterRelatedUpdate | blockRelatedUpdate | trailsRelatedUpdate | particlesRelatedUpdate;
					if (flag37)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.EntityStoreModule.RebuildRenderers(false);
						this._assetUpdateSetupTimes[12] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag38 = trailsRelatedUpdate | particlesRelatedUpdate;
					if (flag38)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.ParticleSystemStoreModule.ResetParticleSystems(true);
						this._assetUpdateSetupTimes[17] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					this._assetUpdateSetupTimer.Restart();
					this._assetUpdateSetupTimes[13] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					bool flag39 = upcomingWorldMapImageLocations != null;
					if (flag39)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.WorldMapModule.BuildTextureAtlas(upcomingWorldMapImageLocations, upcomingWorldMapAtlasPixelsPerLevel);
						this._assetUpdateSetupTimes[14] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool skyRelatedUpdate = skyRelatedUpdate;
					if (skyRelatedUpdate)
					{
						this._assetUpdateSetupTimer.Restart();
						string sunChecksum;
						bool flag40 = this._gameInstance.HashesByServerAssetPath.TryGetValue("Sky/Sun.png", ref sunChecksum);
						if (flag40)
						{
							this._gameInstance.WeatherModule.SkyRenderer.LoadSunTexture(sunChecksum);
						}
						this._assetUpdateSetupTimes[15] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool flag41 = skyRelatedUpdate | screenEffectsRelatedUpdate;
					if (flag41)
					{
						this._assetUpdateSetupTimer.Restart();
						this._gameInstance.WeatherModule.RequestTextureUpdateFromWeather(this._gameInstance.WeatherModule.CurrentWeather, true);
						this._assetUpdateSetupTimes[16] = this._assetUpdateSetupTimer.ElapsedMilliseconds;
					}
					bool soundEffectRelatedUpdate2 = soundEffectRelatedUpdate;
					if (soundEffectRelatedUpdate2)
					{
						this._gameInstance.AmbienceFXModule.SetupAmbienceFXs(clonedAmbienceFXs, ambienceFXSettings);
						this._gameInstance.AmbienceFXModule.OnAmbienceFXChanged();
						this._gameInstance.AudioModule.OnSoundEffectCollectionChanged();
					}
					bool soundBankRelatedUpdate2 = soundBankRelatedUpdate;
					if (soundBankRelatedUpdate2)
					{
						this._gameInstance.AudioModule.SetupSoundBanks(upcomingWwiseIds);
						this._gameInstance.Engine.Audio.RefreshBanks();
					}
					this._gameInstance.App.DevTools.Info("[AssetUpdate] Assets: Finished in " + TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds) + ".");
					for (int l = 0; l < this._assetUpdateSetupTimes.Length; l++)
					{
						long num2 = this._assetUpdateSetupTimes[l];
						bool flag42 = num2 > 0L;
						if (flag42)
						{
							PacketHandler.AssetUpdateSetupSteps argument2 = (PacketHandler.AssetUpdateSetupSteps)l;
							PacketHandler.Logger.Info<PacketHandler.AssetUpdateSetupSteps, string>("[AssetUpdate] {0}: {1}", argument2, TimeHelper.FormatMillis(num2));
						}
					}
				}, false, false);
			});
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x000558D4 File Offset: 0x00053AD4
		public void ProcessPlaySoundEvent2DPacket(PlaySoundEvent2D packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			uint soundEventIndex = ResourceManager.GetNetworkWwiseId(packet.SoundEventIndex);
			bool flag = soundEventIndex == 0U;
			if (!flag)
			{
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					int playbackId = this._gameInstance.AudioModule.PlayLocalSoundEvent(soundEventIndex);
					this._gameInstance.AudioModule.ActionOnEvent(playbackId, 3);
				}, false, false);
			}
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x0005593C File Offset: 0x00053B3C
		public void ProcessPlaySoundEvent3DPacket(PlaySoundEvent3D packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			uint soundEventIndex = ResourceManager.GetNetworkWwiseId(packet.SoundEventIndex);
			bool flag = soundEventIndex == 0U;
			if (!flag)
			{
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					Vector3 position = new Vector3((float)packet.Position_.X, (float)packet.Position_.Y, (float)packet.Position_.Z);
					this._gameInstance.AudioModule.PlaySoundEvent(soundEventIndex, position, Vector3.Zero);
				}, false, false);
			}
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x000559B0 File Offset: 0x00053BB0
		public void ProcessPlaySoundEventEntityPacket(PlaySoundEventEntity packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			int networkId = packet.NetworkId;
			uint soundEventIndex = ResourceManager.GetNetworkWwiseId(packet.SoundEventIndex);
			bool flag = soundEventIndex == 0U;
			if (!flag)
			{
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.EntityStoreModule.QueueSoundEvent(soundEventIndex, networkId);
				}, false, false);
			}
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x00055A24 File Offset: 0x00053C24
		private void ProcessAuth2Packet(Auth2 packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup);
			sbyte[] array;
			sbyte[] array2;
			this._gameInstance.App.AuthManager.HandleAuth2(packet.NonceA, packet.Cert, out array, out array2);
			this._gameInstance.Connection.SendPacket(new Auth3(array, array2));
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x00055A78 File Offset: 0x00053C78
		private void ProcessAuth4Packet(Auth4 packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup);
			this._gameInstance.App.AuthManager.HandleAuth4(packet.Secret, packet.NonceB);
			this._gameInstance.Connection.SendPacket(new Auth5());
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x00055AC8 File Offset: 0x00053CC8
		private void ProcessAuth6Packet(Auth6 packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup);
			this._gameInstance.App.AuthManager.HandleAuth6();
			bool flag = this._gameInstance.Connection.Referral != null;
			if (flag)
			{
				this._gameInstance.Connection.SendPacket(this._gameInstance.Connection.Referral);
			}
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x00055B30 File Offset: 0x00053D30
		private void ProcessUpdateBlockTypesPacket(UpdateBlockTypes packet)
		{
			UpdateBlockTypes updateBlockTypes = new UpdateBlockTypes(packet);
			UpdateType type = updateBlockTypes.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 != 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateBlockTypes.GetType().Name, this._stage, updateBlockTypes.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] BlockTypes: Starting {0}", updateBlockTypes.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool atlasNeedsUpdate = updateBlockTypes.UpdateBlockTextures || updateBlockTypes.UpdateModelTextures;
				object setupLock = this._setupLock;
				ClientBlockType[] upcomingBlockTypes;
				byte[][] upcomingBlocksAtlasPixelsPerLevel;
				lock (setupLock)
				{
					foreach (KeyValuePair<int, BlockType> keyValuePair in updateBlockTypes.BlockTypes)
					{
						this._networkBlockTypes[keyValuePair.Key] = keyValuePair.Value;
						bool flag2 = keyValuePair.Key > this._highestReceivedBlockId;
						if (flag2)
						{
							this._highestReceivedBlockId = keyValuePair.Key;
						}
					}
					this._gameInstance.MapModule.PrepareBlockTypes(updateBlockTypes.BlockTypes, this._highestReceivedBlockId, atlasNeedsUpdate, ref this._upcomingBlockTypes, ref this._upcomingBlocksImageLocations, ref this._upcomingBlocksAtlasSize, out upcomingBlocksAtlasPixelsPerLevel, this._threadCancellationToken);
					bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					upcomingBlockTypes = new ClientBlockType[this._upcomingBlockTypes.Length];
					Array.Copy(this._upcomingBlockTypes, upcomingBlockTypes, this._upcomingBlockTypes.Length);
				}
				UpdateType updateType = updateBlockTypes.Type;
				bool updateModels = updateBlockTypes.UpdateModels;
				bool updateMapGeometry = updateBlockTypes.UpdateMapGeometry;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					bool atlasNeedsUpdate;
					bool updateModels;
					bool flag4 = atlasNeedsUpdate | updateModels | updateMapGeometry;
					PacketHandler.Logger.Info("[AssetUpdate] BlockTypes: Rebuild all chunks: {0}", flag4);
					this._gameInstance.MapModule.SetupBlockTypes(upcomingBlockTypes, flag4);
					atlasNeedsUpdate = atlasNeedsUpdate;
					if (atlasNeedsUpdate)
					{
						this._gameInstance.MapModule.TextureAtlas.UpdateTexture2DMipMaps(upcomingBlocksAtlasPixelsPerLevel);
						this._gameInstance.UpdateAtlasSizes();
					}
					updateModels = updateModels;
					if (updateModels)
					{
						this._gameInstance.EntityStoreModule.RebuildRenderers(true);
					}
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] BlockTypes: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.BlockTypes);
				foreach (KeyValuePair<int, BlockType> keyValuePair2 in updateBlockTypes.BlockTypes)
				{
					this._networkBlockTypes[keyValuePair2.Key] = keyValuePair2.Value;
					bool flag3 = keyValuePair2.Key > this._highestReceivedBlockId;
					if (flag3)
					{
						this._highestReceivedBlockId = keyValuePair2.Key;
					}
				}
				this._upcomingBlockTypes = new ClientBlockType[this._highestReceivedBlockId + 1];
				this._upcomingBlocksImageLocations = new Dictionary<string, MapModule.AtlasLocation>();
				this._upcomingBlocksAtlasSize = new Point(this._gameInstance.MapModule.TextureAtlas.Width, 0);
				this.FinishedReceivedAssetType(PacketHandler.AssetType.BlockTypes);
			}
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x00055E84 File Offset: 0x00054084
		private void ProcessUpdateBlockHitboxesPacket(UpdateBlockHitboxes packet)
		{
			UpdateBlockHitboxes updateBlockHitboxes = new UpdateBlockHitboxes(packet);
			UpdateType type = updateBlockHitboxes.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateBlockHitboxes.GetType().Name, this._stage, updateBlockHitboxes.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] BlockHitboxes: Starting {0}", updateBlockHitboxes.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = updateBlockHitboxes.MaxId > this._upcomingServerSettings.BlockHitboxes.Length;
				if (flag)
				{
					BlockHitbox[] array = new BlockHitbox[updateBlockHitboxes.MaxId];
					Array.Copy(this._upcomingServerSettings.BlockHitboxes, array, this._upcomingServerSettings.BlockHitboxes.Length);
					this._upcomingServerSettings.BlockHitboxes = array;
				}
				bool flag2 = updateBlockHitboxes.Type == 1;
				if (flag2)
				{
					foreach (KeyValuePair<int, Hitbox[]> keyValuePair in updateBlockHitboxes.BlockHitboxes)
					{
						Hitbox[] value = keyValuePair.Value;
						BoundingBox[] array2 = new BoundingBox[value.Length];
						for (int i = 0; i < value.Length; i++)
						{
							Hitbox hitbox = value[i];
							array2[i] = new BoundingBox(new Vector3(hitbox.MinX, hitbox.MinY, hitbox.MinZ), new Vector3(hitbox.MaxX, hitbox.MaxY, hitbox.MaxZ));
						}
						this._upcomingServerSettings.BlockHitboxes[keyValuePair.Key] = new BlockHitbox(array2);
					}
				}
				else
				{
					foreach (KeyValuePair<int, Hitbox[]> keyValuePair2 in updateBlockHitboxes.BlockHitboxes)
					{
						this._upcomingServerSettings.BlockHitboxes[keyValuePair2.Key] = null;
					}
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = updateBlockHitboxes.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] BlockHitboxes: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.BlockHitboxes);
				bool flag3 = this._upcomingServerSettings.BlockHitboxes == null;
				if (flag3)
				{
					this._upcomingServerSettings.BlockHitboxes = new BlockHitbox[packet.MaxId];
				}
				foreach (KeyValuePair<int, Hitbox[]> keyValuePair3 in updateBlockHitboxes.BlockHitboxes)
				{
					Hitbox[] value2 = keyValuePair3.Value;
					BoundingBox[] array3 = new BoundingBox[value2.Length];
					for (int j = 0; j < value2.Length; j++)
					{
						Hitbox hitbox2 = value2[j];
						array3[j] = new BoundingBox(new Vector3(hitbox2.MinX, hitbox2.MinY, hitbox2.MinZ), new Vector3(hitbox2.MaxX, hitbox2.MaxY, hitbox2.MaxZ));
					}
					this._upcomingServerSettings.BlockHitboxes[keyValuePair3.Key] = new BlockHitbox(array3);
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.BlockHitboxes);
			}
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x0005623C File Offset: 0x0005443C
		private void ProcessUpdateBlockSoundSets(UpdateBlockSoundSets packet)
		{
			UpdateBlockSoundSets updateBlockSoundSets = new UpdateBlockSoundSets(packet);
			UpdateType type = updateBlockSoundSets.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateBlockSoundSets.GetType().Name, this._stage, updateBlockSoundSets.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] BlockSounndSets: Starting {0}", updateBlockSoundSets.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = updateBlockSoundSets.Type == 1;
				if (flag)
				{
					bool flag2 = updateBlockSoundSets.MaxId > this._upcomingServerSettings.BlockSoundSets.Length;
					if (flag2)
					{
						Array.Resize<BlockSoundSet>(ref this._upcomingServerSettings.BlockSoundSets, updateBlockSoundSets.MaxId);
					}
					foreach (KeyValuePair<int, BlockSoundSet> keyValuePair in updateBlockSoundSets.BlockSoundSets)
					{
						this._upcomingServerSettings.BlockSoundSets[keyValuePair.Key] = new BlockSoundSet(keyValuePair.Value);
					}
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = updateBlockSoundSets.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] BlockSoundSets: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.BlockSoundSets);
				this._upcomingServerSettings.BlockSoundSets = new BlockSoundSet[updateBlockSoundSets.MaxId];
				foreach (KeyValuePair<int, BlockSoundSet> keyValuePair2 in updateBlockSoundSets.BlockSoundSets)
				{
					this._upcomingServerSettings.BlockSoundSets[keyValuePair2.Key] = new BlockSoundSet(keyValuePair2.Value);
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.BlockSoundSets);
			}
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x00056478 File Offset: 0x00054678
		private void ProcessUpdateBlockParticleSets(UpdateBlockParticleSets packet)
		{
			UpdateBlockParticleSets updateBlockParticleSets = new UpdateBlockParticleSets(packet);
			UpdateType type = updateBlockParticleSets.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateBlockParticleSets.GetType().Name, this._stage, updateBlockParticleSets.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] BlockParticleSets: Starting {0}", updateBlockParticleSets.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = updateBlockParticleSets.Type == 1;
				if (flag)
				{
					foreach (KeyValuePair<string, BlockParticleSet> keyValuePair in updateBlockParticleSets.BlockParticleSets)
					{
						ClientBlockParticleSet value = new ClientBlockParticleSet();
						ParticleProtocolInitializer.Initialize(keyValuePair.Value, ref value);
						this._upcomingServerSettings.BlockParticleSets[keyValuePair.Key] = value;
					}
				}
				else
				{
					foreach (KeyValuePair<string, BlockParticleSet> keyValuePair2 in updateBlockParticleSets.BlockParticleSets)
					{
						this._upcomingServerSettings.BlockParticleSets.Remove(keyValuePair2.Key);
					}
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = updateBlockParticleSets.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.EntityStoreModule.ResetMovementParticleSystems();
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] BlockParticleSets: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.BlockParticleSets);
				bool flag2 = this._upcomingServerSettings.BlockParticleSets == null;
				if (flag2)
				{
					this._upcomingServerSettings.BlockParticleSets = new Dictionary<string, ClientBlockParticleSet>();
				}
				foreach (KeyValuePair<string, BlockParticleSet> keyValuePair3 in updateBlockParticleSets.BlockParticleSets)
				{
					ClientBlockParticleSet value2 = new ClientBlockParticleSet();
					ParticleProtocolInitializer.Initialize(keyValuePair3.Value, ref value2);
					this._upcomingServerSettings.BlockParticleSets[keyValuePair3.Key] = value2;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.BlockParticleSets);
			}
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x0005670C File Offset: 0x0005490C
		private void ProcessUpdateBlockGroups(UpdateBlockGroups packet)
		{
			UpdateBlockGroups updateBlockGroups = new UpdateBlockGroups(packet);
			UpdateType type = updateBlockGroups.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType - 1 > 1)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this.ReceivedAssetType(PacketHandler.AssetType.BlockTypes | PacketHandler.AssetType.Items);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.BlockTypes | PacketHandler.AssetType.Items);
				bool flag = this._upcomingServerSettings.BlockGroups == null;
				if (flag)
				{
					this._upcomingServerSettings.BlockGroups = new Dictionary<string, BlockGroup>();
				}
				foreach (KeyValuePair<string, BlockGroup> keyValuePair in updateBlockGroups.Groups)
				{
					this._upcomingServerSettings.BlockGroups.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x000567F0 File Offset: 0x000549F0
		private void ProcessSupportValidationResponse(SupportValidationResponse response)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.InteractionModule.BlockPreview.HandleSupportValidationResponse(response);
			}, false, false);
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x00056840 File Offset: 0x00054A40
		private void ProcessChunkPartPacket(ChunkPart packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			Buffer.BlockCopy(packet.Part, 0, this._compressedChunkBuffer, this._compressedChunkBufferPosition, packet.Part.Length);
			this._compressedChunkBufferPosition += packet.Part.Length;
		}

		// Token: 0x0600348D RID: 13453 RVA: 0x00056890 File Offset: 0x00054A90
		private void ProcessSetChunkPacket(SetChunk packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int compressedChunkBufferPosition = this._compressedChunkBufferPosition;
			this._compressedChunkBufferPosition = 0;
			int x = packet.X;
			int y = packet.Y;
			int z = packet.Z;
			bool flag = compressedChunkBufferPosition == 0;
			if (flag)
			{
				this._gameInstance.MapModule.SetChunkBlocks(x, y, z, null, this._upcomingBlockTypes.Length - 1, (byte[])packet.LocalLight, (byte[])packet.GlobalLight);
			}
			else
			{
				Buffer.BlockCopy(this._compressedChunkBuffer, 0, this._decompressedChunkBuffer, 0, this._decompressedChunkBuffer.Length);
				this._gameInstance.MapModule.SetChunkBlocks(x, y, z, this._decompressedChunkBuffer, this._upcomingBlockTypes.Length - 1, (byte[])packet.LocalLight, (byte[])packet.GlobalLight);
			}
		}

		// Token: 0x0600348E RID: 13454 RVA: 0x00056964 File Offset: 0x00054B64
		private void ProcessSetChunkHeightmapPacket(SetChunkHeightmap packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int x = packet.X;
			int z = packet.Z;
			ushort[] array3;
			using (MemoryStream memoryStream = new MemoryStream((byte[])packet.Heightmap))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					ushort num = binaryReader.ReadUInt16();
					ushort[] array = new ushort[(int)num];
					for (int i = 0; i < (int)num; i++)
					{
						array[i] = binaryReader.ReadUInt16();
					}
					int num2 = binaryReader.ReadInt32();
					byte[] array2 = new byte[num2];
					binaryReader.Read(array2, 0, num2);
					this._bitFieldArr.Set(array2);
					array3 = new ushort[1024];
					for (int j = 0; j < 1024; j++)
					{
						array3[j] = array[(int)((ushort)this._bitFieldArr.Get(j))];
					}
				}
			}
			this._gameInstance.MapModule.SetChunkColumnHeights(x, z, array3);
		}

		// Token: 0x0600348F RID: 13455 RVA: 0x00056A90 File Offset: 0x00054C90
		private void ProcessSetChunkTintmapPacket(SetChunkTintmap packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int x = packet.X;
			int z = packet.Z;
			uint[] array3;
			using (MemoryStream memoryStream = new MemoryStream((byte[])packet.Tintmap))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					ushort num = binaryReader.ReadUInt16();
					uint[] array = new uint[(int)num];
					for (int i = 0; i < (int)num; i++)
					{
						array[i] = binaryReader.ReadUInt32();
					}
					int num2 = binaryReader.ReadInt32();
					byte[] array2 = new byte[num2];
					binaryReader.Read(array2, 0, num2);
					this._bitFieldArr.Set(array2);
					array3 = new uint[1024];
					for (int j = 0; j < 1024; j++)
					{
						array3[j] = array[(int)this._bitFieldArr.Get(j)];
					}
				}
			}
			this._gameInstance.MapModule.SetChunkColumnTints(x, z, array3);
		}

		// Token: 0x06003490 RID: 13456 RVA: 0x00056BBC File Offset: 0x00054DBC
		private void ProcessSetChunkEnvironmentsPacket(SetChunkEnvironments packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int x = packet.X;
			int z = packet.Z;
			ushort[][] array;
			using (MemoryStream memoryStream = new MemoryStream((byte[])packet.Environments))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					array = new ushort[1024][];
					for (int i = 0; i < 1024; i++)
					{
						short num = binaryReader.ReadInt16();
						ushort[] array2 = new ushort[(int)(num * 2)];
						for (int j = 0; j < (int)num; j++)
						{
							array2[j * 2] = binaryReader.ReadUInt16();
							array2[j * 2 + 1] = binaryReader.ReadUInt16();
						}
						array[i] = array2;
					}
				}
			}
			this._gameInstance.MapModule.SetChunkColumnEnvironments(x, z, array);
		}

		// Token: 0x06003491 RID: 13457 RVA: 0x00056CC4 File Offset: 0x00054EC4
		private void ProcessSetBlockPacket(ServerSetBlock packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			bool isEnabled = this._gameInstance.InteractionModule.BlockBreakHealth.IsEnabled;
			if (isEnabled)
			{
				this._gameInstance.InteractionModule.BlockBreakHealth.UpdateHealth(-1, packet.X, packet.Y, packet.Z, 1f, 1f);
			}
			this._gameInstance.MapModule.SetBlock(packet.X, packet.Y, packet.Z, packet.BlockId, packet.InteractionStateSound);
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x00056D58 File Offset: 0x00054F58
		private void ProcessUpdateBlockDamagePacket(UpdateBlockDamage packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int block = this._gameInstance.MapModule.GetBlock(packet.BlockPosition_.X, packet.BlockPosition_.Y, packet.BlockPosition_.Z, 1);
			ClientBlockType blockType = this._gameInstance.MapModule.ClientBlockTypes[block];
			bool flag = blockType.DrawType > 0;
			if (flag)
			{
				bool isEnabled = this._gameInstance.InteractionModule.BlockBreakHealth.IsEnabled;
				if (isEnabled)
				{
					this._gameInstance.InteractionModule.BlockBreakHealth.UpdateHealth(block, packet.BlockPosition_.X, packet.BlockPosition_.Y, packet.BlockPosition_.Z, 1f, packet.Damage);
				}
			}
			bool flag2 = packet.Delta >= 0f;
			if (!flag2)
			{
				bool flag3 = blockType.BlockParticleSetId != null;
				if (flag3)
				{
					Vector3 blockPosition = new Vector3((float)packet.BlockPosition_.X, (float)packet.BlockPosition_.Y, (float)packet.BlockPosition_.Z);
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						ParticleSystemProxy particleSystemProxy;
						bool flag4 = this._gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(blockPosition, blockType, ClientBlockParticleEvent.Hit, out particleSystemProxy, true, false);
						if (flag4)
						{
							particleSystemProxy.Position = blockPosition + new Vector3(0.5f) + particleSystemProxy.Position;
						}
					}, false, false);
				}
			}
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x00056ECC File Offset: 0x000550CC
		private void ProcessUnloadChunk(UnloadChunk packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.MapModule.UnloadChunkColumn(packet.ChunkX, packet.ChunkZ);
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x00056EF8 File Offset: 0x000550F8
		private void ProcessViewRadiusPacket(ViewRadius packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int viewRadius = packet.ViewRadius_;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.MapModule.MaxServerViewRadius = viewRadius;
			}, false, false);
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x00056F50 File Offset: 0x00055150
		private void ProcessSetUpdateRatePacket(SetUpdateRate packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			bool flag = packet.UpdatesPerSecond <= 0 || packet.UpdatesPerSecond > 2048;
			if (flag)
			{
				throw new ArgumentException(string.Format("UpdatesPerSecond is out of bounds (<=0 or >2048): ${0}", packet.UpdatesPerSecond));
			}
			int updatesPerSecond = packet.UpdatesPerSecond;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.ServerUpdatesPerSecond = updatesPerSecond;
			}, false, false);
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x00056FE0 File Offset: 0x000551E0
		private void ProcessUpdateFeaturesPacket(UpdateFeatures packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				foreach (KeyValuePair<ClientFeature, bool> keyValuePair in packet.Features)
				{
					this._gameInstance.ClientFeatureModule.SetFeatureEnabled(keyValuePair.Key, keyValuePair.Value);
				}
			}, false, false);
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x00057030 File Offset: 0x00055230
		private void ProcessSetTimeDilationPacket(SetTimeDilation packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			bool flag = (double)packet.TimeDilation <= 0.01 || packet.TimeDilation > 4f;
			if (flag)
			{
				throw new ArgumentException(string.Format("TimeDilation is out of bounds (<=0.01 or >4): ${0}", packet.TimeDilation));
			}
			float timeDilation = packet.TimeDilation;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.TimeDilationModifier = timeDilation;
			}, false, false);
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x000570C8 File Offset: 0x000552C8
		private void ProcessSetClientIdPacket(SetClientId packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int clientId = packet.ClientId;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.SetLocalPlayerId(clientId);
			}, false, false);
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x00057120 File Offset: 0x00055320
		private void ProcessPingPacket(Ping packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup | PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			DateTime utcNow = DateTime.UtcNow;
			InstantData time = new InstantData(packet.Time);
			int packetId = packet.Id;
			int lastPingValueDirect = packet.LastPingValueDirect;
			this._gameInstance.TimeModule.UpdatePing(time, utcNow, 1, lastPingValueDirect);
			this._gameInstance.Connection.SendPacketImmediate(new Pong(packetId, TimeHelper.DateTimeToInstantData(utcNow), 1, (short)this._packets.Count));
			int lastPingValueTick = packet.LastPingValueTick;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				DateTime utcNow2 = DateTime.UtcNow;
				this._gameInstance.TimeModule.UpdatePing(time, utcNow2, 2, lastPingValueTick);
				this._gameInstance.Connection.SendPacket(new Pong(packetId, TimeHelper.DateTimeToInstantData(utcNow2), 2, (short)this._packets.Count));
			}, false, false);
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000571E8 File Offset: 0x000553E8
		private void ProcessReferral(ClientReferral packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup | PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			string hostname = string.Copy(packet.HostTo.Host);
			short port = packet.HostTo.Port;
			PacketHandler.Logger.Info<string, short, bool>("Wanted referral to {0} with port {1} and HDC {2}", hostname, port, packet.HardDisconnect);
			PkixCertPath pkixCertPath = new PkixCertPath(new MemoryStream((byte[])packet.Referral.CertPath), "PEM");
			for (int i = 0; i < pkixCertPath.Certificates.Count; i++)
			{
				PacketHandler.Logger.Info<int, object>("Cert {0}: {1}", i, pkixCertPath.Certificates[i]);
			}
			bool hardDisconnect = packet.HardDisconnect;
			if (hardDisconnect)
			{
				App app = this._gameInstance.App;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					app.GameLoading.Open(hostname, (int)port, new AppMainMenu.MainMenuPage?(AppMainMenu.MainMenuPage.Minigames));
				}, false, false);
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000572FC File Offset: 0x000554FC
		public PacketHandler(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
			this._threadCancellationToken = this._threadCancellationTokenSource.Token;
			this._thread = new Thread(new ThreadStart(this.ProcessPacketsThreadStart))
			{
				Name = "BackgroundPacketHandler",
				IsBackground = true
			};
			this._thread.Start();
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x0600349C RID: 13468 RVA: 0x0005746D File Offset: 0x0005566D
		public bool IsOnThread
		{
			get
			{
				return ThreadHelper.IsOnThread(this._thread);
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x0005747C File Offset: 0x0005567C
		protected override void DoDispose()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._threadCancellationTokenSource.Cancel();
			this._thread.Join();
			bool flag = this._blobFileStream != null;
			if (flag)
			{
				this._blobFileStream.Close();
				this._blobFileStream = null;
				try
				{
					File.Delete(Paths.TempAssetDownload);
				}
				catch
				{
					throw;
				}
			}
			this._threadCancellationTokenSource.Dispose();
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x00057500 File Offset: 0x00055700
		public void Receive(byte[] buffer, int payloadLength)
		{
			using (ProtoBinaryReader protoBinaryReader = ProtoBinaryReader.Create(buffer, payloadLength))
			{
				this.Receive(PacketReader.ReadPacket(protoBinaryReader));
			}
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x00057544 File Offset: 0x00055744
		public void Receive(ProtoPacket packet)
		{
			bool flag = packet.GetId() == 1;
			if (flag)
			{
				App app = this._gameInstance.App;
				string reason = ((Disconnect)packet).Reason;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance.Engine, delegate
				{
					app.Disconnection.SetReason(reason);
				}, true, false);
			}
			else
			{
				bool flag2 = packet.GetId() == 2;
				if (flag2)
				{
					Ping ping = (Ping)packet;
					DateTime utcNow = DateTime.UtcNow;
					this._gameInstance.TimeModule.UpdatePing(ping.Time, utcNow, 0, ping.LastPingValueRaw);
					this._gameInstance.Connection.SendPacketImmediate(new Pong(ping.Id, TimeHelper.DateTimeToInstantData(utcNow), 0, (short)this._packets.Count));
				}
				this._packets.Add(packet, this._threadCancellationToken);
			}
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x00057638 File Offset: 0x00055838
		private void ProcessPacketsThreadStart()
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._thread));
			Stopwatch stopwatch = Stopwatch.StartNew();
			ProtoPacket protoPacket = null;
			while (!this._threadCancellationToken.IsCancellationRequested)
			{
				try
				{
					protoPacket = this._packets.Take(this._threadCancellationToken);
				}
				catch (OperationCanceledException)
				{
					break;
				}
				stopwatch.Restart();
				this.ProcessPacket(protoPacket);
				ref ConnectionToServer.PacketStat ptr = ref this._gameInstance.Connection.PacketStats[protoPacket.GetId()];
				bool flag = ptr.Name == null;
				if (flag)
				{
					ptr.Name = protoPacket.GetType().Name;
				}
				ptr.AddReceivedTime(stopwatch.ElapsedTicks);
			}
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x00057700 File Offset: 0x00055900
		private void ReceivedAssetType(PacketHandler.AssetType assetType)
		{
			long elapsedMilliseconds = this._connectionStopwatch.ElapsedMilliseconds;
			PacketHandler.Logger.Info<PacketHandler.AssetType, long>("Received AssetType {0} at {1}ms", assetType, elapsedMilliseconds);
			this._receivedAssetTypes |= assetType;
			this._lastAssetReceivedMs = elapsedMilliseconds;
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x00057741 File Offset: 0x00055941
		private void FinishedReceivedAssetType(PacketHandler.AssetType assetType)
		{
			PacketHandler.Logger.Info<PacketHandler.AssetType, long>("Finished handling AssetType {0} took {1}ms", assetType, this._connectionStopwatch.ElapsedMilliseconds - this._lastAssetReceivedMs);
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x00057768 File Offset: 0x00055968
		private void ValidateReceivedAssets()
		{
			string text = "";
			foreach (object obj in Enum.GetValues(typeof(PacketHandler.AssetType)))
			{
				bool flag = this._receivedAssetTypes.HasFlag((PacketHandler.AssetType)obj);
				if (!flag)
				{
					bool flag2 = text.Length <= 0;
					if (flag2)
					{
						text += string.Format("{0}", obj);
					}
					else
					{
						text += string.Format(", {0}", obj);
					}
				}
			}
			bool flag3 = text.Length > 0;
			if (flag3)
			{
				throw new Exception("We have not received the asset types of " + text + ".");
			}
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x0005784C File Offset: 0x00055A4C
		private void SetStage(PacketHandler.ConnectionStage stage)
		{
			PacketHandler.Logger.Info<PacketHandler.ConnectionStage, PacketHandler.ConnectionStage, long>("Stage {0} -> {1} took {2}ms", this._stage, stage, this._stageStopwatch.ElapsedMilliseconds);
			this._stage = stage;
			this._stageStopwatch.Restart();
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x00057884 File Offset: 0x00055A84
		private void ValidateStage(PacketHandler.ConnectionStage stageFlags)
		{
			bool flag = !stageFlags.HasFlag(this._stage);
			if (flag)
			{
				throw new Exception(string.Format("Received {0} at {1} connection stage but expected it only during {2}.", this._stageValidationPacketId, this._stage, stageFlags));
			}
			this._stageValidationPacketId = string.Empty;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000578E4 File Offset: 0x00055AE4
		private bool ValidateEntityId(int id)
		{
			return id >= 0;
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x00057900 File Offset: 0x00055B00
		private bool ValidateFloat(float number, float min = float.NegativeInfinity, float max = float.PositiveInfinity)
		{
			bool flag = float.IsNaN(number);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = number < min || number > max;
				result = !flag2;
			}
			return result;
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x00057934 File Offset: 0x00055B34
		private void ProcessPacket(ProtoPacket packet)
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._thread));
			this._stageValidationPacketId = packet.GetType().Name;
			switch (packet.GetId())
			{
			case 2:
				this.ProcessPingPacket((Ping)packet);
				goto IL_D78;
			case 4:
				this.ProcessAddToPlayerListPacket((AddToPlayerList)packet);
				goto IL_D78;
			case 5:
				this.ProcessApplyKnockback((ApplyKnockback)packet);
				goto IL_D78;
			case 54:
				this.ProcessAssetFinalizePacket((AssetFinalize)packet);
				goto IL_D78;
			case 55:
				this.ProcessAssetInitializePacket((AssetInitialize)packet);
				goto IL_D78;
			case 56:
				this.ProcessAssetPartPacket((AssetPart)packet);
				goto IL_D78;
			case 58:
				this.ProcessAuth2Packet((Auth2)packet);
				goto IL_D78;
			case 60:
				this.ProcessAuth4Packet((Auth4)packet);
				goto IL_D78;
			case 62:
				this.ProcessAuth6Packet((Auth6)packet);
				goto IL_D78;
			case 67:
				this.ProcessBuilderToolHideAnchorPacket((BuilderToolHideAnchors)packet);
				goto IL_D78;
			case 73:
				this.ProcessBuilderToolSelectionToolReplyWithClipboard((BuilderToolSelectionToolReplyWithClipboard)packet);
				goto IL_D78;
			case 78:
				this.ProcessBuilderToolShowAnchorPacket((BuilderToolShowAnchor)packet);
				goto IL_D78;
			case 80:
				this.ProcessCameraEffect((CameraShakeEffect)packet);
				goto IL_D78;
			case 81:
				this.ProcessCancelInteractionChain((CancelInteractionChain)packet);
				goto IL_D78;
			case 82:
				this.ProcessChangeVelocity((ChangeVelocity)packet);
				goto IL_D78;
			case 85:
				this.ProcessChunkPartPacket((ChunkPart)packet);
				goto IL_D78;
			case 86:
				this.ProcessClearEditorTimeOverride((ClearEditorTimeOverride)packet);
				goto IL_D78;
			case 87:
				this.ProcessClearPlayerListPacket((ClearPlayerList)packet);
				goto IL_D78;
			case 90:
				this.ProcessReferral((ClientReferral)packet);
				goto IL_D78;
			case 91:
				this.ProcessClientTeleportPacket((ClientTeleport)packet);
				goto IL_D78;
			case 92:
				this.ProcessCloseWindow((CloseWindow)packet);
				goto IL_D78;
			case 93:
				this.ProcessCustomHudPacket((CustomHud)packet);
				goto IL_D78;
			case 94:
				this.ProcessCustomPagePacket((CustomPage)packet);
				goto IL_D78;
			case 96:
				this.ProcessDamageInfo((DamageInfo)packet);
				goto IL_D78;
			case 97:
				this.ProcessDismountNpc((DismountNPC)packet);
				goto IL_D78;
			case 98:
				this.ProcessDisplayDebug((DisplayDebug)packet);
				goto IL_D78;
			case 100:
				this.ProcessEditorBlocksChangePacket((EditorBlocksChange)packet);
				goto IL_D78;
			case 102:
				this.ProcessEntityUpdatesPacket((EntityUpdates)packet);
				goto IL_D78;
			case 103:
				this.ProcessFailureReply((FailureReply)packet);
				goto IL_D78;
			case 104:
				this.ProcessHideEventTitlePacket((HideEventTitle)packet);
				goto IL_D78;
			case 106:
				this.ProcessJoinWorldPacket((JoinWorld)packet);
				goto IL_D78;
			case 108:
				this.ProcessKillFeedMessage((KillFeedMessage)packet);
				goto IL_D78;
			case 110:
				this.ProcessMountNpc((MountNPC)packet);
				goto IL_D78;
			case 113:
				this.ProcessNotificationPacket((Notification)packet);
				goto IL_D78;
			case 115:
				this.ProcessOpenWindow((OpenWindow)packet);
				goto IL_D78;
			case 117:
				this.ProcessPlayAnimation((PlayAnimation)packet);
				goto IL_D78;
			case 118:
				this.ProcessPlayInteractionFor((PlayInteractionFor)packet);
				goto IL_D78;
			case 119:
				this.ProcessPlaySoundEvent2DPacket((PlaySoundEvent2D)packet);
				goto IL_D78;
			case 120:
				this.ProcessPlaySoundEvent3DPacket((PlaySoundEvent3D)packet);
				goto IL_D78;
			case 121:
				this.ProcessPlaySoundEventEntityPacket((PlaySoundEventEntity)packet);
				goto IL_D78;
			case 125:
				this.ProcessRemoveAssetsPacket((RemoveAssets)packet);
				goto IL_D78;
			case 126:
				this.ProcessRemoveFromPlayerListPacket((RemoveFromPlayerList)packet);
				goto IL_D78;
			case 128:
				this.ProcessRequestCommonAssetsRebuildPacket((RequestCommonAssetsRebuild)packet);
				goto IL_D78;
			case 130:
				this.ProcessRequestServerAccess((RequestServerAccess)packet);
				goto IL_D78;
			case 131:
				this.ProcessResetUserInterfaceState((ResetUserInterfaceState)packet);
				goto IL_D78;
			case 132:
				this.ProcessReticleEvent((ReticleEvent)packet);
				goto IL_D78;
			case 136:
				this.ProcessServerInfoPacket((ServerInfo)packet);
				goto IL_D78;
			case 137:
				this.ProcessServerMessagePacket((ServerMessage)packet);
				goto IL_D78;
			case 138:
				this.ProcessSetBlockPacket((ServerSetBlock)packet);
				goto IL_D78;
			case 139:
				this.ProcessServerTagsPacket((ServerTags)packet);
				goto IL_D78;
			case 140:
				this.ProcessSetActiveSlot((SetActiveSlot)packet);
				goto IL_D78;
			case 141:
				this.ProcessSetChunkPacket((SetChunk)packet);
				goto IL_D78;
			case 142:
				this.ProcessSetChunkEnvironmentsPacket((SetChunkEnvironments)packet);
				goto IL_D78;
			case 143:
				this.ProcessSetChunkHeightmapPacket((SetChunkHeightmap)packet);
				goto IL_D78;
			case 144:
				this.ProcessSetChunkTintmapPacket((SetChunkTintmap)packet);
				goto IL_D78;
			case 145:
				this.ProcessSetClientIdPacket((SetClientId)packet);
				goto IL_D78;
			case 147:
				this.ProcessSetEntitySeed((SetEntitySeed)packet);
				goto IL_D78;
			case 148:
				this.ProcessSetGameModePacket((SetGameMode)packet);
				goto IL_D78;
			case 149:
				this.ProcessSetMachinimaActorModelPacket((SetMachinimaActorModel)packet);
				goto IL_D78;
			case 150:
				this.ProcessSetMovementStates((SetMovementStates)packet);
				goto IL_D78;
			case 151:
				this.ProcessSetPagePacket((SetPage)packet);
				goto IL_D78;
			case 152:
				this.ProcessSetServerCamera((SetServerCamera)packet);
				goto IL_D78;
			case 153:
				this.ProcessSetTimeDilationPacket((SetTimeDilation)packet);
				goto IL_D78;
			case 154:
				this.ProcessSetUpdateRatePacket((SetUpdateRate)packet);
				goto IL_D78;
			case 156:
				this.ProcessShowEventTitlePacket((ShowEventTitle)packet);
				goto IL_D78;
			case 160:
				this.ProcessSpawnBlockParticleSystem((SpawnBlockParticleSystem)packet);
				goto IL_D78;
			case 161:
				this.ProcessSpawnModelParticles((SpawnModelParticles)packet);
				goto IL_D78;
			case 162:
				this.ProcessSpawnParticleSystemPacket((SpawnParticleSystem)packet);
				goto IL_D78;
			case 164:
				this.ProcessSuccessReply((SuccessReply)packet);
				goto IL_D78;
			case 166:
				this.ProcessSupportValidationResponse((SupportValidationResponse)packet);
				goto IL_D78;
			case 168:
				this.ProcessSyncInteractionChain((SyncInteractionChain)packet);
				goto IL_D78;
			case 172:
				this.ProcessTrackOrUpdateObjective((TrackOrUpdateObjective)packet);
				goto IL_D78;
			case 175:
				this.ProcessTriggerEditorRequestBlockReply((TriggerEditorRequestBlockReply)packet);
				goto IL_D78;
			case 177:
				this.ProcessTriggerEditorRequestScriptReply((TriggerEditorRequestScriptReply)packet);
				goto IL_D78;
			case 179:
				this.ProcessTriggerEditorRequestScriptsReply((TriggerEditorRequestScriptsReply)packet);
				goto IL_D78;
			case 181:
				this.ProcessTriggerEditorUpdateBlockReply((TriggerEditorUpdateBlockReply)packet);
				goto IL_D78;
			case 183:
				this.ProcessTriggerEditorUpdateScriptReply((TriggerEditorUpdateScriptReply)packet);
				goto IL_D78;
			case 184:
				this.ProcessUnloadChunk((UnloadChunk)packet);
				goto IL_D78;
			case 185:
				this.ProcessUntrackObjective((UntrackObjective)packet);
				goto IL_D78;
			case 186:
				this.ProcessUpdateAmbienceFXPacket((UpdateAmbienceFX)packet);
				goto IL_D78;
			case 187:
				this.ProcessUpdateBlockDamagePacket((UpdateBlockDamage)packet);
				goto IL_D78;
			case 188:
				this.ProcessUpdateBlockGroups((UpdateBlockGroups)packet);
				goto IL_D78;
			case 189:
				this.ProcessUpdateBlockHitboxesPacket((UpdateBlockHitboxes)packet);
				goto IL_D78;
			case 190:
				this.ProcessUpdateBlockParticleSets((UpdateBlockParticleSets)packet);
				goto IL_D78;
			case 191:
				this.ProcessUpdateBlockSoundSets((UpdateBlockSoundSets)packet);
				goto IL_D78;
			case 192:
				this.ProcessUpdateBlockTypesPacket((UpdateBlockTypes)packet);
				goto IL_D78;
			case 193:
				this.ProcessCameraShakeProfiles((UpdateCameraShake)packet);
				goto IL_D78;
			case 194:
				this.ProcessUpdateEditorTimeOverride((UpdateEditorTimeOverride)packet);
				goto IL_D78;
			case 195:
				this.ProcessUpdateEditorWeatherOverride((UpdateEditorWeatherOverride)packet);
				goto IL_D78;
			case 196:
				this.ProcessUpdateEntityEffectsPacket((UpdateEntityEffects)packet);
				goto IL_D78;
			case 197:
				this.ProcessUpdateEntityStatTypes((UpdateEntityStatTypes)packet);
				goto IL_D78;
			case 198:
				this.ProcessUpdateEntityUIComponents((UpdateEntityUIComponents)packet);
				goto IL_D78;
			case 199:
				this.ProcessUpdateEnvironmentsPacket((UpdateEnvironments)packet);
				goto IL_D78;
			case 200:
				this.ProcessUpdateFeaturesPacket((UpdateFeatures)packet);
				goto IL_D78;
			case 201:
				this.ProcessUpdateFieldcraftCategoriesPacket((UpdateFieldcraftCategories)packet);
				goto IL_D78;
			case 202:
				this.ProcessUpdateFluidFXPacket((UpdateFluidFX)packet);
				goto IL_D78;
			case 203:
				this.ProcessUpdateHitboxCollisionConfig((UpdateHitboxCollisionConfig)packet);
				goto IL_D78;
			case 204:
				this.ProcessUpdateImmersiveViewPacket((UpdateImmersiveView)packet);
				goto IL_D78;
			case 205:
				this.ProcessUpdateInteractions((UpdateInteractions)packet);
				goto IL_D78;
			case 206:
				this.ProcessUpdateItemCategoriesPacket((UpdateItemCategories)packet);
				goto IL_D78;
			case 207:
				this.ProcessUpdateItemPlayerAnimationsPacket((UpdateItemPlayerAnimations)packet);
				goto IL_D78;
			case 208:
				this.ProcessUpdateItemQualitiesPacket((UpdateItemQualities)packet);
				goto IL_D78;
			case 209:
				this.ProcessUpdateItemReticles((UpdateItemReticles)packet);
				goto IL_D78;
			case 210:
				this.ProcessUpdateItemsPacket((UpdateItems)packet);
				goto IL_D78;
			case 211:
				this.ProcessUpdateKnownRecipes((UpdateKnownRecipes)packet);
				goto IL_D78;
			case 213:
				this.ProcessUpdateMachinimaScene((UpdateMachinimaScene)packet);
				goto IL_D78;
			case 214:
				this.ProcessUpdateModelVFXsPacket((UpdateModelvfxs)packet);
				goto IL_D78;
			case 215:
				this.ProcessUpdateMovementSettings((UpdateMovementSettings)packet);
				goto IL_D78;
			case 216:
				this.ProcessUpdateObjectiveTask((UpdateObjectiveTask)packet);
				goto IL_D78;
			case 218:
				this.ProcessUpdateParticleSpawnersPacket((UpdateParticleSpawners)packet);
				goto IL_D78;
			case 219:
				this.ProcessUpdateParticleSystemsPacket((UpdateParticleSystems)packet);
				goto IL_D78;
			case 220:
				this.ProcessInventoryPacket((UpdatePlayerInventory)packet);
				goto IL_D78;
			case 221:
				this.ProcessUpdatePlayerListPacket((UpdatePlayerList)packet);
				goto IL_D78;
			case 222:
				this.ProcessUpdateRepulsionConfig((UpdateRepulsionConfig)packet);
				goto IL_D78;
			case 223:
				this.ProcessUpdateResourceTypes((UpdateResourceTypes)packet);
				goto IL_D78;
			case 224:
				this.ProcessUpdateRootInteractions((UpdateRootInteractions)packet);
				goto IL_D78;
			case 226:
				this.ProcessUpdateTimePacket((UpdateTime)packet);
				goto IL_D78;
			case 227:
				this.ProcessUpdateTimeSettingsPacket((UpdateTimeSettings)packet);
				goto IL_D78;
			case 228:
				this.ProcessUpdateTrailsPacket((UpdateTrails)packet);
				goto IL_D78;
			case 229:
				this.ProcessUpdateTranslationsPacket((UpdateTranslations)packet);
				goto IL_D78;
			case 230:
				this.ProcessUpdateUnarmedInteractions((UpdateUnarmedInteractions)packet);
				goto IL_D78;
			case 231:
				this.ProcessViewBobbingProfiles((UpdateViewBobbing)packet);
				goto IL_D78;
			case 232:
				this.ProcessUpdateVisibleHudComponents((UpdateVisibleHudComponents)packet);
				goto IL_D78;
			case 233:
				this.ProcessUpdateWeatherPacket((UpdateWeather)packet);
				goto IL_D78;
			case 234:
				this.ProcessUpdateWeathersPacket((UpdateWeathers)packet);
				goto IL_D78;
			case 235:
				this.ProcessUpdateWindow((UpdateWindow)packet);
				goto IL_D78;
			case 236:
				this.ProcessUpdateWorldMapPacket((UpdateWorldMap)packet);
				goto IL_D78;
			case 237:
				this.ProcessUpdateWorldMapSettingsPacket((UpdateWorldMapSettings)packet);
				goto IL_D78;
			case 239:
				this.ProcessViewRadiusPacket((ViewRadius)packet);
				goto IL_D78;
			case 240:
				this.ProcessWorldLoadFinishedPacket((WorldLoadFinished)packet);
				goto IL_D78;
			case 241:
				this.ProcessWorldLoadProgressPacket((WorldLoadProgress)packet);
				goto IL_D78;
			case 242:
				this.ProcessWorldSettingsPacket((WorldSettings)packet);
				goto IL_D78;
			}
			bool flag = this._unhandledPacketTypes.Add(packet.GetType().Name);
			if (flag)
			{
				PacketHandler.Logger.Warn("Received unhandled packet type: {0}", packet.GetType().Name);
			}
			this._stageValidationPacketId = string.Empty;
			IL_D78:
			bool flag2 = this._stageValidationPacketId != string.Empty;
			if (flag2)
			{
				throw new Exception("Connection stage hasn't been validated for " + this._stageValidationPacketId);
			}
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000586E4 File Offset: 0x000568E4
		public int AddPendingCallback<T>(Disposable disposable, Action<FailureReply, T> callback) where T : ProtoPacket
		{
			bool flag = this._pendingCallbacks.Count > 1000;
			if (flag)
			{
				bool flag2 = (DateTime.Now - this._lastCallbackWarning).TotalSeconds > 5.0;
				if (flag2)
				{
					this._lastCallbackWarning = DateTime.Now;
					PacketHandler.Logger.Warn("There are currently more than 1000 pending packet callbacks. Removing oldest callback...");
				}
				int num = Enumerable.First<int>(this._pendingCallbacks.Keys);
				PacketHandler.PendingCallback pendingCallback;
				this._pendingCallbacks.TryRemove(num, ref pendingCallback);
				pendingCallback.Callback(new FailureReply(num, BsonHelper.ToBson(JToken.FromObject(FormattedMessage.FromMessageId("ui.general.callback.cancelled", null)))), null);
			}
			int num2 = Interlocked.Add(ref this._lastCallbackToken, 1);
			this._pendingCallbacks[num2] = new PacketHandler.PendingCallback
			{
				Callback = delegate(FailureReply err, ProtoPacket res)
				{
					callback(err, (T)((object)res));
				},
				Disposable = disposable
			};
			return num2;
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000587EC File Offset: 0x000569EC
		private void CallPendingCallback(int token, ProtoPacket responsePacket, FailureReply failurePacket)
		{
			PacketHandler.PendingCallback pendingCallback;
			bool flag = this._pendingCallbacks.TryRemove(token, ref pendingCallback);
			if (flag)
			{
				bool disposed = pendingCallback.Disposable.Disposed;
				if (!disposed)
				{
					Action<FailureReply, ProtoPacket> callback = pendingCallback.Callback;
					if (callback != null)
					{
						callback(failurePacket, responsePacket);
					}
				}
			}
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00058834 File Offset: 0x00056A34
		private void ProcessEntityUpdatesPacket(EntityUpdates packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			EntityUpdates entityUpdates = new EntityUpdates(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				bool flag = entityUpdates.Removed != null;
				if (flag)
				{
					foreach (int networkId in entityUpdates.Removed)
					{
						this._gameInstance.EntityStoreModule.Despawn(networkId);
					}
				}
				bool flag2 = entityUpdates.Updates == null;
				if (!flag2)
				{
					foreach (EntityUpdate entityUpdate in entityUpdates.Updates)
					{
						Entity entity;
						bool flag3 = this._gameInstance.EntityStoreModule.Spawn(entityUpdate.NetworkId, out entity);
						bool flag4 = false;
						bool flag5 = false;
						Model model = null;
						Equipment equipment = null;
						Guid? guid = null;
						bool flag6 = entityUpdate.Removed != null;
						if (flag6)
						{
							foreach (ComponentUpdateType componentUpdateType in entityUpdate.Removed)
							{
								switch (componentUpdateType)
								{
								case 0:
									entity.SetName("", false);
									break;
								case 1:
									entity.ClearUIComponents();
									break;
								case 2:
									entity.ClearCombatTexts();
									break;
								case 3:
								case 4:
								case 5:
								case 6:
								case 7:
								case 8:
								case 9:
								case 10:
								case 19:
									throw new NotImplementedException(string.Format("Removing {0} component isn't supported!", componentUpdateType));
								case 11:
									entity.ClearEffects();
									break;
								case 12:
									entity.Interactions.Clear();
									break;
								case 13:
									entity.DynamicLight = ColorRgb.Zero;
									flag4 = true;
									break;
								case 14:
									entity.SetUsable(false);
									break;
								case 15:
									entity.SetIsTangible(true);
									break;
								case 16:
									entity.SetInvulnerable(false);
									break;
								case 17:
									entity.HitboxCollisionConfigIndex = -1;
									break;
								case 18:
									entity.RepulsionConfigIndex = -1;
									break;
								default:
									throw new ArgumentOutOfRangeException();
								}
							}
						}
						bool flag7 = entityUpdate.Updates != null;
						if (flag7)
						{
							foreach (ComponentUpdate componentUpdate in entityUpdate.Updates)
							{
								switch (componentUpdate.Type)
								{
								case 0:
									entity.SetName(componentUpdate.Nameplate_.Text, true);
									break;
								case 1:
									entity.SetUIComponents(componentUpdate.EntityUIComponents);
									break;
								case 2:
									entity.AddCombatText(componentUpdate.CombatTextUpdate_);
									break;
								case 3:
									model = componentUpdate.Model_;
									break;
								case 4:
									entity.PlayerSkin = componentUpdate.Skin;
									flag5 = true;
									break;
								case 5:
									entity.SetItem(componentUpdate.Item_);
									break;
								case 6:
									entity.SetBlock(componentUpdate.BlockId, componentUpdate.BlockEntityScale);
									break;
								case 7:
									equipment = componentUpdate.Equipment_;
									break;
								case 8:
									entity.UpdateEntityStats(componentUpdate.EntityStatUpdates);
									break;
								case 9:
								{
									Vector3 position = entity.Position;
									Vector3 bodyOrientation = entity.BodyOrientation;
									Vector3 lookOrientation = entity.LookOrientation;
									ModelTransformHelper.Decompose(componentUpdate.Transform, ref position, ref bodyOrientation, ref lookOrientation);
									bool flag8 = flag3;
									if (flag8)
									{
										entity.SetSpawnTransform(position, bodyOrientation, lookOrientation);
										bool flag9 = entityUpdate.NetworkId == this._gameInstance.LocalPlayerNetworkId;
										if (flag9)
										{
											entity.SoundObjectReference = AudioDevice.PlayerSoundObjectReference;
										}
										else
										{
											this._gameInstance.AudioModule.TryRegisterSoundObject(position, bodyOrientation, ref entity.SoundObjectReference, false);
										}
									}
									else
									{
										bool flag10 = this._gameInstance.EntityStoreModule.MountEntityLocalId == entityUpdate.NetworkId;
										if (flag10)
										{
											return;
										}
										entity.SetTransform(position, bodyOrientation, lookOrientation);
									}
									break;
								}
								case 10:
								{
									bool flag11 = entityUpdate.NetworkId != this._gameInstance.EntityStoreModule.MountEntityLocalId;
									if (flag11)
									{
										ClientMovementStatesProtocolHelper.Parse(componentUpdate.MovementStates_, ref entity.ServerMovementStates);
									}
									break;
								}
								case 11:
									this._gameInstance.EntityStoreModule.UpdateEffects(entityUpdate.NetworkId, componentUpdate.EntityEffectUpdates);
									break;
								case 12:
									entity.Interactions = componentUpdate.Interactions;
									break;
								case 13:
									ClientItemBaseProtocolInitializer.ParseLightColor(componentUpdate.DynamicLight, ref entity.DynamicLight);
									flag4 = true;
									break;
								case 14:
									entity.SetUsable(true);
									break;
								case 15:
									entity.SetIsTangible(false);
									break;
								case 16:
									entity.SetInvulnerable(true);
									break;
								case 17:
									entity.HitboxCollisionConfigIndex = componentUpdate.HitboxCollisionConfigIndex;
									break;
								case 18:
									entity.RepulsionConfigIndex = componentUpdate.RepulsionConfigIndex;
									break;
								case 19:
									guid = new Guid?(componentUpdate.PredictionId);
									break;
								case 20:
									foreach (int value in componentUpdate.SoundEventIds)
									{
										uint networkWwiseId = ResourceManager.GetNetworkWwiseId(value);
										this._gameInstance.EntityStoreModule.QueueSoundEvent(networkWwiseId, entity.NetworkId);
									}
									break;
								default:
									throw new ArgumentOutOfRangeException();
								}
							}
						}
						bool flag12 = guid != null;
						if (flag12)
						{
							entity.Predictable = true;
							this._gameInstance.EntityStoreModule.MapPrediction(guid.Value, entity);
						}
						bool flag13 = model != null || ((((equipment != null) ? equipment.ArmorIds : null) != null || flag5) && entity.ModelPacket != null);
						if (flag13)
						{
							bool flag14 = !flag5 && model != null;
							if (flag14)
							{
								entity.PlayerSkin = null;
							}
							entity.SetCharacterModel(model, (equipment != null) ? equipment.ArmorIds : null);
							flag4 = true;
						}
						bool flag15 = equipment != null && equipment.RightHandItemId != null && ((equipment != null) ? equipment.LeftHandItemId : null) != null;
						if (flag15)
						{
							string newItemId = (equipment.RightHandItemId == "Empty") ? null : equipment.RightHandItemId;
							string newSecondaryItemId = (equipment.LeftHandItemId == "Empty") ? null : equipment.LeftHandItemId;
							entity.ChangeCharacterItem(newItemId, newSecondaryItemId);
							flag4 = true;
							bool flag16 = entity == this._gameInstance.LocalPlayer;
							if (flag16)
							{
								PacketHandler.Logger.Warn(string.Format("A {0} packet had been received on the local player", 7));
							}
						}
						bool flag17 = flag4;
						if (flag17)
						{
							entity.UpdateLight();
						}
					}
				}
			}, false, false);
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x0005888C File Offset: 0x00056A8C
		private void ProcessJoinWorldPacket(JoinWorld packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			PacketHandler.Logger.Info("Received JoinWorldPacket");
			JoinWorld joinWorld = new JoinWorld(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.CharacterControllerModule.MovementController.MovementEnabled = false;
				PacketHandler.Logger.Info("ProcessJoinWorldPacket: FadeInOut is {0}", joinWorld.FadeInOut);
				this._gameInstance.PrepareJoiningWorld(joinWorld);
			}, false, false);
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x000588F0 File Offset: 0x00056AF0
		private void ProcessClientTeleportPacket(ClientTeleport packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			ClientTeleport clientTeleport = new ClientTeleport(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				Vector3 position = this._gameInstance.LocalPlayer.Position;
				Vector3 bodyOrientation = this._gameInstance.LocalPlayer.BodyOrientation;
				Vector3 lookOrientation = this._gameInstance.LocalPlayer.LookOrientation;
				ModelTransformHelper.Decompose(clientTeleport.ModelTransform_, ref position, ref bodyOrientation, ref lookOrientation);
				this._gameInstance.LocalPlayer.SetTransform(position, bodyOrientation, lookOrientation);
				this._gameInstance.LocalPlayer.SkipTransformLerp();
				this._gameInstance.CharacterControllerModule.MovementController.InvalidateState();
				ClientMovement clientMovement = new ClientMovement();
				clientMovement.MovementStates_ = ClientMovementStatesProtocolHelper.ToPacket(ref this._gameInstance.CharacterControllerModule.MovementController.MovementStates);
				clientMovement.AbsolutePosition = this._gameInstance.LocalPlayer.Position.ToPositionPacket();
				clientMovement.BodyOrientation = this._gameInstance.LocalPlayer.BodyOrientation.ToDirectionPacket();
				clientMovement.LookOrientation = this._gameInstance.LocalPlayer.LookOrientation.ToDirectionPacket();
				clientMovement.TeleportAck_ = new ClientMovement.TeleportAck(clientTeleport.TeleportId);
				this._gameInstance.Connection.SendPacket(clientMovement);
			}, false, false);
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x00058944 File Offset: 0x00056B44
		private void ProcessPlayAnimation(PlayAnimation packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			PlayAnimation playAnimation = new PlayAnimation(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				Entity entity = this._gameInstance.EntityStoreModule.GetEntity(playAnimation.EntityId);
				bool flag = entity == null;
				if (!flag)
				{
					bool flag2 = entity.ModelRenderer == null;
					if (flag2)
					{
						PacketHandler.Logger.Warn("Received server play animation packet for entity {0} with no model", playAnimation.EntityId);
					}
					else
					{
						entity.ClearPassiveAnimationData();
						bool flag3 = playAnimation.ItemAnimationsId != null && playAnimation.Slot == 2;
						if (flag3)
						{
							EntityAnimation entityAnimation = null;
							bool flag4 = playAnimation.AnimationId != null;
							if (flag4)
							{
								ClientItemPlayerAnimations clientItemPlayerAnimations;
								bool itemPlayerAnimation = this._gameInstance.ItemLibraryModule.GetItemPlayerAnimation(playAnimation.ItemAnimationsId, out clientItemPlayerAnimations);
								if (itemPlayerAnimation)
								{
									if (clientItemPlayerAnimations != null)
									{
										Dictionary<string, EntityAnimation> animations = clientItemPlayerAnimations.Animations;
										if (animations != null)
										{
											animations.TryGetValue(playAnimation.AnimationId, out entityAnimation);
										}
									}
								}
							}
							bool flag5 = entityAnimation == null;
							if (flag5)
							{
								entityAnimation = EntityAnimation.Empty;
							}
							entity.SetActionAnimation(entityAnimation, 0f, false, false);
						}
						else
						{
							bool flag6 = playAnimation.Slot == 1 && entity.PredictedStatusCount > 0;
							if (flag6)
							{
								entity.PredictedStatusCount--;
								bool flag7 = playAnimation.AnimationId == "Hurt";
								if (flag7)
								{
									return;
								}
							}
							entity.SetServerAnimation(playAnimation.AnimationId, playAnimation.Slot, 0f, false);
						}
					}
				}
			}, false, false);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x00058998 File Offset: 0x00056B98
		private void ProcessSetEntitySeed(SetEntitySeed packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int entitySeed = packet.EntitySeed;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				Entity.ServerEntitySeed = entitySeed;
			}, false, false);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000589E8 File Offset: 0x00056BE8
		private void ProcessUpdateEntityEffectsPacket(UpdateEntityEffects packet)
		{
			UpdateEntityEffects updateEntityEffects = new UpdateEntityEffects(packet);
			UpdateType type = updateEntityEffects.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateEntityEffects.GetType().Name, this._stage, updateEntityEffects.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] EntityEffects: Starting {0}", updateEntityEffects.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = updateEntityEffects.MaxId > this._entityEffects.Length;
				if (flag)
				{
					Array.Resize<EntityEffect>(ref this._entityEffects, updateEntityEffects.MaxId);
				}
				bool flag2 = updateEntityEffects.Type == 1;
				if (flag2)
				{
					foreach (KeyValuePair<int, EntityEffect> keyValuePair in updateEntityEffects.EntityEffects)
					{
						this._entityEffects[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				else
				{
					foreach (KeyValuePair<int, EntityEffect> keyValuePair2 in updateEntityEffects.EntityEffects)
					{
						this._entityEffects[keyValuePair2.Key] = null;
					}
				}
				EntityEffect[] upcomingEntityEffects;
				this._gameInstance.EntityStoreModule.PrepareEntityEffects(this._entityEffects, out upcomingEntityEffects);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.EntityStoreModule.SetupEntityEffects(upcomingEntityEffects);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] EntityEffects: Finished {0} in {1}", updateEntityEffects.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.EntityEffects);
				bool flag3 = this._entityEffects == null;
				if (flag3)
				{
					this._entityEffects = new EntityEffect[updateEntityEffects.MaxId];
				}
				foreach (KeyValuePair<int, EntityEffect> keyValuePair3 in updateEntityEffects.EntityEffects)
				{
					this._entityEffects[keyValuePair3.Key] = keyValuePair3.Value;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.EntityEffects);
			}
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x00058CD4 File Offset: 0x00056ED4
		private void ProcessUpdateTranslationsPacket(UpdateTranslations packet)
		{
			UpdateType updateType = packet.Type;
			Dictionary<string, string> translations = new Dictionary<string, string>(packet.Translations.Count);
			foreach (KeyValuePair<string, string> keyValuePair in packet.Translations)
			{
				translations[string.Copy(keyValuePair.Key)] = string.Copy(keyValuePair.Value);
			}
			switch (updateType)
			{
			case 0:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
				bool flag = this._stage == PacketHandler.ConnectionStage.SettingUp;
				if (flag)
				{
					this.ReceivedAssetType(PacketHandler.AssetType.Translations);
				}
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.Interface.SetServerMessages(translations);
				}, false, false);
				bool flag2 = this._stage == PacketHandler.ConnectionStage.SettingUp;
				if (flag2)
				{
					this.FinishedReceivedAssetType(PacketHandler.AssetType.Translations);
				}
				break;
			}
			case 1:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Translations: Starting {0}", updateType));
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.Interface.AddServerMessages(translations);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Translations: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			case 2:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Translations: Starting {0}", updateType));
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.Interface.RemoveServerMessages(translations.Keys);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Translations: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			default:
				throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, updateType));
			}
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x00058F34 File Offset: 0x00057134
		private void ProcessUpdateImmersiveViewPacket(UpdateImmersiveView packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				bool flag = packet != null;
				if (flag)
				{
					this._gameInstance.ImmersiveScreenModule.HandleUpdatePacket(packet);
				}
			}, false, false);
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x00058F84 File Offset: 0x00057184
		private void ProcessUpdateWorldMapSettingsPacket(UpdateWorldMapSettings packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			UpdateWorldMapSettings updateWorldMapSettings = new UpdateWorldMapSettings(packet);
			this._gameInstance.WorldMapModule.RunOnWorldMapThread(delegate
			{
				this._gameInstance.WorldMapModule.UpdateMapSettings(updateWorldMapSettings.BiomeDataMap, !updateWorldMapSettings.Disabled, updateWorldMapSettings.AllowTeleportToCoordinates, updateWorldMapSettings.AllowTeleportToMarkers);
			});
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x00058FD4 File Offset: 0x000571D4
		private void ProcessUpdateWorldMapPacket(UpdateWorldMap packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			UpdateWorldMap updateWorldMap = new UpdateWorldMap(packet);
			this._gameInstance.WorldMapModule.RunOnWorldMapThread(delegate
			{
				bool flag = updateWorldMap.Chunks != null;
				if (flag)
				{
					foreach (UpdateWorldMap.Chunk chunk in updateWorldMap.Chunks)
					{
						this._gameInstance.WorldMapModule.SetMapChunk(chunk.ChunkX, chunk.ChunkZ, chunk.Image_);
					}
				}
			});
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				bool flag = updateWorldMap.AddedMarkers != null;
				if (flag)
				{
					for (int i = 0; i < updateWorldMap.AddedMarkers.Length; i++)
					{
						UpdateWorldMap.Marker marker = updateWorldMap.AddedMarkers[i];
						this._gameInstance.WorldMapModule.AddMapMarker(new WorldMapModule.MapMarker
						{
							Id = marker.Id,
							Name = marker.Name,
							MarkerImage = marker.MarkerImage,
							X = (float)marker.Transform_.Position_.X,
							Y = (float)marker.Transform_.Position_.Y,
							Z = (float)marker.Transform_.Position_.Z,
							Yaw = marker.Transform_.Orientation.Yaw,
							Pitch = marker.Transform_.Orientation.Pitch,
							Roll = marker.Transform_.Orientation.Roll
						});
					}
				}
				bool flag2 = updateWorldMap.RemovedMarkers != null;
				if (flag2)
				{
					this._gameInstance.WorldMapModule.RemoveMapMarker(updateWorldMap.RemovedMarkers);
				}
			}, false, false);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x00059048 File Offset: 0x00057248
		private void ProcessShowEventTitlePacket(ShowEventTitle packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			PacketHandler.EventTitle eventTitle = new PacketHandler.EventTitle
			{
				PrimaryTitle = packet.PrimaryTitle,
				SecondaryTitle = packet.SecondaryTitle,
				IsMajor = packet.IsMajor,
				Icon = packet.Icon,
				Duration = packet.Duration,
				FadeInDuration = packet.FadeInDuration,
				FadeOutDuration = packet.FadeOutDuration
			};
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("eventTitle.show", eventTitle, null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x000590F0 File Offset: 0x000572F0
		private void ProcessHideEventTitlePacket(HideEventTitle packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			float fadeOutDuration = packet.FadeOutDuration;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("eventTitle.hide", fadeOutDuration, null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x00059144 File Offset: 0x00057344
		private void ProcessSetPagePacket(SetPage packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			bool flag = packet.Page_ == 7;
			if (flag)
			{
				throw new Exception("CustomPage must be opened with CustomPage packet!");
			}
			Page page = packet.Page_;
			bool canCloseThroughInteraction = packet.CanCloseThroughInteraction;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.InGame.SetCurrentPage(page, page > 0 & canCloseThroughInteraction, false);
			}, false, false);
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x000591BC File Offset: 0x000573BC
		private void ProcessUpdateKnownRecipes(UpdateKnownRecipes packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("crafting.knownRecipesUpdated", Enumerable.ToArray<PacketHandler.ClientKnownRecipe>(Enumerable.Select<KeyValuePair<string, CraftingRecipe>, PacketHandler.ClientKnownRecipe>(packet.Known, (KeyValuePair<string, CraftingRecipe> kr) => new PacketHandler.ClientKnownRecipe(kr.Key, kr.Value))), null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x0005920C File Offset: 0x0005740C
		private void ProcessCustomHudPacket(CustomHud packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			CustomHud customHud = new CustomHud(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.InGame.UpdateCustomHud(customHud);
			}, false, false);
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x00059260 File Offset: 0x00057460
		private void ProcessCustomPagePacket(CustomPage packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			CustomPage customPage = new CustomPage(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.InGame.OpenOrUpdateCustomPage(customPage);
			}, false, false);
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000592B4 File Offset: 0x000574B4
		private void ProcessOpenWindow(OpenWindow packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			PacketHandler.InventoryWindow inventoryWindow = new PacketHandler.InventoryWindow
			{
				Id = packet.Id,
				WindowType = packet.WindowType_,
				WindowDataStringified = string.Copy(packet.WindowData),
				WindowData = ((packet.WindowData != null) ? JObject.Parse(packet.WindowData) : null),
				Inventory = this.ConvertToClientItemStacks(packet.Inventory)
			};
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("inventory.windows.open", inventoryWindow, null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x00059360 File Offset: 0x00057560
		private void ProcessUpdateWindow(UpdateWindow packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			PacketHandler.InventoryWindow inventoryWindow = new PacketHandler.InventoryWindow
			{
				Id = packet.Id,
				WindowDataStringified = string.Copy(packet.WindowData),
				WindowData = ((packet.WindowData != null) ? JObject.Parse(packet.WindowData) : null),
				Inventory = this.ConvertToClientItemStacks(packet.Inventory)
			};
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("inventory.windows.update", inventoryWindow, null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x00059400 File Offset: 0x00057600
		private void ProcessCloseWindow(CloseWindow packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			int packetId = packet.Id;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("inventory.windows.close", packetId, null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x00059454 File Offset: 0x00057654
		private void ProcessFailureReply(FailureReply packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this.CallPendingCallback(packet.Token, null, packet);
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x0005946E File Offset: 0x0005766E
		private void ProcessSuccessReply(SuccessReply packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this.CallPendingCallback(packet.Token, packet, null);
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x00059488 File Offset: 0x00057688
		private void ProcessUpdateVisibleHudComponents(UpdateVisibleHudComponents packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			bool flag = packet.VisibleComponents == null;
			if (flag)
			{
				throw new Exception("VisibleComponents cannot be empty!");
			}
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("hud.visibleComponentsUpdated", packet.VisibleComponents, null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x000594F8 File Offset: 0x000576F8
		private void ProcessDamageInfo(DamageInfo packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			DamageInfo damageInfo = new DamageInfo(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				bool flag = packet.DamageAmount > 0f;
				if (flag)
				{
					this._gameInstance.DamageEffectModule.AddDamageEffect(damageInfo.DamageSourcePosition, damageInfo.DamageAmount, damageInfo.DamageCause_);
				}
				this._gameInstance.InteractionModule.DamageInfos.Add(packet);
			}, false, false);
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x00059558 File Offset: 0x00057758
		private void ProcessReticleEvent(ReticleEvent packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			int eventIndex = packet.EventIndex;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.InGameView.OnReticleServerEvent(eventIndex);
			}, false, false);
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x000595AC File Offset: 0x000577AC
		private void ProcessResetUserInterfaceState(ResetUserInterfaceState packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.InGame.Reset(true);
			}, false, false);
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x000595DC File Offset: 0x000577DC
		private ClientItemStack[] ConvertToClientItemStacks(InventorySection section)
		{
			bool flag = section == null;
			ClientItemStack[] result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ClientItemStack[] array = new ClientItemStack[(int)section.Capacity];
				foreach (KeyValuePair<int, Item> keyValuePair in section.Items)
				{
					array[keyValuePair.Key] = new ClientItemStack(keyValuePair.Value);
				}
				result = array;
			}
			return result;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x00059660 File Offset: 0x00057860
		private void ProcessUpdateItemPlayerAnimationsPacket(UpdateItemPlayerAnimations packet)
		{
			PacketHandler.<>c__DisplayClass136_0 CS$<>8__locals1 = new PacketHandler.<>c__DisplayClass136_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.updateItemPlayerAnimations = new UpdateItemPlayerAnimations(packet);
			UpdateType type = CS$<>8__locals1.updateItemPlayerAnimations.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType - 1 <= 1)
				{
					this.ValidateStage(PacketHandler.ConnectionStage.Playing);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemAnimations: Starting {0}", CS$<>8__locals1.updateItemPlayerAnimations.Type));
					Stopwatch stopwatch = Stopwatch.StartNew();
					object setupLock = this._setupLock;
					Dictionary<string, ClientItemPlayerAnimations> upcomingItemPlayerAnimations;
					lock (setupLock)
					{
						bool flag2 = CS$<>8__locals1.updateItemPlayerAnimations.Type == 1;
						if (flag2)
						{
							foreach (KeyValuePair<string, ItemPlayerAnimations> keyValuePair in CS$<>8__locals1.updateItemPlayerAnimations.ItemPlayerAnimations_)
							{
								this._networkItemPlayerAnimations[keyValuePair.Key] = keyValuePair.Value;
							}
						}
						else
						{
							foreach (KeyValuePair<string, ItemPlayerAnimations> keyValuePair2 in CS$<>8__locals1.updateItemPlayerAnimations.ItemPlayerAnimations_)
							{
								this._networkItemPlayerAnimations.Remove(keyValuePair2.Key);
							}
						}
						this._gameInstance.ItemLibraryModule.PrepareItemPlayerAnimations(this._networkItemPlayerAnimations, out this._upcomingItemPlayerAnimations);
						bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
						if (isCancellationRequested)
						{
							return;
						}
						upcomingItemPlayerAnimations = this._upcomingItemPlayerAnimations;
					}
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						CS$<>8__locals1.<>4__this._gameInstance.ItemLibraryModule.SetupItemPlayerAnimations(upcomingItemPlayerAnimations);
						CS$<>8__locals1.<>4__this._gameInstance.ItemLibraryModule.LinkItemPlayerAnimations();
						CS$<>8__locals1.<>4__this._gameInstance.EntityStoreModule.RebuildRenderers(true);
						CS$<>8__locals1.<>4__this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemAnimations: Finished {0} in {1}", CS$<>8__locals1.updateItemPlayerAnimations.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
					}, false, false);
				}
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.ItemAnimations);
				bool flag3 = this._networkItemPlayerAnimations == null;
				if (flag3)
				{
					this._networkItemPlayerAnimations = new Dictionary<string, ItemPlayerAnimations>();
				}
				foreach (KeyValuePair<string, ItemPlayerAnimations> keyValuePair3 in CS$<>8__locals1.updateItemPlayerAnimations.ItemPlayerAnimations_)
				{
					this._networkItemPlayerAnimations[keyValuePair3.Key] = keyValuePair3.Value;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.ItemAnimations);
			}
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x00059950 File Offset: 0x00057B50
		private void ProcessUpdateItemsPacket(UpdateItems packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Items: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				Dictionary<string, ClientIcon> upcomingIcons = null;
				byte[] iconAtlasPixels = null;
				int iconAtlasWidth = 0;
				int iconAtlasHeight = 0;
				object setupLock = this._setupLock;
				Dictionary<string, ClientItemBase> upcomingItems;
				lock (setupLock)
				{
					bool flag2 = packet.Type == 1;
					if (flag2)
					{
						foreach (KeyValuePair<string, ItemBase> keyValuePair in packet.Items)
						{
							this._networkItems[keyValuePair.Key] = keyValuePair.Value;
						}
					}
					else
					{
						foreach (KeyValuePair<string, ItemBase> keyValuePair2 in packet.Items)
						{
							this._networkItems.Remove(keyValuePair2.Key);
						}
					}
					this._gameInstance.ItemLibraryModule.PrepareItems(packet.Items, this._upcomingEntitiesImageLocations, ref this._upcomingItems, this._threadCancellationToken);
					bool updateIcons = packet.UpdateIcons;
					if (updateIcons)
					{
						this._gameInstance.ItemLibraryModule.PrepareItemIconAtlas(this._networkItems, out upcomingIcons, out iconAtlasPixels, out iconAtlasWidth, out iconAtlasHeight, this._threadCancellationToken);
					}
					bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					upcomingItems = new Dictionary<string, ClientItemBase>(this._upcomingItems);
				}
				Func<string, ClientItemBase> <>9__2;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.ItemLibraryModule.SetupItems(upcomingItems);
					bool flag4 = packet.Type == 1;
					if (flag4)
					{
						Interface @interface = this._gameInstance.App.Interface;
						string name = "items.added";
						IEnumerable<string> keys = packet.Items.Keys;
						Func<string, string> func = (string k) => k;
						Func<string, ClientItemBase> func2;
						if ((func2 = <>9__2) == null)
						{
							func2 = (<>9__2 = ((string k) => upcomingItems[k]));
						}
						@interface.TriggerEvent(name, Enumerable.ToDictionary<string, string, ClientItemBase>(keys, func, func2), null, null, null, null, null);
					}
					else
					{
						this._gameInstance.App.Interface.TriggerEvent("items.removed", Enumerable.ToArray<string>(packet.Items.Keys), null, null, null, null, null);
					}
					bool flag5 = packet.UpdateModels || packet.UpdateIcons;
					if (flag5)
					{
						this._gameInstance.EntityStoreModule.RebuildRenderers(false);
						this._gameInstance.InterfaceRenderPreviewModule.HandleAssetsChanged();
					}
					bool flag6 = upcomingIcons != null;
					if (flag6)
					{
						this._gameInstance.ItemLibraryModule.SetupItemIcons(upcomingIcons, iconAtlasPixels, iconAtlasWidth, iconAtlasHeight);
					}
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Items: Finished {0} in {1}", packet.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.Items);
				bool flag3 = this._networkItems == null;
				if (flag3)
				{
					this._networkItems = new Dictionary<string, ItemBase>();
				}
				foreach (KeyValuePair<string, ItemBase> keyValuePair3 in packet.Items)
				{
					this._networkItems[keyValuePair3.Key] = keyValuePair3.Value;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.Items);
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x00059D00 File Offset: 0x00057F00
		private void ProcessUpdateItemCategoriesPacket(UpdateItemCategories packet)
		{
			switch (packet.Type)
			{
			case 0:
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.ItemCategories);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.InGame.OnItemCategoriesInitialized(packet.ItemCategories);
				}, false, false);
				this.FinishedReceivedAssetType(PacketHandler.AssetType.ItemCategories);
				break;
			case 1:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemCategories: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.InGame.OnItemCategoriesAdded(packet.ItemCategories);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemCategories: Finished {0} in {1}", packet.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			case 2:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemCategories: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.InGame.OnItemCategoriesRemoved(packet.ItemCategories);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemCategories: Finished {0} in {1}", packet.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			default:
				throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
			}
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x00059ED4 File Offset: 0x000580D4
		private void ProcessUpdateFieldcraftCategoriesPacket(UpdateFieldcraftCategories packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType != 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] FieldcraftCategories: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				ClientCraftingCategory[] categories = Enumerable.ToArray<ClientCraftingCategory>(Enumerable.OrderBy<ClientCraftingCategory, int>(Enumerable.Select<ItemCategory, ClientCraftingCategory>(packet.ItemCategories, (ItemCategory category) => new ClientCraftingCategory
				{
					Id = category.Id,
					Icon = category.Icon,
					Order = category.Order
				}), (ClientCraftingCategory category) => category.Order));
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.Interface.TriggerEvent("fieldcraftCategories.added", categories, null, null, null, null, null);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] FieldcraftCategories: Finished {0} in {1}", packet.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.FieldcraftCategories);
				ClientCraftingCategory[] categories = Enumerable.ToArray<ClientCraftingCategory>(Enumerable.OrderBy<ClientCraftingCategory, int>(Enumerable.Select<ItemCategory, ClientCraftingCategory>(packet.ItemCategories, (ItemCategory category) => new ClientCraftingCategory
				{
					Id = category.Id,
					Icon = category.Icon,
					Order = category.Order
				}), (ClientCraftingCategory category) => category.Order));
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.Interface.TriggerEvent("fieldcraftCategories.initialized", categories, null, null, null, null, null);
				}, false, false);
				this.FinishedReceivedAssetType(PacketHandler.AssetType.FieldcraftCategories);
			}
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x0005A0F4 File Offset: 0x000582F4
		private void ProcessUpdateResourceTypes(UpdateResourceTypes packet)
		{
			switch (packet.Type)
			{
			case 0:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.ResourceTypes);
				Dictionary<string, ClientResourceType> resourceTypes = new Dictionary<string, ClientResourceType>();
				foreach (KeyValuePair<string, ResourceType> keyValuePair in packet.ResourceTypes)
				{
					resourceTypes[keyValuePair.Key] = new ClientResourceType(keyValuePair.Value);
				}
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.Interface.TriggerEvent("resourceTypes.initialized", resourceTypes, null, null, null, null, null);
					this._gameInstance.ItemLibraryModule.SetupResourceTypes(resourceTypes);
				}, false, false);
				this.FinishedReceivedAssetType(PacketHandler.AssetType.ResourceTypes);
				break;
			}
			case 1:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ResourceTypes: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				Dictionary<string, ClientResourceType> addedResourceTypes = new Dictionary<string, ClientResourceType>();
				foreach (KeyValuePair<string, ResourceType> keyValuePair2 in packet.ResourceTypes)
				{
					addedResourceTypes[keyValuePair2.Key] = new ClientResourceType(keyValuePair2.Value);
				}
				Dictionary<string, ClientResourceType> resourceTypes = new Dictionary<string, ClientResourceType>(this._gameInstance.ItemLibraryModule.ResourceTypes);
				foreach (KeyValuePair<string, ClientResourceType> keyValuePair3 in addedResourceTypes)
				{
					resourceTypes[keyValuePair3.Key] = keyValuePair3.Value;
				}
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.Interface.TriggerEvent("resourceTypes.added", addedResourceTypes, null, null, null, null, null);
					this._gameInstance.ItemLibraryModule.SetupResourceTypes(resourceTypes);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ResourceTypes: Finished {0} in {1}", packet.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			case 2:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				string[] removedKeys = Enumerable.ToArray<string>(packet.ResourceTypes.Keys);
				Dictionary<string, ClientResourceType> resourceTypes = new Dictionary<string, ClientResourceType>(this._gameInstance.ItemLibraryModule.ResourceTypes);
				foreach (string key in removedKeys)
				{
					resourceTypes.Remove(key);
				}
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.App.Interface.TriggerEvent("resourceTypes.removed", removedKeys, null, null, null, null, null);
					this._gameInstance.ItemLibraryModule.SetupResourceTypes(resourceTypes);
				}, false, false);
				break;
			}
			default:
				throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
			}
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x0005A46C File Offset: 0x0005866C
		private void ProcessUpdateItemQualitiesPacket(UpdateItemQualities packet)
		{
			PacketHandler.<>c__DisplayClass143_0 CS$<>8__locals1 = new PacketHandler.<>c__DisplayClass143_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.packet = packet;
			UpdateType type = CS$<>8__locals1.packet.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType - 1 <= 1)
				{
					this.ValidateStage(PacketHandler.ConnectionStage.Playing);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemQualities: Starting {0}", CS$<>8__locals1.packet.Type));
					Stopwatch stopwatch = Stopwatch.StartNew();
					bool flag = CS$<>8__locals1.packet.MaxId > this._upcomingServerSettings.ItemQualities.Length;
					if (flag)
					{
						Array.Resize<ClientItemQuality>(ref this._upcomingServerSettings.ItemQualities, CS$<>8__locals1.packet.MaxId);
					}
					bool flag2 = CS$<>8__locals1.packet.Type == 1;
					if (flag2)
					{
						foreach (KeyValuePair<int, ItemQuality> keyValuePair in CS$<>8__locals1.packet.ItemQualities)
						{
							this._upcomingServerSettings.ItemQualities[keyValuePair.Key] = new ClientItemQuality(keyValuePair.Value);
						}
					}
					ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						CS$<>8__locals1.<>4__this._gameInstance.SetServerSettings(newServerSettings);
						CS$<>8__locals1.<>4__this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemQualities: Finished {0} in {1}", CS$<>8__locals1.packet.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
					}, false, false);
				}
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.ItemQuality);
				bool flag3 = this._upcomingServerSettings.ItemQualities == null;
				if (flag3)
				{
					this._upcomingServerSettings.ItemQualities = new ClientItemQuality[CS$<>8__locals1.packet.MaxId];
				}
				foreach (KeyValuePair<int, ItemQuality> keyValuePair2 in CS$<>8__locals1.packet.ItemQualities)
				{
					this._upcomingServerSettings.ItemQualities[keyValuePair2.Key] = new ClientItemQuality(keyValuePair2.Value);
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.ItemQuality);
			}
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x0005A6DC File Offset: 0x000588DC
		private void ProcessUpdateItemReticles(UpdateItemReticles packet)
		{
			PacketHandler.<>c__DisplayClass144_0 CS$<>8__locals1 = new PacketHandler.<>c__DisplayClass144_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.packet = packet;
			UpdateType type = CS$<>8__locals1.packet.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", CS$<>8__locals1.packet.GetType().Name, this._stage, CS$<>8__locals1.packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemQualities: Starting {0}", CS$<>8__locals1.packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = CS$<>8__locals1.packet.MaxId > this._upcomingServerSettings.ItemReticleConfigs.Length;
				if (flag)
				{
					Array.Resize<ClientItemReticleConfig>(ref this._upcomingServerSettings.ItemReticleConfigs, CS$<>8__locals1.packet.MaxId);
				}
				bool flag2 = CS$<>8__locals1.packet.Type == 1;
				if (flag2)
				{
					foreach (KeyValuePair<int, ItemReticleConfig> keyValuePair in CS$<>8__locals1.packet.ItemReticleConfigs)
					{
						this._upcomingServerSettings.ItemReticleConfigs[keyValuePair.Key] = new ClientItemReticleConfig(keyValuePair.Value);
					}
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					CS$<>8__locals1.<>4__this._gameInstance.SetServerSettings(newServerSettings);
					CS$<>8__locals1.<>4__this._gameInstance.App.Interface.InGameView.OnReticlesUpdated();
					CS$<>8__locals1.<>4__this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ItemReticleConfigs: Finished {0} in {1}", CS$<>8__locals1.packet.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.ItemReticles);
				bool flag3 = this._upcomingServerSettings.ItemReticleConfigs == null;
				if (flag3)
				{
					this._upcomingServerSettings.ItemReticleConfigs = new ClientItemReticleConfig[CS$<>8__locals1.packet.MaxId];
				}
				foreach (KeyValuePair<int, ItemReticleConfig> keyValuePair2 in CS$<>8__locals1.packet.ItemReticleConfigs)
				{
					this._upcomingServerSettings.ItemReticleConfigs[keyValuePair2.Key] = new ClientItemReticleConfig(keyValuePair2.Value);
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.ItemReticles);
			}
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x0005A98C File Offset: 0x00058B8C
		private void ProcessSetMachinimaActorModelPacket(SetMachinimaActorModel packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				((EntityActor)this._gameInstance.MachinimaModule.GetScene(packet.SceneName).GetActor(packet.ActorName)).SetBaseModel(packet.Model_);
			}, false, false);
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x0005A9DC File Offset: 0x00058BDC
		private void ProcessUpdateMachinimaScene(UpdateMachinimaScene packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.MachinimaModule.HandleSceneUpdatePacket(packet);
			}, false, false);
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x0005AA2C File Offset: 0x00058C2C
		private void ProcessServerMessagePacket(ServerMessage packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			ServerMessage serverMessage = new ServerMessage(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.Chat.AddBsonMessage(serverMessage.Message);
			}, false, false);
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x0005AA84 File Offset: 0x00058C84
		private void ProcessNotificationPacket(Notification packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			Notification notification = new Notification(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.Notifications.AddNotification(notification);
			}, false, false);
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x0005AADC File Offset: 0x00058CDC
		private void ProcessKillFeedMessage(KillFeedMessage packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			KillFeedMessage killFeedMessage = new KillFeedMessage(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.InGameView.KillFeedComponent.OnReceiveNewEntry(killFeedMessage.Decedent, killFeedMessage.Killer, killFeedMessage.Icon);
			}, false, false);
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x0005AB34 File Offset: 0x00058D34
		private void ProcessUpdateModelVFXsPacket(UpdateModelvfxs packet)
		{
			UpdateModelvfxs updateModelVFXs = new UpdateModelvfxs(packet);
			UpdateType type = updateModelVFXs.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateModelVFXs.GetType().Name, this._stage, updateModelVFXs.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ModelVFXs: Starting {0}", updateModelVFXs.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = updateModelVFXs.MaxId > this._modelVFXs.Length;
				if (flag)
				{
					Array.Resize<ModelVFX>(ref this._modelVFXs, updateModelVFXs.MaxId);
				}
				bool flag2 = updateModelVFXs.Type == 1;
				if (flag2)
				{
					foreach (KeyValuePair<int, ModelVFX> keyValuePair in updateModelVFXs.ModelVFXs)
					{
						this._modelVFXs[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				else
				{
					foreach (KeyValuePair<int, ModelVFX> keyValuePair2 in updateModelVFXs.ModelVFXs)
					{
						this._modelVFXs[keyValuePair2.Key] = null;
					}
				}
				ModelVFX[] upcomingModelVFXs;
				this._gameInstance.EntityStoreModule.PrepareModelVFXs(this._modelVFXs, out upcomingModelVFXs);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.EntityStoreModule.SetupModelVFXs(upcomingModelVFXs);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ModelVFXs: Finished {0} in {1}", updateModelVFXs.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.ModelVFX);
				bool flag3 = this._modelVFXs == null;
				if (flag3)
				{
					this._modelVFXs = new ModelVFX[updateModelVFXs.MaxId];
				}
				foreach (KeyValuePair<int, ModelVFX> keyValuePair3 in updateModelVFXs.ModelVFXs)
				{
					this._modelVFXs[keyValuePair3.Key] = keyValuePair3.Value;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.ModelVFX);
			}
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x0005AE20 File Offset: 0x00059020
		private void ProcessTrackOrUpdateObjective(TrackOrUpdateObjective packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("objectives.updateObjective", packet.Objective_, null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x0005AE70 File Offset: 0x00059070
		private void ProcessUntrackObjective(UntrackObjective packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("objectives.removeObjective", packet.ObjectiveUuid, null, null, null, null, null);
			}, false, false);
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x0005AEC0 File Offset: 0x000590C0
		private void ProcessUpdateObjectiveTask(UpdateObjectiveTask packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.TriggerEvent("objectives.updateTask", packet.ObjectiveUuid, packet.TaskIndex, packet.Task, null, null, null);
			}, false, false);
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x0005AF10 File Offset: 0x00059110
		private void ProcessUpdateParticleSystemsPacket(UpdateParticleSystems packet)
		{
			UpdateParticleSystems updateParticleSystems = new UpdateParticleSystems(packet);
			switch (updateParticleSystems.Type)
			{
			case 0:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.ParticleSystems);
				bool flag = this._networkParticleSystems == null;
				if (flag)
				{
					this._networkParticleSystems = new Dictionary<string, ParticleSystem>();
				}
				foreach (KeyValuePair<string, ParticleSystem> keyValuePair in updateParticleSystems.ParticleSystems)
				{
					this._networkParticleSystems[keyValuePair.Key] = keyValuePair.Value;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.ParticleSystems);
				break;
			}
			case 1:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				Stopwatch stopwatch = Stopwatch.StartNew();
				foreach (KeyValuePair<string, ParticleSystem> keyValuePair2 in updateParticleSystems.ParticleSystems)
				{
					this._networkParticleSystems[keyValuePair2.Key] = keyValuePair2.Value;
				}
				Dictionary<string, ParticleSystem> upcomingParticleSystems = new Dictionary<string, ParticleSystem>(updateParticleSystems.ParticleSystems);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.ParticleSystemStoreModule.SetupParticleSystems(upcomingParticleSystems);
					this._gameInstance.ParticleSystemStoreModule.ResetParticleSystems(false);
					this._gameInstance.Chat.Log(string.Format("[AssetUpdate] ParticleSystems: Finished {0} in {1}", updateParticleSystems.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			case 2:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ParticleSystems: Starting {0}", updateParticleSystems.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				foreach (KeyValuePair<string, ParticleSystem> keyValuePair3 in updateParticleSystems.ParticleSystems)
				{
					this._networkParticleSystems.Remove(keyValuePair3.Key);
				}
				Dictionary<string, ParticleSystem> removedParticleSystems = new Dictionary<string, ParticleSystem>(updateParticleSystems.ParticleSystems);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.ParticleSystemStoreModule.RemoveParticleSystems(removedParticleSystems);
					this._gameInstance.ParticleSystemStoreModule.ResetParticleSystems(false);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ParticleSystems: Finished {0} in {1}", updateParticleSystems.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			default:
				throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateParticleSystems.GetType().Name, this._stage, updateParticleSystems.Type));
			}
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x0005B204 File Offset: 0x00059404
		private void ProcessUpdateParticleSpawnersPacket(UpdateParticleSpawners packet)
		{
			UpdateParticleSpawners updateParticleSpawners = new UpdateParticleSpawners(packet);
			switch (updateParticleSpawners.Type)
			{
			case 0:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.ParticleSpawners);
				bool flag = this._networkParticleSpawners == null;
				if (flag)
				{
					this._networkParticleSpawners = new Dictionary<string, ParticleSpawner>();
				}
				foreach (KeyValuePair<string, ParticleSpawner> keyValuePair in updateParticleSpawners.ParticleSpawners)
				{
					this._networkParticleSpawners[keyValuePair.Key] = keyValuePair.Value;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.ParticleSpawners);
				break;
			}
			case 1:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ParticleSpawners: Starting {0}", updateParticleSpawners.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				object setupLock = this._setupLock;
				Dictionary<string, ParticleSystem> upcomingParticleSystems;
				Dictionary<string, ParticleSpawner> upcomingParticleSpawners;
				Dictionary<string, ParticleSettings> upcomingParticles;
				Dictionary<string, Rectangle> upcomingFXImageLocations;
				byte[][] upcomingFXAtlasPixelsPerLevel;
				byte[][] upcomingUVMotionTextureArrayPixelsPerLevel;
				lock (setupLock)
				{
					foreach (KeyValuePair<string, ParticleSpawner> keyValuePair2 in updateParticleSpawners.ParticleSpawners)
					{
						this._networkParticleSpawners[keyValuePair2.Key] = keyValuePair2.Value;
					}
					upcomingParticleSystems = new Dictionary<string, ParticleSystem>(this._networkParticleSystems);
					upcomingParticleSpawners = new Dictionary<string, ParticleSpawner>(this._networkParticleSpawners);
					this._gameInstance.ParticleSystemStoreModule.PrepareParticles(upcomingParticleSpawners, out upcomingParticles, out this._upcomingParticleTextureInfo, out this._upcomingUVMotionTexturePaths, this._threadCancellationToken);
					bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						break;
					}
					this._gameInstance.FXModule.PrepareAtlas(this._upcomingParticleTextureInfo, this._upcomingTrailTextureInfo, out upcomingFXImageLocations, out upcomingFXAtlasPixelsPerLevel, this._threadCancellationToken);
					bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested2)
					{
						break;
					}
					this._gameInstance.FXModule.PrepareUVMotionTextureArray(this._upcomingUVMotionTexturePaths, out upcomingUVMotionTextureArrayPixelsPerLevel);
				}
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.ParticleSystemStoreModule.SetupParticleSpawners(upcomingParticleSystems, upcomingParticleSpawners, upcomingParticles, this._upcomingUVMotionTexturePaths);
					this._gameInstance.FXModule.CreateAtlasTextures(upcomingFXImageLocations, upcomingFXAtlasPixelsPerLevel);
					this._gameInstance.FXModule.CreateUVMotionTextureArray(upcomingUVMotionTextureArrayPixelsPerLevel);
					this._gameInstance.ParticleSystemStoreModule.ResetParticleSystems(false);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ParticleSpawners: Finished {0} in {1}", updateParticleSpawners.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			case 2:
			{
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ParticleSpawners: Starting {0}", updateParticleSpawners.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				object setupLock2 = this._setupLock;
				Dictionary<string, ParticleSystem> upcomingParticleSystems;
				Dictionary<string, ParticleSpawner> upcomingParticleSpawners;
				Dictionary<string, ParticleSettings> upcomingParticles;
				Dictionary<string, Rectangle> upcomingFXImageLocations;
				byte[][] upcomingFXAtlasPixelsPerLevel;
				byte[][] upcomingUVMotionTextureArrayPixelsPerLevel;
				lock (setupLock2)
				{
					foreach (KeyValuePair<string, ParticleSpawner> keyValuePair3 in updateParticleSpawners.ParticleSpawners)
					{
						this._networkParticleSpawners.Remove(keyValuePair3.Key);
					}
					upcomingParticleSystems = new Dictionary<string, ParticleSystem>(this._networkParticleSystems);
					Dictionary<string, ParticleSpawner> networkParticleSpawners = new Dictionary<string, ParticleSpawner>(this._networkParticleSpawners);
					upcomingParticleSpawners = new Dictionary<string, ParticleSpawner>(updateParticleSpawners.ParticleSpawners);
					this._gameInstance.ParticleSystemStoreModule.PrepareParticles(networkParticleSpawners, out upcomingParticles, out this._upcomingParticleTextureInfo, out this._upcomingUVMotionTexturePaths, this._threadCancellationToken);
					bool isCancellationRequested3 = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested3)
					{
						break;
					}
					this._gameInstance.FXModule.PrepareAtlas(this._upcomingParticleTextureInfo, this._upcomingTrailTextureInfo, out upcomingFXImageLocations, out upcomingFXAtlasPixelsPerLevel, this._threadCancellationToken);
					bool isCancellationRequested4 = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested4)
					{
						break;
					}
					this._gameInstance.FXModule.PrepareUVMotionTextureArray(this._upcomingUVMotionTexturePaths, out upcomingUVMotionTextureArrayPixelsPerLevel);
				}
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.ParticleSystemStoreModule.RemoveParticleSpawners(upcomingParticleSystems, upcomingParticleSpawners, upcomingParticles);
					this._gameInstance.FXModule.CreateAtlasTextures(upcomingFXImageLocations, upcomingFXAtlasPixelsPerLevel);
					this._gameInstance.FXModule.CreateUVMotionTextureArray(upcomingUVMotionTextureArrayPixelsPerLevel);
					this._gameInstance.ParticleSystemStoreModule.ResetParticleSystems(false);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] ParticleSpawners: Finished {0} in {1}", updateParticleSpawners.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
				break;
			}
			default:
				throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateParticleSpawners.GetType().Name, this._stage, updateParticleSpawners.Type));
			}
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x0005B758 File Offset: 0x00059958
		private void ProcessSpawnParticleSystemPacket(SpawnParticleSystem packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int systemCount = this._gameInstance.ParticleSystemStoreModule.SystemCount;
			bool flag = systemCount > this._gameInstance.ParticleSystemStoreModule.MaxSpawnedSystems;
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Particle system limit reached: " + this._gameInstance.ParticleSystemStoreModule.MaxSpawnedSystems.ToString() + ": " + ((packet != null) ? packet.ToString() : null));
			}
			else
			{
				SpawnParticleSystem spawnParticleSystem = new SpawnParticleSystem(packet);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					ParticleSystemProxy particleSystemProxy;
					bool flag2 = !this._gameInstance.ParticleSystemStoreModule.TrySpawnSystem(spawnParticleSystem.ParticleSystemId, out particleSystemProxy, false, false);
					if (!flag2)
					{
						particleSystemProxy.Position = new Vector3((float)spawnParticleSystem.Position_.X, (float)spawnParticleSystem.Position_.Y, (float)spawnParticleSystem.Position_.Z);
						particleSystemProxy.Rotation = ((spawnParticleSystem.Rotation != null) ? Quaternion.CreateFromYawPitchRoll(spawnParticleSystem.Rotation.Yaw, spawnParticleSystem.Rotation.Pitch, spawnParticleSystem.Rotation.Roll) : Quaternion.Identity);
						bool flag3 = spawnParticleSystem.Color_ != null;
						if (flag3)
						{
							particleSystemProxy.DefaultColor = UInt32Color.FromRGBA((byte)spawnParticleSystem.Color_.Red, (byte)spawnParticleSystem.Color_.Green, (byte)spawnParticleSystem.Color_.Blue, byte.MaxValue);
						}
						particleSystemProxy.Scale = spawnParticleSystem.Scale;
					}
				}, false, false);
			}
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x0005B824 File Offset: 0x00059A24
		private void ProcessSpawnBlockParticleSystem(SpawnBlockParticleSystem packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int systemCount = this._gameInstance.ParticleSystemStoreModule.SystemCount;
			bool flag = systemCount > this._gameInstance.ParticleSystemStoreModule.MaxSpawnedSystems;
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Particle system limit reached: " + this._gameInstance.ParticleSystemStoreModule.MaxSpawnedSystems.ToString() + ": " + ((packet != null) ? packet.ToString() : null));
			}
			else
			{
				Vector3 position = new Vector3((float)packet.Position_.X, (float)packet.Position_.Y, (float)packet.Position_.Z);
				int blockId = packet.BlockId;
				ClientBlockParticleEvent particleType = packet.ParticleType;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					ClientBlockType blockType = this._gameInstance.MapModule.ClientBlockTypes[blockId];
					ParticleSystemProxy particleSystemProxy;
					bool flag2 = this._gameInstance.ParticleSystemStoreModule.TrySpawnBlockSystem(position, blockType, particleType, out particleSystemProxy, true, false);
					if (flag2)
					{
						particleSystemProxy.Position = position + particleSystemProxy.Position;
					}
				}, false, false);
			}
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x0005B92C File Offset: 0x00059B2C
		private void ProcessSpawnModelParticles(SpawnModelParticles packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			Entity entity = this._gameInstance.EntityStoreModule.GetEntity(packet.EntityId);
			ModelParticle[] modelParticles = packet.ModelParticles;
			bool flag = entity == null || modelParticles == null || modelParticles.Length == 0;
			if (!flag)
			{
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					ModelParticleSettings[] modelParticles;
					ParticleProtocolInitializer.Initialize(modelParticles, out modelParticles, this._gameInstance.EntityStoreModule.NodeNameManager);
					entity.AddModelParticles(modelParticles);
				}, false, false);
			}
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x0005B9C0 File Offset: 0x00059BC0
		private void ProcessInventoryPacket(UpdatePlayerInventory packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			UpdatePlayerInventory updatePlayerInventory = new UpdatePlayerInventory(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.InventoryModule.SetInventory(updatePlayerInventory);
			}, false, false);
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x0005BA18 File Offset: 0x00059C18
		private void ProcessDisplayDebug(DisplayDebug packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			Matrix matrix = new Matrix(packet.Matrix);
			Vector3 color = new Vector3(packet.Color.X, packet.Color.Y, packet.Color.Z);
			float time = packet.Time;
			bool fade = packet.Fade;
			DisplayDebug.DebugShape shape = packet.Shape;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.DebugDisplayModule.Add(shape, matrix, time, color, fade);
			}, false, false);
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x0005BAC4 File Offset: 0x00059CC4
		private void ProcessSetActiveSlot(SetActiveSlot packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			int inventorySectionId = packet.InventorySectionId;
			int activeSlot = packet.ActiveSlot;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				InventorySectionType inventorySectionId = (InventorySectionType)inventorySectionId;
				InventorySectionType inventorySectionType = inventorySectionId;
				switch (inventorySectionType)
				{
				case InventorySectionType.Tools:
					this._gameInstance.InventoryModule.SetActiveToolsSlot(activeSlot, false, false);
					return;
				case InventorySectionType.BuilderMaterial:
					break;
				case InventorySectionType.Consumable:
					this._gameInstance.InventoryModule.SetActiveConsumableSlot(activeSlot, false, false);
					return;
				case InventorySectionType.Utility:
					this._gameInstance.InventoryModule.SetActiveUtilitySlot(activeSlot, false);
					return;
				default:
					if (inventorySectionType == InventorySectionType.Hotbar)
					{
						this._gameInstance.InventoryModule.SetActiveHotbarSlot(activeSlot, false);
						return;
					}
					break;
				}
				throw new Exception("Inventory section with id " + inventorySectionId.ToString() + " cannot select an active slot");
			}, false, false);
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x0005BB28 File Offset: 0x00059D28
		private void ProcessSetGameModePacket(SetGameMode packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			GameMode gameMode = packet.GameMode_;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.SetGameMode(gameMode, false);
			}, false, false);
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x0005BB80 File Offset: 0x00059D80
		private void ProcessSetMovementStates(SetMovementStates packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			SavedMovementStates movementStates = new SavedMovementStates(packet.MovementStates);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.CharacterControllerModule.MovementController.SetSavedMovementStates(movementStates);
			}, false, false);
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x0005BBDC File Offset: 0x00059DDC
		private void ProcessUpdateMovementSettings(UpdateMovementSettings packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			MovementSettings movementSettings = new MovementSettings(packet.MovementSettings_);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.CharacterControllerModule.MovementController.UpdateMovementSettings(movementSettings);
			}, false, false);
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x0005BC38 File Offset: 0x00059E38
		private void ProcessCameraShakeProfiles(UpdateCameraShake packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.CameraModule.CameraShakeController.UpdateCameraShakeAssets(packet);
			}, false, false);
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x0005BC88 File Offset: 0x00059E88
		private void ProcessCameraEffect(CameraShakeEffect packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.CameraModule.CameraShakeController.PlayCameraShake(packet.CameraShakeId, packet.Intensity, packet.Mode);
			}, false, false);
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x0005BCD8 File Offset: 0x00059ED8
		private void ProcessViewBobbingProfiles(UpdateViewBobbing packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.CameraModule.CameraShakeController.UpdateViewBobbingAssets(packet);
			}, false, false);
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x0005BD28 File Offset: 0x00059F28
		private void ProcessChangeVelocity(ChangeVelocity packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			bool flag = !this.ValidateFloat(packet.X, float.NegativeInfinity, float.PositiveInfinity) || !this.ValidateFloat(packet.Y, float.NegativeInfinity, float.PositiveInfinity) || !this.ValidateFloat(packet.Z, float.NegativeInfinity, float.PositiveInfinity);
			if (flag)
			{
				this._gameInstance.App.DevTools.Error(string.Format("Invalid packet data: {0}", packet));
			}
			else
			{
				ChangeVelocity changeVelocity = new ChangeVelocity(packet);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.CharacterControllerModule.MovementController.RequestVelocityChange(changeVelocity.X, changeVelocity.Y, changeVelocity.Z, changeVelocity.ChangeType, changeVelocity.Config);
				}, false, false);
			}
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x0005BDF0 File Offset: 0x00059FF0
		private void ProcessApplyKnockback(ApplyKnockback packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			bool flag = !this.ValidateFloat(packet.X, float.NegativeInfinity, float.PositiveInfinity) || !this.ValidateFloat(packet.Y, float.NegativeInfinity, float.PositiveInfinity) || !this.ValidateFloat(packet.Z, float.NegativeInfinity, float.PositiveInfinity);
			if (flag)
			{
				this._gameInstance.App.DevTools.Error(string.Format("Invalid packet data: {0}", packet));
			}
			else
			{
				ApplyKnockback applyKnockback = new ApplyKnockback(packet);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.CharacterControllerModule.MovementController.ApplyKnockback(applyKnockback);
				}, false, false);
			}
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x0005BEB8 File Offset: 0x0005A0B8
		private void ProcessSetServerCamera(SetServerCamera packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			SetServerCamera setServerCamera = new SetServerCamera(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				bool flag = setServerCamera.ClientCameraView_ == 2;
				if (flag)
				{
					bool flag2 = setServerCamera.CameraSettings == null;
					if (flag2)
					{
						this._gameInstance.CameraModule.ResetCameraController();
					}
					else
					{
						ServerCameraController serverCameraController = this._gameInstance.CameraModule.Controller as ServerCameraController;
						bool flag3 = serverCameraController != null;
						if (flag3)
						{
							serverCameraController.CameraSettings = setServerCamera.CameraSettings;
						}
						else
						{
							this._gameInstance.CameraModule.SetCustomCameraController(new ServerCameraController(this._gameInstance, setServerCamera.CameraSettings));
						}
					}
				}
				else
				{
					this._gameInstance.CameraModule.SetCameraControllerIndex(setServerCamera.ClientCameraView_);
					bool isLocked = setServerCamera.IsLocked;
					if (isLocked)
					{
						this._gameInstance.CameraModule.LockCamera();
					}
				}
			}, false, false);
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x0005BF0C File Offset: 0x0005A10C
		private void ProcessSyncInteractionChain(SyncInteractionChain packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			SyncInteractionChain syncInteractionCHain = new SyncInteractionChain(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.InteractionModule.Handle(syncInteractionCHain);
			}, false, false);
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x0005BF60 File Offset: 0x0005A160
		private void ProcessCancelInteractionChain(CancelInteractionChain packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			CancelInteractionChain cancelInteractionChain = new CancelInteractionChain(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.InteractionModule.Handle(cancelInteractionChain);
			}, false, false);
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x0005BFB4 File Offset: 0x0005A1B4
		private void ProcessPlayInteractionFor(PlayInteractionFor packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			PlayInteractionFor playInteractionFor = new PlayInteractionFor(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				ClientInteraction clientInteraction = this._gameInstance.InteractionModule.Interactions[playInteractionFor.InteractionId];
				Entity entity = this._gameInstance.EntityStoreModule.GetEntity(playInteractionFor.EntityId);
				bool flag = entity == null;
				if (!flag)
				{
					bool isFirstInteraction = playInteractionFor.OperationIndex == 0;
					bool cancel = playInteractionFor.Cancel;
					InteractionMetaStore interactionMetaStore;
					if (cancel)
					{
						Dictionary<int, InteractionMetaStore> dictionary;
						bool flag2 = !entity.InteractionMetaStores.TryGetValue(new ValueTuple<int, ForkedChainId>(playInteractionFor.ChainId, playInteractionFor.ForkedId), out dictionary);
						if (flag2)
						{
							return;
						}
						bool flag3 = !dictionary.TryGetValue(playInteractionFor.OperationIndex, out interactionMetaStore);
						if (flag3)
						{
							return;
						}
						dictionary.Remove(playInteractionFor.OperationIndex);
						bool flag4 = dictionary.Count == 0;
						if (flag4)
						{
							entity.InteractionMetaStores.Remove(new ValueTuple<int, ForkedChainId>(playInteractionFor.ChainId, playInteractionFor.ForkedId));
						}
					}
					else
					{
						Dictionary<int, InteractionMetaStore> dictionary2;
						bool flag5 = !entity.InteractionMetaStores.TryGetValue(new ValueTuple<int, ForkedChainId>(playInteractionFor.ChainId, playInteractionFor.ForkedId), out dictionary2);
						if (flag5)
						{
							dictionary2 = new Dictionary<int, InteractionMetaStore>();
							entity.InteractionMetaStores.Add(new ValueTuple<int, ForkedChainId>(playInteractionFor.ChainId, playInteractionFor.ForkedId), dictionary2);
						}
						interactionMetaStore = new InteractionMetaStore();
						dictionary2.Add(playInteractionFor.OperationIndex, interactionMetaStore);
					}
					bool flag6 = playInteractionFor.InteractionType_ == 6 && entity.NetworkId != this._gameInstance.LocalPlayer.NetworkId;
					if (flag6)
					{
						entity.ConsumableItem = this._gameInstance.ItemLibraryModule.GetItem(playInteractionFor.InteractedItemId);
					}
					InteractionContext interactionContext = null;
					bool flag7 = entity.NetworkId == this._gameInstance.LocalPlayer.NetworkId;
					if (flag7)
					{
						InteractionChain interactionChain;
						this._gameInstance.InteractionModule.Chains.TryGetValue(playInteractionFor.ChainId, out interactionChain);
						ForkedChainId forkedId = playInteractionFor.ForkedId;
						while (forkedId != null && interactionChain != null)
						{
							bool flag8 = !interactionChain.GetForkedChain(forkedId, out interactionChain);
							if (flag8)
							{
								return;
							}
							forkedId = forkedId.ForkedId;
						}
						interactionContext = ((interactionChain != null) ? interactionChain.Context : null);
					}
					bool flag9 = interactionContext == null;
					if (flag9)
					{
						interactionContext = InteractionContext.ForInteraction(entity, playInteractionFor.InteractionType_);
					}
					clientInteraction.HandlePlayFor(this._gameInstance, entity, playInteractionFor.InteractionType_, interactionContext, interactionMetaStore, playInteractionFor.Cancel, isFirstInteraction, false);
				}
			}, false, false);
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x0005C008 File Offset: 0x0005A208
		private void ProcessMountNpc(MountNPC packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			MountNPC mountNpc = new MountNPC(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.CharacterControllerModule.MountNpc(mountNpc);
			}, false, false);
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x0005C05C File Offset: 0x0005A25C
		private void ProcessDismountNpc(DismountNPC packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.CharacterControllerModule.DismountNpc(false);
			}, false, false);
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x0005C08C File Offset: 0x0005A28C
		private void ProcessAddToPlayerListPacket(AddToPlayerList packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup | PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			PacketHandler.PlayerListPlayer[] players = new PacketHandler.PlayerListPlayer[packet.Players.Length];
			for (int i = 0; i < packet.Players.Length; i++)
			{
				players[i] = new PacketHandler.PlayerListPlayer(packet.Players[i]);
			}
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.InGameView.OnPlayerListAdd(players);
			}, false, false);
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x0005C118 File Offset: 0x0005A318
		private void ProcessRemoveFromPlayerListPacket(RemoveFromPlayerList packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup | PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			Guid[] players = (Guid[])packet.Players.Clone();
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.InGameView.OnPlayerListRemove(players);
			}, false, false);
		}

		// Token: 0x060034ED RID: 13549 RVA: 0x0005C178 File Offset: 0x0005A378
		private void ProcessUpdatePlayerListPacket(UpdatePlayerList packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup | PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			Dictionary<Guid, int> players = new Dictionary<Guid, int>(packet.Players);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.InGameView.OnPlayerListUpdate(players);
			}, false, false);
		}

		// Token: 0x060034EE RID: 13550 RVA: 0x0005C1D2 File Offset: 0x0005A3D2
		private void ProcessClearPlayerListPacket(ClearPlayerList packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup | PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.Interface.InGameView.OnPlayerListClear();
			}, false, false);
		}

		// Token: 0x060034EF RID: 13551 RVA: 0x0005C204 File Offset: 0x0005A404
		private void ProcessWorldSettingsPacket(WorldSettings packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup);
			this.SetStage(PacketHandler.ConnectionStage.SettingUp);
			bool flag = packet.WorldHeight % 32 != 0;
			if (flag)
			{
				throw new Exception(string.Format("Invalid world height. Must be a multiple of {0}: {1}", 32, packet.WorldHeight));
			}
			ChunkHelper.SetHeight(packet.WorldHeight);
			Asset[] requiredAssets = null;
			bool flag2 = packet.RequiredAssets != null;
			if (flag2)
			{
				requiredAssets = new Asset[packet.RequiredAssets.Length];
				for (int i = 0; i < packet.RequiredAssets.Length; i++)
				{
					bool flag3 = packet.RequiredAssets[i] != null;
					if (flag3)
					{
						requiredAssets[i] = new Asset(packet.RequiredAssets[i]);
					}
				}
			}
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				bool flag4 = requiredAssets != null;
				if (flag4)
				{
					long elapsedMilliseconds = this._connectionStopwatch.ElapsedMilliseconds;
					PacketHandler.Logger.Info("Handle RequiredAssets at {0}ms", elapsedMilliseconds);
					List<Asset> list = new List<Asset>(requiredAssets.Length);
					for (int j = 0; j < requiredAssets.Length; j++)
					{
						Asset asset = requiredAssets[j];
						string hash = asset.Hash;
						string name = asset.Name;
						this._gameInstance.RegisterHashForServerAsset(name, hash);
						bool flag5;
						AssetManager.MarkAssetAsServerRequired(name, hash, out flag5);
						bool flag6 = !flag5;
						if (flag6)
						{
							list.Add(asset);
						}
					}
					PacketHandler.Logger.Info("Finished handling RequiredAssets, took {0}ms", this._connectionStopwatch.ElapsedMilliseconds - elapsedMilliseconds);
					this._gameInstance.Connection.SendPacket(new RequestAssets(list.ToArray()));
				}
				else
				{
					PacketHandler.Logger.Info("Handle RequiredAssets at {0}ms, No assets to process", this._connectionStopwatch.ElapsedMilliseconds);
					this._gameInstance.Connection.SendPacket(new RequestAssets());
				}
			}, false, false);
		}

		// Token: 0x060034F0 RID: 13552 RVA: 0x0005C2FC File Offset: 0x0005A4FC
		private void ProcessServerTagsPacket(ServerTags packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup | PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
			newServerSettings.ServerTags = new ServerTags(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.SetServerSettings(newServerSettings);
			}, false, false);
		}

		// Token: 0x060034F1 RID: 13553 RVA: 0x0005C368 File Offset: 0x0005A568
		private void ProcessServerInfoPacket(ServerInfo packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.WaitingForSetup | PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.App.InGame.ServerName = packet.ServerName;
				this._gameInstance.App.Interface.InGameView.OnServerInfoUpdate(packet.ServerName, packet.Motd, packet.MaxPlayers);
			}, false, false);
		}

		// Token: 0x060034F2 RID: 13554 RVA: 0x0005C3B8 File Offset: 0x0005A5B8
		private void ProcessWorldLoadProgressPacket(WorldLoadProgress packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
			string status = string.Copy(packet.Status);
			int percentComplete = packet.PercentComplete;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				App app = this._gameInstance.App;
				bool flag = app.Stage == App.AppStage.GameLoading;
				if (flag)
				{
					app.Interface.GameLoadingView.SetStatus(status, (float)percentComplete);
				}
			}, false, false);
		}

		// Token: 0x060034F3 RID: 13555 RVA: 0x0005C420 File Offset: 0x0005A620
		private void ProcessWorldLoadFinishedPacket(WorldLoadFinished packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
			this.ValidateReceivedAssets();
			this.SetStage(PacketHandler.ConnectionStage.Playing);
			DateTime startTime = DateTime.UtcNow;
			EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
			int tasksCount = 9;
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool atlasNeedsUpdate = true;
				byte[][] upcomingBlocksAtlasPixelsPerLevel;
				this._gameInstance.MapModule.PrepareBlockTypes(this._networkBlockTypes, this._highestReceivedBlockId, atlasNeedsUpdate, ref this._upcomingBlockTypes, ref this._upcomingBlocksImageLocations, ref this._upcomingBlocksAtlasSize, out upcomingBlocksAtlasPixelsPerLevel, this._threadCancellationToken);
				bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested2)
				{
					ClientBlockType[] upcomingBlockTypes = new ClientBlockType[this._upcomingBlockTypes.Length];
					Array.Copy(this._upcomingBlockTypes, upcomingBlockTypes, this._upcomingBlockTypes.Length);
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						this._gameInstance.MapModule.SetupBlockTypes(upcomingBlockTypes, true);
						this._gameInstance.MapModule.TextureAtlas.UpdateTexture2DMipMaps(upcomingBlocksAtlasPixelsPerLevel);
					}, false, false);
					int num = Interlocked.Decrement(ref tasksCount);
					bool flag2 = num == 0;
					if (flag2)
					{
						waitHandle.Set();
					}
					PacketHandler.Logger.Info("PrepareBlockTypes: {0} ms", stopwatch.ElapsedMilliseconds);
				}
			});
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				byte[][] upcomingEntitiesAtlasPixelsPerLevel;
				this._gameInstance.EntityStoreModule.PrepareAtlas(out this._upcomingEntitiesImageLocations, out upcomingEntitiesAtlasPixelsPerLevel, this._threadCancellationToken);
				bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested2)
				{
					Dictionary<string, Point> upcomingEntitiesImageLocations = this._upcomingEntitiesImageLocations;
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						this._gameInstance.EntityStoreModule.CreateAtlasTexture(upcomingEntitiesImageLocations, upcomingEntitiesAtlasPixelsPerLevel);
						this._gameInstance.EntityStoreModule.SetupModelsAndAnimations();
					}, false, false);
					int num = Interlocked.Decrement(ref tasksCount);
					bool flag2 = num == 0;
					if (flag2)
					{
						waitHandle.Set();
					}
					PacketHandler.Logger.Info("PrepareEntitiesAtlas: {0}ms", stopwatch.ElapsedMilliseconds);
				}
			});
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._gameInstance.ItemLibraryModule.PrepareItems(this._networkItems, null, ref this._upcomingItems, this._threadCancellationToken);
				bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested2)
				{
					Dictionary<string, ClientIcon> upcomingItemIcons;
					byte[] iconAtlasPixels;
					int iconAtlasWidth;
					int iconAtlasHeight;
					this._gameInstance.ItemLibraryModule.PrepareItemIconAtlas(this._networkItems, out upcomingItemIcons, out iconAtlasPixels, out iconAtlasWidth, out iconAtlasHeight, this._threadCancellationToken);
					bool isCancellationRequested3 = this._threadCancellationToken.IsCancellationRequested;
					if (!isCancellationRequested3)
					{
						this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
						{
							this._gameInstance.ItemLibraryModule.SetupItemIcons(upcomingItemIcons, iconAtlasPixels, iconAtlasWidth, iconAtlasHeight);
						}, false, false);
						int num = Interlocked.Decrement(ref tasksCount);
						bool flag2 = num == 0;
						if (flag2)
						{
							waitHandle.Set();
						}
						PacketHandler.Logger.Info("PrepareItems: {0}ms", stopwatch.ElapsedMilliseconds);
					}
				}
			});
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				Dictionary<string, ParticleSettings> upcomingParticles;
				this._gameInstance.ParticleSystemStoreModule.PrepareParticles(this._networkParticleSpawners, out upcomingParticles, out this._upcomingParticleTextureInfo, out this._upcomingUVMotionTexturePaths, this._threadCancellationToken);
				bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested2)
				{
					this._gameInstance.TrailStoreModule.PrepareTrails(this._networkTrails, out this._upcomingTrailTextureInfo, this._threadCancellationToken);
					bool isCancellationRequested3 = this._threadCancellationToken.IsCancellationRequested;
					if (!isCancellationRequested3)
					{
						Dictionary<string, Rectangle> upcomingFXImageLocations;
						byte[][] upcomingAtlasPixelsPerLevel;
						this._gameInstance.FXModule.PrepareAtlas(this._upcomingParticleTextureInfo, this._upcomingTrailTextureInfo, out upcomingFXImageLocations, out upcomingAtlasPixelsPerLevel, this._threadCancellationToken);
						bool isCancellationRequested4 = this._threadCancellationToken.IsCancellationRequested;
						if (!isCancellationRequested4)
						{
							byte[][] upcomingUVMotionTextureArrayPixelsPerLevel;
							this._gameInstance.FXModule.PrepareUVMotionTextureArray(this._upcomingUVMotionTexturePaths, out upcomingUVMotionTextureArrayPixelsPerLevel);
							EntityEffect[] upcomingEntityEffects;
							this._gameInstance.EntityStoreModule.PrepareEntityEffects(this._entityEffects, out upcomingEntityEffects);
							ModelVFX[] upcomingModelVFXs;
							this._gameInstance.EntityStoreModule.PrepareModelVFXs(this._modelVFXs, out upcomingModelVFXs);
							this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
							{
								this._gameInstance.ParticleSystemStoreModule.SetupParticleSpawners(this._networkParticleSystems, this._networkParticleSpawners, upcomingParticles, this._upcomingUVMotionTexturePaths);
								this._gameInstance.TrailStoreModule.SetupTrailSettings(this._networkTrails);
								this._gameInstance.FXModule.CreateUVMotionTextureArray(upcomingUVMotionTextureArrayPixelsPerLevel);
								this._gameInstance.FXModule.CreateAtlasTextures(upcomingFXImageLocations, upcomingAtlasPixelsPerLevel);
								this._gameInstance.EntityStoreModule.SetupEntityEffects(upcomingEntityEffects);
								this._gameInstance.EntityStoreModule.SetupModelVFXs(upcomingModelVFXs);
							}, false, false);
							int num = Interlocked.Decrement(ref tasksCount);
							bool flag2 = num == 0;
							if (flag2)
							{
								waitHandle.Set();
							}
							PacketHandler.Logger.Info("PrepareParticles: {0}ms", stopwatch.ElapsedMilliseconds);
						}
					}
				}
			});
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._gameInstance.ItemLibraryModule.PrepareItemPlayerAnimations(this._networkItemPlayerAnimations, out this._upcomingItemPlayerAnimations);
				bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested2)
				{
					Dictionary<string, ClientItemPlayerAnimations> upcomingItemAnimations = this._upcomingItemPlayerAnimations;
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						this._gameInstance.ItemLibraryModule.SetupItemPlayerAnimations(upcomingItemAnimations);
					}, false, false);
					int num = Interlocked.Decrement(ref tasksCount);
					bool flag2 = num == 0;
					if (flag2)
					{
						waitHandle.Set();
					}
					PacketHandler.Logger.Info("PrepareItemAnimations: {0}ms", stopwatch.ElapsedMilliseconds);
				}
			});
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._gameInstance.InteractionModule.PrepareInteractions(this._networkInteractions, out this._upcomingInteractions);
				bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested2)
				{
					ClientInteraction[] upcomingInteractions = this._upcomingInteractions;
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						this._gameInstance.InteractionModule.SetupInteractions(upcomingInteractions);
					}, false, false);
					int num = Interlocked.Decrement(ref tasksCount);
					bool flag2 = num == 0;
					if (flag2)
					{
						waitHandle.Set();
					}
					Trace.WriteLine(string.Format("PrepareInterations: {0}ms", stopwatch.ElapsedMilliseconds), "Loading world");
				}
			});
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._gameInstance.InteractionModule.PrepareRootInteractions(this._networkRootInteractions, out this._upcomingRootInteractions);
				bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested2)
				{
					ClientRootInteraction[] upcomingRootInteractions = this._upcomingRootInteractions;
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						this._gameInstance.InteractionModule.SetupRootInteractions(upcomingRootInteractions);
					}, false, false);
					int num = Interlocked.Decrement(ref tasksCount);
					bool flag2 = num == 0;
					if (flag2)
					{
						waitHandle.Set();
					}
					Trace.WriteLine(string.Format("PrepareRootInterations: {0}ms", stopwatch.ElapsedMilliseconds), "Loading world");
				}
			});
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				byte[][] upcomingWorldMapAtlasPixelsPerLevel;
				this._gameInstance.WorldMapModule.PrepareTextureAtlas(out this._upcomingWorldMapImageLocations, out upcomingWorldMapAtlasPixelsPerLevel, this._threadCancellationToken);
				bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested2)
				{
					Dictionary<string, WorldMapModule.Texture> upcomingWorldMapImageLocations = this._upcomingWorldMapImageLocations;
					this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
					{
						this._gameInstance.WorldMapModule.BuildTextureAtlas(upcomingWorldMapImageLocations, upcomingWorldMapAtlasPixelsPerLevel);
					}, false, false);
					int num = Interlocked.Decrement(ref tasksCount);
					bool flag2 = num == 0;
					if (flag2)
					{
						waitHandle.Set();
					}
					PacketHandler.Logger.Info("PrepareWorldMap: {0}ms", stopwatch.ElapsedMilliseconds);
				}
			});
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				AmbienceFX[] clonedAmbienceFXs = new AmbienceFX[this._networkAmbienceFXs.Length];
				for (int i = 0; i < this._networkAmbienceFXs.Length; i++)
				{
					clonedAmbienceFXs[i] = new AmbienceFX(this._networkAmbienceFXs[i]);
				}
				AmbienceFXSettings[] ambienceFXSettings;
				this._gameInstance.AmbienceFXModule.PrepareAmbienceFXs(clonedAmbienceFXs, out ambienceFXSettings);
				Dictionary<string, WwiseResource> upcomingWwiseIds = null;
				this._gameInstance.AudioModule.PrepareSoundBanks(out upcomingWwiseIds);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.AudioModule.SetupSoundBanks(upcomingWwiseIds);
					this._gameInstance.AmbienceFXModule.SetupAmbienceFXs(clonedAmbienceFXs, ambienceFXSettings);
					this._gameInstance.AudioModule.OnSoundEffectCollectionChanged();
					this._gameInstance.InteractionModule.BlockPreview.RegisterSoundObjectReference();
				}, false, false);
				int num = Interlocked.Decrement(ref tasksCount);
				bool flag2 = num == 0;
				if (flag2)
				{
					waitHandle.Set();
				}
				PacketHandler.Logger.Info("PrepareAmbienceFX: {0}ms", stopwatch.ElapsedMilliseconds);
			});
			while (!this._threadCancellationToken.IsCancellationRequested)
			{
				bool flag = waitHandle.WaitOne(100);
				if (flag)
				{
					break;
				}
			}
			bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				this._gameInstance.Connection.SendPacket(new ViewRadius(this._gameInstance.App.Settings.ViewDistance));
				ClientPlayerSkin playerSkin = this._gameInstance.App.PlayerSkin;
				BodyType bodyType = (playerSkin.BodyType == CharacterBodyType.Masculine) ? 0 : 1;
				string skinTone = playerSkin.SkinTone;
				string face = playerSkin.Face;
				CharacterPartId eyes = playerSkin.Eyes;
				string text = (eyes != null) ? eyes.ToString() : null;
				CharacterPartId facialHair = playerSkin.FacialHair;
				string text2 = (facialHair != null) ? facialHair.ToString() : null;
				CharacterPartId haircut = playerSkin.Haircut;
				string text3 = (haircut != null) ? haircut.ToString() : null;
				CharacterPartId eyebrows = playerSkin.Eyebrows;
				string text4 = (eyebrows != null) ? eyebrows.ToString() : null;
				CharacterPartId pants = playerSkin.Pants;
				string text5 = (pants != null) ? pants.ToString() : null;
				CharacterPartId overpants = playerSkin.Overpants;
				string text6 = (overpants != null) ? overpants.ToString() : null;
				CharacterPartId undertop = playerSkin.Undertop;
				string text7 = (undertop != null) ? undertop.ToString() : null;
				CharacterPartId overtop = playerSkin.Overtop;
				string text8 = (overtop != null) ? overtop.ToString() : null;
				CharacterPartId shoes = playerSkin.Shoes;
				string text9 = (shoes != null) ? shoes.ToString() : null;
				CharacterPartId headAccessory = playerSkin.HeadAccessory;
				string text10 = (headAccessory != null) ? headAccessory.ToString() : null;
				CharacterPartId faceAccessory = playerSkin.FaceAccessory;
				string text11 = (faceAccessory != null) ? faceAccessory.ToString() : null;
				CharacterPartId earAccessory = playerSkin.EarAccessory;
				string text12 = (earAccessory != null) ? earAccessory.ToString() : null;
				CharacterPartId skinFeature = playerSkin.SkinFeature;
				string text13 = (skinFeature != null) ? skinFeature.ToString() : null;
				CharacterPartId gloves = playerSkin.Gloves;
				PlayerSkin playerSkin2 = new PlayerSkin(bodyType, skinTone, face, text, text2, text3, text4, text5, text6, text7, text8, text9, text10, text11, text12, text13, (gloves != null) ? gloves.ToString() : null);
				this._gameInstance.Connection.SendPacket(new PlayerOptions(new PlayerOptions(playerSkin2)));
				ServerSettings upcomingServerSettings = this._upcomingServerSettings.Clone();
				this._gameInstance.ItemLibraryModule.PrepareItemUVs(ref this._upcomingItems, this._upcomingEntitiesImageLocations, this._threadCancellationToken);
				Dictionary<string, ClientItemBase> upcomingItems = new Dictionary<string, ClientItemBase>(this._upcomingItems);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					Interface @interface = this._gameInstance.App.Interface;
					@interface.InGameView.ClearMarkupError();
					try
					{
						@interface.InGameCustomUIProvider.LoadDocuments();
					}
					catch (TextParser.TextParserException ex)
					{
						@interface.InGameCustomUIProvider.ClearDocuments();
						bool diagnosticMode = @interface.App.Settings.DiagnosticMode;
						if (!diagnosticMode)
						{
							this._gameInstance.DisconnectWithReason("Failed to load CustomUI documents", ex);
							return;
						}
						@interface.InGameView.DisplayMarkupError(ex.RawMessage, ex.Span);
					}
					catch (Exception exception)
					{
						@interface.InGameCustomUIProvider.ClearDocuments();
						this._gameInstance.DisconnectWithReason("Failed to load CustomUI documents", exception);
						return;
					}
					try
					{
						@interface.InGameCustomUIProvider.LoadTextures(@interface.Desktop.Scale > 1f);
					}
					catch (Exception exception2)
					{
						this._gameInstance.DisconnectWithReason("Failed to load CustomUI textures", exception2);
						return;
					}
					this._gameInstance.SetServerSettings(upcomingServerSettings);
					this._gameInstance.ItemLibraryModule.SetupItems(upcomingItems);
					@interface.TriggerEvent("items.initialized", upcomingItems, null, null, null, null, null);
					@interface.InGameView.ReloadAssetTextures();
					this._gameInstance.UpdateAtlasSizes();
					this._gameInstance.OnSetupComplete();
					DateTime utcNow = DateTime.UtcNow;
					PacketHandler.Logger.Info("Global: {0}ms", (utcNow - startTime).TotalMilliseconds);
				}, false, false);
			}
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x0005C73C File Offset: 0x0005A93C
		private void ProcessUpdateWeathersPacket(UpdateWeathers packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Weather: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.MaxId > this._upcomingServerSettings.Weathers.Length;
				if (flag)
				{
					Array.Resize<ClientWeather>(ref this._upcomingServerSettings.Weathers, packet.MaxId);
				}
				bool flag2 = packet.Type == 1;
				if (flag2)
				{
					foreach (KeyValuePair<int, Weather> keyValuePair in packet.Weathers)
					{
						this._upcomingServerSettings.Weathers[keyValuePair.Key] = new ClientWeather(keyValuePair.Value);
						this._upcomingServerSettings.WeatherIndicesByIds[keyValuePair.Value.Id] = keyValuePair.Key;
					}
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.WeatherModule.OnWeatherCollectionChanged();
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Weather: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.Weather);
				bool flag3 = this._upcomingServerSettings.Weathers == null;
				if (flag3)
				{
					this._upcomingServerSettings.Weathers = new ClientWeather[packet.MaxId];
					this._upcomingServerSettings.WeatherIndicesByIds = new Dictionary<string, int>();
				}
				foreach (KeyValuePair<int, Weather> keyValuePair2 in packet.Weathers)
				{
					this._upcomingServerSettings.Weathers[keyValuePair2.Key] = new ClientWeather(keyValuePair2.Value);
					this._upcomingServerSettings.WeatherIndicesByIds[keyValuePair2.Value.Id] = keyValuePair2.Key;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.Weather);
			}
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x0005C9E0 File Offset: 0x0005ABE0
		private void ProcessUpdateEntityStatTypes(UpdateEntityStatTypes packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] EntityStats: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.MaxId > this._upcomingServerSettings.EntityStatTypes.Length;
				if (flag)
				{
					Array.Resize<ClientEntityStatType>(ref this._upcomingServerSettings.EntityStatTypes, packet.MaxId);
					foreach (Entity entity in this._gameInstance.EntityStoreModule.GetAllEntities())
					{
						bool flag2 = entity == null;
						if (!flag2)
						{
							Array.Resize<ClientEntityStatValue>(ref entity._entityStats, packet.MaxId);
							Array.Resize<ClientEntityStatValue>(ref entity._serverEntityStats, packet.MaxId);
						}
					}
				}
				bool flag3 = packet.Type == 1;
				if (flag3)
				{
					foreach (KeyValuePair<int, EntityStatType> keyValuePair in packet.Types)
					{
						this._upcomingServerSettings.EntityStatTypes[keyValuePair.Key] = new ClientEntityStatType(keyValuePair.Value);
					}
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					DefaultEntityStats.Update(newServerSettings.EntityStatTypes);
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] EntityStats: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.EntityStatTypes);
				bool flag4 = this._upcomingServerSettings.EntityStatTypes == null;
				if (flag4)
				{
					this._upcomingServerSettings.EntityStatTypes = new ClientEntityStatType[packet.MaxId];
				}
				foreach (KeyValuePair<int, EntityStatType> keyValuePair2 in packet.Types)
				{
					this._upcomingServerSettings.EntityStatTypes[keyValuePair2.Key] = new ClientEntityStatType(keyValuePair2.Value);
				}
				DefaultEntityStats.Update(this._upcomingServerSettings.EntityStatTypes);
				this.FinishedReceivedAssetType(PacketHandler.AssetType.EntityStatTypes);
			}
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x0005CCA4 File Offset: 0x0005AEA4
		private void ProcessUpdateEnvironmentsPacket(UpdateEnvironments packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Environment: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.Type == 1;
				if (flag)
				{
					bool flag2 = packet.MaxId > this._upcomingServerSettings.Environments.Length;
					if (flag2)
					{
						ClientWorldEnvironment[] array = new ClientWorldEnvironment[packet.MaxId];
						Array.Copy(this._upcomingServerSettings.Environments, array, this._upcomingServerSettings.Environments.Length);
						this._upcomingServerSettings.Environments = array;
					}
					foreach (KeyValuePair<int, WorldEnvironment> keyValuePair in packet.Environments)
					{
						this._upcomingServerSettings.Environments[keyValuePair.Key] = new ClientWorldEnvironment(keyValuePair.Value);
					}
				}
				else
				{
					foreach (KeyValuePair<int, WorldEnvironment> keyValuePair2 in packet.Environments)
					{
						this._upcomingServerSettings.Environments[keyValuePair2.Key] = null;
					}
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				bool rebuildMapGeometry = packet.RebuildMapGeometry;
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.WeatherModule.OnEnvironmentCollectionChanged();
					bool rebuildMapGeometry = rebuildMapGeometry;
					if (rebuildMapGeometry)
					{
						this._gameInstance.MapModule.DoWithMapGeometryBuilderPaused(true, null);
					}
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Environment: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.Environment);
				this._upcomingServerSettings.Environments = new ClientWorldEnvironment[packet.MaxId];
				foreach (KeyValuePair<int, WorldEnvironment> keyValuePair3 in packet.Environments)
				{
					this._upcomingServerSettings.Environments[keyValuePair3.Key] = new ClientWorldEnvironment(keyValuePair3.Value);
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.Environment);
			}
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x0005CF54 File Offset: 0x0005B154
		private void ProcessUpdateFluidFXPacket(UpdateFluidFX packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] FluidFX: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.MaxId > this._upcomingServerSettings.FluidFXs.Length;
				if (flag)
				{
					FluidFX[] array = new FluidFX[packet.MaxId];
					Array.Copy(this._upcomingServerSettings.FluidFXs, array, this._upcomingServerSettings.FluidFXs.Length);
					this._upcomingServerSettings.FluidFXs = array;
				}
				foreach (KeyValuePair<int, FluidFX> keyValuePair in packet.FluidFX_)
				{
					this._upcomingServerSettings.FluidFXs[keyValuePair.Key] = keyValuePair.Value;
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.WeatherModule.OnFluidFXChanged();
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] FluidFX: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.FluidFX);
				bool flag2 = this._upcomingServerSettings.FluidFXs == null;
				if (flag2)
				{
					this._upcomingServerSettings.FluidFXs = new FluidFX[packet.MaxId];
				}
				foreach (KeyValuePair<int, FluidFX> keyValuePair2 in packet.FluidFX_)
				{
					this._upcomingServerSettings.FluidFXs[keyValuePair2.Key] = keyValuePair2.Value;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.FluidFX);
			}
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x0005D19C File Offset: 0x0005B39C
		private void ProcessUpdateRootInteractions(UpdateRootInteractions packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.Chat.Log(string.Format("[AssetUpdate] RootInteractions: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.MaxId > this._networkRootInteractions.Length;
				if (flag)
				{
					RootInteraction[] array = new RootInteraction[packet.MaxId];
					Array.Copy(this._networkRootInteractions, array, this._networkRootInteractions.Length);
					this._networkRootInteractions = array;
				}
				object setupLock = this._setupLock;
				ClientRootInteraction[] upcomingRootInteractions;
				lock (setupLock)
				{
					bool flag3 = packet.Type == 1;
					if (flag3)
					{
						foreach (KeyValuePair<int, RootInteraction> keyValuePair in packet.Interactions)
						{
							this._networkRootInteractions[keyValuePair.Key] = new RootInteraction(keyValuePair.Value);
						}
					}
					else
					{
						foreach (KeyValuePair<int, RootInteraction> keyValuePair2 in packet.Interactions)
						{
							this._networkRootInteractions[keyValuePair2.Key] = null;
						}
					}
					this._gameInstance.InteractionModule.PrepareRootInteractions(this._networkRootInteractions, out this._upcomingRootInteractions);
					bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					upcomingRootInteractions = this._upcomingRootInteractions;
				}
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.InteractionModule.SetupRootInteractions(upcomingRootInteractions);
					this._gameInstance.Chat.Log(string.Format("[AssetUpdate] RootInteractions: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.RootInteractions);
				bool flag4 = this._networkRootInteractions == null;
				if (flag4)
				{
					this._networkRootInteractions = new RootInteraction[packet.MaxId];
				}
				foreach (KeyValuePair<int, RootInteraction> keyValuePair3 in packet.Interactions)
				{
					this._networkRootInteractions[keyValuePair3.Key] = new RootInteraction(keyValuePair3.Value);
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.RootInteractions);
			}
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x0005D48C File Offset: 0x0005B68C
		private void ProcessUpdateInteractions(UpdateInteractions packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Interactions: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.MaxId > this._networkInteractions.Length;
				if (flag)
				{
					Interaction[] array = new Interaction[packet.MaxId];
					Array.Copy(this._networkInteractions, array, this._networkInteractions.Length);
					this._networkInteractions = array;
				}
				object setupLock = this._setupLock;
				ClientInteraction[] upcomingInteractions;
				lock (setupLock)
				{
					bool flag3 = packet.Type == 1;
					if (flag3)
					{
						foreach (KeyValuePair<int, Interaction> keyValuePair in packet.Interactions)
						{
							this._networkInteractions[keyValuePair.Key] = new Interaction(keyValuePair.Value);
						}
					}
					else
					{
						foreach (KeyValuePair<int, Interaction> keyValuePair2 in packet.Interactions)
						{
							this._networkInteractions[keyValuePair2.Key] = null;
						}
					}
					this._gameInstance.InteractionModule.PrepareInteractions(this._networkInteractions, out this._upcomingInteractions);
					bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					upcomingInteractions = this._upcomingInteractions;
				}
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.InteractionModule.SetupInteractions(upcomingInteractions);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Interactions: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.Interactions);
				bool flag4 = this._networkInteractions == null;
				if (flag4)
				{
					this._networkInteractions = new Interaction[packet.MaxId];
				}
				foreach (KeyValuePair<int, Interaction> keyValuePair3 in packet.Interactions)
				{
					this._networkInteractions[keyValuePair3.Key] = new Interaction(keyValuePair3.Value);
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.Interactions);
			}
		}

		// Token: 0x060034FA RID: 13562 RVA: 0x0005D780 File Offset: 0x0005B980
		private void ProcessUpdateUnarmedInteractions(UpdateUnarmedInteractions packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 != 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] UnarmedInteractions: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				this._upcomingServerSettings.UnarmedInteractions = packet.Interactions;
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] UnarmedInteractions: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.UnarmedInteractions);
				this._upcomingServerSettings.UnarmedInteractions = packet.Interactions;
				this.FinishedReceivedAssetType(PacketHandler.AssetType.UnarmedInteractions);
			}
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x0005D8B0 File Offset: 0x0005BAB0
		private void ProcessUpdateRepulsionConfig(UpdateRepulsionConfig packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] RepulsionConfig: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.MaxId > this._upcomingServerSettings.RepulsionConfigs.Length;
				if (flag)
				{
					ClientRepulsionConfig[] array = new ClientRepulsionConfig[packet.MaxId];
					Array.Copy(this._upcomingServerSettings.RepulsionConfigs, array, this._upcomingServerSettings.RepulsionConfigs.Length);
					this._upcomingServerSettings.RepulsionConfigs = array;
				}
				foreach (KeyValuePair<int, RepulsionConfig> keyValuePair in packet.RepulsionConfigs)
				{
					ClientRepulsionConfig clientRepulsionConfig = new ClientRepulsionConfig();
					ClientRepulsionConfigInitializer.Initialize(keyValuePair.Value, ref clientRepulsionConfig);
					this._upcomingServerSettings.RepulsionConfigs[keyValuePair.Key] = clientRepulsionConfig;
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] RepulsionConfig: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.RepulsionConfig);
				bool flag2 = this._upcomingServerSettings.RepulsionConfigs == null;
				if (flag2)
				{
					this._upcomingServerSettings.RepulsionConfigs = new ClientRepulsionConfig[packet.MaxId];
				}
				foreach (KeyValuePair<int, RepulsionConfig> keyValuePair2 in packet.RepulsionConfigs)
				{
					ClientRepulsionConfig clientRepulsionConfig2 = new ClientRepulsionConfig();
					ClientRepulsionConfigInitializer.Initialize(keyValuePair2.Value, ref clientRepulsionConfig2);
					this._upcomingServerSettings.RepulsionConfigs[keyValuePair2.Key] = clientRepulsionConfig2;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.RepulsionConfig);
			}
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x0005DB24 File Offset: 0x0005BD24
		private void ProcessUpdateHitboxCollisionConfig(UpdateHitboxCollisionConfig packet)
		{
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] HitboxCollisionConfig: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.MaxId > this._upcomingServerSettings.HitboxCollisionConfigs.Length;
				if (flag)
				{
					ClientHitboxCollisionConfig[] array = new ClientHitboxCollisionConfig[packet.MaxId];
					Array.Copy(this._upcomingServerSettings.HitboxCollisionConfigs, array, this._upcomingServerSettings.HitboxCollisionConfigs.Length);
					this._upcomingServerSettings.HitboxCollisionConfigs = array;
				}
				foreach (KeyValuePair<int, HitboxCollisionConfig> keyValuePair in packet.HitboxCollisionConfigs)
				{
					ClientHitboxCollisionConfig clientHitboxCollisionConfig = new ClientHitboxCollisionConfig();
					ClientHitboxCollisionConfigInitializer.Initialize(keyValuePair.Value, ref clientHitboxCollisionConfig);
					this._upcomingServerSettings.HitboxCollisionConfigs[keyValuePair.Key] = clientHitboxCollisionConfig;
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] HitboxCollisionConfig: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.HitboxCollisionConfig);
				bool flag2 = this._upcomingServerSettings.HitboxCollisionConfigs == null;
				if (flag2)
				{
					this._upcomingServerSettings.HitboxCollisionConfigs = new ClientHitboxCollisionConfig[packet.MaxId];
				}
				foreach (KeyValuePair<int, HitboxCollisionConfig> keyValuePair2 in packet.HitboxCollisionConfigs)
				{
					ClientHitboxCollisionConfig clientHitboxCollisionConfig2 = new ClientHitboxCollisionConfig();
					ClientHitboxCollisionConfigInitializer.Initialize(keyValuePair2.Value, ref clientHitboxCollisionConfig2);
					this._upcomingServerSettings.HitboxCollisionConfigs[keyValuePair2.Key] = clientHitboxCollisionConfig2;
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.HitboxCollisionConfig);
			}
		}

		// Token: 0x060034FD RID: 13565 RVA: 0x0005DD98 File Offset: 0x0005BF98
		private void ProcessUpdateEntityUIComponents(UpdateEntityUIComponents packet)
		{
			InGameView inGameView = this._gameInstance.App.Interface.InGameView;
			UpdateType type = packet.Type;
			UpdateType updateType2 = type;
			if (updateType2 != null)
			{
				if (updateType2 - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", packet.GetType().Name, this._stage, packet.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] EntityUIComponentConfig: Starting {0}", packet.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = packet.MaxId > this._upcomingServerSettings.EntityUIComponents.Length;
				if (flag)
				{
					ClientEntityUIComponent[] array = new ClientEntityUIComponent[packet.MaxId];
					Array.Copy(this._upcomingServerSettings.EntityUIComponents, array, this._upcomingServerSettings.EntityUIComponents.Length);
					this._upcomingServerSettings.EntityUIComponents = array;
				}
				foreach (KeyValuePair<int, EntityUIComponent> keyValuePair in packet.Components)
				{
					EntityUIComponent.EntityUIType type2 = keyValuePair.Value.Type;
					EntityUIComponent.EntityUIType entityUIType = type2;
					if (entityUIType != null)
					{
						if (entityUIType == 1)
						{
							this._upcomingServerSettings.EntityUIComponents[keyValuePair.Key] = new ClientCombatTextUIComponent(keyValuePair.Key, keyValuePair.Value, inGameView.EntityUIContainer.CombatTextUIComponentRenderer);
						}
					}
					else
					{
						this._upcomingServerSettings.EntityUIComponents[keyValuePair.Key] = new ClientEntityStatUIComponent(keyValuePair.Key, keyValuePair.Value, inGameView.EntityUIContainer.EntityStatUIComponentRenderer);
					}
				}
				ServerSettings newServerSettings = this._upcomingServerSettings.Clone();
				UpdateType updateType = packet.Type;
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.SetServerSettings(newServerSettings);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] EntityUIComponentConfig: Finished {0} in {1}", updateType, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.EntityUIComponents);
				bool flag2 = this._upcomingServerSettings.EntityUIComponents == null;
				if (flag2)
				{
					this._upcomingServerSettings.EntityUIComponents = new ClientEntityUIComponent[packet.MaxId];
				}
				foreach (KeyValuePair<int, EntityUIComponent> keyValuePair2 in packet.Components)
				{
					EntityUIComponent.EntityUIType type3 = keyValuePair2.Value.Type;
					EntityUIComponent.EntityUIType entityUIType2 = type3;
					if (entityUIType2 != null)
					{
						if (entityUIType2 == 1)
						{
							this._upcomingServerSettings.EntityUIComponents[keyValuePair2.Key] = new ClientCombatTextUIComponent(keyValuePair2.Key, keyValuePair2.Value, inGameView.EntityUIContainer.CombatTextUIComponentRenderer);
						}
					}
					else
					{
						this._upcomingServerSettings.EntityUIComponents[keyValuePair2.Key] = new ClientEntityStatUIComponent(keyValuePair2.Key, keyValuePair2.Value, inGameView.EntityUIContainer.EntityStatUIComponentRenderer);
					}
				}
				this.FinishedReceivedAssetType(PacketHandler.AssetType.EntityUIComponents);
			}
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x0005E0E4 File Offset: 0x0005C2E4
		private void ValidateSingleplayer()
		{
			bool flag = this._gameInstance.App.SingleplayerServer == null;
			if (flag)
			{
				throw new Exception("Received " + this._stageValidationPacketId + " at but not connected to a singleplayer server!");
			}
			this._stageValidationPacketId = string.Empty;
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x0005E130 File Offset: 0x0005C330
		private void ProcessRequestServerAccess(RequestServerAccess packet)
		{
			this.ValidateSingleplayer();
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.Chat.Log("Port Mapping is not implemented!");
			}, false, false);
			this._gameInstance.Connection.SendPacket(new UpdateServerAccess(0, new HostAddress[0]));
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x0005E188 File Offset: 0x0005C388
		private void ProcessEditorBlocksChangePacket(EditorBlocksChange packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				bool flag = packet.Selection != null;
				if (flag)
				{
					this._gameInstance.BuilderToolsModule.SelectionArea.UpdateSelection(new Vector3((float)packet.Selection.MinX, (float)packet.Selection.MinY, (float)packet.Selection.MinZ), new Vector3((float)packet.Selection.MaxX, (float)packet.Selection.MaxY, (float)packet.Selection.MaxZ));
				}
				bool flag2 = packet.BlocksChange != null;
				if (flag2)
				{
					this._gameInstance.BuilderToolsModule.Paste.UpdateBlockSet(packet.BlocksChange);
				}
			}, false, false);
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x0005E1D8 File Offset: 0x0005C3D8
		private void ProcessBuilderToolShowAnchorPacket(BuilderToolShowAnchor packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			Vector3 position = new Vector3((float)packet.X, (float)packet.Y, (float)packet.Z);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.BuilderToolsModule.Anchor.ShowAnchor(position);
			}, false, false);
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x0005E240 File Offset: 0x0005C440
		private void ProcessBuilderToolSelectionToolReplyWithClipboard(BuilderToolSelectionToolReplyWithClipboard packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.BuilderToolsModule.PlaySelection.TryEnterTranslationModeWithClipboard(packet.BlocksChange);
			}, false, false);
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x0005E28F File Offset: 0x0005C48F
		private void ProcessBuilderToolHideAnchorPacket(BuilderToolHideAnchors packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.BuilderToolsModule.Anchor.HideAnchors();
			}, false, false);
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x0005E2C0 File Offset: 0x0005C4C0
		private void ProcessUpdateTrailsPacket(UpdateTrails packet)
		{
			UpdateTrails updateTrails = new UpdateTrails(packet);
			UpdateType type = updateTrails.Type;
			UpdateType updateType = type;
			if (updateType != null)
			{
				if (updateType - 1 > 1)
				{
					throw new Exception(string.Format("Received invalid packet UpdateType for {0} at {1} with type {2}.", updateTrails.GetType().Name, this._stage, updateTrails.Type));
				}
				this.ValidateStage(PacketHandler.ConnectionStage.Playing);
				this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Trails: Starting {0}", updateTrails.Type));
				Stopwatch stopwatch = Stopwatch.StartNew();
				bool flag = updateTrails.Type == 1;
				if (flag)
				{
					foreach (KeyValuePair<string, Trail> keyValuePair in updateTrails.Trails)
					{
						this._networkTrails[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				else
				{
					foreach (KeyValuePair<string, Trail> keyValuePair2 in updateTrails.Trails)
					{
						this._networkTrails.Remove(keyValuePair2.Key);
					}
				}
				object setupLock = this._setupLock;
				Dictionary<string, Rectangle> upcomingFXImageLocations;
				byte[][] upcomingFXAtlasPixelsPerLevel;
				lock (setupLock)
				{
					this._gameInstance.TrailStoreModule.PrepareTrails(this._networkTrails, out this._upcomingTrailTextureInfo, this._threadCancellationToken);
					bool isCancellationRequested = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					this._gameInstance.FXModule.PrepareAtlas(this._upcomingParticleTextureInfo, this._upcomingTrailTextureInfo, out upcomingFXImageLocations, out upcomingFXAtlasPixelsPerLevel, this._threadCancellationToken);
					bool isCancellationRequested2 = this._threadCancellationToken.IsCancellationRequested;
					if (isCancellationRequested2)
					{
						return;
					}
				}
				Dictionary<string, Trail> upcomingTrails = new Dictionary<string, Trail>(this._networkTrails);
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this._gameInstance.TrailStoreModule.SetupTrailSettings(upcomingTrails);
					this._gameInstance.FXModule.CreateAtlasTextures(upcomingFXImageLocations, upcomingFXAtlasPixelsPerLevel);
					this._gameInstance.EntityStoreModule.RebuildRenderers(false);
					this._gameInstance.ParticleSystemStoreModule.ResetParticleSystems(true);
					this._gameInstance.App.DevTools.Info(string.Format("[AssetUpdate] Trails: Finished {0} in {1}", updateTrails.Type, TimeHelper.FormatMillis(stopwatch.ElapsedMilliseconds)));
				}, false, false);
			}
			else
			{
				this.ValidateStage(PacketHandler.ConnectionStage.SettingUp);
				this.ReceivedAssetType(PacketHandler.AssetType.Trails);
				this._networkTrails = updateTrails.Trails;
				this.FinishedReceivedAssetType(PacketHandler.AssetType.Trails);
			}
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x0005E5A0 File Offset: 0x0005C7A0
		private void ProcessTriggerEditorUpdateScriptReply(TriggerEditorUpdateScriptReply packet)
		{
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x0005E5A3 File Offset: 0x0005C7A3
		private void ProcessTriggerEditorRequestScriptsReply(TriggerEditorRequestScriptsReply packet)
		{
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x0005E5A6 File Offset: 0x0005C7A6
		private void ProcessTriggerEditorRequestScriptReply(TriggerEditorRequestScriptReply packet)
		{
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x0005E5A9 File Offset: 0x0005C7A9
		private void ProcessTriggerEditorRequestBlockReply(TriggerEditorRequestBlockReply packet)
		{
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x0005E5AC File Offset: 0x0005C7AC
		private void ProcessTriggerEditorUpdateBlockReply(TriggerEditorUpdateBlockReply packet)
		{
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x0005E5B0 File Offset: 0x0005C7B0
		private void ProcessUpdateTimePacket(UpdateTime packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			InstantData gameTime = new InstantData(packet.GameTime);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.TimeModule.ProcessGameTimeFromServer(gameTime);
			}, false, false);
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x0005E60C File Offset: 0x0005C80C
		private void ProcessUpdateTimeSettingsPacket(UpdateTimeSettings packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.SettingUp | PacketHandler.ConnectionStage.Playing);
			UpdateTimeSettings time = new UpdateTimeSettings(packet);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				TimeModule.SecondsPerGameDay = time.SecondsPerGameDay;
				WeatherModule.TotalMoonPhases = (byte)time.TotalMoonPhases;
				WeatherModule.DaylightPortion = (byte)time.DaylightPortion;
				this._gameInstance.TimeModule.IsServerTimePaused = time.TimePaused;
				this._gameInstance.WeatherModule.OnDaylightPortionChanged();
			}, false, false);
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x0005E664 File Offset: 0x0005C864
		private void ProcessUpdateWeatherPacket(UpdateWeather packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			int weatherIndex = packet.WeatherIndex;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.WeatherModule.SetServerWeather(weatherIndex);
			}, false, false);
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x0005E6B8 File Offset: 0x0005C8B8
		private void ProcessUpdateEditorTimeOverride(UpdateEditorTimeOverride packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			InstantData gameTime = new InstantData(packet.GameTime);
			bool isPaused = packet.Paused;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.TimeModule.ProcessEditorTimeOverride(gameTime, isPaused);
			}, false, false);
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x0005E71D File Offset: 0x0005C91D
		private void ProcessClearEditorTimeOverride(ClearEditorTimeOverride packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.TimeModule.ProcessClearEditorTimeOverride();
			}, false, false);
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x0005E750 File Offset: 0x0005C950
		private void ProcessUpdateEditorWeatherOverride(UpdateEditorWeatherOverride packet)
		{
			this.ValidateStage(PacketHandler.ConnectionStage.Playing);
			int weatherIndex = packet.WeatherIndex;
			this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
			{
				this._gameInstance.WeatherModule.SetEditorWeatherOverride(weatherIndex);
			}, false, false);
		}

		// Token: 0x04001795 RID: 6037
		public AmbienceFX[] _networkAmbienceFXs;

		// Token: 0x04001796 RID: 6038
		private FileStream _blobFileStream;

		// Token: 0x04001797 RID: 6039
		private Asset _blobAsset;

		// Token: 0x04001798 RID: 6040
		private readonly List<string> _assetNamesToUpdate = new List<string>();

		// Token: 0x04001799 RID: 6041
		private readonly long[] _assetUpdatePrepareTimes = new long[typeof(PacketHandler.AssetUpdatePrepareSteps).GetEnumValues().Length];

		// Token: 0x0400179A RID: 6042
		private readonly Stopwatch _assetUpdatePrepareTimer = new Stopwatch();

		// Token: 0x0400179B RID: 6043
		private readonly long[] _assetUpdateSetupTimes = new long[typeof(PacketHandler.AssetUpdateSetupSteps).GetEnumValues().Length];

		// Token: 0x0400179C RID: 6044
		private readonly Stopwatch _assetUpdateSetupTimer = new Stopwatch();

		// Token: 0x0400179D RID: 6045
		private static readonly Regex HashRegex = new Regex("^[A-Fa-f0-9]{64}$");

		// Token: 0x0400179E RID: 6046
		private int _highestReceivedBlockId;

		// Token: 0x0400179F RID: 6047
		private Dictionary<int, BlockType> _networkBlockTypes = new Dictionary<int, BlockType>();

		// Token: 0x040017A0 RID: 6048
		private ClientBlockType[] _upcomingBlockTypes;

		// Token: 0x040017A1 RID: 6049
		private Dictionary<string, MapModule.AtlasLocation> _upcomingBlocksImageLocations;

		// Token: 0x040017A2 RID: 6050
		private Point _upcomingBlocksAtlasSize;

		// Token: 0x040017A3 RID: 6051
		private const int MaxChunkBufferSize = 327683;

		// Token: 0x040017A4 RID: 6052
		private readonly byte[] _compressedChunkBuffer = new byte[327683];

		// Token: 0x040017A5 RID: 6053
		private int _compressedChunkBufferPosition = 0;

		// Token: 0x040017A6 RID: 6054
		private readonly byte[] _decompressedChunkBuffer = new byte[327683];

		// Token: 0x040017A7 RID: 6055
		private readonly BitFieldArr _bitFieldArr = new BitFieldArr(10, 1024);

		// Token: 0x040017A8 RID: 6056
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040017A9 RID: 6057
		private readonly GameInstance _gameInstance;

		// Token: 0x040017AA RID: 6058
		private readonly BlockingCollection<ProtoPacket> _packets = new BlockingCollection<ProtoPacket>();

		// Token: 0x040017AB RID: 6059
		private readonly HashSet<string> _unhandledPacketTypes = new HashSet<string>();

		// Token: 0x040017AC RID: 6060
		private readonly Thread _thread;

		// Token: 0x040017AD RID: 6061
		private readonly CancellationTokenSource _threadCancellationTokenSource = new CancellationTokenSource();

		// Token: 0x040017AE RID: 6062
		private readonly CancellationToken _threadCancellationToken;

		// Token: 0x040017AF RID: 6063
		private readonly ServerSettings _upcomingServerSettings = new ServerSettings();

		// Token: 0x040017B0 RID: 6064
		private readonly Stopwatch _connectionStopwatch = Stopwatch.StartNew();

		// Token: 0x040017B1 RID: 6065
		private readonly Stopwatch _stageStopwatch = Stopwatch.StartNew();

		// Token: 0x040017B2 RID: 6066
		private PacketHandler.ConnectionStage _stage = PacketHandler.ConnectionStage.WaitingForSetup;

		// Token: 0x040017B3 RID: 6067
		private string _stageValidationPacketId;

		// Token: 0x040017B4 RID: 6068
		private PacketHandler.AssetType _receivedAssetTypes;

		// Token: 0x040017B5 RID: 6069
		private long _lastAssetReceivedMs;

		// Token: 0x040017B6 RID: 6070
		private ConcurrentDictionary<int, PacketHandler.PendingCallback> _pendingCallbacks = new ConcurrentDictionary<int, PacketHandler.PendingCallback>();

		// Token: 0x040017B7 RID: 6071
		private int _lastCallbackToken;

		// Token: 0x040017B8 RID: 6072
		private DateTime _lastCallbackWarning;

		// Token: 0x040017B9 RID: 6073
		private EntityEffect[] _entityEffects;

		// Token: 0x040017BA RID: 6074
		private Dictionary<string, ItemPlayerAnimations> _networkItemPlayerAnimations;

		// Token: 0x040017BB RID: 6075
		private Dictionary<string, ClientItemPlayerAnimations> _upcomingItemPlayerAnimations;

		// Token: 0x040017BC RID: 6076
		private Dictionary<string, ItemBase> _networkItems;

		// Token: 0x040017BD RID: 6077
		private Dictionary<string, ClientItemBase> _upcomingItems = new Dictionary<string, ClientItemBase>();

		// Token: 0x040017BE RID: 6078
		private ModelVFX[] _modelVFXs;

		// Token: 0x040017BF RID: 6079
		private Dictionary<string, ParticleSystem> _networkParticleSystems;

		// Token: 0x040017C0 RID: 6080
		private Dictionary<string, ParticleSpawner> _networkParticleSpawners;

		// Token: 0x040017C1 RID: 6081
		private Dictionary<string, PacketHandler.TextureInfo> _upcomingParticleTextureInfo;

		// Token: 0x040017C2 RID: 6082
		private List<string> _upcomingUVMotionTexturePaths;

		// Token: 0x040017C3 RID: 6083
		private readonly object _setupLock = new object();

		// Token: 0x040017C4 RID: 6084
		private Dictionary<string, Point> _upcomingEntitiesImageLocations;

		// Token: 0x040017C5 RID: 6085
		private Dictionary<string, WorldMapModule.Texture> _upcomingWorldMapImageLocations;

		// Token: 0x040017C6 RID: 6086
		private Interaction[] _networkInteractions;

		// Token: 0x040017C7 RID: 6087
		private ClientInteraction[] _upcomingInteractions;

		// Token: 0x040017C8 RID: 6088
		private RootInteraction[] _networkRootInteractions;

		// Token: 0x040017C9 RID: 6089
		private ClientRootInteraction[] _upcomingRootInteractions;

		// Token: 0x040017CA RID: 6090
		private Dictionary<string, PacketHandler.TextureInfo> _upcomingTrailTextureInfo;

		// Token: 0x040017CB RID: 6091
		private Dictionary<string, Trail> _networkTrails;

		// Token: 0x02000C29 RID: 3113
		private enum AssetUpdatePrepareSteps
		{
			// Token: 0x04003DB8 RID: 15800
			PrepareBlockTypes,
			// Token: 0x04003DB9 RID: 15801
			CopyBlockTypes,
			// Token: 0x04003DBA RID: 15802
			PrepareItemIconAtlas,
			// Token: 0x04003DBB RID: 15803
			PrepareEntityAtlas,
			// Token: 0x04003DBC RID: 15804
			PrepareItems,
			// Token: 0x04003DBD RID: 15805
			PrepareParticles,
			// Token: 0x04003DBE RID: 15806
			PrepareTrails,
			// Token: 0x04003DBF RID: 15807
			PrepareItemPlayerAnimations,
			// Token: 0x04003DC0 RID: 15808
			PrepareInteractions,
			// Token: 0x04003DC1 RID: 15809
			PrepareWorldMapAtlas,
			// Token: 0x04003DC2 RID: 15810
			PrepareFXAtlas,
			// Token: 0x04003DC3 RID: 15811
			PrepareSoundEffects,
			// Token: 0x04003DC4 RID: 15812
			PrepareSoundBanks
		}

		// Token: 0x02000C2A RID: 3114
		private enum AssetUpdateSetupSteps
		{
			// Token: 0x04003DC6 RID: 15814
			SetupBlockTypes,
			// Token: 0x04003DC7 RID: 15815
			SetupBlockAtlas,
			// Token: 0x04003DC8 RID: 15816
			SetupItemIcons,
			// Token: 0x04003DC9 RID: 15817
			SetupItemPlayerAnimations,
			// Token: 0x04003DCA RID: 15818
			SetupItems,
			// Token: 0x04003DCB RID: 15819
			SetupInteractions,
			// Token: 0x04003DCC RID: 15820
			SetupParticleSpawners,
			// Token: 0x04003DCD RID: 15821
			SetupTrails,
			// Token: 0x04003DCE RID: 15822
			SetupFXAtlas,
			// Token: 0x04003DCF RID: 15823
			SetupEntityAtlas,
			// Token: 0x04003DD0 RID: 15824
			SetupModelsAndAnimation,
			// Token: 0x04003DD1 RID: 15825
			UpdateAtlasSizes,
			// Token: 0x04003DD2 RID: 15826
			RebuildEntityRenderers,
			// Token: 0x04003DD3 RID: 15827
			UpdateInterfaceRenderPreview,
			// Token: 0x04003DD4 RID: 15828
			BuildWorldMapAtlas,
			// Token: 0x04003DD5 RID: 15829
			ReloadSkyRenderer,
			// Token: 0x04003DD6 RID: 15830
			UpdateWeatherTexture,
			// Token: 0x04003DD7 RID: 15831
			ResetParticleSystems
		}

		// Token: 0x02000C2B RID: 3115
		[Flags]
		private enum ConnectionStage : byte
		{
			// Token: 0x04003DD9 RID: 15833
			WaitingForSetup = 2,
			// Token: 0x04003DDA RID: 15834
			SettingUp = 4,
			// Token: 0x04003DDB RID: 15835
			Playing = 8
		}

		// Token: 0x02000C2C RID: 3116
		[Flags]
		private enum AssetType : uint
		{
			// Token: 0x04003DDD RID: 15837
			AmbienceFX = 2U,
			// Token: 0x04003DDE RID: 15838
			BlockHitboxes = 4U,
			// Token: 0x04003DDF RID: 15839
			BlockTypes = 8U,
			// Token: 0x04003DE0 RID: 15840
			Environment = 16U,
			// Token: 0x04003DE1 RID: 15841
			FluidFX = 32U,
			// Token: 0x04003DE2 RID: 15842
			Items = 64U,
			// Token: 0x04003DE3 RID: 15843
			ItemCategories = 128U,
			// Token: 0x04003DE4 RID: 15844
			ParticleSpawners = 256U,
			// Token: 0x04003DE5 RID: 15845
			ParticleSystems = 512U,
			// Token: 0x04003DE6 RID: 15846
			ResourceTypes = 1024U,
			// Token: 0x04003DE7 RID: 15847
			Weather = 2048U,
			// Token: 0x04003DE8 RID: 15848
			Translations = 4096U,
			// Token: 0x04003DE9 RID: 15849
			FieldcraftCategories = 8192U,
			// Token: 0x04003DEA RID: 15850
			Trails = 16384U,
			// Token: 0x04003DEB RID: 15851
			EntityEffects = 32768U,
			// Token: 0x04003DEC RID: 15852
			BlockParticleSets = 65536U,
			// Token: 0x04003DED RID: 15853
			ItemAnimations = 131072U,
			// Token: 0x04003DEE RID: 15854
			Interactions = 262144U,
			// Token: 0x04003DEF RID: 15855
			RootInteractions = 524288U,
			// Token: 0x04003DF0 RID: 15856
			UnarmedInteractions = 1048576U,
			// Token: 0x04003DF1 RID: 15857
			BlockSoundSets = 2097152U,
			// Token: 0x04003DF2 RID: 15858
			EntityStatTypes = 4194304U,
			// Token: 0x04003DF3 RID: 15859
			ItemQuality = 8388608U,
			// Token: 0x04003DF4 RID: 15860
			ItemReticles = 16777216U,
			// Token: 0x04003DF5 RID: 15861
			HitboxCollisionConfig = 33554432U,
			// Token: 0x04003DF6 RID: 15862
			RepulsionConfig = 67108864U,
			// Token: 0x04003DF7 RID: 15863
			ModelVFX = 134217728U,
			// Token: 0x04003DF8 RID: 15864
			EntityUIComponents = 268435456U
		}

		// Token: 0x02000C2D RID: 3117
		public class PendingCallback
		{
			// Token: 0x04003DF9 RID: 15865
			public Action<FailureReply, ProtoPacket> Callback;

			// Token: 0x04003DFA RID: 15866
			public Disposable Disposable;
		}

		// Token: 0x02000C2E RID: 3118
		public class InventoryWindow
		{
			// Token: 0x04003DFB RID: 15867
			public int Id;

			// Token: 0x04003DFC RID: 15868
			public WindowType WindowType;

			// Token: 0x04003DFD RID: 15869
			public string WindowDataStringified;

			// Token: 0x04003DFE RID: 15870
			public JObject WindowData;

			// Token: 0x04003DFF RID: 15871
			public ClientItemStack[] Inventory;
		}

		// Token: 0x02000C2F RID: 3119
		public class EventTitle
		{
			// Token: 0x04003E00 RID: 15872
			public float Duration;

			// Token: 0x04003E01 RID: 15873
			public float FadeInDuration;

			// Token: 0x04003E02 RID: 15874
			public float FadeOutDuration;

			// Token: 0x04003E03 RID: 15875
			public string PrimaryTitle;

			// Token: 0x04003E04 RID: 15876
			public string SecondaryTitle;

			// Token: 0x04003E05 RID: 15877
			public bool IsMajor;

			// Token: 0x04003E06 RID: 15878
			public string Icon;
		}

		// Token: 0x02000C30 RID: 3120
		public class ClientKnownRecipe
		{
			// Token: 0x060062AE RID: 25262 RVA: 0x00206C20 File Offset: 0x00204E20
			public ClientKnownRecipe(string itemId, CraftingRecipe recipe)
			{
				this.ItemId = itemId;
				this.Recipe = new ClientItemCraftingRecipe(recipe);
			}

			// Token: 0x04003E07 RID: 15879
			public string ItemId;

			// Token: 0x04003E08 RID: 15880
			public ClientItemCraftingRecipe Recipe;
		}

		// Token: 0x02000C31 RID: 3121
		public class PlayerListPlayer
		{
			// Token: 0x060062AF RID: 25263 RVA: 0x00206C3D File Offset: 0x00204E3D
			public PlayerListPlayer(AddToPlayerList.PlayerListPlayer player)
			{
				this.Uuid = player.Uuid_;
				this.DisplayName = player.DisplayName;
				this.Ping = player.Ping;
			}

			// Token: 0x04003E09 RID: 15881
			public Guid Uuid;

			// Token: 0x04003E0A RID: 15882
			public string DisplayName;

			// Token: 0x04003E0B RID: 15883
			public int Ping;
		}

		// Token: 0x02000C32 RID: 3122
		public class TextureInfo
		{
			// Token: 0x04003E0C RID: 15884
			public string Checksum;

			// Token: 0x04003E0D RID: 15885
			public int Width;

			// Token: 0x04003E0E RID: 15886
			public int Height;
		}
	}
}
