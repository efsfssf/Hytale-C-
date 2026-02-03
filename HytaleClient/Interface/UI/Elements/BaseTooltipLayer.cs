using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000852 RID: 2130
	public abstract class BaseTooltipLayer : Element
	{
		// Token: 0x06003B21 RID: 15137 RVA: 0x0008ACF9 File Offset: 0x00088EF9
		protected BaseTooltipLayer(Desktop desktop) : base(desktop, null)
		{
			this._layoutMode = LayoutMode.Top;
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x0008AD18 File Offset: 0x00088F18
		public void Start(bool resetTimer = false)
		{
			bool flag = resetTimer && this._timer < this.ShowDelay;
			if (flag)
			{
				this._timer = 0f;
			}
			bool isActive = this._isActive;
			if (!isActive)
			{
				this._isActive = true;
				this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			}
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x0008AD74 File Offset: 0x00088F74
		private void Animate(float deltaTime)
		{
			bool flag = this._timer < this.ShowDelay;
			if (flag)
			{
				this._timer = Math.Min(this.ShowDelay, this._timer + deltaTime);
				bool flag2 = this._timer >= this.ShowDelay;
				if (!flag2)
				{
					return;
				}
				this.Desktop.SetPassiveLayer(this);
			}
			this.UpdatePosition();
			base.Layout(null, true);
		}

		// Token: 0x06003B24 RID: 15140
		protected abstract void UpdatePosition();

		// Token: 0x06003B25 RID: 15141 RVA: 0x0008ADF0 File Offset: 0x00088FF0
		public void Stop()
		{
			bool flag = !this._isActive;
			if (!flag)
			{
				bool flag2 = this._timer >= this.ShowDelay;
				if (flag2)
				{
					this.Desktop.SetPassiveLayer(null);
				}
				this._timer = 0f;
				this._isActive = false;
				this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			}
		}

		// Token: 0x04001B48 RID: 6984
		[UIMarkupProperty]
		public float ShowDelay = 0.5f;

		// Token: 0x04001B49 RID: 6985
		private float _timer;

		// Token: 0x04001B4A RID: 6986
		private bool _isActive;
	}
}
