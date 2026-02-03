using System;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B1 RID: 2225
	internal abstract class BaseItemSlotSelector : InterfaceComponent
	{
		// Token: 0x0600406A RID: 16490 RVA: 0x000B9660 File Offset: 0x000B7860
		public BaseItemSlotSelector(InGameView inGameView, Element parent, bool enableEmptySlot) : base(inGameView.Interface, parent)
		{
			this._inGameView = inGameView;
			this._enableEmptySlot = enableEmptySlot;
			this._itemStacks = new ClientItemStack[enableEmptySlot ? 4 : 5];
			this._itemIcons = new TextureArea[enableEmptySlot ? 4 : 5];
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x000B96D9 File Offset: 0x000B78D9
		protected override void OnMounted()
		{
			this._hoveredSlot = this.SelectedSlot;
			this.UpdateHoveredRotation();
			this._doneChange = false;
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x000B96F8 File Offset: 0x000B78F8
		protected override void OnUnmounted()
		{
			bool flag = !this._doneChange;
			if (flag)
			{
				this.OnSlotSelected(this._enableEmptySlot ? (this._hoveredSlot - 1) : this._hoveredSlot, false);
			}
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x000B9738 File Offset: 0x000B7938
		public ClientItemStack GetItemStack(int index)
		{
			bool flag = index >= this._itemStacks.Length;
			ClientItemStack result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this._itemStacks[index];
			}
			return result;
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x000B9768 File Offset: 0x000B7968
		public void SetItemStacks(ClientItemStack[] stacks)
		{
			this._itemStacks = stacks;
			for (int i = 0; i < this._itemStacks.Length; i++)
			{
				ClientItemStack clientItemStack = stacks[i];
				ClientItemBase clientItemBase;
				bool flag = clientItemStack == null || !this._inGameView.Items.TryGetValue(clientItemStack.Id, out clientItemBase) || clientItemBase.Icon == null;
				if (flag)
				{
					this._itemIcons[i] = null;
				}
				else
				{
					this._itemIcons[i] = this._inGameView.GetTextureAreaForItemIcon(clientItemBase.Icon);
				}
			}
			this.ItemStacksChanged();
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x000B97F8 File Offset: 0x000B79F8
		protected UIFragment Build(Document doc)
		{
			base.Clear();
			this.Background = doc.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "Overlay");
			this._containerBackgroundTexture = doc.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "Background");
			this._containerBackgroundAnchor = doc.ResolveNamedValue<Anchor>(this.Desktop.Provider, "Anchor");
			this._hoveredBackgroundTexture = doc.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "HoveredBackground");
			this._hoveredBackgroundAnchor = doc.ResolveNamedValue<Anchor>(this.Desktop.Provider, "HoveredBackgroundAnchor");
			this._selectedBackgroundTexture = doc.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "SelectedBackground");
			this._selectedBackgroundAnchor = doc.ResolveNamedValue<Anchor>(this.Desktop.Provider, "SelectedBackgroundAnchor");
			this._itemIconSize = doc.ResolveNamedValue<int>(this.Desktop.Provider, "ItemIconSize");
			this._itemIconOffset = doc.ResolveNamedValue<int>(this.Desktop.Provider, "ItemIconOffset");
			this._sliceAngle = (float)doc.ResolveNamedValue<int>(this.Desktop.Provider, "SliceAngle");
			doc.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "HoverSound", out this._hoverSound);
			return doc.Instantiate(this.Desktop, this);
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x000B9954 File Offset: 0x000B7B54
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._quantityFont = this.Desktop.Provider.GetFontFamily("Default").BoldFont;
			this._containerBackgroundPatch = this.Desktop.MakeTexturePatch(this._containerBackgroundTexture);
			this._hoveredBackgroundPatch = this.Desktop.MakeTexturePatch(this._hoveredBackgroundTexture);
			this._selectedBackgroundPatch = this.Desktop.MakeTexturePatch(this._selectedBackgroundTexture);
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x000B99D0 File Offset: 0x000B7BD0
		protected override void LayoutSelf()
		{
			int num = this.Desktop.ScaleRound((float)this._containerBackgroundAnchor.Width.GetValueOrDefault());
			int num2 = this.Desktop.ScaleRound((float)this._containerBackgroundAnchor.Height.GetValueOrDefault());
			float num3 = (float)this._rectangleAfterPadding.Left + (float)this._rectangleAfterPadding.Width / 2f - (float)num / 2f;
			float num4 = (float)this._rectangleAfterPadding.Top + (float)this._rectangleAfterPadding.Height / 2f - (float)num2 / 2f;
			this._containerBackgroundRectangle = new Rectangle((int)num3, (int)num4, num, num2);
			int num5 = this.Desktop.ScaleRound((float)this._hoveredBackgroundAnchor.Width.Value);
			int x = this._rectangleAfterPadding.Left + this._rectangleAfterPadding.Width / 2 - num5 / 2;
			int y = this._rectangleAfterPadding.Top + this._rectangleAfterPadding.Height / 2 + this.Desktop.ScaleRound((float)this._hoveredBackgroundAnchor.Top.Value);
			this._hoveredBackgroundRectangle = new Rectangle(x, y, num5, this.Desktop.ScaleRound((float)this._hoveredBackgroundAnchor.Height.Value));
			int num6 = this.Desktop.ScaleRound((float)this._selectedBackgroundAnchor.Width.Value);
			int x2 = this._rectangleAfterPadding.Left + this._rectangleAfterPadding.Width / 2 - num6 / 2;
			int y2 = this._rectangleAfterPadding.Top + this._rectangleAfterPadding.Height / 2 + this.Desktop.ScaleRound((float)this._selectedBackgroundAnchor.Top.Value);
			this._selectedBackgroundRectangle = new Rectangle(x2, y2, num6, this.Desktop.ScaleRound((float)this._selectedBackgroundAnchor.Height.Value));
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x000B9BD3 File Offset: 0x000B7DD3
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x000B9BE8 File Offset: 0x000B7DE8
		protected void RefreshHoveredSlot()
		{
			Point point = new Point(this.Desktop.MousePosition.X - this._rectangleAfterPadding.Left, this.Desktop.MousePosition.Y - this._rectangleAfterPadding.Top);
			Point point2 = new Point(point.X - this._rectangleAfterPadding.Width / 2, point.Y - this._rectangleAfterPadding.Height / 2);
			bool flag = point2.X * point2.X + point2.Y * point2.Y < 625;
			if (!flag)
			{
				this._mousePointerAngle = MathHelper.ToDegrees((float)Math.Atan2((double)point2.Y, (double)point2.X));
				float num = this._mousePointerAngle - (90f - this._sliceAngle / 2f);
				bool flag2 = num < 0f;
				if (flag2)
				{
					num += 360f;
				}
				num %= 360f;
				int hoveredSlot = this._hoveredSlot;
				this._hoveredSlot = ((num < this._sliceAngle) ? 0 : ((int)((num - this._sliceAngle) / this._sliceAngle + 1f)));
				bool flag3 = this._hoveredSlot != hoveredSlot;
				if (flag3)
				{
					this.OnSlotHovered();
					this.UpdateHoveredRotation();
				}
			}
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x000B9D3A File Offset: 0x000B7F3A
		protected override void OnMouseMove()
		{
			this.RefreshHoveredSlot();
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x000B9D43 File Offset: 0x000B7F43
		private void UpdateHoveredRotation()
		{
			this._hoveredRotation = ((this._hoveredSlot == 0) ? 0f : ((float)this._hoveredSlot * this._sliceAngle));
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x000B9D6C File Offset: 0x000B7F6C
		public void ResetState()
		{
			for (int i = 0; i < this._itemStacks.Length; i++)
			{
				this._itemIcons[i] = null;
				this._itemStacks[i] = null;
			}
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x000B9DA8 File Offset: 0x000B7FA8
		protected static void CreateRotationMatrixAroundPoint(int x, int y, int width, float yaw, out Matrix matrix)
		{
			yaw = MathHelper.ToRadians(yaw);
			Matrix.CreateTranslation((float)x + (float)width / 2f, (float)y, 0f, out matrix);
			Matrix matrix2;
			Matrix.CreateRotationZ(yaw, out matrix2);
			Matrix.Multiply(ref matrix2, ref matrix, out matrix);
			Matrix.CreateTranslation((float)(-(float)x) - (float)width / 2f, (float)(-(float)y), 0f, out matrix2);
			Matrix.Multiply(ref matrix2, ref matrix, out matrix);
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x000B9E18 File Offset: 0x000B8018
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			this.Desktop.Batcher2D.RequestDrawPatch(this._containerBackgroundPatch, this._containerBackgroundRectangle, this.Desktop.Scale);
			int num = this.Desktop.ScaleRound((float)this._itemIconSize);
			int num2 = this.Desktop.ScaleRound((float)this._itemIconSize * 1.1f);
			float yaw = (this.SelectedSlot == 0) ? 0f : ((float)this.SelectedSlot * this._sliceAngle);
			bool flag = this.SelectedSlot == 0;
			if (flag)
			{
				this.Desktop.Batcher2D.RequestDrawPatch(this._selectedBackgroundPatch, this._selectedBackgroundRectangle, this.Desktop.Scale);
			}
			else
			{
				Matrix transformationMatrix;
				BaseItemSlotSelector.CreateRotationMatrixAroundPoint(this._selectedBackgroundRectangle.X, this._rectangleAfterPadding.Y + this._rectangleAfterPadding.Height / 2, this._selectedBackgroundRectangle.Width, yaw, out transformationMatrix);
				this.Desktop.Batcher2D.SetTransformationMatrix(transformationMatrix);
				this.Desktop.Batcher2D.RequestDrawPatch(this._selectedBackgroundPatch, this._selectedBackgroundRectangle, this.Desktop.Scale);
				this.Desktop.Batcher2D.SetTransformationMatrix(Matrix.Identity);
			}
			bool flag2 = this._hoveredSlot != 0;
			if (flag2)
			{
				Matrix transformationMatrix;
				BaseItemSlotSelector.CreateRotationMatrixAroundPoint(this._hoveredBackgroundRectangle.X, this._rectangleAfterPadding.Y + this._rectangleAfterPadding.Height / 2, this._hoveredBackgroundRectangle.Width, this._hoveredRotation, out transformationMatrix);
				this.Desktop.Batcher2D.SetTransformationMatrix(transformationMatrix);
				this.Desktop.Batcher2D.RequestDrawPatch(this._hoveredBackgroundPatch, this._hoveredBackgroundRectangle, this.Desktop.Scale);
				this.Desktop.Batcher2D.SetTransformationMatrix(Matrix.Identity);
			}
			else
			{
				this.Desktop.Batcher2D.SetTransformationMatrix(Matrix.Identity);
				this.Desktop.Batcher2D.RequestDrawPatch(this._hoveredBackgroundPatch, this._hoveredBackgroundRectangle, this.Desktop.Scale);
			}
			int num3 = this.Desktop.ScaleRound((float)this._itemIconOffset);
			for (int i = 0; i < this._itemStacks.Length; i++)
			{
				bool flag3 = this._itemStacks[i] == null;
				if (!flag3)
				{
					float num4 = 90f + (float)i * this._sliceAngle;
					bool enableEmptySlot = this._enableEmptySlot;
					if (enableEmptySlot)
					{
						num4 += this._sliceAngle;
					}
					float num5 = MathHelper.ToRadians(num4);
					double num6 = (double)((float)this._rectangleAfterPadding.Left + (float)this._rectangleAfterPadding.Width / 2f) + Math.Cos((double)num5) * (double)num3 - (double)((float)num / 2f);
					double num7 = (double)((float)this._rectangleAfterPadding.Top + (float)this._rectangleAfterPadding.Height / 2f) + Math.Sin((double)num5) * (double)num3 - (double)((float)num / 2f);
					bool flag4 = this._itemIcons[i] != null;
					if (flag4)
					{
						bool flag5 = i == (this._enableEmptySlot ? (this._hoveredSlot - 1) : this._hoveredSlot);
						Rectangle destRect;
						if (flag5)
						{
							int num8 = (num2 - num) / 2;
							destRect = new Rectangle((int)num6 - num8, (int)num7 - num8, num2, num2);
						}
						else
						{
							destRect = new Rectangle((int)num6, (int)num7, num, num);
						}
						this.Desktop.Batcher2D.RequestDrawTexture(this._itemIcons[i].Texture, this._itemIcons[i].Rectangle, destRect, UInt32Color.White);
					}
					bool flag6 = this._itemStacks[i].Quantity > 1;
					if (flag6)
					{
						string text = this._itemStacks[i].Quantity.ToString();
						float x = (float)num6 + (float)num - (float)this.Desktop.ScaleRound(this._quantityFont.CalculateTextWidth(text) * this._quantityFontSize / (float)this._quantityFont.BaseSize);
						float y = (float)num7 + (float)num - 26f * this.Desktop.Scale;
						this.Desktop.Batcher2D.RequestDrawText(this._quantityFont, this._quantityFontSize * this.Desktop.Scale, text, new Vector3(x, y, 0f), UInt32Color.White, false, false, 0f);
					}
				}
			}
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x000BA2A4 File Offset: 0x000B84A4
		protected virtual void OnSlotHovered()
		{
			bool flag = this._hoverSound != null;
			if (flag)
			{
				this.Desktop.Provider.PlaySound(this._hoverSound);
			}
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x000BA2D6 File Offset: 0x000B84D6
		protected virtual void ItemStacksChanged()
		{
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x000BA2D9 File Offset: 0x000B84D9
		protected virtual void OnSlotSelected(int slot, bool click)
		{
		}

		// Token: 0x04001EB1 RID: 7857
		private const int DeadZone = 25;

		// Token: 0x04001EB2 RID: 7858
		private PatchStyle _containerBackgroundTexture;

		// Token: 0x04001EB3 RID: 7859
		protected Anchor _containerBackgroundAnchor;

		// Token: 0x04001EB4 RID: 7860
		private PatchStyle _hoveredBackgroundTexture;

		// Token: 0x04001EB5 RID: 7861
		private Anchor _hoveredBackgroundAnchor;

		// Token: 0x04001EB6 RID: 7862
		private PatchStyle _selectedBackgroundTexture;

		// Token: 0x04001EB7 RID: 7863
		private Anchor _selectedBackgroundAnchor;

		// Token: 0x04001EB8 RID: 7864
		private SoundStyle _hoverSound;

		// Token: 0x04001EB9 RID: 7865
		private TexturePatch _containerBackgroundPatch;

		// Token: 0x04001EBA RID: 7866
		private Rectangle _containerBackgroundRectangle;

		// Token: 0x04001EBB RID: 7867
		private TexturePatch _hoveredBackgroundPatch;

		// Token: 0x04001EBC RID: 7868
		private Rectangle _hoveredBackgroundRectangle;

		// Token: 0x04001EBD RID: 7869
		private TexturePatch _selectedBackgroundPatch;

		// Token: 0x04001EBE RID: 7870
		private Rectangle _selectedBackgroundRectangle;

		// Token: 0x04001EBF RID: 7871
		private Font _quantityFont;

		// Token: 0x04001EC0 RID: 7872
		private int _itemIconSize;

		// Token: 0x04001EC1 RID: 7873
		private int _itemIconOffset;

		// Token: 0x04001EC2 RID: 7874
		private float _sliceAngle;

		// Token: 0x04001EC3 RID: 7875
		protected float _mousePointerAngle;

		// Token: 0x04001EC4 RID: 7876
		protected bool _enableEmptySlot;

		// Token: 0x04001EC5 RID: 7877
		private int _capacity;

		// Token: 0x04001EC6 RID: 7878
		public int SelectedSlot = 0;

		// Token: 0x04001EC7 RID: 7879
		protected ClientItemStack[] _itemStacks;

		// Token: 0x04001EC8 RID: 7880
		private TextureArea[] _itemIcons;

		// Token: 0x04001EC9 RID: 7881
		protected bool _doneChange = false;

		// Token: 0x04001ECA RID: 7882
		protected int _hoveredSlot = 0;

		// Token: 0x04001ECB RID: 7883
		private float _hoveredRotation = 0f;

		// Token: 0x04001ECC RID: 7884
		protected float _quantityFontSize = 18f;

		// Token: 0x04001ECD RID: 7885
		protected readonly InGameView _inGameView;
	}
}
