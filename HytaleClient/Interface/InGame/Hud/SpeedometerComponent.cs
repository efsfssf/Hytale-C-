using System;
using System.Linq;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008C4 RID: 2244
	internal class SpeedometerComponent : InterfaceComponent
	{
		// Token: 0x06004122 RID: 16674 RVA: 0x000BED38 File Offset: 0x000BCF38
		public SpeedometerComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x000BEDA4 File Offset: 0x000BCFA4
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/Speedometer.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._xLabel = uifragment.Get<Label>("X");
			this._xMaxLabel = uifragment.Get<Label>("XMax");
			this._zLabel = uifragment.Get<Label>("Z");
			this._zMaxLabel = uifragment.Get<Label>("ZMax");
			this._yLabel = uifragment.Get<Label>("Y");
			this._yMaxLabel = uifragment.Get<Label>("YMax");
			this._magnitudeLatLabel = uifragment.Get<Label>("MagnitudeLat");
			this._magnitudeLatMaxLabel = uifragment.Get<Label>("MagnitudeLatMax");
			this._magnitudeLabel = uifragment.Get<Label>("Magnitude");
			this._magnitudeMaxLabel = uifragment.Get<Label>("MagnitudeMax");
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x000BEE84 File Offset: 0x000BD084
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Update));
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x000BEE9F File Offset: 0x000BD09F
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Update));
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x000BEEBC File Offset: 0x000BD0BC
		private void Update(float deltaTime)
		{
			Vector3 velocity = this._inGameView.InGame.Instance.CharacterControllerModule.MovementController.Velocity;
			double num = Math.Round((double)velocity.X, 3);
			double num2 = Math.Round((double)velocity.Z, 3);
			double num3 = Math.Round((double)velocity.Y, 3);
			float num4 = (float)Math.Sqrt((double)(velocity.X * velocity.X) + (double)(velocity.Z * velocity.Z));
			float num5 = (float)Math.Sqrt((double)(velocity.X * velocity.X) + (double)(velocity.Z * velocity.Z) + (double)(velocity.Y * velocity.Y));
			this._xLabel.Text = string.Format("{0:F}", num);
			this._zLabel.Text = string.Format("{0:F}", num2);
			this._yLabel.Text = string.Format("{0:F}", num3);
			this._magnitudeLatLabel.Text = string.Format("{0:F}", num4);
			this._magnitudeLabel.Text = string.Format("{0:F}", num5);
			this._xVelocityComparison[0] = velocity.X;
			this._xVelocityComparison[1] = this._maxValues.X;
			this._zVelocityComparison[0] = velocity.Z;
			this._zVelocityComparison[1] = this._maxValues.Z;
			this._yVelocityComparison[0] = velocity.Y;
			this._yVelocityComparison[1] = this._maxValues.Y;
			this._lateralMagnitudeComparison[0] = num4;
			this._lateralMagnitudeComparison[1] = this._maxValues.LateralMagnitude;
			this._magnitudeComparison[0] = num5;
			this._magnitudeComparison[1] = this._maxValues.Magnitude;
			this._maxValues.X = Enumerable.Max(Enumerable.Select<float, float>(this._xVelocityComparison, new Func<float, float>(Math.Abs)));
			this._maxValues.Z = Enumerable.Max(Enumerable.Select<float, float>(this._zVelocityComparison, new Func<float, float>(Math.Abs)));
			float num6 = Enumerable.Min(this._yVelocityComparison);
			float num7 = Enumerable.Max(this._yVelocityComparison);
			this._maxValues.Y = ((Math.Abs(num6) > num7) ? num6 : num7);
			this._maxValues.LateralMagnitude = Enumerable.Max(Enumerable.Select<float, float>(this._lateralMagnitudeComparison, new Func<float, float>(Math.Abs)));
			this._maxValues.Magnitude = Enumerable.Max(Enumerable.Select<float, float>(this._magnitudeComparison, new Func<float, float>(Math.Abs)));
			this._maxValuesUpdateTimer += deltaTime;
			bool flag = this._maxValuesUpdateTimer > 2f;
			if (flag)
			{
				this._xMaxLabel.Text = string.Format(" ({0:F})", this._maxValues.X);
				this._zMaxLabel.Text = string.Format(" ({0:F})", this._maxValues.Z);
				this._yMaxLabel.Text = string.Format(" ({0:F})", this._maxValues.Y);
				this._magnitudeLatMaxLabel.Text = string.Format(" ({0:F})", this._maxValues.LateralMagnitude);
				this._magnitudeMaxLabel.Text = string.Format(" ({0:F})", this._maxValues.Magnitude);
				this._maxValues.Reset();
				this._maxValuesUpdateTimer = 0f;
			}
			base.Layout(null, true);
		}

		// Token: 0x04001F55 RID: 8021
		private const float MaxValuesUpdateInterval = 2f;

		// Token: 0x04001F56 RID: 8022
		private readonly InGameView _inGameView;

		// Token: 0x04001F57 RID: 8023
		private Label _xLabel;

		// Token: 0x04001F58 RID: 8024
		private Label _xMaxLabel;

		// Token: 0x04001F59 RID: 8025
		private Label _zLabel;

		// Token: 0x04001F5A RID: 8026
		private Label _zMaxLabel;

		// Token: 0x04001F5B RID: 8027
		private Label _yLabel;

		// Token: 0x04001F5C RID: 8028
		private Label _yMaxLabel;

		// Token: 0x04001F5D RID: 8029
		private Label _magnitudeLatLabel;

		// Token: 0x04001F5E RID: 8030
		private Label _magnitudeLatMaxLabel;

		// Token: 0x04001F5F RID: 8031
		private Label _magnitudeLabel;

		// Token: 0x04001F60 RID: 8032
		private Label _magnitudeMaxLabel;

		// Token: 0x04001F61 RID: 8033
		private float[] _xVelocityComparison = new float[2];

		// Token: 0x04001F62 RID: 8034
		private float[] _zVelocityComparison = new float[2];

		// Token: 0x04001F63 RID: 8035
		private float[] _yVelocityComparison = new float[2];

		// Token: 0x04001F64 RID: 8036
		private float[] _lateralMagnitudeComparison = new float[2];

		// Token: 0x04001F65 RID: 8037
		private float[] _magnitudeComparison = new float[2];

		// Token: 0x04001F66 RID: 8038
		private SpeedometerComponent.MaxValues _maxValues;

		// Token: 0x04001F67 RID: 8039
		private float _maxValuesUpdateTimer;

		// Token: 0x04001F68 RID: 8040
		public bool Enabled = false;

		// Token: 0x02000D83 RID: 3459
		private struct MaxValues
		{
			// Token: 0x06006587 RID: 25991 RVA: 0x00211CBA File Offset: 0x0020FEBA
			public void Reset()
			{
				this.X = 0f;
				this.Z = 0f;
				this.Y = 0f;
				this.LateralMagnitude = 0f;
				this.Magnitude = 0f;
			}

			// Token: 0x0400423D RID: 16957
			public float X;

			// Token: 0x0400423E RID: 16958
			public float Z;

			// Token: 0x0400423F RID: 16959
			public float Y;

			// Token: 0x04004240 RID: 16960
			public float LateralMagnitude;

			// Token: 0x04004241 RID: 16961
			public float Magnitude;
		}
	}
}
