using System;
using System.Diagnostics;
using HytaleClient.Data;
using HytaleClient.Math;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.Core
{
	// Token: 0x02000B80 RID: 2944
	public sealed class Window : Disposable
	{
		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x06005A83 RID: 23171 RVA: 0x001C2B30 File Offset: 0x001C0D30
		// (set) Token: 0x06005A84 RID: 23172 RVA: 0x001C2B38 File Offset: 0x001C0D38
		public float ViewportScale { get; private set; }

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x06005A85 RID: 23173 RVA: 0x001C2B41 File Offset: 0x001C0D41
		// (set) Token: 0x06005A86 RID: 23174 RVA: 0x001C2B49 File Offset: 0x001C0D49
		public Rectangle Viewport { get; private set; }

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x06005A87 RID: 23175 RVA: 0x001C2B52 File Offset: 0x001C0D52
		// (set) Token: 0x06005A88 RID: 23176 RVA: 0x001C2B5A File Offset: 0x001C0D5A
		public double AspectRatio { get; private set; }

		// Token: 0x06005A89 RID: 23177 RVA: 0x001C2B64 File Offset: 0x001C0D64
		public Window(Window.WindowSettings settings)
		{
			this._zoomedMinimumSize = (this._minimumSize = settings.MinimumSize);
			this._zoomedNormalSize = settings.InitialSize;
			this._borderless = settings.Borderless;
			this.MinAspectRatio = (double)settings.MinAspectRatio;
			this.MaxAspectRatio = (double)settings.MaxAspectRatio;
			bool flag = this.MaxAspectRatio < this.MinAspectRatio;
			if (flag)
			{
				throw new ArgumentException("MaxAspectRatio must be >= MinAspectRatio");
			}
			SDL.SDL_WindowFlags sdl_WindowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_HIDDEN | SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI;
			bool resizable = settings.Resizable;
			if (resizable)
			{
				sdl_WindowFlags |= SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
			}
			bool borderless = settings.Borderless;
			if (borderless)
			{
				sdl_WindowFlags |= SDL.SDL_WindowFlags.SDL_WINDOW_BORDERLESS;
			}
			this.Handle = SDL.SDL_CreateWindow(settings.Title, 805240832, 805240832, settings.InitialSize.X, settings.InitialSize.Y, sdl_WindowFlags);
			bool flag2 = this.Handle == IntPtr.Zero;
			if (flag2)
			{
				throw new Exception("Failed to create window: " + SDL.SDL_GetError());
			}
			bool flag3 = settings.Icon != null;
			if (flag3)
			{
				settings.Icon.DoWithSurface(settings.Icon.Width, settings.Icon.Height, delegate(IntPtr surface)
				{
					SDL.SDL_SetWindowIcon(this.Handle, surface);
				}, 255U, 65280U, 16711680U, 4278190080U);
			}
			this.Id = SDL.SDL_GetWindowID(this.Handle);
			SDL.SDL_SysWMinfo sdl_SysWMinfo = default(SDL.SDL_SysWMinfo);
			SDL.SDL_GetWindowWMInfo(this.Handle, ref sdl_SysWMinfo);
			this._win32Handle = sdl_SysWMinfo.info.win.window;
			this.SetState(settings.InitialState, this._borderless, false);
			SDL.SDL_SetWindowPosition(this.Handle, 805240832, 805240832);
		}

		// Token: 0x06005A8A RID: 23178 RVA: 0x001C2D3F File Offset: 0x001C0F3F
		protected override void DoDispose()
		{
			SDL.SDL_DestroyWindow(this.Handle);
		}

		// Token: 0x06005A8B RID: 23179 RVA: 0x001C2D4E File Offset: 0x001C0F4E
		public void Show()
		{
			SDL.SDL_ShowWindow(this.Handle);
		}

		// Token: 0x06005A8C RID: 23180 RVA: 0x001C2D5D File Offset: 0x001C0F5D
		public void Raise()
		{
			SDL.SDL_RaiseWindow(this.Handle);
		}

		// Token: 0x06005A8D RID: 23181 RVA: 0x001C2D6C File Offset: 0x001C0F6C
		public Window.WindowState GetState()
		{
			uint num = SDL.SDL_GetWindowFlags(this.Handle);
			bool flag = (num & 1U) > 0U;
			Window.WindowState result;
			if (flag)
			{
				result = Window.WindowState.Fullscreen;
			}
			else
			{
				bool flag2 = (num & 128U) > 0U;
				if (flag2)
				{
					result = Window.WindowState.Maximized;
				}
				else
				{
					bool flag3 = (num & 64U) > 0U;
					if (flag3)
					{
						result = Window.WindowState.Minimized;
					}
					else
					{
						result = Window.WindowState.Normal;
					}
				}
			}
			return result;
		}

		// Token: 0x06005A8E RID: 23182 RVA: 0x001C2DBC File Offset: 0x001C0FBC
		public Point GetSize()
		{
			int x;
			int y;
			SDL.SDL_GetWindowSize(this.Handle, out x, out y);
			return new Point(x, y);
		}

		// Token: 0x06005A8F RID: 23183 RVA: 0x001C2DE8 File Offset: 0x001C0FE8
		public void SetState(Window.WindowState state, bool borderless, bool recalculateZoom)
		{
			this._borderless = borderless;
			bool flag = state == Window.WindowState.Fullscreen;
			if (flag)
			{
				SDL.SDL_RestoreWindow(this.Handle);
				bool borderless2 = this._borderless;
				if (borderless2)
				{
					SDL.SDL_SetWindowFullscreen(this.Handle, 4097U);
				}
				else
				{
					SDL.SDL_DisplayMode sdl_DisplayMode;
					SDL.SDL_GetDesktopDisplayMode(0, out sdl_DisplayMode);
					SDL.SDL_SetWindowSize(this.Handle, sdl_DisplayMode.w, sdl_DisplayMode.h);
					SDL.SDL_SetWindowFullscreen(this.Handle, 1U);
				}
			}
			else
			{
				SDL.SDL_SetWindowFullscreen(this.Handle, 0U);
				bool flag2 = state == Window.WindowState.Maximized && this._borderless;
				if (flag2)
				{
					state = Window.WindowState.Normal;
				}
				bool flag3 = state == Window.WindowState.Normal;
				if (flag3)
				{
					SDL.SDL_RestoreWindow(this.Handle);
				}
				SDL.SDL_SetWindowBordered(this.Handle, this._borderless ? SDL.SDL_bool.SDL_FALSE : SDL.SDL_bool.SDL_TRUE);
				SDL.SDL_SetWindowMinimumSize(this.Handle, this._zoomedMinimumSize.X, this._zoomedMinimumSize.Y);
				SDL.SDL_SetWindowSize(this.Handle, this._zoomedNormalSize.X, this._zoomedNormalSize.Y);
				SDL.SDL_SetWindowPosition(this.Handle, 805240832, 805240832);
				bool flag4 = state == Window.WindowState.Maximized;
				if (flag4)
				{
					SDL.SDL_MaximizeWindow(this.Handle);
				}
				else
				{
					bool flag5 = state == Window.WindowState.Minimized;
					if (flag5)
					{
						SDL.SDL_MinimizeWindow(this.Handle);
					}
				}
			}
			this.SetupViewport(recalculateZoom);
		}

		// Token: 0x06005A90 RID: 23184 RVA: 0x001C2F48 File Offset: 0x001C1148
		public void SetupViewport(bool recalculateZoom = false)
		{
			Window.WindowState state = this.GetState();
			int num;
			int num2;
			SDL.SDL_GetWindowSize(this.Handle, out num, out num2);
			bool flag = state == Window.WindowState.Normal || state == Window.WindowState.Minimized;
			if (flag)
			{
				this._zoomedNormalSize = new Point(num, num2);
			}
			int num3 = SDL.SDL_GetWindowDisplayIndex(this.Handle);
			SDL.SDL_Rect sdl_Rect;
			SDL.SDL_GetDisplayUsableBounds(num3, ref sdl_Rect);
			double num4 = 1.0;
			int num5;
			int num6;
			SDL.SDL_GL_GetDrawableSize(this.Handle, out num5, out num6);
			this._drawableScale = (double)num6 / (double)num2;
			bool flag2 = num6 != num2;
			if (flag2)
			{
				num4 = 1.0;
			}
			else
			{
				uint num7;
				bool flag3 = WindowsDPIHelper.TryGetDpiForWindow(this._win32Handle, out num7);
				if (flag3)
				{
					num4 = num7 / 96.0;
					this._zoomedMinimumSize = new Point((int)((double)this._minimumSize.X * num4), (int)((double)this._minimumSize.Y * num4));
					bool flag4 = this._zoomedMinimumSize.X >= sdl_Rect.w || this._zoomedMinimumSize.Y >= sdl_Rect.h;
					if (flag4)
					{
						num4 = 1.0;
						this._zoomedMinimumSize = this._minimumSize;
					}
				}
			}
			Debug.Assert(this._drawableScale == 1.0 || num4 == 1.0);
			if (recalculateZoom)
			{
				this.RecalculateZoom(false, num4, this._minimumSize.X, this._minimumSize.Y);
			}
			else
			{
				bool flag5 = num4 != this._monitorZoom;
				if (flag5)
				{
					this.RecalculateZoom(true, num4, this._zoomedNormalSize.X, this._zoomedNormalSize.Y);
				}
			}
			this.ViewportScale = (float)(this._drawableScale * this._monitorZoom);
			this.AspectRatio = Math.Min(Math.Max((double)num / (double)num2, this.MinAspectRatio), this.MaxAspectRatio);
			int num8 = (int)((double)num6 * this.AspectRatio);
			int num9 = (int)((double)num5 / this.AspectRatio);
			bool flag6 = num9 > num6;
			if (flag6)
			{
				num9 = num6;
				num8 = (int)((double)num9 * this.AspectRatio);
			}
			bool flag7 = num8 > num5;
			if (flag7)
			{
				num8 = num5;
				num9 = (int)((double)num8 / this.AspectRatio);
			}
			this.Viewport = new Rectangle((num5 - num8) / 2, (num6 - num9) / 2, num8, num9);
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x001C31B8 File Offset: 0x001C13B8
		public void RecalculateZoom(bool calculateViaCompare, double newMonitorZoom, int width, int height)
		{
			Window.WindowState state = this.GetState();
			SDL.SDL_Rect sdl_Rect;
			SDL.SDL_GetDisplayBounds(SDL.SDL_GetWindowDisplayIndex(this.Handle), out sdl_Rect);
			int num;
			int num2;
			SDL.SDL_GL_GetDrawableSize(this.Handle, out num, out num2);
			if (calculateViaCompare)
			{
				double num3 = newMonitorZoom / this._monitorZoom;
				this._monitorZoom = newMonitorZoom;
				this._zoomedNormalSize = new Point(MathHelper.Clamp((int)((double)width * num3), this._zoomedMinimumSize.X, sdl_Rect.w), MathHelper.Clamp((int)((double)height * num3), this._zoomedMinimumSize.Y, sdl_Rect.h));
			}
			else
			{
				this._zoomedNormalSize = new Point(MathHelper.Clamp((int)((double)width * this._monitorZoom), this._zoomedMinimumSize.X, sdl_Rect.w), MathHelper.Clamp((int)((double)height * this._monitorZoom), this._zoomedMinimumSize.Y, sdl_Rect.h));
			}
			SDL.SDL_SetWindowMinimumSize(this.Handle, this._zoomedMinimumSize.X, this._zoomedMinimumSize.Y);
			bool flag = state == Window.WindowState.Normal || state == Window.WindowState.Minimized;
			if (flag)
			{
				SDL.SDL_SetWindowSize(this.Handle, this._zoomedNormalSize.X, this._zoomedNormalSize.Y);
				int x;
				int y;
				SDL.SDL_GetWindowPosition(this.Handle, out x, out y);
				SDL.SDL_SetWindowPosition(this.Handle, x, y);
				num = (width = this._zoomedNormalSize.X);
				num2 = (height = this._zoomedNormalSize.Y);
			}
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x001C3334 File Offset: 0x001C1534
		public Point TransformSDLToViewportCoords(int x, int y)
		{
			return new Point((int)((double)x * this._drawableScale) - this.Viewport.X, (int)((double)y * this._drawableScale) - this.Viewport.Y);
		}

		// Token: 0x06005A93 RID: 23187 RVA: 0x001C3378 File Offset: 0x001C1578
		public void UpdateSize(ScreenResolution resolution)
		{
			this._minimumSize.X = resolution.Width;
			this._minimumSize.Y = resolution.Height;
			this._zoomedMinimumSize.X = resolution.Width;
			this._zoomedMinimumSize.Y = resolution.Height;
			this._zoomedNormalSize.X = resolution.Width;
			this._zoomedNormalSize.Y = resolution.Height;
		}

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x06005A94 RID: 23188 RVA: 0x001C33EC File Offset: 0x001C15EC
		// (set) Token: 0x06005A95 RID: 23189 RVA: 0x001C33F4 File Offset: 0x001C15F4
		public bool IsMouseLocked { get; private set; }

		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x06005A96 RID: 23190 RVA: 0x001C33FD File Offset: 0x001C15FD
		// (set) Token: 0x06005A97 RID: 23191 RVA: 0x001C3405 File Offset: 0x001C1605
		public bool IsFocused { get; private set; } = true;

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x06005A98 RID: 23192 RVA: 0x001C340E File Offset: 0x001C160E
		// (set) Token: 0x06005A99 RID: 23193 RVA: 0x001C3416 File Offset: 0x001C1616
		public bool IsCursorVisible { get; private set; }

		// Token: 0x06005A9A RID: 23194 RVA: 0x001C3420 File Offset: 0x001C1620
		public void OnFocusChanged(bool isFocused)
		{
			this.IsFocused = isFocused;
			bool isMouseLocked = this.IsMouseLocked;
			if (isMouseLocked)
			{
				this.ApplyMouseSettings();
			}
		}

		// Token: 0x06005A9B RID: 23195 RVA: 0x001C3447 File Offset: 0x001C1647
		public void SetCursorVisible(bool visible)
		{
			this.IsCursorVisible = visible;
			this.ApplyMouseSettings();
		}

		// Token: 0x06005A9C RID: 23196 RVA: 0x001C345C File Offset: 0x001C165C
		public void SetMouseLock(bool enabled)
		{
			bool isMouseLocked = this.IsMouseLocked;
			this.IsMouseLocked = enabled;
			this.ApplyMouseSettings();
			int num;
			int num2;
			SDL.SDL_GetWindowSize(this.Handle, out num, out num2);
			bool flag = !enabled && isMouseLocked && this.IsFocused;
			if (flag)
			{
				SDL.SDL_WarpMouseInWindow(this.Handle, num / 2, num2 / 2);
			}
		}

		// Token: 0x06005A9D RID: 23197 RVA: 0x001C34B7 File Offset: 0x001C16B7
		private void ApplyMouseSettings()
		{
			SDL.SDL_SetRelativeMouseMode((this.IsMouseLocked && this.IsFocused && !this.IsCursorVisible) ? SDL.SDL_bool.SDL_TRUE : SDL.SDL_bool.SDL_FALSE);
		}

		// Token: 0x06005A9E RID: 23198 RVA: 0x001C34DC File Offset: 0x001C16DC
		public Vector2 SDLToNormalizedScreenCenterCoords(int x, int y)
		{
			Point point = this.TransformSDLToViewportCoords(x, y);
			float num = (float)this.Viewport.Width / 2f;
			float num2 = (float)this.Viewport.Height / 2f;
			return new Vector2(((float)point.X - num) / num, ((float)point.Y - num2) / num2);
		}

		// Token: 0x0400388B RID: 14475
		public readonly IntPtr Handle;

		// Token: 0x0400388C RID: 14476
		private readonly IntPtr _win32Handle;

		// Token: 0x0400388D RID: 14477
		public readonly uint Id;

		// Token: 0x04003891 RID: 14481
		public readonly double MinAspectRatio;

		// Token: 0x04003892 RID: 14482
		public readonly double MaxAspectRatio;

		// Token: 0x04003893 RID: 14483
		private bool _borderless;

		// Token: 0x04003894 RID: 14484
		private Point _minimumSize;

		// Token: 0x04003895 RID: 14485
		private double _drawableScale = 1.0;

		// Token: 0x04003896 RID: 14486
		private double _monitorZoom = 1.0;

		// Token: 0x04003897 RID: 14487
		private Point _zoomedMinimumSize;

		// Token: 0x04003898 RID: 14488
		private Point _zoomedNormalSize;

		// Token: 0x02000F68 RID: 3944
		public enum WindowState
		{
			// Token: 0x04004AE1 RID: 19169
			Normal,
			// Token: 0x04004AE2 RID: 19170
			Minimized,
			// Token: 0x04004AE3 RID: 19171
			Maximized,
			// Token: 0x04004AE4 RID: 19172
			Fullscreen
		}

		// Token: 0x02000F69 RID: 3945
		public class WindowSettings
		{
			// Token: 0x04004AE5 RID: 19173
			public string Title;

			// Token: 0x04004AE6 RID: 19174
			public Image Icon;

			// Token: 0x04004AE7 RID: 19175
			public bool Resizable;

			// Token: 0x04004AE8 RID: 19176
			public bool Borderless;

			// Token: 0x04004AE9 RID: 19177
			public Window.WindowState InitialState;

			// Token: 0x04004AEA RID: 19178
			public Point MinimumSize;

			// Token: 0x04004AEB RID: 19179
			public Point InitialSize;

			// Token: 0x04004AEC RID: 19180
			public float MinAspectRatio = 0f;

			// Token: 0x04004AED RID: 19181
			public float MaxAspectRatio = float.PositiveInfinity;
		}
	}
}
