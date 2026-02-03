using System;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x0200089A RID: 2202
	internal class ItemQuantityPopup : Element
	{
		// Token: 0x06003F9C RID: 16284 RVA: 0x000B34C4 File Offset: 0x000B16C4
		public ItemQuantityPopup(InGameView inGame) : base(inGame.Desktop, null)
		{
			this._inGameView = inGame;
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x000B34DC File Offset: 0x000B16DC
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Pages/Inventory/ItemQuantityPopup.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._container = uifragment.Get<Group>("Container");
			this._itemIcon = uifragment.Get<ItemGrid>("ItemIcon");
			this._itemIcon.AreItemsDraggable = false;
			this._itemIcon.Slots = new ItemGridSlot[1];
			this._itemIcon.Style.SlotBackground = null;
			uifragment.Get<Button>("ConfirmButton").Activating = new Action(this.Validate);
			uifragment.Get<Button>("CancelButton").Activating = new Action(this.Dismiss);
			this._numberField = uifragment.Get<NumberField>("NumberField");
			this._numberField.ValueChanged = new Action(this.OnNumberFieldChanged);
			this._slider = uifragment.Get<Slider>("Slider");
			this._slider.ValueChanged = new Action(this.OnSliderChanged);
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x000B35F1 File Offset: 0x000B17F1
		protected override void OnUnmounted()
		{
			this._callback = null;
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x000B35FC File Offset: 0x000B17FC
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x000B361C File Offset: 0x000B181C
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			base.OnKeyDown(keycode, repeat);
			if (keycode != SDL.SDL_Keycode.SDLK_DOWN)
			{
				if (keycode == SDL.SDL_Keycode.SDLK_UP)
				{
					bool flag = this._quantity >= this._maxQuantity;
					if (!flag)
					{
						this._quantity++;
						this._slider.Value = this._quantity;
						this._slider.Layout(null, true);
						this._numberField.Value = this._quantity;
						this._itemIcon.Slots[0].ItemStack.Quantity = this._quantity;
					}
				}
			}
			else
			{
				bool flag2 = this._quantity <= 0;
				if (!flag2)
				{
					this._quantity--;
					this._slider.Value = this._quantity;
					this._slider.Layout(null, true);
					this._numberField.Value = this._quantity;
					this._itemIcon.Slots[0].ItemStack.Quantity = this._quantity;
				}
			}
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x000B375C File Offset: 0x000B195C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = !this._container.RectangleAfterPadding.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x000B3798 File Offset: 0x000B1998
		private void OnNumberFieldChanged()
		{
			bool flag = !this._numberField.HasValidValue;
			if (!flag)
			{
				this._slider.Value = (int)this._numberField.Value;
				this._slider.Layout(null, true);
				this._quantity = this._slider.Value;
				this._itemIcon.Slots[0].ItemStack.Quantity = this._quantity;
			}
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x000B381C File Offset: 0x000B1A1C
		private void OnSliderChanged()
		{
			this._numberField.Value = this._slider.Value;
			this._quantity = this._slider.Value;
			this._itemIcon.Slots[0].ItemStack.Quantity = this._quantity;
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x000B3874 File Offset: 0x000B1A74
		protected internal override void Validate()
		{
			this._callback(this._quantity);
			this.Desktop.SetTransientLayer(null);
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x000B3896 File Offset: 0x000B1A96
		protected internal override void Dismiss()
		{
			this._callback(0);
			this.Desktop.SetTransientLayer(null);
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x000B38B4 File Offset: 0x000B1AB4
		public void Open(Point slotPosition, int maxQuantity, int startingQuantity, string itemId, Action<int> callback)
		{
			this._callback = callback;
			this._quantity = startingQuantity;
			this._maxQuantity = maxQuantity;
			this._slider.Max = maxQuantity;
			this._slider.Value = startingQuantity;
			this._numberField.Format.MaxValue = maxQuantity;
			this._numberField.Value = startingQuantity;
			this._itemIcon.SetItemStacks(new ClientItemStack[]
			{
				new ClientItemStack(itemId, startingQuantity)
			}, 0);
			this.Anchor.Left = slotPosition.X - this._container.Anchor.Width / 2;
			int? num = slotPosition.Y + this._container.Padding.Top;
			int? height = this._container.Anchor.Height;
			this.Anchor.Top = ((num != null & height != null) ? new int?(num.GetValueOrDefault() - height.GetValueOrDefault() - 8) : null);
			this.Desktop.SetTransientLayer(this);
		}

		// Token: 0x04001E34 RID: 7732
		private readonly InGameView _inGameView;

		// Token: 0x04001E35 RID: 7733
		private int _maxQuantity;

		// Token: 0x04001E36 RID: 7734
		private int _quantity;

		// Token: 0x04001E37 RID: 7735
		private NumberField _numberField;

		// Token: 0x04001E38 RID: 7736
		private Slider _slider;

		// Token: 0x04001E39 RID: 7737
		private Group _container;

		// Token: 0x04001E3A RID: 7738
		private ItemGrid _itemIcon;

		// Token: 0x04001E3B RID: 7739
		private Action<int> _callback;
	}
}
