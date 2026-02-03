using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BA5 RID: 2981
	internal class AssetSelector : Element
	{
		// Token: 0x06005C51 RID: 23633 RVA: 0x001D0BD0 File Offset: 0x001CEDD0
		public AssetSelector(Desktop desktop, AssetEditorOverlay assetEditorOverlay, AssetSelectorDropdown dropdown) : base(desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
			this._dropdown = dropdown;
			this._container = new Group(this.Desktop, this)
			{
				Anchor = new Anchor
				{
					Height = new int?(400)
				},
				Padding = new Padding(0),
				LayoutMode = LayoutMode.Top,
				Background = new PatchStyle(UInt32Color.FromRGBA(20, 20, 20, 230))
			};
			Group parent = new Group(this.Desktop, this._container);
			this._searchInput = new TextField(this.Desktop, parent)
			{
				Background = new PatchStyle(UInt32Color.Black),
				PlaceholderText = this.Desktop.Provider.GetText("ui.assetEditor.assetSelector.findAsset", null, true),
				Anchor = new Anchor
				{
					Height = new int?(30)
				},
				Decoration = new InputFieldDecorationStyle
				{
					Default = new InputFieldDecorationStyleState
					{
						Icon = new InputFieldIcon
						{
							Texture = new PatchStyle("AssetEditor/SearchIcon.png")
							{
								Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 63)
							},
							Width = 16,
							Height = 16,
							Offset = 5
						}
					}
				},
				Padding = new Padding
				{
					Left = new int?(27)
				},
				PlaceholderStyle = new InputFieldStyle
				{
					TextColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 102),
					RenderItalics = true,
					FontSize = 14f
				},
				Style = new InputFieldStyle
				{
					FontSize = 14f
				},
				ValueChanged = delegate()
				{
					AssetTree dropdownAssetTree = this._assetEditorOverlay.DropdownAssetTree;
					dropdownAssetTree.SearchQuery = this._searchInput.Value;
					dropdownAssetTree.BuildTree();
					dropdownAssetTree.Layout(null, true);
				},
				KeyDown = delegate(SDL.SDL_Keycode key)
				{
					bool flag = key != SDL.SDL_Keycode.SDLK_DOWN && key != SDL.SDL_Keycode.SDLK_UP;
					if (!flag)
					{
						AssetTree dropdownAssetTree = this._assetEditorOverlay.DropdownAssetTree;
						this.Desktop.FocusElement(dropdownAssetTree, true);
						dropdownAssetTree.OnKeyDown(key, 0);
					}
				}
			};
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x001D0DB9 File Offset: 0x001CEFB9
		private void SetupSearchFilter(string filter)
		{
			this._searchInput.Value = filter + ": ";
			this._searchInput.ValueChanged();
			this.Desktop.FocusElement(this._searchInput, true);
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x001D0DF8 File Offset: 0x001CEFF8
		protected override void OnMounted()
		{
			this._searchInput.Value = "";
			AssetTree dropdownAssetTree = this._assetEditorOverlay.DropdownAssetTree;
			dropdownAssetTree.FocusSearch = new Action(this.FocusSearch);
			dropdownAssetTree.PopupMenuEnabled = false;
			dropdownAssetTree.ShowVirtualAssets = true;
			AssetTree assetTree = dropdownAssetTree;
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add(this._dropdown.AssetType);
			assetTree.AssetTypesToDisplay = hashSet;
			dropdownAssetTree.SelectingDirectoryFilter = new Action<string>(this.SetupSearchFilter);
			dropdownAssetTree.FileEntryActivating = delegate(AssetTree.AssetTreeEntry entry)
			{
				bool flag2 = entry.AssetType != this._dropdown.AssetType;
				if (!flag2)
				{
					this._dropdown.CloseDropdown(entry.Name);
				}
			};
			dropdownAssetTree.SearchQuery = "";
			AssetTypeConfig assetTypeConfig;
			bool flag = !this._assetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(this._dropdown.AssetType, out assetTypeConfig);
			if (!flag)
			{
				dropdownAssetTree.DirectoriesToDisplay = new List<string>
				{
					assetTypeConfig.Path
				};
				AssetTree assetTree2 = dropdownAssetTree;
				HashSet<string> hashSet2 = new HashSet<string>();
				hashSet2.Add(assetTypeConfig.Id);
				assetTree2.AssetTypesToDisplay = hashSet2;
				this.UpdateSelectedItem();
				dropdownAssetTree.SetUncollapsedState(assetTypeConfig.Path, true, false);
				List<AssetFile> assets = this._assetEditorOverlay.Assets.GetAssets(assetTypeConfig.AssetTree);
				string[] array = assetTypeConfig.Path.Split(new char[]
				{
					'/'
				});
				string rootPath = string.Join("/", array, 0, array.Length - 1);
				dropdownAssetTree.UpdateFiles(assets, rootPath);
				this.Desktop.FocusElement(this._searchInput, true);
				this._container.Add(dropdownAssetTree, -1);
			}
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x001D0F71 File Offset: 0x001CF171
		protected override void OnUnmounted()
		{
			this._container.Remove(this._assetEditorOverlay.DropdownAssetTree);
		}

		// Token: 0x06005C55 RID: 23637 RVA: 0x001D0F8C File Offset: 0x001CF18C
		public void UpdateSelectedItem()
		{
			bool flag = this._dropdown.Value != null;
			if (flag)
			{
				string filePath;
				bool flag2 = this._assetEditorOverlay.Assets.TryGetPathForAssetId(this._dropdown.AssetType, this._dropdown.Value, out filePath, false);
				if (flag2)
				{
					this._assetEditorOverlay.DropdownAssetTree.SelectEntry(new AssetReference(this._dropdown.AssetType, filePath), false);
				}
			}
			else
			{
				this._assetEditorOverlay.DropdownAssetTree.DeselectEntry();
			}
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x001D1018 File Offset: 0x001CF218
		public override Element HitTest(Point position)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !this._anchoredRectangle.Contains(position);
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

		// Token: 0x06005C57 RID: 23639 RVA: 0x001D105C File Offset: 0x001CF25C
		protected override void ApplyStyles()
		{
			int num = this.Desktop.UnscaleRound((float)this._dropdown.AnchoredRectangle.X);
			int num2 = this.Desktop.UnscaleRound((float)this._dropdown.AnchoredRectangle.Top);
			int num3 = this.Desktop.UnscaleRound((float)this._dropdown.AnchoredRectangle.Width);
			int num4 = this.Desktop.UnscaleRound((float)this._dropdown.AnchoredRectangle.Height);
			int num5 = this.Desktop.UnscaleRound((float)this.Desktop.ViewportRectangle.Height);
			int num6 = this.Desktop.UnscaleRound((float)this.Desktop.ViewportRectangle.Width);
			this._container.Anchor.Width = new int?(num3);
			this._container.Anchor.Top = new int?(num2 + num4);
			int? num7 = this._container.Anchor.Top + this._container.Anchor.Height;
			int num8 = num5;
			bool flag = num7.GetValueOrDefault() > num8 & num7 != null;
			if (flag)
			{
				this._container.Anchor.Height = new int?(Math.Min(num2, 400));
				this._container.Anchor.Top = num2 - this._container.Anchor.Height;
			}
			else
			{
				this._container.Anchor.Height = new int?(Math.Min(num5 - this._container.Anchor.Top.Value, 400));
			}
			this._container.Anchor.Left = new int?(num);
			num7 = this._container.Anchor.Left + this._container.Anchor.Width;
			num8 = num6;
			bool flag2 = num7.GetValueOrDefault() > num8 & num7 != null;
			if (flag2)
			{
				this._container.Anchor.Left = num + num3 - this._container.Anchor.Width;
			}
		}

		// Token: 0x06005C58 RID: 23640 RVA: 0x001D133C File Offset: 0x001CF53C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				bool flag2 = (long)evt.Button == 1L;
				if (flag2)
				{
					this._dropdown.CloseDropdown(null);
				}
				else
				{
					this._dropdown.CloseDropdown(null);
				}
			}
		}

		// Token: 0x06005C59 RID: 23641 RVA: 0x001D13A4 File Offset: 0x001CF5A4
		protected internal override void OnKeyDown(SDL.SDL_Keycode keyCode, int repeat)
		{
			bool flag = this.Desktop.IsShortcutKeyDown && keyCode == SDL.SDL_Keycode.SDLK_f;
			if (flag)
			{
				this.FocusSearch();
			}
		}

		// Token: 0x06005C5A RID: 23642 RVA: 0x001D13D4 File Offset: 0x001CF5D4
		private void FocusSearch()
		{
			this.Desktop.FocusElement(this._searchInput, true);
			this._searchInput.SelectAll();
		}

		// Token: 0x06005C5B RID: 23643 RVA: 0x001D13F6 File Offset: 0x001CF5F6
		protected internal override void Dismiss()
		{
			this._dropdown.CloseDropdown(null);
		}

		// Token: 0x040039D5 RID: 14805
		private const int Height = 400;

		// Token: 0x040039D6 RID: 14806
		private readonly TextField _searchInput;

		// Token: 0x040039D7 RID: 14807
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x040039D8 RID: 14808
		private readonly AssetSelectorDropdown _dropdown;

		// Token: 0x040039D9 RID: 14809
		private readonly Group _container;
	}
}
