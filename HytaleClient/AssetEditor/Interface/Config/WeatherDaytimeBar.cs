using System;
using HytaleClient.AssetEditor.Backends;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BD0 RID: 3024
	internal class WeatherDaytimeBar : Element
	{
		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x06005F47 RID: 24391 RVA: 0x001EBB0F File Offset: 0x001E9D0F
		// (set) Token: 0x06005F48 RID: 24392 RVA: 0x001EBB17 File Offset: 0x001E9D17
		public int CurrentHour { get; private set; }

		// Token: 0x06005F49 RID: 24393 RVA: 0x001EBB20 File Offset: 0x001E9D20
		public WeatherDaytimeBar(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
		}

		// Token: 0x06005F4A RID: 24394 RVA: 0x001EBB38 File Offset: 0x001E9D38
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/WeatherDaytimeBar.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._timelineGroup = uifragment.Get<Group>("Timeline");
			this._timeMarker = uifragment.Get<Group>("TimeMarker");
		}

		// Token: 0x06005F4B RID: 24395 RVA: 0x001EBB95 File Offset: 0x001E9D95
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06005F4C RID: 24396 RVA: 0x001EBBB0 File Offset: 0x001E9DB0
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06005F4D RID: 24397 RVA: 0x001EBBCC File Offset: 0x001E9DCC
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
					bool flag3 = base.CapturedMouseButton == null;
					if (flag3)
					{
						GameTimeState gameTime = this._assetEditorOverlay.Interface.App.Editor.GameTime;
						float num = gameTime.GameDayProgressInHours / 24f;
						this._timeMarker.Anchor.Left = new int?(this.Desktop.UnscaleRound(num * (float)this._timelineGroup.AnchoredRectangle.Width - (float)this._timeMarker.AnchoredRectangle.Width / 2f));
						this._timeMarker.Layout(null, true);
						this.UpdateTimelineEditors(num);
					}
				}
			}
		}

		// Token: 0x06005F4E RID: 24398 RVA: 0x001EBCE4 File Offset: 0x001E9EE4
		public override Element HitTest(Point position)
		{
			bool flag = this._waitingForLayoutAfterMount || !this._anchoredRectangle.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = ((!this._timelineGroup.AnchoredRectangle.Contains(position)) ? base.HitTest(position) : this);
			}
			return result;
		}

		// Token: 0x06005F4F RID: 24399 RVA: 0x001EBD38 File Offset: 0x001E9F38
		public void ResetState()
		{
			this.CurrentHour = 0;
			this._timeMarker.Anchor.Left = new int?(this._timeMarker.Anchor.Width.Value / 2);
		}

		// Token: 0x06005F50 RID: 24400 RVA: 0x001EBD7D File Offset: 0x001E9F7D
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			this.UpdateTimeFromMousePosition();
		}

		// Token: 0x06005F51 RID: 24401 RVA: 0x001EBD88 File Offset: 0x001E9F88
		protected override void OnMouseMove()
		{
			bool flag = base.CapturedMouseButton == null;
			if (!flag)
			{
				this.UpdateTimeFromMousePosition();
			}
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x001EBDB4 File Offset: 0x001E9FB4
		private void UpdateTimeFromMousePosition()
		{
			float num = (float)(this.Desktop.MousePosition.X - this._timelineGroup.AnchoredRectangle.Left) / (float)this._timelineGroup.AnchoredRectangle.Width;
			num = MathHelper.Clamp(num, 0f, 1f);
			this._timeMarker.Anchor.Left = new int?(this.Desktop.UnscaleRound(num * (float)this._timelineGroup.AnchoredRectangle.Width - (float)this._timeMarker.AnchoredRectangle.Width / 2f));
			this._timeMarker.Layout(null, true);
			this._assetEditorOverlay.Interface.App.Editor.GameTime.SetTimeOverride(num);
			this.UpdateTimelineEditors(num);
		}

		// Token: 0x06005F53 RID: 24403 RVA: 0x001EBE98 File Offset: 0x001EA098
		private void UpdateTimelineEditors(float timePercentage)
		{
			int num = (int)Math.Min(Math.Floor((double)(timePercentage * 24f)), 23.0);
			bool flag = num != this.CurrentHour;
			if (flag)
			{
				foreach (TimelineEditor timelineEditor in this._assetEditorOverlay.ConfigEditor.MountedTimelineEditors)
				{
					timelineEditor.UpdateHighlightedHour(num);
					timelineEditor.Layout(null, true);
				}
				this.CurrentHour = num;
			}
		}

		// Token: 0x04003B4B RID: 15179
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003B4C RID: 15180
		private Group _timelineGroup;

		// Token: 0x04003B4D RID: 15181
		private Group _timeMarker;

		// Token: 0x04003B4F RID: 15183
		private float _secondsSinceLastTimeUpdate;
	}
}
