using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Audio;
using HytaleClient.Core;
using HytaleClient.Data.Audio;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;
using Wwise;

namespace HytaleClient.InGame.Modules.Audio
{
	// Token: 0x02000992 RID: 2450
	internal class AudioModule : Module
	{
		// Token: 0x06004E3D RID: 20029 RVA: 0x00159DBC File Offset: 0x00157FBC
		public AudioModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._audio = this._gameInstance.Engine.Audio;
			this._audio.RefreshBanks();
		}

		// Token: 0x06004E3E RID: 20030 RVA: 0x00159E09 File Offset: 0x00158009
		protected override void DoDispose()
		{
		}

		// Token: 0x06004E3F RID: 20031 RVA: 0x00159E0C File Offset: 0x0015800C
		public void PrepareSoundBanks(out Dictionary<string, WwiseResource> upcomingWwiseIds)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingWwiseIds = new Dictionary<string, WwiseResource>();
			string hash;
			bool flag = this._gameInstance.HashesByServerAssetPath.TryGetValue("SoundBanks/Wwise_IDs.h", ref hash);
			if (flag)
			{
				try
				{
					WwiseHeaderParser.Parse(AssetManager.GetAssetLocalPathUsingHash(hash), out upcomingWwiseIds);
				}
				catch (Exception exception)
				{
					AudioModule.Logger.Error(exception, "Failed to load wwise header file.");
				}
			}
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x00159E84 File Offset: 0x00158084
		public void SetupSoundBanks(Dictionary<string, WwiseResource> upcomingWwiseIds)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._audio.ResourceManager.SetupWwiseIds(upcomingWwiseIds);
			string hash;
			bool flag = this._gameInstance.HashesByServerAssetPath.TryGetValue("SoundBanks/Windows/Init.bnk", ref hash);
			if (flag)
			{
				this._audio.ResourceManager.FilePathsByFileName["Init.bnk"] = AssetManager.GetAssetLocalPathUsingHash(hash);
			}
			string hash2;
			bool flag2 = this._gameInstance.HashesByServerAssetPath.TryGetValue("SoundBanks/Windows/Master.bnk", ref hash2);
			if (flag2)
			{
				this._audio.ResourceManager.FilePathsByFileName["Master.bnk"] = AssetManager.GetAssetLocalPathUsingHash(hash2);
			}
			string hash3;
			bool flag3 = this._gameInstance.HashesByServerAssetPath.TryGetValue("SoundBanks/Windows/Music.bnk", ref hash3);
			if (flag3)
			{
				this._audio.ResourceManager.FilePathsByFileName["Music.bnk"] = AssetManager.GetAssetLocalPathUsingHash(hash3);
			}
			foreach (KeyValuePair<string, string> keyValuePair in this._gameInstance.HashesByServerAssetPath)
			{
				bool flag4 = !keyValuePair.Key.StartsWith("SoundBanks/Windows/") || !keyValuePair.Key.EndsWith(".wem");
				if (!flag4)
				{
					string text = keyValuePair.Key.Substring("SoundBanks/Windows/".Length);
					this._audio.ResourceManager.FilePathsByFileName[text] = AssetManager.GetAssetLocalPathUsingHash(keyValuePair.Value);
				}
			}
			this._audio.RefreshBanks();
		}

		// Token: 0x06004E41 RID: 20033 RVA: 0x0015A034 File Offset: 0x00158234
		public bool TryRegisterSoundObject(Vector3 position, Vector3 orientation, ref AudioDevice.SoundObjectReference soundObjectReference, bool hasUniqueEvent = false)
		{
			return this._audio.TryRegisterSoundObject(position, orientation, ref soundObjectReference, false, hasUniqueEvent);
		}

		// Token: 0x06004E42 RID: 20034 RVA: 0x0015A057 File Offset: 0x00158257
		public void UnregisterSoundObject(ref AudioDevice.SoundObjectReference soundObjectReference)
		{
			this._audio.UnregisterSoundObject(ref soundObjectReference);
		}

		// Token: 0x06004E43 RID: 20035 RVA: 0x0015A068 File Offset: 0x00158268
		public bool TryPlayLocalBlockSoundEvent(int blockSoundSetIndex, BlockSoundEvent blockSoundEvent, ref int playbackId)
		{
			bool flag = blockSoundSetIndex >= this._gameInstance.ServerSettings.BlockSoundSets.Length || blockSoundSetIndex < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int value;
				bool flag2 = this._gameInstance.ServerSettings.BlockSoundSets[blockSoundSetIndex].SoundEventIndices.TryGetValue(blockSoundEvent, out value);
				if (flag2)
				{
					uint networkWwiseId = ResourceManager.GetNetworkWwiseId(value);
					bool flag3 = networkWwiseId > 0U;
					if (flag3)
					{
						bool flag4 = playbackId != -1;
						if (flag4)
						{
							this._gameInstance.AudioModule.ActionOnEvent(playbackId, 3);
						}
						playbackId = this._gameInstance.AudioModule.PlayLocalSoundEvent(networkWwiseId);
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004E44 RID: 20036 RVA: 0x0015A114 File Offset: 0x00158314
		public int PlayLocalSoundEvent(string soundEventId)
		{
			uint soundEventIndex;
			bool flag = this._audio.ResourceManager.WwiseEventIds.TryGetValue(soundEventId, out soundEventIndex);
			int result;
			if (flag)
			{
				result = this.PlayLocalSoundEvent(soundEventIndex);
			}
			else
			{
				AudioModule.Logger.Warn("Could not load sound: {0}", soundEventId);
				result = -1;
			}
			return result;
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x0015A160 File Offset: 0x00158360
		public void PlaySoundEvent(string soundEventId, Vector3 position, Vector3 orientation)
		{
			uint soundEventIndex;
			bool flag = this._audio.ResourceManager.WwiseEventIds.TryGetValue(soundEventId, out soundEventIndex);
			if (flag)
			{
				this.PlaySoundEvent(soundEventIndex, position, orientation);
			}
			else
			{
				AudioModule.Logger.Warn("Could not load sound: {0}", soundEventId);
			}
		}

		// Token: 0x06004E46 RID: 20038 RVA: 0x0015A1AC File Offset: 0x001583AC
		public bool TryPlayBlockSoundEvent(int blockSoundSetIndex, BlockSoundEvent blockSoundEvent, Vector3 worldPosition, Vector3 orientation)
		{
			bool flag = blockSoundSetIndex >= this._gameInstance.ServerSettings.BlockSoundSets.Length || blockSoundSetIndex < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int value;
				bool flag2 = this._gameInstance.ServerSettings.BlockSoundSets[blockSoundSetIndex].SoundEventIndices.TryGetValue(blockSoundEvent, out value);
				if (flag2)
				{
					uint networkWwiseId = ResourceManager.GetNetworkWwiseId(value);
					bool flag3 = networkWwiseId > 0U;
					if (flag3)
					{
						this._gameInstance.AudioModule.PlaySoundEvent(networkWwiseId, worldPosition, orientation);
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x0015A238 File Offset: 0x00158438
		public int PlayLocalSoundEvent(uint soundEventIndex)
		{
			bool flag = soundEventIndex == 0U;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				result = this._audio.PostEvent(soundEventIndex, AudioDevice.PlayerSoundObjectReference);
			}
			return result;
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x0015A268 File Offset: 0x00158468
		public void PlaySoundEvent(uint soundEventIndex, Vector3 position, Vector3 orientation)
		{
			bool flag = soundEventIndex == 0U;
			if (!flag)
			{
				AudioDevice.SoundObjectReference soundObjectReference = default(AudioDevice.SoundObjectReference);
				bool flag2 = this._audio.TryRegisterSoundObject(position, orientation, ref soundObjectReference, true, false);
				if (flag2)
				{
					this._audio.PostEvent(soundEventIndex, soundObjectReference);
				}
			}
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x0015A2B0 File Offset: 0x001584B0
		public void PlaySoundEvent(uint soundEventIndex, Vector3 position, Vector3 orientation, ref AudioDevice.SoundEventReference soundEventReference)
		{
			bool flag = soundEventIndex == 0U;
			if (!flag)
			{
				AudioDevice.SoundObjectReference soundObjectReference = default(AudioDevice.SoundObjectReference);
				bool flag2 = this._audio.TryRegisterSoundObject(position, orientation, ref soundObjectReference, true, false);
				if (flag2)
				{
					soundEventReference.PlaybackId = this._audio.PostEvent(soundEventIndex, soundObjectReference);
				}
			}
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x0015A2FC File Offset: 0x001584FC
		public void PlaySoundEvent(uint soundEventIndex, AudioDevice.SoundObjectReference soundObjectId, ref AudioDevice.SoundEventReference soundEventReference)
		{
			bool flag = soundEventIndex == 0U;
			if (!flag)
			{
				int playbackId = this._audio.PostEvent(soundEventIndex, soundObjectId);
				soundEventReference.PlaybackId = playbackId;
			}
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x0015A329 File Offset: 0x00158529
		public void ActionOnEvent(ref AudioDevice.SoundEventReference soundEventReference, SoundEngine.AkActionOnEventType actionType)
		{
			Debug.Assert(soundEventReference.PlaybackId != -1);
			this._audio.ActionOnEvent(soundEventReference.PlaybackId, actionType, 0, 4);
			soundEventReference.PlaybackId = -1;
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x0015A35A File Offset: 0x0015855A
		public void ActionOnEvent(int playbackId, SoundEngine.AkActionOnEventType actionType)
		{
			Debug.Assert(playbackId != -1);
			this._audio.ActionOnEvent(playbackId, actionType, 0, 4);
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x0015A37A File Offset: 0x0015857A
		public void OnSoundEffectCollectionChanged()
		{
			this.CurrentEffectSoundEventIndex = 0U;
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x0015A384 File Offset: 0x00158584
		public void SetWorldSoundEffect(uint effectIndex)
		{
			bool flag = effectIndex != this.CurrentEffectSoundEventIndex;
			if (flag)
			{
				this.CurrentEffectSoundEventIndex = effectIndex;
				bool flag2 = this._currentEffectPlaybackId != -1;
				if (flag2)
				{
					this._audio.ActionOnEvent(this._currentEffectPlaybackId, 0, 0, 4);
				}
				bool flag3 = this.CurrentEffectSoundEventIndex == 0U;
				if (flag3)
				{
					this._currentEffectPlaybackId = -1;
				}
				else
				{
					this._currentEffectPlaybackId = this.PlayLocalSoundEvent(this.CurrentEffectSoundEventIndex);
				}
			}
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x0015A400 File Offset: 0x00158600
		public void Update(float deltaTime)
		{
			this._accumulatedDeltaTime += deltaTime;
			bool flag = this._accumulatedDeltaTime < 0.033333335f;
			if (!flag)
			{
				this._accumulatedDeltaTime = 0f;
				ICameraController controller = this._gameInstance.CameraModule.Controller;
				Vector3 position = controller.Position;
				this._audio.SetListenerPosition(position, controller.Rotation);
				EntityStoreModule entityStoreModule = this._gameInstance.EntityStoreModule;
				AudioDevice.SoundObjectReference[] soundObjectReferences = entityStoreModule.GetSoundObjectReferences();
				BoundingSphere[] boundingVolumes = entityStoreModule.GetBoundingVolumes();
				Vector3[] orientations = entityStoreModule.GetOrientations();
				int entitiesCount = entityStoreModule.GetEntitiesCount();
				bool flag2 = this._currentFrameId % 2 == 0;
				bool flag3 = !flag2 && this._currentFrameId % 4 - 1 == 0;
				bool flag4 = !flag2 && !flag3 && this._currentFrameId % 8 - 3 == 0;
				for (int i = entityStoreModule.PlayerEntityLocalId + 1; i < entitiesCount; i++)
				{
					Vector3 center = boundingVolumes[i].Center;
					float num = Vector3.DistanceSquared(position, center);
					bool flag5 = num < 2304f;
					bool flag6;
					if (flag5)
					{
						flag6 = true;
					}
					else
					{
						bool flag7 = num < 9408f;
						if (flag7)
						{
							flag6 = flag2;
						}
						else
						{
							bool flag8 = num < 25600f;
							if (flag8)
							{
								flag6 = flag3;
							}
							else
							{
								flag6 = flag4;
							}
						}
					}
					bool flag9 = flag6;
					if (flag9)
					{
						this._audio.SetPosition(soundObjectReferences[i], center, orientations[i]);
					}
				}
				this._currentFrameId++;
			}
		}

		// Token: 0x04002975 RID: 10613
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002976 RID: 10614
		public const float AudioTimeStep = 0.033333335f;

		// Token: 0x04002977 RID: 10615
		private float _accumulatedDeltaTime;

		// Token: 0x04002978 RID: 10616
		public uint CurrentEffectSoundEventIndex = 0U;

		// Token: 0x04002979 RID: 10617
		private int _currentEffectPlaybackId = -1;

		// Token: 0x0400297A RID: 10618
		private AudioDevice _audio;

		// Token: 0x0400297B RID: 10619
		private int _currentFrameId = 0;
	}
}
