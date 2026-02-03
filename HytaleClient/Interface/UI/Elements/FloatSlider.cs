using System;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200085F RID: 2143
	[UIMarkupElement]
	public class FloatSlider : InputElement<float>
	{
		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x00091A54 File Offset: 0x0008FC54
		// (set) Token: 0x06003BF2 RID: 15346 RVA: 0x00091A6D File Offset: 0x0008FC6D
		public override float Value
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

		// Token: 0x06003BF3 RID: 15347 RVA: 0x00091A78 File Offset: 0x0008FC78
		public FloatSlider(Desktop desktop, Element parent) : base(desktop, parent)
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

		// Token: 0x06003BF4 RID: 15348 RVA: 0x00091B04 File Offset: 0x0008FD04
		protected override void ApplyStyles()
		{
			this._paddedBackgroundPatch = ((this.Style.Background != null) ? this.Desktop.MakeTexturePatch(this.Style.Background) : null);
			this._fill.Background = this.Style.Fill;
			this._handle.Anchor.Width = new int?(this.Style.HandleWidth);
			this._handle.Anchor.Height = new int?(this.Style.HandleHeight);
			this._handle.Background = this.Style.Handle;
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x00091BAC File Offset: 0x0008FDAC
		protected override void LayoutSelf()
		{
			int num = this.Desktop.UnscaleRound((float)this._rectangleAfterPadding.Width);
			int num2 = (int)Math.Round((double)((this.Value - this.Min) * (float)num / (this.Max - this.Min)), MidpointRounding.AwayFromZero);
			this._fill.Anchor.Width = new int?(num2);
			this._handle.Anchor.Left = new int?((int)((float)num2 - (float)this.Style.HandleWidth / 2f));
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x00091C3C File Offset: 0x0008FE3C
		protected override void AfterChildrenLayout()
		{
			this.UpdateHitboxRectangle();
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x00091C48 File Offset: 0x0008FE48
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

		// Token: 0x06003BF8 RID: 15352 RVA: 0x00091D5C File Offset: 0x0008FF5C
		protected override void ApplyParentScroll(Point scaledParentScroll)
		{
			base.ApplyParentScroll(scaledParentScroll);
			this.UpdateHitboxRectangle();
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x00091D6E File Offset: 0x0008FF6E
		public override Element HitTest(Point position)
		{
			return this._hitboxRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x00091D84 File Offset: 0x0008FF84
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = (long)evt.Button != 1L;
			if (!flag)
			{
				this._isDragging = true;
				this.SetValueFromMouseX(this.Desktop.MousePosition.X);
			}
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x00091DC4 File Offset: 0x0008FFC4
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

		// Token: 0x06003BFC RID: 15356 RVA: 0x00091E18 File Offset: 0x00090018
		protected override void OnMouseMove()
		{
			bool isDragging = this._isDragging;
			if (isDragging)
			{
				this.SetValueFromMouseX(this.Desktop.MousePosition.X);
			}
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00091E48 File Offset: 0x00090048
		private void SetValueFromMouseX(int x)
		{
			int num = this.Desktop.UnscaleRound((float)this._rectangleAfterPadding.Width);
			int num2 = MathHelper.Clamp(this.Desktop.UnscaleRound((float)(x - this._rectangleAfterPadding.X)), 0, num);
			float num3 = this.Min + (float)Math.Round((double)((float)num2 * (this.Max - this.Min) / (float)num), 2);
			num3 = (float)Math.Round(Math.Round((double)(num3 / this.Step)) * (double)this.Step, 2);
			num3 = MathHelper.Clamp(num3, this.Min, this.Max);
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

		// Token: 0x06003BFE RID: 15358 RVA: 0x00091F24 File Offset: 0x00090124
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

		// Token: 0x06003BFF RID: 15359 RVA: 0x00091FA8 File Offset: 0x000901A8
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool flag = this._paddedBackgroundPatch != null;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._paddedBackgroundPatch, this._rectangleAfterPadding, this.Desktop.Scale);
			}
		}

		// Token: 0x04001BB1 RID: 7089
		[UIMarkupProperty]
		public float Min;

		// Token: 0x04001BB2 RID: 7090
		[UIMarkupProperty]
		public float Max;

		// Token: 0x04001BB3 RID: 7091
		[UIMarkupProperty]
		public float Step = 1f;

		// Token: 0x04001BB4 RID: 7092
		private float _value;

		// Token: 0x04001BB5 RID: 7093
		[UIMarkupProperty]
		public SliderStyle Style = SliderStyle.MakeDefault();

		// Token: 0x04001BB6 RID: 7094
		public Action MouseButtonReleased;

		// Token: 0x04001BB7 RID: 7095
		private readonly Group _fill;

		// Token: 0x04001BB8 RID: 7096
		private readonly Group _handle;

		// Token: 0x04001BB9 RID: 7097
		private Rectangle _hitboxRectangle;

		// Token: 0x04001BBA RID: 7098
		private bool _isDragging;

		// Token: 0x04001BBB RID: 7099
		private TexturePatch _paddedBackgroundPatch;
	}
}
