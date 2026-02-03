using System;
using HytaleClient.Interface.UI.Markup;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200084F RID: 2127
	[UIMarkupElement]
	public class TimerLabel : Label
	{
		// Token: 0x1700102E RID: 4142
		// (set) Token: 0x06003B06 RID: 15110 RVA: 0x0008A340 File Offset: 0x00088540
		[UIMarkupProperty]
		public int Seconds
		{
			set
			{
				this._milliseconds = (float)value;
				this._millisecondsLeft = (float)value;
			}
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x0008A353 File Offset: 0x00088553
		public TimerLabel(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x0008A35F File Offset: 0x0008855F
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this.UpdateText();
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x0008A381 File Offset: 0x00088581
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x0008A39C File Offset: 0x0008859C
		private void Animate(float deltaTime)
		{
			bool flag = this._millisecondsLeft == 0f || this.Paused;
			if (!flag)
			{
				this._millisecondsLeft -= deltaTime;
				bool flag2 = this._millisecondsLeft < 0f;
				if (flag2)
				{
					this._millisecondsLeft = 0f;
				}
				this.UpdateText();
			}
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x0008A3F8 File Offset: 0x000885F8
		private void UpdateText()
		{
			float num = (this.Direction == TimerLabel.TimerDirection.CountUp) ? (this._milliseconds - this._millisecondsLeft) : this._millisecondsLeft;
			double num2 = Math.Floor((double)num);
			int num3 = (int)Math.Floor(num2 / 3600.0);
			int num4 = (int)Math.Floor(num2 / 60.0);
			int num5 = (int)num2 % 60;
			base.Text = ((num3 > 0) ? string.Format("{0:D2}:{1:D2}:{2:D2}", num3, num4, num5) : string.Format("{0:D2}:{1:D2}", num4, num5));
			base.Layout(null, true);
		}

		// Token: 0x04001B37 RID: 6967
		private float _millisecondsLeft;

		// Token: 0x04001B38 RID: 6968
		private float _milliseconds;

		// Token: 0x04001B39 RID: 6969
		[UIMarkupProperty]
		public TimerLabel.TimerDirection Direction;

		// Token: 0x04001B3A RID: 6970
		[UIMarkupProperty]
		public bool Paused;

		// Token: 0x02000D22 RID: 3362
		public enum TimerDirection
		{
			// Token: 0x040040E7 RID: 16615
			CountDown,
			// Token: 0x040040E8 RID: 16616
			CountUp
		}
	}
}
