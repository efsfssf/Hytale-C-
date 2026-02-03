using System;
using HytaleClient.Core;
using SDL2;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD2 RID: 2770
	internal class InputBinding
	{
		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06005767 RID: 22375 RVA: 0x001A7BF5 File Offset: 0x001A5DF5
		// (set) Token: 0x06005768 RID: 22376 RVA: 0x001A7C04 File Offset: 0x001A5E04
		public SDL.SDL_Keycode? Keycode
		{
			get
			{
				return new SDL.SDL_Keycode?(this._keycode);
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.Type = InputBinding.BindingType.Keycode;
					this._keycode = value.Value;
					this.BoundInputLabel = SDL.SDL_GetKeyName(this._keycode);
				}
			}
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06005769 RID: 22377 RVA: 0x001A7C44 File Offset: 0x001A5E44
		// (set) Token: 0x0600576A RID: 22378 RVA: 0x001A7C54 File Offset: 0x001A5E54
		public Input.MouseButton? MouseButton
		{
			get
			{
				return new Input.MouseButton?(this._mouseButton);
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.Type = InputBinding.BindingType.MouseButton;
					this._mouseButton = value.Value;
					this.BoundInputLabel = InputBinding.GetMouseBoundInputLabel(this._mouseButton);
				}
			}
		}

		// Token: 0x0600576B RID: 22379 RVA: 0x001A7C94 File Offset: 0x001A5E94
		public InputBinding(InputBinding binding = null)
		{
			bool flag = binding != null;
			if (flag)
			{
				bool flag2 = binding.Type == InputBinding.BindingType.Keycode;
				if (flag2)
				{
					this.Keycode = binding.Keycode;
				}
				else
				{
					this.MouseButton = binding.MouseButton;
				}
			}
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x001A7CDC File Offset: 0x001A5EDC
		public static InputBinding FromScancode(SDL.SDL_Scancode scancode)
		{
			return new InputBinding(null)
			{
				Keycode = new SDL.SDL_Keycode?(SDL.SDL_GetKeyFromScancode(scancode))
			};
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x001A7CF6 File Offset: 0x001A5EF6
		public static InputBinding FromKeycode(SDL.SDL_Keycode keycode)
		{
			return new InputBinding(null)
			{
				Keycode = new SDL.SDL_Keycode?(keycode)
			};
		}

		// Token: 0x0600576E RID: 22382 RVA: 0x001A7D0B File Offset: 0x001A5F0B
		public static InputBinding FromMouseButton(Input.MouseButton button)
		{
			return new InputBinding(null)
			{
				MouseButton = new Input.MouseButton?(button)
			};
		}

		// Token: 0x0600576F RID: 22383 RVA: 0x001A7D20 File Offset: 0x001A5F20
		public static string GetMouseBoundInputLabel(Input.MouseButton mouseButton)
		{
			string result;
			switch (mouseButton)
			{
			case Input.MouseButton.SDL_BUTTON_LEFT:
				result = "Left Mouse";
				break;
			case Input.MouseButton.SDL_BUTTON_MIDDLE:
				result = "Middle Mouse";
				break;
			case Input.MouseButton.SDL_BUTTON_RIGHT:
				result = "Right Mouse";
				break;
			default:
			{
				string str = "Mouse ";
				int num = (int)mouseButton;
				result = str + num.ToString();
				break;
			}
			}
			return result;
		}

		// Token: 0x0400351F RID: 13599
		public int Id;

		// Token: 0x04003520 RID: 13600
		public InputBinding.BindingType Type;

		// Token: 0x04003521 RID: 13601
		private SDL.SDL_Keycode _keycode;

		// Token: 0x04003522 RID: 13602
		private Input.MouseButton _mouseButton;

		// Token: 0x04003523 RID: 13603
		public string BoundInputLabel;

		// Token: 0x02000F19 RID: 3865
		public enum BindingType : byte
		{
			// Token: 0x04004A12 RID: 18962
			Keycode,
			// Token: 0x04004A13 RID: 18963
			MouseButton
		}
	}
}
