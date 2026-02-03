using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BB8 RID: 3000
	internal class AutoCompleteMenu : Element
	{
		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x06005DEB RID: 24043 RVA: 0x001DF5B4 File Offset: 0x001DD7B4
		// (set) Token: 0x06005DEC RID: 24044 RVA: 0x001DF5BC File Offset: 0x001DD7BC
		public TextEditor TextEditor { get; private set; }

		// Token: 0x06005DED RID: 24045 RVA: 0x001DF5C8 File Offset: 0x001DD7C8
		public AutoCompleteMenu(Desktop desktop) : base(desktop, null)
		{
			this.Anchor = new Anchor
			{
				Height = new int?(80)
			};
			this.Padding = new Padding
			{
				Full = new int?(5)
			};
			this.Background = new PatchStyle(UInt32Color.Black);
			base.Visible = false;
			this._layoutMode = LayoutMode.TopScrolling;
			this._scrollbarStyle.Size = 8;
			this._scrollbarStyle.Spacing = 0;
		}

		// Token: 0x06005DEE RID: 24046 RVA: 0x001DF654 File Offset: 0x001DD854
		public void Build()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/AutoCompleteMenu.ui", out document);
			this._buttonStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "ButtonStyle");
			this._buttonFocusedStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "ButtonFocusedStyle");
			this._buttonPadding = document.ResolveNamedValue<Padding>(this.Desktop.Provider, "ButtonPadding");
			this._buttonAnchor = document.ResolveNamedValue<Anchor>(this.Desktop.Provider, "ButtonAnchor");
		}

		// Token: 0x06005DEF RID: 24047 RVA: 0x001DF6EC File Offset: 0x001DD8EC
		public void Open(TextEditor textEditor, int x, int y, int width)
		{
			this.TextEditor = textEditor;
			this.Anchor.Left = new int?(x);
			this.Anchor.Top = new int?(y);
			this.Anchor.Width = new int?(width);
			base.Visible = false;
			base.Clear();
			this.TextEditor.ConfigEditor.AssetEditorOverlay.Add(this, -1);
			base.Layout(new Rectangle?(this.Parent.AnchoredRectangle), true);
		}

		// Token: 0x06005DF0 RID: 24048 RVA: 0x001DF776 File Offset: 0x001DD976
		public void Close()
		{
			this.TextEditor.ConfigEditor.AssetEditorOverlay.Remove(this);
			this.TextEditor = null;
		}

		// Token: 0x06005DF1 RID: 24049 RVA: 0x001DF798 File Offset: 0x001DD998
		protected override void OnUnmounted()
		{
			this._focusedTag = null;
			this._strings = null;
		}

		// Token: 0x06005DF2 RID: 24050 RVA: 0x001DF7AC File Offset: 0x001DD9AC
		public void SetupResults(HashSet<string> strings)
		{
			Debug.Assert(strings != null);
			base.Clear();
			this._focusedTag = null;
			this._strings = strings;
			foreach (string text in strings)
			{
				AutoCompleteMenu.AutoCompleteMenuButton autoCompleteMenuButton = new AutoCompleteMenu.AutoCompleteMenuButton(this, text);
				autoCompleteMenuButton.Name = text;
				autoCompleteMenuButton.Text = text;
				autoCompleteMenuButton.Anchor = this._buttonAnchor;
				autoCompleteMenuButton.Padding = this._buttonPadding;
				autoCompleteMenuButton.Style = this._buttonStyle;
			}
			this.Anchor.Height = strings.Count * 20 + this.Padding.Vertical;
			base.Visible = (strings.Count > 0);
			base.Layout(new Rectangle?(this.Parent.AnchoredRectangle), true);
		}

		// Token: 0x06005DF3 RID: 24051 RVA: 0x001DF8BC File Offset: 0x001DDABC
		protected internal override void OnKeyDown(SDL.SDL_Keycode keyCode, int repeat)
		{
			base.OnKeyDown(keyCode, repeat);
			if (keyCode != SDL.SDL_Keycode.SDLK_DOWN)
			{
				if (keyCode == SDL.SDL_Keycode.SDLK_UP)
				{
					bool flag = this._strings.Count == 0;
					if (!flag)
					{
						this.SelectNext(true);
					}
				}
			}
			else
			{
				bool flag2 = this._strings.Count == 0;
				if (!flag2)
				{
					this.SelectNext(false);
				}
			}
		}

		// Token: 0x06005DF4 RID: 24052 RVA: 0x001DF92C File Offset: 0x001DDB2C
		private void SelectNext(bool invert)
		{
			bool flag = this._focusedTag != null;
			if (flag)
			{
				TextButton textButton = base.Find<TextButton>(this._focusedTag);
				textButton.Style = this._buttonStyle;
				textButton.Layout(null, true);
			}
			List<string> list = Enumerable.ToList<string>(this._strings);
			int num = (this._focusedTag == null) ? 0 : (list.IndexOf(this._focusedTag) + (invert ? -1 : 1));
			bool flag2 = num >= this._strings.Count;
			if (flag2)
			{
				num = 0;
			}
			else
			{
				bool flag3 = num < 0;
				if (flag3)
				{
					num = this._strings.Count - 1;
				}
			}
			this._focusedTag = list[num];
			TextButton textButton2 = base.Find<TextButton>(this._focusedTag);
			textButton2.Style = this._buttonFocusedStyle;
			textButton2.Layout(null, true);
		}

		// Token: 0x06005DF5 RID: 24053 RVA: 0x001DFA14 File Offset: 0x001DDC14
		protected internal override void Validate()
		{
			bool flag = this._focusedTag == null;
			if (!flag)
			{
				this.TextEditor.OnAutoCompleteSelectedValue(this._focusedTag);
				this.Desktop.FocusElement(null, true);
			}
		}

		// Token: 0x04003AB1 RID: 15025
		private HashSet<string> _strings;

		// Token: 0x04003AB2 RID: 15026
		private string _focusedTag;

		// Token: 0x04003AB3 RID: 15027
		private TextButton.TextButtonStyle _buttonStyle;

		// Token: 0x04003AB4 RID: 15028
		private TextButton.TextButtonStyle _buttonFocusedStyle;

		// Token: 0x04003AB5 RID: 15029
		private Padding _buttonPadding;

		// Token: 0x04003AB6 RID: 15030
		private Anchor _buttonAnchor;

		// Token: 0x02000FCF RID: 4047
		public class AutoCompleteMenuButton : TextButton
		{
			// Token: 0x060069B4 RID: 27060 RVA: 0x0021EF8E File Offset: 0x0021D18E
			public AutoCompleteMenuButton(AutoCompleteMenu parent, string tag) : base(parent.Desktop, parent)
			{
				this._tag = tag;
				this._menu = parent;
			}

			// Token: 0x060069B5 RID: 27061 RVA: 0x0021EFB0 File Offset: 0x0021D1B0
			protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
			{
				base.OnMouseButtonUp(evt, activate);
				bool flag = activate && (long)evt.Button == 1L && !this.Disabled;
				if (flag)
				{
					this._menu.TextEditor.OnAutoCompleteSelectedValue(this._tag);
				}
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this._menu.Close();
				}
			}

			// Token: 0x04004C12 RID: 19474
			private readonly string _tag;

			// Token: 0x04004C13 RID: 19475
			private readonly AutoCompleteMenu _menu;
		}
	}
}
