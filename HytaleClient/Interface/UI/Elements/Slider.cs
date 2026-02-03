using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000873 RID: 2163
	[UIMarkupElement]
	public class Slider : InputElement<int>
	{
		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06003CFF RID: 15615 RVA: 0x0009AEF0 File Offset: 0x000990F0
		// (set) Token: 0x06003D00 RID: 15616 RVA: 0x0009AF09 File Offset: 0x00099109
		public override int Value
		{
			get
			{
				return MathHelper.Clamp(this._value, this.Min, this.Max);
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06003D01 RID: 15617 RVA: 0x0009AF14 File Offset: 0x00099114
		public Slider(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._fill = new Group(this.Desktop, this)
			{
				Anchor = new Anchor
				{
					Left = new int?(0)
				}
			};
			this._handle = new Group(this.Desktop, this)
			{
				Anchor = new Anchor
				{
					Left = new int?(0)
				}
			};
		}

		// Token: 0x06003D02 RID: 15618 RVA: 0x0009AF9C File Offset: 0x0009919C
		protected override void ApplyStyles()
		{
			this._paddedBackgroundPatch = ((this.Style.Background != null) ? this.Desktop.MakeTexturePatch(this.Style.Background) : null);
			this._fill.Background = this.Style.Fill;
			this._handle.Anchor.Width = new int?(this.Style.HandleWidth);
			this._handle.Anchor.Height = new int?(this.Style.HandleHeight);
			this._handle.Background = this.Style.Handle;
		}

		// Token: 0x06003D03 RID: 15619 RVA: 0x0009B044 File Offset: 0x00099244
		protected override void LayoutSelf()
		{
			int num = this.Desktop.UnscaleRound((float)this._rectangleAfterPadding.Width);
			int num2 = (int)Math.Round((double)((float)(this.Value - this.Min) * (float)num / (float)(this.Max - this.Min)), MidpointRounding.AwayFromZero);
			this._fill.Anchor.Width = new int?(num2);
			this._handle.Anchor.Left = new int?((int)((float)num2 - (float)this.Style.HandleWidth / 2f));
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x0009B0D5 File Offset: 0x000992D5
		protected override void AfterChildrenLayout()
		{
			this.UpdateHitboxRectangle();
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x0009B0E0 File Offset: 0x000992E0
		private void UpdateHitboxRectangle()
		{
			int num = this._anchoredRectangle.Width;
			bool flag = this._anchoredRectangle.X > this._handle.AnchoredRectangle.X;
			if (flag)
			{
				num += this._anchoredRectangle.X - this._handle.AnchoredRectangle.X;
			}
			else
			{
				bool flag2 = this._handle.AnchoredRectangle.Right > this._anchoredRectangle.Right;
				if (flag2)
				{
					num += this._handle.AnchoredRectangle.Right - this._anchoredRectangle.Right;
				}
			}
			this._hitboxRectangle = new Rectangle(Math.Min(this._anchoredRectangle.Left, this._handle.AnchoredRectangle.Left), Math.Min(this._anchoredRectangle.Top, this._handle.AnchoredRectangle.Top), num, Math.Max(this._anchoredRectangle.Height, this._handle.AnchoredRectangle.Height));
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x0009B1F4 File Offset: 0x000993F4
		protected override void ApplyParentScroll(Point scaledParentScroll)
		{
			base.ApplyParentScroll(scaledParentScroll);
			this.UpdateHitboxRectangle();
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x0009B206 File Offset: 0x00099406
		public override Element HitTest(Point position)
		{
			return this._hitboxRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x0009B21C File Offset: 0x0009941C
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = (long)evt.Button != 1L;
			if (!flag)
			{
				this._isDragging = true;
				this.SetValueFromMouseX(this.Desktop.MousePosition.X);
			}
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x0009B25C File Offset: 0x0009945C
		protected override void OnMouseEnter()
		{
			ButtonSounds sounds = this.Style.Sounds;
			bool flag = ((sounds != null) ? sounds.MouseHover : null) != null;
			if (flag)
			{
				IUIProvider provider = this.Desktop.Provider;
				ButtonSounds sounds2 = this.Style.Sounds;
				provider.PlaySound((sounds2 != null) ? sounds2.MouseHover : null);
			}
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x0009B2B0 File Offset: 0x000994B0
		protected override void OnMouseMove()
		{
			bool isDragging = this._isDragging;
			if (isDragging)
			{
				this.SetValueFromMouseX(this.Desktop.MousePosition.X);
			}
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x0009B2E0 File Offset: 0x000994E0
		private void SetValueFromMouseX(int x)
		{
			int num = this.Desktop.UnscaleRound((float)this._rectangleAfterPadding.Width);
			int num2 = MathHelper.Clamp(this.Desktop.UnscaleRound((float)(x - this._rectangleAfterPadding.X)), 0, num);
			int num3 = MathHelper.Clamp(this.Min + (int)(Math.Round((double)((float)num2 * (float)(this.Max - this.Min) / (float)num / (float)this.Step)) * (double)this.Step), this.Min, this.Max);
			bool flag = this.Value != num3;
			if (flag)
			{
				this.Value = num3;
				base.Layout(null, true);
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x0009B3AC File Offset: 0x000995AC
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = (long)evt.Button != 1L;
			if (!flag)
			{
				ButtonSounds sounds = this.Style.Sounds;
				bool flag2 = ((sounds != null) ? sounds.Activate : null) != null;
				if (flag2)
				{
					IUIProvider provider = this.Desktop.Provider;
					ButtonSounds sounds2 = this.Style.Sounds;
					provider.PlaySound((sounds2 != null) ? sounds2.Activate : null);
				}
				this._isDragging = false;
				Action mouseButtonReleased = this.MouseButtonReleased;
				if (mouseButtonReleased != null)
				{
					mouseButtonReleased();
				}
			}
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x0009B430 File Offset: 0x00099630
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool flag = this._paddedBackgroundPatch != null;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._paddedBackgroundPatch, this._rectangleAfterPadding, this.Desktop.Scale);
			}
		}

		// Token: 0x04001C62 RID: 7266
		[UIMarkupProperty]
		public int Min;

		// Token: 0x04001C63 RID: 7267
		[UIMarkupProperty]
		public int Max;

		// Token: 0x04001C64 RID: 7268
		[UIMarkupProperty]
		public int Step = 1;

		// Token: 0x04001C65 RID: 7269
		private int _value;

		// Token: 0x04001C66 RID: 7270
		[UIMarkupProperty]
		public SliderStyle Style = SliderStyle.MakeDefault();

		// Token: 0x04001C67 RID: 7271
		public Action MouseButtonReleased;

		// Token: 0x04001C68 RID: 7272
		private readonly Group _fill;

		// Token: 0x04001C69 RID: 7273
		private readonly Group _handle;

		// Token: 0x04001C6A RID: 7274
		private Rectangle _hitboxRectangle;

		// Token: 0x04001C6B RID: 7275
		private bool _isDragging;

		// Token: 0x04001C6C RID: 7276
		private TexturePatch _paddedBackgroundPatch;
	}
}
