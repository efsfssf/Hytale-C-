using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.UI
{
	// Token: 0x0200007F RID: 127
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct RectInternal : IGettable<Rect>, ISettable<Rect>, IDisposable
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000077D4 File Offset: 0x000059D4
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x000077EC File Offset: 0x000059EC
		public int X
		{
			get
			{
				return this.m_X;
			}
			set
			{
				this.m_X = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x000077F8 File Offset: 0x000059F8
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x00007810 File Offset: 0x00005A10
		public int Y
		{
			get
			{
				return this.m_Y;
			}
			set
			{
				this.m_Y = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0000781C File Offset: 0x00005A1C
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x00007834 File Offset: 0x00005A34
		public uint Width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x00007840 File Offset: 0x00005A40
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x00007858 File Offset: 0x00005A58
		public uint Height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00007862 File Offset: 0x00005A62
		public void Set(ref Rect other)
		{
			this.m_ApiVersion = 1;
			this.X = other.X;
			this.Y = other.Y;
			this.Width = other.Width;
			this.Height = other.Height;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000078A0 File Offset: 0x00005AA0
		public void Set(ref Rect? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.X = other.Value.X;
				this.Y = other.Value.Y;
				this.Width = other.Value.Width;
				this.Height = other.Value.Height;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00007915 File Offset: 0x00005B15
		public void Dispose()
		{
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00007918 File Offset: 0x00005B18
		public void Get(out Rect output)
		{
			output = default(Rect);
			output.Set(ref this);
		}

		// Token: 0x0400029B RID: 667
		private int m_ApiVersion;

		// Token: 0x0400029C RID: 668
		private int m_X;

		// Token: 0x0400029D RID: 669
		private int m_Y;

		// Token: 0x0400029E RID: 670
		private uint m_Width;

		// Token: 0x0400029F RID: 671
		private uint m_Height;
	}
}
