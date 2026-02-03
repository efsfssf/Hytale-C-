using System;
using System.Collections.Generic;
using HytaleClient.Data.UserSettings;
using SDL2;

namespace HytaleClient.Core
{
	// Token: 0x02000B7E RID: 2942
	internal class Input
	{
		// Token: 0x06005A63 RID: 23139 RVA: 0x001C1D68 File Offset: 0x001BFF68
		public Input(Engine engine, InputBindings inputBindings)
		{
			this._engine = engine;
			this.SetInputBindings(inputBindings);
		}

		// Token: 0x06005A64 RID: 23140 RVA: 0x001C1DD8 File Offset: 0x001BFFD8
		public void SetInputBindings(InputBindings bindings)
		{
			this._inputBindings = bindings;
			this._keyStates = new Input.KeyState[this._inputBindings.AllBindings.Count];
			this._keyBehaviours = new Input.KeyBehaviour[this._inputBindings.AllBindings.Count];
			this._keyBehaviours[this._inputBindings.ToggleCrouch.Id] = new Input.KeyBehaviour
			{
				Toggle = true,
				ReverseInputId = this._inputBindings.Crouch.Id
			};
			this._keyBehaviours[this._inputBindings.ToggleSprint.Id] = new Input.KeyBehaviour
			{
				Toggle = true,
				CopyToInputId = this._inputBindings.Sprint.Id
			};
			this._keyBehaviours[this._inputBindings.ToggleWalk.Id] = new Input.KeyBehaviour
			{
				Toggle = true,
				CopyToInputId = this._inputBindings.Walk.Id
			};
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x001C1EEC File Offset: 0x001C00EC
		public void ResetKeys()
		{
			this._keysSet.Clear();
			this._keysDownSet.Clear();
			this._keysHeldSet.Clear();
			this._keysUpSet.Clear();
			for (int i = 0; i < this._keyStates.Length; i++)
			{
				this._keyStates[i] = default(Input.KeyState);
			}
		}

		// Token: 0x06005A66 RID: 23142 RVA: 0x001C1F55 File Offset: 0x001C0155
		public void ResetMouseButtons()
		{
			this._mouseButtonsSet.Clear();
			this._mouseButtonsDownSet.Clear();
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x001C1F70 File Offset: 0x001C0170
		public void EndUserInput()
		{
			this._keysDownSet.Clear();
			this._keysUpSet.Clear();
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x001C1F8C File Offset: 0x001C018C
		public void OnUserInput(SDL.SDL_Event evt)
		{
			SDL.SDL_EventType type = evt.type;
			SDL.SDL_EventType sdl_EventType = type;
			if (sdl_EventType <= SDL.SDL_EventType.SDL_KEYUP)
			{
				if (sdl_EventType == SDL.SDL_EventType.SDL_KEYDOWN)
				{
					bool flag = evt.key.repeat == 0;
					if (flag)
					{
						this._keysSet.Add(evt.key.keysym.sym);
						this._keysDownSet.Add(evt.key.keysym.sym);
						this._keysHeldSet.Add(evt.key.keysym.sym);
					}
					return;
				}
				if (sdl_EventType == SDL.SDL_EventType.SDL_KEYUP)
				{
					this._keysSet.Remove(evt.key.keysym.sym);
					this._keysHeldSet.Remove(evt.key.keysym.sym);
					this._keysUpSet.Add(evt.key.keysym.sym);
					return;
				}
			}
			else if (sdl_EventType != SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
			{
				if (sdl_EventType == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
				{
					this._mouseButtonsSet.Remove((Input.MouseButton)evt.button.button);
					this._mouseButtonsDownSet.Remove((Input.MouseButton)evt.button.button);
					return;
				}
			}
			else
			{
				bool mouseInputDisabled = this.MouseInputDisabled;
				if (mouseInputDisabled)
				{
					return;
				}
				this._mouseButtonsSet.Add((Input.MouseButton)evt.button.button);
				this._mouseButtonsDownSet.Add((Input.MouseButton)evt.button.button);
				return;
			}
			throw new ArgumentOutOfRangeException("evt", evt.type.ToString());
		}

		// Token: 0x06005A69 RID: 23145 RVA: 0x001C2134 File Offset: 0x001C0334
		public void UpdateBindings()
		{
			for (int i = 0; i < this._inputBindings.AllBindings.Count; i++)
			{
				InputBinding inputBinding = this._inputBindings.AllBindings[i];
				Input.KeyState keyState = this._keyStates[inputBinding.Id];
				Input.KeyBehaviour keyBehaviour = this._keyBehaviours[inputBinding.Id];
				keyState.Prev = keyState.Key;
				bool flag = inputBinding.Keycode != null;
				if (flag)
				{
					keyState.Key = this._keysHeldSet.Contains(inputBinding.Keycode.Value);
					keyState.Down = this._keysDownSet.Contains(inputBinding.Keycode.Value);
					keyState.Up = this._keysUpSet.Contains(inputBinding.Keycode.Value);
				}
				bool toggle = keyBehaviour.Toggle;
				if (toggle)
				{
					keyState.Key = (keyState.Down ? (!keyState.Prev) : keyState.Prev);
				}
				bool flag2 = keyBehaviour.CopyToInputId != 0;
				if (flag2)
				{
					Input.KeyState keyState2 = this._keyStates[keyBehaviour.CopyToInputId];
					keyState2.Prev |= keyState.Prev;
					keyState2.Key |= keyState.Key;
					keyState2.Down |= keyState.Down;
					keyState2.Up |= keyState.Up;
					this._keyStates[keyBehaviour.CopyToInputId] = keyState2;
				}
				bool flag3 = keyBehaviour.ReverseInputId != 0;
				if (flag3)
				{
					Input.KeyState keyState3 = this._keyStates[keyBehaviour.ReverseInputId];
					keyState3.Prev ^= keyState.Prev;
					keyState3.Key ^= keyState.Key;
					keyState3.Down ^= keyState.Down;
					keyState3.Up ^= keyState.Up;
					this._keyStates[keyBehaviour.ReverseInputId] = keyState3;
				}
				this._keyStates[inputBinding.Id] = keyState;
			}
		}

		// Token: 0x06005A6A RID: 23146 RVA: 0x001C2360 File Offset: 0x001C0560
		public static bool EventMatchesBinding(SDL.SDL_Event evt, InputBinding binding)
		{
			bool flag = binding.Type == InputBinding.BindingType.Keycode;
			bool result;
			if (flag)
			{
				result = ((evt.type == SDL.SDL_EventType.SDL_KEYDOWN || evt.type == SDL.SDL_EventType.SDL_KEYUP) && evt.key.keysym.sym == binding.Keycode.Value);
			}
			else
			{
				result = ((evt.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN || evt.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP) && evt.button.button == (byte)binding.MouseButton.Value);
			}
			return result;
		}

		// Token: 0x06005A6B RID: 23147 RVA: 0x001C23F8 File Offset: 0x001C05F8
		public bool ConsumeBinding(InputBinding binding, bool ignoreKeyInputDisabled = false)
		{
			bool flag = binding.Type == InputBinding.BindingType.Keycode;
			bool result;
			if (flag)
			{
				bool flag2 = this.KeyInputDisabled && !ignoreKeyInputDisabled;
				result = (!flag2 && this._keysSet.Remove(binding.Keycode.Value));
			}
			else
			{
				bool flag3 = !this._engine.Window.IsMouseLocked;
				result = (!flag3 && this._mouseButtonsSet.Remove(binding.MouseButton.Value));
			}
			return result;
		}

		// Token: 0x06005A6C RID: 23148 RVA: 0x001C2484 File Offset: 0x001C0684
		public bool ConsumeKey(SDL.SDL_Scancode scancode, bool ignoreKeyInputDisabled = false)
		{
			bool flag = this.KeyInputDisabled && !ignoreKeyInputDisabled;
			return !flag && this._keysSet.Remove(SDL.SDL_GetKeyFromScancode(scancode));
		}

		// Token: 0x06005A6D RID: 23149 RVA: 0x001C24C0 File Offset: 0x001C06C0
		public bool CanConsumeBinding(InputBinding binding, bool ignoreKeyInputDisabled = false)
		{
			bool flag = binding.Type == InputBinding.BindingType.Keycode;
			bool result;
			if (flag)
			{
				bool flag2 = this.KeyInputDisabled && !ignoreKeyInputDisabled;
				result = (!flag2 && this._keysSet.Contains(binding.Keycode.Value));
			}
			else
			{
				bool flag3 = !this._engine.Window.IsMouseLocked || this.MouseInputDisabled;
				result = (!flag3 && this._mouseButtonsSet.Contains(binding.MouseButton.Value));
			}
			return result;
		}

		// Token: 0x06005A6E RID: 23150 RVA: 0x001C2554 File Offset: 0x001C0754
		public bool IsBindingHeld(InputBinding binding, bool ignoreKeyInputDisabled = false)
		{
			bool flag = binding.Type == InputBinding.BindingType.Keycode;
			bool result;
			if (flag)
			{
				bool flag2 = this.KeyInputDisabled && !ignoreKeyInputDisabled;
				result = (!flag2 && this._keyStates[binding.Id].Key);
			}
			else
			{
				bool flag3 = !this._engine.Window.IsMouseLocked;
				result = (!flag3 && this._mouseButtonsDownSet.Contains(binding.MouseButton.Value));
			}
			return result;
		}

		// Token: 0x06005A6F RID: 23151 RVA: 0x001C25DC File Offset: 0x001C07DC
		public bool IsBindingDown(InputBinding binding, bool ignoreKeyInputDisabled = false)
		{
			bool flag = binding.Type == InputBinding.BindingType.Keycode;
			bool result;
			if (flag)
			{
				bool flag2 = this.KeyInputDisabled && !ignoreKeyInputDisabled;
				result = (!flag2 && this._keyStates[binding.Id].Down);
			}
			else
			{
				bool flag3 = !this._engine.Window.IsMouseLocked;
				result = (!flag3 && this._mouseButtonsDownSet.Contains(binding.MouseButton.Value));
			}
			return result;
		}

		// Token: 0x06005A70 RID: 23152 RVA: 0x001C2664 File Offset: 0x001C0864
		public bool IsBindingUp(InputBinding binding, bool ignoreKeyInputDisabled = false)
		{
			bool flag = binding.Type == InputBinding.BindingType.Keycode;
			bool result;
			if (flag)
			{
				bool flag2 = this.KeyInputDisabled && !ignoreKeyInputDisabled;
				result = (!flag2 && this._keyStates[binding.Id].Up);
			}
			else
			{
				bool flag3 = !this._engine.Window.IsMouseLocked;
				if (!flag3)
				{
					throw new NotSupportedException("Mouse Up event not supported.");
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06005A71 RID: 23153 RVA: 0x001C26D8 File Offset: 0x001C08D8
		public bool IsKeyHeld(SDL.SDL_Scancode scancode, bool ignoreKeyInputDisabled = false)
		{
			bool flag = this.KeyInputDisabled && !ignoreKeyInputDisabled;
			return !flag && this._keysHeldSet.Contains(SDL.SDL_GetKeyFromScancode(scancode));
		}

		// Token: 0x06005A72 RID: 23154 RVA: 0x001C2714 File Offset: 0x001C0914
		public bool IsAnyKeyHeld(bool ignoreKeyInputDisabled = false)
		{
			bool flag = this.KeyInputDisabled && !ignoreKeyInputDisabled;
			return !flag && this._keysHeldSet.Count > 0;
		}

		// Token: 0x06005A73 RID: 23155 RVA: 0x001C274C File Offset: 0x001C094C
		public bool IsShiftHeld()
		{
			return this.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT, false) || this.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_RSHIFT, false);
		}

		// Token: 0x06005A74 RID: 23156 RVA: 0x001C277C File Offset: 0x001C097C
		public bool IsAltHeld()
		{
			return this.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_LALT, false) || this.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_RALT, false);
		}

		// Token: 0x06005A75 RID: 23157 RVA: 0x001C27AC File Offset: 0x001C09AC
		public bool IsCtrlHeld()
		{
			return this.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_LCTRL, false) || this.IsKeyHeld(SDL.SDL_Scancode.SDL_SCANCODE_RCTRL, false);
		}

		// Token: 0x06005A76 RID: 23158 RVA: 0x001C27DC File Offset: 0x001C09DC
		public bool IsAnyModifierHeld()
		{
			return this.IsShiftHeld() || this.IsAltHeld() || this.IsCtrlHeld();
		}

		// Token: 0x06005A77 RID: 23159 RVA: 0x001C2808 File Offset: 0x001C0A08
		public bool IsOnlyShiftHeld()
		{
			return this.IsShiftHeld() && !this.IsAltHeld() && !this.IsCtrlHeld();
		}

		// Token: 0x06005A78 RID: 23160 RVA: 0x001C283C File Offset: 0x001C0A3C
		public bool IsOnlyAltHeld()
		{
			return this.IsAltHeld() && !this.IsShiftHeld() && !this.IsCtrlHeld();
		}

		// Token: 0x06005A79 RID: 23161 RVA: 0x001C2870 File Offset: 0x001C0A70
		public bool IsOnlyCtrlHeld()
		{
			return this.IsCtrlHeld() && !this.IsShiftHeld() && !this.IsAltHeld();
		}

		// Token: 0x06005A7A RID: 23162 RVA: 0x001C28A4 File Offset: 0x001C0AA4
		public bool IsMouseButtonDown(Input.MouseButton button)
		{
			return this._mouseButtonsDownSet.Contains(button);
		}

		// Token: 0x0400387A RID: 14458
		private readonly Engine _engine;

		// Token: 0x0400387B RID: 14459
		public bool KeyInputDisabled;

		// Token: 0x0400387C RID: 14460
		public bool MouseInputDisabled = false;

		// Token: 0x0400387D RID: 14461
		private readonly HashSet<SDL.SDL_Keycode> _keysSet = new HashSet<SDL.SDL_Keycode>();

		// Token: 0x0400387E RID: 14462
		private readonly HashSet<SDL.SDL_Keycode> _keysDownSet = new HashSet<SDL.SDL_Keycode>();

		// Token: 0x0400387F RID: 14463
		private readonly HashSet<SDL.SDL_Keycode> _keysHeldSet = new HashSet<SDL.SDL_Keycode>();

		// Token: 0x04003880 RID: 14464
		private readonly HashSet<SDL.SDL_Keycode> _keysUpSet = new HashSet<SDL.SDL_Keycode>();

		// Token: 0x04003881 RID: 14465
		private readonly HashSet<Input.MouseButton> _mouseButtonsSet = new HashSet<Input.MouseButton>();

		// Token: 0x04003882 RID: 14466
		private readonly HashSet<Input.MouseButton> _mouseButtonsDownSet = new HashSet<Input.MouseButton>();

		// Token: 0x04003883 RID: 14467
		private Input.KeyState[] _keyStates;

		// Token: 0x04003884 RID: 14468
		private InputBindings _inputBindings;

		// Token: 0x04003885 RID: 14469
		private Input.KeyBehaviour[] _keyBehaviours;

		// Token: 0x02000F65 RID: 3941
		public enum MouseButton : uint
		{
			// Token: 0x04004AD4 RID: 19156
			SDL_BUTTON_LEFT = 1U,
			// Token: 0x04004AD5 RID: 19157
			SDL_BUTTON_MIDDLE,
			// Token: 0x04004AD6 RID: 19158
			SDL_BUTTON_RIGHT,
			// Token: 0x04004AD7 RID: 19159
			SDL_BUTTON_X1,
			// Token: 0x04004AD8 RID: 19160
			SDL_BUTTON_X2
		}

		// Token: 0x02000F66 RID: 3942
		public struct KeyState
		{
			// Token: 0x04004AD9 RID: 19161
			public bool Key;

			// Token: 0x04004ADA RID: 19162
			public bool Prev;

			// Token: 0x04004ADB RID: 19163
			public bool Down;

			// Token: 0x04004ADC RID: 19164
			public bool Up;
		}

		// Token: 0x02000F67 RID: 3943
		public struct KeyBehaviour
		{
			// Token: 0x04004ADD RID: 19165
			public bool Toggle;

			// Token: 0x04004ADE RID: 19166
			public int CopyToInputId;

			// Token: 0x04004ADF RID: 19167
			public int ReverseInputId;
		}
	}
}
