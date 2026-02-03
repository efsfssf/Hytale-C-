using System;
using HytaleClient.Data.EntityStats;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B9 RID: 2233
	internal abstract class EntityStatBarComponent : InterfaceComponent
	{
		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x060040C5 RID: 16581 RVA: 0x000BC2F8 File Offset: 0x000BA4F8
		public bool Display
		{
			get
			{
				return !this._hideIfFull || this._lerpValue != this._targetValue;
			}
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x000BC326 File Offset: 0x000BA526
		public EntityStatBarComponent(InGameView view, string documentPath, bool hideIfFull = false) : base(view.Interface, view.HudContainer)
		{
			this._documentPath = documentPath;
			this._hideIfFull = hideIfFull;
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x000BC360 File Offset: 0x000BA560
		public virtual void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument(this._documentPath, out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._progressBar = uifragment.Get<ProgressBar>("ProgressBar");
			this._lerpValue = this._targetValue;
			this.UpdateProgress();
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x060040C8 RID: 16584 RVA: 0x000BC3D8 File Offset: 0x000BA5D8
		protected virtual void UpdateProgress()
		{
			this._progressBar.Value = this._lerpValue;
			bool hideIfFull = this._hideIfFull;
			if (hideIfFull)
			{
				base.Visible = (this._progressBar.Value < 1f);
			}
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x000BC41A File Offset: 0x000BA61A
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this.UpdateVisibility();
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x000BC43D File Offset: 0x000BA63D
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060040CB RID: 16587
		protected abstract void UpdateVisibility();

		// Token: 0x060040CC RID: 16588 RVA: 0x000BC458 File Offset: 0x000BA658
		protected virtual void Animate(float deltaTime)
		{
			bool flag = this._lerpValue == this._targetValue;
			if (!flag)
			{
				this._lerpValue = MathHelper.Lerp(this._lerpValue, this._targetValue, deltaTime * 5f);
				bool flag2 = MathHelper.Distance(this._targetValue, this._lerpValue) < 0.001f;
				if (flag2)
				{
					this._lerpValue = this._targetValue;
				}
				this.UpdateProgress();
				base.Layout(null, true);
			}
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x000BC4D8 File Offset: 0x000BA6D8
		public virtual void ResetState()
		{
			this._progressBar.Value = 1f;
			this._lerpValue = 1f;
			this._targetValue = 1f;
			bool hideIfFull = this._hideIfFull;
			if (hideIfFull)
			{
				base.Visible = false;
			}
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x000BC520 File Offset: 0x000BA720
		public virtual void OnStatChanged(ClientEntityStatValue value)
		{
			this._targetValue = ((value.Max > 0f) ? value.AsPercentage() : 0f);
			bool flag = !this._hideIfFull || this._targetValue == this._lerpValue;
			if (!flag)
			{
				base.Visible = true;
				base.Layout(null, true);
			}
		}

		// Token: 0x04001F0C RID: 7948
		private const float ProgressBarAnimationSpeed = 5f;

		// Token: 0x04001F0D RID: 7949
		private string _documentPath;

		// Token: 0x04001F0E RID: 7950
		private bool _hideIfFull;

		// Token: 0x04001F0F RID: 7951
		private ProgressBar _progressBar;

		// Token: 0x04001F10 RID: 7952
		private float _lerpValue = 1f;

		// Token: 0x04001F11 RID: 7953
		private float _targetValue = 1f;
	}
}
