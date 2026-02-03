using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200085B RID: 2139
	[UIMarkupElement(AcceptsChildren = true)]
	public class DropdownBox : InputElement<string>
	{
		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06003B6D RID: 15213 RVA: 0x0008C673 File Offset: 0x0008A873
		// (set) Token: 0x06003B6E RID: 15214 RVA: 0x0008C67C File Offset: 0x0008A87C
		[UIMarkupProperty]
		public IReadOnlyList<DropdownBox.DropdownEntryInfo> Entries
		{
			get
			{
				return this._entries;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					this._entries = new List<DropdownBox.DropdownEntryInfo>();
				}
				else
				{
					List<DropdownBox.DropdownEntryInfo> list = value as List<DropdownBox.DropdownEntryInfo>;
					this._entries = ((list != null) ? list : new List<DropdownBox.DropdownEntryInfo>(value));
				}
				this._dropdownLayer.OnEntriesChanged();
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06003B6F RID: 15215 RVA: 0x0008C6C8 File Offset: 0x0008A8C8
		// (set) Token: 0x06003B70 RID: 15216 RVA: 0x0008C6D0 File Offset: 0x0008A8D0
		[UIMarkupProperty]
		public List<string> SelectedValues
		{
			get
			{
				return this._selectedValues;
			}
			set
			{
				this._selectedValues = (value ?? new List<string>());
				this._dropdownLayer.OnSelectedValuesChanged();
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06003B71 RID: 15217 RVA: 0x0008C6EF File Offset: 0x0008A8EF
		// (set) Token: 0x06003B72 RID: 15218 RVA: 0x0008C6FC File Offset: 0x0008A8FC
		public override string Value
		{
			get
			{
				return Enumerable.FirstOrDefault<string>(this._selectedValues);
			}
			set
			{
				bool flag = this._selectedValues.Count == 0;
				if (flag)
				{
					this._selectedValues.Add(value ?? "");
				}
				else
				{
					this._selectedValues[0] = (value ?? "");
				}
				this._dropdownLayer.OnSelectedValuesChanged();
			}
		}

		// Token: 0x1700103C RID: 4156
		// (set) Token: 0x06003B73 RID: 15219 RVA: 0x0008C75B File Offset: 0x0008A95B
		public Action DropdownToggled
		{
			set
			{
				this._dropdownLayer.DropdownToggled = value;
			}
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06003B74 RID: 15220 RVA: 0x0008C769 File Offset: 0x0008A969
		public bool IsOpen
		{
			get
			{
				return this._dropdownLayer.IsMounted;
			}
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0008C778 File Offset: 0x0008A978
		public List<int> selectedIndexes()
		{
			List<int> list = new List<int>();
			int num = 0;
			foreach (DropdownBox.DropdownEntryInfo dropdownEntryInfo in this._entries)
			{
				bool flag = this._selectedValues.Contains(dropdownEntryInfo.Value);
				if (flag)
				{
					list.Add(num);
				}
				num++;
			}
			return list;
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0008C7FC File Offset: 0x0008A9FC
		public DropdownBox(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._entries = new List<DropdownBox.DropdownEntryInfo>();
			this._selectedValues = new List<string>();
			this._icon = new Group(this.Desktop, this);
			this._label = new Label(this.Desktop, this);
			this._arrow = new Element(this.Desktop, this);
			this._dropdownLayer = new DropdownLayer(this);
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0008C884 File Offset: 0x0008AA84
		internal override void AddFromMarkup(Element child)
		{
			DropdownEntry dropdownEntry = child as DropdownEntry;
			bool flag = dropdownEntry == null;
			if (flag)
			{
				throw new Exception("Children of DropdownBox must be of type DropdownEntry");
			}
			this._dropdownLayer.AddEntry(dropdownEntry);
			this._entries.Add(new DropdownBox.DropdownEntryInfo(dropdownEntry.Text, dropdownEntry.Value, false));
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0008C8DA File Offset: 0x0008AADA
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06003B79 RID: 15225 RVA: 0x0008C8F0 File Offset: 0x0008AAF0
		protected override void OnUnmounted()
		{
			bool isMounted = this._dropdownLayer.IsMounted;
			if (isMounted)
			{
				this.CloseDropdown(false);
			}
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x0008C918 File Offset: 0x0008AB18
		protected override void ApplyStyles()
		{
			bool disabled = this.Disabled;
			if (disabled)
			{
				this.Background = (this.Style.DisabledBackground ?? this.Style.DefaultBackground);
				this._arrow.Background = new PatchStyle
				{
					TexturePath = (this.Style.DisabledArrowTexturePath ?? this.Style.DefaultArrowTexturePath)
				};
			}
			else
			{
				bool flag = this._dropdownLayer.IsMounted || (base.CapturedMouseButton != null && (long)base.CapturedMouseButton.Value == 1L);
				if (flag)
				{
					PatchStyle background;
					if ((background = this.Style.PressedBackground) == null)
					{
						background = (this.Style.HoveredBackground ?? this.Style.DefaultBackground);
					}
					this.Background = background;
					Element arrow = this._arrow;
					PatchStyle patchStyle = new PatchStyle();
					UIPath texturePath;
					if ((texturePath = this.Style.PressedArrowTexturePath) == null)
					{
						texturePath = (this.Style.HoveredArrowTexturePath ?? this.Style.DefaultArrowTexturePath);
					}
					patchStyle.TexturePath = texturePath;
					arrow.Background = patchStyle;
				}
				else
				{
					bool isHovered = base.IsHovered;
					if (isHovered)
					{
						this.Background = (this.Style.HoveredBackground ?? this.Style.DefaultBackground);
						this._arrow.Background = new PatchStyle
						{
							TexturePath = (this.Style.HoveredArrowTexturePath ?? this.Style.DefaultArrowTexturePath)
						};
					}
					else
					{
						this.Background = this.Style.DefaultBackground;
						this._arrow.Background = new PatchStyle
						{
							TexturePath = this.Style.DefaultArrowTexturePath
						};
					}
				}
			}
			base.ApplyStyles();
			bool flag2 = this.Style.IconTexturePath != null;
			if (flag2)
			{
				this._icon.Anchor.Width = new int?(this.Style.IconWidth);
				this._icon.Anchor.Height = new int?(this.Style.IconHeight);
				this._icon.Anchor.Left = new int?(this.Style.HorizontalPadding);
				this._icon.Background = new PatchStyle
				{
					TexturePath = this.Style.IconTexturePath
				};
			}
			this._arrow.Anchor.Width = new int?(this.Style.ArrowWidth);
			this._arrow.Anchor.Height = new int?(this.Style.ArrowHeight);
			this._arrow.Anchor.Right = new int?(this.Style.HorizontalPadding);
			bool flag3 = this.MaxSelection == 1 && this.ShowLabel;
			if (flag3)
			{
				LabelStyle labelStyle = (this.Disabled && this.Style.DisabledLabelStyle != null) ? this.Style.DisabledLabelStyle : this.Style.LabelStyle;
				bool flag4 = labelStyle != null;
				if (flag4)
				{
					this._label.Style = labelStyle;
				}
				string firstValue = Enumerable.FirstOrDefault<string>(this.SelectedValues);
				string text = this.DisplayNonExistingValue ? firstValue : "";
				Label label = this._label;
				string text2;
				if (this._entries.Count <= 0)
				{
					text2 = text;
				}
				else
				{
					DropdownBox.DropdownEntryInfo dropdownEntryInfo = Enumerable.FirstOrDefault<DropdownBox.DropdownEntryInfo>(this._entries, (DropdownBox.DropdownEntryInfo e) => e.Value.Equals(firstValue));
					text2 = (((dropdownEntryInfo != null) ? dropdownEntryInfo.Label : null) ?? text);
				}
				label.Text = text2;
				this._label.Anchor.Left = (this._label.Anchor.Right = new int?(this.Style.HorizontalPadding));
			}
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x0008CCE0 File Offset: 0x0008AEE0
		protected override void OnMouseEnter()
		{
			bool disabled = this.Disabled;
			if (disabled)
			{
				SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
			}
			else
			{
				SDL.SDL_SetCursor(this.Desktop.Cursors.Hand);
				DropdownBoxSounds sounds = this.Style.Sounds;
				bool flag = ((sounds != null) ? sounds.MouseHover : null) != null;
				if (flag)
				{
					this.Desktop.Provider.PlaySound(this.Style.Sounds.MouseHover);
				}
			}
			base.Layout(null, true);
		}

		// Token: 0x06003B7C RID: 15228 RVA: 0x0008CD7C File Offset: 0x0008AF7C
		protected override void OnMouseLeave()
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
			base.Layout(null, true);
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x0008CDB4 File Offset: 0x0008AFB4
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = this.Disabled || (long)evt.Button != 1L;
			if (!flag)
			{
				DropdownBoxSounds sounds = this.Style.Sounds;
				bool flag2 = ((sounds != null) ? sounds.Activate : null) != null;
				if (flag2)
				{
					this.Desktop.Provider.PlaySound(this.Style.Sounds.Activate);
				}
				base.Layout(null, true);
				this.Open();
			}
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0008CE38 File Offset: 0x0008B038
		internal void SelectEntry(DropdownEntry entry)
		{
			bool flag = entry == null;
			if (!flag)
			{
				bool flag2 = this.MaxSelection != 1 && this.SelectedValues.Contains(entry.Value);
				if (flag2)
				{
					entry.Selected = false;
					this.SelectedValues.Remove(entry.Value);
				}
				else
				{
					bool flag3 = this.MaxSelection < 1 || Enumerable.Count<string>(this.SelectedValues) + 1 < this.MaxSelection;
					if (flag3)
					{
						this.SelectedValues.Add(entry.Value);
						entry.Selected = true;
					}
					else
					{
						bool flag4 = this.MaxSelection == 1;
						if (flag4)
						{
							this._dropdownLayer.DeselectEntries();
							this.SelectedValues.Clear();
							this.SelectedValues.Add(entry.Value);
							entry.Selected = true;
							this.CloseDropdown(false);
						}
					}
				}
				base.Layout(null, true);
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
			}
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x0008CF44 File Offset: 0x0008B144
		internal void CloseDropdown(bool playSound = true)
		{
			bool flag;
			if (playSound)
			{
				DropdownBoxSounds sounds = this.Style.Sounds;
				flag = (((sounds != null) ? sounds.Close : null) != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.Desktop.Provider.PlaySound(this.Style.Sounds.Close);
			}
			this.Desktop.SetTransientLayer(null);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0008CFC0 File Offset: 0x0008B1C0
		protected override void LayoutSelf()
		{
			bool isMounted = this._dropdownLayer.IsMounted;
			if (isMounted)
			{
				this._dropdownLayer.Layout(null, true);
			}
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x0008CFF4 File Offset: 0x0008B1F4
		public T GetEnumValue<T>() where T : struct, IConvertible
		{
			bool flag = !typeof(T).IsEnum;
			if (flag)
			{
				throw new ArgumentException("T must be an enum");
			}
			return (T)((object)Enum.Parse(typeof(T), Enumerable.FirstOrDefault<string>(this.SelectedValues)));
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x0008D046 File Offset: 0x0008B246
		public void Open()
		{
			this.Desktop.SetTransientLayer(this._dropdownLayer);
		}

		// Token: 0x04001B62 RID: 7010
		[UIMarkupProperty]
		public bool Disabled;

		// Token: 0x04001B63 RID: 7011
		[UIMarkupProperty]
		public DropdownBoxStyle Style = new DropdownBoxStyle();

		// Token: 0x04001B64 RID: 7012
		private List<DropdownBox.DropdownEntryInfo> _entries;

		// Token: 0x04001B65 RID: 7013
		private List<string> _selectedValues;

		// Token: 0x04001B66 RID: 7014
		[UIMarkupProperty]
		public string PanelTitleText;

		// Token: 0x04001B67 RID: 7015
		[UIMarkupProperty]
		public int MaxSelection = 1;

		// Token: 0x04001B68 RID: 7016
		[UIMarkupProperty]
		public bool ShowSearchInput;

		// Token: 0x04001B69 RID: 7017
		[UIMarkupProperty]
		public bool ShowLabel = true;

		// Token: 0x04001B6A RID: 7018
		[UIMarkupProperty]
		public string NoItemsText;

		// Token: 0x04001B6B RID: 7019
		public bool DisplayNonExistingValue;

		// Token: 0x04001B6C RID: 7020
		private readonly Group _icon;

		// Token: 0x04001B6D RID: 7021
		private readonly Label _label;

		// Token: 0x04001B6E RID: 7022
		private readonly Element _arrow;

		// Token: 0x04001B6F RID: 7023
		private readonly DropdownLayer _dropdownLayer;

		// Token: 0x02000D27 RID: 3367
		[UIMarkupData]
		public class DropdownEntryInfo
		{
			// Token: 0x060064E3 RID: 25827 RVA: 0x002102AB File Offset: 0x0020E4AB
			public DropdownEntryInfo()
			{
			}

			// Token: 0x060064E4 RID: 25828 RVA: 0x002102B5 File Offset: 0x0020E4B5
			public DropdownEntryInfo(string text, string value, bool selected = false)
			{
				this.Label = text;
				this.Value = value;
				this.Selected = selected;
			}

			// Token: 0x040040EE RID: 16622
			public string Label;

			// Token: 0x040040EF RID: 16623
			public string Value;

			// Token: 0x040040F0 RID: 16624
			public bool Selected;
		}
	}
}
