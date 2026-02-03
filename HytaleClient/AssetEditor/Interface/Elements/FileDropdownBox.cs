using System;
using System.Collections.Generic;
using HytaleClient.Data;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BAB RID: 2987
	internal class FileDropdownBox : Element
	{
		// Token: 0x170013A8 RID: 5032
		// (set) Token: 0x06005CA7 RID: 23719 RVA: 0x001D441C File Offset: 0x001D261C
		public Action<string, Action<FormattedMessage>> CreatingDirectory
		{
			set
			{
				this._fileDropdownLayer.FileSelector.CreatingDirectory = value;
			}
		}

		// Token: 0x170013A9 RID: 5033
		// (set) Token: 0x06005CA8 RID: 23720 RVA: 0x001D442F File Offset: 0x001D262F
		public Action SelectingInList
		{
			set
			{
				this._fileDropdownLayer.FileSelector.Selecting = value;
			}
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x06005CA9 RID: 23721 RVA: 0x001D4442 File Offset: 0x001D2642
		public bool IsOpen
		{
			get
			{
				return this._fileDropdownLayer.IsMounted;
			}
		}

		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x06005CAA RID: 23722 RVA: 0x001D444F File Offset: 0x001D264F
		// (set) Token: 0x06005CAB RID: 23723 RVA: 0x001D4461 File Offset: 0x001D2661
		public bool AllowMultipleFileSelection
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.AllowMultipleFileSelection;
			}
			set
			{
				this._fileDropdownLayer.FileSelector.AllowMultipleFileSelection = value;
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x06005CAC RID: 23724 RVA: 0x001D4474 File Offset: 0x001D2674
		// (set) Token: 0x06005CAD RID: 23725 RVA: 0x001D447C File Offset: 0x001D267C
		public HashSet<string> SelectedFiles
		{
			get
			{
				return this._selectedFiles;
			}
			set
			{
				this._selectedFiles = value;
				this._fileDropdownLayer.FileSelector.SelectedFiles = value;
				this._label.Text = ((value == null || value.Count == 0) ? "" : string.Join(", ", value));
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this._label.Layout(null, true);
				}
			}
		}

		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06005CAE RID: 23726 RVA: 0x001D44EC File Offset: 0x001D26EC
		public HashSet<string> SelectedFilesInList
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.SelectedFiles;
			}
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x06005CAF RID: 23727 RVA: 0x001D44FE File Offset: 0x001D26FE
		public string CurrentPath
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.CurrentPath;
			}
		}

		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x06005CB0 RID: 23728 RVA: 0x001D4510 File Offset: 0x001D2710
		// (set) Token: 0x06005CB1 RID: 23729 RVA: 0x001D4522 File Offset: 0x001D2722
		public string[] AllowedDirectories
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.AllowedDirectories;
			}
			set
			{
				this._fileDropdownLayer.FileSelector.AllowedDirectories = value;
			}
		}

		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x06005CB2 RID: 23730 RVA: 0x001D4535 File Offset: 0x001D2735
		public string SearchQuery
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.SearchQuery;
			}
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x06005CB3 RID: 23731 RVA: 0x001D4547 File Offset: 0x001D2747
		// (set) Token: 0x06005CB4 RID: 23732 RVA: 0x001D4559 File Offset: 0x001D2759
		public bool AllowDirectorySelection
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.AllowDirectorySelection;
			}
			set
			{
				this._fileDropdownLayer.FileSelector.AllowDirectorySelection = value;
			}
		}

		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x06005CB5 RID: 23733 RVA: 0x001D456C File Offset: 0x001D276C
		// (set) Token: 0x06005CB6 RID: 23734 RVA: 0x001D457E File Offset: 0x001D277E
		public bool IsSearchEnabled
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.IsSearchEnabled;
			}
			set
			{
				this._fileDropdownLayer.FileSelector.IsSearchEnabled = value;
			}
		}

		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x06005CB7 RID: 23735 RVA: 0x001D4591 File Offset: 0x001D2791
		// (set) Token: 0x06005CB8 RID: 23736 RVA: 0x001D45A3 File Offset: 0x001D27A3
		public bool AllowDirectoryCreation
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.AllowDirectoryCreation;
			}
			set
			{
				this._fileDropdownLayer.FileSelector.AllowDirectoryCreation = value;
			}
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x06005CB9 RID: 23737 RVA: 0x001D45B7 File Offset: 0x001D27B7
		// (set) Token: 0x06005CBA RID: 23738 RVA: 0x001D45C9 File Offset: 0x001D27C9
		public bool SupportsUITextures
		{
			get
			{
				return this._fileDropdownLayer.FileSelector.SupportsUiTextures;
			}
			set
			{
				this._fileDropdownLayer.FileSelector.SupportsUiTextures = value;
			}
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x001D45DC File Offset: 0x001D27DC
		public FileDropdownBox(Desktop desktop, Element parent, string templatePath, Func<List<FileSelector.File>> fileGetter) : base(desktop, parent)
		{
			this._fileDropdownLayer = new FileDropdownLayer(this, templatePath, fileGetter);
			this._fileDropdownLayer.FileSelector.ActivatingSelection = delegate()
			{
				this._selectedFiles = new HashSet<string>();
				foreach (string text in this._fileDropdownLayer.FileSelector.SelectedFiles)
				{
					bool flag = this.SupportsUITextures && text.EndsWith("@2x.png");
					if (flag)
					{
						this._selectedFiles.Add(text.Substring(0, text.Length - "@2x.png".Length) + ".png");
					}
					else
					{
						this._selectedFiles.Add(text);
					}
				}
				Action valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged();
				}
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					HashSet<string> selectedFiles = this._selectedFiles;
					this._label.Text = ((selectedFiles == null || selectedFiles.Count == 0) ? "" : string.Join(", ", selectedFiles));
					this._label.Layout(null, true);
					this.CloseDropdown();
				}
			};
			this._layoutMode = LayoutMode.Left;
			this._label = new Label(this.Desktop, this)
			{
				FlexWeight = 1
			};
			this._arrow = new Element(this.Desktop, this);
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x001D4650 File Offset: 0x001D2850
		protected override void OnUnmounted()
		{
			bool isMounted = this._fileDropdownLayer.IsMounted;
			if (isMounted)
			{
				this.CloseDropdown();
			}
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x001D4674 File Offset: 0x001D2874
		public override Element HitTest(Point position)
		{
			return this._anchoredRectangle.Contains(position) ? this : null;
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x001D4688 File Offset: 0x001D2888
		protected override void ApplyStyles()
		{
			bool flag = this.IsOpen || (base.CapturedMouseButton != null && (long)base.CapturedMouseButton.Value == 1L);
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
			base.ApplyStyles();
			this._arrow.Anchor.Width = new int?(this.Style.ArrowWidth);
			this._arrow.Anchor.Height = new int?(this.Style.ArrowHeight);
			this._arrow.Anchor.Right = new int?(this.Style.HorizontalPadding);
			this._label.Style = (this.Style.LabelStyle ?? new LabelStyle());
			this._label.Anchor.Left = (this._label.Anchor.Right = new int?(this.Style.HorizontalPadding));
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x001D4890 File Offset: 0x001D2A90
		protected override void AfterChildrenLayout()
		{
			FontFamily fontFamily = this.Desktop.Provider.GetFontFamily(this._label.Style.FontName.Value);
			Font font = this._label.Style.RenderBold ? fontFamily.BoldFont : fontFamily.RegularFont;
			string text = "";
			float num = (float)this._label.RectangleAfterPadding.Width / this.Desktop.Scale - font.GetCharacterAdvance(8230) * this._label.Style.FontSize / (float)font.BaseSize;
			string text2 = (this.SelectedFiles == null || this.SelectedFiles.Count == 0) ? "" : string.Join(", ", this.SelectedFiles);
			for (int i = text2.Length - 1; i >= 0; i--)
			{
				float num2 = font.GetCharacterAdvance((ushort)text2[i]) * this._label.Style.FontSize / (float)font.BaseSize;
				num -= num2;
				bool flag = num <= 0f;
				if (flag)
				{
					text = "…" + text;
					break;
				}
				text = text2[i].ToString() + text;
				num -= this._label.Style.LetterSpacing;
			}
			this._label.Text = text;
			this._label.Layout(null, true);
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x001D4A24 File Offset: 0x001D2C24
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.Layout(null, true);
			bool flag = !activate || (long)evt.Button != 1L;
			if (!flag)
			{
				this.Open();
			}
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x001D4A64 File Offset: 0x001D2C64
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			base.Layout(null, true);
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x001D4A84 File Offset: 0x001D2C84
		protected override void OnMouseEnter()
		{
			base.Layout(null, true);
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x001D4AA4 File Offset: 0x001D2CA4
		protected override void OnMouseLeave()
		{
			base.Layout(null, true);
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x001D4AC4 File Offset: 0x001D2CC4
		internal void CloseDropdown()
		{
			this.Desktop.SetTransientLayer(null);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				Action dropdownToggled = this.DropdownToggled;
				if (dropdownToggled != null)
				{
					dropdownToggled();
				}
				base.Layout(null, true);
			}
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x001D4B10 File Offset: 0x001D2D10
		public void Open()
		{
			this.Desktop.SetTransientLayer(this._fileDropdownLayer);
			this._fileDropdownLayer.FileSelector.SelectedFiles = this._selectedFiles;
			bool isSearchEnabled = this._fileDropdownLayer.FileSelector.IsSearchEnabled;
			if (isSearchEnabled)
			{
				this._fileDropdownLayer.FileSelector.FocusSearch();
			}
			Action dropdownToggled = this.DropdownToggled;
			if (dropdownToggled != null)
			{
				dropdownToggled();
			}
			this._fileDropdownLayer.FileSelector.List.ScrollFirstActiveEntryIntoView();
			base.Layout(null, true);
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x001D4BA8 File Offset: 0x001D2DA8
		public void Setup(string currentDirectory, List<FileSelector.File> files)
		{
			bool flag = !currentDirectory.StartsWith("/");
			if (flag)
			{
				currentDirectory = "/" + currentDirectory;
			}
			FileSelector fileSelector = this._fileDropdownLayer.FileSelector;
			fileSelector.SetCurrentPath(currentDirectory, true);
			fileSelector.Files = files;
			fileSelector.Layout(null, true);
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x001D4C03 File Offset: 0x001D2E03
		public void SetPreviewImage(Image image)
		{
			this._fileDropdownLayer.FileSelector.SetPreviewImage(image);
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x001D4C18 File Offset: 0x001D2E18
		protected override void LayoutSelf()
		{
			base.LayoutSelf();
			bool isMounted = this._fileDropdownLayer.IsMounted;
			if (isMounted)
			{
				this._fileDropdownLayer.Layout(null, true);
			}
		}

		// Token: 0x04003A0A RID: 14858
		public Action ValueChanged;

		// Token: 0x04003A0B RID: 14859
		public Action DropdownToggled;

		// Token: 0x04003A0C RID: 14860
		public FileDropdownBoxStyle Style;

		// Token: 0x04003A0D RID: 14861
		private HashSet<string> _selectedFiles;

		// Token: 0x04003A0E RID: 14862
		private readonly FileDropdownLayer _fileDropdownLayer;

		// Token: 0x04003A0F RID: 14863
		private readonly Label _label;

		// Token: 0x04003A10 RID: 14864
		private readonly Element _arrow;
	}
}
