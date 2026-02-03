using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000870 RID: 2160
	[UIMarkupElement]
	public class ProgressBar : Element
	{
		// Token: 0x06003CE7 RID: 15591 RVA: 0x00099A0D File Offset: 0x00097C0D
		public ProgressBar(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x00099A28 File Offset: 0x00097C28
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._barPatch = ((this.BarTexturePath != null) ? this.Desktop.MakeTexturePatch(new PatchStyle(this.BarTexturePath.Value)) : null);
			this._effectPatch = ((this.EffectTexturePath != null) ? this.Desktop.MakeTexturePatch(new PatchStyle(this.EffectTexturePath.Value)) : null);
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x00099A98 File Offset: 0x00097C98
		protected override void LayoutSelf()
		{
			base.LayoutSelf();
			this._visibleBarRectangle = this._anchoredRectangle;
			bool flag = this.Alignment == ProgressBar.ProgressBarAlignment.Horizontal;
			if (flag)
			{
				this._visibleBarRectangle.Width = (int)((float)this._visibleBarRectangle.Width * this.Value);
				bool flag2 = this.Direction == ProgressBar.ProgressBarDirection.Start;
				if (flag2)
				{
					this._visibleBarRectangle.X = this._visibleBarRectangle.X + (this._anchoredRectangle.Width - this._visibleBarRectangle.Width);
				}
				this._effectRectangle = new Rectangle(((this.Direction == ProgressBar.ProgressBarDirection.End) ? this._visibleBarRectangle.Right : this._visibleBarRectangle.Left) - this.Desktop.ScaleRound((float)this.EffectOffset), (int)((float)(this._anchoredRectangle.Top - this.Desktop.ScaleRound((float)this.EffectHeight / 2f)) + (float)this._anchoredRectangle.Height / 2f), this.Desktop.ScaleRound((float)this.EffectWidth), this.Desktop.ScaleRound((float)this.EffectHeight));
			}
			else
			{
				this._visibleBarRectangle.Height = (int)((float)this._visibleBarRectangle.Height * this.Value);
				bool flag3 = this.Direction == ProgressBar.ProgressBarDirection.Start;
				if (flag3)
				{
					this._visibleBarRectangle.Y = this._visibleBarRectangle.Y + (this._anchoredRectangle.Height - this._visibleBarRectangle.Height);
				}
				this._effectRectangle = new Rectangle((int)((float)(this._anchoredRectangle.Left - this.Desktop.ScaleRound((float)this.EffectWidth / 2f)) + (float)this._anchoredRectangle.Width / 2f), ((this.Direction == ProgressBar.ProgressBarDirection.End) ? this._visibleBarRectangle.Bottom : this._visibleBarRectangle.Top) - this.Desktop.ScaleRound((float)this.EffectOffset), this.Desktop.ScaleRound((float)this.EffectWidth), this.Desktop.ScaleRound((float)this.EffectHeight));
			}
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x00099CB4 File Offset: 0x00097EB4
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			this.Desktop.Batcher2D.PushScissor(this._visibleBarRectangle);
			this.Desktop.Batcher2D.RequestDrawPatch(this._barPatch, this._anchoredRectangle, this.Desktop.Scale);
			this.Desktop.Batcher2D.PopScissor();
			bool flag = this._effectPatch != null;
			if (flag)
			{
				byte a = byte.MaxValue;
				bool flag2 = this.Value < 0.2f;
				if (flag2)
				{
					a = (byte)(this.Value / 0.2f * 255f);
				}
				else
				{
					bool flag3 = this.Value > 0.8f;
					if (flag3)
					{
						a = (byte)((1f - this.Value) / 0.19999999f * 255f);
					}
				}
				this.Desktop.Batcher2D.RequestDrawPatch(this._effectPatch.TextureArea.Texture, this._effectPatch.TextureArea.Rectangle, this._effectPatch.HorizontalBorder, this._effectPatch.VerticalBorder, this._effectPatch.TextureArea.Scale, this._effectRectangle, 0f, UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, a));
			}
		}

		// Token: 0x04001C49 RID: 7241
		[UIMarkupProperty]
		public UIPath BarTexturePath;

		// Token: 0x04001C4A RID: 7242
		[UIMarkupProperty]
		public UIPath EffectTexturePath;

		// Token: 0x04001C4B RID: 7243
		[UIMarkupProperty]
		public int EffectWidth;

		// Token: 0x04001C4C RID: 7244
		[UIMarkupProperty]
		public int EffectHeight;

		// Token: 0x04001C4D RID: 7245
		[UIMarkupProperty]
		public int EffectOffset;

		// Token: 0x04001C4E RID: 7246
		[UIMarkupProperty]
		public ProgressBar.ProgressBarAlignment Alignment = ProgressBar.ProgressBarAlignment.Horizontal;

		// Token: 0x04001C4F RID: 7247
		[UIMarkupProperty]
		public ProgressBar.ProgressBarDirection Direction = ProgressBar.ProgressBarDirection.End;

		// Token: 0x04001C50 RID: 7248
		private TexturePatch _barPatch;

		// Token: 0x04001C51 RID: 7249
		private TexturePatch _effectPatch;

		// Token: 0x04001C52 RID: 7250
		private Rectangle _visibleBarRectangle;

		// Token: 0x04001C53 RID: 7251
		private Rectangle _effectRectangle;

		// Token: 0x04001C54 RID: 7252
		[UIMarkupProperty]
		public float Value;

		// Token: 0x02000D3E RID: 3390
		public enum ProgressBarAlignment
		{
			// Token: 0x04004144 RID: 16708
			Vertical,
			// Token: 0x04004145 RID: 16709
			Horizontal
		}

		// Token: 0x02000D3F RID: 3391
		public enum ProgressBarDirection
		{
			// Token: 0x04004147 RID: 16711
			Start,
			// Token: 0x04004148 RID: 16712
			End
		}
	}
}
