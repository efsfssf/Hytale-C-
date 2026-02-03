using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Pages.InventoryPanels.SelectionCommands
{
	// Token: 0x020008A5 RID: 2213
	internal class MaterialConfigurationInput : Element
	{
		// Token: 0x0600401B RID: 16411 RVA: 0x000B76FE File Offset: 0x000B58FE
		public MaterialConfigurationInput(InGameView inGameView, Desktop desktop, MaterialConfigurationInput.OnRemove onRemove, bool hasWeight = true, bool hasPitch = true, int blockCapacity = 1) : base(desktop, null)
		{
			this._onRemove = onRemove;
			this._inGameView = inGameView;
			this._hasWeight = hasWeight;
			this._hasPitch = hasPitch;
			this._blockCapacity = blockCapacity;
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x000B7730 File Offset: 0x000B5930
		public void Build()
		{
			base.Clear();
			Group root = new Group(this.Desktop, this)
			{
				LayoutMode = LayoutMode.Top
			};
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Pages/Inventory/BuilderTools/Input/BlockSelectorConfiguration.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, root);
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "CreateEyedropUnselect", out this._unselectSound);
			document.TryResolveNamedValue<SoundStyle>(this.Desktop.Provider, "CreateEyedropSelect", out this._selectSound);
			this._blockSelector = uifragment.Get<BlockSelector>("BlockSelector");
			this._blockSelector.Capacity = this._blockCapacity;
			this._weightSlider = uifragment.Get<SliderNumberField>("WeightSlider");
			bool flag = !this._hasWeight;
			if (flag)
			{
				uifragment.Get<Group>("WeightSliderContainer").Visible = false;
			}
			Group group = uifragment.Get<Group>("PitchContainer");
			this._pitchDropdown = new PitchDropdown(this._inGameView, this.Desktop);
			this._pitchDropdown.Build();
			group.Add(this._pitchDropdown, -1);
			bool flag2 = !this._hasPitch;
			if (flag2)
			{
				group.Visible = false;
			}
			this._removeButton = uifragment.Get<Button>("ActionClear");
			this._removeButton.Activating = delegate()
			{
				this._inGameView.InGame.Instance.AudioModule.PlayLocalSoundEvent("UI_CLEAR");
				bool flag3 = this._onRemove != null;
				if (flag3)
				{
					this._onRemove(this);
				}
			};
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x000B788B File Offset: 0x000B5A8B
		protected override void OnMounted()
		{
			this._blockSelector.ValueChanged = delegate()
			{
				string value = this._blockSelector.Value;
				bool flag = string.IsNullOrEmpty(value);
				if (flag)
				{
					this.Desktop.Provider.PlaySound(this._unselectSound);
				}
				else
				{
					this.Desktop.Provider.PlaySound(this._selectSound);
				}
				this.SetPitchValues();
				this._blockSelector.Value = value;
				base.Layout(null, true);
			};
			this.SetPitchValues();
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x000B78AC File Offset: 0x000B5AAC
		private void SetPitchValues()
		{
			bool hasPitch = this._hasPitch;
			if (hasPitch)
			{
				this._pitchDropdown.SetPitchValues(this._blockSelector.Value);
			}
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x000B78DC File Offset: 0x000B5ADC
		public string GetCommandArgs()
		{
			string text = this._blockSelector.Value;
			int value = this._weightSlider.Value;
			bool flag = string.IsNullOrEmpty(text);
			if (flag)
			{
				text = "Empty";
			}
			string text2 = text ?? "";
			bool hasPitch = this._hasPitch;
			if (hasPitch)
			{
				text2 += this._pitchDropdown.GetCommandArg();
			}
			bool hasWeight = this._hasWeight;
			if (hasWeight)
			{
				text2 = string.Format("{0} {1}", value, text2);
			}
			return text2;
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x000B7960 File Offset: 0x000B5B60
		public void HideRemoveButton()
		{
			this._removeButton.Visible = false;
		}

		// Token: 0x06004021 RID: 16417 RVA: 0x000B7970 File Offset: 0x000B5B70
		public void ShowRemoveButton()
		{
			this._removeButton.Visible = true;
		}

		// Token: 0x04001E7E RID: 7806
		private MaterialConfigurationInput.OnRemove _onRemove;

		// Token: 0x04001E7F RID: 7807
		private Button _removeButton;

		// Token: 0x04001E80 RID: 7808
		private InGameView _inGameView;

		// Token: 0x04001E81 RID: 7809
		private PitchDropdown _pitchDropdown;

		// Token: 0x04001E82 RID: 7810
		private BlockSelector _blockSelector;

		// Token: 0x04001E83 RID: 7811
		private SliderNumberField _weightSlider;

		// Token: 0x04001E84 RID: 7812
		private bool _hasWeight;

		// Token: 0x04001E85 RID: 7813
		private bool _hasPitch;

		// Token: 0x04001E86 RID: 7814
		private int _blockCapacity;

		// Token: 0x04001E87 RID: 7815
		private SoundStyle _unselectSound;

		// Token: 0x04001E88 RID: 7816
		private SoundStyle _selectSound;

		// Token: 0x02000D79 RID: 3449
		// (Invoke) Token: 0x0600657E RID: 25982
		public delegate void OnRemove(MaterialConfigurationInput materialConfigurationInput);
	}
}
