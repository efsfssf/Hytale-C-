using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Math;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000AB5 RID: 2741
	internal class ParticleSpawner
	{
		// Token: 0x0600564C RID: 22092 RVA: 0x0019C288 File Offset: 0x0019A488
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ComputeParticleScale(int particleIndex, int keyframeIndex, ref ParticleSettings.ScaleKeyframe scaleKeyframe, out Vector2 result)
		{
			float num = this._random.NextFloat(scaleKeyframe.Min.X, scaleKeyframe.Max.X);
			float num2 = this._random.NextFloat(scaleKeyframe.Min.Y, scaleKeyframe.Max.Y);
			bool flag = this._particleSettings.ScaleRatio != ParticleSettings.ScaleRatioConstraint.None;
			if (flag)
			{
				ref float ptr = ref this._particleBuffer.ScaleRatio[particleIndex];
				bool flag2 = keyframeIndex == 0;
				if (flag2)
				{
					ptr = ((this._particleSettings.ScaleRatio == ParticleSettings.ScaleRatioConstraint.Preserved) ? (num / num2) : 1f);
				}
				num2 = num * ptr;
			}
			result = new Vector2(num, num2);
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x0019C33C File Offset: 0x0019A53C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ComputeParticleRotation(int rotationIdex, ref ParticleSettings.RotationKeyframe rotationKeyframe, out Vector3 result)
		{
			result.X = this._random.NextFloat(rotationKeyframe.Min.X, rotationKeyframe.Max.X);
			result.Y = this._random.NextFloat(rotationKeyframe.Min.Y, rotationKeyframe.Max.Y);
			result.Z = this._random.NextFloat(rotationKeyframe.Min.Z, rotationKeyframe.Max.Z);
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x0019C3C0 File Offset: 0x0019A5C0
		private void UpdateParticleAnimation(int particleIndex, float deltaTime)
		{
			ref ParticleBuffers.ParticleLifeData ptr = ref this._particleBuffer.Life[particleIndex];
			ref Vector2 ptr2 = ref this._particleBuffer.Scale[particleIndex];
			ref ParticleBuffers.ParticleSimulationData ptr3 = ref this._particleBuffer.Data0[particleIndex];
			ref ParticleBuffers.ParticleRenderData ptr4 = ref this._particleBuffer.Data1[particleIndex];
			float num = ptr.LifeSpan - ptr.LifeSpanTimer;
			UInt32Color uint32Color = this.DefaultColor;
			UInt32Color uint32Color2 = this.DefaultColor;
			float num2 = 1f;
			float value = 1f;
			bool flag = BitUtils.IsBitOn(2, ptr4.BoolData);
			bool flag2 = flag && this._spawnerSettings.ParticleCollisionAction == ParticleFXSystem.ParticleCollisionAction.LastFrame;
			if (flag2)
			{
				bool flag3 = this._particleSettings.ColorKeyframes.Length != 0;
				if (flag3)
				{
					ptr4.Color = this._particleSettings.ColorKeyframes[this._particleSettings.ColorKeyframes.Length - 1].Color.ABGR;
				}
				bool flag4 = this._particleSettings.OpacityKeyframes.Length != 0;
				if (flag4)
				{
					ptr4.Color |= (uint)(this._particleSettings.OpacityKeyframes[this._particleSettings.OpacityKeyframes.Length - 1].Opacity * 255f) << 24;
				}
				bool flag5 = this._particleSettings.ScaleKeyframes.Length != 0 && (int)ptr3.ScaleAnimationIndex != this._particleSettings.ScaleKeyframes.Length - 1;
				if (flag5)
				{
					ref ParticleSettings.ScaleKeyframe scaleKeyframe = ref this._particleSettings.ScaleKeyframes[this._particleSettings.ScaleKeyframes.Length - 1];
					this.ComputeParticleScale(particleIndex, this._particleSettings.ScaleKeyframes.Length - 1, ref scaleKeyframe, out ptr2);
					ptr3.ScaleAnimationIndex = (byte)(this._particleSettings.ScaleKeyframes.Length - 1);
				}
				bool flag6 = this._particleSettings.RotationKeyframes.Length != 0 && (int)ptr3.RotationAnimationIndex != this._particleSettings.RotationKeyframes.Length - 1;
				if (flag6)
				{
					this.ComputeParticleRotation(this._particleSettings.RotationKeyframes.Length - 1, ref this._particleSettings.RotationKeyframes[this._particleSettings.RotationKeyframes.Length - 1], out ptr3.CurrentRotation);
					ptr3.RotationAnimationIndex = (byte)(this._particleSettings.RotationKeyframes.Length - 1);
				}
				ptr4.Rotation = ptr3.RotationOffset * Quaternion.CreateFromYawPitchRoll(ptr3.CurrentRotation.Yaw, ptr3.CurrentRotation.Pitch, ptr3.CurrentRotation.Roll);
				bool flag7 = this._particleSettings.TextureIndexKeyFrames.Length != 0 && (int)ptr3.TextureAnimationIndex != this._particleSettings.TextureIndexKeyFrames.Length - 1;
				if (flag7)
				{
					ref ParticleSettings.RangeKeyframe ptr5 = ref this._particleSettings.TextureIndexKeyFrames[this._particleSettings.TextureIndexKeyFrames.Length - 1];
					ptr4.TargetTextureIndex = (byte)this._random.Next((int)ptr5.Min, (int)(ptr5.Max + 1));
					ptr4.PrevTargetTextureIndex = ptr4.TargetTextureIndex;
					ptr4.TargetTextureBlendProgress = 0;
					ptr3.TextureAnimationIndex = (byte)(this._particleSettings.TextureIndexKeyFrames.Length - 1);
				}
			}
			else
			{
				float num3 = 0.01f * ptr.LifeSpan;
				float num4 = 0f;
				float num5 = 100f;
				for (int i = 0; i < (int)this._particleSettings.ColorKeyFrameCount; i++)
				{
					ref ParticleSettings.ColorKeyframe ptr6 = ref this._particleSettings.ColorKeyframes[i];
					float num6 = (float)ptr6.Time * num3;
					bool flag8 = num6 <= num;
					if (!flag8)
					{
						uint32Color2 = ptr6.Color;
						num5 = num6;
						break;
					}
					uint32Color = ptr6.Color;
					uint32Color2 = uint32Color;
					num4 = num6;
				}
				float num7 = num5 - num4;
				float num8 = (num7 != 0f) ? ((num - num4) / num7) : 0f;
				ptr4.Color = ((uint)MathHelper.Lerp((float)uint32Color.GetR(), (float)uint32Color2.GetR(), num8) | (uint)MathHelper.Lerp((float)uint32Color.GetG(), (float)uint32Color2.GetG(), num8) << 8 | (uint)MathHelper.Lerp((float)uint32Color.GetB(), (float)uint32Color2.GetB(), num8) << 16);
				num4 = 0f;
				num5 = 100f;
				for (int j = 0; j < (int)this._particleSettings.OpacityKeyFrameCount; j++)
				{
					ref ParticleSettings.OpacityKeyframe ptr7 = ref this._particleSettings.OpacityKeyframes[j];
					float num9 = (float)ptr7.Time * num3;
					bool flag9 = num9 <= num;
					if (!flag9)
					{
						value = ptr7.Opacity;
						num5 = num9;
						break;
					}
					num2 = ptr7.Opacity;
					value = num2;
					num4 = num9;
				}
				num7 = num5 - num4;
				num8 = ((num7 != 0f) ? ((num - num4) / num7) : 0f);
				ptr4.Color |= (uint)(MathHelper.Lerp(num2, value, num8) * 255f) << 24;
				bool flag10 = ptr3.ScaleNextKeyframeTime <= num;
				if (flag10)
				{
					float num10 = 0f;
					for (byte b = ptr3.ScaleAnimationIndex; b < this._particleSettings.ScaleKeyFrameCount; b += 1)
					{
						ref ParticleSettings.ScaleKeyframe ptr8 = ref this._particleSettings.ScaleKeyframes[(int)b];
						float num11 = (float)ptr8.Time * num3;
						bool flag11 = num11 <= num;
						if (!flag11)
						{
							this.ComputeParticleScale(particleIndex, (int)b, ref ptr8, out ptr3.ScaleStep);
							ptr3.ScaleStep /= num11 - num10;
							ptr3.ScaleAnimationIndex = b;
							ptr3.ScaleNextKeyframeTime = num11;
							break;
						}
						bool flag12 = b == 0;
						if (flag12)
						{
							this.ComputeParticleScale(particleIndex, (int)b, ref ptr8, out ptr2);
						}
						ptr3.ScaleStep = Vector2.Zero;
						num10 = num11;
						ptr3.ScaleNextKeyframeTime = ptr.LifeSpan;
					}
				}
				ptr2 += ptr3.ScaleStep * deltaTime;
				bool flag13 = ptr3.RotationNextKeyframeTime <= num;
				if (flag13)
				{
					float num12 = 0f;
					for (byte b2 = ptr3.RotationAnimationIndex; b2 < this._particleSettings.RotationKeyFrameCount; b2 += 1)
					{
						ref ParticleSettings.RotationKeyframe ptr9 = ref this._particleSettings.RotationKeyframes[(int)b2];
						float num13 = (float)ptr9.Time * num3;
						bool flag14 = num13 <= num;
						if (!flag14)
						{
							this.ComputeParticleRotation((int)b2, ref ptr9, out ptr3.RotationStep);
							ptr3.RotationStep /= num13 - num12;
							ptr3.RotationAnimationIndex = b2;
							ptr3.RotationNextKeyframeTime = num13;
							break;
						}
						bool flag15 = b2 == 0;
						if (flag15)
						{
							this.ComputeParticleRotation((int)b2, ref ptr9, out ptr3.CurrentRotation);
						}
						ptr3.RotationStep = Vector3.Zero;
						num12 = num13;
						ptr3.RotationNextKeyframeTime = ptr.LifeSpan;
					}
				}
				ptr3.CurrentRotation += ptr3.RotationStep * deltaTime;
				ptr4.Rotation = Quaternion.CreateFromYawPitchRoll(ptr3.CurrentRotation.Yaw, ptr3.CurrentRotation.Pitch, ptr3.CurrentRotation.Roll);
				bool flag16 = ptr3.TextureNextKeyframeTime <= num;
				if (flag16)
				{
					for (byte b3 = ptr3.TextureAnimationIndex; b3 < this._particleSettings.TextureKeyFrameCount; b3 += 1)
					{
						ref ParticleSettings.RangeKeyframe ptr10 = ref this._particleSettings.TextureIndexKeyFrames[(int)b3];
						float num14 = (float)ptr10.Time * num3;
						bool flag17 = num14 <= num;
						if (!flag17)
						{
							ptr3.TextureAnimationIndex = b3;
							ptr3.TextureNextKeyframeTime = num14;
							ptr4.PrevTargetTextureIndex = ptr4.TargetTextureIndex;
							ptr4.TargetTextureIndex = (byte)this._random.Next((int)ptr10.Min, (int)(ptr10.Max + 1));
							break;
						}
						ptr3.TextureNextKeyframeTime = ptr.LifeSpan;
					}
				}
				bool flag18 = ptr3.TextureAnimationIndex >= 1;
				if (flag18)
				{
					num4 = (float)this._particleSettings.TextureIndexKeyFrames[(int)(ptr3.TextureAnimationIndex - 1)].Time * num3;
					num5 = (float)this._particleSettings.TextureIndexKeyFrames[(int)ptr3.TextureAnimationIndex].Time * num3;
				}
				else
				{
					num5 = (num4 = 0f);
				}
				num7 = num5 - num4;
				num8 = ((num7 != 0f) ? ((num - num4) / num7) : 0f);
				num8 = MathHelper.Clamp(num8, 0f, 1f);
				bool flag19 = !this._useSpriteBlending;
				if (flag19)
				{
					num8 = ((num8 == 1f) ? 1f : 0f);
				}
				ptr4.TargetTextureBlendProgress = (ushort)(num8 * 65535f);
			}
		}

		// Token: 0x17001328 RID: 4904
		// (get) Token: 0x0600564F RID: 22095 RVA: 0x0019CC9B File Offset: 0x0019AE9B
		public int ParticleVertexDataStartIndex
		{
			get
			{
				return this._particleVertexDataStartIndex;
			}
		}

		// Token: 0x17001329 RID: 4905
		// (get) Token: 0x06005650 RID: 22096 RVA: 0x0019CCA3 File Offset: 0x0019AEA3
		public int ParticleDrawCount
		{
			get
			{
				return this._particleDrawCount;
			}
		}

		// Token: 0x1700132A RID: 4906
		// (get) Token: 0x06005651 RID: 22097 RVA: 0x0019CCAB File Offset: 0x0019AEAB
		public FXSystem.RenderMode RenderMode
		{
			get
			{
				return this._spawnerSettings.RenderMode;
			}
		}

		// Token: 0x1700132B RID: 4907
		// (get) Token: 0x06005652 RID: 22098 RVA: 0x0019CCB8 File Offset: 0x0019AEB8
		public ParticleFXSystem.ParticleCollisionBlockType ParticleCollisionBlockType
		{
			get
			{
				return this._spawnerSettings.ParticleCollisionBlockType;
			}
		}

		// Token: 0x1700132C RID: 4908
		// (get) Token: 0x06005653 RID: 22099 RVA: 0x0019CCC5 File Offset: 0x0019AEC5
		public ParticleFXSystem.ParticleCollisionAction ParticleCollisionAction
		{
			get
			{
				return this._spawnerSettings.ParticleCollisionAction;
			}
		}

		// Token: 0x1700132D RID: 4909
		// (get) Token: 0x06005654 RID: 22100 RVA: 0x0019CCD2 File Offset: 0x0019AED2
		public int ActiveParticles
		{
			get
			{
				return this._activeParticles;
			}
		}

		// Token: 0x1700132E RID: 4910
		// (get) Token: 0x06005655 RID: 22101 RVA: 0x0019CCDA File Offset: 0x0019AEDA
		// (set) Token: 0x06005656 RID: 22102 RVA: 0x0019CCE2 File Offset: 0x0019AEE2
		public float Scale { get; private set; } = 1f;

		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06005657 RID: 22103 RVA: 0x0019CCEB File Offset: 0x0019AEEB
		// (set) Token: 0x06005658 RID: 22104 RVA: 0x0019CCF3 File Offset: 0x0019AEF3
		public float ScaleFactor { get; private set; } = 1f;

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06005659 RID: 22105 RVA: 0x0019CCFC File Offset: 0x0019AEFC
		public float LightInfluence
		{
			get
			{
				return this._spawnerSettings.LightInfluence;
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x0600565A RID: 22106 RVA: 0x0019CD09 File Offset: 0x0019AF09
		public bool IsDistortion
		{
			get
			{
				return this._spawnerSettings.RenderMode == FXSystem.RenderMode.Distortion;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x0600565B RID: 22107 RVA: 0x0019CD19 File Offset: 0x0019AF19
		public bool IsLowRes
		{
			get
			{
				return this._spawnerSettings.IsLowRes;
			}
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x0019CD28 File Offset: 0x0019AF28
		public ParticleSpawner(ParticleFXSystem particleFXSystem, UpdateParticleCollisionFunc updateCollisionFunc, InitParticleFunc initParticleFunc, Random random, ParticleSpawnerSettings spawnerSettings, bool useSoftParticles, Vector2 textureAltasInverseSize, int particleBufferStartIndex)
		{
			this._particleFXSystem = particleFXSystem;
			this._updateCollisionFunc = updateCollisionFunc;
			this._initParticleFunc = initParticleFunc;
			this._random = random;
			this._spawnerSettings = spawnerSettings;
			this._particleSettings = spawnerSettings.ParticleSettings;
			this._particleBuffer = this._particleFXSystem.ParticleBuffer;
			this._particleCount = this._spawnerSettings.MaxConcurrentParticles;
			this._particleBufferStartIndex = particleBufferStartIndex;
			this._drawData.UVMotion = new Vector4(this._spawnerSettings.UVMotion.Speed, this._spawnerSettings.UVMotion.Strength, this._spawnerSettings.UVMotion.Scale);
			this._drawData.UVMotionTextureId = (float)this._spawnerSettings.UVMotion.TextureId;
			this._drawData.AddRandomUVOffset = (this._spawnerSettings.UVMotion.AddRandomUVOffset ? 1f : 0f);
			this._drawData.StrengthCurveType = (int)this._spawnerSettings.UVMotion.StrengthCurveType;
			this._drawData.IntersectionHighlight = new Vector4(this._spawnerSettings.IntersectionHighlight.Color.X, this._spawnerSettings.IntersectionHighlight.Color.Y, this._spawnerSettings.IntersectionHighlight.Color.Z, this._spawnerSettings.IntersectionHighlight.Threshold);
			this._drawData.CameraOffset = this._spawnerSettings.CameraOffset;
			this._drawData.VelocityStretchMultiplier = this._spawnerSettings.VelocityStretchMultiplier;
			this._drawData.SoftParticlesFadeFactor = this._particleSettings.SoftParticlesFadeFactor;
			this._useSpriteBlending = this._particleSettings.UseSpriteBlending;
			for (int i = this._particleBufferStartIndex; i < this._particleCount + this._particleBufferStartIndex; i++)
			{
				ref ParticleBuffers.ParticleLifeData ptr = ref this._particleBuffer.Life[i];
				ref Vector2 ptr2 = ref this._particleBuffer.Scale[i];
				ptr.LifeSpanTimer = 0f;
				ptr2 = Vector2.Zero;
			}
			this.UpdateTextures(textureAltasInverseSize);
			this._particlesLeftToEmit = this._random.Next(this._spawnerSettings.TotalParticles.X, this._spawnerSettings.TotalParticles.Y + 1);
			this._lifeSpanTimer = this._spawnerSettings.LifeSpan;
			this._hasWaves = (this._spawnerSettings.WaveDelay != Vector2.Zero);
			this._useSoftParticles = useSoftParticles;
			this._particlesPerWave = this._spawnerSettings.MaxConcurrentParticles;
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x0019D034 File Offset: 0x0019B234
		public void Dispose()
		{
			this._particleBufferStartIndex = 0;
			this._particleCount = 0;
			this._initParticleFunc = null;
			this._updateCollisionFunc = null;
			this._particleFXSystem = null;
		}

		// Token: 0x0600565E RID: 22110 RVA: 0x0019D05C File Offset: 0x0019B25C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Expire(bool clearActiveParticles = false)
		{
			if (clearActiveParticles)
			{
				this._activeParticles = 0;
			}
			this._particlesLeftToEmit = 0;
		}

		// Token: 0x0600565F RID: 22111 RVA: 0x0019D080 File Offset: 0x0019B280
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsExpired()
		{
			return this.ActiveParticles == 0 && this._particlesLeftToEmit == 0;
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x0019D0A8 File Offset: 0x0019B2A8
		private void UpdateTextures(Vector2 textureAltasInverseSize)
		{
			this._textureAltasInverseSize = textureAltasInverseSize;
			this._drawData.ImageLocation = this._particleSettings.ImageLocation;
			UShortVector2 frameSize = this._particleSettings.FrameSize;
			bool flag = frameSize.X == 0 || frameSize.Y == 0 || (int)frameSize.X > this._drawData.ImageLocation.Width || (int)frameSize.Y > this._drawData.ImageLocation.Height;
			if (flag)
			{
				this._drawData.FrameSize.X = (ushort)this._drawData.ImageLocation.Width;
				this._drawData.FrameSize.Y = (ushort)this._drawData.ImageLocation.Height;
			}
			else
			{
				this._drawData.FrameSize = frameSize;
			}
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x0019D17C File Offset: 0x0019B37C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SpawnAtPosition(Vector3 position, Quaternion rotation)
		{
			this._lastPosition = position;
			this.Position = position;
			this._lastRotation = rotation;
			this.Rotation = rotation;
		}

		// Token: 0x06005662 RID: 22114 RVA: 0x0019D1AA File Offset: 0x0019B3AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetScale(float scale)
		{
			this.Scale = scale;
			this.ScaleFactor = 1f / this.Scale;
		}

		// Token: 0x06005663 RID: 22115 RVA: 0x0019D1C8 File Offset: 0x0019B3C8
		public void LightUpdate()
		{
			Quaternion quaternion;
			bool flag;
			Vector3 vector;
			Vector3 vector2;
			Quaternion quaternion2;
			Quaternion quaternion3;
			this.ComputeSimulationParameters(out quaternion, out flag, out vector, out vector2, out quaternion2, out quaternion3);
			int num = 0;
			for (int i = this._particleBufferStartIndex; i < this._particleCount + this._particleBufferStartIndex; i++)
			{
				ref ParticleBuffers.ParticleLifeData ptr = ref this._particleBuffer.Life[i];
				ref ParticleBuffers.ParticleRenderData ptr2 = ref this._particleBuffer.Data1[i];
				ref Vector2 ptr3 = ref this._particleBuffer.Scale[i];
				bool flag2 = ptr.LifeSpanTimer <= 0f;
				if (!flag2)
				{
					bool flag3 = BitUtils.IsBitOn(2, ptr2.BoolData);
					Vector3 value = flag3 ? vector : vector2;
					Quaternion rotation = flag3 ? quaternion2 : quaternion3;
					ptr2.Velocity = Vector3.Transform(ptr2.Velocity, rotation);
					ptr2.Position = Vector3.Transform(ptr2.Position, rotation) + value;
					bool flag4 = ptr3 != Vector2.Zero;
					if (flag4)
					{
						num++;
					}
				}
			}
			this._particleDrawCount = num;
			this.UpdatePostSimulation(flag ? Quaternion.Identity : quaternion);
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x0019D300 File Offset: 0x0019B500
		public void Update(float deltaTime, int totalSteps)
		{
			float num = 0f;
			bool flag = this.ConsumeSpawnerLifeSpan(deltaTime, ref num);
			if (!flag)
			{
				Quaternion quaternion;
				bool flag2;
				Vector3 vector;
				Vector3 vector2;
				Quaternion quaternion2;
				Quaternion quaternion3;
				this.ComputeSimulationParameters(out quaternion, out flag2, out vector, out vector2, out quaternion2, out quaternion3);
				int num2 = 0;
				for (int i = this._particleBufferStartIndex; i < this._particleCount + this._particleBufferStartIndex; i++)
				{
					bool flag3 = this.TryToSpawnParticles(i, deltaTime, ref num);
					if (!flag3)
					{
						bool flag4 = this.UpdateParticleSimulation(i, deltaTime, totalSteps, ref vector, ref vector2, ref quaternion2, ref quaternion3, ref quaternion);
						bool flag5 = flag4;
						if (flag5)
						{
							num2++;
						}
					}
				}
				this._particleDrawCount = num2;
				this.UpdatePostSimulation(flag2 ? Quaternion.Identity : quaternion);
			}
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x0019D3B8 File Offset: 0x0019B5B8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool ConsumeSpawnerLifeSpan(float deltaTime, ref float emitParticles)
		{
			bool flag = this._hasWaves && this._waveTimer > 0f;
			bool result;
			if (flag)
			{
				this._waveTimer -= deltaTime;
				result = !this._spawnerSettings.SpawnBurst;
			}
			else
			{
				bool flag2 = this._lifeSpanTimer > 0f;
				if (flag2)
				{
					this._lifeSpanTimer -= deltaTime;
					bool flag3 = this._lifeSpanTimer <= 0f;
					if (flag3)
					{
						this.Expire(false);
					}
				}
				float num = this._spawnerSettings.SpawnBurst ? 1f : deltaTime;
				emitParticles = this._random.NextFloat(this._spawnerSettings.SpawnRate.X, this._spawnerSettings.SpawnRate.Y) * num + this._particleCrumbs;
				this._particleCrumbs = emitParticles % 1f;
				bool spawnBurst = this._spawnerSettings.SpawnBurst;
				if (spawnBurst)
				{
					this._particlesPerWave = (int)emitParticles;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x0019D4C0 File Offset: 0x0019B6C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool TryToSpawnParticles(int particleIndex, float deltaTime, ref float emitParticles)
		{
			ref ParticleBuffers.ParticleLifeData ptr = ref this._particleBuffer.Life[particleIndex];
			bool flag = !this.IsPaused && !this._waveEnded && this._waveTimer <= 0f && emitParticles >= 1f && this._particlesLeftToEmit != 0 && ptr.LifeSpanTimer <= 0f;
			if (flag)
			{
				bool flag2 = this.InitiateParticle(particleIndex);
				if (!flag2)
				{
					return true;
				}
				this._activeParticles++;
				this._particlesInWave++;
				bool flag3 = this._hasWaves && !this._waveEnded && this._particlesInWave == this._particlesPerWave;
				if (flag3)
				{
					bool spawnBurst = this._spawnerSettings.SpawnBurst;
					this._particlesInWave = 0;
					this._waveEnded = !spawnBurst;
					bool flag4 = spawnBurst;
					if (flag4)
					{
						this._waveTimer = this._random.NextFloat(this._spawnerSettings.WaveDelay.X, this._spawnerSettings.WaveDelay.Y);
					}
				}
				emitParticles -= 1f;
				bool flag5 = this._particlesLeftToEmit > 0;
				if (flag5)
				{
					this._particlesLeftToEmit--;
				}
			}
			else
			{
				bool flag6 = ptr.LifeSpanTimer <= 0f;
				if (flag6)
				{
					return true;
				}
				ptr.LifeSpanTimer -= deltaTime;
				bool flag7 = ptr.LifeSpanTimer <= 0f;
				if (flag7)
				{
					bool flag8 = this.ActiveParticles > 0;
					if (flag8)
					{
						this._activeParticles--;
					}
					bool flag9 = this._waveEnded && this.ActiveParticles == 0;
					if (flag9)
					{
						this._waveEnded = false;
						this._waveTimer = this._random.NextFloat(this._spawnerSettings.WaveDelay.X, this._spawnerSettings.WaveDelay.Y);
					}
					this._particleBuffer.Scale[particleIndex] = Vector2.Zero;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x0019D6E8 File Offset: 0x0019B8E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ComputeSimulationParameters(out Quaternion inverseRotation, out bool rotateWithSpawner, out Vector3 collisionPositionOffset, out Vector3 positionOffset, out Quaternion collisionRotationOffset, out Quaternion rotationOffset)
		{
			float scaleFactor = (this.IsFirstPerson == this._wasFirstPerson) ? this._spawnerSettings.TrailSpawnerPositionMultiplier : 0f;
			float amount = (this.IsFirstPerson == this._wasFirstPerson) ? this._spawnerSettings.TrailSpawnerRotationMultiplier : 0f;
			inverseRotation = Quaternion.Inverse(this.Rotation);
			rotateWithSpawner = this._spawnerSettings.ParticleRotateWithSpawner;
			collisionPositionOffset = Vector3.Transform(this._lastPosition - this.Position, inverseRotation);
			positionOffset = collisionPositionOffset * scaleFactor * this.ScaleFactor;
			collisionRotationOffset = inverseRotation * this._lastRotation;
			rotationOffset = Quaternion.Slerp(Quaternion.Identity, collisionRotationOffset, amount);
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x0019D7C8 File Offset: 0x0019B9C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool UpdateParticleSimulation(int particleIndex, float deltaTime, int totalSteps, ref Vector3 collisionPositionOffset, ref Vector3 positionOffset, ref Quaternion collisionRotationOffset, ref Quaternion rotationOffset, ref Quaternion inverseRotation)
		{
			ref ParticleBuffers.ParticleSimulationData ptr = ref this._particleBuffer.Data0[particleIndex];
			ref ParticleBuffers.ParticleRenderData ptr2 = ref this._particleBuffer.Data1[particleIndex];
			ref Vector2 ptr3 = ref this._particleBuffer.Scale[particleIndex];
			bool flag = BitUtils.IsBitOn(2, ptr2.BoolData);
			Vector3 value = flag ? collisionPositionOffset : positionOffset;
			Quaternion rotation = flag ? collisionRotationOffset : rotationOffset;
			ptr2.Velocity = Vector3.Transform(ptr2.Velocity, rotation);
			ptr2.Position = Vector3.Transform(ptr2.Position, rotation) + value;
			Vector3 position = ptr2.Position;
			bool flag2 = !flag;
			if (flag2)
			{
				for (int i = 0; i < totalSteps; i++)
				{
					ptr2.AttractorVelocity = Vector3.Zero;
					for (int j = 0; j < this._spawnerSettings.Attractors.Length; j++)
					{
						this._spawnerSettings.Attractors[j].Apply(ptr2.Position, ptr.SpawnerPositionAtSpawn - this.Position, ref ptr2.Velocity, ref ptr2.AttractorVelocity);
					}
					ptr2.Position += ptr2.Velocity + ptr2.AttractorVelocity;
				}
				bool flag3 = !this._spawnerSettings.ParticleRotateWithSpawner;
				if (flag3)
				{
					ptr.RotationOffset = rotationOffset * ptr.RotationOffset;
				}
				bool flag4 = this._spawnerSettings.ParticleCollisionBlockType > ParticleFXSystem.ParticleCollisionBlockType.None;
				if (flag4)
				{
					ref ParticleBuffers.ParticleLifeData particleLife = ref this._particleBuffer.Life[particleIndex];
					this._updateCollisionFunc(this, ref ptr, ref ptr2, ref ptr3, ref particleLife, position, inverseRotation);
				}
			}
			this.UpdateParticleAnimation(particleIndex, deltaTime);
			return ptr3 != Vector2.Zero;
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x0019D9C5 File Offset: 0x0019BBC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdatePostSimulation(Quaternion inverseRotation)
		{
			this._drawData.InverseRotation = inverseRotation;
			this._lastPosition = this.Position;
			this._lastRotation = this.Rotation;
			this._wasFirstPerson = this.IsFirstPerson;
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x0019D9F8 File Offset: 0x0019BBF8
		public void UpdateLife(float deltaTime)
		{
			float num = 0f;
			bool flag = this.ConsumeSpawnerLifeSpan(deltaTime, ref num);
			if (!flag)
			{
				for (int i = this._particleBufferStartIndex; i < this._particleCount + this._particleBufferStartIndex; i++)
				{
					this.TryToSpawnParticles(i, deltaTime, ref num);
				}
			}
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x0019DA4C File Offset: 0x0019BC4C
		public void UpdateSimulation(float deltaTime, int totalSteps)
		{
			Quaternion quaternion;
			bool flag;
			Vector3 vector;
			Vector3 vector2;
			Quaternion quaternion2;
			Quaternion quaternion3;
			this.ComputeSimulationParameters(out quaternion, out flag, out vector, out vector2, out quaternion2, out quaternion3);
			int num = 0;
			for (int i = this._particleBufferStartIndex; i < this._particleCount + this._particleBufferStartIndex; i++)
			{
				ref ParticleBuffers.ParticleLifeData ptr = ref this._particleBuffer.Life[i];
				bool flag2 = ptr.LifeSpanTimer <= 0f;
				if (!flag2)
				{
					bool flag3 = this.UpdateParticleSimulation(i, deltaTime, totalSteps, ref vector, ref vector2, ref quaternion2, ref quaternion3, ref quaternion);
					bool flag4 = flag3;
					if (flag4)
					{
						num++;
					}
				}
			}
			this._particleDrawCount = num;
			this.UpdatePostSimulation(flag ? Quaternion.Identity : quaternion);
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x0019DB04 File Offset: 0x0019BD04
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateLight(Vector4 staticLightColor)
		{
			staticLightColor.W = this._spawnerSettings.LightInfluence;
			this._drawData.StaticLightColorAndInfluence = staticLightColor;
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x0019DB28 File Offset: 0x0019BD28
		private bool InitiateParticle(int particleIndex)
		{
			ref ParticleBuffers.ParticleSimulationData ptr = ref this._particleBuffer.Data0[particleIndex];
			ref ParticleBuffers.ParticleRenderData ptr2 = ref this._particleBuffer.Data1[particleIndex];
			ref Vector2 ptr3 = ref this._particleBuffer.Scale[particleIndex];
			ref ParticleBuffers.ParticleLifeData ptr4 = ref this._particleBuffer.Life[particleIndex];
			float x = this._random.NextFloat(this._spawnerSettings.EmitOffsetMin.X, this._spawnerSettings.EmitOffsetMax.X);
			float y = this._random.NextFloat(this._spawnerSettings.EmitOffsetMin.Y, this._spawnerSettings.EmitOffsetMax.Y);
			float z = this._random.NextFloat(this._spawnerSettings.EmitOffsetMin.Z, this._spawnerSettings.EmitOffsetMax.Z);
			Vector3 emitPosition = this.GetEmitPosition(this._spawnerSettings.EmitShape, new Vector3(x, y, z));
			bool flag = !this._initParticleFunc(this, ref emitPosition);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ptr3 = Vector2.Zero;
				ptr4.LifeSpan = this._random.NextFloat(this._spawnerSettings.ParticleLifeSpan.X, this._spawnerSettings.ParticleLifeSpan.Y);
				ptr4.LifeSpanTimer = ptr4.LifeSpan;
				ptr.SpawnerPositionAtSpawn = this.Position;
				ptr.RotationAnimationIndex = 0;
				ptr.RotationNextKeyframeTime = 0f;
				ptr.RotationStep = Vector3.Zero;
				ptr.RotationOffset = Quaternion.Identity;
				ptr.CurrentRotation = Vector3.Zero;
				ptr.ScaleAnimationIndex = 0;
				ptr.ScaleNextKeyframeTime = 0f;
				ptr.ScaleStep = Vector2.Zero;
				ptr.TextureAnimationIndex = 0;
				ptr.TextureNextKeyframeTime = 0f;
				ptr2.TargetTextureIndex = 0;
				ptr2.Seed = (ushort)this._random.Next();
				bool flag2 = this._particleSettings.TextureIndexKeyFrames != null;
				if (flag2)
				{
					float num = 0.01f * ptr4.LifeSpan;
					ref ParticleSettings.RangeKeyframe ptr5 = ref this._particleSettings.TextureIndexKeyFrames[0];
					float num2 = (float)ptr5.Time * num;
					ptr2.TargetTextureIndex = (byte)this._random.Next((int)ptr5.Min, (int)(ptr5.Max + 1));
					ptr.TextureNextKeyframeTime = ((this._particleSettings.TextureIndexKeyFrames.Length > 1) ? num2 : ptr4.LifeSpan);
				}
				ptr2.PrevTargetTextureIndex = ptr2.TargetTextureIndex;
				ptr2.TargetTextureBlendProgress = 0;
				ptr2.Position = emitPosition;
				ptr2.Rotation = ParticleSettings.DefaultRotation;
				ptr2.BoolData = 0;
				bool flag3 = false;
				bool flag4 = false;
				switch (this._particleSettings.UVOption)
				{
				case ParticleSettings.UVOptions.RandomFlipU:
					flag3 = (this._random.NextDouble() < 0.5);
					break;
				case ParticleSettings.UVOptions.RandomFlipV:
					flag4 = (this._random.NextDouble() < 0.5);
					break;
				case ParticleSettings.UVOptions.RandomFlipUV:
					flag3 = (this._random.NextDouble() < 0.5);
					flag4 = flag3;
					break;
				case ParticleSettings.UVOptions.FlipU:
					flag3 = true;
					break;
				case ParticleSettings.UVOptions.FlipV:
					flag4 = true;
					break;
				case ParticleSettings.UVOptions.FlipUV:
					flag3 = true;
					flag4 = true;
					break;
				}
				bool flag5 = flag3;
				if (flag5)
				{
					BitUtils.SwitchOnBit(0, ref ptr2.BoolData);
				}
				bool flag6 = flag4;
				if (flag6)
				{
					BitUtils.SwitchOnBit(1, ref ptr2.BoolData);
				}
				bool useEmitDirection = this._spawnerSettings.UseEmitDirection;
				Quaternion rotation;
				if (useEmitDirection)
				{
					Vector3 vector = emitPosition;
					bool flag7 = vector == Vector3.Zero;
					if (flag7)
					{
						vector.X = this._random.NextFloat(-1f, 1f);
						vector.Y = this._random.NextFloat(-1f, 1f);
						vector.Z = this._random.NextFloat(-1f, 1f);
						vector.Normalize();
					}
					Vector3 emitRotationVector = this.GetEmitRotationVector(this._spawnerSettings.EmitShape, vector);
					rotation = Quaternion.CreateFromVectors(Vector3.Forward, emitRotationVector);
				}
				else
				{
					float yaw = this._random.NextFloat(this._spawnerSettings.InitialVelocityMin.Yaw, this._spawnerSettings.InitialVelocityMax.Yaw);
					float pitch = this._random.NextFloat(this._spawnerSettings.InitialVelocityMin.Pitch, this._spawnerSettings.InitialVelocityMax.Pitch);
					rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0f);
				}
				float scaleFactor = this._random.NextFloat(this._spawnerSettings.InitialVelocityMin.Speed, this._spawnerSettings.InitialVelocityMax.Speed);
				ptr2.Velocity = Vector3.Transform(Vector3.Forward * scaleFactor, rotation);
				result = true;
			}
			return result;
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x0019DFF8 File Offset: 0x0019C1F8
		private Vector3 GetEmitPosition(ParticleSpawnerSettings.Shape emitShape, Vector3 position)
		{
			Vector3 result = Vector3.Zero;
			switch (emitShape)
			{
			case ParticleSpawnerSettings.Shape.Sphere:
			{
				float num = (float)this._random.NextDouble() * 6.2831855f;
				float num2 = (float)this._random.NextDouble() * 6.2831855f;
				result = new Vector3(position.X * (float)Math.Cos((double)num) * (float)Math.Cos((double)num2), position.Y * (float)Math.Sin((double)num) * (float)Math.Cos((double)num2), position.Z * (float)Math.Sin((double)num2));
				break;
			}
			case ParticleSpawnerSettings.Shape.Cube:
			{
				bool flag = position.Z == 0f;
				int num3;
				if (flag)
				{
					num3 = 0;
				}
				else
				{
					bool flag2 = position.X == 0f;
					if (flag2)
					{
						num3 = 1;
					}
					else
					{
						bool flag3 = position.Y == 0f;
						if (flag3)
						{
							num3 = 2;
						}
						else
						{
							num3 = this._random.Next(2);
						}
					}
				}
				float num4 = 0f;
				float num5 = 0f;
				float yaw = 0f;
				float pitch = 0f;
				bool flag4 = num3 == 0;
				if (flag4)
				{
					num4 = position.X * 2f;
					num5 = position.Y * 2f;
					position.Z = this._random.NextFloat(0f, position.Z * 2f) - position.Z;
				}
				else
				{
					bool flag5 = num3 == 1;
					if (flag5)
					{
						num4 = position.Z * 2f;
						num5 = position.Y * 2f;
						position.Z = this._random.NextFloat(0f, position.X * 2f) - position.X;
						yaw = 1.5707964f;
					}
					else
					{
						bool flag6 = num3 == 2;
						if (flag6)
						{
							num4 = position.X * 2f;
							num5 = position.Z * 2f;
							position.Z = this._random.NextFloat(0f, position.Y * 2f) - position.Y;
							pitch = 1.5707964f;
						}
					}
				}
				float num6 = this._random.NextFloat(0f, num4 * 2f + num5 * 2f);
				bool flag7 = num6 < num4;
				if (flag7)
				{
					position.X = num6 - num4 * 0.5f;
					position.Y = -num5 * 0.5f;
				}
				else
				{
					bool flag8 = num6 < num4 * 2f;
					if (flag8)
					{
						position.X = num6 - num4 - num4 * 0.5f;
						position.Y = num5 * 0.5f;
					}
					else
					{
						bool flag9 = num6 < num4 * 2f + num5;
						if (flag9)
						{
							position.X = -num4 * 0.5f;
							position.Y = num6 - num4 * 2f - num5 * 0.5f;
						}
						else
						{
							position.X = num4 * 0.5f;
							position.Y = num6 - (num4 * 2f + num5) - num5 * 0.5f;
						}
					}
				}
				result = Vector3.Transform(position, Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0f));
				break;
			}
			case ParticleSpawnerSettings.Shape.Circle:
			{
				float num7 = (float)this._random.NextDouble() * 6.2831855f;
				result.X = ((position.X != 0f) ? (position.X * (float)Math.Cos((double)num7)) : 0f);
				bool flag10 = position.Y != 0f;
				if (flag10)
				{
					result.Y = ((position.X == 0f) ? (position.Y * (float)Math.Cos((double)num7)) : (position.Y * (float)Math.Sin((double)num7)));
				}
				else
				{
					result.Y = 0f;
				}
				result.Z = ((position.Z != 0f) ? (position.Z * (float)Math.Sin((double)num7)) : 0f);
				break;
			}
			case ParticleSpawnerSettings.Shape.FullCube:
				position.X = position.X * 2f - this._spawnerSettings.EmitOffsetMax.X;
				position.Y = position.Y * 2f - this._spawnerSettings.EmitOffsetMax.Y;
				position.Z = position.Z * 2f - this._spawnerSettings.EmitOffsetMax.Z;
				result = position;
				break;
			}
			return result;
		}

		// Token: 0x0600566F RID: 22127 RVA: 0x0019E47C File Offset: 0x0019C67C
		private Vector3 GetEmitRotationVector(ParticleSpawnerSettings.Shape emitShape, Vector3 spawnPosition)
		{
			return Vector3.Normalize(spawnPosition);
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x0019E494 File Offset: 0x0019C694
		public void ReserveVertexDataStorage(ref FXVertexBuffer vertexBuffer, ushort drawId)
		{
			this._drawId = drawId;
			this._particleVertexDataStartIndex = vertexBuffer.ReserveVertexDataStorage(this._particleDrawCount);
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x0019E4B0 File Offset: 0x0019C6B0
		public unsafe void PrepareForDraw(Vector3 cameraPosition, ref FXVertexBuffer vertexBuffer, IntPtr gpuDrawDataPtr)
		{
			Debug.Assert(this.ActiveParticles > 0, "No need to call PrepareForDraw() on a ParticleSpawner that has 0 particles active.");
			int num = 0;
			uint num2 = (this._spawnerSettings.LinearFiltering ? 1U : 0U) << FXVertex.ConfigBitShiftLinearFiltering;
			num2 |= (this._useSoftParticles ? 1U : 0U) << FXVertex.ConfigBitShiftSoftParticles;
			num2 |= (uint)((uint)this._spawnerSettings.RenderMode << (FXVertex.ConfigBitShiftBlendMode & 31));
			num2 |= (this.IsFirstPerson ? 1U : 0U) << FXVertex.ConfigBitShiftIsFirstPerson;
			num2 |= (uint)((uint)this._drawId << FXVertex.ConfigBitShiftDrawId);
			float scaleFactor = (this._spawnerSettings.RotationInfluence == ParticleFXSystem.ParticleRotationInfluence.Billboard || this._spawnerSettings.RotationInfluence == ParticleFXSystem.ParticleRotationInfluence.BillboardY || this._spawnerSettings.RotationInfluence == ParticleFXSystem.ParticleRotationInfluence.BillboardVelocity) ? this.Scale : 1f;
			for (int i = this._particleBufferStartIndex; i < this._particleCount + this._particleBufferStartIndex; i++)
			{
				ref ParticleBuffers.ParticleRenderData ptr = ref this._particleBuffer.Data1[i];
				ref Vector2 ptr2 = ref this._particleBuffer.Scale[i];
				ref ParticleBuffers.ParticleLifeData ptr3 = ref this._particleBuffer.Life[i];
				bool flag = ptr2 == Vector2.Zero;
				if (!flag)
				{
					bool flag2 = BitUtils.IsBitOn(0, ptr.BoolData);
					bool flag3 = BitUtils.IsBitOn(1, ptr.BoolData);
					bool flag4 = BitUtils.IsBitOn(2, ptr.BoolData);
					int particleIndex = num + this._particleVertexDataStartIndex;
					vertexBuffer.SetParticleVertexDataPositionAndScale(particleIndex, ptr.Position, ptr2 * scaleFactor);
					Vector3 vector = ptr.Velocity + ptr.AttractorVelocity;
					bool flag5 = this._spawnerSettings.RotationInfluence == ParticleFXSystem.ParticleRotationInfluence.BillboardVelocity;
					if (flag5)
					{
						vector = Vector3.Transform(vector, this.Rotation);
					}
					vertexBuffer.SetParticleVertexDataVelocityAndRotation(particleIndex, vector, new Vector4(ptr.Rotation.X, ptr.Rotation.Y, ptr.Rotation.Z, ptr.Rotation.W));
					uint textureInfo = (uint)((int)ptr.TargetTextureBlendProgress << 16 | (int)ptr.PrevTargetTextureIndex << 8 | (int)ptr.TargetTextureIndex);
					vertexBuffer.SetParticleVertexDataTextureInfo(particleIndex, textureInfo);
					vertexBuffer.SetParticleVertexDataColor(particleIndex, ptr.Color);
					ParticleFXSystem.ParticleRotationInfluence particleRotationInfluence = (!flag4) ? this._spawnerSettings.RotationInfluence : this._spawnerSettings.ParticleCollisionRotationInfluence;
					uint num3 = num2 | (uint)((uint)particleRotationInfluence << (FXVertex.ConfigBitShiftQuadType & 31));
					num3 |= (flag2 ? 1U : 0U) << FXVertex.ConfigBitShiftInvertUTexture;
					num3 |= (flag3 ? 1U : 0U) << FXVertex.ConfigBitShiftInvertVTexture;
					vertexBuffer.SetVertexDataConfig(particleIndex, num3);
					ushort seed = ptr.Seed;
					float num4 = ptr3.LifeSpan - ptr3.LifeSpanTimer;
					ushort num5 = (ushort)(num4 / ptr3.LifeSpan * 65535f);
					uint seedAndLifeRatio = (uint)((int)seed << 16 | (int)num5);
					vertexBuffer.SetParticleVertexDataSeedAndLifeRatio(particleIndex, seedAndLifeRatio);
					num++;
				}
			}
			bool flag6 = num != this._particleDrawCount;
			if (flag6)
			{
				ParticleSpawner.Logger.Info(string.Format("Unexpected divergence between visibleParticleId & _particleDrawCount : {0} vs {1}.", num, this._particleDrawCount));
			}
			Vector3 vector2 = (!this.IsFirstPerson) ? (this.Position - cameraPosition) : this.Position;
			Matrix matrix;
			Matrix.CreateScale(this.Scale, out matrix);
			Matrix matrix2;
			Matrix.CreateFromQuaternion(ref this.Rotation, out matrix2);
			Matrix.Multiply(ref matrix, ref matrix2, out matrix);
			Matrix.CreateTranslation(ref vector2, out matrix2);
			Matrix matrix3;
			Matrix.Multiply(ref matrix, ref matrix2, out matrix3);
			IntPtr pointer = IntPtr.Add(gpuDrawDataPtr, (int)this._drawId * FXRenderer.DrawDataSize);
			Matrix* ptr4 = (Matrix*)pointer.ToPointer();
			*ptr4 = matrix3;
			Vector4* ptr5 = (Vector4*)IntPtr.Add(pointer, sizeof(Matrix)).ToPointer();
			*ptr5 = this._drawData.StaticLightColorAndInfluence;
			ptr5[1] = new Vector4(this._drawData.InverseRotation.X, this._drawData.InverseRotation.Y, this._drawData.InverseRotation.Z, this._drawData.InverseRotation.W);
			ptr5[2] = new Vector4((float)this._drawData.ImageLocation.X, (float)this._drawData.ImageLocation.Y, (float)this._drawData.ImageLocation.Width, (float)this._drawData.ImageLocation.Height);
			ptr5[3] = new Vector4((float)this._drawData.FrameSize.X, (float)this._drawData.FrameSize.Y, this._drawData.UVMotionTextureId, 0f);
			ptr5[4] = this._drawData.UVMotion;
			ptr5[5] = this._drawData.IntersectionHighlight;
			ptr5[6] = new Vector4(this._drawData.CameraOffset, this._drawData.VelocityStretchMultiplier, this._drawData.SoftParticlesFadeFactor, this._drawData.AddRandomUVOffset);
			ptr5[7] = new Vector4((float)this._drawData.StrengthCurveType, 0f, 0f, 0f);
		}

		// Token: 0x04003396 RID: 13206
		public const byte CollisionFrameTime = 101;

		// Token: 0x04003397 RID: 13207
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003398 RID: 13208
		public Vector3 Position;

		// Token: 0x04003399 RID: 13209
		public Quaternion Rotation = Quaternion.Identity;

		// Token: 0x0400339C RID: 13212
		public UInt32Color DefaultColor;

		// Token: 0x0400339D RID: 13213
		public bool IsOvergroundOnly = false;

		// Token: 0x0400339E RID: 13214
		public bool IsFirstPerson = false;

		// Token: 0x0400339F RID: 13215
		public bool IsPaused = false;

		// Token: 0x040033A0 RID: 13216
		private readonly ParticleSpawnerSettings _spawnerSettings;

		// Token: 0x040033A1 RID: 13217
		private readonly ParticleSettings _particleSettings;

		// Token: 0x040033A2 RID: 13218
		private readonly ParticleBuffers _particleBuffer;

		// Token: 0x040033A3 RID: 13219
		private ParticleFXSystem _particleFXSystem;

		// Token: 0x040033A4 RID: 13220
		private UpdateParticleCollisionFunc _updateCollisionFunc;

		// Token: 0x040033A5 RID: 13221
		private InitParticleFunc _initParticleFunc;

		// Token: 0x040033A6 RID: 13222
		private ParticleSpawner.DrawData _drawData;

		// Token: 0x040033A7 RID: 13223
		private int _particleBufferStartIndex;

		// Token: 0x040033A8 RID: 13224
		private int _particleCount;

		// Token: 0x040033A9 RID: 13225
		private ushort _drawId;

		// Token: 0x040033AA RID: 13226
		private int _particleVertexDataStartIndex;

		// Token: 0x040033AB RID: 13227
		private int _particleDrawCount;

		// Token: 0x040033AC RID: 13228
		private Vector3 _lastPosition;

		// Token: 0x040033AD RID: 13229
		private Quaternion _lastRotation;

		// Token: 0x040033AE RID: 13230
		private Random _random;

		// Token: 0x040033AF RID: 13231
		private Vector2 _textureAltasInverseSize;

		// Token: 0x040033B0 RID: 13232
		private bool _useSpriteBlending = false;

		// Token: 0x040033B1 RID: 13233
		private bool _useSoftParticles = false;

		// Token: 0x040033B2 RID: 13234
		private int _particlesLeftToEmit;

		// Token: 0x040033B3 RID: 13235
		private int _activeParticles = 0;

		// Token: 0x040033B4 RID: 13236
		private float _particleCrumbs = 0f;

		// Token: 0x040033B5 RID: 13237
		private float _lifeSpanTimer;

		// Token: 0x040033B6 RID: 13238
		private int _particlesPerWave;

		// Token: 0x040033B7 RID: 13239
		private int _particlesInWave;

		// Token: 0x040033B8 RID: 13240
		private float _waveTimer;

		// Token: 0x040033B9 RID: 13241
		private readonly bool _hasWaves = false;

		// Token: 0x040033BA RID: 13242
		private bool _waveEnded = false;

		// Token: 0x040033BB RID: 13243
		private bool _wasFirstPerson = false;

		// Token: 0x02000F01 RID: 3841
		private struct DrawData
		{
			// Token: 0x04004989 RID: 18825
			public Vector4 StaticLightColorAndInfluence;

			// Token: 0x0400498A RID: 18826
			public Quaternion InverseRotation;

			// Token: 0x0400498B RID: 18827
			public Rectangle ImageLocation;

			// Token: 0x0400498C RID: 18828
			public UShortVector2 FrameSize;

			// Token: 0x0400498D RID: 18829
			public Vector4 UVMotion;

			// Token: 0x0400498E RID: 18830
			public float UVMotionTextureId;

			// Token: 0x0400498F RID: 18831
			public float AddRandomUVOffset;

			// Token: 0x04004990 RID: 18832
			public int StrengthCurveType;

			// Token: 0x04004991 RID: 18833
			public Vector4 IntersectionHighlight;

			// Token: 0x04004992 RID: 18834
			public float CameraOffset;

			// Token: 0x04004993 RID: 18835
			public float VelocityStretchMultiplier;

			// Token: 0x04004994 RID: 18836
			public float SoftParticlesFadeFactor;
		}
	}
}
