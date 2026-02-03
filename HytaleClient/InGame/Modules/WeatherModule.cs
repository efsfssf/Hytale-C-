using System;
using System.Diagnostics;
using HytaleClient.Data.Map;
using HytaleClient.Data.Weather;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Particles;
using HytaleClient.Graphics.Sky;
using HytaleClient.InGame.Modules.Map;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008FD RID: 2301
	internal class WeatherModule : Module
	{
		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x060044A6 RID: 17574 RVA: 0x000EA731 File Offset: 0x000E8931
		public int CurrentWeatherIndex
		{
			get
			{
				return (this._editorWeatherOverrideIndex != 0) ? this._editorWeatherOverrideIndex : this._currentEnvironmentWeatherIndex;
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x060044A7 RID: 17575 RVA: 0x000EA749 File Offset: 0x000E8949
		public int NextWeatherIndex
		{
			get
			{
				return this._targetEnvironmentWeatherIndex;
			}
		}

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x000EA751 File Offset: 0x000E8951
		public ClientWeather CurrentWeather
		{
			get
			{
				return this._gameInstance.ServerSettings.Weathers[this.CurrentWeatherIndex];
			}
		}

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x060044A9 RID: 17577 RVA: 0x000EA76A File Offset: 0x000E896A
		public ClientWeather NextWeather
		{
			get
			{
				return this._gameInstance.ServerSettings.Weathers[this._targetEnvironmentWeatherIndex];
			}
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x060044AA RID: 17578 RVA: 0x000EA783 File Offset: 0x000E8983
		public bool IsChangingWeather
		{
			get
			{
				return this._editorWeatherOverrideIndex == 0 && this._targetEnvironmentWeatherIndex != 0;
			}
		}

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x060044AB RID: 17579 RVA: 0x000EA799 File Offset: 0x000E8999
		public float NextWeatherProgress
		{
			get
			{
				return 1f - this._changeWeatherTimer * 0.1f;
			}
		}

		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x060044AC RID: 17580 RVA: 0x000EA7AD File Offset: 0x000E89AD
		// (set) Token: 0x060044AD RID: 17581 RVA: 0x000EA7B5 File Offset: 0x000E89B5
		public int CurrentEnvironmentIndex { get; private set; } = 0;

		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x060044AE RID: 17582 RVA: 0x000EA7BE File Offset: 0x000E89BE
		public ClientWorldEnvironment CurrentEnvironment
		{
			get
			{
				return this._gameInstance.ServerSettings.Environments[this.CurrentEnvironmentIndex];
			}
		}

		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x060044AF RID: 17583 RVA: 0x000EA7D7 File Offset: 0x000E89D7
		// (set) Token: 0x060044B0 RID: 17584 RVA: 0x000EA7DF File Offset: 0x000E89DF
		public float SunLight { get; private set; }

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x060044B1 RID: 17585 RVA: 0x000EA7E8 File Offset: 0x000E89E8
		public Vector3 SunlightColor
		{
			get
			{
				return this._sunlightColor;
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x060044B2 RID: 17586 RVA: 0x000EA7F0 File Offset: 0x000E89F0
		public Vector3 SunColor
		{
			get
			{
				return this._sunColor;
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x060044B3 RID: 17587 RVA: 0x000EA7F8 File Offset: 0x000E89F8
		public Vector4 MoonColor
		{
			get
			{
				return this._moonColor;
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x000EA800 File Offset: 0x000E8A00
		public Vector4 SunGlowColor
		{
			get
			{
				return this._sunGlowColor;
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x060044B5 RID: 17589 RVA: 0x000EA808 File Offset: 0x000E8A08
		public Vector4 MoonGlowColor
		{
			get
			{
				return this._moonGlowColor;
			}
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x060044B6 RID: 17590 RVA: 0x000EA810 File Offset: 0x000E8A10
		public float SunScale
		{
			get
			{
				return this._sunScale;
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x060044B7 RID: 17591 RVA: 0x000EA818 File Offset: 0x000E8A18
		public float MoonScale
		{
			get
			{
				return this._moonScale;
			}
		}

		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x060044B8 RID: 17592 RVA: 0x000EA820 File Offset: 0x000E8A20
		public Vector3 ColorFilter
		{
			get
			{
				return this._colorFilter;
			}
		}

		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x060044B9 RID: 17593 RVA: 0x000EA828 File Offset: 0x000E8A28
		// (set) Token: 0x060044BA RID: 17594 RVA: 0x000EA830 File Offset: 0x000E8A30
		public FluidFX FluidFX { get; private set; }

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x060044BB RID: 17595 RVA: 0x000EA839 File Offset: 0x000E8A39
		// (set) Token: 0x060044BC RID: 17596 RVA: 0x000EA841 File Offset: 0x000E8A41
		public int FluidFXIndex { get; private set; } = 0;

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x060044BD RID: 17597 RVA: 0x000EA84A File Offset: 0x000E8A4A
		public bool IsUnderWater
		{
			get
			{
				return this.FluidFXIndex != 0;
			}
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x060044BE RID: 17598 RVA: 0x000EA855 File Offset: 0x000E8A55
		// (set) Token: 0x060044BF RID: 17599 RVA: 0x000EA85D File Offset: 0x000E8A5D
		public float FluidHeight { get; private set; }

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x060044C0 RID: 17600 RVA: 0x000EA866 File Offset: 0x000E8A66
		// (set) Token: 0x060044C1 RID: 17601 RVA: 0x000EA86E File Offset: 0x000E8A6E
		public Vector3 FluidBlockLightColor { get; private set; }

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x060044C2 RID: 17602 RVA: 0x000EA877 File Offset: 0x000E8A77
		public Vector3 WaterTintColor
		{
			get
			{
				return this._waterTintColor;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x060044C3 RID: 17603 RVA: 0x000EA87F File Offset: 0x000E8A7F
		// (set) Token: 0x060044C4 RID: 17604 RVA: 0x000EA887 File Offset: 0x000E8A87
		public Vector3 NormalizedSunPosition { get; private set; }

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x060044C5 RID: 17605 RVA: 0x000EA890 File Offset: 0x000E8A90
		public Vector4 SkyTopGradientColor
		{
			get
			{
				return this._skyTopGradientColor;
			}
		}

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x060044C6 RID: 17606 RVA: 0x000EA898 File Offset: 0x000E8A98
		public Vector4 SkyBottomGradientColor
		{
			get
			{
				return this._skyBottomGradientColor;
			}
		}

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x060044C7 RID: 17607 RVA: 0x000EA8A0 File Offset: 0x000E8AA0
		public Vector4 SunsetColor
		{
			get
			{
				return this._sunsetColor;
			}
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x060044C8 RID: 17608 RVA: 0x000EA8A8 File Offset: 0x000E8AA8
		// (set) Token: 0x060044C9 RID: 17609 RVA: 0x000EA8B0 File Offset: 0x000E8AB0
		public float SunAngle { get; private set; }

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x060044CA RID: 17610 RVA: 0x000EA8B9 File Offset: 0x000E8AB9
		public float CloudsTransitionOpacity
		{
			get
			{
				return this._cloudsTransitionOpacity;
			}
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x060044CB RID: 17611 RVA: 0x000EA8C1 File Offset: 0x000E8AC1
		public Vector3 FogColor
		{
			get
			{
				return this._fogColor;
			}
		}

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x060044CC RID: 17612 RVA: 0x000EA8C9 File Offset: 0x000E8AC9
		// (set) Token: 0x060044CD RID: 17613 RVA: 0x000EA8D1 File Offset: 0x000E8AD1
		public float LerpFogStart { get; private set; }

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x060044CE RID: 17614 RVA: 0x000EA8DA File Offset: 0x000E8ADA
		// (set) Token: 0x060044CF RID: 17615 RVA: 0x000EA8E2 File Offset: 0x000E8AE2
		public float LerpFogEnd { get; private set; }

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x060044D0 RID: 17616 RVA: 0x000EA8EB File Offset: 0x000E8AEB
		public float FogHeightFalloff
		{
			get
			{
				return this._fogHeightFalloff;
			}
		}

		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x060044D1 RID: 17617 RVA: 0x000EA8F3 File Offset: 0x000E8AF3
		public float FogDensity
		{
			get
			{
				return this._fogDensity;
			}
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x000EA8FC File Offset: 0x000E8AFC
		public WeatherModule(GameInstance gameInstance) : base(gameInstance)
		{
			this.SkyRenderer = new SkyRenderer(this._gameInstance.Engine);
			this.OnDaylightPortionChanged();
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x000EA99A File Offset: 0x000E8B9A
		protected override void DoDispose()
		{
			this.SkyRenderer.Dispose();
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x000EA9AC File Offset: 0x000E8BAC
		public override void Initialize()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.SkyRenderer.Initialize();
			string sunChecksum;
			bool flag = this._gameInstance.HashesByServerAssetPath.TryGetValue("Sky/Sun.png", ref sunChecksum);
			if (flag)
			{
				this.SkyRenderer.LoadSunTexture(sunChecksum);
			}
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x000EA9FC File Offset: 0x000E8BFC
		public void UpdateMoonPhase()
		{
			DateTime gameTime = this._gameInstance.TimeModule.GameTime;
			int num = (gameTime.DayOfYear - 1) % (int)WeatherModule.TotalMoonPhases;
			int moonPhase = this._moonPhase;
			bool flag = gameTime.Hour < 12;
			if (flag)
			{
				bool flag2 = num == 0;
				if (flag2)
				{
					this._moonPhase = (int)(WeatherModule.TotalMoonPhases - 1);
				}
				else
				{
					this._moonPhase = num - 1;
				}
			}
			else
			{
				this._moonPhase = num;
			}
			bool flag3 = this._moonPhase != moonPhase;
			if (flag3)
			{
				this.RequestMoonTextureUpdateFromWeather(this.CurrentWeather, false);
			}
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x000EAA90 File Offset: 0x000E8C90
		public void Update(float deltaTime)
		{
			this._accumulatedDeltaTime += deltaTime;
			bool flag = this._accumulatedDeltaTime < 0.016666668f;
			if (!flag)
			{
				Vector3 position = this._gameInstance.CameraModule.Controller.Position;
				int num = (int)Math.Floor((double)position.X);
				int num2 = (int)Math.Floor((double)position.Y);
				int num3 = (int)Math.Floor((double)position.Z);
				int block = this._gameInstance.MapModule.GetBlock(num, num2, num3, 0);
				bool flag2 = this.FluidFXIndex != 0;
				ClientBlockType clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[block];
				bool flag3 = clientBlockType.FluidBlockId != block;
				if (flag3)
				{
					clientBlockType = this._gameInstance.MapModule.ClientBlockTypes[clientBlockType.FluidBlockId];
				}
				int fluidFXIndex = clientBlockType.FluidFXIndex;
				bool flag4 = this.FluidFXIndex != fluidFXIndex;
				if (flag4)
				{
					this.FluidFXIndex = fluidFXIndex;
					this.FluidFX = this._gameInstance.ServerSettings.FluidFXs[this.FluidFXIndex];
					flag2 = (this.FluidFXIndex != 0);
					bool flag5 = flag2;
					if (flag5)
					{
						int num4 = num2;
						while (this._gameInstance.MapModule.GetBlock(num, num4, num3, 0) != 0)
						{
							num4++;
						}
						this.FluidHeight = (float)num4;
					}
				}
				Vector3 position2 = this._gameInstance.LocalPlayer.Position;
				int num5 = (int)Math.Floor((double)position2.X);
				int worldY = (int)Math.Floor((double)position2.Y);
				int num6 = (int)Math.Floor((double)position2.Z);
				int num7 = num5 >> 5;
				int num8 = num6 >> 5;
				ChunkColumn chunkColumn = this._gameInstance.MapModule.GetChunkColumn(num7, num8);
				bool flag6 = chunkColumn != null;
				if (flag6)
				{
					int chunkX = num5 - num7 * 32;
					int chunkZ = num6 - num8 * 32;
					ushort environmentId = ChunkHelper.GetEnvironmentId(chunkColumn.Environments, chunkX, chunkZ, worldY);
					bool flag7 = this.CurrentEnvironmentIndex == 0 || this.CurrentEnvironmentIndex != (int)environmentId;
					if (flag7)
					{
						this.CurrentEnvironmentIndex = (int)environmentId;
					}
				}
				else
				{
					this.CurrentEnvironmentIndex = 0;
				}
				bool isChangingWeather = this.IsChangingWeather;
				if (isChangingWeather)
				{
					this._changeWeatherTimer = Math.Max(0f, this._changeWeatherTimer - 0.016666668f);
					bool flag8 = this.NextWeatherProgress == 1f;
					if (flag8)
					{
						this._currentEnvironmentWeatherIndex = this._targetEnvironmentWeatherIndex;
						this._targetEnvironmentWeatherIndex = 0;
						this.UpdateEnvironmentWeather(false);
					}
				}
				TimeModule timeModule = this._gameInstance.TimeModule;
				float gameDayProgressInHours = timeModule.GameDayProgressInHours;
				bool flag9 = gameDayProgressInHours < this._halfNightDuration;
				if (flag9)
				{
					this.SunAngle = MathHelper.WrapAngle((gameDayProgressInHours * this._inverseAllNightDay - this._halfNightDuration * this._inverseAllNightDay) * 6.2831855f);
				}
				else
				{
					bool flag10 = gameDayProgressInHours > 24f - this._halfNightDuration;
					if (flag10)
					{
						this.SunAngle = MathHelper.WrapAngle((gameDayProgressInHours * this._inverseAllNightDay - (24f + this._halfNightDuration) * this._inverseAllNightDay) * 6.2831855f);
					}
					else
					{
						this.SunAngle = MathHelper.WrapAngle((gameDayProgressInHours * this._inverseAllDaylightDay - (12f - this._halfDaylightDuration) * this._inverseAllDaylightDay) * 6.2831855f);
					}
				}
				Vector3 normalizedSunPosition = Vector3.Transform(new Vector3((float)Math.Cos((double)this.SunAngle), (float)Math.Sin((double)this.SunAngle) * this.SunHeight, (float)Math.Sin((double)this.SunAngle)), this.SkyRotation);
				normalizedSunPosition.Normalize();
				this.NormalizedSunPosition = normalizedSunPosition;
				ClientWeather currentWeather = this.CurrentWeather;
				this.UpdateTimeValue(out this._sunlightDampingMultiplier, gameDayProgressInHours, currentWeather.SunlightDampingMultipliers);
				this.UpdateColor(out this._sunlightColor, gameDayProgressInHours, currentWeather.SunlightColors);
				this.UpdateColor(out this._sunColor, gameDayProgressInHours, currentWeather.SunColors);
				this.UpdateColor(out this._moonColor, gameDayProgressInHours, currentWeather.MoonColors);
				this.UpdateColor(out this._sunGlowColor, gameDayProgressInHours, currentWeather.SunGlowColors);
				this.UpdateColor(out this._moonGlowColor, gameDayProgressInHours, currentWeather.MoonGlowColors);
				this.UpdateTimeValue(out this._sunScale, gameDayProgressInHours, currentWeather.SunScales);
				this.UpdateTimeValue(out this._moonScale, gameDayProgressInHours, currentWeather.MoonScales);
				this.UpdateColor(out this._colorFilter, gameDayProgressInHours, currentWeather.ColorFilters);
				this.UpdateColor(out this._gameInstance.ScreenEffectStoreModule.WeatherScreenEffectRenderer.Color, gameDayProgressInHours, currentWeather.ScreenEffectColors);
				this.UpdateColor(out this._skyTopGradientColor, gameDayProgressInHours, currentWeather.SkyTopColors);
				this.UpdateColor(out this._skyBottomGradientColor, gameDayProgressInHours, currentWeather.SkyBottomColors);
				this.UpdateColor(out this._sunsetColor, gameDayProgressInHours, currentWeather.SkySunsetColors);
				this.StarsOpacity = 1f;
				this.UpdateTimeValue(out this._fogHeightFalloff, gameDayProgressInHours, currentWeather.FogHeightFalloffs);
				this.UpdateTimeValue(out this._fogDensity, gameDayProgressInHours, currentWeather.FogDensities);
				this._cloudOffset = ((timeModule.IsServerTimePaused && !timeModule.IsEditorTimeOverrideActive) ? ((this._cloudOffset + this._accumulatedDeltaTime * ((float)TimeModule.SecondsPerGameDay / 86400f)) % 1f) : (gameDayProgressInHours % 1f));
				for (int i = 0; i < currentWeather.Clouds.Length; i++)
				{
					this.UpdateColor(out this.SkyRenderer.CloudColors[i], gameDayProgressInHours, currentWeather.Clouds[i].Colors);
					float num9;
					this.UpdateTimeValue(out num9, gameDayProgressInHours, currentWeather.Clouds[i].Speeds);
					int num10 = 0;
					foreach (Tuple<float, float> tuple in currentWeather.Clouds[i].Speeds)
					{
						bool flag11 = tuple.Item1 < gameDayProgressInHours;
						if (flag11)
						{
							num10 = (int)tuple.Item2;
						}
					}
					this.SkyRenderer.CloudOffsets[i] = (float)num10 * this._cloudOffset;
				}
				this.UpdateColor(out this._waterTintColor, gameDayProgressInHours, currentWeather.WaterTints);
				bool flag12 = flag2;
				if (flag12)
				{
					Vector4 lightColorAtBlockPosition = this._gameInstance.MapModule.GetLightColorAtBlockPosition(num, num2, num3);
					this.FluidBlockLightColor = Vector3.Max(WeatherModule.MinFluidLightColor, Vector3.Lerp(this.FluidBlockLightColor, new Vector3(lightColorAtBlockPosition.X, lightColorAtBlockPosition.Y, lightColorAtBlockPosition.Z), 0.01f));
					switch (this.FluidFX.FogMode)
					{
					case 0:
						this._fogColor = new Vector3((float)((byte)this.FluidFX.FogColor.Red) / 255f, (float)((byte)this.FluidFX.FogColor.Green) / 255f, (float)((byte)this.FluidFX.FogColor.Blue) / 255f);
						break;
					case 1:
						this._fogColor = new Vector3((float)((byte)this.FluidFX.FogColor.Red) / 255f * this.FluidBlockLightColor.X, (float)((byte)this.FluidFX.FogColor.Green) / 255f * this.FluidBlockLightColor.Y, (float)((byte)this.FluidFX.FogColor.Blue) / 255f * this.FluidBlockLightColor.Z);
						break;
					case 2:
						this._fogColor = new Vector3(this._waterTintColor.X * this.FluidBlockLightColor.X, this._waterTintColor.Y * this.FluidBlockLightColor.Y, this._waterTintColor.Z * this.FluidBlockLightColor.Z);
						break;
					}
					this.FogDepthStart = this.FluidFX.FogDepthStart;
					this.FogDepthFalloff = this.FluidFX.FogDepthFalloff;
					string currentFluidParticleSystemId = this._currentFluidParticleSystemId;
					FluidParticle particle = this.FluidFX.Particle;
					bool flag13 = currentFluidParticleSystemId != ((particle != null) ? particle.SystemId : null);
					if (flag13)
					{
						FluidParticle particle2 = this.FluidFX.Particle;
						this._currentFluidParticleSystemId = ((particle2 != null) ? particle2.SystemId : null);
						bool flag14 = this._fluidParticleSystemProxy != null;
						if (flag14)
						{
							this._fluidParticleSystemProxy.Expire(false);
							this._fluidParticleSystemProxy = null;
						}
						bool flag15 = this._currentFluidParticleSystemId != null && this._gameInstance.ParticleSystemStoreModule.TrySpawnSystem(this._currentFluidParticleSystemId, out this._fluidParticleSystemProxy, false, true);
						if (flag15)
						{
							bool flag16 = this.FluidFX.Particle.Color_ != null;
							if (flag16)
							{
								this._fluidParticleSystemProxy.DefaultColor = UInt32Color.FromRGBA((byte)this.FluidFX.Particle.Color_.Red, (byte)this.FluidFX.Particle.Color_.Green, (byte)this.FluidFX.Particle.Color_.Blue, byte.MaxValue);
							}
							this._fluidParticleSystemProxy.Scale = this.FluidFX.Particle.Scale;
						}
					}
					bool flag17 = this._previousEnvironmentIndex != this.CurrentEnvironmentIndex || this.FluidFXIndex != this._previousFluidFXId || this._resetFluidEnvironmentParticleSystem;
					if (flag17)
					{
						FluidParticle fluidParticle;
						bool flag18 = this.CurrentEnvironment.FluidParticles.TryGetValue(this.FluidFXIndex, out fluidParticle);
						if (flag18)
						{
							bool flag19 = fluidParticle.SystemId != this._currentFluidEnvironmentParticleSystemId;
							if (flag19)
							{
								this._currentFluidEnvironmentParticleSystemId = fluidParticle.SystemId;
								bool flag20 = this._fluidEnvironmentParticleSystemProxy != null;
								if (flag20)
								{
									this._fluidEnvironmentParticleSystemProxy.Expire(false);
									this._fluidEnvironmentParticleSystemProxy = null;
								}
								bool flag21 = this._currentFluidEnvironmentParticleSystemId != null && this._gameInstance.ParticleSystemStoreModule.TrySpawnSystem(this._currentFluidEnvironmentParticleSystemId, out this._fluidEnvironmentParticleSystemProxy, false, true);
								if (flag21)
								{
									bool flag22 = fluidParticle.Color_ != null;
									if (flag22)
									{
										this._fluidEnvironmentParticleSystemProxy.DefaultColor = UInt32Color.FromRGBA((byte)fluidParticle.Color_.Red, (byte)fluidParticle.Color_.Green, (byte)fluidParticle.Color_.Blue, byte.MaxValue);
									}
									this._fluidEnvironmentParticleSystemProxy.Scale = fluidParticle.Scale;
								}
							}
						}
						else
						{
							bool flag23 = this._fluidEnvironmentParticleSystemProxy != null;
							if (flag23)
							{
								this._fluidEnvironmentParticleSystemProxy.Expire(false);
								this._fluidEnvironmentParticleSystemProxy = null;
								this._currentFluidEnvironmentParticleSystemId = null;
							}
						}
						this._resetFluidEnvironmentParticleSystem = false;
					}
				}
				else
				{
					this.UpdateColor(out this._fogColor, gameDayProgressInHours, currentWeather.FogColors);
					this.FogDepthFalloff = 0f;
					bool flag24 = this._fluidParticleSystemProxy != null;
					if (flag24)
					{
						this._fluidParticleSystemProxy.Expire(false);
						this._fluidParticleSystemProxy = null;
						this._currentFluidParticleSystemId = null;
					}
					bool flag25 = this._fluidEnvironmentParticleSystemProxy != null;
					if (flag25)
					{
						this._fluidEnvironmentParticleSystemProxy.Expire(false);
						this._fluidEnvironmentParticleSystemProxy = null;
						this._currentFluidEnvironmentParticleSystemId = null;
					}
				}
				float value = flag2 ? this.FluidFX.FogDistance.Near : currentWeather.Fog.Near;
				this.LerpFogStart = MathHelper.Min(0f, value);
				float value2 = flag2 ? this.FluidFX.FogDistance.Far : currentWeather.Fog.Far;
				float value3 = (this._gameInstance.WeatherModule.ActiveFogMode == WeatherModule.FogMode.Static) ? ((float)this._gameInstance.App.Settings.ViewDistance) : this._gameInstance.MapModule.EffectiveViewDistance;
				float num11 = MathHelper.Min(value2, MathHelper.Max(value3, 48f));
				this.LerpFogEnd = ((num11 <= this.LerpFogEnd) ? num11 : MathHelper.Lerp(this.LerpFogEnd, num11, 0.05f));
				bool isChangingWeather2 = this.IsChangingWeather;
				if (isChangingWeather2)
				{
					ClientWeather nextWeather = this.NextWeather;
					float value4;
					this.UpdateTimeValue(out value4, gameDayProgressInHours, nextWeather.SunlightDampingMultipliers);
					this._sunlightDampingMultiplier = MathHelper.Lerp(this._sunlightDampingMultiplier, value4, this.NextWeatherProgress);
					this.UpdateColor(out this._tempColor, gameDayProgressInHours, nextWeather.SunlightColors);
					Vector3.Lerp(ref this._sunlightColor, ref this._tempColor, this.NextWeatherProgress, out this._sunlightColor);
					this.UpdateColor(out this._tempColor, gameDayProgressInHours, nextWeather.SunColors);
					Vector3.Lerp(ref this._sunColor, ref this._tempColor, this.NextWeatherProgress, out this._sunColor);
					this.UpdateColor(out this._tempColorAlpha, gameDayProgressInHours, nextWeather.MoonColors);
					Vector4.Lerp(ref this._moonColor, ref this._tempColorAlpha, this.NextWeatherProgress, out this._moonColor);
					this.UpdateColor(out this._tempColorAlpha, gameDayProgressInHours, nextWeather.SunGlowColors);
					Vector4.Lerp(ref this._sunGlowColor, ref this._tempColorAlpha, this.NextWeatherProgress, out this._sunGlowColor);
					this.UpdateColor(out this._tempColorAlpha, gameDayProgressInHours, nextWeather.MoonGlowColors);
					Vector4.Lerp(ref this._moonGlowColor, ref this._tempColorAlpha, this.NextWeatherProgress, out this._moonGlowColor);
					this.UpdateTimeValue(out value4, gameDayProgressInHours, nextWeather.SunScales);
					this._sunScale = MathHelper.Lerp(this._sunScale, value4, this.NextWeatherProgress);
					this.UpdateTimeValue(out value4, gameDayProgressInHours, nextWeather.MoonScales);
					this._moonScale = MathHelper.Lerp(this._moonScale, value4, this.NextWeatherProgress);
					this.UpdateColor(out this._tempColor, gameDayProgressInHours, nextWeather.ColorFilters);
					Vector3.Lerp(ref this._colorFilter, ref this._tempColor, this.NextWeatherProgress, out this._colorFilter);
					this.UpdateColor(out this._tempColorAlpha, gameDayProgressInHours, nextWeather.ScreenEffectColors);
					Vector4.Lerp(ref this._gameInstance.ScreenEffectStoreModule.WeatherScreenEffectRenderer.Color, ref this._tempColorAlpha, this.NextWeatherProgress, out this._gameInstance.ScreenEffectStoreModule.WeatherScreenEffectRenderer.Color);
					this.UpdateColor(out this._tempColorAlpha, gameDayProgressInHours, nextWeather.SkyTopColors);
					Vector4.Lerp(ref this._skyTopGradientColor, ref this._tempColorAlpha, this.NextWeatherProgress, out this._skyTopGradientColor);
					this.UpdateColor(out this._tempColorAlpha, gameDayProgressInHours, nextWeather.SkyBottomColors);
					Vector4.Lerp(ref this._skyBottomGradientColor, ref this._tempColorAlpha, this.NextWeatherProgress, out this._skyBottomGradientColor);
					this.UpdateColor(out this._tempColorAlpha, gameDayProgressInHours, nextWeather.SkySunsetColors);
					Vector4.Lerp(ref this._sunsetColor, ref this._tempColorAlpha, this.NextWeatherProgress, out this._sunsetColor);
					this.UpdateColor(out this._tempColor, gameDayProgressInHours, nextWeather.WaterTints);
					Vector3.Lerp(ref this._waterTintColor, ref this._tempColor, this.NextWeatherProgress, out this._waterTintColor);
					this.UpdateTimeValue(out value4, gameDayProgressInHours, nextWeather.FogHeightFalloffs);
					this._fogHeightFalloff = MathHelper.Lerp(this._fogHeightFalloff, value4, this.NextWeatherProgress);
					this.UpdateTimeValue(out value4, gameDayProgressInHours, nextWeather.FogDensities);
					this._fogDensity = MathHelper.Lerp(this._fogDensity, value4, this.NextWeatherProgress);
					for (int k = 0; k < nextWeather.Clouds.Length; k++)
					{
						this.UpdateColor(out this._tempColorAlpha, gameDayProgressInHours, nextWeather.Clouds[k].Colors);
						Vector4.Lerp(ref this.SkyRenderer.CloudColors[k], ref this._tempColorAlpha, this.NextWeatherProgress, out this.SkyRenderer.CloudColors[k]);
					}
					float num12 = 0f;
					bool flag26 = this.NextWeatherProgress < 0.25f;
					if (flag26)
					{
						num12 = 1f - this.NextWeatherProgress * 4f;
					}
					this._cloudsTransitionOpacity = (this._transitionClouds ? num12 : 1f);
					this._moonTransitionOpacity = (this._transitionMoon ? num12 : 1f);
					this._starsTransitionOpacity = (this._transitionStars ? num12 : 1f);
					this._screenEffectTransitionOpacity = (this._transitionScreenEffect ? num12 : 1f);
					bool flag27 = this.NextWeatherProgress >= 0.75f && !this._hasRequestedSkyTextureUpdate;
					if (flag27)
					{
						this._hasRequestedSkyTextureUpdate = true;
						this.RequestTextureUpdateFromWeather(nextWeather, false);
						this.SetWeatherParticles(nextWeather.Particle);
					}
					bool flag28 = !flag2;
					if (flag28)
					{
						this.UpdateColor(out this._tempColor, gameDayProgressInHours, nextWeather.FogColors);
						Vector3.Lerp(ref this._fogColor, ref this._tempColor, this.NextWeatherProgress, out this._fogColor);
						float near = nextWeather.Fog.Near;
						float value5 = MathHelper.Min(0f, near);
						this.LerpFogStart = MathHelper.Lerp(this.LerpFogStart, value5, this.NextWeatherProgress);
						float far = nextWeather.Fog.Far;
						float value6 = MathHelper.Min(far, MathHelper.Max(value3, 48f));
						this.LerpFogEnd = MathHelper.Lerp(this.LerpFogEnd, value6, this.NextWeatherProgress);
					}
				}
				this.SunLight = MathHelper.Clamp((float)(Math.Sin((double)this.SunAngle) * 2.0 + 0.2) * this._sunlightDampingMultiplier, 0.125f, this._sunlightDampingMultiplier);
				bool flag29 = flag2;
				if (flag29)
				{
					this.FluidHorizonPosition = new Vector3(position.X, this.FluidHeight / 2f, position.Z);
					this.FluidHorizonScale = new Vector3(this.LerpFogEnd, this.FluidHeight, this.LerpFogEnd);
				}
				bool flag30 = !this.SkyRenderer.IsCloudsTextureLoading;
				if (flag30)
				{
					this._cloudsTransitionOpacity = MathHelper.Lerp(this._cloudsTransitionOpacity, 1f, 0.01f);
				}
				for (int l = 0; l < currentWeather.Clouds.Length; l++)
				{
					Vector4[] cloudColors = this.SkyRenderer.CloudColors;
					int num13 = l;
					cloudColors[num13].W = cloudColors[num13].W * this._cloudsTransitionOpacity;
				}
				bool flag31 = -this.NormalizedSunPosition.Y < 0.2f;
				if (flag31)
				{
					this._moonColor.W = this._moonColor.W * (-this.NormalizedSunPosition.Y / 0.2f);
				}
				bool flag32 = !this.SkyRenderer.IsMoonTextureLoading;
				if (flag32)
				{
					this._moonTransitionOpacity = MathHelper.Lerp(this._moonTransitionOpacity, 1f, 0.01f);
				}
				this._moonColor.W = this._moonColor.W * this._moonTransitionOpacity;
				bool flag33 = !this.SkyRenderer.IsStarsTextureLoading;
				if (flag33)
				{
					this._starsTransitionOpacity = MathHelper.Lerp(this._starsTransitionOpacity, 1f, 0.01f);
				}
				this.StarsOpacity *= this._starsTransitionOpacity;
				bool flag34 = this._gameInstance.ScreenEffectStoreModule.EntityScreenEffects.Count > 0;
				if (flag34)
				{
					this._screenEffectTransitionOpacity = MathHelper.Lerp(this._screenEffectTransitionOpacity, 0f, 0.01f);
				}
				else
				{
					bool flag35 = !this._gameInstance.ScreenEffectStoreModule.WeatherScreenEffectRenderer.IsScreenEffectTextureLoading;
					if (flag35)
					{
						this._screenEffectTransitionOpacity = MathHelper.Lerp(this._screenEffectTransitionOpacity, 1f, 0.01f);
					}
				}
				ScreenEffectRenderer weatherScreenEffectRenderer = this._gameInstance.ScreenEffectStoreModule.WeatherScreenEffectRenderer;
				weatherScreenEffectRenderer.Color.W = weatherScreenEffectRenderer.Color.W * this._screenEffectTransitionOpacity;
				bool flag36 = this._particleSystemProxy != null;
				if (flag36)
				{
					this._particleSystemProxy.Position = position + (position - this._lastCameraPosition) * this._particleSystemPositionOffsetMultiplier;
					this._particleSystemProxy.Rotation = Quaternion.CreateFromYawPitchRoll(this._gameInstance.CameraModule.Controller.Rotation.Yaw, 0f, 0f);
					this._lastCameraPosition = position;
				}
				bool flag37 = this._fluidParticleSystemProxy != null;
				if (flag37)
				{
					this._fluidParticleSystemProxy.Position = position;
					this._fluidParticleSystemProxy.Rotation = Quaternion.CreateFromYawPitchRoll(this._gameInstance.CameraModule.Controller.Rotation.Yaw, 0f, 0f);
				}
				bool flag38 = this._fluidEnvironmentParticleSystemProxy != null;
				if (flag38)
				{
					this._fluidEnvironmentParticleSystemProxy.Position = position;
					this._fluidEnvironmentParticleSystemProxy.Rotation = Quaternion.CreateFromYawPitchRoll(this._gameInstance.CameraModule.Controller.Rotation.Yaw, 0f, 0f);
				}
				this._accumulatedDeltaTime = 0f;
				this._previousFluidFXId = this.FluidFXIndex;
				this._previousEnvironmentIndex = this.CurrentEnvironmentIndex;
			}
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x000EBF54 File Offset: 0x000EA154
		private float GetProgress(float startTime, float endTime, float dayTime)
		{
			bool flag = startTime > endTime;
			if (flag)
			{
				endTime += 24f;
				bool flag2 = dayTime < startTime;
				if (flag2)
				{
					dayTime += 24f;
				}
			}
			return (dayTime - startTime) / (endTime - startTime);
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x000EBF94 File Offset: 0x000EA194
		private void UpdateTimeValue(out float value, float dayTime, Tuple<float, float>[] list)
		{
			bool flag = list.Length == 1;
			if (flag)
			{
				value = list[0].Item2;
			}
			else
			{
				int num = list.Length - 1;
				for (int i = 0; i < list.Length; i++)
				{
					bool flag2 = list[i].Item1 <= dayTime;
					if (flag2)
					{
						num = i;
					}
				}
				Tuple<float, float> tuple = list[num];
				int num2 = (num + 1) % list.Length;
				Tuple<float, float> tuple2 = list[num2];
				value = MathHelper.Lerp(tuple.Item2, tuple2.Item2, this.GetProgress(tuple.Item1, tuple2.Item1, dayTime));
			}
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x000EC02C File Offset: 0x000EA22C
		private void UpdateColor(out Vector3 color, float dayTime, Tuple<float, Color>[] list)
		{
			bool flag = list.Length == 1;
			if (flag)
			{
				Tuple<float, Color> tuple = list[0];
				color.X = (float)((byte)tuple.Item2.Red) / 255f;
				color.Y = (float)((byte)tuple.Item2.Green) / 255f;
				color.Z = (float)((byte)tuple.Item2.Blue) / 255f;
			}
			else
			{
				int num = list.Length - 1;
				for (int i = 0; i < list.Length; i++)
				{
					bool flag2 = list[i].Item1 <= dayTime;
					if (flag2)
					{
						num = i;
					}
				}
				Tuple<float, Color> tuple2 = list[num];
				int num2 = (num + 1) % list.Length;
				Tuple<float, Color> tuple3 = list[num2];
				byte r = (byte)tuple2.Item2.Red;
				byte g = (byte)tuple2.Item2.Green;
				byte b = (byte)tuple2.Item2.Blue;
				ColorHsva color2 = ColorHsva.FromRgba(r, g, b, byte.MaxValue);
				byte r2 = (byte)tuple3.Item2.Red;
				byte g2 = (byte)tuple3.Item2.Green;
				byte b2 = (byte)tuple3.Item2.Blue;
				ColorHsva color3 = ColorHsva.FromRgba(r2, g2, b2, byte.MaxValue);
				float num3;
				ColorHsva.Lerp(color2, color3, this.GetProgress(tuple2.Item1, tuple3.Item1, dayTime)).ToRgba(out color.X, out color.Y, out color.Z, out num3);
			}
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x000EC19C File Offset: 0x000EA39C
		private void UpdateColor(out Vector4 color, float dayTime, Tuple<float, ColorAlpha>[] list)
		{
			bool flag = list.Length == 1;
			if (flag)
			{
				Tuple<float, ColorAlpha> tuple = list[0];
				color.X = (float)((byte)tuple.Item2.Red) / 255f;
				color.Y = (float)((byte)tuple.Item2.Green) / 255f;
				color.Z = (float)((byte)tuple.Item2.Blue) / 255f;
				color.W = (float)((byte)tuple.Item2.Alpha) / 255f;
			}
			else
			{
				int num = list.Length - 1;
				for (int i = 0; i < list.Length; i++)
				{
					bool flag2 = list[i].Item1 <= dayTime;
					if (flag2)
					{
						num = i;
					}
				}
				Tuple<float, ColorAlpha> tuple2 = list[num];
				int num2 = (num + 1) % list.Length;
				Tuple<float, ColorAlpha> tuple3 = list[num2];
				byte a = (byte)tuple2.Item2.Alpha;
				byte r = (byte)tuple2.Item2.Red;
				byte g = (byte)tuple2.Item2.Green;
				byte b = (byte)tuple2.Item2.Blue;
				ColorHsva color2 = ColorHsva.FromRgba(r, g, b, a);
				byte a2 = (byte)tuple3.Item2.Alpha;
				byte r2 = (byte)tuple3.Item2.Red;
				byte g2 = (byte)tuple3.Item2.Green;
				byte b2 = (byte)tuple3.Item2.Blue;
				ColorHsva color3 = ColorHsva.FromRgba(r2, g2, b2, a2);
				ColorHsva.Lerp(color2, color3, this.GetProgress(tuple2.Item1, tuple3.Item1, dayTime)).ToRgba(out color.X, out color.Y, out color.Z, out color.W);
			}
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x000EC340 File Offset: 0x000EA540
		public void UpdateEnvironmentWeather(bool fromForecast = false)
		{
			bool flag = this._targetEnvironmentWeatherIndex != 0;
			if (!flag)
			{
				bool flag2 = this._currentEnvironmentWeatherIndex != 0 && this._serverWeatherIndex == this._currentEnvironmentWeatherIndex;
				if (!flag2)
				{
					float gameDayProgressInHours = this._gameInstance.TimeModule.GameDayProgressInHours;
					bool flag3 = this._currentEnvironmentWeatherIndex == 0 || this._editorWeatherOverrideIndex != 0;
					if (flag3)
					{
						this._currentEnvironmentWeatherIndex = this._serverWeatherIndex;
						bool flag4 = this._editorWeatherOverrideIndex == 0;
						if (flag4)
						{
							ClientWeather currentWeather = this.CurrentWeather;
							this.RequestTextureUpdateFromWeather(currentWeather, false);
							this.SetWeatherParticles(currentWeather.Particle);
							this._hasRequestedSkyTextureUpdate = true;
						}
					}
					else
					{
						this._targetEnvironmentWeatherIndex = this._serverWeatherIndex;
						this._changeWeatherTimer = 10f;
						this._hasRequestedSkyTextureUpdate = false;
						ClientWeather clientWeather = this._gameInstance.ServerSettings.Weathers[this._currentEnvironmentWeatherIndex];
						ClientWeather nextWeather = this.NextWeather;
						this._transitionClouds = (nextWeather.Clouds.Length != clientWeather.Clouds.Length);
						for (int i = 0; i < nextWeather.Clouds.Length; i++)
						{
							this._transitionClouds = (this._transitionClouds || clientWeather.Clouds[i].Texture != nextWeather.Clouds[i].Texture);
							if (!fromForecast)
							{
								int num = 0;
								foreach (Tuple<float, float> tuple in clientWeather.Clouds[i].Speeds)
								{
									bool flag5 = tuple.Item1 < gameDayProgressInHours;
									if (flag5)
									{
										num = (int)tuple.Item2;
									}
								}
								int num2 = 0;
								foreach (Tuple<float, float> tuple2 in nextWeather.Clouds[i].Speeds)
								{
									bool flag6 = tuple2.Item1 < gameDayProgressInHours;
									if (flag6)
									{
										num2 = (int)tuple2.Item2;
									}
								}
								this._transitionClouds = (this._transitionClouds || num != num2);
								bool transitionClouds = this._transitionClouds;
								if (transitionClouds)
								{
									break;
								}
							}
						}
						this._transitionMoon = false;
						string a;
						string b;
						bool flag7 = clientWeather.Moons.TryGetValue(this._moonPhase, out a) && nextWeather.Moons.TryGetValue(this._moonPhase, out b);
						if (flag7)
						{
							this._transitionMoon = (a != b);
						}
						this._transitionStars = (clientWeather.Stars != nextWeather.Stars);
						this._transitionScreenEffect = (clientWeather.ScreenEffect != nextWeather.ScreenEffect);
						this.SetWeatherParticles(null);
					}
				}
			}
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x000EC5F0 File Offset: 0x000EA7F0
		public void SetServerWeather(int weatherIndex)
		{
			this._serverWeatherIndex = weatherIndex;
			this.UpdateEnvironmentWeather(true);
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x000EC604 File Offset: 0x000EA804
		public void SetEditorWeatherOverride(int weatherIndex)
		{
			this._editorWeatherOverrideIndex = weatherIndex;
			ClientWeather currentWeather = this.CurrentWeather;
			this.RequestTextureUpdateFromWeather(currentWeather, false);
			this.SetWeatherParticles(currentWeather.Particle);
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x000EC638 File Offset: 0x000EA838
		private void SetWeatherParticles(Weather.WeatherParticle particle)
		{
			bool flag = this._particleSystemProxy != null;
			if (flag)
			{
				this._particleSystemProxy.Expire(false);
				this._particleSystemProxy = null;
			}
			bool flag2 = particle == null || particle.SystemId == null;
			if (!flag2)
			{
				bool flag3 = this._gameInstance.ParticleSystemStoreModule.TrySpawnSystem(particle.SystemId, out this._particleSystemProxy, false, true);
				if (flag3)
				{
					bool flag4 = particle.Color_ != null;
					if (flag4)
					{
						this._particleSystemProxy.DefaultColor = UInt32Color.FromRGBA((byte)particle.Color_.Red, (byte)particle.Color_.Green, (byte)particle.Color_.Blue, byte.MaxValue);
					}
					this._particleSystemProxy.Scale = ((particle.Scale != 0f) ? particle.Scale : 1f);
					this._particleSystemProxy.IsOvergroundOnly = particle.IsOvergroundOnly;
				}
				this._particleSystemPositionOffsetMultiplier = particle.PositionOffsetMultiplier;
			}
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x000EC72D File Offset: 0x000EA92D
		public void ResetParticleSystems()
		{
			this.SetWeatherParticles(this.CurrentWeather.Particle);
			this._currentFluidParticleSystemId = null;
			this._currentFluidEnvironmentParticleSystemId = null;
			this._resetFluidEnvironmentParticleSystem = true;
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x000EC757 File Offset: 0x000EA957
		public void OnFluidFXChanged()
		{
			this.FluidFXIndex = 0;
			this._currentFluidParticleSystemId = null;
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x000EC76C File Offset: 0x000EA96C
		public void OnDaylightPortionChanged()
		{
			this._daylightDuration = (float)WeatherModule.DaylightPortion * 24f * 0.01f;
			this._halfDaylightDuration = this._daylightDuration * 0.5f;
			this._inverseAllDaylightDay = 1f / (this._daylightDuration * 2f);
			this._nightDuration = 24f - this._daylightDuration;
			this._halfNightDuration = this._nightDuration * 0.5f;
			this._inverseAllNightDay = 1f / (this._nightDuration * 2f);
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x000EC7F8 File Offset: 0x000EA9F8
		public void OnEnvironmentCollectionChanged()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.CurrentEnvironmentIndex = 0;
			this._previousEnvironmentIndex = 0;
			this._currentFluidEnvironmentParticleSystemId = null;
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x000EC81C File Offset: 0x000EAA1C
		public void OnWeatherCollectionChanged()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.SetWeatherParticles(this.CurrentWeather.Particle);
			this.RequestTextureUpdateFromWeather(this.CurrentWeather, true);
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x000EC84A File Offset: 0x000EAA4A
		public void RequestTextureUpdateFromWeather(ClientWeather weather, bool forceUpdate = false)
		{
			this.RequestScreenEffectTextureUpdateFromWeather(weather, forceUpdate);
			this.RequestStarsTextureUpdateFromWeather(weather, forceUpdate);
			this.RequestMoonTextureUpdateFromWeather(weather, forceUpdate);
			this.RequestCloudsTextureUpdateFromWeather(weather, forceUpdate);
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x000EC874 File Offset: 0x000EAA74
		public void RequestScreenEffectTextureUpdateFromWeather(ClientWeather weather, bool forceUpdate = false)
		{
			string targetScreenEffectTextureChecksum = null;
			bool flag = weather.ScreenEffect != null;
			if (flag)
			{
				bool flag2 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(weather.ScreenEffect, ref targetScreenEffectTextureChecksum);
				if (flag2)
				{
					this._gameInstance.App.DevTools.Error("Missing weather screen effect asset " + weather.ScreenEffect + " for weather " + weather.Id);
				}
			}
			this._gameInstance.ScreenEffectStoreModule.WeatherScreenEffectRenderer.RequestTextureUpdate(targetScreenEffectTextureChecksum, forceUpdate);
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x000EC900 File Offset: 0x000EAB00
		public void RequestStarsTextureUpdateFromWeather(ClientWeather weather, bool forceUpdate = false)
		{
			string targetStarsTextureChecksum = null;
			bool flag = weather.Stars != null;
			if (flag)
			{
				bool flag2 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(weather.Stars, ref targetStarsTextureChecksum);
				if (flag2)
				{
					this._gameInstance.App.DevTools.Error("Missing stars asset " + weather.Stars + " for weather " + weather.Id);
				}
			}
			this.SkyRenderer.RequestStarsTextureUpdate(targetStarsTextureChecksum, forceUpdate);
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x000EC980 File Offset: 0x000EAB80
		public void RequestMoonTextureUpdateFromWeather(ClientWeather weather, bool forceUpdate = false)
		{
			string targetMoonTextureChecksum = null;
			string text;
			bool flag = weather.Moons.TryGetValue(this._moonPhase, out text) && !this._gameInstance.HashesByServerAssetPath.TryGetValue(text, ref targetMoonTextureChecksum);
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Missing weather moon asset " + text + " for weather " + weather.Id);
			}
			this.SkyRenderer.RequestMoonTextureUpdate(targetMoonTextureChecksum, forceUpdate);
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x000ECA00 File Offset: 0x000EAC00
		public void RequestCloudsTextureUpdateFromWeather(ClientWeather weather, bool forceUpdate = false)
		{
			string[] array = new string[4];
			for (int i = 0; i < 4; i++)
			{
				bool flag = weather.Clouds[i].Texture != null && !this._gameInstance.HashesByServerAssetPath.TryGetValue(weather.Clouds[i].Texture, ref array[i]);
				if (flag)
				{
					this._gameInstance.App.DevTools.Error("Missing weather cloud asset " + weather.Clouds[i].Texture + " for weather " + weather.Id);
				}
			}
			this.SkyRenderer.RequestCloudsTextureUpdate(array, forceUpdate);
		}

		// Token: 0x04002213 RID: 8723
		public static byte TotalMoonPhases = 6;

		// Token: 0x04002214 RID: 8724
		public static byte DaylightPortion = 50;

		// Token: 0x04002215 RID: 8725
		public const float SunDefaultHeight = 2f;

		// Token: 0x04002216 RID: 8726
		private const float ChangeWeatherDelay = 10f;

		// Token: 0x04002217 RID: 8727
		private const float InverseChangeWeatherDelay = 0.1f;

		// Token: 0x04002218 RID: 8728
		private const float MoonsetHeight = 0.2f;

		// Token: 0x04002219 RID: 8729
		public readonly SkyRenderer SkyRenderer;

		// Token: 0x0400221B RID: 8731
		private int _previousEnvironmentIndex;

		// Token: 0x0400221C RID: 8732
		private int _serverWeatherIndex = 0;

		// Token: 0x0400221D RID: 8733
		private int _editorWeatherOverrideIndex = 0;

		// Token: 0x0400221E RID: 8734
		private int _currentEnvironmentWeatherIndex = 0;

		// Token: 0x0400221F RID: 8735
		private int _targetEnvironmentWeatherIndex = 0;

		// Token: 0x04002220 RID: 8736
		private float _changeWeatherTimer;

		// Token: 0x04002221 RID: 8737
		private bool _hasRequestedSkyTextureUpdate;

		// Token: 0x04002222 RID: 8738
		private float _accumulatedDeltaTime;

		// Token: 0x04002223 RID: 8739
		private int _moonPhase;

		// Token: 0x04002227 RID: 8743
		private int _previousFluidFXId = -1;

		// Token: 0x04002229 RID: 8745
		public Vector3 FluidHorizonPosition;

		// Token: 0x0400222A RID: 8746
		public Vector3 FluidHorizonScale;

		// Token: 0x0400222C RID: 8748
		private static readonly Vector3 MinFluidLightColor = new Vector3(0.4f);

		// Token: 0x0400222F RID: 8751
		public WeatherModule.FogMode ActiveFogMode = WeatherModule.FogMode.Dynamic;

		// Token: 0x04002232 RID: 8754
		public float FogDepthStart;

		// Token: 0x04002233 RID: 8755
		public float FogDepthFalloff;

		// Token: 0x04002234 RID: 8756
		private const float FogLerpFactor = 0.05f;

		// Token: 0x04002235 RID: 8757
		private const float MinFogLength = 48f;

		// Token: 0x04002236 RID: 8758
		private Vector3 _tempColor;

		// Token: 0x04002237 RID: 8759
		private Vector4 _tempColorAlpha;

		// Token: 0x04002238 RID: 8760
		private Vector4 _skyTopGradientColor;

		// Token: 0x04002239 RID: 8761
		private Vector4 _skyBottomGradientColor;

		// Token: 0x0400223A RID: 8762
		private Vector4 _sunsetColor;

		// Token: 0x0400223B RID: 8763
		private Vector3 _sunlightColor = Vector3.One;

		// Token: 0x0400223C RID: 8764
		private Vector3 _sunColor;

		// Token: 0x0400223D RID: 8765
		private Vector4 _moonColor;

		// Token: 0x0400223E RID: 8766
		private Vector4 _sunGlowColor;

		// Token: 0x0400223F RID: 8767
		private Vector4 _moonGlowColor;

		// Token: 0x04002240 RID: 8768
		public float StarsOpacity;

		// Token: 0x04002241 RID: 8769
		private float _starsTransitionOpacity;

		// Token: 0x04002242 RID: 8770
		private float _moonTransitionOpacity;

		// Token: 0x04002243 RID: 8771
		private bool _transitionClouds;

		// Token: 0x04002244 RID: 8772
		private bool _transitionMoon;

		// Token: 0x04002245 RID: 8773
		private bool _transitionStars;

		// Token: 0x04002246 RID: 8774
		public float SunHeight = 2f;

		// Token: 0x04002247 RID: 8775
		private float _sunlightDampingMultiplier;

		// Token: 0x04002248 RID: 8776
		private float _sunScale;

		// Token: 0x04002249 RID: 8777
		private float _moonScale;

		// Token: 0x0400224A RID: 8778
		private float _daylightDuration;

		// Token: 0x0400224B RID: 8779
		private float _halfDaylightDuration;

		// Token: 0x0400224C RID: 8780
		private float _inverseAllDaylightDay;

		// Token: 0x0400224D RID: 8781
		private float _nightDuration;

		// Token: 0x0400224E RID: 8782
		private float _halfNightDuration;

		// Token: 0x0400224F RID: 8783
		private float _inverseAllNightDay;

		// Token: 0x04002250 RID: 8784
		public Quaternion SkyRotation = Quaternion.Identity;

		// Token: 0x04002251 RID: 8785
		private Vector3 _colorFilter;

		// Token: 0x04002252 RID: 8786
		private Vector3 _waterTintColor;

		// Token: 0x04002253 RID: 8787
		private float _cloudOffset = 0f;

		// Token: 0x04002254 RID: 8788
		private float _cloudsTransitionOpacity;

		// Token: 0x04002255 RID: 8789
		private Vector3 _fogColor;

		// Token: 0x04002256 RID: 8790
		private float _fogHeightFalloff;

		// Token: 0x04002257 RID: 8791
		private float _fogDensity;

		// Token: 0x04002258 RID: 8792
		private ParticleSystemProxy _particleSystemProxy;

		// Token: 0x04002259 RID: 8793
		private ParticleSystemProxy _fluidParticleSystemProxy;

		// Token: 0x0400225A RID: 8794
		private ParticleSystemProxy _fluidEnvironmentParticleSystemProxy;

		// Token: 0x0400225B RID: 8795
		private string _currentFluidParticleSystemId;

		// Token: 0x0400225C RID: 8796
		private string _currentFluidEnvironmentParticleSystemId;

		// Token: 0x0400225D RID: 8797
		private bool _resetFluidEnvironmentParticleSystem = false;

		// Token: 0x0400225E RID: 8798
		private float _particleSystemPositionOffsetMultiplier;

		// Token: 0x0400225F RID: 8799
		private Vector3 _lastCameraPosition;

		// Token: 0x04002260 RID: 8800
		private bool _transitionScreenEffect;

		// Token: 0x04002261 RID: 8801
		private float _screenEffectTransitionOpacity;

		// Token: 0x02000DCB RID: 3531
		public enum FogMode
		{
			// Token: 0x040043FA RID: 17402
			Dynamic,
			// Token: 0x040043FB RID: 17403
			Static,
			// Token: 0x040043FC RID: 17404
			Off
		}
	}
}
