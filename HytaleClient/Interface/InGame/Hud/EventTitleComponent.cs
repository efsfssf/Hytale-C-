using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Networking;
using Utf8Json;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008BA RID: 2234
	internal class EventTitleComponent : InterfaceComponent
	{
		// Token: 0x060040CF RID: 16591 RVA: 0x000BC588 File Offset: 0x000BA788
		public EventTitleComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this.InGameView = inGameView;
			this.Interface.RegisterForEventFromEngine<PacketHandler.EventTitle>("eventTitle.show", new Action<PacketHandler.EventTitle>(this.OnShowEventTitle));
			this.Interface.RegisterForEventFromEngine<int>("eventTitle.hide", new Action<int>(this.OnHideEventTitle));
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x000BC5F8 File Offset: 0x000BA7F8
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/EventTitle/Major.ui", out document);
			this._majorTitleContainer = document.Instantiate(this.Desktop, null).RootElements[0];
			Document document2;
			this.Interface.TryGetDocument("InGame/Hud/EventTitle/Minor.ui", out document2);
			this._minorTitleContainer = document2.Instantiate(this.Desktop, null).RootElements[0];
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x000BC670 File Offset: 0x000BA870
		private void OnShowEventTitle(PacketHandler.EventTitle title)
		{
			FormattedMessage message = JsonSerializer.Deserialize<FormattedMessage>(title.PrimaryTitle);
			FormattedMessage message2 = JsonSerializer.Deserialize<FormattedMessage>(title.SecondaryTitle);
			this.Queue(FormattedMessageConverter.GetString(message, this.InGameView.Interface), FormattedMessageConverter.GetString(message2, this.InGameView.Interface), title.IsMajor, title.Duration);
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x000BC6CB File Offset: 0x000BA8CB
		private void OnHideEventTitle(int fadeDuration)
		{
			this.ResetState();
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x000BC6D4 File Offset: 0x000BA8D4
		protected override void OnUnmounted()
		{
			bool isAnimating = this._isAnimating;
			if (isAnimating)
			{
				this.StopAnimation();
			}
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x000BC6F4 File Offset: 0x000BA8F4
		public void ResetState()
		{
			base.Clear();
			bool isAnimating = this._isAnimating;
			if (isAnimating)
			{
				this.StopAnimation();
			}
			this._displayTimer = 0f;
			this._totalPendingDuration = 0f;
			this._currentTitle = null;
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x000BC738 File Offset: 0x000BA938
		private void Animate(float deltaTime)
		{
			this._displayTimer += deltaTime;
			Debug.Assert(this._currentTitle != null || this._queue.Count > 0);
			bool flag = this._currentTitle != null;
			if (flag)
			{
				bool flag2 = this._currentTitle.ExpiresAt > this._displayTimer;
				if (flag2)
				{
					return;
				}
				bool flag3 = this._queue.Count == 0;
				if (flag3)
				{
					this.ResetState();
					return;
				}
			}
			this.Show(this._queue.Dequeue());
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x000BC7CC File Offset: 0x000BA9CC
		public void Queue(string primaryTitle, string secondaryTitle = null, bool isMajor = true, float duration = 4f)
		{
			this._totalPendingDuration += duration;
			this._queue.Enqueue(new EventTitleComponent.Title
			{
				Primary = primaryTitle,
				Secondary = secondaryTitle,
				IsMajor = isMajor,
				ExpiresAt = this._totalPendingDuration
			});
			bool flag = !this._isAnimating;
			if (flag)
			{
				this.StartAnimation();
			}
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x000BC82F File Offset: 0x000BAA2F
		private void StartAnimation()
		{
			Debug.Assert(!this._isAnimating);
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this._isAnimating = true;
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x000BC860 File Offset: 0x000BAA60
		private void StopAnimation()
		{
			Debug.Assert(this._isAnimating);
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			this._isAnimating = false;
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x000BC890 File Offset: 0x000BAA90
		private void Show(EventTitleComponent.Title title)
		{
			base.Clear();
			this._currentTitle = title;
			base.Add(title.IsMajor ? this._majorTitleContainer : this._minorTitleContainer, -1);
			base.Find<Label>("PrimaryTitle").Text = title.Primary;
			base.Find<Label>("SecondaryTitle").Text = title.Secondary;
			base.Layout(null, true);
		}

		// Token: 0x04001F12 RID: 7954
		public readonly InGameView InGameView;

		// Token: 0x04001F13 RID: 7955
		private Element _majorTitleContainer;

		// Token: 0x04001F14 RID: 7956
		private Element _minorTitleContainer;

		// Token: 0x04001F15 RID: 7957
		private readonly Queue<EventTitleComponent.Title> _queue = new Queue<EventTitleComponent.Title>();

		// Token: 0x04001F16 RID: 7958
		private EventTitleComponent.Title _currentTitle;

		// Token: 0x04001F17 RID: 7959
		private bool _isAnimating;

		// Token: 0x04001F18 RID: 7960
		private float _displayTimer;

		// Token: 0x04001F19 RID: 7961
		private float _totalPendingDuration;

		// Token: 0x02000D7F RID: 3455
		private class Title
		{
			// Token: 0x04004223 RID: 16931
			public string Primary;

			// Token: 0x04004224 RID: 16932
			public string Secondary;

			// Token: 0x04004225 RID: 16933
			public bool IsMajor;

			// Token: 0x04004226 RID: 16934
			public float ExpiresAt;
		}
	}
}
