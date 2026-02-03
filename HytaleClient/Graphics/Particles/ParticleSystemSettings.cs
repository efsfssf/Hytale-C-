using System;
using System.Diagnostics;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000ABD RID: 2749
	internal class ParticleSystemSettings
	{
		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x060056B1 RID: 22193 RVA: 0x001A0671 File Offset: 0x0019E871
		public ParticleSystemSettings.SystemSpawnerSettings[] SystemSpawnerSettingsList
		{
			get
			{
				return this._systemSpawnerSettingsList;
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x060056B2 RID: 22194 RVA: 0x001A0679 File Offset: 0x0019E879
		public byte SystemSpawnerCount
		{
			get
			{
				return this._systemSpawnerCount;
			}
		}

		// Token: 0x060056B4 RID: 22196 RVA: 0x001A0696 File Offset: 0x0019E896
		public void CreateSpawnerSettingsStorage(byte itemCount)
		{
			this._systemSpawnerSettingsList = new ParticleSystemSettings.SystemSpawnerSettings[(int)itemCount];
			this._systemSpawnerCount = itemCount;
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x001A06AC File Offset: 0x0019E8AC
		public void DeleteSpawnerSettings(byte index)
		{
			Debug.Assert(index < this.SystemSpawnerCount);
			Debug.Assert(this.SystemSpawnerCount > 0);
			for (int i = (int)index; i <= (int)(this.SystemSpawnerCount - 2); i++)
			{
				this._systemSpawnerSettingsList[i] = this._systemSpawnerSettingsList[i + 1];
			}
			this._systemSpawnerCount -= 1;
		}

		// Token: 0x04003418 RID: 13336
		public const int MaxDefaultConcurrentSpawners = 1;

		// Token: 0x04003419 RID: 13337
		public const int MaxPossibleConcurrentSpawners = 10;

		// Token: 0x0400341A RID: 13338
		public const float DefaultCullDistance = 40f;

		// Token: 0x0400341B RID: 13339
		public const float DefaultCullDistanceSquared = 1600f;

		// Token: 0x0400341C RID: 13340
		public const float DefaultBoundingRadius = 10f;

		// Token: 0x0400341D RID: 13341
		public static readonly Vector2 DefaultSingleSpawnerLifeSpan = new Vector2(-1f, -1f);

		// Token: 0x0400341E RID: 13342
		public static readonly Vector2 DefaultSpawnerLifeSpan = new Vector2(2f, 2f);

		// Token: 0x0400341F RID: 13343
		public static readonly Vector2 DefaultSpawnRate = Vector2.One;

		// Token: 0x04003420 RID: 13344
		public bool IsImportant;

		// Token: 0x04003421 RID: 13345
		public float CullDistanceSquared;

		// Token: 0x04003422 RID: 13346
		public float BoundingRadius;

		// Token: 0x04003423 RID: 13347
		public float LifeSpan = -1f;

		// Token: 0x04003424 RID: 13348
		private ParticleSystemSettings.SystemSpawnerSettings[] _systemSpawnerSettingsList;

		// Token: 0x04003425 RID: 13349
		private byte _systemSpawnerCount;

		// Token: 0x02000F09 RID: 3849
		public class SystemSpawnerSettings
		{
			// Token: 0x040049C5 RID: 18885
			public string ParticleSpawnerId;

			// Token: 0x040049C6 RID: 18886
			public ParticleSpawnerSettings ParticleSpawnerSettings;

			// Token: 0x040049C7 RID: 18887
			public bool FixedRotation = false;

			// Token: 0x040049C8 RID: 18888
			public Vector3 PositionOffset = Vector3.Zero;

			// Token: 0x040049C9 RID: 18889
			public Quaternion RotationOffset = Quaternion.Identity;

			// Token: 0x040049CA RID: 18890
			public Vector2 LifeSpan = ParticleSystemSettings.DefaultSingleSpawnerLifeSpan;

			// Token: 0x040049CB RID: 18891
			public float StartDelay = 0f;

			// Token: 0x040049CC RID: 18892
			public Vector2 SpawnRate = ParticleSystemSettings.DefaultSpawnRate;

			// Token: 0x040049CD RID: 18893
			public Vector2 WaveDelay;

			// Token: 0x040049CE RID: 18894
			public int TotalSpawners = 1;

			// Token: 0x040049CF RID: 18895
			public int MaxConcurrent = 1;

			// Token: 0x040049D0 RID: 18896
			public ParticleSpawnerSettings.InitialVelocity InitialVelocityMin;

			// Token: 0x040049D1 RID: 18897
			public ParticleSpawnerSettings.InitialVelocity InitialVelocityMax;

			// Token: 0x040049D2 RID: 18898
			public Vector3 EmitOffsetMin;

			// Token: 0x040049D3 RID: 18899
			public Vector3 EmitOffsetMax;

			// Token: 0x040049D4 RID: 18900
			public ParticleAttractor[] Attractors = new ParticleAttractor[0];
		}
	}
}
