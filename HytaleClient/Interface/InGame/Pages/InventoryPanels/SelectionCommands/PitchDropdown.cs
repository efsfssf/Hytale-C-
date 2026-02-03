using System;
using System.Collections.Generic;
using HytaleClient.Data.Map;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008A6 RID: 2214
	internal class PitchDropdown : Element
	{
		// Token: 0x06004024 RID: 16420 RVA: 0x000B7A41 File Offset: 0x000B5C41
		public PitchDropdown(InGameView inGameView, Desktop desktop) : base(desktop, null)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x000B7A84 File Offset: 0x000B5C84
		public void Build()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/PitchDropdown.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._pitchDropdown = uifragment.Get<DropdownBox>("PitchDropdown");
		}

		// Token: 0x06004026 RID: 16422 RVA: 0x000B7ACC File Offset: 0x000B5CCC
		public void SetPitchValues(string blockName)
		{
			bool flag = string.IsNullOrEmpty(blockName);
			if (flag)
			{
				blockName = "Empty";
			}
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>();
			List<string> list2 = new List<string>
			{
				"0"
			};
			ClientBlockType clientBlockTypeFromName = this._inGameView.InGame.Instance.MapModule.GetClientBlockTypeFromName(blockName);
			foreach (KeyValuePair<string, string> keyValuePair in this.PitchKeyValues)
			{
				bool flag2 = clientBlockTypeFromName.Variants.ContainsKey(keyValuePair.Key);
				if (flag2)
				{
					list2.Add(keyValuePair.Value);
				}
			}
			foreach (string text in list2)
			{
				list.Add(new DropdownBox.DropdownEntryInfo(text, text, false));
			}
			this._pitchDropdown.Entries = list;
			this._pitchDropdown.Value = list2[0];
		}

		// Token: 0x06004027 RID: 16423 RVA: 0x000B7BF8 File Offset: 0x000B5DF8
		public string GetCommandArg()
		{
			string text = "";
			bool flag = this._pitchDropdown.Value != "0";
			if (flag)
			{
				text = text + "|Pitch=" + this._pitchDropdown.Value;
			}
			return text;
		}

		// Token: 0x04001E89 RID: 7817
		private InGameView _inGameView;

		// Token: 0x04001E8A RID: 7818
		private DropdownBox _pitchDropdown;

		// Token: 0x04001E8B RID: 7819
		private readonly Dictionary<string, string> PitchKeyValues = new Dictionary<string, string>
		{
			{
				"Pitch=90",
				"90"
			},
			{
				"Pitch=180",
				"180"
			}
		};
	}
}
