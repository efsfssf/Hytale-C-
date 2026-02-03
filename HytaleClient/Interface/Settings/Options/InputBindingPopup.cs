using System;
using System.Collections.Generic;
using HytaleClient.Core;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using SDL2;

namespace HytaleClient.Interface.Settings.Options
{
	// Token: 0x02000812 RID: 2066
	internal class InputBindingPopup : Panel
	{
		// Token: 0x06003953 RID: 14675 RVA: 0x0007A90C File Offset: 0x00078B0C
		public InputBindingPopup(SettingsComponent settingsComponent) : base(settingsComponent.Desktop, null)
		{
			this._settingsComponent = settingsComponent;
			Document document;
			settingsComponent.Interface.TryGetDocument("Common/Settings/InputBindingPopup.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._bindingName = uifragment.Get<Label>("BindingName");
		}

		// Token: 0x06003954 RID: 14676 RVA: 0x0007A964 File Offset: 0x00078B64
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			bool flag = keycode != SDL.SDL_Keycode.SDLK_ESCAPE;
			if (flag)
			{
				this._settingsComponent.OnInputBindingKeyPress(keycode);
			}
			else
			{
				base.OnKeyDown(keycode, repeat);
			}
		}

		// Token: 0x06003955 RID: 14677 RVA: 0x0007A996 File Offset: 0x00078B96
		protected override void OnMouseButtonDown(MouseButtonEvent @event)
		{
			this._settingsComponent.OnInputBindingMousePress((Input.MouseButton)@event.Button);
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x0007A9AC File Offset: 0x00078BAC
		public void Setup(string binding)
		{
			this._bindingName.Text = this.Desktop.Provider.GetText("ui.settings.bindingPopup.bind", new Dictionary<string, string>
			{
				{
					"binding",
					this.Desktop.Provider.GetText("ui.settings.bindings." + binding, null, true)
				}
			}, true);
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x0007AA0A File Offset: 0x00078C0A
		protected internal override void Dismiss()
		{
			this._settingsComponent.StopEditingInputBinding();
		}

		// Token: 0x0400193C RID: 6460
		private readonly Label _bindingName;

		// Token: 0x0400193D RID: 6461
		private readonly SettingsComponent _settingsComponent;
	}
}
