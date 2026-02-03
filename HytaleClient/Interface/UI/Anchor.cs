using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI
{
	// Token: 0x02000825 RID: 2085
	[UIMarkupData]
	public struct Anchor
	{
		// Token: 0x1700100E RID: 4110
		// (set) Token: 0x06003A25 RID: 14885 RVA: 0x00083290 File Offset: 0x00081490
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

		// Token: 0x1700100F RID: 4111
		// (set) Token: 0x06003A26 RID: 14886 RVA: 0x000832C0 File Offset: 0x000814C0
		public int? Horizontal
		{
			set
			{
				this.Right = value;
				this.Left = value;
			}
		}

		// Token: 0x17001010 RID: 4112
		// (set) Token: 0x06003A27 RID: 14887 RVA: 0x000832E0 File Offset: 0x000814E0
		public int? Vertical
		{
			set
			{
				this.Bottom = value;
				this.Top = value;
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x000832FD File Offset: 0x000814FD
		// (set) Token: 0x06003A29 RID: 14889 RVA: 0x00083308 File Offset: 0x00081508
		public int? Width
		{
			get
			{
				return this._width;
			}
			set
			{
				bool flag = this._maxWidth != null;
				if (flag)
				{
					throw new Exception("Can't set Width, MaxWidth has been set.");
				}
				this._width = value;
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x06003A2A RID: 14890 RVA: 0x00083337 File Offset: 0x00081537
		// (set) Token: 0x06003A2B RID: 14891 RVA: 0x00083340 File Offset: 0x00081540
		public int? MinWidth
		{
			get
			{
				return this._minWidth;
			}
			set
			{
				bool flag = this._width != null;
				if (flag)
				{
					throw new Exception("Can't set MinWidth, Width has been set.");
				}
				this._minWidth = value;
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06003A2C RID: 14892 RVA: 0x0008336F File Offset: 0x0008156F
		// (set) Token: 0x06003A2D RID: 14893 RVA: 0x00083378 File Offset: 0x00081578
		public int? MaxWidth
		{
			get
			{
				return this._maxWidth;
			}
			set
			{
				bool flag = this._width != null;
				if (flag)
				{
					throw new Exception("Can't set MaxWidth, Width has been set.");
				}
				this._maxWidth = value;
			}
		}

		// Token: 0x04001A1F RID: 6687
		public int? Left;

		// Token: 0x04001A20 RID: 6688
		public int? Right;

		// Token: 0x04001A21 RID: 6689
		public int? Top;

		// Token: 0x04001A22 RID: 6690
		public int? Bottom;

		// Token: 0x04001A23 RID: 6691
		private int? _width;

		// Token: 0x04001A24 RID: 6692
		private int? _minWidth;

		// Token: 0x04001A25 RID: 6693
		private int? _maxWidth;

		// Token: 0x04001A26 RID: 6694
		public int? Height;
	}
}
