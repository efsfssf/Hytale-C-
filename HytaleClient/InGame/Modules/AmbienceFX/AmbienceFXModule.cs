using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Data.AmbienceFX;
using HytaleClient.Data.Map;
using HytaleClient.Data.Weather;
using HytaleClient.InGame.Commands;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.AmbienceFX
{
	// Token: 0x02000994 RID: 2452
	internal class AmbienceFXModule : Module
	{
		// Token: 0x06004E52 RID: 20050 RVA: 0x0015A5A4 File Offset: 0x001587A4
		private void StopAmbientBeds()
		{
			foreach (int playbackId in this._ambientBedPlaybacksByAmbienceFXIndices.Values)
			{
				this._gameInstance.AudioModule.ActionOnEvent(playbackId, 0);
			}
			this._ambientBedPlaybacksByAmbienceFXIndices.Clear();
		}

		// Token: 0x06004E53 RID: 20051 RVA: 0x0015A61C File Offset: 0x0015881C
		private void PlayAmbientBeds()
		{
			for (int i = 0; i < this._ambientBedTaskCount; i++)
			{
				ref AmbienceFXModule.AmbientBedTask ptr = ref this._ambientBedTasks[i];
				int value = this._gameInstance.AudioModule.PlayLocalSoundEvent(ptr.AmbientBedSoundEventIndex);
				this._ambientBedPlaybacksByAmbienceFXIndices[ptr.AmbienceFXIndex] = value;
			}
			List<int> list = new List<int>();
			foreach (int num in this._ambientBedPlaybacksByAmbienceFXIndices.Keys)
			{
				bool flag = !this._allActiveAmbienceFXIndices.Contains(num);
				if (flag)
				{
					list.Add(num);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				int key = list[j];
				this._gameInstance.AudioModule.ActionOnEvent(this._ambientBedPlaybacksByAmbienceFXIndices[key], 0);
				this._ambientBedPlaybacksByAmbienceFXIndices.Remove(key);
			}
		}

		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06004E54 RID: 20052 RVA: 0x0015A740 File Offset: 0x00158940
		// (set) Token: 0x06004E55 RID: 20053 RVA: 0x0015A748 File Offset: 0x00158948
		public AmbienceFXSettings[] AmbienceFXs { get; private set; }

		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06004E56 RID: 20054 RVA: 0x0015A751 File Offset: 0x00158951
		// (set) Token: 0x06004E57 RID: 20055 RVA: 0x0015A759 File Offset: 0x00158959
		public int MusicAmbienceFXIndex { get; private set; } = 0;

		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06004E58 RID: 20056 RVA: 0x0015A762 File Offset: 0x00158962
		// (set) Token: 0x06004E59 RID: 20057 RVA: 0x0015A76A File Offset: 0x0015896A
		public uint CurrentSoundEffectSoundEventIndex { get; private set; }

		// Token: 0x06004E5A RID: 20058 RVA: 0x0015A774 File Offset: 0x00158974
		public AmbienceFXModule(GameInstance gameInstance) : base(gameInstance)
		{
			this.SetupAmbienceDebug();
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x0015A840 File Offset: 0x00158A40
		protected override void DoDispose()
		{
			this.StopCurrentMusic();
			this.StopAmbientBeds();
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x0015A854 File Offset: 0x00158A54
		public void Update(float deltaTime)
		{
			this._accumulatedDeltaTime += deltaTime;
			bool flag = this._accumulatedDeltaTime < 0.016666668f;
			if (!flag)
			{
				this._currentPlayerPosition = this._gameInstance.LocalPlayer.Position;
				this._lastAnalyzeEnvironmentTime += this._accumulatedDeltaTime;
				bool flag2 = this._lastAnalyzeEnvironmentTime > 60f || Vector3.DistanceSquared(this._currentPlayerPosition, this._lastPlayerPosition) > 9f;
				if (flag2)
				{
					this._lastAnalyzeEnvironmentTime = 0f;
					this._lastPlayerPosition = this._currentPlayerPosition;
					this.AnalyzeEnvironment();
				}
				this.UpdateActiveAmbienceFXs();
				this.Play3DSounds();
				this._accumulatedDeltaTime = 0f;
			}
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x0015A914 File Offset: 0x00158B14
		public void PrepareAmbienceFXs(AmbienceFX[] networkAmbienceFXs, out AmbienceFXSettings[] upcomingAmbienceFXs)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingAmbienceFXs = new AmbienceFXSettings[networkAmbienceFXs.Length];
			List<AmbienceFXSoundSettings> validSoundSettings = new List<AmbienceFXSoundSettings>();
			for (int i = 0; i < networkAmbienceFXs.Length; i++)
			{
				AmbienceFXSettings ambienceFXSettings = new AmbienceFXSettings();
				AmbienceFX networkAmbienceFX = networkAmbienceFXs[i];
				AmbienceFXProtocolInitializer.Initialize(networkAmbienceFX, ref ambienceFXSettings, validSoundSettings);
				upcomingAmbienceFXs[i] = ambienceFXSettings;
			}
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x0015A970 File Offset: 0x00158B70
		public void SetupAmbienceFXs(AmbienceFX[] _networkAmbienceFXs, AmbienceFXSettings[] upcomingSettings)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.AmbienceFXs = upcomingSettings;
			for (int i = 0; i < this.AmbienceFXs.Length; i++)
			{
				this._ambienceFXIndicesByIds[this.AmbienceFXs[i].Id] = i;
			}
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x0015A9C4 File Offset: 0x00158BC4
		public void OnAmbienceFXChanged()
		{
			this.MusicAmbienceFXIndex = 0;
			this.StopCurrentMusic();
			this.StopAmbientBeds();
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x0015A9E0 File Offset: 0x00158BE0
		private void AnalyzeEnvironment()
		{
			int num = (int)Math.Floor((double)this._currentPlayerPosition.X);
			int num2 = (int)Math.Floor((double)this._currentPlayerPosition.Y);
			int num3 = (int)Math.Floor((double)this._currentPlayerPosition.Z);
			this._altitude = num2;
			this._hasRoof = false;
			for (int i = num2 + 2; i < num2 + 32; i++)
			{
				int block = this._gameInstance.MapModule.GetBlock(num, i, num3, 0);
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				bool flag = clientBlockType.CollisionMaterial != 1 || clientBlockType.Opacity > 0;
				if (!flag)
				{
					this._hasRoof = true;
					break;
				}
			}
			this._hasFloor = false;
			for (int j = num2 - 1; j > num2 - 4; j--)
			{
				int block2 = this._gameInstance.MapModule.GetBlock(num, j, num3, 0);
				bool flag2 = this._gameInstance.MapModule.ClientBlockTypes[block2].CollisionMaterial != 1;
				if (!flag2)
				{
					this._hasFloor = true;
					break;
				}
			}
			int num4 = 0;
			for (int k = num + 1; k < num + 24; k++)
			{
				int block3 = this._gameInstance.MapModule.GetBlock(k, num2 + 2, num3, 0);
				bool flag3 = this._gameInstance.MapModule.ClientBlockTypes[block3].CollisionMaterial != 1;
				if (!flag3)
				{
					num4++;
					break;
				}
			}
			for (int l = num - 1; l > num - 24; l--)
			{
				int block4 = this._gameInstance.MapModule.GetBlock(l, num2 + 2, num3, 0);
				bool flag4 = this._gameInstance.MapModule.ClientBlockTypes[block4].CollisionMaterial != 1;
				if (!flag4)
				{
					num4++;
					break;
				}
			}
			for (int m = num3 + 1; m < num3 + 24; m++)
			{
				int block5 = this._gameInstance.MapModule.GetBlock(num, num2 + 2, m, 0);
				bool flag5 = this._gameInstance.MapModule.ClientBlockTypes[block5].CollisionMaterial != 1;
				if (!flag5)
				{
					num4++;
					break;
				}
			}
			for (int n = num3 - 1; n > num3 - 24; n--)
			{
				int block6 = this._gameInstance.MapModule.GetBlock(num, num2 + 2, n, 0);
				bool flag6 = this._gameInstance.MapModule.ClientBlockTypes[block6].CollisionMaterial != 1;
				if (!flag6)
				{
					num4++;
					break;
				}
			}
			this._wallsCount = num4;
			int worldChunkX = num >> 5;
			int y = num2 >> 5;
			int worldChunkZ = num3 >> 5;
			ChunkColumn chunkColumn = this._gameInstance.MapModule.GetChunkColumn(worldChunkX, worldChunkZ);
			Chunk chunk = (chunkColumn != null) ? chunkColumn.GetChunk(y) : null;
			bool flag7 = ((chunk != null) ? chunk.Data.BorderedLightAmounts : null) != null;
			if (flag7)
			{
				int indexInChunk = ChunkHelper.IndexOfWorldBlockInChunk(num, num2, num3);
				int num5 = ChunkHelper.IndexOfBlockInBorderedChunk(indexInChunk, 0, 0, 0);
				ushort num6 = chunk.Data.BorderedLightAmounts[num5];
				this._sunLightLevel = (num6 >> 12 & 15);
				this._torchLightLevel = ((num6 >> 8 & 15) | (num6 >> 4 & 15) | (int)(num6 & 15));
			}
			else
			{
				this._sunLightLevel = 0;
				this._torchLightLevel = 0;
			}
			this._globalLightLevel = (int)MathHelper.Max((float)this._sunLightLevel, (float)this._torchLightLevel);
			this._environmentStats.TotalStats = 0;
			for (int num7 = 0; num7 < 16; num7++)
			{
				for (int num8 = 0; num8 < 16; num8++)
				{
					for (int num9 = 0; num9 < 12; num9++)
					{
						int num10 = num + (num7 - 8) * 3 + this._random.Next() % 3;
						int num11 = num2 + (num9 - 6) * 3 + this._random.Next() % 3;
						int num12 = num3 + (num8 - 8) * 3 + this._random.Next() % 3;
						int block7 = this._gameInstance.MapModule.GetBlock(num10, num11, num12, 0);
						this.AddToStats(this._gameInstance.MapModule.ClientBlockTypes[block7].BlockSoundSetIndex, num10, num11, num12);
					}
				}
			}
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x0015AE64 File Offset: 0x00159064
		private void UpdateActiveAmbienceFXs()
		{
			int musicAmbienceFXIndex = this.MusicAmbienceFXIndex;
			this.MusicAmbienceFXIndex = 0;
			int num = 0;
			uint currentSoundEffectSoundEventIndex = this.CurrentSoundEffectSoundEventIndex;
			this.CurrentSoundEffectSoundEventIndex = 0U;
			int num2 = 0;
			this._ambientBedTaskCount = 0;
			this._soundTaskCount = 0;
			this._allActiveAmbienceFXIndices.Clear();
			int currentEnvironmentIndex = this._gameInstance.WeatherModule.CurrentEnvironmentIndex;
			int weatherIndex = (this._gameInstance.WeatherModule.IsChangingWeather && this._gameInstance.WeatherModule.NextWeatherProgress > 0.75f) ? this._gameInstance.WeatherModule.NextWeatherIndex : this._gameInstance.WeatherModule.CurrentWeatherIndex;
			int fluidFXIndex = this._gameInstance.WeatherModule.FluidFXIndex;
			for (int i = 0; i < this.AmbienceFXs.Length; i++)
			{
				AmbienceFXSettings ambienceFXSettings = this.AmbienceFXs[i];
				string text;
				bool flag = ambienceFXSettings.Conditions != null && !this.IsValidAmbience(ambienceFXSettings.Conditions, currentEnvironmentIndex, weatherIndex, fluidFXIndex, out text, false);
				if (!flag)
				{
					bool flag2 = ambienceFXSettings.AmbientBedSoundEventIndex != 0U && !this._ambientBedPlaybacksByAmbienceFXIndices.ContainsKey(i);
					if (flag2)
					{
						ArrayUtils.GrowArrayIfNecessary<AmbienceFXModule.AmbientBedTask>(ref this._ambientBedTasks, this._ambientBedTaskCount, 10);
						this._ambientBedTasks[this._ambientBedTaskCount].AmbientBedSoundEventIndex = ambienceFXSettings.AmbientBedSoundEventIndex;
						this._ambientBedTasks[this._ambientBedTaskCount].AmbienceFXIndex = i;
						this._ambientBedTaskCount++;
					}
					bool flag3 = ambienceFXSettings.Sounds != null && ambienceFXSettings.Sounds.Length != 0;
					if (flag3)
					{
						ArrayUtils.GrowArrayIfNecessary<AmbienceFXSoundSettings>(ref this._soundTasks, this._soundTaskCount, 10);
						for (int j = 0; j < ambienceFXSettings.Sounds.Length; j++)
						{
							this._soundTasks[this._soundTaskCount] = ambienceFXSettings.Sounds[j];
							this._soundTaskCount++;
						}
					}
					bool flag4 = ambienceFXSettings.MusicSoundEventIndex > 0U;
					if (flag4)
					{
						bool flag5 = this.MusicAmbienceFXIndex == 0;
						if (flag5)
						{
							this.MusicAmbienceFXIndex = i;
						}
						bool flag6 = num >= this._allMusicAmbienceFXIndices.Length;
						if (flag6)
						{
							Array.Resize<int>(ref this._allMusicAmbienceFXIndices, num + 2);
						}
						this._allMusicAmbienceFXIndices[num] = i;
						num++;
					}
					bool flag7 = ambienceFXSettings.EffectSoundEventIndex > 0U;
					if (flag7)
					{
						bool flag8 = this.CurrentSoundEffectSoundEventIndex == 0U;
						if (flag8)
						{
							this.CurrentSoundEffectSoundEventIndex = ambienceFXSettings.EffectSoundEventIndex;
						}
						this._allSoundEffectAmbienceFXIndices[num2] = i;
						num2++;
					}
					this._allActiveAmbienceFXIndices.Add(i);
				}
			}
			bool flag9 = musicAmbienceFXIndex != this.MusicAmbienceFXIndex;
			if (flag9)
			{
				bool flag10 = this.MusicAmbienceFXIndex == 0;
				if (flag10)
				{
					this._gameInstance.App.DevTools.Error("No music ambienceFX found!");
				}
				else
				{
					bool flag11 = num > 1;
					if (flag11)
					{
						string[] array = new string[num];
						for (int k = 0; k < num; k++)
						{
							array[k] = this.AmbienceFXs[this._allMusicAmbienceFXIndices[k]].Id;
						}
						this._gameInstance.App.DevTools.Error("Trying to set several music ambienceFX at the same time: " + string.Join(", ", array) + ".");
					}
				}
				this.CurrentMusicSoundEventIndex = this.AmbienceFXs[this.MusicAmbienceFXIndex].MusicSoundEventIndex;
				bool flag12 = this.CurrentMusicSoundEventIndex != this.AmbienceFXs[musicAmbienceFXIndex].MusicSoundEventIndex;
				if (flag12)
				{
					this.StopCurrentMusic();
					this._currentMusicPlaybackId = this._gameInstance.AudioModule.PlayLocalSoundEvent(this.CurrentMusicSoundEventIndex);
				}
			}
			bool flag13 = currentSoundEffectSoundEventIndex != this.CurrentSoundEffectSoundEventIndex;
			if (flag13)
			{
				this._gameInstance.AudioModule.SetWorldSoundEffect(this.CurrentSoundEffectSoundEventIndex);
				bool flag14 = num2 > 1;
				if (flag14)
				{
					string[] array2 = new string[num2];
					for (int l = 0; l < num2; l++)
					{
						array2[l] = this.AmbienceFXs[this._allSoundEffectAmbienceFXIndices[l]].Id;
					}
					this._gameInstance.App.DevTools.Error("Trying to set several sound effects at the same time: " + string.Join(", ", array2) + ".");
				}
			}
			this.PlayAmbientBeds();
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x0015B2E0 File Offset: 0x001594E0
		private void Play3DSounds()
		{
			for (int i = 0; i < this._soundTaskCount; i++)
			{
				AmbienceFXSoundSettings ambienceFXSoundSettings = this._soundTasks[i];
				bool flag = ambienceFXSoundSettings.LastTime == 0f;
				if (flag)
				{
					ambienceFXSoundSettings.NextTime = (float)this._random.Next() % (ambienceFXSoundSettings.Frequency.Min + 1f);
					ambienceFXSoundSettings.LastTime = 1f;
				}
				else
				{
					ambienceFXSoundSettings.NextTime -= 0.016666668f;
					bool flag2 = ambienceFXSoundSettings.NextTime > 0f;
					if (!flag2)
					{
						ambienceFXSoundSettings.NextTime = this._random.NextFloat(ambienceFXSoundSettings.Frequency.Min, ambienceFXSoundSettings.Frequency.Max);
						bool flag3 = ambienceFXSoundSettings.Play3D != AmbienceFXSoundSettings.AmbienceFXSoundPlay3D.No;
						if (flag3)
						{
							this._gameInstance.AudioModule.PlaySoundEvent(ambienceFXSoundSettings.SoundEventIndex, this.GetSoundPosition(ambienceFXSoundSettings), Vector3.Zero);
						}
						else
						{
							int playbackId = this._gameInstance.AudioModule.PlayLocalSoundEvent(ambienceFXSoundSettings.SoundEventIndex);
							this._gameInstance.AudioModule.ActionOnEvent(playbackId, 3);
						}
					}
				}
			}
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x0015B414 File Offset: 0x00159614
		private Vector3 GetSoundPosition(AmbienceFXSoundSettings sound)
		{
			Vector3 vector = this._currentPlayerPosition;
			bool flag = sound.BlockSoundSetIndex != 0;
			if (flag)
			{
				for (int i = 0; i < this._environmentStats.TotalStats; i++)
				{
					ref int ptr = ref this._environmentStats.BlockSoundSetIndices[i];
					bool flag2 = sound.BlockSoundSetIndex != ptr;
					if (!flag2)
					{
						bool flag3 = sound.Play3D == AmbienceFXSoundSettings.AmbienceFXSoundPlay3D.LocationName;
						if (flag3)
						{
							vector = this._environmentStats.GetClosestBlock(i, this._currentPlayerPosition);
						}
						ref BlockEnvironmentStats.BlockStats ptr2 = ref this._environmentStats.Stats[i];
						switch (sound.Altitude)
						{
						case AmbienceFXSoundSettings.AmbienceFXAltitude.Lowest:
							vector.Y = (float)ptr2.LowestAltitude;
							break;
						case AmbienceFXSoundSettings.AmbienceFXAltitude.Highest:
							vector.Y = (float)ptr2.HighestAltitude;
							break;
						case AmbienceFXSoundSettings.AmbienceFXAltitude.Random:
						{
							int num = this._random.Next(3);
							int num2 = num;
							if (num2 != 1)
							{
								if (num2 == 2)
								{
									vector.Y = (float)ptr2.LowestAltitude;
								}
							}
							else
							{
								vector.Y = (float)ptr2.HighestAltitude;
							}
							break;
						}
						}
					}
				}
			}
			int num3 = sound.Radius.Max - sound.Radius.Min;
			bool flag4 = num3 == 0;
			Vector3 result;
			if (flag4)
			{
				result = vector;
			}
			else
			{
				result = new Vector3(vector.X + (float)((this._random.Next() % num3 + sound.Radius.Min) * (this._random.Next() % 2 * 2 - 1)), vector.Y + ((float)(this._random.Next() % num3) * 0.5f + (float)sound.Radius.Min * 0.5f) * (float)(this._random.Next() % 2 * 2 - 1), vector.Z + (float)((this._random.Next() % num3 + sound.Radius.Min) * (this._random.Next() % 2 * 2 - 1)));
			}
			return result;
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x0015B628 File Offset: 0x00159828
		private bool IsValidAmbience(AmbienceFXConditionSettings conditions, int environmentIndex, int weatherIndex, int fluidFxIndex, out string debug, bool debugEnabled = false)
		{
			debug = null;
			bool flag = this._wallsCount < conditions.Walls.Min;
			bool result;
			if (flag)
			{
				if (debugEnabled)
				{
					debug = string.Format("Not enough walls: min={0}", conditions.Walls.Min);
				}
				result = false;
			}
			else
			{
				bool flag2 = this._wallsCount > conditions.Walls.Max;
				if (flag2)
				{
					if (debugEnabled)
					{
						debug = string.Format("Too many walls: max={0}", conditions.Walls.Max);
					}
					result = false;
				}
				else
				{
					bool flag3 = conditions.Roof && !this._hasRoof;
					if (flag3)
					{
						if (debugEnabled)
						{
							debug = "Roof is wrong";
						}
						result = false;
					}
					else
					{
						bool flag4 = conditions.Floor && !this._hasFloor;
						if (flag4)
						{
							if (debugEnabled)
							{
								debug = "Floor is wrong";
							}
							result = false;
						}
						else
						{
							bool flag5 = this._altitude < conditions.Altitude.Min;
							if (flag5)
							{
								if (debugEnabled)
								{
									debug = string.Format("Altitude too low: min={0}", conditions.Altitude.Min);
								}
								result = false;
							}
							else
							{
								bool flag6 = this._altitude > conditions.Altitude.Max;
								if (flag6)
								{
									if (debugEnabled)
									{
										debug = string.Format("Altitude too high: max={0}", conditions.Altitude.Max);
									}
									result = false;
								}
								else
								{
									float gameDayProgressInHours = this._gameInstance.TimeModule.GameDayProgressInHours;
									bool flag7 = conditions.DayTime.Min < conditions.DayTime.Max;
									if (flag7)
									{
										bool flag8 = gameDayProgressInHours < conditions.DayTime.Min || gameDayProgressInHours > conditions.DayTime.Max;
										if (flag8)
										{
											if (debugEnabled)
											{
												debug = string.Format("Not in right time range: {0}-{1}", conditions.DayTime.Min, conditions.DayTime.Max);
											}
											return false;
										}
									}
									else
									{
										bool flag9 = gameDayProgressInHours < conditions.DayTime.Min && gameDayProgressInHours > conditions.DayTime.Max;
										if (flag9)
										{
											if (debugEnabled)
											{
												debug = string.Format("Not in right time range: {0}/{1}", conditions.DayTime.Min, conditions.DayTime.Max);
											}
											return false;
										}
									}
									bool flag10 = this._sunLightLevel < conditions.SunLightLevel.Min;
									if (flag10)
									{
										if (debugEnabled)
										{
											debug = string.Format("Not enough sunlight: min={0}", conditions.SunLightLevel.Min);
										}
										result = false;
									}
									else
									{
										bool flag11 = this._sunLightLevel > conditions.SunLightLevel.Max;
										if (flag11)
										{
											if (debugEnabled)
											{
												debug = string.Format("Too much sunlight: max={0}", conditions.SunLightLevel.Max);
											}
											result = false;
										}
										else
										{
											bool flag12 = this._torchLightLevel < conditions.TorchLightLevel.Min;
											if (flag12)
											{
												if (debugEnabled)
												{
													debug = string.Format("Not enough torchlight: min={0}", conditions.TorchLightLevel.Min);
												}
												result = false;
											}
											else
											{
												bool flag13 = this._torchLightLevel > conditions.TorchLightLevel.Max;
												if (flag13)
												{
													if (debugEnabled)
													{
														debug = string.Format("Too much torchlight: max={0}", conditions.TorchLightLevel.Max);
													}
													result = false;
												}
												else
												{
													bool flag14 = this._globalLightLevel < conditions.GlobalLightLevel.Min;
													if (flag14)
													{
														if (debugEnabled)
														{
															debug = string.Format("Not enough global light: min={0}", conditions.GlobalLightLevel.Min);
														}
														result = false;
													}
													else
													{
														bool flag15 = this._globalLightLevel > conditions.GlobalLightLevel.Max;
														if (flag15)
														{
															if (debugEnabled)
															{
																debug = string.Format("Too much global light: max={0}", conditions.GlobalLightLevel.Max);
															}
															result = false;
														}
														else
														{
															bool flag16 = conditions.EnvironmentIndices != null;
															if (flag16)
															{
																bool flag17 = false;
																for (int i = 0; i < conditions.EnvironmentIndices.Length; i++)
																{
																	bool flag18 = conditions.EnvironmentIndices[i] == environmentIndex;
																	if (flag18)
																	{
																		flag17 = true;
																		break;
																	}
																}
																bool flag19 = !flag17;
																if (flag19)
																{
																	if (debugEnabled)
																	{
																		string[] array = new string[conditions.EnvironmentIndices.Length];
																		for (int j = 0; j < conditions.EnvironmentIndices.Length; j++)
																		{
																			array[j] = this._gameInstance.ServerSettings.Environments[conditions.EnvironmentIndices[j]].Id;
																		}
																		debug = string.Concat(new string[]
																		{
																			"Environment invalid: ",
																			this._gameInstance.WeatherModule.CurrentEnvironment.Id,
																			" not in [ ",
																			string.Join(", ", array),
																			" ]"
																		});
																	}
																	return false;
																}
															}
															bool flag20 = conditions.WeatherIndices != null;
															if (flag20)
															{
																bool flag21 = false;
																for (int k = 0; k < conditions.WeatherIndices.Length; k++)
																{
																	bool flag22 = conditions.WeatherIndices[k] == weatherIndex;
																	if (flag22)
																	{
																		flag21 = true;
																		break;
																	}
																}
																bool flag23 = !flag21;
																if (flag23)
																{
																	if (debugEnabled)
																	{
																		ClientWeather clientWeather = this._gameInstance.ServerSettings.Weathers[weatherIndex];
																		string[] array2 = new string[conditions.WeatherIndices.Length];
																		for (int l = 0; l < conditions.WeatherIndices.Length; l++)
																		{
																			array2[l] = this._gameInstance.ServerSettings.Weathers[conditions.WeatherIndices[l]].Id;
																		}
																		debug = string.Concat(new string[]
																		{
																			"Weather invalid: ",
																			clientWeather.Id,
																			" not in [ ",
																			string.Join(", ", array2),
																			" ]"
																		});
																	}
																	return false;
																}
															}
															bool flag24 = conditions.FluidFXIndices != null;
															if (flag24)
															{
																bool flag25 = conditions.FluidFXIndices.Length == 0 && fluidFxIndex == 0;
																for (int m = 0; m < conditions.FluidFXIndices.Length; m++)
																{
																	bool flag26 = conditions.FluidFXIndices[m] == fluidFxIndex;
																	if (flag26)
																	{
																		flag25 = true;
																		break;
																	}
																}
																bool flag27 = !flag25;
																if (flag27)
																{
																	if (debugEnabled)
																	{
																		debug = "FluidFX invalid: " + this._gameInstance.WeatherModule.FluidFX.Id;
																	}
																	return false;
																}
															}
															bool flag28 = conditions.SurroundingBlockSoundSets != null;
															if (flag28)
															{
																for (int n = 0; n < conditions.SurroundingBlockSoundSets.Length; n++)
																{
																	AmbienceFXConditionSettings.AmbienceFXBlockSoundSet ambienceFXBlockSoundSet = conditions.SurroundingBlockSoundSets[n];
																	float min = ambienceFXBlockSoundSet.Percent.Min;
																	float max = ambienceFXBlockSoundSet.Percent.Max;
																	bool flag29 = this.IsActive(ambienceFXBlockSoundSet.BlockSoundSetIndex, min, max);
																	if (!flag29)
																	{
																		if (debugEnabled)
																		{
																			BlockSoundSet arg = this._gameInstance.ServerSettings.BlockSoundSets[ambienceFXBlockSoundSet.BlockSoundSetIndex];
																			debug = string.Format("Required environment not meeting criteria: {0} min={1}, max={2}", arg, min, max);
																		}
																		return false;
																	}
																}
															}
															result = true;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004E65 RID: 20069 RVA: 0x0015BDD8 File Offset: 0x00159FD8
		public bool IsActive(int blockSoundSetIndex, float min, float max)
		{
			for (int i = 0; i < this._environmentStats.TotalStats; i++)
			{
				ref int ptr = ref this._environmentStats.BlockSoundSetIndices[i];
				bool flag = ptr != blockSoundSetIndex;
				if (!flag)
				{
					ref BlockEnvironmentStats.BlockStats ptr2 = ref this._environmentStats.Stats[i];
					return ptr2.Percent >= min && ptr2.Percent <= max;
				}
			}
			return min == 0f;
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x0015BE60 File Offset: 0x0015A060
		private void AddToStats(int blockSoundSetIndex, int x, int y, int z)
		{
			for (int i = 0; i < this._environmentStats.TotalStats; i++)
			{
				ref int ptr = ref this._environmentStats.BlockSoundSetIndices[i];
				bool flag = blockSoundSetIndex != ptr;
				if (!flag)
				{
					this._environmentStats.Add(i, x, y, z);
					return;
				}
			}
			this._environmentStats.Initialize(blockSoundSetIndex, x, y, z);
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x0015BED0 File Offset: 0x0015A0D0
		private void SetupAmbienceDebug()
		{
			this._gameInstance.RegisterCommand("getAmbience", new GameInstance.Command(this.GetAmbienceCommand));
			this._gameInstance.RegisterCommand("getAmbienceEnv", new GameInstance.Command(this.GetAmbienceEnvCommand));
			this._gameInstance.RegisterCommand("checkAmbience", new GameInstance.Command(this.CheckAmbienceCommand));
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x0015BF38 File Offset: 0x0015A138
		[Usage("getAmbience", new string[]
		{

		})]
		[Description("Dumps current ambience information")]
		private void GetAmbienceCommand(string[] args)
		{
			bool flag = args.Length != 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			this._gameInstance.Chat.Log("Music:");
			string str;
			bool flag2 = !this._gameInstance.Engine.Audio.ResourceManager.DebugWwiseIds.TryGetValue(this.CurrentMusicSoundEventIndex, out str);
			if (flag2)
			{
				str = "None";
			}
			this._gameInstance.Chat.Log(this.AmbienceFXs[this.MusicAmbienceFXIndex].Id + " - " + str);
			this._gameInstance.Chat.Log("Ambient Beds:");
			foreach (int num in this._ambientBedPlaybacksByAmbienceFXIndices.Keys)
			{
				AmbienceFXSettings ambienceFXSettings = this.AmbienceFXs[num];
				string str2;
				bool flag3 = !this._gameInstance.Engine.Audio.ResourceManager.DebugWwiseIds.TryGetValue(ambienceFXSettings.AmbientBedSoundEventIndex, out str2);
				if (flag3)
				{
					str2 = "Not found";
				}
				this._gameInstance.Chat.Log(ambienceFXSettings.Id + " - " + str2);
			}
			List<string> list = new List<string>();
			foreach (int num2 in this._allActiveAmbienceFXIndices)
			{
				AmbienceFXSettings ambienceFXSettings2 = this.AmbienceFXs[num2];
				bool flag4 = ambienceFXSettings2.Sounds != null;
				if (flag4)
				{
					list.Add(ambienceFXSettings2.Id);
				}
			}
			this._gameInstance.Chat.Log("Sounds:");
			this._gameInstance.Chat.Log(string.Join(", ", list));
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x0015C13C File Offset: 0x0015A33C
		[Usage("getAmbienceEnv", new string[]
		{

		})]
		[Description("Prints ambience environment data to chat")]
		private void GetAmbienceEnvCommand(string[] args)
		{
			bool flag = args.Length != 0;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			this._gameInstance.Chat.Log("Environment analysis:");
			this._gameInstance.Chat.Log(string.Format("Altitude ={0}, HasFloor={1}, HasRoof={2}, WallsCount={3}, ", new object[]
			{
				this._altitude,
				this._hasFloor,
				this._hasRoof,
				this._wallsCount
			}) + string.Format("DayTime={0}, SunLight={1}, TorchLight={2}, GlobalLight={3}", new object[]
			{
				this._gameInstance.TimeModule.GameDayProgressInHours,
				this._sunLightLevel,
				this._torchLightLevel,
				this._globalLightLevel
			}));
			this._gameInstance.Chat.Log(string.Format("{0} blocks analyzed in {1}x{2}x{3} cuboid => {4} block environment types found", new object[]
			{
				3072,
				48,
				48,
				36,
				this._environmentStats.TotalStats
			}));
			for (int i = 0; i < this._environmentStats.TotalStats; i++)
			{
				this._gameInstance.Chat.Log(this._environmentStats.GetDebugData(i, this._gameInstance.ServerSettings.BlockSoundSets[this._environmentStats.BlockSoundSetIndices[i]].Id));
			}
		}

		// Token: 0x06004E6A RID: 20074 RVA: 0x0015C2DC File Offset: 0x0015A4DC
		[Usage("checkAmbience", new string[]
		{
			"[ambienceName]"
		})]
		[Description("Dumps information about an ambience by name")]
		private void CheckAmbienceCommand(string[] args)
		{
			bool flag = args.Length != 1;
			if (flag)
			{
				throw new InvalidCommandUsage();
			}
			string text = args[0];
			int num;
			bool flag2 = !this._ambienceFXIndicesByIds.TryGetValue(text, out num);
			if (flag2)
			{
				this._gameInstance.Chat.Log("Ambience \"" + text + "\" not found...");
			}
			else
			{
				AmbienceFXSettings ambienceFXSettings = this.AmbienceFXs[num];
				int currentEnvironmentIndex = this._gameInstance.WeatherModule.CurrentEnvironmentIndex;
				int weatherIndex = (this._gameInstance.WeatherModule.IsChangingWeather && this._gameInstance.WeatherModule.NextWeatherProgress > 0.75f) ? this._gameInstance.WeatherModule.NextWeatherIndex : this._gameInstance.WeatherModule.CurrentWeatherIndex;
				int fluidFXIndex = this._gameInstance.WeatherModule.FluidFXIndex;
				string str;
				bool flag3 = ambienceFXSettings.Conditions != null && !this.IsValidAmbience(ambienceFXSettings.Conditions, currentEnvironmentIndex, weatherIndex, fluidFXIndex, out str, true);
				if (flag3)
				{
					this._gameInstance.Chat.Log("Ambience \"" + text + "\" can't be triggered because: " + str);
				}
				else
				{
					this._gameInstance.Chat.Log("Ambience \"" + text + "\" can be triggered!");
					bool flag4 = ambienceFXSettings.MusicSoundEventIndex == 0U;
					if (flag4)
					{
						this._gameInstance.Chat.Log("Notice this ambience doesn't have any music to play.");
					}
					else
					{
						string str2;
						bool flag5 = !this._gameInstance.Engine.Audio.ResourceManager.DebugWwiseIds.TryGetValue(ambienceFXSettings.MusicSoundEventIndex, out str2);
						if (flag5)
						{
							str2 = "Not found";
						}
						this._gameInstance.Chat.Log("This ambience should play music : " + str2);
					}
					bool flag6 = ambienceFXSettings.AmbientBedSoundEventIndex == 0U;
					if (flag6)
					{
						this._gameInstance.Chat.Log("Notice this ambience doesn't have any ambient bed to play.");
					}
					else
					{
						string str3;
						bool flag7 = !this._gameInstance.Engine.Audio.ResourceManager.DebugWwiseIds.TryGetValue(ambienceFXSettings.AmbientBedSoundEventIndex, out str3);
						if (flag7)
						{
							str3 = "Not found";
						}
						this._gameInstance.Chat.Log("This ambience should play ambient bed: " + str3);
					}
				}
			}
		}

		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06004E6B RID: 20075 RVA: 0x0015C52E File Offset: 0x0015A72E
		// (set) Token: 0x06004E6C RID: 20076 RVA: 0x0015C536 File Offset: 0x0015A736
		public uint CurrentMusicSoundEventIndex { get; private set; }

		// Token: 0x06004E6D RID: 20077 RVA: 0x0015C540 File Offset: 0x0015A740
		private void StopCurrentMusic()
		{
			bool flag = this._currentMusicPlaybackId != -1;
			if (flag)
			{
				this._gameInstance.Engine.Audio.ActionOnEvent(this._currentMusicPlaybackId, 0, 5000, 0);
				this._currentMusicPlaybackId = -1;
			}
		}

		// Token: 0x04002988 RID: 10632
		private readonly Dictionary<int, int> _ambientBedPlaybacksByAmbienceFXIndices = new Dictionary<int, int>();

		// Token: 0x04002989 RID: 10633
		private const float AnalyzeEnvironmentDelay = 60f;

		// Token: 0x0400298A RID: 10634
		private const float AnalyzeEnvironmentSquareDistance = 9f;

		// Token: 0x0400298B RID: 10635
		private const int ZoneRadius = 8;

		// Token: 0x0400298C RID: 10636
		private const int ZoneRadiusHeight = 6;

		// Token: 0x0400298D RID: 10637
		private const int ZoneThreshold = 3;

		// Token: 0x0400298E RID: 10638
		private const int BlocksToAnalyze = 3072;

		// Token: 0x0400298F RID: 10639
		public const int EmptyAmbienceId = 0;

		// Token: 0x04002991 RID: 10641
		private readonly Dictionary<string, int> _ambienceFXIndicesByIds = new Dictionary<string, int>();

		// Token: 0x04002992 RID: 10642
		private const int AllMusicAmbienceDefaultSize = 4;

		// Token: 0x04002993 RID: 10643
		private const int AllMusicAmbienceGrow = 2;

		// Token: 0x04002994 RID: 10644
		private const int AmbienceTasksDefaultSize = 25;

		// Token: 0x04002995 RID: 10645
		private const int AmbienceTasksGrowth = 10;

		// Token: 0x04002996 RID: 10646
		private readonly HashSet<int> _allActiveAmbienceFXIndices = new HashSet<int>();

		// Token: 0x04002997 RID: 10647
		private int _soundTaskCount = 0;

		// Token: 0x04002998 RID: 10648
		private AmbienceFXSoundSettings[] _soundTasks = new AmbienceFXSoundSettings[25];

		// Token: 0x04002999 RID: 10649
		private int _ambientBedTaskCount = 0;

		// Token: 0x0400299A RID: 10650
		private AmbienceFXModule.AmbientBedTask[] _ambientBedTasks = new AmbienceFXModule.AmbientBedTask[25];

		// Token: 0x0400299B RID: 10651
		private int[] _allMusicAmbienceFXIndices = new int[4];

		// Token: 0x0400299D RID: 10653
		private int[] _allSoundEffectAmbienceFXIndices = new int[4];

		// Token: 0x0400299F RID: 10655
		private int _currentSoundEffectPlaybackId = -1;

		// Token: 0x040029A0 RID: 10656
		private readonly Random _random = new Random();

		// Token: 0x040029A1 RID: 10657
		private float _lastAnalyzeEnvironmentTime = 60f;

		// Token: 0x040029A2 RID: 10658
		private Vector3 _currentPlayerPosition;

		// Token: 0x040029A3 RID: 10659
		private Vector3 _lastPlayerPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x040029A4 RID: 10660
		private float _accumulatedDeltaTime;

		// Token: 0x040029A5 RID: 10661
		private readonly BlockEnvironmentStats _environmentStats = new BlockEnvironmentStats(3072);

		// Token: 0x040029A6 RID: 10662
		private int _altitude;

		// Token: 0x040029A7 RID: 10663
		private bool _hasRoof;

		// Token: 0x040029A8 RID: 10664
		private bool _hasFloor;

		// Token: 0x040029A9 RID: 10665
		private int _wallsCount;

		// Token: 0x040029AA RID: 10666
		private int _sunLightLevel;

		// Token: 0x040029AB RID: 10667
		private int _torchLightLevel;

		// Token: 0x040029AC RID: 10668
		private int _globalLightLevel;

		// Token: 0x040029AD RID: 10669
		private const int MusicFadeDuration = 5000;

		// Token: 0x040029AF RID: 10671
		private int _currentMusicPlaybackId;

		// Token: 0x02000E85 RID: 3717
		private struct AmbientBedTask
		{
			// Token: 0x040046E4 RID: 18148
			public uint AmbientBedSoundEventIndex;

			// Token: 0x040046E5 RID: 18149
			public int AmbienceFXIndex;
		}
	}
}
