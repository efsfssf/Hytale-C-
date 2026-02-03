using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Data.Items;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008A7 RID: 2215
	internal class ReplaceCommand : BaseSelectionCommand
	{
		// Token: 0x06004028 RID: 16424 RVA: 0x000B7C41 File Offset: 0x000B5E41
		public ReplaceCommand(InGameView inGameView, Desktop desktop, Element parent = null) : base(inGameView, desktop, parent)
		{
		}

		// Token: 0x06004029 RID: 16425 RVA: 0x000B7C5C File Offset: 0x000B5E5C
		public override void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Pages/Inventory/BuilderTools/CommandReplace.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._materialContainer = uifragment.Get<Group>("MaterialContainer");
			this.AddMaterial();
			this._addMaterialButton = uifragment.Get<TextButton>("AddMaterialButton");
			this._addMaterialButton.Activating = delegate()
			{
				this.AddMaterial();
				this.Parent.Parent.Layout(null, true);
			};
			this._replaceMaterialContainer = uifragment.Get<Group>("ReplaceMaterialContainer");
			this._replaceMaterial = new MaterialConfigurationInput(this._inGameView, this.Desktop, null, false, true, 1);
			this._replaceMaterial.Build();
			this._replaceMaterial.HideRemoveButton();
			this._replaceMaterialContainer.Add(this._replaceMaterial, -1);
			this._swapReplaceMaterialContainer = uifragment.Get<Group>("SwapReplaceMaterialContainer");
			this._swapReplaceMaterial = new MaterialConfigurationInput(this._inGameView, this.Desktop, null, false, false, 1);
			this._swapReplaceMaterial.Build();
			this._swapReplaceMaterial.HideRemoveButton();
			this._swapReplaceMaterialContainer.Add(this._swapReplaceMaterial, -1);
			this._swapMaterialContainer = uifragment.Get<Group>("SwapMaterialContainer");
			this._swapMaterial = new MaterialConfigurationInput(this._inGameView, this.Desktop, null, false, false, 7);
			this._swapMaterial.Build();
			this._swapMaterial.HideRemoveButton();
			this._swapMaterialContainer.Add(this._swapMaterial, -1);
			this._swapCheckbox = uifragment.Get<CheckBox>("SwapCheckbox");
			this._swapCheckbox.ValueChanged = delegate()
			{
				this.ToggleSwapMaterialVisibility();
			};
			this.ToggleSwapMaterialVisibility();
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x000B7E0C File Offset: 0x000B600C
		private void ToggleSwapMaterialVisibility()
		{
			bool value = this._swapCheckbox.Value;
			if (value)
			{
				this._swapMaterialContainer.Visible = true;
				this._swapReplaceMaterialContainer.Visible = true;
				this._replaceMaterialContainer.Visible = false;
				this._materialContainer.Visible = false;
				this._addMaterialButton.Visible = false;
				base.Layout(null, true);
			}
			else
			{
				this._swapMaterialContainer.Visible = false;
				this._swapReplaceMaterialContainer.Visible = false;
				this._replaceMaterialContainer.Visible = true;
				this._materialContainer.Visible = true;
				this._addMaterialButton.Visible = true;
				base.Layout(null, true);
			}
		}

		// Token: 0x0600402B RID: 16427 RVA: 0x000B7ED0 File Offset: 0x000B60D0
		private void AddMaterial()
		{
			MaterialConfigurationInput materialConfigurationInput = new MaterialConfigurationInput(this._inGameView, this.Desktop, new MaterialConfigurationInput.OnRemove(this.OnRemoveMaterial), false, true, 1);
			materialConfigurationInput.Build();
			this._materialContainer.Add(materialConfigurationInput, -1);
			this._materials.Add(materialConfigurationInput);
			this.ManageRemoveButtonVisibility();
		}

		// Token: 0x0600402C RID: 16428 RVA: 0x000B7F28 File Offset: 0x000B6128
		private void OnRemoveMaterial(MaterialConfigurationInput materialConfigurationInput)
		{
			bool flag = this._materials.Count <= 1;
			if (!flag)
			{
				this._materialContainer.Remove(materialConfigurationInput);
				this._materials.Remove(materialConfigurationInput);
				this.Parent.Parent.Layout(null, true);
				this.ManageRemoveButtonVisibility();
			}
		}

		// Token: 0x0600402D RID: 16429 RVA: 0x000B7F8C File Offset: 0x000B618C
		public override string GetChatCommand()
		{
			string text = "";
			bool value = this._swapCheckbox.Value;
			string result;
			if (value)
			{
				text = this.GetMaterialSet(this._swapMaterial.GetCommandArgs());
				result = "/replace --swap " + text + " " + this.GetMaterialSet(this._swapReplaceMaterial.GetCommandArgs());
			}
			else
			{
				foreach (MaterialConfigurationInput materialConfigurationInput in this._materials)
				{
					text += materialConfigurationInput.GetCommandArgs();
					bool flag = materialConfigurationInput != Enumerable.Last<MaterialConfigurationInput>(this._materials);
					if (flag)
					{
						text += ",";
					}
				}
				result = "/replace --exact " + text + " " + this._replaceMaterial.GetCommandArgs();
			}
			return result;
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x000B8080 File Offset: 0x000B6280
		private string GetMaterialSet(string materialsArgs)
		{
			string[] array = materialsArgs.Split(new char[]
			{
				','
			});
			string text = "";
			foreach (string text2 in array)
			{
				ClientItemBase item = this._inGameView.InGame.Instance.ItemLibraryModule.GetItem(text2);
				bool flag = item == null;
				if (!flag)
				{
					text += item.Set;
					bool flag2 = text2 != Enumerable.Last<string>(array);
					if (flag2)
					{
						text += ",";
					}
				}
			}
			return text;
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x000B8120 File Offset: 0x000B6320
		private void ManageRemoveButtonVisibility()
		{
			bool flag = this._materials.Count > 1;
			if (flag)
			{
				foreach (MaterialConfigurationInput materialConfigurationInput in this._materials)
				{
					materialConfigurationInput.ShowRemoveButton();
				}
				base.Layout(null, true);
			}
			else
			{
				foreach (MaterialConfigurationInput materialConfigurationInput2 in this._materials)
				{
					materialConfigurationInput2.HideRemoveButton();
				}
			}
		}

		// Token: 0x04001E8C RID: 7820
		private Group _materialContainer;

		// Token: 0x04001E8D RID: 7821
		private Group _swapMaterialContainer;

		// Token: 0x04001E8E RID: 7822
		private Group _replaceMaterialContainer;

		// Token: 0x04001E8F RID: 7823
		private Group _swapReplaceMaterialContainer;

		// Token: 0x04001E90 RID: 7824
		private CheckBox _swapCheckbox;

		// Token: 0x04001E91 RID: 7825
		private List<MaterialConfigurationInput> _materials = new List<MaterialConfigurationInput>();

		// Token: 0x04001E92 RID: 7826
		private MaterialConfigurationInput _replaceMaterial;

		// Token: 0x04001E93 RID: 7827
		private MaterialConfigurationInput _swapReplaceMaterial;

		// Token: 0x04001E94 RID: 7828
		private MaterialConfigurationInput _swapMaterial;

		// Token: 0x04001E95 RID: 7829
		private TextButton _addMaterialButton;
	}
}
