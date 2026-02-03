using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using HytaleClient.Audio.Commands;
using HytaleClient.Core;
using HytaleClient.Math;
using HytaleClient.Utils;
using NLog;
using Wwise;

namespace HytaleClient.Audio
{
	// Token: 0x02000B82 RID: 2946
	internal class AudioDevice : Disposable
	{
		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06005AA1 RID: 23201 RVA: 0x001C355F File Offset: 0x001C175F
		public int PlaybackCount
		{
			get
			{
				return this._currentEventPlaybacksByPlaybackId.Count;
			}
		}

		// Token: 0x06005AA2 RID: 23202 RVA: 0x001C356C File Offset: 0x001C176C
		public void RefreshBanks()
		{
			int num = this._commandMemoryPool.TakeSlot();
			bool flag = num >= 64;
			if (flag)
			{
				ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[num];
				ptr.Type = CommandType.RefreshBanks;
				this._commandIdQueue.Enqueue(num);
			}
		}

		// Token: 0x06005AA3 RID: 23203 RVA: 0x001C35C0 File Offset: 0x001C17C0
		private void ProcessRefreshBanks()
		{
			for (int i = 1; i < this._soundObjectBuffers.Count; i++)
			{
				uint num = this._soundObjectBuffers.SoundObjectId[i];
				bool flag = num == 0U;
				if (!flag)
				{
					this.UnregisterSoundObject(num, i);
				}
			}
			this.UnloadBanks();
			SoundEngine.ClearCustomPaths();
			foreach (KeyValuePair<string, string> keyValuePair in this.ResourceManager.FilePathsByFileName)
			{
				SoundEngine.RegisterCustomPath(keyValuePair.Key, keyValuePair.Value);
			}
			this.LoadBanks();
		}

