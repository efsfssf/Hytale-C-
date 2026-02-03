using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000856 RID: 2134
	[UIMarkupElement]
	internal class CircularProgressBar : Element
	{
		// Token: 0x06003B33 RID: 15155 RVA: 0x0008B09C File Offset: 0x0008929C
		public CircularProgressBar(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003B34 RID: 15156 RVA: 0x0008B0B4 File Offset: 0x000892B4
		private void PrepareForDrawTriangle(float rot, float percentage, Rectangle dest)
		{
			TextureArea whitePixel = this.Desktop.Provider.WhitePixel;
			this.Desktop.Batcher2D.SetTransformationMatrix(new Vector3((float)dest.X, (float)dest.Y, 0f), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(rot)), 1f);
			Vector3 topLeft = new Vector3(0f, (float)(-(float)dest.Height), 0f);
			Vector3 bottomLeft = new Vector3(0f, 0f, 0f);
			Vector3 bottonRight = new Vector3((float)dest.Width * percentage, (float)(-(float)dest.Height) + (float)dest.Height * percentage, 0f);
			this.Desktop.Batcher2D.RequestDrawTextureTriangle(whitePixel.Texture, whitePixel.Rectangle, topLeft, bottomLeft, bottonRight, this.Color);
		}

		// Token: 0x06003B35 RID: 15157 RVA: 0x0008B190 File Offset: 0x00089390
		protected override void PrepareForDrawSelf()
		{
			Rectangle dest = new Rectangle(this._anchoredRectangle.X + this._anchoredRectangle.Width / 2, this._anchoredRectangle.Y + this._anchoredRectangle.Height / 2, this._anchoredRectangle.Width / 2, this._anchoredRectangle.Height / 2);
			this.PrepareForDrawTriangle(0f, CircularProgressBar.GetPercentage(0, this.Value), dest);
			this.PrepareForDrawTriangle(90f, CircularProgressBar.GetPercentage(1, this.Value), dest);
			this.PrepareForDrawTriangle(180f, CircularProgressBar.GetPercentage(2, this.Value), dest);
			this.PrepareForDrawTriangle(270f, CircularProgressBar.GetPercentage(3, this.Value), dest);
			this.Desktop.Batcher2D.SetTransformationMatrix(Matrix.Identity);
		}

		// Token: 0x06003B36 RID: 15158 RVA: 0x0008B26C File Offset: 0x0008946C
		private static float GetPercentage(int index, float Value)
		{
			Value = MathHelper.Clamp(Value, 0f, 1f);
			return MathHelper.Clamp((Value - 0.25f * (float)index) / 0.25f, 0f, 1f);
		}

		// Token: 0x04001B4B RID: 6987
		[UIMarkupProperty]
		public float Value = 0f;

		// Token: 0x04001B4C RID: 6988
		[UIMarkupProperty]
		public UInt32Color Color;
	}
}
