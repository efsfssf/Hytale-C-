using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006CE RID: 1742
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct Vec3fInternal : IGettable<Vec3f>, ISettable<Vec3f>, IDisposable
	{
		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06002D3E RID: 11582 RVA: 0x00042D18 File Offset: 0x00040F18
		// (set) Token: 0x06002D3F RID: 11583 RVA: 0x00042D30 File Offset: 0x00040F30
		public float x
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		// Token: 0x17000D87 RID: 3463
		// (get) Token: 0x06002D40 RID: 11584 RVA: 0x00042D3C File Offset: 0x00040F3C
		// (set) Token: 0x06002D41 RID: 11585 RVA: 0x00042D54 File Offset: 0x00040F54
		public float y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		// Token: 0x17000D88 RID: 3464
		// (get) Token: 0x06002D42 RID: 11586 RVA: 0x00042D60 File Offset: 0x00040F60
		// (set) Token: 0x06002D43 RID: 11587 RVA: 0x00042D78 File Offset: 0x00040F78
		public float z
		{
			get
			{
				return this.m_z;
			}
			set
			{
				this.m_z = value;
			}
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x00042D82 File Offset: 0x00040F82
		public void Set(ref Vec3f other)
		{
			this.x = other.x;
			this.y = other.y;
			this.z = other.z;
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x00042DAC File Offset: 0x00040FAC
		public void Set(ref Vec3f? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.x = other.Value.x;
				this.y = other.Value.y;
				this.z = other.Value.z;
			}
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x00042E05 File Offset: 0x00041005
		public void Dispose()
		{
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x00042E08 File Offset: 0x00041008
		public void Get(out Vec3f output)
		{
			output = default(Vec3f);
			output.Set(ref this);
		}

		// Token: 0x040013EA RID: 5098
		private float m_x;

		// Token: 0x040013EB RID: 5099
		private float m_y;

		// Token: 0x040013EC RID: 5100
		private float m_z;
	}
}
