using System;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000AB0 RID: 2736
	internal class ModelParticleSettings
	{
		// Token: 0x06005606 RID: 22022 RVA: 0x0019A5E8 File Offset: 0x001987E8
		public ModelParticleSettings(string systemId = "")
		{
			this.SystemId = systemId;
			this.Color = UInt32Color.Transparent;
			this.Scale = 1f;
			this.TargetNodeNameId = -1;
			this.TargetEntityPart = 0;
			this.TargetNodeIndex = 0;
			this.PositionOffset = Vector3.Zero;
			this.RotationOffset = Quaternion.Identity;
			this.DetachedFromModel = false;
		}

		// Token: 0x04003327 RID: 13095
		public string SystemId;

		// Token: 0x04003328 RID: 13096
		public UInt32Color Color;

		// Token: 0x04003329 RID: 13097
		public float Scale;

		// Token: 0x0400332A RID: 13098
		public int TargetNodeNameId;

		// Token: 0x0400332B RID: 13099
		public EntityPart TargetEntityPart;

		// Token: 0x0400332C RID: 13100
		public int TargetNodeIndex;

		// Token: 0x0400332D RID: 13101
		public Vector3 PositionOffset;

		// Token: 0x0400332E RID: 13102
		public Quaternion RotationOffset;

		// Token: 0x0400332F RID: 13103
		public bool DetachedFromModel;
	}
}
