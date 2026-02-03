using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices
{
	// Token: 0x0200001D RID: 29
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PageQueryInternal : IGettable<PageQuery>, ISettable<PageQuery>, IDisposable
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600032A RID: 810 RVA: 0x00004734 File Offset: 0x00002934
		// (set) Token: 0x0600032B RID: 811 RVA: 0x0000474C File Offset: 0x0000294C
		public int StartIndex
		{
			get
			{
				return this.m_StartIndex;
			}
			set
			{
				this.m_StartIndex = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600032C RID: 812 RVA: 0x00004758 File Offset: 0x00002958
		// (set) Token: 0x0600032D RID: 813 RVA: 0x00004770 File Offset: 0x00002970
		public int MaxCount
		{
			get
			{
				return this.m_MaxCount;
			}
			set
			{
				this.m_MaxCount = value;
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000477A File Offset: 0x0000297A
		public void Set(ref PageQuery other)
		{
			this.m_ApiVersion = 1;
			this.StartIndex = other.StartIndex;
			this.MaxCount = other.MaxCount;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000047A0 File Offset: 0x000029A0
		public void Set(ref PageQuery? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.StartIndex = other.Value.StartIndex;
				this.MaxCount = other.Value.MaxCount;
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000047EB File Offset: 0x000029EB
		public void Dispose()
		{
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000047EE File Offset: 0x000029EE
		public void Get(out PageQuery output)
		{
			output = default(PageQuery);
			output.Set(ref this);
		}

		// Token: 0x04000050 RID: 80
		private int m_ApiVersion;

		// Token: 0x04000051 RID: 81
		private int m_StartIndex;

		// Token: 0x04000052 RID: 82
		private int m_MaxCount;
	}
}
