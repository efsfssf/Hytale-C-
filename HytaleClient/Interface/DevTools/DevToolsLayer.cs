using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.DevTools
{
	// Token: 0x020008D6 RID: 2262
	internal class DevToolsLayer : Element
	{
		// Token: 0x060041C9 RID: 16841 RVA: 0x000C4490 File Offset: 0x000C2690
		public DevToolsLayer(Interface @interface) : base(@interface.Desktop, null)
		{
			this._interface = @interface;
			this._overlayDesktop = new Desktop(@interface, @interface.Engine.Graphics, @interface.Engine.Graphics.Batcher2D);
			this.DevTools = new DevToolsOverlay(@interface, this._overlayDesktop);
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x000C44F7 File Offset: 0x000C26F7
		protected override void OnMounted()
		{
			this._overlayDesktop.SetLayer(0, this.DevTools);
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x000C4525 File Offset: 0x000C2725
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			this._overlayDesktop.ClearAllLayers();
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x000C454C File Offset: 0x000C274C
		public void Build()
		{
			this.DevTools.Build();
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x000C455B File Offset: 0x000C275B
		private void Animate(float deltaTime)
		{
			this._overlayDesktop.Update(deltaTime);
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x000C456A File Offset: 0x000C276A
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x000C457E File Offset: 0x000C277E
		protected override void OnMouseMove()
		{
			this._overlayDesktop.OnMouseMove(this.Desktop.MousePosition);
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x000C4597 File Offset: 0x000C2797
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			this._overlayDesktop.OnMouseDown(evt.Button, evt.Clicks);
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x000C45B1 File Offset: 0x000C27B1
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			this._overlayDesktop.OnMouseUp(evt.Button, evt.Clicks);
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x000C45CB File Offset: 0x000C27CB
		protected internal override void OnKeyDown(SDL.SDL_Keycode keyCode, int repeat)
		{
			this._overlayDesktop.OnKeyDown(keyCode, repeat);
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x000C45DB File Offset: 0x000C27DB
		protected internal override void OnKeyUp(SDL.SDL_Keycode keyCode)
		{
			this._overlayDesktop.OnKeyUp(keyCode);
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x000C45EA File Offset: 0x000C27EA
		protected internal override void OnTextInput(string text)
		{
			this._overlayDesktop.OnTextInput(text);
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x000C45FC File Offset: 0x000C27FC
		protected internal override bool OnMouseWheel(Point offset)
		{
			this._overlayDesktop.OnMouseWheel(offset);
			return true;
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x000C461C File Offset: 0x000C281C
		protected override void LayoutSelf()
		{
			this._overlayDesktop.SetViewport(this.Desktop.ViewportRectangle, this._interface.Engine.Window.ViewportScale * this._scale);
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x000C4651 File Offset: 0x000C2851
		protected override void PrepareForDrawSelf()
		{
			this._overlayDesktop.PrepareForDraw();
		}

		// Token: 0x04002020 RID: 8224
		private readonly Desktop _overlayDesktop;

		// Token: 0x04002021 RID: 8225
		private readonly Interface _interface;

		// Token: 0x04002022 RID: 8226
		private float _scale = 1f;

		// Token: 0x04002023 RID: 8227
		public readonly DevToolsOverlay DevTools;
	}
}
