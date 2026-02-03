using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008AA RID: 2218
	internal class WallCommand : BaseSelectionCommand
	{
		// Token: 0x0600403D RID: 16445 RVA: 0x000B8522 File Offset: 0x000B6722
		public WallCommand(InGameView inGameView, Desktop desktop, Element parent = null) : base(inGameView, desktop, parent)
		{
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x000B8530 File Offset: 0x000B6730
		public override void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Pages/Inventory/BuilderTools/CommandWall.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._materialSelector = uifragment.Get<BlockSelector>("MaterialSelector");
			this._roofCheckbox = uifragment.Get<CheckBox>("RoofCheckbox");
			this._floorCheckbox = uifragment.Get<CheckBox>("FloorCheckbox");
			this._thicknessSlider = uifragment.Get<SliderNumberField>("ThicknessSlider");
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "CreateEyedropUnselect", out this._unselectSound);
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "CreateEyedropSelect", out this._selectSound);
			Group group = uifragment.Get<Group>("PitchContainer");
			this._pithDropdown = new PitchDropdown(this._inGameView, this.Desktop);
			this._pithDropdown.Build();
			group.Add(this._pithDropdown, -1);
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x000B8628 File Offset: 0x000B6828
		public override string GetChatCommand()
		{
			string value = this._materialSelector.Value;
			bool value2 = this._roofCheckbox.Value;
			bool value3 = this._floorCheckbox.Value;
			int value4 = this._thicknessSlider.Value;
			string text = string.Format("/wall {0}{1} --thickness={2}", value, this._pithDropdown.GetCommandArg(), value4);
			bool flag = value2;
			if (flag)
			{
				text += " --roof";
			}
			bool flag2 = value3;
			if (flag2)
			{
				text += " --floor";
			}
			return text;
		}

		// Token: 0x06004040 RID: 16448 RVA: 0x000B86B6 File Offset: 0x000B68B6
		protected override void OnMounted()
		{
			this._materialSelector.ValueChanged = delegate()
			{
				string value = this._materialSelector.Value;
				bool flag = string.IsNullOrEmpty(value);
				if (flag)
				{
					this.Desktop.Provider.PlaySound(this._unselectSound);
				}
				else
				{
					this.Desktop.Provider.PlaySound(this._selectSound);
				}
				this._pithDropdown.SetPitchValues(value);
				this._materialSelector.Value = value;
				base.Layout(null, true);
			};
			this._pithDropdown.SetPitchValues(this._materialSelector.Value);
		}

		// Token: 0x04001E9C RID: 7836
		private CheckBox _floorCheckbox;

		// Token: 0x04001E9D RID: 7837
		private CheckBox _roofCheckbox;

		// Token: 0x04001E9E RID: 7838
		private BlockSelector _materialSelector;

		// Token: 0x04001E9F RID: 7839
		private SliderNumberField _thicknessSlider;

		// Token: 0x04001EA0 RID: 7840
		private PitchDropdown _pithDropdown;

		// Token: 0x04001EA1 RID: 7841
		private SoundStyle _unselectSound;

		// Token: 0x04001EA2 RID: 7842
		private SoundStyle _selectSound;
	}
}
