using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008A2 RID: 2210
	internal abstract class BaseMultipleMaterialsCommand : BaseSelectionCommand
	{
		// Token: 0x0600400D RID: 16397 RVA: 0x000B735F File Offset: 0x000B555F
		public BaseMultipleMaterialsCommand(InGameView inGameView, Desktop desktop, Element parent = null) : base(inGameView, desktop, parent)
		{
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x000B7378 File Offset: 0x000B5578
		public override void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Pages/Inventory/BuilderTools/MultipleMaterialsCommand.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._materialContainer = uifragment.Get<Group>("MaterialContainer");
			this._addMaterialButton = uifragment.Get<TextButton>("AddMaterialButton");
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x000B73D8 File Offset: 0x000B55D8
		protected override void OnMounted()
		{
			this._addMaterialButton.Activating = delegate()
			{
				this.AddMaterial();
				this.Parent.Parent.Layout(null, true);
			};
			bool flag = this._materials.Count == 0;
			if (flag)
			{
				this.AddMaterial();
			}
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x000B7418 File Offset: 0x000B5618
		private void AddMaterial()
		{
			MaterialConfigurationInput materialConfigurationInput = new MaterialConfigurationInput(this._inGameView, this.Desktop, new MaterialConfigurationInput.OnRemove(this.OnRemoveMaterial), true, true, 1);
			materialConfigurationInput.Build();
			this._materialContainer.Add(materialConfigurationInput, -1);
			this._materials.Add(materialConfigurationInput);
			this.ManageRemoveButtonVisibility();
			this.ManageAddMaterialVisibility();
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x000B7478 File Offset: 0x000B5678
		private void OnRemoveMaterial(MaterialConfigurationInput materialConfigurationInput)
		{
			bool flag = this._materials.Count <= 1;
			if (!flag)
			{
				this._materialContainer.Remove(materialConfigurationInput);
				this._materials.Remove(materialConfigurationInput);
				this.Parent.Parent.Layout(null, true);
				this.ManageRemoveButtonVisibility();
				this.ManageAddMaterialVisibility();
			}
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x000B74E0 File Offset: 0x000B56E0
		public override string GetChatCommand()
		{
			string text = "";
			foreach (MaterialConfigurationInput materialConfigurationInput in this._materials)
			{
				text += materialConfigurationInput.GetCommandArgs();
				bool flag = materialConfigurationInput != Enumerable.Last<MaterialConfigurationInput>(this._materials);
				if (flag)
				{
					text += ",";
				}
			}
			return "[" + text + "]";
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x000B7580 File Offset: 0x000B5780
		private void ManageAddMaterialVisibility()
		{
			this._addMaterialButton.Visible = (this._materials.Count <= 10);
			this.Parent.Parent.Layout(null, true);
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x000B75C8 File Offset: 0x000B57C8
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

		// Token: 0x04001E7A RID: 7802
		private Group _materialContainer;

		// Token: 0x04001E7B RID: 7803
		private List<MaterialConfigurationInput> _materials = new List<MaterialConfigurationInput>();

		// Token: 0x04001E7C RID: 7804
		private TextButton _addMaterialButton;
	}
}
