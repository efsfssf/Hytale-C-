using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Coherent.UI;
using HytaleClient.Core;
using HytaleClient.Math;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.Interface.CoherentUI
{
	// Token: 0x020008DB RID: 2267
	internal static class CoUIViewInputForwarder
	{
		// Token: 0x06004201 RID: 16897 RVA: 0x000C67B8 File Offset: 0x000C49B8
		static CoUIViewInputForwarder()
		{
			CoUIViewInputForwarder.KeysMap = new Dictionary<SDL.SDL_Keycode, int>(33);
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_INSERT] = 45;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_HOME] = 36;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_DELETE] = 46;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_END] = 35;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_PAGEDOWN] = 34;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_RIGHT] = 39;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_LEFT] = 37;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_DOWN] = 40;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_UP] = 38;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_LSHIFT] = 160;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_RSHIFT] = 16;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_LCTRL] = 17;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_RCTRL] = 17;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_LALT] = 18;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_RALT] = 18;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_CAPSLOCK] = 20;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F1] = 112;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F2] = 113;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F3] = 114;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F4] = 115;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F5] = 116;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F6] = 117;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F7] = 118;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F8] = 119;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F9] = 120;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F10] = 121;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F11] = 122;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_F12] = 123;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_RETURN] = 13;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_BACKSPACE] = 8;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_KP_ENTER] = 13;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_LGUI] = 91;
			CoUIViewInputForwarder.KeysMap[SDL.SDL_Keycode.SDLK_RGUI] = 93;
			bool flag = 33 != CoUIViewInputForwarder.KeysMap.Count;
			if (flag)
			{
				throw new Exception("keysMap capacity should be updated.");
			}
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x000C6A60 File Offset: 0x000C4C60
		public unsafe static void OnUserInput(WebView webView, SDL.SDL_Event evt, Window window)
		{
			SDL.SDL_EventType type = evt.type;
			SDL.SDL_EventType sdl_EventType = type;
			if (sdl_EventType - SDL.SDL_EventType.SDL_KEYDOWN > 1U)
			{
				if (sdl_EventType != SDL.SDL_EventType.SDL_TEXTINPUT)
				{
					if (sdl_EventType - SDL.SDL_EventType.SDL_MOUSEMOTION <= 3U)
					{
						Point mousePosition = window.TransformSDLToViewportCoords(evt.button.x, evt.button.y);
						Point mouseWheel = new Point(evt.wheel.x, evt.wheel.y);
						CoUIViewInputForwarder.SendMouseEvent(webView, evt.type, (int)evt.button.button, mousePosition, mouseWheel);
					}
				}
				else
				{
					byte* ptr = &evt.text.text.FixedElementField;
					while (*ptr > 0)
					{
						ptr++;
					}
					int num = (int)((long)(ptr - &evt.text.text.FixedElementField));
					Marshal.Copy((IntPtr)((void*)(&evt.text.text.FixedElementField)), CoUIViewInputForwarder.TextBytes, 0, num);
					string @string = Encoding.UTF8.GetString(CoUIViewInputForwarder.TextBytes, 0, num);
					CoUIViewInputForwarder.SendTextInputEvent(webView, @string);
				}
			}
			else
			{
				SDL.SDL_Keycode sym = evt.key.keysym.sym;
				SDL.SDL_Scancode scancode = evt.key.keysym.scancode;
				CoUIViewInputForwarder.SendKeyboardEvent(webView, evt.type, sym, scancode, evt.key.repeat, evt.key.keysym.mod);
			}
		}

		// Token: 0x06004203 RID: 16899 RVA: 0x000C6BD8 File Offset: 0x000C4DD8
		public static void SendKeyboardEvent(WebView webView, SDL.SDL_EventType eventType, SDL.SDL_Keycode keycode, SDL.SDL_Scancode scancode, byte repeat, SDL.SDL_Keymod keymod)
		{
			CoUIViewInputForwarder.KeyEventData.Type = ((eventType == SDL.SDL_EventType.SDL_KEYDOWN) ? 1 : 2);
			CoUIViewInputForwarder.KeyEventData.IsAutoRepeat = (repeat > 0);
			CoUIViewInputForwarder.KeyEventData.IsNumPad = (keycode >= SDL.SDL_Keycode.SDLK_KP_DIVIDE && keycode <= SDL.SDL_Keycode.SDLK_KP_PERIOD);
			CoUIViewInputForwarder.SetModifiersState(CoUIViewInputForwarder.KeyEventData.Modifiers, keymod);
			bool flag = scancode >= SDL.SDL_Scancode.SDL_SCANCODE_MINUS && scancode <= SDL.SDL_Scancode.SDL_SCANCODE_SLASH;
			if (!flag)
			{
				int keyCode;
				bool flag2 = CoUIViewInputForwarder.KeysMap.TryGetValue(keycode, out keyCode);
				if (flag2)
				{
					CoUIViewInputForwarder.KeyEventData.KeyCode = keyCode;
					switch (keycode)
					{
					case SDL.SDL_Keycode.SDLK_LCTRL:
					case SDL.SDL_Keycode.SDLK_RCTRL:
						CoUIViewInputForwarder.KeyEventData.Modifiers.IsCtrlDown = (eventType == SDL.SDL_EventType.SDL_KEYDOWN);
						break;
					case SDL.SDL_Keycode.SDLK_LALT:
					case SDL.SDL_Keycode.SDLK_RALT:
						CoUIViewInputForwarder.KeyEventData.Modifiers.IsAltDown = (eventType == SDL.SDL_EventType.SDL_KEYDOWN);
						break;
					case SDL.SDL_Keycode.SDLK_LGUI:
					case SDL.SDL_Keycode.SDLK_RGUI:
						CoUIViewInputForwarder.KeyEventData.Modifiers.IsMetaDown = (eventType == SDL.SDL_EventType.SDL_KEYDOWN);
						break;
					}
					webView.KeyEvent(CoUIViewInputForwarder.KeyEventData);
				}
				else
				{
					CoUIViewInputForwarder.KeyEventData.KeyCode = (int)char.ToUpperInvariant((char)keycode);
					webView.KeyEvent(CoUIViewInputForwarder.KeyEventData);
					bool flag3 = CoUIViewInputForwarder.KeyEventData.Type == 1;
					if (flag3)
					{
						CoUIViewInputForwarder.KeyEventData.Type = 3;
						bool isCtrlDown = CoUIViewInputForwarder.KeyEventData.Modifiers.IsCtrlDown;
						if (!isCtrlDown)
						{
							bool flag4 = CoUIViewInputForwarder.KeyEventData.Modifiers.IsAltDown && BuildInfo.Platform != Platform.MacOS;
							if (flag4)
							{
								CoUIViewInputForwarder.KeyEventData.KeyCode = (int)char.ToLowerInvariant((char)CoUIViewInputForwarder.KeyEventData.KeyCode);
								webView.KeyEvent(CoUIViewInputForwarder.KeyEventData);
							}
							else
							{
								bool isMetaDown = CoUIViewInputForwarder.KeyEventData.Modifiers.IsMetaDown;
								if (isMetaDown)
								{
									CoUIViewInputForwarder.KeyEventData.KeyCode = (int)char.ToLowerInvariant((char)CoUIViewInputForwarder.KeyEventData.KeyCode);
									webView.KeyEvent(CoUIViewInputForwarder.KeyEventData);
								}
							}
						}
					}
				}
			}
			bool flag5 = eventType == SDL.SDL_EventType.SDL_KEYDOWN;
			if (flag5)
			{
				if (keycode == SDL.SDL_Keycode.SDLK_RETURN || keycode == SDL.SDL_Keycode.SDLK_KP_ENTER)
				{
					CoUIViewInputForwarder.KeyEventData.Type = 3;
					webView.KeyEvent(CoUIViewInputForwarder.KeyEventData);
				}
			}
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x000C6E44 File Offset: 0x000C5044
		public static void SendTextInputEvent(WebView webView, string text)
		{
			CoUIViewInputForwarder.KeyEventData.Type = 3;
			for (int i = 0; i < text.Length; i++)
			{
				CoUIViewInputForwarder.KeyEventData.KeyCode = (int)text[i];
				webView.KeyEvent(CoUIViewInputForwarder.KeyEventData);
			}
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x000C6E94 File Offset: 0x000C5094
		public static void SendMouseEvent(WebView webView, SDL.SDL_EventType eventType, int mouseButton, Point mousePosition, Point mouseWheel)
		{
			uint num = SDL.SDL_GetMouseState(IntPtr.Zero, IntPtr.Zero);
			CoUIViewInputForwarder.MouseEventData.MouseModifiers.IsLeftButtonDown = ((num & SDL.SDL_BUTTON_LMASK) > 0U);
			CoUIViewInputForwarder.MouseEventData.MouseModifiers.IsMiddleButtonDown = ((num & SDL.SDL_BUTTON_MMASK) > 0U);
			CoUIViewInputForwarder.MouseEventData.MouseModifiers.IsRightButtonDown = ((num & SDL.SDL_BUTTON_RMASK) > 0U);
			CoUIViewInputForwarder.SetModifiersState(CoUIViewInputForwarder.MouseEventData.Modifiers, SDL.SDL_GetModState());
			CoUIViewInputForwarder.MouseEventData.WheelX = 0f;
			CoUIViewInputForwarder.MouseEventData.WheelY = 0f;
			bool flag = eventType == SDL.SDL_EventType.SDL_MOUSEWHEEL;
			if (flag)
			{
				bool isShiftDown = CoUIViewInputForwarder.MouseEventData.Modifiers.IsShiftDown;
				if (isShiftDown)
				{
					CoUIViewInputForwarder.MouseEventData.WheelX = (float)mouseWheel.Y;
					CoUIViewInputForwarder.MouseEventData.WheelY = (float)mouseWheel.X;
				}
				else
				{
					CoUIViewInputForwarder.MouseEventData.WheelY = (float)mouseWheel.Y;
					CoUIViewInputForwarder.MouseEventData.WheelX = (float)mouseWheel.X;
				}
			}
			else
			{
				CoUIViewInputForwarder.MouseEventData.X = mousePosition.X;
				CoUIViewInputForwarder.MouseEventData.Y = mousePosition.Y;
			}
			switch (mouseButton)
			{
			case 1:
				CoUIViewInputForwarder.MouseEventData.Button = 0;
				break;
			case 2:
				CoUIViewInputForwarder.MouseEventData.Button = 1;
				break;
			case 3:
				CoUIViewInputForwarder.MouseEventData.Button = 2;
				break;
			}
			switch (eventType)
			{
			case SDL.SDL_EventType.SDL_MOUSEMOTION:
				CoUIViewInputForwarder.MouseEventData.Type = 0;
				break;
			case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
				CoUIViewInputForwarder.MouseEventData.Type = 1;
				break;
			case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
				CoUIViewInputForwarder.MouseEventData.Type = 2;
				break;
			case SDL.SDL_EventType.SDL_MOUSEWHEEL:
				CoUIViewInputForwarder.MouseEventData.Type = 3;
				break;
			}
			webView.MouseEvent(CoUIViewInputForwarder.MouseEventData);
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x000C7080 File Offset: 0x000C5280
		public static void ResetMousePosition(WebView webView, Window window)
		{
			CoUIViewInputForwarder.MouseEventData.Type = 0;
			CoUIViewInputForwarder.MouseEventData.X = window.Viewport.Width / 2;
			CoUIViewInputForwarder.MouseEventData.Y = window.Viewport.Height / 2;
			CoUIViewInputForwarder.MouseEventData.MouseModifiers.IsLeftButtonDown = false;
			CoUIViewInputForwarder.MouseEventData.MouseModifiers.IsMiddleButtonDown = false;
			CoUIViewInputForwarder.MouseEventData.MouseModifiers.IsLeftButtonDown = false;
			CoUIViewInputForwarder.MouseEventData.WheelX = 0f;
			CoUIViewInputForwarder.MouseEventData.WheelY = 0f;
			webView.MouseEvent(CoUIViewInputForwarder.MouseEventData);
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x000C712C File Offset: 0x000C532C
		private static void SetModifiersState(EventModifiersState coUImods, SDL.SDL_Keymod mods)
		{
			bool flag = (mods & SDL.SDL_Keymod.KMOD_RALT) != SDL.SDL_Keymod.KMOD_NONE || ((mods & SDL.SDL_Keymod.KMOD_LCTRL) != SDL.SDL_Keymod.KMOD_NONE && (mods & SDL.SDL_Keymod.KMOD_LALT) > SDL.SDL_Keymod.KMOD_NONE);
			coUImods.IsNumLockOn = ((mods & SDL.SDL_Keymod.KMOD_NUM) > SDL.SDL_Keymod.KMOD_NONE);
			coUImods.IsCapsOn = ((mods & SDL.SDL_Keymod.KMOD_CAPS) > SDL.SDL_Keymod.KMOD_NONE);
			coUImods.IsCtrlDown = ((mods & SDL.SDL_Keymod.KMOD_CTRL) != SDL.SDL_Keymod.KMOD_NONE && !flag);
			coUImods.IsAltDown = ((mods & SDL.SDL_Keymod.KMOD_ALT) != SDL.SDL_Keymod.KMOD_NONE && !flag);
			coUImods.IsShiftDown = ((mods & SDL.SDL_Keymod.KMOD_SHIFT) > SDL.SDL_Keymod.KMOD_NONE);
			coUImods.IsMetaDown = ((mods & SDL.SDL_Keymod.KMOD_GUI) > SDL.SDL_Keymod.KMOD_NONE);
		}

		// Token: 0x0400204A RID: 8266
		private static readonly Dictionary<SDL.SDL_Keycode, int> KeysMap;

		// Token: 0x0400204B RID: 8267
		private static readonly KeyEventData KeyEventData = new KeyEventData();

		// Token: 0x0400204C RID: 8268
		private static readonly MouseEventData MouseEventData = new MouseEventData();

		// Token: 0x0400204D RID: 8269
		private static readonly byte[] TextBytes = new byte[256];
	}
}
