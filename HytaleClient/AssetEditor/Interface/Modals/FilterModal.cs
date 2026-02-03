using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Modals
{
	// Token: 0x02000BA1 RID: 2977
	internal class FilterModal : Element
	{
		// Token: 0x06005C1C RID: 23580 RVA: 0x001CF194 File Offset: 0x001CD394
		public FilterModal(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x001CF1B8 File Offset: 0x001CD3B8
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/FilterModal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._container = uifragment.Get<Group>("Container");
			this._entriesContainer = uifragment.Get<Group>("Entries");
			this._searchInput = uifragment.Get<TextField>("SearchInput");
			this._searchInput.ValueChanged = new Action(this.OnSearchChanged);
			this._searchInput.KeyDown = new Action<SDL.SDL_Keycode>(this.OnSearchKeyDown);
			uifragment.Get<TextButton>("CloseButton").Activating = new Action(this.Dismiss);
			uifragment.Get<TextButton>("SelectAllButton").Activating = new Action(this.OnActivateSelectAll);
			uifragment.Get<TextButton>("DeselectAllButton").Activating = new Action(this.OnActivateDeselectAll);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Setup();
			}
		}

		// Token: 0x06005C1E RID: 23582 RVA: 0x001CF2BA File Offset: 0x001CD4BA
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x001CF2C8 File Offset: 0x001CD4C8
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x001CF311 File Offset: 0x001CD511
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			base.OnKeyDown(keycode, repeat);
			this.HandleKeyShortcuts(keycode);
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x001CF328 File Offset: 0x001CD528
		private void HandleKeyShortcuts(SDL.SDL_Keycode keycode)
		{
			bool flag = !this.Desktop.IsShortcutKeyDown;
			if (!flag)
			{
				if (keycode == SDL.SDL_Keycode.SDLK_a)
				{
					bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
					if (isShiftKeyDown)
					{
						this.OnActivateDeselectAll();
					}
					else
					{
						this.OnActivateSelectAll();
					}
				}
			}
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x001CF377 File Offset: 0x001CD577
		private void OnSearchChanged()
		{
			this.Setup();
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x001CF380 File Offset: 0x001CD580
		private void OnSearchKeyDown(SDL.SDL_Keycode keycode)
		{
			this.HandleKeyShortcuts(keycode);
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x001CF38A File Offset: 0x001CD58A
		private void OnActivateDeselectAll()
		{
			this._assetEditorOverlay.GetDisplayedAssetTypes().Clear();
			this.ApplyButtonStyles();
			this._assetEditorOverlay.OnDisplayedAssetTypesChanged();
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x001CF3B4 File Offset: 0x001CD5B4
		private void OnActivateSelectAll()
		{
			HashSet<string> displayedAssetTypes = this._assetEditorOverlay.GetDisplayedAssetTypes();
			displayedAssetTypes.Clear();
			foreach (AssetTypeConfig assetTypeConfig in this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.Values)
			{
				bool flag = assetTypeConfig.AssetTree != this._assetEditorOverlay.Interface.App.Settings.ActiveAssetTree || assetTypeConfig.IsVirtual;
				if (!flag)
				{
					displayedAssetTypes.Add(assetTypeConfig.Id);
				}
			}
			this.ApplyButtonStyles();
			this._assetEditorOverlay.OnDisplayedAssetTypesChanged();
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x001CF474 File Offset: 0x001CD674
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(4);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x001CF483 File Offset: 0x001CD683
		public void ResetState()
		{
			this._entriesContainer.Clear();
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x001CF494 File Offset: 0x001CD694
		public void Setup()
		{
			this._entriesContainer.Clear();
			this._buttons.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/FilterEntry.ui", out document);
			this._defaultStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "DefaultStyle");
			this._selectedStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Desktop.Provider, "SelectedStyle");
			IOrderedEnumerable<string> orderedEnumerable = Enumerable.OrderBy<string, string>(this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.Keys, (string k) => this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[k].Name);
			HashSet<string> displayedAssetTypes = this._assetEditorOverlay.GetDisplayedAssetTypes();
			string text = this._searchInput.Value.Trim().ToLowerInvariant();
			foreach (string key in orderedEnumerable)
			{
				AssetTypeConfig assetTypeConfig = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes[key];
				bool flag = assetTypeConfig.AssetTree != this._assetEditorOverlay.Interface.App.Settings.ActiveAssetTree || assetTypeConfig.IsVirtual;
				if (!flag)
				{
					bool flag2 = text != "" && !assetTypeConfig.Name.ToLowerInvariant().Contains(text);
					if (!flag2)
					{
						UIFragment uifragment = document.Instantiate(this.Desktop, this._entriesContainer);
						Button button = uifragment.Get<Button>("Button");
						button.Activating = delegate()
						{
							this.OnButtonActivate(assetTypeConfig.Id);
						};
						button.DoubleClicking = new Action(this.Dismiss);
						button.Style = (displayedAssetTypes.Contains(assetTypeConfig.Id) ? this._selectedStyle : this._defaultStyle);
						uifragment.Get<Label>("NameLabel").Text = assetTypeConfig.Name;
						uifragment.Get<Group>("Icon").Background = assetTypeConfig.Icon;
						this._buttons.Add(assetTypeConfig.Id, button);
					}
				}
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._entriesContainer.Layout(null, true);
			}
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x001CF734 File Offset: 0x001CD934
		private void ApplyButtonStyles()
		{
			HashSet<string> displayedAssetTypes = this._assetEditorOverlay.GetDisplayedAssetTypes();
			foreach (KeyValuePair<string, Button> keyValuePair in this._buttons)
			{
				keyValuePair.Value.Style = (displayedAssetTypes.Contains(keyValuePair.Key) ? this._selectedStyle : this._defaultStyle);
				keyValuePair.Value.Layout(null, true);
			}
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x001CF7D4 File Offset: 0x001CD9D4
		private void OnButtonActivate(string key)
		{
			HashSet<string> displayedAssetTypes = this._assetEditorOverlay.GetDisplayedAssetTypes();
			bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
			if (isShiftKeyDown)
			{
				bool flag = displayedAssetTypes.Contains(key);
				if (flag)
				{
					displayedAssetTypes.Remove(key);
				}
				else
				{
					displayedAssetTypes.Add(key);
				}
			}
			else
			{
				bool flag2 = displayedAssetTypes.Count == 1 && displayedAssetTypes.Contains(key);
				if (flag2)
				{
					displayedAssetTypes.Clear();
				}
				else
				{
					displayedAssetTypes.Clear();
					displayedAssetTypes.Add(key);
				}
			}
			this.ApplyButtonStyles();
			this._assetEditorOverlay.OnDisplayedAssetTypesChanged();
		}

		// Token: 0x06005C2B RID: 23595 RVA: 0x001CF868 File Offset: 0x001CDA68
		public void Open()
		{
			bool flag = this.Desktop.GetLayer(4) != null;
			if (!flag)
			{
				bool flag2 = this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.Count == 0;
				if (!flag2)
				{
					this._searchInput.Value = "";
					this.Setup();
					this.Desktop.SetLayer(4, this);
				}
			}
		}

		// Token: 0x040039AD RID: 14765
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x040039AE RID: 14766
		private Group _container;

		// Token: 0x040039AF RID: 14767
		private Group _entriesContainer;

		// Token: 0x040039B0 RID: 14768
		private TextField _searchInput;

		// Token: 0x040039B1 RID: 14769
		private Button.ButtonStyle _defaultStyle;

		// Token: 0x040039B2 RID: 14770
		private Button.ButtonStyle _selectedStyle;

		// Token: 0x040039B3 RID: 14771
		private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
	}
}
