using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A4C RID: 2636
	public class TextureArea
	{
		// Token: 0x060053EC RID: 21484 RVA: 0x00180A51 File Offset: 0x0017EC51
		public TextureArea(Texture texture, int x, int y, int width, int height, int scale)
		{
			this.Texture = texture;
			this.Rectangle = new Rectangle(x, y, width, height);
			this.Scale = scale;
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x00180A7B File Offset: 0x0017EC7B
		private TextureArea()
		{
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x00180A88 File Offset: 0x0017EC88
		public TextureArea Clone()
		{
			return new TextureArea
			{
				Texture = this.Texture,
				Rectangle = this.Rectangle,
				Scale = this.Scale
			};
		}

		// Token: 0x04002EE0 RID: 12000
		public Texture Texture;

		// Token: 0x04002EE1 RID: 12001
		public Rectangle Rectangle;

		// Token: 0x04002EE2 RID: 12002
		public int Scale;
	}
}
