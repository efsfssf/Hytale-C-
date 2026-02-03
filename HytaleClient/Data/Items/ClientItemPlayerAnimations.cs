using System;
using System.Collections.Generic;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF5 RID: 2805
	internal class ClientItemPlayerAnimations
	{
		// Token: 0x0600583E RID: 22590 RVA: 0x001AC40C File Offset: 0x001AA60C
		public ClientItemPlayerAnimations(ItemPlayerAnimations networkAnimations)
		{
			this.Id = networkAnimations.Id;
			this.WiggleWeights = networkAnimations.WiggleWeights_;
			bool flag = networkAnimations.Camera != null;
			if (flag)
			{
				this.Camera = new CameraSettings(networkAnimations.Camera);
				bool flag2 = this.Camera.Yaw != null && this.Camera.Yaw.AngleRange != null;
				if (flag2)
				{
					this.Camera.Yaw.AngleRange.Min = MathHelper.ToRadians(this.Camera.Yaw.AngleRange.Min);
					this.Camera.Yaw.AngleRange.Max = MathHelper.ToRadians(this.Camera.Yaw.AngleRange.Max);
				}
				bool flag3 = this.Camera.Pitch != null && this.Camera.Pitch.AngleRange != null;
				if (flag3)
				{
					this.Camera.Pitch.AngleRange.Min = MathHelper.ToRadians(this.Camera.Pitch.AngleRange.Min);
					this.Camera.Pitch.AngleRange.Max = MathHelper.ToRadians(this.Camera.Pitch.AngleRange.Max);
				}
			}
		}

		// Token: 0x040036B8 RID: 14008
		public const string DefaultId = "Default";

		// Token: 0x040036B9 RID: 14009
		public string Id;

		// Token: 0x040036BA RID: 14010
		public readonly Dictionary<string, EntityAnimation> Animations = new Dictionary<string, EntityAnimation>();

		// Token: 0x040036BB RID: 14011
		public readonly ItemPlayerAnimations.WiggleWeights WiggleWeights;

		// Token: 0x040036BC RID: 14012
		public readonly CameraSettings Camera;
	}
}
