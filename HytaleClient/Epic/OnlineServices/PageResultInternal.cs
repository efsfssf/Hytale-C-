using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices
{
	// Token: 0x0200001F RID: 31
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PageResultInternal : IGettable<PageResult>, ISettable<PageResult>, IDisposable
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000339 RID: 825 RVA: 0x00004860 File Offset: 0x00002A60
		// (set) Token: 0x0600033A RID: 826 RVA: 0x00004878 File Offset: 0x00002A78
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00004884 File Offset: 0x00002A84
		// (set) Token: 0x0600033C RID: 828 RVA: 0x0000489C File Offset: 0x00002A9C
		public int Count
		{
			get
			{
				return this.m_Count;
			}
			set
			{
				this.m_Count = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600033D RID: 829 RVA: 0x000048A8 File Offset: 0x00002AA8
		// (set) Token: 0x0600033E RID: 830 RVA: 0x000048C0 File Offset: 0x00002AC0
		public int TotalCount
		{
			get
			{
				return this.m_TotalCount;
			}
			set
			{
				this.m_TotalCount = value;
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000048CA File Offset: 0x00002ACA
		public void Set(ref PageResult other)
		{
			this.StartIndex = other.StartIndex;
			this.Count = other.Count;
			this.TotalCount = other.TotalCount;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000048F4 File Offset: 0x00002AF4
		public void Set(ref PageResult? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.StartIndex = other.Value.StartIndex;
				this.Count = other.Value.Count;
				this.TotalCount = other.Value.TotalCount;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000494D File Offset: 0x00002B4D
		public void Dispose()
		{
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00004950 File Offset: 0x00002B50
		public void Get(out PageResult output)
		{
			output = default(PageResult);
			output.Set(ref this);
		}

		// Token: 0x04000056 RID: 86
		private int m_StartIndex;

		// Token: 0x04000057 RID: 87
		private int m_Count;

		// Token: 0x04000058 RID: 88
		private int m_TotalCount;
	}
}
