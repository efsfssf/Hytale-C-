using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Trails
{
	// Token: 0x02000AAD RID: 2733
	internal class TrailProxy
	{
		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x06005601 RID: 22017 RVA: 0x0019A560 File Offset: 0x00198760
		// (set) Token: 0x06005602 RID: 22018 RVA: 0x0019A568 File Offset: 0x00198768
		public bool IsFirstPerson { get; private set; } = false;

		// Token: 0x06005603 RID: 22019 RVA: 0x0019A574 File Offset: 0x00198774
		public void SetFirstPerson(bool isFirstPerson)
		{
			this.IsFirstPerson = isFirstPerson;
			bool flag = this.Trail != null;
			if (flag)
			{
				this.Trail.IsFirstPerson = isFirstPerson;
			}
		}

		// Token: 0x0400330C RID: 13068
		public TrailSettings Settings;

		// Token: 0x0400330D RID: 13069
		public Trail Trail;

		// Token: 0x0400330E RID: 13070
		public Vector2 TextureAltasInverseSize;

		// Token: 0x0400330F RID: 13071
		public bool Visible = true;

		// Token: 0x04003310 RID: 13072
		public Vector3 Position;

		// Token: 0x04003311 RID: 13073
		public Quaternion Rotation = Quaternion.Identity;

		// Token: 0x04003312 RID: 13074
		public float Scale = 1f;

		// Token: 0x04003314 RID: 13076
		public bool IsLocalPlayer = false;

		// Token: 0x04003315 RID: 13077
		public bool IsExpired = false;
	}
}
