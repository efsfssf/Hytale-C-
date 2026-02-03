using System;
using HytaleClient.Math;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000ABA RID: 2746
	internal class ParticleSystem
	{
		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06005681 RID: 22145 RVA: 0x0019EB22 File Offset: 0x0019CD22
		public float CullDistanceSquared
		{
			get
			{
				return this._particleSystemSettings.CullDistanceSquared;
			}
		}

		// Token: 0x17001334 RID: 4916
		// (get) Token: 0x06005682 RID: 22146 RVA: 0x0019EB2F File Offset: 0x0019CD2F
		public float BoundingRadius
		{
			get
			{
				return this._particleSystemSettings.BoundingRadius;
			}
		}

		// Token: 0x17001335 RID: 4917
		// (get) Token: 0x06005683 RID: 22147 RVA: 0x0019EB3C File Offset: 0x0019CD3C
		// (set) Token: 0x06005684 RID: 22148 RVA: 0x0019EB44 File Offset: 0x0019CD44
		public int Id { get; private set; }

		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x06005685 RID: 22149 RVA: 0x0019EB4D File Offset: 0x0019CD4D
		// (set) Token: 0x06005686 RID: 22150 RVA: 0x0019EB55 File Offset: 0x0019CD55
		public bool IsExpiring { get; private set; } = false;

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x06005687 RID: 22151 RVA: 0x0019EB5E File Offset: 0x0019CD5E
		// (set) Token: 0x06005688 RID: 22152 RVA: 0x0019EB66 File Offset: 0x0019CD66
		public bool IsExpired { get; private set; } = false;

		// Token: 0x17001338 RID: 4920
		// (get) Token: 0x06005689 RID: 22153 RVA: 0x0019EB6F File Offset: 0x0019CD6F
		// (set) Token: 0x0600568A RID: 22154 RVA: 0x0019EB77 File Offset: 0x0019CD77
		public bool IsPaused { get; private set; } = false;

		// Token: 0x17001339 RID: 4921
		// (get) Token: 0x0600568B RID: 22155 RVA: 0x0019EB80 File Offset: 0x0019CD80
		// (set) Token: 0x0600568C RID: 22156 RVA: 0x0019EB88 File Offset: 0x0019CD88
		public bool IsFirstPerson { get; private set; } = false;

		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x0600568D RID: 22157 RVA: 0x0019EB91 File Offset: 0x0019CD91
		public bool IsImportant
		{
			get
			{
				return this._particleSystemSettings.IsImportant;
			}
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x0019EBA0 File Offset: 0x0019CDA0
		public ParticleSystem(ParticleFXSystem particleFXSystem, bool isGPULowEnd, UpdateSpawnerLightingFunc updateLightingFunc, UpdateParticleCollisionFunc updateCollisionFunc, InitParticleFunc initParticleFunc, Vector2 textureAltasInverseSize, int id, ParticleSystemSettings particleSystemSettings)
		{
			this.Id = id;
			this._particleFXSystem = particleFXSystem;
			this._isGPULowEnd = isGPULowEnd;
			this._updateLightingFunc = updateLightingFunc;
			this._updateCollisionFunc = updateCollisionFunc;
			this._initParticleFunc = initParticleFunc;
			this._textureAltasInverseSize = textureAltasInverseSize;
			this._random = new Random(id);
			this._particleSystemSettings = particleSystemSettings;
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x0019EC5D File Offset: 0x0019CE5D
		public void Dispose()
		{
			this.Release();
			this._initParticleFunc = null;
			this._updateCollisionFunc = null;
			this._updateLightingFunc = null;
			this._particleFXSystem = null;
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x0019EC84 File Offset: 0x0019CE84
		public bool Initialize()
		{
			this._lifeSpanTimer = this._particleSystemSettings.LifeSpan;
			this._hasUniqueSpawners = true;
			int num = 0;
			int num2 = 0;
			this.SpawnerGroups = new ParticleSystem.SystemSpawnerGroup[(int)this._particleSystemSettings.SystemSpawnerCount];
			for (int i = 0; i < (int)this._particleSystemSettings.SystemSpawnerCount; i++)
			{
				this.SpawnerGroups[i].Id = i;
				this.SpawnerGroups[i].Settings = this._particleSystemSettings.SystemSpawnerSettingsList[i];
				this.SpawnerGroups[i].StartTimer = this.SpawnerGroups[i].Settings.StartDelay;
				this.SpawnerGroups[i].SpawnersLeft = this.SpawnerGroups[i].Settings.TotalSpawners;
				this.SpawnerGroups[i].IsSingleSpawner = (this.SpawnerGroups[i].SpawnersLeft == 1);
				this.SpawnerGroups[i].HasWaves = (this.SpawnerGroups[i].Settings.WaveDelay != Vector2.Zero);
				this._hasUniqueSpawners = (this._hasUniqueSpawners && this.SpawnerGroups[i].IsSingleSpawner);
				this.SpawnerGroups[i].SpawnerIdStart = num;
				this.SpawnerGroups[i].SpawnerParticlesIdStart = num2;
				int maxConcurrent = this.SpawnerGroups[i].Settings.MaxConcurrent;
				num += maxConcurrent;
				num2 += maxConcurrent * this.SpawnerGroups[i].Settings.ParticleSpawnerSettings.MaxConcurrentParticles;
			}
			this.SystemSpawners = new ParticleSystem.SystemSpawner[num];
			this.AliveSpawnerCount = 0;
			this._maxParticles = num2;
			this._particleBufferStartIndex = this._particleFXSystem.RequestParticleBufferStorage(this._maxParticles);
			return this._particleBufferStartIndex >= 0 && this._particleBufferStartIndex < this._particleFXSystem.ParticleBufferStorageMaxCount;
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x0019EE98 File Offset: 0x0019D098
		public void Release()
		{
			this.Proxy = null;
			for (int i = 0; i < this.AliveSpawnerCount; i++)
			{
				this.SystemSpawners[i].ParticleSpawner.Dispose();
				this.SystemSpawners[i].ParticleSpawner = null;
			}
			this.AliveSpawnerCount = 0;
			bool flag = this._maxParticles > 0;
			if (flag)
			{
				this._particleFXSystem.ReleaseParticleBufferStorage(this._particleBufferStartIndex, this._maxParticles);
			}
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x0019EF1C File Offset: 0x0019D11C
		public void Expire(bool instant = false)
		{
			for (int i = 0; i < this.AliveSpawnerCount; i++)
			{
				this.SystemSpawners[i].ParticleSpawner.Expire(instant);
			}
			this.IsExpiring = true;
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x0019EF64 File Offset: 0x0019D164
		public void Pause(bool pause = true)
		{
			for (int i = 0; i < this.AliveSpawnerCount; i++)
			{
				ref ParticleSystem.SystemSpawner ptr = ref this.SystemSpawners[i];
				ptr.ParticleSpawner.IsPaused = pause;
				this.UpdateSpawnerPositionAndRotation(i);
			}
			this.IsPaused = pause;
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x0019EFB4 File Offset: 0x0019D1B4
		public void LightUpdate()
		{
			for (int i = 0; i < this.AliveSpawnerCount; i++)
			{
				ref ParticleSystem.SystemSpawner ptr = ref this.SystemSpawners[i];
				this.UpdateSpawnerPositionAndRotation(i);
				ptr.ParticleSpawner.LightUpdate();
			}
		}

		// Token: 0x06005695 RID: 22165 RVA: 0x0019EFFC File Offset: 0x0019D1FC
		public void Update(float deltaTime)
		{
			this.ConsumeSystemLifeSpan(deltaTime);
			this.TryToCreateSpawners(deltaTime);
			int totalSteps = this.UpdateAttractorSteps(deltaTime);
			bool flag;
			for (int i = 0; i < this.AliveSpawnerCount; i += (flag ? 0 : 1))
			{
				ParticleSpawner particleSpawner = this.SystemSpawners[i].ParticleSpawner;
				this.UpdateSpawnerSimulation(i, totalSteps);
				particleSpawner.Update(deltaTime, totalSteps);
				this._updateLightingFunc(particleSpawner);
				flag = this.CheckSpawnerDeath(i, deltaTime);
			}
		}

		// Token: 0x06005696 RID: 22166 RVA: 0x0019F07C File Offset: 0x0019D27C
		private void ConsumeSystemLifeSpan(float deltaTime)
		{
			bool flag = this._lifeSpanTimer > 0f;
			if (flag)
			{
				this._lifeSpanTimer -= deltaTime;
				bool flag2 = this._lifeSpanTimer <= 0f;
				if (flag2)
				{
					this.Expire(false);
				}
			}
		}

		// Token: 0x06005697 RID: 22167 RVA: 0x0019F0C8 File Offset: 0x0019D2C8
		private void TryToCreateSpawners(float deltaTime)
		{
			this._isWaitingForSpawners = false;
			bool flag = !this.IsExpiring && !this.IsPaused;
			if (flag)
			{
				for (int i = 0; i < this.SpawnerGroups.Length; i++)
				{
					ref ParticleSystem.SystemSpawnerGroup ptr = ref this.SpawnerGroups[i];
					bool flag2 = ptr.StartTimer > 0f;
					if (flag2)
					{
						ptr.StartTimer -= deltaTime;
						this._isWaitingForSpawners = true;
					}
					else
					{
						bool flag3 = ptr.SpawnersLeft == 0 || ptr.ActiveSpawners >= ptr.Settings.MaxConcurrent || ptr.WaveEnded;
						if (!flag3)
						{
							bool isSingleSpawner = ptr.IsSingleSpawner;
							if (isSingleSpawner)
							{
								int systemSpawnerId;
								int num;
								this.TakeStorageSlot(ref ptr, out systemSpawnerId, out num);
								this.InitializeSpawner(ref ptr, systemSpawnerId, this._particleBufferStartIndex + num);
								ptr.SpawnersLeft = 0;
							}
							else
							{
								bool flag4 = ptr.HasWaves && ptr.WaveTimer > 0f;
								if (flag4)
								{
									ptr.WaveTimer -= deltaTime;
								}
								else
								{
									float num2 = this._random.NextFloat(ptr.Settings.SpawnRate.X, ptr.Settings.SpawnRate.Y) * deltaTime + ptr.SpawnerCrumbs;
									ptr.SpawnerCrumbs = num2 % 1f;
									bool flag5 = num2 >= 1f;
									if (flag5)
									{
										int systemSpawnerId2;
										int num3;
										this.TakeStorageSlot(ref ptr, out systemSpawnerId2, out num3);
										this.InitializeSpawner(ref ptr, systemSpawnerId2, this._particleBufferStartIndex + num3);
										ptr.ActiveSpawners++;
										bool flag6 = ptr.SpawnersLeft > 0;
										if (flag6)
										{
											ptr.SpawnersLeft--;
										}
										bool flag7 = ptr.HasWaves && !ptr.WaveEnded && ptr.ActiveSpawners == ptr.Settings.MaxConcurrent;
										if (flag7)
										{
											ptr.WaveEnded = true;
										}
									}
								}
							}
						}
					}
				}
			}
			this.IsExpired = ((this._hasUniqueSpawners && !this._isWaitingForSpawners) || this.IsExpiring);
		}

		// Token: 0x06005698 RID: 22168 RVA: 0x0019F2D8 File Offset: 0x0019D4D8
		private int UpdateAttractorSteps(float deltaTime)
		{
			this._attractorStep += deltaTime;
			int num = 0;
			while (this._attractorStep >= 0.016666668f)
			{
				num++;
				this._attractorStep -= 0.016666668f;
			}
			return num;
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x0019F32C File Offset: 0x0019D52C
		private bool CheckSpawnerDeath(int spawnerIndex, float deltaTime)
		{
			ref ParticleSystem.SystemSpawner ptr = ref this.SystemSpawners[spawnerIndex];
			bool result = false;
			bool flag = ptr.LifeSpanTimer > 0f;
			if (flag)
			{
				ptr.LifeSpanTimer -= deltaTime;
				bool flag2 = ptr.LifeSpanTimer <= 0f;
				if (flag2)
				{
					ptr.ParticleSpawner.Expire(false);
				}
			}
			bool flag3 = ptr.ParticleSpawner.IsExpired();
			if (flag3)
			{
				result = true;
				ref ParticleSystem.SystemSpawnerGroup ptr2 = ref this.SpawnerGroups[ptr.GroupId];
				this.FreeStorageSlot(ref ptr2, ptr.Id);
				bool flag4 = ptr2.ActiveSpawners > 0;
				if (flag4)
				{
					ptr2.ActiveSpawners--;
				}
				bool flag5 = ptr2.WaveEnded && ptr2.ActiveSpawners == 0;
				if (flag5)
				{
					ptr2.WaveEnded = false;
					ptr2.WaveTimer = this._random.NextFloat(ptr2.Settings.WaveDelay.X, ptr2.Settings.WaveDelay.Y);
				}
				ptr.ParticleSpawner.Dispose();
				this.SystemSpawners[spawnerIndex] = this.SystemSpawners[this.AliveSpawnerCount - 1];
				this.SystemSpawners[this.AliveSpawnerCount - 1].ParticleSpawner = null;
				this.AliveSpawnerCount--;
			}
			else
			{
				this.IsExpired = false;
			}
			return result;
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x0019F4A0 File Offset: 0x0019D6A0
		private void UpdateSpawnerSimulation(int spawnerIndex, int totalSteps)
		{
			ref ParticleSystem.SystemSpawner ptr = ref this.SystemSpawners[spawnerIndex];
			ParticleSystemSettings.SystemSpawnerSettings settings = this.SpawnerGroups[ptr.GroupId].Settings;
			for (float num = 0f; num < (float)totalSteps; num += 1f)
			{
				ptr.AttractorVelocity = Vector3.Zero;
				for (int i = 0; i < settings.Attractors.Length; i++)
				{
					settings.Attractors[i].Apply(ptr.Position, ptr.SystemPositionAtSpawn - this.Position, ref ptr.Velocity, ref ptr.AttractorVelocity);
				}
				ptr.Position += ptr.Velocity + ptr.AttractorVelocity;
			}
			this.UpdateSpawnerPositionAndRotation(spawnerIndex);
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x0019F580 File Offset: 0x0019D780
		private void UpdateSpawnerPositionAndRotation(int spawnerIndex)
		{
			ref ParticleSystem.SystemSpawner ptr = ref this.SystemSpawners[spawnerIndex];
			ParticleSystemSettings.SystemSpawnerSettings settings = this.SpawnerGroups[ptr.GroupId].Settings;
			ptr.ParticleSpawner.Position = this.Position + Vector3.Transform(ptr.Position + settings.PositionOffset, this.Rotation) * this.Scale;
			bool flag = !settings.FixedRotation;
			if (flag)
			{
				ptr.ParticleSpawner.Rotation = this.Rotation * settings.RotationOffset;
			}
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x0019F618 File Offset: 0x0019D818
		public void UpdateLife(float deltaTime)
		{
			this.ConsumeSystemLifeSpan(deltaTime);
			this.TryToCreateSpawners(deltaTime);
			bool flag;
			for (int i = 0; i < this.AliveSpawnerCount; i += (flag ? 0 : 1))
			{
				ref ParticleSystem.SystemSpawner ptr = ref this.SystemSpawners[i];
				ptr.ParticleSpawner.UpdateLife(deltaTime);
				flag = this.CheckSpawnerDeath(i, deltaTime);
			}
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x0019F678 File Offset: 0x0019D878
		public void UpdateSimulation(float deltaTime)
		{
			int totalSteps = this.UpdateAttractorSteps(deltaTime);
			for (int i = 0; i < this.AliveSpawnerCount; i++)
			{
				ParticleSpawner particleSpawner = this.SystemSpawners[i].ParticleSpawner;
				this.UpdateSpawnerSimulation(i, totalSteps);
				particleSpawner.UpdateSimulation(deltaTime, totalSteps);
				this._updateLightingFunc(particleSpawner);
			}
		}

		// Token: 0x0600569E RID: 22174 RVA: 0x0019F6D8 File Offset: 0x0019D8D8
		public void PrepareForDraw(Vector3 cameraPosition, ref FXVertexBuffer vertexBuffer, IntPtr gpuDrawDataPtr)
		{
			for (int i = 0; i < this.AliveSpawnerCount; i++)
			{
				bool flag = this.SystemSpawners[i].ParticleSpawner.ActiveParticles > 0;
				if (flag)
				{
					this.SystemSpawners[i].ParticleSpawner.PrepareForDraw(cameraPosition, ref vertexBuffer, gpuDrawDataPtr);
				}
			}
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x0019F738 File Offset: 0x0019D938
		public void SetFirstPerson(bool isFirstPerson)
		{
			this.IsFirstPerson = isFirstPerson;
			for (int i = 0; i < this.AliveSpawnerCount; i++)
			{
				this.SystemSpawners[i].ParticleSpawner.IsFirstPerson = this.IsFirstPerson;
			}
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x0019F784 File Offset: 0x0019D984
		private void InitializeSpawner(ref ParticleSystem.SystemSpawnerGroup group, int systemSpawnerId, int particleBufferStartIndex)
		{
			ParticleSpawnerSettings particleSpawnerSettings = group.Settings.ParticleSpawnerSettings;
			ParticleSettings particleSettings = particleSpawnerSettings.ParticleSettings;
			bool useSoftParticles = particleSettings.SoftParticlesOption == ParticleSettings.SoftParticles.Require || (particleSettings.SoftParticlesOption == ParticleSettings.SoftParticles.Enable && !this._isGPULowEnd);
			ParticleSpawner particleSpawner = new ParticleSpawner(this._particleFXSystem, this._updateCollisionFunc, this._initParticleFunc, this._random, particleSpawnerSettings, useSoftParticles, this._textureAltasInverseSize, particleBufferStartIndex);
			ref ParticleSystem.SystemSpawner ptr = ref this.SystemSpawners[this.AliveSpawnerCount];
			this.AliveSpawnerCount++;
			ptr.Id = systemSpawnerId;
			ptr.GroupId = group.Id;
			ptr.ParticleSpawner = particleSpawner;
			ptr.LifeSpanTimer = this._random.NextFloat(group.Settings.LifeSpan.X, group.Settings.LifeSpan.Y);
			ptr.SystemPositionAtSpawn = this.Position;
			float num = this._random.NextFloat(group.Settings.EmitOffsetMin.X, group.Settings.EmitOffsetMax.X);
			float num2 = this._random.NextFloat(group.Settings.EmitOffsetMin.Y, group.Settings.EmitOffsetMax.Y);
			float num3 = this._random.NextFloat(group.Settings.EmitOffsetMin.Z, group.Settings.EmitOffsetMax.Z);
			float num4 = (float)this._random.NextDouble() * 6.2831855f;
			float num5 = (float)this._random.NextDouble() * 6.2831855f;
			ptr.Position = new Vector3(num * (float)Math.Cos((double)num4) * (float)Math.Cos((double)num5), num2 * (float)Math.Sin((double)num4) * (float)Math.Cos((double)num5), num3 * (float)Math.Sin((double)num5));
			float yaw = this._random.NextFloat(group.Settings.InitialVelocityMin.Yaw, group.Settings.InitialVelocityMax.Yaw);
			float pitch = this._random.NextFloat(group.Settings.InitialVelocityMin.Pitch, group.Settings.InitialVelocityMax.Pitch);
			float scaleFactor = this._random.NextFloat(group.Settings.InitialVelocityMin.Speed, group.Settings.InitialVelocityMax.Speed);
			ptr.Velocity = Vector3.Transform(Vector3.Forward * scaleFactor, Quaternion.CreateFromYawPitchRoll(yaw, pitch, 0f));
			ptr.ParticleSpawner.SpawnAtPosition(this.Position + Vector3.Transform(ptr.Position + group.Settings.PositionOffset, this.Rotation) * this.Scale, (!group.Settings.FixedRotation) ? (this.Rotation * group.Settings.RotationOffset) : group.Settings.RotationOffset);
			ptr.ParticleSpawner.IsOvergroundOnly = this.IsOvergroundOnly;
			ptr.ParticleSpawner.SetScale(this.Scale);
			ptr.ParticleSpawner.IsFirstPerson = this.IsFirstPerson;
			ptr.ParticleSpawner.DefaultColor = this.DefaultColor;
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x0019FAC8 File Offset: 0x0019DCC8
		private void TakeStorageSlot(ref ParticleSystem.SystemSpawnerGroup group, out int systemSpawnerId, out int spawnerParticleBufferStartIndex)
		{
			int num = (int)BitUtils.FindFirstBitOff(group.ActiveSpawnersBits);
			bool flag = BitUtils.IsBitOn(num, group.ActiveSpawnersBits);
			if (flag)
			{
				ParticleSystem.Logger.Info("Error in the ActiveSpawnersBits management.");
			}
			BitUtils.SwitchOnBit(num, ref group.ActiveSpawnersBits);
			systemSpawnerId = num + group.SpawnerIdStart;
			spawnerParticleBufferStartIndex = group.SpawnerParticlesIdStart + num * group.Settings.ParticleSpawnerSettings.MaxConcurrentParticles;
		}

		// Token: 0x060056A2 RID: 22178 RVA: 0x0019FB34 File Offset: 0x0019DD34
		private void FreeStorageSlot(ref ParticleSystem.SystemSpawnerGroup group, int systemSpawnerId)
		{
			int bitId = systemSpawnerId - group.SpawnerIdStart;
			bool flag = !BitUtils.IsBitOn(bitId, group.ActiveSpawnersBits);
			if (flag)
			{
				ParticleSystem.Logger.Info("Error in the ActiveSpawnersBits management.");
			}
			BitUtils.SwitchOffBit(bitId, ref group.ActiveSpawnersBits);
		}

		// Token: 0x040033DD RID: 13277
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040033DE RID: 13278
		public ParticleSystemProxy Proxy;

		// Token: 0x040033DF RID: 13279
		public Vector3 Position;

		// Token: 0x040033E0 RID: 13280
		public Quaternion Rotation = Quaternion.Identity;

		// Token: 0x040033E1 RID: 13281
		public float Scale = 1f;

		// Token: 0x040033E2 RID: 13282
		public UInt32Color DefaultColor = ParticleSettings.DefaultColor;

		// Token: 0x040033E8 RID: 13288
		public bool IsOvergroundOnly = false;

		// Token: 0x040033E9 RID: 13289
		public ParticleSystem.SystemSpawnerGroup[] SpawnerGroups;

		// Token: 0x040033EA RID: 13290
		public ParticleSystem.SystemSpawner[] SystemSpawners;

		// Token: 0x040033EB RID: 13291
		public int AliveSpawnerCount;

		// Token: 0x040033EC RID: 13292
		private readonly ParticleSystemSettings _particleSystemSettings;

		// Token: 0x040033ED RID: 13293
		private ParticleFXSystem _particleFXSystem;

		// Token: 0x040033EE RID: 13294
		private bool _isGPULowEnd;

		// Token: 0x040033EF RID: 13295
		private UpdateSpawnerLightingFunc _updateLightingFunc;

		// Token: 0x040033F0 RID: 13296
		private UpdateParticleCollisionFunc _updateCollisionFunc;

		// Token: 0x040033F1 RID: 13297
		private InitParticleFunc _initParticleFunc;

		// Token: 0x040033F2 RID: 13298
		private int _particleBufferStartIndex;

		// Token: 0x040033F3 RID: 13299
		private int _maxParticles;

		// Token: 0x040033F4 RID: 13300
		private int _nextSystemSpawnerId;

		// Token: 0x040033F5 RID: 13301
		private Random _random;

		// Token: 0x040033F6 RID: 13302
		private float _attractorStep = 0f;

		// Token: 0x040033F7 RID: 13303
		private bool _hasUniqueSpawners = true;

		// Token: 0x040033F8 RID: 13304
		private bool _isWaitingForSpawners = false;

		// Token: 0x040033F9 RID: 13305
		private float _lifeSpanTimer;

		// Token: 0x040033FA RID: 13306
		private Vector2 _textureAltasInverseSize;

		// Token: 0x02000F07 RID: 3847
		public struct SystemSpawnerGroup
		{
			// Token: 0x040049B0 RID: 18864
			public int Id;

			// Token: 0x040049B1 RID: 18865
			public ParticleSystemSettings.SystemSpawnerSettings Settings;

			// Token: 0x040049B2 RID: 18866
			public float SpawnerCrumbs;

			// Token: 0x040049B3 RID: 18867
			public bool IsSingleSpawner;

			// Token: 0x040049B4 RID: 18868
			public int SpawnersLeft;

			// Token: 0x040049B5 RID: 18869
			public int ActiveSpawners;

			// Token: 0x040049B6 RID: 18870
			public uint ActiveSpawnersBits;

			// Token: 0x040049B7 RID: 18871
			public int SpawnerIdStart;

			// Token: 0x040049B8 RID: 18872
			public int SpawnerParticlesIdStart;

			// Token: 0x040049B9 RID: 18873
			public float StartTimer;

			// Token: 0x040049BA RID: 18874
			public bool HasWaves;

			// Token: 0x040049BB RID: 18875
			public float WaveTimer;

			// Token: 0x040049BC RID: 18876
			public bool WaveEnded;
		}

		// Token: 0x02000F08 RID: 3848
		public struct SystemSpawner
		{
			// Token: 0x040049BD RID: 18877
			public int Id;

			// Token: 0x040049BE RID: 18878
			public int GroupId;

			// Token: 0x040049BF RID: 18879
			public ParticleSpawner ParticleSpawner;

			// Token: 0x040049C0 RID: 18880
			public Vector3 Position;

			// Token: 0x040049C1 RID: 18881
			public Vector3 SystemPositionAtSpawn;

			// Token: 0x040049C2 RID: 18882
			public Vector3 Velocity;

			// Token: 0x040049C3 RID: 18883
			public Vector3 AttractorVelocity;

			// Token: 0x040049C4 RID: 18884
			public float LifeSpanTimer;
		}
	}
}
