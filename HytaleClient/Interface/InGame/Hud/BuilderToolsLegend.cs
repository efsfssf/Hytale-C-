using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Data.Items;
using HytaleClient.Data.UserSettings;
using HytaleClient.InGame.Modules.BuilderTools.Tools;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Brush;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B3 RID: 2227
	internal class BuilderToolsLegend : InterfaceComponent
	{
		// Token: 0x06004082 RID: 16514 RVA: 0x000BA560 File Offset: 0x000B8760
		public BuilderToolsLegend(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x000BA588 File Offset: 0x000B8788
		public void Build()
		{
			base.Clear();
			this._mouseHintIcons.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/BuilderToolsLegend/Legend.ui", out document);
			this._rowHintTextStyle = document.ResolveNamedValue<LabelStyle>(this.Desktop.Provider, "RowHintTextStyle");
			this._rowHintIconSize = document.ResolveNamedValue<int>(this.Desktop.Provider, "RowHintIconSize");
			this._mouseHintIcons.Add(Input.MouseButton.SDL_BUTTON_LEFT, document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "RowHintIconMouseLeftClick"));
			this._mouseHintIcons.Add(Input.MouseButton.SDL_BUTTON_MIDDLE, document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "RowHintIconMouseMiddleClick"));
			this._mouseHintIcons.Add(Input.MouseButton.SDL_BUTTON_RIGHT, document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "RowHintIconMouseRightClick"));
			this._container = new Group(this.Desktop, this);
			UIFragment uifragment = document.Instantiate(this.Desktop, this._container);
			this._toggleLegendKey = uifragment.Get<Label>("ToggleLegendKey");
			this._hintPickMaterial = uifragment.Get<Group>("HintPickMaterial");
			this._hintSetMask = uifragment.Get<Group>("HintSetMask");
			this._hintAddMask = uifragment.Get<Group>("HintAddMask");
			this._hintFavoriteMaterials = uifragment.Get<Group>("HintFavoriteMaterials");
			this._selectedMaterial = uifragment.Get<ItemGrid>("SelectedMaterial");
			this._selectedMaterial.Slots = new ItemGridSlot[1];
			this._hintUndo = uifragment.Get<Group>("HintUndo");
			this._hintRedo = uifragment.Get<Group>("HintRedo");
			this._hintAddRemoveFavoriteMaterial = uifragment.Get<Group>("HintAddRemoveFavoriteMaterial");
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.UpdateInputHints(false, false);
			}
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x000BA73F File Offset: 0x000B893F
		public void SetSelectedMaterial(string material)
		{
			this._selectedMaterial.Slots[0] = new ItemGridSlot((material == null) ? null : new ClientItemStack(material, 1));
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x000BA761 File Offset: 0x000B8961
		public void SetSelectedMaterial(ClientItemStack stack)
		{
			this._selectedMaterial.Slots[0] = new ItemGridSlot(stack);
		}

		// Token: 0x06004086 RID: 16518 RVA: 0x000BA778 File Offset: 0x000B8978
		public void ResetState()
		{
			this._selectedMaterial.Slots = new ItemGridSlot[1];
			this._hintPickMaterial.Clear();
			this._hintSetMask.Clear();
			this._hintAddMask.Clear();
			this._hintFavoriteMaterials.Clear();
			this._hintUndo.Clear();
			this._hintRedo.Clear();
			this._hintAddRemoveFavoriteMaterial.Clear();
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x000BA7EC File Offset: 0x000B89EC
		public void UpdateInputHints(bool doClear = true, bool doLayout = false)
		{
			if (doClear)
			{
				this._hintPickMaterial.Clear();
				this._hintSetMask.Clear();
				this._hintAddMask.Clear();
				this._hintFavoriteMaterials.Clear();
				this._hintUndo.Clear();
				this._hintRedo.Clear();
				this._hintAddRemoveFavoriteMaterial.Clear();
			}
			InputBindings inputBindings = this._inGameView.InGame.Instance.App.Settings.InputBindings;
			this._toggleLegendKey.Text = " [" + inputBindings.ToggleBuilderToolsLegend.BoundInputLabel + "]";
			this.SetHintText(this._hintFavoriteMaterials, inputBindings.TertiaryItemAction.BoundInputLabel);
			this.SetHintText(this._hintUndo, inputBindings.UndoItemAction.BoundInputLabel);
			this.SetHintText(this._hintRedo, inputBindings.RedoItemAction.BoundInputLabel);
			this.SetHintText(this._hintAddRemoveFavoriteMaterial, inputBindings.AddRemoveFavoriteMaterialItemAction.BoundInputLabel);
			PatchStyle icon;
			bool flag = inputBindings.PickBlock.MouseButton != null && this._mouseHintIcons.TryGetValue(inputBindings.PickBlock.MouseButton.Value, out icon);
			if (flag)
			{
				this.SetHintIcon(this._hintPickMaterial, icon);
				this.SetHintIcon(this._hintSetMask, icon);
				this.SetHintIcon(this._hintAddMask, icon);
			}
			else
			{
				string boundInputLabel = inputBindings.PickBlock.BoundInputLabel;
				this.SetHintText(this._hintPickMaterial, boundInputLabel);
				this.SetHintText(this._hintSetMask, boundInputLabel);
				this.SetHintText(this._hintAddMask, boundInputLabel);
			}
			if (doLayout)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x000BA9BC File Offset: 0x000B8BBC
		private void SetHintText(Group parent, string text)
		{
			Label label = new Label(this.Desktop, parent);
			label.Style = this._rowHintTextStyle;
			label.Text = text;
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x000BA9E0 File Offset: 0x000B8BE0
		private void SetHintIcon(Group parent, PatchStyle icon)
		{
			Group group = new Group(this.Desktop, parent);
			group.Anchor = new Anchor
			{
				Width = new int?(this._rowHintIconSize),
				Height = new int?(this._rowHintIconSize)
			};
			group.Background = icon;
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x000BAA34 File Offset: 0x000B8C34
		public void ActiveToolChange(ToolInstance toolInstance)
		{
			string selectedMaterial;
			if (toolInstance == null)
			{
				selectedMaterial = null;
			}
			else
			{
				BrushData brushData = toolInstance.BrushData;
				selectedMaterial = ((brushData != null) ? brushData.Material : null);
			}
			this.SetSelectedMaterial(selectedMaterial);
			this.UpdateInputHints(true, true);
		}

		// Token: 0x04001ED7 RID: 7895
		private readonly InGameView _inGameView;

		// Token: 0x04001ED8 RID: 7896
		private LabelStyle _rowHintTextStyle;

		// Token: 0x04001ED9 RID: 7897
		private int _rowHintIconSize;

		// Token: 0x04001EDA RID: 7898
		private Label _toggleLegendKey;

		// Token: 0x04001EDB RID: 7899
		private Group _container;

		// Token: 0x04001EDC RID: 7900
		private Group _hintPickMaterial;

		// Token: 0x04001EDD RID: 7901
		private Group _hintSetMask;

		// Token: 0x04001EDE RID: 7902
		private Group _hintAddMask;

		// Token: 0x04001EDF RID: 7903
		private Group _hintFavoriteMaterials;

		// Token: 0x04001EE0 RID: 7904
		private Group _hintUndo;

		// Token: 0x04001EE1 RID: 7905
		private Group _hintRedo;

		// Token: 0x04001EE2 RID: 7906
		private Group _hintAddRemoveFavoriteMaterial;

		// Token: 0x04001EE3 RID: 7907
		private ItemGrid _selectedMaterial;

		// Token: 0x04001EE4 RID: 7908
		private readonly Dictionary<Input.MouseButton, PatchStyle> _mouseHintIcons = new Dictionary<Input.MouseButton, PatchStyle>();
	}
}