		// Token: 0x06005AA4 RID: 23204 RVA: 0x001C367C File Offset: 0x001C187C
		public bool TryRegisterSoundObject(Vector3 position, Vector3 orientation, ref AudioDevice.SoundObjectReference soundObjectReference, bool hasSingleEvent = false, bool hasUniqueEvent = false)
		{
			soundObjectReference.SlotId = -1;
			soundObjectReference.SoundObjectId = 0U;
			bool flag = hasSingleEvent && Vector3.DistanceSquared(position, this._currentListenerPosition) >= 10000f;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int num = this._commandMemoryPool.TakeSlot();
				int num2 = this._soundObjectMemoryPool.TakeSlot();
				bool flag2 = num < 64 || num2 < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					soundObjectReference.SlotId = num2;
					soundObjectReference.SoundObjectId = this._nextSoundObjectId;
					ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[num];
					ptr.Type = CommandType.RegisterSoundObject;
					ptr.SoundObjectReference = soundObjectReference;
					ptr.WorldPosition = position;
					ptr.WorldOrientation = orientation;
					byte boolData = 0;
					if (hasSingleEvent)
					{
						BitUtils.SwitchOnBit(0, ref boolData);
					}
					if (hasUniqueEvent)
					{
						BitUtils.SwitchOnBit(1, ref boolData);
					}
					ptr.BoolData = boolData;
					this._commandIdQueue.Enqueue(num);
					this._nextSoundObjectId += 1U;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005AA5 RID: 23205 RVA: 0x001C3790 File Offset: 0x001C1990
		private void ProcessRegisterSoundObject(ref CommandBuffers.CommandData registerSoundObject)
		{
			AudioDevice.SoundObjectReference soundObjectReference = registerSoundObject.SoundObjectReference;
			bool flag = Vector3.DistanceSquared(registerSoundObject.WorldPosition, this._currentListenerPosition) < 10000f;
			byte b = 0;
			bool flag2 = BitUtils.IsBitOn(0, registerSoundObject.BoolData);
			if (flag2)
			{
				BitUtils.SwitchOnBit(0, ref b);
			}
			bool flag3 = BitUtils.IsBitOn(1, registerSoundObject.BoolData);
			if (flag3)
			{
				BitUtils.SwitchOnBit(1, ref b);
			}
			this.GetWwiseOrientations(registerSoundObject.WorldOrientation, ref this._soundObjectBuffers.FrontOrientation[soundObjectReference.SlotId], ref this._soundObjectBuffers.TopOrientation[soundObjectReference.SlotId]);
			bool flag4 = flag;
			if (flag4)
			{
				SoundEngine.RegisterGameObject((ulong)soundObjectReference.SoundObjectId);
				Vector3 vector = registerSoundObject.WorldPosition - this._currentListenerPosition;
				Vector3 vector2 = this._soundObjectBuffers.FrontOrientation[soundObjectReference.SlotId];
				Vector3 vector3 = this._soundObjectBuffers.TopOrientation[soundObjectReference.SlotId];
				SoundEngine.SetPosition((ulong)soundObjectReference.SoundObjectId, vector.X, vector.Y, -vector.Z, vector2.X, vector2.Y, vector2.Z, vector3.X, vector3.Y, vector3.Z);
				BitUtils.SwitchOnBit(2, ref b);
			}
			this._soundObjectBuffers.SoundObjectId[soundObjectReference.SlotId] = soundObjectReference.SoundObjectId;
			this._soundObjectBuffers.Position[soundObjectReference.SlotId] = registerSoundObject.WorldPosition;
			this._soundObjectBuffers.BoolData[soundObjectReference.SlotId] = b;
			this._soundObjectBuffers.LastPlaybackId[soundObjectReference.SlotId] = -1;
			this._soundObjectCount++;
			ArrayUtils.GrowArrayIfNecessary<int>(ref this._sortedSoundObjectSlotIds, this._soundObjectCount, 500);
			ArrayUtils.GrowArrayIfNecessary<float>(ref this._sortedSoundObjectSquaredDistanceToListener, this._soundObjectCount, 500);
		}

		// Token: 0x06005AA6 RID: 23206 RVA: 0x001C3978 File Offset: 0x001C1B78
		public void UnregisterSoundObject(ref AudioDevice.SoundObjectReference soundObjectReference)
		{
			bool flag = soundObjectReference.SlotId == -1;
			if (!flag)
			{
				int num = this._commandMemoryPool.TakeSlot();
				bool flag2 = num >= 64;
				if (flag2)
				{
					ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[num];
					ptr.Type = CommandType.UnregisterSoundObject;
					ptr.SoundObjectReference = soundObjectReference;
					this._commandIdQueue.Enqueue(num);
				}
				soundObjectReference = AudioDevice.SoundObjectReference.Empty;
			}
		}

		// Token: 0x06005AA7 RID: 23207 RVA: 0x001C39F4 File Offset: 0x001C1BF4
		private void ProcessUnregisterSoundObject(ref CommandBuffers.CommandData unRegisterSoundObject)
		{
			Debug.Assert(unRegisterSoundObject.SoundObjectReference.SoundObjectId != 1U, "The player sound object should never get unregistered");
			uint num = this._soundObjectBuffers.SoundObjectId[unRegisterSoundObject.SoundObjectReference.SlotId];
			bool flag = unRegisterSoundObject.SoundObjectReference.SoundObjectId != num;
			if (flag)
			{
				AudioDevice.Logger.Warn("Trying to unregister unreferenced soundobject");
			}
			else
			{
				this.UnregisterSoundObject(unRegisterSoundObject.SoundObjectReference.SoundObjectId, unRegisterSoundObject.SoundObjectReference.SlotId);
			}
		}

		// Token: 0x06005AA8 RID: 23208 RVA: 0x001C3A7C File Offset: 0x001C1C7C
		public void SetListenerPosition(Vector3 position, Vector3 orientation)
		{
			int num = this._commandMemoryPool.TakeSlot();
			bool flag = num >= 64;
			if (flag)
			{
				ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[num];
				ptr.Type = CommandType.SetListenerPosition;
				ptr.WorldPosition = position;
				ptr.WorldOrientation = orientation;
				this._commandIdQueue.Enqueue(num);
			}
		}

		// Token: 0x06005AA9 RID: 23209 RVA: 0x001C3AE0 File Offset: 0x001C1CE0
		private void ProcessSetListenerPosition(ref CommandBuffers.CommandData setListenerPosition)
		{
			this._currentListenerPosition = setListenerPosition.WorldPosition;
			Vector3 zero = Vector3.Zero;
			Vector3 zero2 = Vector3.Zero;
			this.GetWwiseOrientations(setListenerPosition.WorldOrientation, ref zero, ref zero2);
			SoundEngine.SetPosition(1UL, 0f, 0f, 0f, zero.X, zero.Y, zero.Z, zero2.X, zero2.Y, zero2.Z);
		}

		// Token: 0x06005AAA RID: 23210 RVA: 0x001C3B54 File Offset: 0x001C1D54
		public int PostEvent(uint eventId, AudioDevice.SoundObjectReference soundObjectReference)
		{
			Debug.Assert(eventId > 0U, "Expected valid sound event");
			bool flag = soundObjectReference.SlotId == -1;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int nextPlaybackId = this._nextPlaybackId;
				int num = this._commandMemoryPool.TakeSlot();
				bool flag2 = num >= 64;
				if (flag2)
				{
					ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[num];
					ptr.Type = CommandType.PostEvent;
					ptr.SoundObjectReference = soundObjectReference;
					ptr.EventId = eventId;
					ptr.PlaybackId = nextPlaybackId;
					this._commandIdQueue.Enqueue(num);
				}
				this._nextPlaybackId++;
				result = nextPlaybackId;
			}
			return result;
		}

		// Token: 0x06005AAB RID: 23211 RVA: 0x001C3C00 File Offset: 0x001C1E00
		private bool TryPrepareEvent(uint eventId)
		{
			uint num;
			bool flag = !this._eventReferenceCountByEventId.TryGetValue(eventId, out num);
			if (flag)
			{
				SoundEngine.AKRESULT akresult = SoundEngine.PrepareEvent(0, eventId);
				bool flag2 = akresult != 1;
				if (flag2)
				{
					string str;
					bool flag3 = !this.ResourceManager.DebugWwiseIds.TryGetValue(eventId, out str);
					if (flag3)
					{
						str = eventId.ToString();
					}
					AudioDevice.Logger.Warn("Failed to load event: " + str);
					return false;
				}
				num = 0U;
			}
			num += 1U;
			this._eventReferenceCountByEventId[eventId] = num;
			return true;
		}

