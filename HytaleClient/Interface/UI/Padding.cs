using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI
{
	// Token: 0x0200082B RID: 2091
	[UIMarkupData]
	public struct Padding
	{
		// Token: 0x17001026 RID: 4134
		// (set) Token: 0x06003A8A RID: 14986 RVA: 0x000858F4 File Offset: 0x00083AF4
		public int? Full
		{
			set
			{
				this.Bottom = value;
				this.Top = value;
				this.Right = value;
				this.Left = value;
			}
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06003A8B RID: 14987 RVA: 0x00085924 File Offset: 0x00083B24
		// (set) Token: 0x06003A8C RID: 14988 RVA: 0x00085974 File Offset: 0x00083B74
		public int? Horizontal
		{
			get
			{
				return this.Left + this.Right;
			}
			set
			{
				this.Right = value;
				this.Left = value;
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06003A8D RID: 14989 RVA: 0x00085994 File Offset: 0x00083B94
		// (set) Token: 0x06003A8E RID: 14990 RVA: 0x000859E4 File Offset: 0x00083BE4
		public int? Vertical
		{
			get
			{
				return this.Top + this.Bottom;
			}
			set
			{
				this.Bottom = value;
				this.Top = value;
			}
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x00085A02 File Offset: 0x00083C02
		public Padding(int full)
		{
			this.Left = new int?(full);
			this.Right = new int?(full);
			this.Top = new int?(full);
			this.Bottom = new int?(full);
		}

		// Token: 0x04001A5C RID: 6748
		public int? Left;

		// Token: 0x04001A5D RID: 6749
		public int? Right;

		// Token: 0x04001A5E RID: 6750
		public int? Top;

		// Token: 0x04001A5F RID: 6751
		public int? Bottom;
	}
}
