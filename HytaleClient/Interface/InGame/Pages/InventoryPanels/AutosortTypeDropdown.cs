using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels
{
	// Token: 0x02000891 RID: 2193
	internal class AutosortTypeDropdown : Element
	{
		// Token: 0x06003EE8 RID: 16104 RVA: 0x000AC2E8 File Offset: 0x000AA4E8
		public AutosortTypeDropdown(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x000AC2F4 File Offset: 0x000AA4F4
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Pages/Inventory/AutosortTypeDropdown.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._dropdown = uifragment.Get<DropdownBox>("AutosortTypeDropdown");
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>();
			foreach (object obj in Enum.GetValues(typeof(SortType)))
			{
				SortType sortType = (SortType)obj;
				string text = this.Desktop.Provider.GetText(string.Format("ui.windows.autoSort.types.{0}", sortType), null, true);
				list.Add(new DropdownBox.DropdownEntryInfo(text, sortType.ToString(), sortType == this.SortType));
			}
			this._dropdown.Entries = list;
			this._dropdown.ValueChanged = delegate()
			{
				this.SetType(this._dropdown.SelectedValues[0]);
			};
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x000AC40C File Offset: 0x000AA60C
		public void SetType(string type)
		{
			this.SetType((SortType)Enum.Parse(typeof(SortType), type));
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x000AC42A File Offset: 0x000AA62A
		public void SetType(SortType type)
		{
			this.SortType = type;
			this.SortTypeChanged();
		}

		// Token: 0x04001DB1 RID: 7601
		private DropdownBox _dropdown;

		// Token: 0x04001DB2 RID: 7602
		public SortType SortType;

		// Token: 0x04001DB3 RID: 7603
		public Action SortTypeChanged;
	}
}