		// Token: 0x06005AAC RID: 23212 RVA: 0x001C3C98 File Offset: 0x001C1E98
		private void ProcessPostEvent(ref CommandBuffers.CommandData postEvent)
		{
			uint num = this._soundObjectBuffers.SoundObjectId[postEvent.SoundObjectReference.SlotId];
			bool flag = postEvent.SoundObjectReference.SoundObjectId != num;
			if (flag)
			{
				AudioDevice.Logger.Warn("Trying to post event on unreferenced soundobject");
			}
			else
			{
				byte bitfield = this._soundObjectBuffers.BoolData[postEvent.SoundObjectReference.SlotId];
				bool flag2 = BitUtils.IsBitOn(1, bitfield);
				if (flag2)
				{
					this._soundObjectBuffers.LastPlaybackId[postEvent.SoundObjectReference.SlotId] = postEvent.PlaybackId;
					this._soundEventIdsByPlaybackIds.Add(postEvent.PlaybackId, postEvent.EventId);
				}
				bool flag3 = !BitUtils.IsBitOn(2, bitfield);
				if (!flag3)
				{
					bool flag4 = !this.TryPrepareEvent(postEvent.EventId);
					if (flag4)
					{
						bool flag5 = BitUtils.IsBitOn(0, this._soundObjectBuffers.BoolData[postEvent.SoundObjectReference.SlotId]);
						if (flag5)
						{
							this.UnregisterSoundObject(postEvent.SoundObjectReference.SoundObjectId, postEvent.SoundObjectReference.SlotId);
						}
					}
					this.PostEventToWwise(postEvent.PlaybackId, postEvent.EventId, postEvent.SoundObjectReference);
				}
			}
		}

		// Token: 0x06005AAD RID: 23213 RVA: 0x001C3DC8 File Offset: 0x001C1FC8
		private void PostEventToWwise(int playbackId, uint eventId, AudioDevice.SoundObjectReference soundObjectReference)
		{
			uint num = SoundEngine.PostEvent(eventId, (ulong)soundObjectReference.SoundObjectId, 1, this._defaultStopEventCallback);
			bool flag = num > 0U;
			if (flag)
			{
				this._playbackIdsByWwisePlaybackId[num] = playbackId;
				this._currentEventPlaybacksByPlaybackId[playbackId] = new AudioDevice.EventPlayback
				{
					WwisePlaybackId = num,
					EventId = eventId,
					SoundObjectReference = soundObjectReference
				};
			}
			else
			{
				string str;
				bool flag2 = !this.ResourceManager.DebugWwiseIds.TryGetValue(eventId, out str);
				if (flag2)
				{
					str = eventId.ToString();
				}
				AudioDevice.Logger.Warn("Failed to play event: " + str);
				this.RemoveEventReference(eventId);
			}
		}

		// Token: 0x06005AAE RID: 23214 RVA: 0x001C3E7C File Offset: 0x001C207C
		public void ActionOnEvent(int playbackId, SoundEngine.AkActionOnEventType actionType, int transitionDuration = 0, SoundEngine.AkCurveInterpolation fadeCurveType = 4)
		{
			bool flag = playbackId == -1;
			if (!flag)
			{
				int num = this._commandMemoryPool.TakeSlot();
				bool flag2 = num >= 64;
				if (flag2)
				{
					ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[num];
					ptr.Type = CommandType.ActionOnEvent;
					ptr.PlaybackId = playbackId;
					ptr.ActionType = actionType;
					ptr.TransitionDuration = transitionDuration;
					ptr.FadeCurveType = fadeCurveType;
					this._commandIdQueue.Enqueue(num);
				}
			}
		}

		// Token: 0x06005AAF RID: 23215 RVA: 0x001C3EF8 File Offset: 0x001C20F8
		private void ProcessActionOnEvent(ref CommandBuffers.CommandData actionOnEvent)
		{
			AudioDevice.EventPlayback eventPlayback;
			bool flag = this._currentEventPlaybacksByPlaybackId.TryGetValue(actionOnEvent.PlaybackId, out eventPlayback);
			if (flag)
			{
				SoundEngine.ExecuteActionOnPlayingID(actionOnEvent.ActionType, eventPlayback.WwisePlaybackId, actionOnEvent.TransitionDuration, actionOnEvent.FadeCurveType);
			}
		}

		// Token: 0x06005AB0 RID: 23216 RVA: 0x001C3F40 File Offset: 0x001C2140
		public void SetPosition(AudioDevice.SoundObjectReference soundObjectReference, Vector3 position, Vector3 frontOrientation)
		{
			bool flag = soundObjectReference.SlotId == -1;
			if (!flag)
			{
				int num = this._commandMemoryPool.TakeSlot();
				bool flag2 = num >= 64;
				if (flag2)
				{
					ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[num];
					ptr.Type = CommandType.SetPosition;
					ptr.SoundObjectReference = soundObjectReference;
					ptr.WorldPosition = position;
					ptr.WorldOrientation = frontOrientation;
					this._commandIdQueue.Enqueue(num);
				}
			}
		}

		// Token: 0x06005AB1 RID: 23217 RVA: 0x001C3FB8 File Offset: 0x001C21B8
		private void ProcessSetPosition(ref CommandBuffers.CommandData postEvent)
		{
			Debug.Assert(postEvent.SoundObjectReference.SoundObjectId != 1U, "The player soundobject should always be at position 0 0 0");
			uint num = this._soundObjectBuffers.SoundObjectId[postEvent.SoundObjectReference.SlotId];
			bool flag = postEvent.SoundObjectReference.SoundObjectId != num;
			if (flag)
			{
				AudioDevice.Logger.Warn("Trying to set position on unreferenced soundobject");
			}
			else
			{
				this._soundObjectBuffers.Position[postEvent.SoundObjectReference.SlotId] = postEvent.WorldPosition;
				this.GetWwiseOrientations(postEvent.WorldOrientation, ref this._soundObjectBuffers.FrontOrientation[postEvent.SoundObjectReference.SlotId], ref this._soundObjectBuffers.TopOrientation[postEvent.SoundObjectReference.SlotId]);
			}
		}

