using System;
using System.Collections.Generic;
using HytaleClient.Core;
using SDL2;

namespace HytaleClient.InGame.Modules.Shortcuts
{
	// Token: 0x02000900 RID: 2304
	internal class KeybindShortcut : Shortcut
	{
		// Token: 0x0600452E RID: 17710 RVA: 0x000F0CF0 File Offset: 0x000EEEF0
		public KeybindShortcut(string keys, string command) : base(keys, command)
		{
			this._keycodes = this.ParseKeybindings(keys);
			base.Name = this.GetKeyNames();
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x000F0D2C File Offset: 0x000EEF2C
		private SDL.SDL_Keycode[] ParseKeybindings(string keyStr)
		{
			keyStr = keyStr.Trim().ToLower();
			string[] array = keyStr.Split(new char[]
			{
				'+'
			});
			List<SDL.SDL_Keycode> list = new List<SDL.SDL_Keycode>();
			foreach (string text in array)
			{
				bool flag = this.ParseModifierKey(text);
				if (!flag)
				{
					string name = KeybindShortcut._replaceKeys.ContainsKey(text) ? KeybindShortcut._replaceKeys[text] : text;
					SDL.SDL_Keycode sdl_Keycode = SDL.SDL_GetKeyFromName(name);
					bool flag2 = sdl_Keycode > SDL.SDL_Keycode.SDLK_UNKNOWN;
					if (!flag2)
					{
						throw new Exception(text);
					}
					list.Add(sdl_Keycode);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x000F0DDC File Offset: 0x000EEFDC
		private bool ParseModifierKey(string str)
		{
			bool flag = str.IndexOf("shift") > -1;
			bool result;
			if (flag)
			{
				this._shiftMod = true;
				result = true;
			}
			else
			{
				bool flag2 = str.IndexOf("ctrl") > -1;
				if (flag2)
				{
					this._ctrlMod = true;
					result = true;
				}
				else
				{
					bool flag3 = str.IndexOf("alt") > -1;
					if (flag3)
					{
						this._altMod = true;
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x000F0E4C File Offset: 0x000EF04C
		public bool IsActive(Input input)
		{
			bool flag = !this.CheckModifiers(input);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < this._keycodes.Length; i++)
				{
					bool flag2 = !input.IsKeyHeld(SDL.SDL_GetScancodeFromKey(this._keycodes[i]), false);
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x000F0EAC File Offset: 0x000EF0AC
		private bool CheckModifiers(Input input)
		{
			bool flag = (this._shiftMod && !input.IsShiftHeld()) || (!this._shiftMod && input.IsShiftHeld());
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = (this._ctrlMod && !input.IsCtrlHeld()) || (!this._ctrlMod && input.IsCtrlHeld());
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = (this._altMod && !input.IsAltHeld()) || (!this._altMod && input.IsAltHeld());
					result = !flag3;
				}
			}
			return result;
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x000F0F44 File Offset: 0x000EF144
		public bool ConsumeKeybinds(Input input)
		{
			for (int i = 0; i < this._keycodes.Length; i++)
			{
				bool flag = !input.ConsumeKey(SDL.SDL_GetScancodeFromKey(this._keycodes[i]), false);
				if (flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x000F0F90 File Offset: 0x000EF190
		private string GetKeyNames()
		{
			string text = "";
			for (int i = 0; i < this._keycodes.Length; i++)
			{
				bool flag = i > 0;
				if (flag)
				{
					text += "+";
				}
				text += KeybindShortcut.GetKeyName(this._keycodes[i]);
			}
			string str = "";
			bool shiftMod = this._shiftMod;
			if (shiftMod)
			{
				str = "Shift+";
			}
			bool ctrlMod = this._ctrlMod;
			if (ctrlMod)
			{
				str += "Ctrl+";
			}
			bool altMod = this._altMod;
			if (altMod)
			{
				str += "Alt+";
			}
			return str + text;
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x000F103C File Offset: 0x000EF23C
		public static string FixKeyList(string keyStr)
		{
			keyStr = keyStr.Trim().ToLower();
			string[] array = keyStr.Split(new char[]
			{
				'+'
			});
			List<SDL.SDL_Keycode> list = new List<SDL.SDL_Keycode>();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			foreach (string text in array)
			{
				bool flag4 = text.IndexOf("shift") > -1;
				if (flag4)
				{
					flag = true;
				}
				else
				{
					bool flag5 = text.IndexOf("ctrl") > -1;
					if (flag5)
					{
						flag2 = true;
					}
					else
					{
						bool flag6 = text.IndexOf("alt") > -1;
						if (flag6)
						{
							flag3 = true;
						}
						else
						{
							string name = KeybindShortcut._replaceKeys.ContainsKey(text) ? KeybindShortcut._replaceKeys[text] : text;
							SDL.SDL_Keycode sdl_Keycode = SDL.SDL_GetKeyFromName(name);
							bool flag7 = sdl_Keycode > SDL.SDL_Keycode.SDLK_UNKNOWN;
							if (!flag7)
							{
								throw new Exception(text);
							}
							list.Add(sdl_Keycode);
						}
					}
				}
			}
			SDL.SDL_Keycode[] array3 = list.ToArray();
			string text2 = "";
			for (int j = 0; j < array3.Length; j++)
			{
				bool flag8 = j > 0;
				if (flag8)
				{
					text2 += "+";
				}
				text2 += KeybindShortcut.GetKeyName(array3[j]);
			}
			string str = "";
			bool flag9 = flag;
			if (flag9)
			{
				str = "Shift+";
			}
			bool flag10 = flag2;
			if (flag10)
			{
				str += "Ctrl+";
			}
			bool flag11 = flag3;
			if (flag11)
			{
				str += "Alt+";
			}
			return str + text2;
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x000F11D8 File Offset: 0x000EF3D8
		private static string GetKeyName(SDL.SDL_Keycode key)
		{
			string text = SDL.SDL_GetKeyName(key);
			text = text.Substring(text.IndexOf("_") + 1).ToLower();
			for (int i = 0; i < KeybindShortcut._fixedNames.Length; i++)
			{
				bool flag = text == KeybindShortcut._fixedNames[i].ToLower();
				if (flag)
				{
					return KeybindShortcut._fixedNames[i];
				}
			}
			bool flag2 = text.Length > 1;
			if (flag2)
			{
				text = char.ToUpper(text[0]).ToString() + text.Substring(1);
			}
			else
			{
				text = text.ToUpper();
			}
			return text;
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x000F127F File Offset: 0x000EF47F
		public override string ToString()
		{
			return this.GetKeyNames() + " - " + base.Command;
		}

		// Token: 0x040022B5 RID: 8885
		private static readonly string[] _fixedNames = new string[]
		{
			"PageUp",
			"PageDown",
			"ScrollLock",
			"CapsLock"
		};

		// Token: 0x040022B6 RID: 8886
		private static readonly Dictionary<string, string> _replaceKeys = new Dictionary<string, string>
		{
			{
				"kp1",
				"keypad 1"
			},
			{
				"kp2",
				"keypad 2"
			},
			{
				"kp3",
				"keypad 3"
			},
			{
				"kp4",
				"keypad 4"
			},
			{
				"kp5",
				"keypad 5"
			},
			{
				"kp6",
				"keypad 6"
			},
			{
				"kp7",
				"keypad 7"
			},
			{
				"kp8",
				"keypad 8"
			},
			{
				"kp9",
				"keypad 9"
			},
			{
				"kp0",
				"keypad 0"
			},
			{
				"kp/",
				"keypad /"
			},
			{
				"kp*",
				"keypad *"
			},
			{
				"kp-",
				"keypad -"
			},
			{
				"kpplus",
				"keypad +"
			},
			{
				"kpenter",
				"keypad enter"
			},
			{
				"kp.",
				"keypad ."
			}
		};

		// Token: 0x040022B7 RID: 8887
		private const char KeybindSeperator = '+';

		// Token: 0x040022B8 RID: 8888
		private SDL.SDL_Keycode[] _keycodes;

		// Token: 0x040022B9 RID: 8889
		private bool _shiftMod = false;

		// Token: 0x040022BA RID: 8890
		private bool _ctrlMod = false;

		// Token: 0x040022BB RID: 8891
		private bool _altMod = false;
	}
}
