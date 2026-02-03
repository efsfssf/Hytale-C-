using System;
using System.Runtime.CompilerServices;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008BC RID: 2236
	internal class InputBindingsComponent : InterfaceComponent
	{
		// Token: 0x060040DC RID: 16604 RVA: 0x000BC92F File Offset: 0x000BAB2F
		public InputBindingsComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x000BC94C File Offset: 0x000BAB4C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/InputBindings.ui", out document);
			this._backgroundStyle = document.ResolveNamedValue<PatchStyle>(this.Interface, "BackgroundStyle");
			this._backgroundStyleActive = document.ResolveNamedValue<PatchStyle>(this.Interface, "BackgroundStyleActive");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._bindingQuests = uifragment.Get<Button>("BindingQuests");
			this._bindingBook = uifragment.Get<Button>("BindingBook");
			this._bindingMap = uifragment.Get<Button>("BindingMap");
			this._bindingMap.Activating = delegate()
			{
				this._inGameView.InGame.SetCurrentPage(6, false, true);
			};
			this._bindingInventory = uifragment.Get<Button>("BindingInventory");
			this._bindingInventory.Activating = delegate()
			{
				this._inGameView.InGame.SetCurrentPage(2, false, true);
			};
			this._bindingSocial = uifragment.Get<Button>("BindingSocial");
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.UpdateBindings(false);
			}
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x000BCA45 File Offset: 0x000BAC45
		protected override void OnMounted()
		{
			this.UpdateBindings(false);
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x000BCA4F File Offset: 0x000BAC4F
		public void ResetState()
		{
			this.UpdateBindings(false);
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x000BCA59 File Offset: 0x000BAC59
		public void OnWorldMapSettingsUpdated()
		{
			this.UpdateBindings(true);
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x000BCA64 File Offset: 0x000BAC64
		public void UpdateBindings(bool doLayout = true)
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				this.<UpdateBindings>g__UpdateState|13_0(this._bindingQuests, "Quests", false, "???");
				this.<UpdateBindings>g__UpdateState|13_0(this._bindingBook, "Book", false, "???");
				this.<UpdateBindings>g__UpdateState|13_0(this._bindingInventory, "Inventory", this._inGameView.InGame.CurrentPage == 2 && this._inGameView.InventoryPage.IsFieldcraft, this._inGameView.Interface.App.Settings.InputBindings.OpenInventory.BoundInputLabel);
				this.<UpdateBindings>g__UpdateState|13_0(this._bindingMap, "Map", this._inGameView.InGame.CurrentPage == 6, this._inGameView.Interface.App.Settings.InputBindings.OpenMap.BoundInputLabel);
				this.<UpdateBindings>g__UpdateState|13_0(this._bindingSocial, "Social", false, "???");
				this._bindingMap.Visible = this._inGameView.InGame.Instance.WorldMapModule.IsWorldMapEnabled;
				if (doLayout)
				{
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x000BCBDC File Offset: 0x000BADDC
		[CompilerGenerated]
		private void <UpdateBindings>g__UpdateState|13_0(Button button, string id, bool isActive, string label)
		{
			button.Background = (isActive ? this._backgroundStyleActive : this._backgroundStyle);
			button.Find<Label>("Label").Text = label;
			button.Find<Group>("Icon").Background = new PatchStyle("InGame/Hud/InputBindingIcon" + id + (isActive ? "Active" : "") + ".png");
		}

		// Token: 0x04001F1A RID: 7962
		private InGameView _inGameView;

		// Token: 0x04001F1B RID: 7963
		private Button _bindingQuests;

		// Token: 0x04001F1C RID: 7964
		private Button _bindingBook;

		// Token: 0x04001F1D RID: 7965
		private Button _bindingMap;

		// Token: 0x04001F1E RID: 7966
		private Button _bindingInventory;

		// Token: 0x04001F1F RID: 7967
		private Button _bindingSocial;

		// Token: 0x04001F20 RID: 7968
		private PatchStyle _backgroundStyle;

		// Token: 0x04001F21 RID: 7969
		private PatchStyle _backgroundStyleActive;
	}
}
