using System;
using HytaleClient.Application;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface
{
	// Token: 0x02000802 RID: 2050
	internal class GameLoadingView : InterfaceComponent
	{
		// Token: 0x060038BF RID: 14527 RVA: 0x00075884 File Offset: 0x00073A84
		public GameLoadingView(Interface @interface) : base(@interface, null)
		{
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x00075890 File Offset: 0x00073A90
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("GameLoading/GameLoading.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._statusTextLabel = uifragment.Get<Label>("StatusText");
			uifragment.Get<TextButton>("CancelButton").Activating = new Action(this.Dismiss);
			this._progressBar = uifragment.Get<ProgressBar>("ProgressBar");
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
			}
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0007592B File Offset: 0x00073B2B
		protected override void OnMounted()
		{
			this.Interface.SocialBar.SetContainer(base.Find<Group>("SocialBarContainer"));
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x00075962 File Offset: 0x00073B62
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x0007597C File Offset: 0x00073B7C
		private void Animate(float deltaTime)
		{
			bool flag = !base.Visible;
			if (!flag)
			{
				App app = this.Interface.App;
				bool flag2 = app.GameLoading.LoadingStage == AppGameLoading.GameLoadingStage.WaitingForServerToShutdown && app.ShuttingDownSingleplayerServer == null;
				if (flag2)
				{
					app.GameLoading.StartSingleplayerServer();
				}
				else
				{
					bool flag3 = app.GameLoading.LoadingStage >= AppGameLoading.GameLoadingStage.Loading;
					if (flag3)
					{
						app.InGame.Instance.OnNewFrame(deltaTime, false);
					}
				}
				bool flag4 = this._targetProgress != this._lerpProgress;
				if (flag4)
				{
					bool flag5 = this._lerpProgress > this._targetProgress;
					if (flag5)
					{
						this._lerpProgress = this._targetProgress;
					}
					this._lerpProgress = MathHelper.Lerp(this._lerpProgress, this._targetProgress, Math.Min(deltaTime * 20f, 1f));
					this._progressBar.Value = this._lerpProgress;
					this._progressBar.Layout(null, true);
				}
			}
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x00075A88 File Offset: 0x00073C88
		public void SetStatus(string statusText, float percent)
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				this._statusTextLabel.Text = statusText;
				this._targetProgress = percent / 100f;
				base.Layout(null, true);
			}
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x00075AD2 File Offset: 0x00073CD2
		protected internal override void Dismiss()
		{
			this.Interface.App.GameLoading.Abort();
		}

		// Token: 0x040018A3 RID: 6307
		private Label _statusTextLabel;

		// Token: 0x040018A4 RID: 6308
		private ProgressBar _progressBar;

		// Token: 0x040018A5 RID: 6309
		private float _targetProgress;

		// Token: 0x040018A6 RID: 6310
		private float _lerpProgress;
	}
}
