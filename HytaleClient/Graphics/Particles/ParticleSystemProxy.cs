using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000ABC RID: 2748
	internal class ParticleSystemProxy
	{
		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x060056A8 RID: 22184 RVA: 0x001A059F File Offset: 0x0019E79F
		// (set) Token: 0x060056A9 RID: 22185 RVA: 0x001A05A7 File Offset: 0x0019E7A7
		public bool IsFirstPerson { get; private set; } = false;

		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x060056AA RID: 22186 RVA: 0x001A05B0 File Offset: 0x0019E7B0
		// (set) Token: 0x060056AB RID: 22187 RVA: 0x001A05B8 File Offset: 0x0019E7B8
		public bool IsExpired { get; private set; } = false;

		// Token: 0x1700133D RID: 4925
		// (get) Token: 0x060056AC RID: 22188 RVA: 0x001A05C1 File Offset: 0x0019E7C1
		// (set) Token: 0x060056AD RID: 22189 RVA: 0x001A05C9 File Offset: 0x0019E7C9
		public bool HasInstantExpire { get; private set; } = false;

		// Token: 0x060056AE RID: 22190 RVA: 0x001A05D2 File Offset: 0x0019E7D2
		public void SetFirstPerson(bool isFirstPerson)
		{
			this.IsFirstPerson = isFirstPerson;
			ParticleSystem particleSystem = this.ParticleSystem;
			if (particleSystem != null)
			{
				particleSystem.SetFirstPerson(isFirstPerson);
			}
		}

		// Token: 0x060056AF RID: 22191 RVA: 0x001A05F0 File Offset: 0x0019E7F0
		public void Expire(bool instant = false)
		{
			this.IsExpired = true;
			this.HasInstantExpire = instant;
		}

		// Token: 0x04003409 RID: 13321
		public ParticleSystemSettings Settings;

		// Token: 0x0400340A RID: 13322
		public ParticleSystem ParticleSystem;

		// Token: 0x0400340B RID: 13323
		public Vector2 TextureAltasInverseSize;

		// Token: 0x0400340C RID: 13324
		public bool VisibilityPrediction = true;

		// Token: 0x0400340D RID: 13325
		public bool Visible = true;

		// Token: 0x0400340E RID: 13326
		public Vector3 Position;

		// Token: 0x0400340F RID: 13327
		public Quaternion Rotation = Quaternion.Identity;

		// Token: 0x04003410 RID: 13328
		public float Scale = 1f;

		// Token: 0x04003411 RID: 13329
		public UInt32Color DefaultColor = ParticleSettings.DefaultColor;

		// Token: 0x04003413 RID: 13331
		public bool IsOvergroundOnly = false;

		// Token: 0x04003414 RID: 13332
		public bool IsLocalPlayer = false;

		// Token: 0x04003415 RID: 13333
		public bool IsTracked = false;
	}
}
