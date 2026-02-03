using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x0200085D RID: 2141
	public class DropdownLayer : Group
	{
		// Token: 0x06003B8C RID: 15244 RVA: 0x0008D3E0 File Offset: 0x0008B5E0
		public DropdownLayer(DropdownBox box) : base(box.Desktop, null)
		{
			this.DropdownBox = box;
			this._container = new Group(this.Desktop, this)
			{
				LayoutMode = LayoutMode.Top
			};
			this.Label = new Label(this.Desktop, this._container)
			{
				Visible = false
			};
			this._noItemsLabel = new Label(this.Desktop, this._container);
			this.EntriesContainer = new Group(this.Desktop, this._container)
			{
				LayoutMode = LayoutMode.TopScrolling,
				FlexWeight = 1
			};
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0008D488 File Offset: 0x0008B688
		protected override void OnMounted()
		{
			bool flag = this._searchInput != null;
			if (flag)
			{
				this.Desktop.FocusElement(this._searchInput, true);
			}
			Action dropdownToggled = this.DropdownToggled;
			if (dropdownToggled != null)
			{
				dropdownToggled();
			}
		}

		// Token: 0x06003B8E RID: 15246 RVA: 0x0008D4CC File Offset: 0x0008B6CC
		protected override void OnUnmounted()
		{
			bool flag = this._focusedEntry != null;
			if (flag)
			{
				this._focusedEntry = null;
				this.UpdateFocusedDropdownEntry();
			}
			bool flag2 = this._searchInput != null;
			if (flag2)
			{
				this._searchInput.Value = "";
				this.OnEntriesChanged();
			}
			Action dropdownToggled = this.DropdownToggled;
			if (dropdownToggled != null)
			{
				dropdownToggled();
			}
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x0008D530 File Offset: 0x0008B730
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x0008D540 File Offset: 0x0008B740
		protected override void ApplyStyles()
		{
			DropdownLayer.<>c__DisplayClass13_0 CS$<>8__locals1 = new DropdownLayer.<>c__DisplayClass13_0();
			CS$<>8__locals1.<>4__this = this;
			base.ApplyStyles();
			bool showSearchInput = this.DropdownBox.ShowSearchInput;
			if (showSearchInput)
			{
				DropdownBoxStyle.DropdownBoxSearchInputStyle searchInputStyle = this.DropdownBox.Style.SearchInputStyle;
				bool flag = searchInputStyle == null;
				if (flag)
				{
					throw new Exception(string.Format("Search input enabled in dropdown box but SearchInputStyle not provided: {0}", this.DropdownBox));
				}
				bool flag2 = this._searchInput == null;
				if (flag2)
				{
					this._searchInput = new TextField(this.Desktop, null)
					{
						ValueChanged = new Action(this.OnEntriesChanged),
						KeyDown = delegate(SDL.SDL_Keycode keycode)
						{
							CS$<>8__locals1.<>4__this.OnKeyDown(keycode, 0);
						}
					};
					this._container.Add(this._searchInput, 0);
					bool isMounted = base.IsMounted;
					if (isMounted)
					{
						this.Desktop.FocusElement(this._searchInput, true);
					}
				}
				this._searchInput.Style = searchInputStyle.Style;
				this._searchInput.PlaceholderStyle = searchInputStyle.PlaceholderStyle;
				this._searchInput.PlaceholderText = searchInputStyle.PlaceholderText;
				this._searchInput.Anchor = searchInputStyle.Anchor;
				this._searchInput.Padding = searchInputStyle.Padding;
				this._searchInput.Decoration = new InputFieldDecorationStyle
				{
					Default = new InputFieldDecorationStyleState
					{
						Icon = searchInputStyle.Icon,
						Background = searchInputStyle.Background,
						ClearButtonStyle = searchInputStyle.ClearButtonStyle
					}
				};
			}
			else
			{
				bool flag3 = this._searchInput != null;
				if (flag3)
				{
					this._container.Remove(this._searchInput);
					this._searchInput = null;
				}
			}
			this._container.Background = this.DropdownBox.Style.PanelBackground;
			this._container.Padding = new Padding(this.DropdownBox.Style.PanelPadding);
			bool flag4 = this.DropdownBox.PanelTitleText != null;
			if (flag4)
			{
				this.Label.Anchor = new Anchor
				{
					Left = new int?(this.DropdownBox.Style.HorizontalEntryPadding),
					Right = new int?(this.DropdownBox.Style.HorizontalEntryPadding),
					Height = new int?(this.DropdownBox.Style.EntryHeight)
				};
				this.Label.Style = this.DropdownBox.Style.PanelTitleLabelStyle;
				this.Label.Text = this.DropdownBox.PanelTitleText;
				this.Label.Visible = true;
			}
			this.EntriesContainer.ScrollbarStyle = (this.DropdownBox.Style.PanelScrollbarStyle ?? ScrollbarStyle.MakeDefault());
			bool flag5 = this._entryComponents.Count == 0 && this.DropdownBox.NoItemsText != null;
			if (flag5)
			{
				this._noItemsLabel.Visible = true;
				this._noItemsLabel.Anchor = new Anchor
				{
					Left = new int?(this.DropdownBox.Style.HorizontalEntryPadding),
					Right = new int?(this.DropdownBox.Style.HorizontalEntryPadding),
					Height = new int?(this.DropdownBox.Style.EntryHeight)
				};
				this._noItemsLabel.Style = this.DropdownBox.Style.NoItemsLabelStyle;
				this._noItemsLabel.Text = this.DropdownBox.NoItemsText;
			}
			else
			{
				this._noItemsLabel.Visible = false;
				foreach (DropdownEntry dropdownEntry in this._entryComponents)
				{
					dropdownEntry.ApplyStylesFromDropdownBox();
				}
			}
			CS$<>8__locals1.listHeight = this.DropdownBox.Style.EntryHeight * Math.Min(this.DropdownBox.Style.EntriesInViewport, this._entryComponents.Count) + this.DropdownBox.Style.PanelPadding * 2 + 2;
			bool flag6 = this._searchInput != null;
			if (flag6)
			{
				CS$<>8__locals1.listHeight += this._searchInput.Anchor.Height.GetValueOrDefault();
			}
			int num = this.DropdownBox.Style.PanelPadding - 2;
			CS$<>8__locals1.dropdownLeft = this.Desktop.UnscaleRound((float)this.DropdownBox.AnchoredRectangle.X) - num;
			CS$<>8__locals1.dropdownTop = this.Desktop.UnscaleRound((float)this.DropdownBox.AnchoredRectangle.Top) - num;
			CS$<>8__locals1.dropdownWidth = this.Desktop.UnscaleRound((float)this.DropdownBox.AnchoredRectangle.Width);
			CS$<>8__locals1.dropdownHeight = this.Desktop.UnscaleRound((float)this.DropdownBox.AnchoredRectangle.Height);
			int valueOrDefault = this.DropdownBox.Style.PanelWidth.GetValueOrDefault(CS$<>8__locals1.dropdownWidth);
			bool visible = this.Label.Visible;
			if (visible)
			{
				CS$<>8__locals1.listHeight += this.DropdownBox.Style.EntryHeight;
			}
			bool visible2 = this._noItemsLabel.Visible;
			if (visible2)
			{
				CS$<>8__locals1.listHeight += this.DropdownBox.Style.EntryHeight;
			}
			this._container.Anchor = new Anchor
			{
				Width = new int?(valueOrDefault),
				Height = new int?(CS$<>8__locals1.listHeight)
			};
			CS$<>8__locals1.windowHeight = this.Desktop.UnscaleRound((float)this.Desktop.ViewportRectangle.Height);
			CS$<>8__locals1.windowWidth = this.Desktop.UnscaleRound((float)this.Desktop.ViewportRectangle.Width);
			CS$<>8__locals1.<ApplyStyles>g__ApplyAlignment|0(this.DropdownBox.Style.PanelAlign, true);
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x0008DB68 File Offset: 0x0008BD68
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			bool flag = this._searchInput != null && this._searchInput.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (!flag)
			{
				bool flag2 = (long)evt.Button == 1L;
				if (flag2)
				{
					this.DropdownBox.CloseDropdown(true);
				}
			}
		}

		// Token: 0x06003B92 RID: 15250 RVA: 0x0008DBC4 File Offset: 0x0008BDC4
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			base.OnKeyDown(keycode, repeat);
			bool flag = this._searchInput != null && this.Desktop.IsShortcutKeyDown && keycode == SDL.SDL_Keycode.SDLK_f;
			if (flag)
			{
				this.Desktop.FocusElement(this._searchInput, true);
				this._focusedEntry = null;
			}
			else
			{
				bool flag2 = keycode == SDL.SDL_Keycode.SDLK_UP || keycode == SDL.SDL_Keycode.SDLK_DOWN;
				if (flag2)
				{
					bool flag3 = this._entryComponents.Count == 0;
					if (flag3)
					{
						return;
					}
					this.Desktop.FocusElement(null, true);
					bool flag4 = this._focusedEntry != null;
					if (flag4)
					{
						int num = this._entryComponents.FindIndex((DropdownEntry e) => e.Value == this._focusedEntry.Value) + ((keycode == SDL.SDL_Keycode.SDLK_UP) ? -1 : 1);
						bool flag5 = num < 0;
						if (flag5)
						{
							bool flag6 = this._searchInput != null;
							if (flag6)
							{
								this.Desktop.FocusElement(this._searchInput, true);
								this._focusedEntry = null;
							}
							else
							{
								this._focusedEntry = this._entryComponents[this._entryComponents.Count - 1];
							}
						}
						else
						{
							this._focusedEntry = this._entryComponents[(num >= this._entryComponents.Count) ? 0 : num];
						}
					}
					else
					{
						this._focusedEntry = ((keycode == SDL.SDL_Keycode.SDLK_UP) ? Enumerable.Last<DropdownEntry>(this._entryComponents) : Enumerable.First<DropdownEntry>(this._entryComponents));
					}
				}
			}
			this.UpdateFocusedDropdownEntry();
		}

		// Token: 0x06003B93 RID: 15251 RVA: 0x0008DD4C File Offset: 0x0008BF4C
		private void UpdateFocusedDropdownEntry()
		{
			foreach (DropdownEntry dropdownEntry in this._entryComponents)
			{
				dropdownEntry.OutlineSize = (float)((this._focusedEntry != null && dropdownEntry == this._focusedEntry) ? this.DropdownBox.Style.FocusOutlineSize : 0);
			}
		}

		// Token: 0x06003B94 RID: 15252 RVA: 0x0008DDC8 File Offset: 0x0008BFC8
		protected internal override void Dismiss()
		{
			this.DropdownBox.CloseDropdown(true);
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x0008DDD7 File Offset: 0x0008BFD7
		internal void OnActivateEntry(DropdownEntry entry)
		{
			this.DropdownBox.SelectEntry(entry);
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x0008DDE8 File Offset: 0x0008BFE8
		public void OnSelectedValuesChanged()
		{
			foreach (DropdownEntry dropdownEntry in this._entryComponents)
			{
				dropdownEntry.Selected = this.DropdownBox.SelectedValues.Contains(dropdownEntry.Value);
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x0008DE70 File Offset: 0x0008C070
		public void OnEntriesChanged()
		{
			this.EntriesContainer.Clear();
			this._entryComponents.Clear();
			string text = (this._searchInput == null) ? "" : this._searchInput.Value.Trim().ToLowerInvariant();
			bool flag = this._focusedEntry != null;
			foreach (DropdownBox.DropdownEntryInfo dropdownEntryInfo in this.DropdownBox.Entries)
			{
				bool flag2 = text != "" && !dropdownEntryInfo.Label.ToLowerInvariant().Contains(text);
				if (!flag2)
				{
					DropdownEntry dropdownEntry = new DropdownEntry(this, dropdownEntryInfo.Value, dropdownEntryInfo.Label, dropdownEntryInfo.Selected);
					this._entryComponents.Add(dropdownEntry);
					bool flag3 = this._focusedEntry != null && dropdownEntryInfo.Value == this._focusedEntry.Value;
					if (flag3)
					{
						dropdownEntry.OutlineSize = (float)((this._focusedEntry != null && dropdownEntryInfo.Value == this._focusedEntry.Value) ? this.DropdownBox.Style.FocusOutlineSize : 0);
						flag = false;
					}
					bool flag4 = this.DropdownBox.SelectedValues.Contains(dropdownEntryInfo.Value);
					if (flag4)
					{
						dropdownEntryInfo.Selected = true;
						dropdownEntry.Selected = true;
					}
				}
			}
			bool flag5 = flag;
			if (flag5)
			{
				this._focusedEntry = null;
			}
			bool isMounted = this.EntriesContainer.IsMounted;
			if (isMounted)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x0008E038 File Offset: 0x0008C238
		public void AddEntry(DropdownEntry entry)
		{
			entry.Layer = this;
			entry.Activating = delegate()
			{
				this.OnActivateEntry(entry);
			};
			this._entryComponents.Add(entry);
			this.EntriesContainer.Add(entry, -1);
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x0008E0A4 File Offset: 0x0008C2A4
		public void DeselectEntries()
		{
			foreach (DropdownEntry dropdownEntry in this._entryComponents)
			{
				dropdownEntry.Selected = false;
			}
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x0008E0FC File Offset: 0x0008C2FC
		public DropdownEntry GetEntryByValue(string value)
		{
			return this._entryComponents.Find((DropdownEntry e) => e.Value.Equals(value));
		}

		// Token: 0x06003B9B RID: 15259 RVA: 0x0008E132 File Offset: 0x0008C332
		protected internal override void Validate()
		{
			this.OnActivateEntry(this._focusedEntry);
		}

		// Token: 0x04001B76 RID: 7030
		public readonly DropdownBox DropdownBox;

		// Token: 0x04001B77 RID: 7031
		public readonly Label Label;

		// Token: 0x04001B78 RID: 7032
		public readonly Group EntriesContainer;

		// Token: 0x04001B79 RID: 7033
		private readonly Group _container;

		// Token: 0x04001B7A RID: 7034
		private readonly Label _noItemsLabel;

		// Token: 0x04001B7B RID: 7035
		private readonly List<DropdownEntry> _entryComponents = new List<DropdownEntry>();

		// Token: 0x04001B7C RID: 7036
		private TextField _searchInput;

		// Token: 0x04001B7D RID: 7037
		private DropdownEntry _focusedEntry;

		// Token: 0x04001B7E RID: 7038
		public Action DropdownToggled;
	}
}
