using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Backends;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BBF RID: 3007
	internal class DayTimeControls : Element
	{
		// Token: 0x06005E4C RID: 24140 RVA: 0x001E2577 File Offset: 0x001E0777
		public DayTimeControls(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
			this.FlexWeight = 1;
			this._layoutMode = LayoutMode.Left;
		}

		// Token: 0x06005E4D RID: 24141 RVA: 0x001E259D File Offset: 0x001E079D
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this.UpdateLockPreviewButton();
		}

		// Token: 0x06005E4E RID: 24142 RVA: 0x001E25BF File Offset: 0x001E07BF
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06005E4F RID: 24143 RVA: 0x001E25DC File Offset: 0x001E07DC
		private void Animate(float deltaTime)
		{
			bool flag = !(this._assetEditorOverlay.Backend is ServerAssetEditorBackend);
			if (!flag)
			{
				this._secondsSinceLastTimeUpdate += deltaTime;
				bool flag2 = (double)this._secondsSinceLastTimeUpdate >= 0.3;
				if (flag2)
				{
					this._secondsSinceLastTimeUpdate = 0f;
					this.SetDay(this._assetEditorOverlay.Interface.App.Editor.GameTime.Time.DayOfYear, true);
				}
			}
		}

		// Token: 0x06005E50 RID: 24144 RVA: 0x001E266C File Offset: 0x001E086C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/DayTimeControls.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._togglePauseTimeCheckBox = uifragment.Get<CheckBox>("TogglePauseTime");
			this._togglePauseTimeCheckBox.ValueChanged = delegate()
			{
				this._assetEditorOverlay.Interface.App.Editor.GameTime.SetTimePaused(this._togglePauseTimeCheckBox.Value);
			};
			this._previousDay = uifragment.Get<Button>("PreviousDay");
			this._previousDay.Activating = delegate()
			{
				this.ModifyLocalDaytimeOverride(-1);
			};
			this._nextDay = uifragment.Get<Button>("NextDay");
			this._nextDay.Activating = delegate()
			{
				this.ModifyLocalDaytimeOverride(1);
			};
			this._lockPreview = uifragment.Get<CheckBox>("LockTimeAndWeather");
			this._lockPreview.ValueChanged = delegate()
			{
				this._assetEditorOverlay.Interface.App.Editor.GameTime.SetLocked(this._lockPreview.Value);
			};
			this._currentDayLabel = uifragment.Get<Label>("CurrentDay");
			this.SetDay(0, false);
		}

		// Token: 0x06005E51 RID: 24145 RVA: 0x001E2761 File Offset: 0x001E0961
		public void ResetState()
		{
			this._togglePauseTimeCheckBox.Value = this._assetEditorOverlay.Interface.App.Editor.GameTime.IsPaused;
			this.UpdateLockPreviewButton();
		}

		// Token: 0x06005E52 RID: 24146 RVA: 0x001E2798 File Offset: 0x001E0998
		private void UpdateLockPreviewButton()
		{
			this._lockPreview.Value = this._assetEditorOverlay.Interface.App.Editor.GameTime.IsLocked;
			this._lockPreview.Layout(null, true);
		}

		// Token: 0x06005E53 RID: 24147 RVA: 0x001E27E8 File Offset: 0x001E09E8
		private void SetDay(int day, bool doLayout = true)
		{
			this._currentDayLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.weatherDaytimeBar.currentDay", new Dictionary<string, string>
			{
				{
					"day, number",
					this.Desktop.Provider.FormatNumber(day)
				}
			}, true);
			if (doLayout)
			{
				this._currentDayLabel.Layout(null, true);
			}
		}

		// Token: 0x06005E54 RID: 24148 RVA: 0x001E2855 File Offset: 0x001E0A55
		private void ModifyLocalDaytimeOverride(int mod)
		{
			this._assetEditorOverlay.Interface.App.Editor.GameTime.ModifyDayOverride(mod);
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x001E287C File Offset: 0x001E0A7C
		public void OnGameTimePauseUpdated(bool isPaused)
		{
			this._togglePauseTimeCheckBox.Value = isPaused;
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._togglePauseTimeCheckBox.Layout(null, true);
			}
		}

		// Token: 0x04003AE7 RID: 15079
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003AE8 RID: 15080
		private CheckBox _togglePauseTimeCheckBox;

		// Token: 0x04003AE9 RID: 15081
		private Button _previousDay;

		// Token: 0x04003AEA RID: 15082
		private Button _nextDay;

		// Token: 0x04003AEB RID: 15083
		private CheckBox _lockPreview;

		// Token: 0x04003AEC RID: 15084
		private Label _currentDayLabel;

		// Token: 0x04003AED RID: 15085
		private float _secondsSinceLastTimeUpdate;
	}
}
