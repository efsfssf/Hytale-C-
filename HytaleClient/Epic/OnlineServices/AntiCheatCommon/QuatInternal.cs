using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.AntiCheatCommon
{
	// Token: 0x020006C4 RID: 1732
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QuatInternal : IGettable<Quat>, ISettable<Quat>, IDisposable
	{
		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06002CFD RID: 11517 RVA: 0x00042798 File Offset: 0x00040998
		// (set) Token: 0x06002CFE RID: 11518 RVA: 0x000427B0 File Offset: 0x000409B0
		public float w
		{
			get
			{
				return this.m_w;
			}
			set
			{
				this.m_w = value;
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x000427BC File Offset: 0x000409BC
		// (set) Token: 0x06002D00 RID: 11520 RVA: 0x000427D4 File Offset: 0x000409D4
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

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06002D01 RID: 11521 RVA: 0x000427E0 File Offset: 0x000409E0
		// (set) Token: 0x06002D02 RID: 11522 RVA: 0x000427F8 File Offset: 0x000409F8
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

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06002D03 RID: 11523 RVA: 0x00042804 File Offset: 0x00040A04
		// (set) Token: 0x06002D04 RID: 11524 RVA: 0x0004281C File Offset: 0x00040A1C
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

		// Token: 0x06002D05 RID: 11525 RVA: 0x00042826 File Offset: 0x00040A26
		public void Set(ref Quat other)
		{
			this.w = other.w;
			this.x = other.x;
			this.y = other.y;
			this.z = other.z;
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x00042860 File Offset: 0x00040A60
		public void Set(ref Quat? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.w = other.Value.w;
				this.x = other.Value.x;
				this.y = other.Value.y;
				this.z = other.Value.z;
			}
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000428CE File Offset: 0x00040ACE
		public void Dispose()
		{
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000428D1 File Offset: 0x00040AD1
		public void Get(out Quat output)
		{
			output = default(Quat);
			output.Set(ref this);
		}

		// Token: 0x040013CB RID: 5067
		private float m_w;

		// Token: 0x040013CC RID: 5068
		private float m_x;

		// Token: 0x040013CD RID: 5069
		private float m_y;

		// Token: 0x040013CE RID: 5070
		private float m_z;
	}
}
