using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.InGame.Modules;
using HytaleClient.InGame.Modules.BuilderTools.Tools;
using HytaleClient.Interface.InGame.Pages.InventoryPanels;
using HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Interface.InGame.Pages
{
	// Token: 0x02000890 RID: 2192
	internal class ToolsSettingsPage : HytaleClient.Interface.InGame.Pages.InventoryPanels.Panel
	{
		// Token: 0x06003EDF RID: 16095 RVA: 0x000ABD08 File Offset: 0x000A9F08
		public ToolsSettingsPage(InGameView inGameView, Element parent = null) : base(inGameView, parent)
		{
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x000ABD24 File Offset: 0x000A9F24
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/ToolsSettings.ui", out document);
			this._fragment = document.Instantiate(this.Desktop, this);
			Group group = this._fragment.Get<Group>("ToolPanel");
			this._builderToolPanel = new BuilderToolPanel(this._inGameView, null);
			this._builderToolPanel.Build();
			group.Add(this._builderToolPanel, -1);
			Group group2 = this._fragment.Get<Group>("SelectionCommandsPanelContainer");
			this._selectionCommandsPanel = new SelectionCommandsPanel(this._inGameView, null);
			this._selectionCommandsPanel.Build();
			group2.Add(this._selectionCommandsPanel, -1);
			this._selectionCommandsPanel.Visible = false;
			this._builderToolPanel.Visible = false;
			this._shapeToolSelector = this._fragment.Get<Group>("ShapeToolSelector");
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x000ABE0C File Offset: 0x000AA00C
		protected void OnToolSelected(int itemSlot)
		{
			ClientItemStack toolsItem = this._inGameView.InGame.Instance.InventoryModule.GetToolsItem(itemSlot);
			this._inGameView.InGame.Instance.BuilderToolsModule.TrySelectActiveTool(-8, itemSlot, toolsItem);
			this._inGameView.InGame.Instance.InventoryModule.SetActiveToolsSlot(itemSlot, true, true);
			this.OnPlayerCharacterItemChanged(ItemChangeType.Other);
			this._builderToolPanel.Visible = this._inGameView.InGame.Instance.BuilderToolsModule.HasConfigurationToolBrushDataOrArguments();
			this._selectionCommandsPanel.Visible = (toolsItem.Id == "EditorTool_PlaySelection");
			base.Layout(null, true);
		}

		// Token: 0x06003EE2 RID: 16098 RVA: 0x000ABED0 File Offset: 0x000AA0D0
		public void SelectToolById(string itemId)
		{
			ClientItemStack[] toolItemStacks = this._inGameView.InGame.Instance.InventoryModule.GetToolItemStacks();
			ClientItemStack clientItemStack = null;
			int slot = 0;
			for (int i = 0; i < toolItemStacks.Length; i++)
			{
				ClientItemStack clientItemStack2 = toolItemStacks[i];
				bool flag = ((clientItemStack2 != null) ? clientItemStack2.Id : null) == itemId;
				if (flag)
				{
					clientItemStack = toolItemStacks[i];
					slot = i;
					break;
				}
			}
			bool flag2 = clientItemStack == null;
			if (!flag2)
			{
				this._inGameView.InGame.Instance.BuilderToolsModule.TrySelectActiveTool(-8, slot, clientItemStack);
				this._inGameView.InGame.Instance.InventoryModule.SetActiveToolsSlot(slot, true, true);
				this.OnPlayerCharacterItemChanged(ItemChangeType.Other);
				this._builderToolPanel.Visible = this._inGameView.InGame.Instance.BuilderToolsModule.HasConfigurationToolBrushDataOrArguments();
				base.Layout(null, true);
			}
		}

		// Token: 0x06003EE3 RID: 16099 RVA: 0x000ABFC0 File Offset: 0x000AA1C0
		protected override void OnMounted()
		{
			DropdownBox dropdown = this._fragment.Get<DropdownBox>("Dropdown");
			dropdown.ValueChanged = delegate()
			{
				List<int> list2 = dropdown.selectedIndexes();
				int itemSlot = (list2.Count > 0) ? list2[0] : 0;
				this.OnToolSelected(itemSlot);
			};
			List<DropdownBox.DropdownEntryInfo> list = new List<DropdownBox.DropdownEntryInfo>();
			ClientItemStack activeToolsItem = this._inGameView.InGame.Instance.InventoryModule.GetActiveToolsItem();
			foreach (ClientItemStack clientItemStack in this._inGameView.InGame.Instance.InventoryModule.GetToolItemStacks())
			{
				bool flag = clientItemStack == null || !this._inGameView.Items.ContainsKey(clientItemStack.Id);
				if (!flag)
				{
					BuilderTool builderTool = this._inGameView.Items[clientItemStack.Id].BuilderTool;
					bool flag2 = builderTool == null;
					if (!flag2)
					{
						string text = this.Desktop.Provider.GetText("builderTools.tools." + builderTool.Id + ".name", null, true);
						list.Add(new DropdownBox.DropdownEntryInfo(text, clientItemStack.Id, false));
						bool flag3 = activeToolsItem != null && clientItemStack.Id == activeToolsItem.Id;
						if (flag3)
						{
							dropdown.SelectedValues = new List<string>
							{
								activeToolsItem.Id
							};
							this.OnToolSelected(this._inGameView.InGame.Instance.InventoryModule.GetActiveToolsSlot());
						}
					}
				}
			}
			dropdown.Entries = list;
		}

		// Token: 0x06003EE4 RID: 16100 RVA: 0x000AC16C File Offset: 0x000AA36C
		public bool ContainPosition(Point pos)
		{
			bool flag = this._shapeToolSelector.AnchoredRectangle.Contains(pos);
			bool flag2 = !this._builderToolPanel.Visible && !this._selectionCommandsPanel.Visible;
			bool result;
			if (flag2)
			{
				result = flag;
			}
			else
			{
				result = (flag || this._builderToolPanel.AnchoredRectangle.Contains(pos) || this._selectionCommandsPanel.AnchoredRectangle.Contains(pos));
			}
			return result;
		}

		// Token: 0x06003EE5 RID: 16101 RVA: 0x000AC1E9 File Offset: 0x000AA3E9
		protected override void OnUnmounted()
		{
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x000AC1EC File Offset: 0x000AA3EC
		private bool ContainsNonAssignedMaterialField(JObject jObject)
		{
			bool flag = jObject == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (KeyValuePair<string, JToken> keyValuePair in jObject)
				{
					string key = keyValuePair.Key;
					IEnumerator<KeyValuePair<string, JToken>> enumerator;
					keyValuePair = enumerator.Current;
					JToken value = keyValuePair.Value;
					bool flag2 = value is JObject;
					if (flag2)
					{
						bool flag3 = this.ContainsNonAssignedMaterialField((JObject)value);
						if (flag3)
						{
							return true;
						}
					}
					bool flag4 = key.Contains("Material");
					if (flag4)
					{
						return value == null || value.ToString().Equals("Empty");
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x000AC2A0 File Offset: 0x000AA4A0
		public void OnPlayerCharacterItemChanged(ItemChangeType changeType)
		{
			bool flag = changeType == ItemChangeType.Dropped;
			if (flag)
			{
				this.OnMounted();
			}
			else
			{
				ClientItemStack activeToolsItem = this._inGameView.InGame.Instance.InventoryModule.GetActiveToolsItem();
				this._builderToolPanel.ConfiguringToolChange(activeToolsItem);
			}
		}

		// Token: 0x04001DAD RID: 7597
		private UIFragment _fragment = null;

		// Token: 0x04001DAE RID: 7598
		private BuilderToolPanel _builderToolPanel = null;

		// Token: 0x04001DAF RID: 7599
		private Group _shapeToolSelector;

		// Token: 0x04001DB0 RID: 7600
		public SelectionCommandsPanel _selectionCommandsPanel;
	}
}
