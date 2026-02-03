using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000894 RID: 2196
	[UIMarkupElement]
	internal class BlockSelector : InputElement<string>
	{
		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06003F0C RID: 16140 RVA: 0x000ADD95 File Offset: 0x000ABF95
		// (set) Token: 0x06003F0D RID: 16141 RVA: 0x000ADD9D File Offset: 0x000ABF9D
		[UIMarkupProperty]
		public int Capacity
		{
			get
			{
				return this._capacity;
			}
			set
			{
				this._itemGrid.SlotsPerRow = value;
				this._capacity = value;
				this.InitialiseSlots();
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06003F0E RID: 16142 RVA: 0x000ADDBA File Offset: 0x000ABFBA
		// (set) Token: 0x06003F0F RID: 16143 RVA: 0x000ADDC2 File Offset: 0x000ABFC2
		public override string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				this.RebuildSlots();
			}
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x000ADDD4 File Offset: 0x000ABFD4
		public BlockSelector(Desktop desktop, Element parent) : base(desktop, parent)
		{
			IUIProvider provider = desktop.Provider;
			IUIProvider iuiprovider = provider;
			CustomUIProvider customUIProvider = iuiprovider as CustomUIProvider;
			if (customUIProvider == null)
			{
				Interface @interface = iuiprovider as Interface;
				if (@interface == null)
				{
					throw new Exception("IUIProvider must be of type CustomUIProvider or Interface");
				}
				this._inGameView = @interface.InGameView;
			}
			else
			{
				this._inGameView = customUIProvider.Interface.InGameView;
			}
			this._itemGrid = new ItemGrid(this.Desktop, this)
			{
				SlotMouseEntered = new Action<int>(this.SlotMouseEntered),
				SlotMouseExited = new Action<int>(this.SlotMouseExited),
				SlotMouseDragCompleted = delegate(int dragSlotIndex, Element element, ItemGrid.ItemDragData dragData)
				{
					this.SlotMouseDragCompleted(dragSlotIndex);
				},
				SlotMouseDragExited = new Action<int, int>(this.SlotMouseDragExited),
				Dropped = new Action<int, Element, ItemGrid.ItemDragData>(this.Dropped),
				SlotClicking = new Action<int, int>(this.SlotClicking)
			};
			this.InitialiseSlots();
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x000ADED8 File Offset: 0x000AC0D8
		private void InitialiseSlots()
		{
			this._itemGrid.Slots = new ItemGridSlot[this.Capacity];
			for (int i = 0; i < this.Capacity; i++)
			{
				this._itemGrid.Slots[i] = new ItemGridSlot();
			}
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x000ADF24 File Offset: 0x000AC124
		public override Point ComputeScaledMinSize(int? maxWidth, int? maxHeight)
		{
			this.ApplyStyles();
			return base.ComputeScaledMinSize(maxWidth, maxHeight);
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x000ADF45 File Offset: 0x000AC145
		protected override void ApplyStyles()
		{
			this._itemGrid.Style = this.Style.ItemGridStyle;
			base.ApplyStyles();
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x000ADF65 File Offset: 0x000AC165
		protected override void OnMounted()
		{
			this.UpdateDropSlot();
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x000ADF70 File Offset: 0x000AC170
		private void SlotMouseEntered(int slotIndex)
		{
			ItemGridSlot itemGridSlot = this._itemGrid.Slots[slotIndex];
			bool flag = this.Desktop.IsMouseDragging && itemGridSlot.ItemStack == null;
			if (flag)
			{
				this._itemGrid.Slots[this._dropSlotIndex].Overlay = this.Style.SlotHoverOverlay;
			}
			else
			{
				bool flag2 = itemGridSlot.ItemStack != null;
				if (flag2)
				{
					itemGridSlot.Overlay = (this.Desktop.IsMouseDragging ? this.Style.SlotHoverOverlay : this.Style.SlotDeleteIcon);
				}
			}
			this._itemGrid.Layout(null, true);
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x000AE020 File Offset: 0x000AC220
		private void SlotMouseExited(int slotIndex)
		{
			bool flag = this._dropSlotIndex > -1;
			if (flag)
			{
				this._itemGrid.Slots[this._dropSlotIndex].Overlay = null;
			}
			bool flag2 = slotIndex > -1;
			if (flag2)
			{
				this._itemGrid.Slots[slotIndex].Overlay = null;
			}
			this._itemGrid.Layout(null, true);
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x000AE084 File Offset: 0x000AC284
		private void SlotMouseDragCompleted(int dragSlotIndex)
		{
			bool isSwapping = this._isSwapping;
			if (isSwapping)
			{
				this._isSwapping = false;
			}
			else
			{
				for (int i = dragSlotIndex; i < this._itemGrid.Slots.Length - 1; i++)
				{
					this._itemGrid.Slots[i].ItemStack = this._itemGrid.Slots[i + 1].ItemStack;
					this._itemGrid.Slots[i + 1].ItemStack = null;
					bool flag = this._itemGrid.Slots[i].ItemStack == null;
					if (flag)
					{
						break;
					}
				}
				foreach (ItemGridSlot itemGridSlot in this._itemGrid.Slots)
				{
					itemGridSlot.Overlay = null;
				}
				this.UpdateDropSlot();
				this.UpdateValueFromSlots();
			}
		}

		// Token: 0x06003F18 RID: 16152 RVA: 0x000AE160 File Offset: 0x000AC360
		private void SlotMouseDragExited(int dragSlotIndex, int mouseOverSlotIndex)
		{
			foreach (ItemGridSlot itemGridSlot in this._itemGrid.Slots)
			{
				itemGridSlot.Overlay = null;
			}
		}

		// Token: 0x06003F19 RID: 16153 RVA: 0x000AE194 File Offset: 0x000AC394
		private void Dropped(int slotIndex, Element element, ItemGrid.ItemDragData dragData)
		{
			ClientItemBase clientItemBase = this._inGameView.Items[dragData.ItemStack.Id];
			bool flag = clientItemBase.BlockId == 0;
			if (!flag)
			{
				bool flag2 = this._itemGrid.Slots[slotIndex].ItemStack != null;
				if (flag2)
				{
					bool flag3 = element == this._itemGrid;
					if (flag3)
					{
						this._itemGrid.Slots[dragData.ItemGridIndex].ItemStack = this._itemGrid.Slots[slotIndex].ItemStack;
						this._isSwapping = true;
					}
					this._itemGrid.Slots[slotIndex].ItemStack = dragData.ItemStack;
				}
				else
				{
					this._itemGrid.Slots[this._dropSlotIndex].ItemStack = dragData.ItemStack;
				}
				this.UpdateDropSlot();
				this.UpdateValueFromSlots();
			}
		}

		// Token: 0x06003F1A RID: 16154 RVA: 0x000AE271 File Offset: 0x000AC471
		private void SlotClicking(int index, int button)
		{
			this.EmptySlot(index);
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x000AE27C File Offset: 0x000AC47C
		public void EmptySlot(int index)
		{
			bool flag = this._dropSlotIndex == index || this._itemGrid.Slots[index].ItemStack == null;
			if (!flag)
			{
				this._itemGrid.Slots[index].ItemStack = null;
				this.UpdateValueFromSlots();
				this.RebuildSlots();
			}
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x000AE2D2 File Offset: 0x000AC4D2
		public void Reset()
		{
			this.Value = "";
			Action valueChanged = this.ValueChanged;
			if (valueChanged != null)
			{
				valueChanged();
			}
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x000AE2F4 File Offset: 0x000AC4F4
		private void UpdateValueFromSlots()
		{
			List<string> list = new List<string>();
			foreach (ItemGridSlot itemGridSlot in this._itemGrid.Slots)
			{
				bool flag = itemGridSlot.ItemStack == null;
				if (!flag)
				{
					list.Add(itemGridSlot.ItemStack.Id);
				}
			}
			this._value = string.Join(",", list);
			Action valueChanged = this.ValueChanged;
			if (valueChanged != null)
			{
				valueChanged();
			}
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x000AE370 File Offset: 0x000AC570
		private void RebuildSlots()
		{
			this.InitialiseSlots();
			bool flag = this._value == null;
			if (!flag)
			{
				string[] array = this._value.Split(new char[]
				{
					','
				});
				int num = 0;
				while (num < array.Length && num < this._itemGrid.Slots.Length)
				{
					string[] array2 = array[num].Split(new char[]
					{
						'%'
					});
					string key = array2[array2.Length - 1];
					ClientItemBase clientItemBase;
					bool flag2 = this._inGameView.Items.TryGetValue(key, out clientItemBase);
					if (flag2)
					{
						this._itemGrid.Slots[num] = new ItemGridSlot(new ClientItemStack(clientItemBase.Id, 1));
					}
					num++;
				}
				this.UpdateDropSlot();
			}
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x000AE438 File Offset: 0x000AC638
		private void UpdateDropSlot()
		{
			foreach (ItemGridSlot itemGridSlot in this._itemGrid.Slots)
			{
				itemGridSlot.Icon = null;
			}
			int dropSlotIndex = -1;
			for (int j = 0; j < this._itemGrid.Slots.Length; j++)
			{
				ItemGridSlot itemGridSlot2 = this._itemGrid.Slots[j];
				bool flag = itemGridSlot2.ItemStack != null;
				if (!flag)
				{
					itemGridSlot2.Icon = this.Style.SlotDropIcon;
					dropSlotIndex = j;
					this._itemGrid.Slots[j] = itemGridSlot2;
					break;
				}
			}
			this._dropSlotIndex = dropSlotIndex;
			this._itemGrid.Layout(null, true);
		}

		// Token: 0x04001DD0 RID: 7632
		private readonly InGameView _inGameView;

		// Token: 0x04001DD1 RID: 7633
		private ItemGrid _itemGrid;

		// Token: 0x04001DD2 RID: 7634
		private int _capacity;

		// Token: 0x04001DD3 RID: 7635
		private int _dropSlotIndex;

		// Token: 0x04001DD4 RID: 7636
		private bool _isSwapping;

		// Token: 0x04001DD5 RID: 7637
		[UIMarkupProperty]
		public BlockSelector.BlockSelectorStyle Style = new BlockSelector.BlockSelectorStyle();

		// Token: 0x04001DD6 RID: 7638
		private string _value = "";

		// Token: 0x02000D65 RID: 3429
		[UIMarkupData]
		public class BlockSelectorStyle
		{
			// Token: 0x040041D3 RID: 16851
			public ItemGrid.ItemGridStyle ItemGridStyle = new ItemGrid.ItemGridStyle();

			// Token: 0x040041D4 RID: 16852
			public PatchStyle SlotDropIcon;

			// Token: 0x040041D5 RID: 16853
			public PatchStyle SlotDeleteIcon;

			// Token: 0x040041D6 RID: 16854
			public PatchStyle SlotHoverOverlay;
		}
	}
}
