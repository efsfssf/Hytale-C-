using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B8 RID: 2232
	internal class CustomHud : InterfaceComponent
	{
		// Token: 0x060040BB RID: 16571 RVA: 0x000BC114 File Offset: 0x000BA314
		public CustomHud(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this._inGameView = inGameView;
			this._hudDesktop = new Desktop(this.Interface.InGameCustomUIProvider, this.Desktop.Graphics, this.Interface.Engine.Graphics.Batcher2D);
			this._hudLayer = new Element(this._hudDesktop, null);
		}

		// Token: 0x060040BC RID: 16572 RVA: 0x000BC184 File Offset: 0x000BA384
		public void Build()
		{
			base.Clear();
			this._hudDesktop.ClearAllLayers();
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._hudDesktop.SetLayer(0, this._hudLayer);
			}
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x000BC1C2 File Offset: 0x000BA3C2
		public void ResetState()
		{
			this._hudLayer.Clear();
		}

		// Token: 0x060040BE RID: 16574 RVA: 0x000BC1D0 File Offset: 0x000BA3D0
		protected override void OnMounted()
		{
			this._hudDesktop.SetLayer(0, this._hudLayer);
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060040BF RID: 16575 RVA: 0x000BC1FE File Offset: 0x000BA3FE
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			this._hudDesktop.ClearAllLayers();
		}

		// Token: 0x060040C0 RID: 16576 RVA: 0x000BC225 File Offset: 0x000BA425
		private void Animate(float deltaTime)
		{
			this._hudDesktop.Update(deltaTime);
		}

		// Token: 0x060040C1 RID: 16577 RVA: 0x000BC234 File Offset: 0x000BA434
		public void Apply(CustomHud packet)
		{
			bool clear = packet.Clear;
			if (clear)
			{
				this._hudLayer.Clear();
			}
			try
			{
				this.Interface.InGameCustomUIProvider.ApplyCommands(packet.Commands, this._hudLayer);
			}
			catch (Exception exception)
			{
				this._hudLayer.Clear();
				this._inGameView.DisconnectWithError("Failed to apply CustomUI HUD commands", exception);
			}
		}

		// Token: 0x060040C2 RID: 16578 RVA: 0x000BC2AC File Offset: 0x000BA4AC
		protected override void LayoutSelf()
		{
			this._hudDesktop.SetViewport(this.Desktop.ViewportRectangle, this.Desktop.Scale);
		}

		// Token: 0x060040C3 RID: 16579 RVA: 0x000BC2D1 File Offset: 0x000BA4D1
		protected override void PrepareForDrawSelf()
		{
			this._hudDesktop.PrepareForDraw();
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x000BC2DF File Offset: 0x000BA4DF
		public void OnChangeDrawOutlines()
		{
			this._hudDesktop.DrawOutlines = this.Desktop.DrawOutlines;
		}

		// Token: 0x04001F09 RID: 7945
		private readonly InGameView _inGameView;

		// Token: 0x04001F0A RID: 7946
		private readonly Desktop _hudDesktop;

		// Token: 0x04001F0B RID: 7947
		private readonly Element _hudLayer;
	}
}
