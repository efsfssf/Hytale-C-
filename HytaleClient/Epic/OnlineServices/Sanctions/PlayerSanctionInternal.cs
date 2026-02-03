using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sanctions
{
	// Token: 0x020001A4 RID: 420
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PlayerSanctionInternal : IGettable<PlayerSanction>, ISettable<PlayerSanction>, IDisposable
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00011A4C File Offset: 0x0000FC4C
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x00011A64 File Offset: 0x0000FC64
		public long TimePlaced
		{
			get
			{
				return this.m_TimePlaced;
			}
			set
			{
				this.m_TimePlaced = value;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00011A70 File Offset: 0x0000FC70
		// (set) Token: 0x06000C27 RID: 3111 RVA: 0x00011A91 File Offset: 0x0000FC91
		public Utf8String Action
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Action, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Action);
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00011AA4 File Offset: 0x0000FCA4
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x00011ABC File Offset: 0x0000FCBC
		public long TimeExpires
		{
			get
			{
				return this.m_TimeExpires;
			}
			set
			{
				this.m_TimeExpires = value;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x00011AC8 File Offset: 0x0000FCC8
		// (set) Token: 0x06000C2B RID: 3115 RVA: 0x00011AE9 File Offset: 0x0000FCE9
		public Utf8String ReferenceId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_ReferenceId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ReferenceId);
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x00011AF9 File Offset: 0x0000FCF9
		public void Set(ref PlayerSanction other)
		{
			this.m_ApiVersion = 2;
			this.TimePlaced = other.TimePlaced;
			this.Action = other.Action;
			this.TimeExpires = other.TimeExpires;
			this.ReferenceId = other.ReferenceId;
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00011B38 File Offset: 0x0000FD38
		public void Set(ref PlayerSanction? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 2;
				this.TimePlaced = other.Value.TimePlaced;
				this.Action = other.Value.Action;
				this.TimeExpires = other.Value.TimeExpires;
				this.ReferenceId = other.Value.ReferenceId;
			}
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00011BAD File Offset: 0x0000FDAD
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Action);
			Helper.Dispose(ref this.m_ReferenceId);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00011BC8 File Offset: 0x0000FDC8
		public void Get(out PlayerSanction output)
		{
			output = default(PlayerSanction);
			output.Set(ref this);
		}

		// Token: 0x0400058A RID: 1418
		private int m_ApiVersion;

		// Token: 0x0400058B RID: 1419
		private long m_TimePlaced;

		// Token: 0x0400058C RID: 1420
		private IntPtr m_Action;

		// Token: 0x0400058D RID: 1421
		private long m_TimeExpires;

		// Token: 0x0400058E RID: 1422
		private IntPtr m_ReferenceId;
	}
}