		// Token: 0x06005AB2 RID: 23218 RVA: 0x001C4086 File Offset: 0x001C2286
		private void ProcessRTPC(ref CommandBuffers.CommandData rtpc)
		{
			SoundEngine.SetRTPC((ulong)rtpc.RTPCId, rtpc.Volume);
		}

		// Token: 0x06005AB3 RID: 23219 RVA: 0x001C409C File Offset: 0x001C229C
		private void RemoveEventReference(uint eventId)
		{
			Debug.Assert(ThreadHelper.IsOnThread(this.Thread));
			uint num;
			bool flag = this._eventReferenceCountByEventId.TryGetValue(eventId, out num);
			if (flag)
			{
				num -= 1U;
				bool flag2 = num == 0U;
				if (flag2)
				{
					SoundEngine.PrepareEvent(1, eventId);
					this._eventReferenceCountByEventId.Remove(eventId);
				}
				else
				{
					this._eventReferenceCountByEventId[eventId] = num;
				}
			}
		}

		// Token: 0x06005AB4 RID: 23220 RVA: 0x001C4104 File Offset: 0x001C2304
		private void ProcessCommand(int commandId)
		{
			ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[commandId];
			switch (ptr.Type)
			{
			case CommandType.RefreshBanks:
				this.ProcessRefreshBanks();
				this._commandMemoryPool.ReleaseSlot(commandId);
				break;
			case CommandType.RegisterSoundObject:
				this.ProcessRegisterSoundObject(ref ptr);
				this._commandMemoryPool.ReleaseSlot(commandId);
				break;
			case CommandType.UnregisterSoundObject:
				this.ProcessUnregisterSoundObject(ref ptr);
				this._commandMemoryPool.ReleaseSlot(commandId);
				break;
			case CommandType.SetPosition:
				this.ProcessSetPosition(ref ptr);
				this._commandMemoryPool.ReleaseSlot(commandId);
				break;
			case CommandType.SetListenerPosition:
				this.ProcessSetListenerPosition(ref ptr);
				this._commandMemoryPool.ReleaseSlot(commandId);
				break;
			case CommandType.PostEvent:
				this.ProcessPostEvent(ref ptr);
				this._commandMemoryPool.ReleaseSlot(commandId);
				break;
			case CommandType.ActionOnEvent:
				this.ProcessActionOnEvent(ref ptr);
				this._commandMemoryPool.ReleaseSlot(commandId);
				break;
			case CommandType.SetRTPC:
				this.ProcessRTPC(ref ptr);
				this._commandMemoryPool.ReleasePrioritySlot(commandId);
				break;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06005AB5 RID: 23221 RVA: 0x001C4220 File Offset: 0x001C2420
		public int OutputDeviceCount
		{
			get
			{
				return this._outputDeviceCount;
			}
		}

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06005AB6 RID: 23222 RVA: 0x001C4228 File Offset: 0x001C2428
		// (set) Token: 0x06005AB7 RID: 23223 RVA: 0x001C4251 File Offset: 0x001C2451
		public float MasterVolume
		{
			get
			{
				Debug.Assert(ThreadHelper.IsOnThread(this.Thread));
				return this._masterVolume;
			}
			set
			{
				value = MathHelper.Clamp(value, 0f, 100f);
				this.SetRTPC(this._masterVolumeRTPCName, value);
			}
		}

		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06005AB8 RID: 23224 RVA: 0x001C4274 File Offset: 0x001C2474
		// (set) Token: 0x06005AB9 RID: 23225 RVA: 0x001C427C File Offset: 0x001C247C
		internal Thread Thread { get; private set; }

		// Token: 0x06005ABA RID: 23226 RVA: 0x001C4288 File Offset: 0x001C2488
		public AudioDevice(uint outputDeviceId, string masterVolumeRTPCName, float masterVolume, string[] categoryVolumeRTPCs, float[] categoryVolumes, int soundGroupCount)
		{
			AudioDevice.ErrorDelegate = new AudioDevice.WwiseErrorDelegate(AudioDevice.WwiseErrorCallback);
			SoundEngine.SetLocalOutput(Marshal.GetFunctionPointerForDelegate<AudioDevice.WwiseErrorDelegate>(AudioDevice.ErrorDelegate));
			this._currentOutputDeviceId = outputDeviceId;
			this._masterVolumeRTPCName = masterVolumeRTPCName;
			this._masterVolume = masterVolume;
			this.AudioCategoryStates = new AudioCategoryState[categoryVolumes.Length];
			for (int i = 0; i < this.AudioCategoryStates.Length; i++)
			{
				this.AudioCategoryStates[i] = new AudioCategoryState(i, categoryVolumeRTPCs[i], categoryVolumes[i]);
			}
			SoundEngine.Init();
			SoundEngine.SetBasePath(Path.Combine(Paths.BuiltInAssets, "Common/SoundBanks/Windows"));
			this._commandMemoryPool = new CommandMemoryPool();
			this._commandMemoryPool.Initialize();
			this.ResourceManager = new ResourceManager();
			this._defaultStopEventCallback = delegate(int callbackType, IntPtr eventCallbackInfo)
			{
				SoundEngine.EventCallbackInfo eventCallbackInfo2 = (SoundEngine.EventCallbackInfo)Marshal.PtrToStructure(eventCallbackInfo, typeof(SoundEngine.EventCallbackInfo));
				this._stoppedWwisePlaybackIds.Enqueue(eventCallbackInfo2.PlayingId);
			};
			this._soundObjectMemoryPool = new SoundObjectMemoryPool();
			this._soundObjectMemoryPool.Initialize();
			this._soundObjectBuffers = this._soundObjectMemoryPool.SoundObjects;
			this.Thread = new Thread(new ThreadStart(this.AudioDeviceThreadStart))
			{
				Name = "AudioDeviceThread",
				IsBackground = true
			};
			this.Thread.SetApartmentState(ApartmentState.STA);
			this.Thread.Start();
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x001C4469 File Offset: 0x001C2669
		protected override void DoDispose()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._threadAlive = false;
			this.Thread.Join();
			this.Thread = null;
			this._soundObjectMemoryPool.Release();
			SoundEngine.Term();
		}

		// Token: 0x06005ABC RID: 23228 RVA: 0x001C44A6 File Offset: 0x001C26A6
		public void SetCategoryVolume(int categoryId, float volume)
		{
			this.SetRTPC(this.AudioCategoryStates[categoryId].RtpcName, MathHelper.Clamp(volume, 0f, 100f));
		}

		// Token: 0x06005ABD RID: 23229 RVA: 0x001C44D4 File Offset: 0x001C26D4
		public void SetRTPC(string rtpcName, float value)
		{
			uint rtcpId;
			bool flag = !this.ResourceManager.WwiseGameParameterIds.TryGetValue(rtpcName, out rtcpId);
			if (flag)
			{
				AudioDevice.Logger.Warn("Unknown RTPC {0}", rtpcName);
			}
			else
			{
				this.SetRTPC(rtcpId, value);
			}
		}

		// Token: 0x06005ABE RID: 23230 RVA: 0x001C451C File Offset: 0x001C271C
		public void SetRTPC(uint rtcpId, float value)
		{
			int num = this._commandMemoryPool.TakePrioritySlot();
			bool flag = num < 0;
			if (!flag)
			{
				ref CommandBuffers.CommandData ptr = ref this._commandMemoryPool.Commands.Data[num];
				ptr.Type = CommandType.SetRTPC;
				ptr.Volume = value;
				ptr.RTPCId = rtcpId;
				this._commandIdQueue.Enqueue(num);
			}
		}

		// Token: 0x06005ABF RID: 23231 RVA: 0x001C457C File Offset: 0x001C277C
		public void ReplaceOutputDevice(uint outputDeviceId)
		{
			bool flag = outputDeviceId != this._currentOutputDeviceId;
			if (flag)
			{
				this._currentOutputDeviceId = outputDeviceId;
				SoundEngine.ReplaceOutput(this._currentOutputDeviceId);
			}
		}

		// Token: 0x06005AC0 RID: 23232 RVA: 0x001C45B0 File Offset: 0x001C27B0
		private void RegisterPlayerSoundObject()
		{
			int num = this._soundObjectMemoryPool.TakeSlot();
			Debug.Assert(num == 0, "The player SoundObject is expected to be in SlotId 0");
			this._soundObjectBuffers.SoundObjectId[AudioDevice.PlayerSoundObjectReference.SlotId] = 1U;
			BitUtils.SwitchOnBit(2, ref this._soundObjectBuffers.BoolData[AudioDevice.PlayerSoundObjectReference.SlotId]);
			SoundEngine.RegisterGameObject(1UL);
			SoundEngine.SetDefaultListeners(1UL, 1);
		}

		// Token: 0x06005AC1 RID: 23233 RVA: 0x001C4624 File Offset: 0x001C2824
		private void AudioDeviceThreadStart()
		{
			this.LoadBanks();
			SoundEngine.SetRTPC(this._masterVolumeRTPCName, this._masterVolume);
			for (int i = 0; i < this.AudioCategoryStates.Length; i++)
			{
				ref AudioCategoryState ptr = ref this.AudioCategoryStates[i];
				SoundEngine.SetRTPC(ptr.RtpcName, ptr.Volume);
			}
			bool flag = this._currentOutputDeviceId > 0U;
			if (flag)
			{
				SoundEngine.ReplaceOutput(this._currentOutputDeviceId);
			}
			this.RegisterPlayerSoundObject();
			while (this._threadAlive)
			{
				for (;;)
				{
					int commandId;
					bool flag2 = this._commandIdQueue.TryDequeue(out commandId);
					if (!flag2)
					{
						break;
					}
					this.ProcessCommand(commandId);
				}
				int num = 0;
				for (int j = 1; j < this._soundObjectBuffers.Count; j++)
				{
					uint num2 = this._soundObjectBuffers.SoundObjectId[j];
					bool flag3 = num2 == 0U;
					if (!flag3)
					{
						this._sortedSoundObjectSlotIds[num] = j;
						this._sortedSoundObjectSquaredDistanceToListener[num] = Vector3.DistanceSquared(this._soundObjectBuffers.Position[j], this._currentListenerPosition);
						num++;
					}
				}
				Debug.Assert(this._soundObjectCount == num, "Number of sorted SoundObjects is incorrect");
				Array.Sort<float, int>(this._sortedSoundObjectSquaredDistanceToListener, this._sortedSoundObjectSlotIds, 0, this._soundObjectCount);
				for (int k = 0; k < this._soundObjectCount; k++)
				{
					int num3 = this._sortedSoundObjectSlotIds[k];
					float num4 = this._sortedSoundObjectSquaredDistanceToListener[k];
					uint num5 = this._soundObjectBuffers.SoundObjectId[num3];
					byte bitfield = this._soundObjectBuffers.BoolData[num3];
					bool flag4 = BitUtils.IsBitOn(2, bitfield);
					bool flag5 = !flag4 && num4 < 10000f && k < 100;
					if (flag5)
					{
						SoundEngine.RegisterGameObject((ulong)num5);
						int num6 = this._soundObjectBuffers.LastPlaybackId[num3];
						bool flag6 = num6 != -1 && !this._currentEventPlaybacksByPlaybackId.ContainsKey(num6);
						if (flag6)
						{
							uint eventId;
							bool flag7 = this._soundEventIdsByPlaybackIds.TryGetValue(num6, out eventId) && this.TryPrepareEvent(eventId);
							if (flag7)
							{
								this.PostEventToWwise(num6, eventId, new AudioDevice.SoundObjectReference(num5, num3));
							}
						}
						BitUtils.SwitchOnBit(2, ref this._soundObjectBuffers.BoolData[num3]);
						flag4 = true;
					}
					else
					{
						bool flag8 = flag4 && (num4 > 10000f || k >= 100);
						if (flag8)
						{
							SoundEngine.StopAll((ulong)num5);
							SoundEngine.UnRegisterGameObject((ulong)num5);
							BitUtils.SwitchOffBit(2, ref this._soundObjectBuffers.BoolData[num3]);
							flag4 = false;
						}
					}
					bool flag9 = flag4;
					if (flag9)
					{
						Vector3 vector = this._soundObjectBuffers.Position[num3] - this._currentListenerPosition;
						Vector3 vector2 = this._soundObjectBuffers.FrontOrientation[num3];
						Vector3 vector3 = this._soundObjectBuffers.TopOrientation[num3];
						SoundEngine.SetPosition((ulong)num5, vector.X, vector.Y, -vector.Z, vector2.X, vector2.Y, vector2.Z, vector3.X, vector3.Y, vector3.Z);
					}
				}
				SoundEngine.RenderAudio();
				for (;;)
				{
					uint key;
					bool flag10 = this._stoppedWwisePlaybackIds.TryDequeue(out key);
					if (!flag10)
					{
						break;
					}
					int key2;
					bool flag11 = this._playbackIdsByWwisePlaybackId.TryGetValue(key, out key2);
					if (flag11)
					{
						AudioDevice.EventPlayback eventPlayback = this._currentEventPlaybacksByPlaybackId[key2];
						this._currentEventPlaybacksByPlaybackId.Remove(key2);
						this._playbackIdsByWwisePlaybackId.Remove(key);
						int slotId = eventPlayback.SoundObjectReference.SlotId;
						uint soundObjectId = eventPlayback.SoundObjectReference.SoundObjectId;
						bool flag12 = this._soundObjectBuffers.SoundObjectId[slotId] == soundObjectId;
						if (flag12)
						{
							byte bitfield2 = this._soundObjectBuffers.BoolData[slotId];
							bool flag13 = BitUtils.IsBitOn(0, bitfield2);
							if (flag13)
							{
								this.UnregisterSoundObject(soundObjectId, slotId);
							}
						}
						this.RemoveEventReference(eventPlayback.EventId);
					}
				}
				Thread.Sleep(16);
			}
			this.UnloadBanks();
		}

		// Token: 0x06005AC2 RID: 23234 RVA: 0x001C4A68 File Offset: 0x001C2C68
		public AudioDevice.OutputDevice[] GetOutputDevices()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			Thread thread = new Thread(delegate()
			{
				uint num;
				SoundEngine.GetDeviceCount(ref num);
				ArrayUtils.GrowArrayIfNecessary<AudioDevice.OutputDevice>(ref this._outputDevices, (int)num, 0);
				SoundEngine.DeviceDescription[] array = new SoundEngine.DeviceDescription[num];
				SoundEngine.GetDevices(array, ref num);
				this._outputDeviceCount = (int)num;
				int num2 = 0;
				while ((long)num2 < (long)((ulong)num))
				{
					ref SoundEngine.DeviceDescription ptr = ref array[num2];
					this._outputDevices[num2] = new AudioDevice.OutputDevice
					{
						Id = ptr.DeviceId,
						Name = Marshal.PtrToStringAuto(ptr.DeviceName)
					};
					num2++;
				}
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			return this._outputDevices;
		}

		// Token: 0x06005AC3 RID: 23235 RVA: 0x001C4AB4 File Offset: 0x001C2CB4
		private void LoadBanks()
		{
			uint num;
			SoundEngine.LoadBank("Init.bnk", ref num);
			uint num2;
			SoundEngine.LoadBank("UI.bnk", ref num2);
			uint num3;
			SoundEngine.LoadBank("Master.bnk", ref num3);
			uint num4;
			SoundEngine.LoadBank("Music.bnk", ref num4);
		}

		// Token: 0x06005AC4 RID: 23236 RVA: 0x001C4AF8 File Offset: 0x001C2CF8
		private void UnloadBanks()
		{
			foreach (AudioDevice.EventPlayback eventPlayback in this._currentEventPlaybacksByPlaybackId.Values)
			{
				this.RemoveEventReference(eventPlayback.EventId);
			}
			Debug.Assert(this._eventReferenceCountByEventId.Count == 0);
			this._playbackIdsByWwisePlaybackId.Clear();
			this._currentEventPlaybacksByPlaybackId.Clear();
			SoundEngine.UnloadBank("Music.bnk");
			SoundEngine.UnloadBank("Master.bnk");
			SoundEngine.UnloadBank("UI.bnk");
			SoundEngine.UnloadBank("Init.bnk");
		}

		// Token: 0x06005AC5 RID: 23237 RVA: 0x001C4BB4 File Offset: 0x001C2DB4
		private void UnregisterSoundObject(uint soundObjectId, int slotId)
		{
			Debug.Assert(ThreadHelper.IsOnThread(this.Thread));
			bool flag = BitUtils.IsBitOn(2, this._soundObjectBuffers.BoolData[slotId]);
			if (flag)
			{
				SoundEngine.StopAll((ulong)soundObjectId);
				SoundEngine.UnRegisterGameObject((ulong)soundObjectId);
			}
			int num = this._soundObjectBuffers.LastPlaybackId[slotId];
			bool flag2 = num != -1;
			if (flag2)
			{
				this._soundEventIdsByPlaybackIds.Remove(num);
			}
			this._soundObjectBuffers.SoundObjectId[slotId] = 0U;
			this._soundObjectMemoryPool.ReleaseSlot(slotId);
			this._soundObjectCount--;
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x001C4C50 File Offset: 0x001C2E50
		private void GetWwiseOrientations(Vector3 orientation, ref Vector3 frontOrientation, ref Vector3 topOrientation)
		{
			Vector3 value = new Vector3(0f, 0f, 1f);
			Vector3 value2 = new Vector3(0f, 1f, 0f);
			Quaternion rotation;
			Quaternion.CreateFromYawPitchRoll(-orientation.Yaw, -orientation.Pitch, -orientation.Roll, out rotation);
			frontOrientation = Vector3.Transform(value, rotation);
			frontOrientation.Normalize();
			topOrientation = Vector3.Transform(value2, rotation);
			topOrientation.Normalize();
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x001C4CD4 File Offset: 0x001C2ED4
		private static void WwiseErrorCallback(int errorCode, IntPtr message, int errorLevel, int playingId, int gameObjectId)
		{
			bool flag = message != IntPtr.Zero;
			if (flag)
			{
				AudioDevice.Logger.Warn<int, string>("Audio: {0} - {1}", errorCode, Marshal.PtrToStringAuto(message));
			}
			else
			{
				AudioDevice.Logger.Warn("Audio: {0}", errorCode);
			}
		}

		// Token: 0x0400389F RID: 14495
		private SoundObjectMemoryPool _soundObjectMemoryPool;

		// Token: 0x040038A0 RID: 14496
		private SoundObjectBuffers _soundObjectBuffers;

		// Token: 0x040038A1 RID: 14497
		private Dictionary<uint, uint> _eventReferenceCountByEventId = new Dictionary<uint, uint>();

		// Token: 0x040038A2 RID: 14498
		private Dictionary<uint, int> _playbackIdsByWwisePlaybackId = new Dictionary<uint, int>();

		// Token: 0x040038A3 RID: 14499
		private Dictionary<int, AudioDevice.EventPlayback> _currentEventPlaybacksByPlaybackId = new Dictionary<int, AudioDevice.EventPlayback>();

		// Token: 0x040038A4 RID: 14500
		private Dictionary<int, uint> _soundEventIdsByPlaybackIds = new Dictionary<int, uint>();

		// Token: 0x040038A5 RID: 14501
		public const string InitBank = "Init.bnk";

		// Token: 0x040038A6 RID: 14502
		public const string MasterBank = "Master.bnk";

		// Token: 0x040038A7 RID: 14503
		public const string UIBank = "UI.bnk";

		// Token: 0x040038A8 RID: 14504
		public const string MusicBank = "Music.bnk";

		// Token: 0x040038A9 RID: 14505
		private const int MaxLiveSoundObjects = 100;

		// Token: 0x040038AA RID: 14506
		private const float LiveSoundObjectCullingSquaredDistance = 10000f;

		// Token: 0x040038AB RID: 14507
		public const uint DefaultOutputDeviceId = 0U;

		// Token: 0x040038AC RID: 14508
		public const int EmptySoundEventIndex = 0;

		// Token: 0x040038AD RID: 14509
		public const int EmptySoundObjectId = 0;

		// Token: 0x040038AE RID: 14510
		public const int PlayerSoundObjectId = 1;

		// Token: 0x040038AF RID: 14511
		public const int NoSlotId = -1;

		// Token: 0x040038B0 RID: 14512
		public const int PlayerSoundObjectSlotId = 0;

		// Token: 0x040038B1 RID: 14513
		public static AudioDevice.SoundObjectReference PlayerSoundObjectReference = new AudioDevice.SoundObjectReference
		{
			SoundObjectId = 1U,
			SlotId = 0
		};

		// Token: 0x040038B2 RID: 14514
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040038B3 RID: 14515
		public const int NoPlaybackId = -1;

		// Token: 0x040038B4 RID: 14516
		private uint _currentOutputDeviceId = 0U;

		// Token: 0x040038B5 RID: 14517
		private int _outputDeviceCount = 0;

		// Token: 0x040038B6 RID: 14518
		private AudioDevice.OutputDevice[] _outputDevices = new AudioDevice.OutputDevice[2];

		// Token: 0x040038B8 RID: 14520
		private volatile bool _threadAlive = true;

		// Token: 0x040038B9 RID: 14521
		internal readonly AudioCategoryState[] AudioCategoryStates;

		// Token: 0x040038BA RID: 14522
		public readonly ResourceManager ResourceManager;

		// Token: 0x040038BB RID: 14523
		private uint _nextSoundObjectId = 2U;

		// Token: 0x040038BC RID: 14524
		private int _soundObjectCount = 0;

		// Token: 0x040038BD RID: 14525
		private const int SoundObjectDistanceDefaultSize = 1000;

		// Token: 0x040038BE RID: 14526
		private const int SoundObjectDistanceGrowth = 500;

		// Token: 0x040038BF RID: 14527
		private int[] _sortedSoundObjectSlotIds = new int[1000];

		// Token: 0x040038C0 RID: 14528
		private float[] _sortedSoundObjectSquaredDistanceToListener = new float[1000];

		// Token: 0x040038C1 RID: 14529
		private int _nextPlaybackId = 0;

		// Token: 0x040038C2 RID: 14530
		private readonly ConcurrentQueue<int> _commandIdQueue = new ConcurrentQueue<int>();

		// Token: 0x040038C3 RID: 14531
		private string _masterVolumeRTPCName;

		// Token: 0x040038C4 RID: 14532
		private float _masterVolume;

		// Token: 0x040038C5 RID: 14533
		private Vector3 _currentListenerPosition;

		// Token: 0x040038C6 RID: 14534
		private CommandMemoryPool _commandMemoryPool;

		// Token: 0x040038C7 RID: 14535
		private ConcurrentQueue<uint> _stoppedWwisePlaybackIds = new ConcurrentQueue<uint>();

		// Token: 0x040038C8 RID: 14536
		private SoundEngine.EventCallbackFunc _defaultStopEventCallback;

		// Token: 0x040038C9 RID: 14537
		private static AudioDevice.WwiseErrorDelegate ErrorDelegate;

		// Token: 0x02000F6A RID: 3946
		private struct EventPlayback
		{
			// Token: 0x04004AEE RID: 19182
			public uint WwisePlaybackId;

			// Token: 0x04004AEF RID: 19183
			public uint EventId;

			// Token: 0x04004AF0 RID: 19184
			public AudioDevice.SoundObjectReference SoundObjectReference;
		}

		// Token: 0x02000F6B RID: 3947
		public struct OutputDevice
		{
			// Token: 0x04004AF1 RID: 19185
			public uint Id;

			// Token: 0x04004AF2 RID: 19186
			public string Name;
		}

		// Token: 0x02000F6C RID: 3948
		// (Invoke) Token: 0x060068EC RID: 26860
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate void WwiseErrorDelegate(int errorCode, IntPtr message, int errorLevel, int playingId, int gameObjectId);

		// Token: 0x02000F6D RID: 3949
		public struct SoundObjectReference
		{
			// Token: 0x1700147B RID: 5243
			// (get) Token: 0x060068EF RID: 26863 RVA: 0x0021C5BC File Offset: 0x0021A7BC
			public static AudioDevice.SoundObjectReference Empty
			{
				get
				{
					return AudioDevice.SoundObjectReference._empty;
				}
			}

			// Token: 0x060068F0 RID: 26864 RVA: 0x0021C5D3 File Offset: 0x0021A7D3
			public SoundObjectReference(uint soundObjectSlotId, int slotId)
			{
				this.SoundObjectId = soundObjectSlotId;
				this.SlotId = slotId;
			}

			// Token: 0x04004AF3 RID: 19187
			public int SlotId;

			// Token: 0x04004AF4 RID: 19188
			public uint SoundObjectId;

			// Token: 0x04004AF5 RID: 19189
			private static AudioDevice.SoundObjectReference _empty = new AudioDevice.SoundObjectReference(0U, -1);
		}

		// Token: 0x02000F6E RID: 3950
		public struct SoundEventReference
		{
			// Token: 0x1700147C RID: 5244
			// (get) Token: 0x060068F2 RID: 26866 RVA: 0x0021C5F4 File Offset: 0x0021A7F4
			public static AudioDevice.SoundEventReference None
			{
				get
				{
					return AudioDevice.SoundEventReference.none;
				}
			}

			// Token: 0x060068F3 RID: 26867 RVA: 0x0021C60B File Offset: 0x0021A80B
			public SoundEventReference(AudioDevice.SoundObjectReference soundObjectSlotId, int playbackId)
			{
				this.SoundObjectReference = soundObjectSlotId;
				this.PlaybackId = playbackId;
			}

			// Token: 0x04004AF6 RID: 19190
			public AudioDevice.SoundObjectReference SoundObjectReference;

			// Token: 0x04004AF7 RID: 19191
			public int PlaybackId;

			// Token: 0x04004AF8 RID: 19192
			private static AudioDevice.SoundEventReference none = new AudioDevice.SoundEventReference(AudioDevice.SoundObjectReference.Empty, -1);
		}
	}
}
