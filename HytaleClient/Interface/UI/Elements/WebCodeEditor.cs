using System;
using System.Diagnostics;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Interface.CoherentUI;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200087A RID: 2170
	internal class WebCodeEditor : Element
	{
		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x06003D4B RID: 15691 RVA: 0x0009C4E8 File Offset: 0x0009A6E8
		public bool IsInitialized
		{
			get
			{
				return this._webView != null;
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x06003D4C RID: 15692 RVA: 0x0009C4F3 File Offset: 0x0009A6F3
		// (set) Token: 0x06003D4D RID: 15693 RVA: 0x0009C4FC File Offset: 0x0009A6FC
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				bool isEditorReady = this._isEditorReady;
				if (isEditorReady)
				{
					this._webView.TriggerEvent("setValue", value ?? "", null, null, null, null);
				}
			}
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x06003D4E RID: 15694 RVA: 0x0009C53A File Offset: 0x0009A73A
		// (set) Token: 0x06003D4F RID: 15695 RVA: 0x0009C544 File Offset: 0x0009A744
		public WebCodeEditor.EditorLanguage Language
		{
			get
			{
				return this._language;
			}
			set
			{
				this._language = value;
				bool isEditorReady = this._isEditorReady;
				if (isEditorReady)
				{
					this._webView.TriggerEvent("setLanguage", value.ToString(), null, null, null, null);
				}
			}
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x0009C585 File Offset: 0x0009A785
		public WebCodeEditor(BaseInterface @interface, Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._interface = @interface;
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x0009C598 File Offset: 0x0009A798
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this.Animate(0f);
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x0009C5BF File Offset: 0x0009A7BF
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			TextureArea textureArea = this._textureArea;
			if (textureArea != null)
			{
				textureArea.Texture.Dispose();
			}
			this._textureArea = null;
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x0009C5F8 File Offset: 0x0009A7F8
		public void InitEditor()
		{
			Debug.Assert(!this.IsInitialized);
			this._webView = new WebView(this._interface.Engine, this._interface.CoUiManager, "coui://monaco-editor/index.html#" + this.Language.ToString(), 1, 1, this._interface.Engine.Window.ViewportScale);
			this._webView.RegisterForEvent("ready", this._interface, new Action(this.OnEditorReady));
			this._webView.RegisterForEvent<string>("didChangeContent", this._interface, new Action<string>(this.OnDidChangeContent));
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x0009C6B4 File Offset: 0x0009A8B4
		public void DisposeEditor()
		{
			this._webView.UnregisterFromEvent("ready");
			this._webView.UnregisterFromEvent("didChangeContent");
			WebView oldWebView = this._webView;
			this._webView = null;
			this._isEditorReady = false;
			Action <>9__1;
			this._interface.CoUiManager.RunInThread(delegate
			{
				oldWebView.Destroy();
				Engine engine = this._interface.Engine;
				Disposable engine2 = this._interface.Engine;
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						oldWebView.Dispose();
					});
				}
				engine.RunOnMainThread(engine2, action, false, false);
			});
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x0009C728 File Offset: 0x0009A928
		protected override void LayoutSelf()
		{
			base.LayoutSelf();
			bool flag = this._webView == null || !this._webView.IsReady;
			if (!flag)
			{
				int width = base.AnchoredRectangle.Width;
				int height = base.AnchoredRectangle.Height;
				bool flag2 = this._webView.Width != width || this._webView.Height != height;
				if (flag2)
				{
					this._webView.Resize(width, height, this._interface.Engine.Window.ViewportScale);
				}
				bool flag3 = this._textureArea == null || this._webView.Width != width || this._webView.Height != height;
				if (flag3)
				{
					TextureArea textureArea = this._textureArea;
					if (textureArea != null)
					{
						textureArea.Texture.Dispose();
					}
					Texture texture = new Texture(Texture.TextureTypes.Texture2D);
					texture.CreateTexture2D(width, height, null, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
					this._textureArea = new TextureArea(texture, 0, 0, width, height, 1);
				}
			}
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x0009C858 File Offset: 0x0009AA58
		private void Animate(float deltaTime)
		{
			bool flag = this._textureArea == null;
			if (!flag)
			{
				this.Desktop.Graphics.GL.BindTexture(GL.TEXTURE_2D, this._textureArea.Texture.GLTexture);
				this._webView.RenderToTexture();
			}
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x0009C8AC File Offset: 0x0009AAAC
		private void OnEditorReady()
		{
			this._isEditorReady = true;
			this._webView.TriggerEvent("setValue", this._value ?? "", null, null, null, null);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x0009C900 File Offset: 0x0009AB00
		private void OnDidChangeContent(string value)
		{
			this._value = value;
			Action valueChanged = this.ValueChanged;
			if (valueChanged != null)
			{
				valueChanged();
			}
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x0009C91C File Offset: 0x0009AB1C
		protected override void PrepareForDrawSelf()
		{
			bool flag = !this._isEditorReady;
			if (!flag)
			{
				this.Desktop.Batcher2D.RequestDrawTexture(this._textureArea.Texture, this._textureArea.Rectangle, this._anchoredRectangle, UInt32Color.White);
			}
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x0009C96C File Offset: 0x0009AB6C
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
				result = (base.HitTest(position) ?? this);
			}
			return result;
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x0009C9AC File Offset: 0x0009ABAC
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			WebCodeEditor.<>c__DisplayClass26_0 CS$<>8__locals1 = new WebCodeEditor.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.evt = evt;
			this.Desktop.FocusElement(this, true);
			CS$<>8__locals1.mousePosition = this.Desktop.MousePosition;
			WebCodeEditor.<>c__DisplayClass26_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.mousePosition.X = CS$<>8__locals2.mousePosition.X - base.AnchoredRectangle.X;
			WebCodeEditor.<>c__DisplayClass26_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.mousePosition.Y = CS$<>8__locals3.mousePosition.Y - base.AnchoredRectangle.Y;
			this._interface.CoUiManager.RunInThread(delegate
			{
				CoUIViewInputForwarder.SendMouseEvent(CS$<>8__locals1.<>4__this._webView, SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN, CS$<>8__locals1.evt.Button, CS$<>8__locals1.mousePosition, Point.Zero);
			});
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x0009CA40 File Offset: 0x0009AC40
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			WebCodeEditor.<>c__DisplayClass27_0 CS$<>8__locals1 = new WebCodeEditor.<>c__DisplayClass27_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.evt = evt;
			CS$<>8__locals1.mousePosition = this.Desktop.MousePosition;
			WebCodeEditor.<>c__DisplayClass27_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.mousePosition.X = CS$<>8__locals2.mousePosition.X - base.AnchoredRectangle.X;
			WebCodeEditor.<>c__DisplayClass27_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.mousePosition.Y = CS$<>8__locals3.mousePosition.Y - base.AnchoredRectangle.Y;
			this._interface.CoUiManager.RunInThread(delegate
			{
				CoUIViewInputForwarder.SendMouseEvent(CS$<>8__locals1.<>4__this._webView, SDL.SDL_EventType.SDL_MOUSEBUTTONUP, CS$<>8__locals1.evt.Button, CS$<>8__locals1.mousePosition, Point.Zero);
			});
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x0009CAC4 File Offset: 0x0009ACC4
		protected override void OnMouseMove()
		{
			WebCodeEditor.<>c__DisplayClass28_0 CS$<>8__locals1 = new WebCodeEditor.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.mousePosition = this.Desktop.MousePosition;
			WebCodeEditor.<>c__DisplayClass28_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.mousePosition.X = CS$<>8__locals2.mousePosition.X - base.AnchoredRectangle.X;
			WebCodeEditor.<>c__DisplayClass28_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.mousePosition.Y = CS$<>8__locals3.mousePosition.Y - base.AnchoredRectangle.Y;
			this._interface.CoUiManager.RunInThread(delegate
			{
				CoUIViewInputForwarder.SendMouseEvent(CS$<>8__locals1.<>4__this._webView, SDL.SDL_EventType.SDL_MOUSEMOTION, 0, CS$<>8__locals1.mousePosition, Point.Zero);
			});
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x0009CB44 File Offset: 0x0009AD44
		protected internal override bool OnMouseWheel(Point offset)
		{
			WebCodeEditor.<>c__DisplayClass29_0 CS$<>8__locals1 = new WebCodeEditor.<>c__DisplayClass29_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.offset = offset;
			CS$<>8__locals1.mousePosition = this.Desktop.MousePosition;
			WebCodeEditor.<>c__DisplayClass29_0 CS$<>8__locals2 = CS$<>8__locals1;
			CS$<>8__locals2.mousePosition.X = CS$<>8__locals2.mousePosition.X - base.AnchoredRectangle.X;
			WebCodeEditor.<>c__DisplayClass29_0 CS$<>8__locals3 = CS$<>8__locals1;
			CS$<>8__locals3.mousePosition.Y = CS$<>8__locals3.mousePosition.Y - base.AnchoredRectangle.Y;
			this._interface.CoUiManager.RunInThread(delegate
			{
				CoUIViewInputForwarder.SendMouseEvent(CS$<>8__locals1.<>4__this._webView, SDL.SDL_EventType.SDL_MOUSEWHEEL, 0, CS$<>8__locals1.mousePosition, CS$<>8__locals1.offset);
			});
			return true;
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x0009CBD0 File Offset: 0x0009ADD0
		protected internal override void OnKeyDown(SDL.SDL_Keycode keyCode, int repeat)
		{
			SDL.SDL_Scancode scanCode = SDL.SDL_GetScancodeFromKey(keyCode);
			SDL.SDL_Keymod keymod = SDL.SDL_Keymod.KMOD_NONE;
			bool isCtrlKeyDown = this.Desktop.IsCtrlKeyDown;
			if (isCtrlKeyDown)
			{
				keymod |= SDL.SDL_Keymod.KMOD_CTRL;
			}
			bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
			if (isShiftKeyDown)
			{
				keymod |= SDL.SDL_Keymod.KMOD_SHIFT;
			}
			bool isAltKeyDown = this.Desktop.IsAltKeyDown;
			if (isAltKeyDown)
			{
				keymod |= SDL.SDL_Keymod.KMOD_LALT;
			}
			bool isGuiKeyDown = this.Desktop.IsGuiKeyDown;
			if (isGuiKeyDown)
			{
				keymod |= SDL.SDL_Keymod.KMOD_GUI;
			}
			this._interface.CoUiManager.RunInThread(delegate
			{
				CoUIViewInputForwarder.SendKeyboardEvent(this._webView, SDL.SDL_EventType.SDL_KEYDOWN, keyCode, scanCode, 0, keymod);
			});
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x0009CCAC File Offset: 0x0009AEAC
		protected internal override void OnKeyUp(SDL.SDL_Keycode keyCode)
		{
			SDL.SDL_Scancode scanCode = SDL.SDL_GetScancodeFromKey(keyCode);
			SDL.SDL_Keymod keymod = SDL.SDL_Keymod.KMOD_NONE;
			bool isCtrlKeyDown = this.Desktop.IsCtrlKeyDown;
			if (isCtrlKeyDown)
			{
				keymod |= SDL.SDL_Keymod.KMOD_CTRL;
			}
			bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
			if (isShiftKeyDown)
			{
				keymod |= SDL.SDL_Keymod.KMOD_SHIFT;
			}
			bool isAltKeyDown = this.Desktop.IsAltKeyDown;
			if (isAltKeyDown)
			{
				keymod |= SDL.SDL_Keymod.KMOD_LALT;
			}
			bool isGuiKeyDown = this.Desktop.IsGuiKeyDown;
			if (isGuiKeyDown)
			{
				keymod |= SDL.SDL_Keymod.KMOD_GUI;
			}
			this._interface.CoUiManager.RunInThread(delegate
			{
				CoUIViewInputForwarder.SendKeyboardEvent(this._webView, SDL.SDL_EventType.SDL_KEYUP, keyCode, scanCode, 0, keymod);
			});
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x0009CD88 File Offset: 0x0009AF88
		protected internal override void OnTextInput(string text)
		{
			this._interface.CoUiManager.RunInThread(delegate
			{
				CoUIViewInputForwarder.SendTextInputEvent(this._webView, text);
			});
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x0009CDC7 File Offset: 0x0009AFC7
		protected internal override void OnBlur()
		{
			WebView webView = this._webView;
			if (webView != null)
			{
				webView.TriggerEvent("blur", null, null, null, null, null);
			}
		}

		// Token: 0x04001C88 RID: 7304
		private WebView _webView;

		// Token: 0x04001C89 RID: 7305
		private TextureArea _textureArea;

		// Token: 0x04001C8A RID: 7306
		private bool _isEditorReady;

		// Token: 0x04001C8B RID: 7307
		private string _value;

		// Token: 0x04001C8C RID: 7308
		public Action ValueChanged;

		// Token: 0x04001C8D RID: 7309
		private WebCodeEditor.EditorLanguage _language;

		// Token: 0x04001C8E RID: 7310
		private readonly BaseInterface _interface;

		// Token: 0x02000D4A RID: 3402
		public enum EditorLanguage
		{
			// Token: 0x04004173 RID: 16755
			Json,
			// Token: 0x04004174 RID: 16756
			Plaintext
		}
	}
}
