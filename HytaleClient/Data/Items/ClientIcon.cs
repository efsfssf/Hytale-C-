using System;
using Coherent.UI.Binding;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AEF RID: 2799
	[CoherentType]
	internal class ClientIcon
	{
		// Token: 0x06005834 RID: 22580 RVA: 0x001AC192 File Offset: 0x001AA392
		public ClientIcon(int x, int y, int size)
		{
			this.X = x;
			this.Y = y;
			this.Size = size;
		}

		// Token: 0x04003677 RID: 13943
		public const int MaxSize = 64;

		// Token: 0x04003678 RID: 13944
		[CoherentProperty("x")]
		public readonly int X;

		// Token: 0x04003679 RID: 13945
		[CoherentProperty("y")]
		public readonly int Y;

		// Token: 0x0400367A RID: 13946
		[CoherentProperty("size")]
		public readonly int Size;
	}
}
