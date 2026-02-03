using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Data;
using HytaleClient.Graphics;
using HytaleClient.Interface;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BAD RID: 2989
	internal class FileSelector : Element
	{
		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x06005CD1 RID: 23761 RVA: 0x001D515B File Offset: 0x001D335B
		// (set) Token: 0x06005CD2 RID: 23762 RVA: 0x001D5164 File Offset: 0x001D3364
		public List<FileSelector.File> Files
		{
			get
			{
				return this._files;
			}
			set
			{
				bool isSingleDirectoryForNavigationSelected = this._isSingleDirectoryForNavigationSelected;
				if (isSingleDirectoryForNavigationSelected)
				{
					this._isSingleDirectoryForNavigationSelected = false;
				}
				this._files = value;
			}
		}

		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x06005CD3 RID: 23763 RVA: 0x001D518C File Offset: 0x001D338C
		// (set) Token: 0x06005CD4 RID: 23764 RVA: 0x001D5194 File Offset: 0x001D3394
		public HashSet<string> SelectedFiles
		{
			get
			{
				return this._selectedFiles;
			}
			set
			{
				this._selectedFiles = ((value != null) ? new HashSet<string>(value) : new HashSet<string>());
			}
		}

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x06005CD5 RID: 23765 RVA: 0x001D51AC File Offset: 0x001D33AC
		// (set) Token: 0x06005CD6 RID: 23766 RVA: 0x001D51B4 File Offset: 0x001D33B4
		public bool AllowDirectoryCreation
		{
			get
			{
				return this._allowDirectoryCreation;
			}
			set
			{
				this._allowDirectoryCreation = value;
				this._createDirectoryButton.Visible = value;
				this._createDirectoryField.Visible = value;
			}
		}

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06005CD7 RID: 23767 RVA: 0x001D51D8 File Offset: 0x001D33D8
		public string SearchQuery
		{
			get
			{
				return this._searchInput.Value;
			}
		}

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06005CD8 RID: 23768 RVA: 0x001D51E5 File Offset: 0x001D33E5
		// (set) Token: 0x06005CD9 RID: 23769 RVA: 0x001D51ED File Offset: 0x001D33ED
		public string CurrentPath { get; private set; } = "/";

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06005CDA RID: 23770 RVA: 0x001D51F6 File Offset: 0x001D33F6
		public FileSelectorList List
		{
			get
			{
				return this._fileList;
			}
		}

		// Token: 0x06005CDB RID: 23771 RVA: 0x001D5200 File Offset: 0x001D3400
		public FileSelector(Desktop desktop, Element parent, string template, Func<List<FileSelector.File>> fileGetter) : base(desktop, parent)
		{
			this._fileGetter = fileGetter;
			this.Build(template);
		}

		// Token: 0x06005CDC RID: 23772 RVA: 0x001D5268 File Offset: 0x001D3468
		private void Build(string templatePath)
		{
			Document document;
			this.Desktop.Provider.TryGetDocument(templatePath, out document);
			this._breadcrumbButtonStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Desktop.Provider, "BreadcrumbButtonStyle");
			this._breadcrumbArrowLabelStyle = document.ResolveNamedValue<LabelStyle>(this.Desktop.Provider, "BreadcrumbArrowLabelStyle");
			this._breadcrumbCurrentLabelStyle = document.ResolveNamedValue<LabelStyle>(this.Desktop.Provider, "BreadcrumbCurrentLabelStyle");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			uifragment.Get<TextButton>("CancelButton").Activating = delegate()
			{
				Action cancelling = this.Cancelling;
				if (cancelling != null)
				{
					cancelling();
				}
			};
			this._selectButton = uifragment.Get<TextButton>("SelectButton");
			this._selectButton.Activating = new Action(this.OpenSelectedFile);
			this._createDirectoryButton = uifragment.Get<TextButton>("CreateDirectoryButton");
			this._createDirectoryButton.Visible = this.AllowDirectoryCreation;
			this._createDirectoryButton.Activating = new Action(this.OnCreateDirectoryActivating);
			this._createDirectoryField = uifragment.Get<TextField>("CreateDirectoryField");
			this._createDirectoryField.Visible = this.AllowDirectoryCreation;
			this._createDirectoryField.Validating = new Action(this.OnCreateDirectoryActivating);
			this._fileList = new FileSelectorList(this.Desktop, uifragment.Get<Group>("FileList"), this);
			ScrollbarStyle scrollbarStyle;
			bool flag = document.TryResolveNamedValue<ScrollbarStyle>(this.Desktop.Provider, "ScrollbarStyle", out scrollbarStyle);
			if (flag)
			{
				this._fileList.ScrollbarStyle = scrollbarStyle;
			}
			this._currentDirectoryInfo = uifragment.Get<Group>("CurrentDirectory");
			this._searchInput = uifragment.Get<TextField>("SearchInput");
			this._searchInput.KeyDown = new Action<SDL.SDL_Keycode>(this.HandleCommonKeyEvent);
			this._searchInput.ValueChanged = delegate()
			{
				this.SelectedFiles = null;
				this.Files = this._fileGetter();
				this.UpdateDirectoryLabel();
				base.Layout(null, true);
			};
			this._errorLabel = uifragment.Get<Label>("ErrorLabel");
			this._errorLabel.Visible = false;
			this._previewContainer = uifragment.Get<Group>("PreviewContainer");
			this._selectedFileLabel = uifragment.Get<Label>("SelectedFileLabel");
			this._backButton = uifragment.Get<TextButton>("BackButton");
			this._backButton.Activating = new Action(this.OnGoBack);
			this._forwardButton = uifragment.Get<TextButton>("ForwardButton");
			this._forwardButton.Activating = new Action(this.OnGoForward);
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x001D54CC File Offset: 0x001D36CC
		protected override void OnMounted()
		{
			this._searchInput.Value = "";
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x001D54E0 File Offset: 0x001D36E0
		protected override void OnUnmounted()
		{
			this._errorLabel.Visible = false;
			this.SelectedFiles = new HashSet<string>();
			this._forwardStack.Clear();
			this._searchInput.Value = "";
			this._files.Clear();
			this.ClearPreview(false);
			bool isSingleDirectoryForNavigationSelected = this._isSingleDirectoryForNavigationSelected;
			if (isSingleDirectoryForNavigationSelected)
			{
				this._isSingleDirectoryForNavigationSelected = false;
				this.SelectedFiles = null;
			}
		}

		// Token: 0x06005CDF RID: 23775 RVA: 0x001D5554 File Offset: 0x001D3754
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

		// Token: 0x06005CE0 RID: 23776 RVA: 0x001D5598 File Offset: 0x001D3798
		private void OnCreateDirectoryActivating()
		{
			this._errorLabel.Visible = false;
			bool flag = this.SearchQuery != "";
			if (!flag)
			{
				string text = this._createDirectoryField.Value.Trim();
				bool flag2 = text == "";
				if (!flag2)
				{
					foreach (FileSelector.File file in this._files)
					{
						bool flag3 = file.Name.ToLowerInvariant() == text.ToLowerInvariant();
						if (flag3)
						{
							this._errorLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.errors.createDirectoryExists", null, true);
							this._errorLabel.Visible = true;
							this._errorLabel.Parent.Layout(null, true);
							return;
						}
					}
					bool flag4 = text.Contains("/") || text.Contains("\\");
					if (flag4)
					{
						this._errorLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.errors.directoryInvalidCharacters", null, true);
						this._errorLabel.Visible = true;
						this._errorLabel.Parent.Layout(null, true);
					}
					else
					{
						this._createDirectoryField.Value = "";
						Action<string, Action<FormattedMessage>> creatingDirectory = this.CreatingDirectory;
						if (creatingDirectory != null)
						{
							creatingDirectory(text, delegate(FormattedMessage errorMessage)
							{
								bool flag5 = errorMessage == null;
								if (!flag5)
								{
									this._errorLabel.TextSpans = FormattedMessageConverter.GetLabelSpans(errorMessage, this.Desktop.Provider, new SpanStyle
									{
										Color = new UInt32Color?(this._errorLabel.Style.TextColor)
									}, false);
									this._errorLabel.Visible = true;
									this._errorLabel.Parent.Layout(null, true);
								}
							});
						}
					}
				}
			}
		}

		// Token: 0x06005CE1 RID: 23777 RVA: 0x001D5748 File Offset: 0x001D3948
		public string GetFullPathOfFile(string filename)
		{
			bool flag = this.SearchQuery == "";
			string result;
			if (flag)
			{
				result = (this.CurrentPath + "/" + filename).TrimStart(new char[]
				{
					'/'
				});
			}
			else
			{
				result = filename;
			}
			return result;
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x001D5794 File Offset: 0x001D3994
		internal void SetCurrentPath(string path, bool clearForwardStack)
		{
			this.CurrentPath = path;
			Debug.Assert(this.CurrentPath.StartsWith("/"));
			Debug.Assert(this.CurrentPath.Length == 1 || !this.CurrentPath.EndsWith("/"));
			this.UpdateDirectoryLabel();
			if (clearForwardStack)
			{
				this._forwardStack.Clear();
			}
			this._backButton.Disabled = (this.CurrentPath == "/");
			this._forwardButton.Disabled = (this._forwardStack.Count == 0);
		}

		// Token: 0x06005CE3 RID: 23779 RVA: 0x001D5838 File Offset: 0x001D3A38
		private void SelectNextEntry(bool invert)
		{
			bool flag = this.Files.Count == 0;
			if (!flag)
			{
				string text = Enumerable.FirstOrDefault<string>(this.SelectedFiles);
				int num = invert ? (this.Files.Count - 1) : 0;
				bool flag2 = text != null;
				if (flag2)
				{
					for (int i = 0; i < this.Files.Count; i++)
					{
						FileSelector.File file = this.Files[i];
						bool flag3 = this.GetFullPathOfFile(file.Name) != text;
						if (!flag3)
						{
							num = (invert ? (i - 1) : (i + 1));
							bool flag4 = num >= this.Files.Count;
							if (flag4)
							{
								num = 0;
							}
							else
							{
								bool flag5 = num < 0;
								if (flag5)
								{
									num = this.Files.Count - 1;
								}
							}
							break;
						}
					}
				}
				HashSet<string> hashSet = new HashSet<string>();
				hashSet.Add(this.GetFullPathOfFile(this.Files[num].Name));
				this.SelectedFiles = hashSet;
				this._isSingleDirectoryForNavigationSelected = this.Files[num].IsDirectory;
				this.ClearPreview(true);
				Action selecting = this.Selecting;
				if (selecting != null)
				{
					selecting();
				}
				this.UpdateSelectedFileLabel(true);
				this.UpdateSelectButtonState(true);
				this._fileList.Layout(null, true);
				this._fileList.ScrollFirstActiveEntryIntoView();
			}
		}

		// Token: 0x06005CE4 RID: 23780 RVA: 0x001D59AC File Offset: 0x001D3BAC
		private void OnGoBack()
		{
			string[] array = this.CurrentPath.Split(new char[]
			{
				'/'
			}, StringSplitOptions.RemoveEmptyEntries);
			bool flag = array.Length == 0;
			if (!flag)
			{
				this._forwardStack.Push(array[array.Length - 1]);
				this._searchInput.Value = "";
				string path = "/" + string.Join("/", Enumerable.Take<string>(array, array.Length - 1));
				this.SetCurrentPath(path, false);
				this.SelectedFiles = null;
				this.Files = this._fileGetter();
				base.Layout(null, true);
			}
		}

		// Token: 0x06005CE5 RID: 23781 RVA: 0x001D5A58 File Offset: 0x001D3C58
		private void OnGoForward()
		{
			bool flag = this._forwardStack.Count == 0;
			if (!flag)
			{
				string path = this._forwardStack.Pop();
				string path2 = AssetPathUtils.CombinePaths(this.CurrentPath, path);
				this._searchInput.Value = "";
				this.SetCurrentPath(path2, false);
				this.SelectedFiles = null;
				this.Files = this._fileGetter();
				base.Layout(null, true);
			}
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x001D5AD8 File Offset: 0x001D3CD8
		private void HandleCommonKeyEvent(SDL.SDL_Keycode keyCode)
		{
			if (keyCode != SDL.SDL_Keycode.SDLK_RETURN)
			{
				if (keyCode != SDL.SDL_Keycode.SDLK_DOWN)
				{
					if (keyCode == SDL.SDL_Keycode.SDLK_UP)
					{
						this.SelectNextEntry(true);
					}
				}
				else
				{
					this.SelectNextEntry(false);
				}
			}
			else
			{
				bool flag = this.SelectedFiles.Count == 0;
				if (!flag)
				{
					string first = Enumerable.First<string>(this.SelectedFiles);
					FileSelector.File file = Enumerable.FirstOrDefault<FileSelector.File>(this.Files, (FileSelector.File sf) => this.GetFullPathOfFile(sf.Name) == first);
					bool flag2 = this.IsFileInAllowedDirectory(file);
					if (flag2)
					{
						bool flag3 = this.SelectedFiles.Count == 1 && file != null && file.IsDirectory;
						if (flag3)
						{
							this.OpenDirectory(first);
						}
						else
						{
							this.OpenSelectedFile();
						}
					}
				}
			}
		}

		// Token: 0x06005CE7 RID: 23783 RVA: 0x001D5BB8 File Offset: 0x001D3DB8
		protected internal override void OnKeyUp(SDL.SDL_Keycode keyCode)
		{
			this.HandleCommonKeyEvent(keyCode);
			if (keyCode <= SDL.SDL_Keycode.SDLK_f)
			{
				if (keyCode != SDL.SDL_Keycode.SDLK_a)
				{
					if (keyCode == SDL.SDL_Keycode.SDLK_f)
					{
						bool flag = !this.Desktop.IsShortcutKeyDown || !this._searchInput.IsMounted;
						if (!flag)
						{
							this.Desktop.FocusElement(this._searchInput, true);
						}
					}
				}
				else
				{
					bool flag2 = !this.Desktop.IsShortcutKeyDown;
					if (!flag2)
					{
						bool flag3 = !this.IsDirectoryAllowed(this.CurrentPath);
						if (!flag3)
						{
							HashSet<string> hashSet = new HashSet<string>();
							foreach (FileSelector.File file in this.Files)
							{
								bool flag4 = !this.AllowDirectorySelection && file.IsDirectory;
								if (!flag4)
								{
									hashSet.Add(this.GetFullPathOfFile(file.Name));
								}
							}
							bool flag5 = this.SelectedFiles != null && Enumerable.SequenceEqual<string>(this.SelectedFiles, hashSet);
							if (flag5)
							{
								this.SelectedFiles = null;
							}
							else
							{
								this.SelectedFiles = hashSet;
							}
							this._fileList.Layout(null, true);
						}
					}
				}
			}
			else if (keyCode != SDL.SDL_Keycode.SDLK_RIGHT)
			{
				if (keyCode == SDL.SDL_Keycode.SDLK_LEFT)
				{
					this.OnGoBack();
				}
			}
			else
			{
				this.OnGoForward();
			}
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x001D5D50 File Offset: 0x001D3F50
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this.UpdateSelectButtonState(false);
			this._searchInput.Visible = this.IsSearchEnabled;
		}

		// Token: 0x06005CE9 RID: 23785 RVA: 0x001D5D74 File Offset: 0x001D3F74
		internal bool IsFileInAllowedDirectory(FileSelector.File file)
		{
			bool flag = this.AllowedDirectories != null;
			if (flag)
			{
				bool isDirectory = file.IsDirectory;
				if (isDirectory)
				{
					bool flag2 = this.IsDirectoryAllowed(this.CurrentPath);
					if (flag2)
					{
						return true;
					}
					string value = AssetPathUtils.CombinePaths(this.CurrentPath, file.Name) + "/";
					foreach (string text in this.AllowedDirectories)
					{
						bool flag3 = !text.StartsWith(value);
						if (!flag3)
						{
							return true;
						}
					}
					return false;
				}
				else
				{
					bool flag4 = !this.IsDirectoryAllowed(this.CurrentPath);
					if (flag4)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x001D5E34 File Offset: 0x001D4034
		public void SetPreviewImage(Image image)
		{
			Debug.Assert(base.IsMounted);
			bool flag = !base.IsMounted;
			if (!flag)
			{
				PatchStyle background = this._previewContainer.Background;
				if (background != null)
				{
					background.TextureArea.Texture.Dispose();
				}
				TextureArea textureArea = ExternalTextureLoader.FromImage(image);
				this._previewContainer.Background = new PatchStyle(textureArea);
				this._previewContainer.Anchor.Width = new int?(image.Width);
				this._previewContainer.Anchor.Height = new int?(image.Height);
				int num = this.Desktop.UnscaleRound((float)this._previewContainer.Parent.RectangleAfterPadding.Width);
				int num2 = this.Desktop.UnscaleRound((float)this._previewContainer.Parent.RectangleAfterPadding.Height);
				int? num3 = this._previewContainer.Anchor.Width;
				int num4 = num;
				bool flag2 = num3.GetValueOrDefault() > num4 & num3 != null;
				if (flag2)
				{
					Element previewContainer = this._previewContainer;
					float num5 = (float)this._previewContainer.Anchor.Height.Value;
					num3 = this._previewContainer.Anchor.Width;
					previewContainer.Anchor.Height = new int?((int)(num5 / ((num3 != null) ? new float?((float)num3.GetValueOrDefault()) : null) * (float)num).Value);
					this._previewContainer.Anchor.Width = new int?(num);
				}
				num3 = this._previewContainer.Anchor.Height;
				num4 = num2;
				bool flag3 = num3.GetValueOrDefault() > num4 & num3 != null;
				if (flag3)
				{
					Element previewContainer2 = this._previewContainer;
					float num5 = (float)this._previewContainer.Anchor.Width.Value;
					num3 = this._previewContainer.Anchor.Height;
					previewContainer2.Anchor.Width = new int?((int)(num5 / ((num3 != null) ? new float?((float)num3.GetValueOrDefault()) : null) * (float)num2).Value);
					this._previewContainer.Anchor.Width = new int?(num2);
				}
				this._previewContainer.Parent.Layout(null, true);
			}
		}

		// Token: 0x06005CEB RID: 23787 RVA: 0x001D6140 File Offset: 0x001D4340
		private void ClearPreview(bool doLayout = true)
		{
			PatchStyle background = this._previewContainer.Background;
			if (background != null)
			{
				background.TextureArea.Texture.Dispose();
			}
			this._previewContainer.Background = null;
			if (doLayout)
			{
				this._previewContainer.Parent.Layout(null, true);
			}
		}

		// Token: 0x06005CEC RID: 23788 RVA: 0x001D619C File Offset: 0x001D439C
		private void UpdateSelectedFileLabel(bool doLayout)
		{
			bool flag = this.SelectedFiles.Count == 0;
			if (flag)
			{
				this._selectedFileLabel.Text = "";
			}
			else
			{
				bool flag2 = this.SelectedFiles.Count == 1;
				if (flag2)
				{
					this._selectedFileLabel.Text = Enumerable.First<string>(this.SelectedFiles);
				}
				else
				{
					this._selectedFileLabel.Text = this.Desktop.Provider.GetText("ui.assetEditor.fileSelector.filesSelected", new Dictionary<string, string>
					{
						{
							"count",
							this.Desktop.Provider.FormatNumber(this.SelectedFiles.Count)
						}
					}, true);
				}
			}
			if (doLayout)
			{
				this._selectedFileLabel.Layout(null, true);
			}
		}

		// Token: 0x06005CED RID: 23789 RVA: 0x001D626C File Offset: 0x001D446C
		public void OnSelectFile(FileSelector.File file)
		{
			string fullPathOfFile = this.GetFullPathOfFile(file.Name);
			bool flag = this.AllowMultipleFileSelection && this.Desktop.IsShiftKeyDown;
			if (flag)
			{
				FileSelector.File[] array = this.Files.ToArray();
				string[] visibleFileNames = Enumerable.ToArray<string>(Enumerable.Select<FileSelector.File, string>(array, (FileSelector.File f) => this.GetFullPathOfFile(f.Name)));
				IEnumerable<string> enumerable = new string[]
				{
					fullPathOfFile
				};
				bool flag2 = this.SelectedFiles != null && this._fileList.AreAllSelectedFilesInList;
				if (flag2)
				{
					enumerable = Enumerable.Concat<string>(enumerable, this.SelectedFiles);
				}
				int num = int.MaxValue;
				int num2 = 0;
				IEnumerable<string> enumerable2 = enumerable;
				Func<string, int> func;
				Func<string, int> <>9__1;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = ((string f) => Array.IndexOf<string>(visibleFileNames, f)));
				}
				foreach (int num3 in Enumerable.Select<string, int>(enumerable2, func))
				{
					bool flag3 = num3 < num;
					if (flag3)
					{
						num = num3;
					}
					bool flag4 = num3 > num2;
					if (flag4)
					{
						num2 = num3;
					}
				}
				bool flag5 = num == -1 && num2 == -1;
				if (flag5)
				{
					return;
				}
				HashSet<string> hashSet = new HashSet<string>();
				for (int i = num; i <= num2; i++)
				{
					bool flag6 = !this.AllowDirectorySelection && array[i].IsDirectory;
					if (!flag6)
					{
						hashSet.Add(this.GetFullPathOfFile(array[i].Name));
					}
				}
				this.ClearPreview(true);
				this.SelectedFiles = hashSet;
				Action selecting = this.Selecting;
				if (selecting != null)
				{
					selecting();
				}
			}
			else
			{
				bool flag7 = this.AllowMultipleFileSelection && this.Desktop.IsShortcutKeyDown;
				if (flag7)
				{
					bool flag8 = file.IsDirectory && !this.AllowDirectorySelection;
					if (flag8)
					{
						return;
					}
					this.ClearPreview(true);
					bool flag9 = this.SelectedFiles != null && this._fileList.AreAllSelectedFilesInList;
					if (flag9)
					{
						bool flag10 = this.SelectedFiles.Contains(fullPathOfFile);
						if (flag10)
						{
							bool flag11 = this.SelectedFiles.Count == 1;
							if (flag11)
							{
								this.SelectedFiles = null;
							}
							else
							{
								this.SelectedFiles.Remove(fullPathOfFile);
							}
						}
						else
						{
							this.SelectedFiles.Add(fullPathOfFile);
						}
					}
					else
					{
						HashSet<string> hashSet2 = new HashSet<string>();
						hashSet2.Add(fullPathOfFile);
						this.SelectedFiles = hashSet2;
					}
					Action selecting2 = this.Selecting;
					if (selecting2 != null)
					{
						selecting2();
					}
				}
				else
				{
					bool flag12 = file.IsDirectory && !this.AllowDirectorySelection;
					if (flag12)
					{
						return;
					}
					this.ClearPreview(true);
					bool flag13 = this.SelectedFiles != null && this.SelectedFiles.Count > 0 && Enumerable.First<string>(this.SelectedFiles) == fullPathOfFile;
					if (flag13)
					{
						this.SelectedFiles = null;
					}
					else
					{
						HashSet<string> hashSet3 = new HashSet<string>();
						hashSet3.Add(fullPathOfFile);
						this.SelectedFiles = hashSet3;
					}
					Action selecting3 = this.Selecting;
					if (selecting3 != null)
					{
						selecting3();
					}
				}
			}
			this.UpdateSelectedFileLabel(true);
			this.UpdateSelectButtonState(true);
			this._fileList.Layout(null, true);
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x001D65C8 File Offset: 0x001D47C8
		public void OnOpenFile(FileSelector.File file)
		{
			bool isDirectory = file.IsDirectory;
			if (isDirectory)
			{
				this.OpenDirectory(this.GetFullPathOfFile(file.Name));
			}
			else
			{
				HashSet<string> hashSet = new HashSet<string>();
				hashSet.Add(this.GetFullPathOfFile(file.Name));
				this.SelectedFiles = hashSet;
				this.UpdateSelectButtonState(true);
				this.OpenSelectedFile();
			}
		}

		// Token: 0x06005CEF RID: 23791 RVA: 0x001D6628 File Offset: 0x001D4828
		private bool IsDirectoryAllowed(string directoryToCheck)
		{
			bool flag = !directoryToCheck.EndsWith("/");
			if (flag)
			{
				directoryToCheck += "/";
			}
			foreach (string value in this.AllowedDirectories)
			{
				bool flag2 = directoryToCheck.StartsWith(value);
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005CF0 RID: 23792 RVA: 0x001D668C File Offset: 0x001D488C
		private void OpenSelectedFile()
		{
			bool disabled = this._selectButton.Disabled;
			if (!disabled)
			{
				bool flag = !this.AllowDirectorySelection;
				if (flag)
				{
					using (HashSet<string>.Enumerator enumerator = this.SelectedFiles.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string file = enumerator.Current;
							bool isDirectory = Enumerable.First<FileSelector.File>(this.Files, (FileSelector.File f) => this.GetFullPathOfFile(f.Name) == file).IsDirectory;
							if (isDirectory)
							{
								return;
							}
						}
					}
				}
				this._forwardStack.Clear();
				bool flag2 = this.SelectedFiles.Count == 0;
				if (flag2)
				{
					this._selectedFiles.Add(this.CurrentPath.Trim(new char[]
					{
						'/'
					}));
				}
				Action activatingSelection = this.ActivatingSelection;
				if (activatingSelection != null)
				{
					activatingSelection();
				}
			}
		}

		// Token: 0x06005CF1 RID: 23793 RVA: 0x001D678C File Offset: 0x001D498C
		private void OpenDirectory(string path)
		{
			this._searchInput.Value = "";
			this.SetCurrentPath("/" + path, true);
			this.SelectedFiles = null;
			this.Files = this._fileGetter();
			base.Layout(null, true);
		}

		// Token: 0x06005CF2 RID: 23794 RVA: 0x001D67EC File Offset: 0x001D49EC
		private void UpdateDirectoryLabel()
		{
			this._currentDirectoryInfo.Clear();
			bool flag = this.SearchQuery == "";
			if (flag)
			{
				string[] array = this.CurrentPath.Trim(new char[]
				{
					'/'
				}).Split(new char[]
				{
					'/'
				});
				string text = array[array.Length - 1];
				string text2 = "";
				bool flag2 = text != "";
				if (flag2)
				{
					this.<UpdateDirectoryLabel>g__CreateButton|69_0("Root", text2);
				}
				for (int i = 0; i < array.Length - 1; i++)
				{
					text2 += array[i];
					this.<UpdateDirectoryLabel>g__CreateButton|69_0(array[i], text2);
					text2 += "/";
				}
				Label label = new Label(this.Desktop, this._currentDirectoryInfo);
				label.Text = ((text == "") ? "Root" : text);
				label.Anchor = new Anchor
				{
					Horizontal = new int?(5)
				};
				label.Style = this._breadcrumbCurrentLabelStyle;
			}
			else
			{
				Label label2 = new Label(this.Desktop, this._currentDirectoryInfo);
				label2.Text = "Search \"" + this.SearchQuery + "\"";
				label2.Style = this._breadcrumbCurrentLabelStyle;
			}
		}

		// Token: 0x06005CF3 RID: 23795 RVA: 0x001D6944 File Offset: 0x001D4B44
		private void UpdateSelectButtonState(bool doLayout = false)
		{
			this._createDirectoryButton.Disabled = (this.SearchQuery != "");
			if (doLayout)
			{
				this._createDirectoryButton.Layout(null, true);
			}
			HashSet<string> selectedFiles = this.SelectedFiles;
			bool flag = selectedFiles.Count == 0 || this._isSingleDirectoryForNavigationSelected;
			if (flag)
			{
				this._selectButton.Disabled = !this.AllowDirectorySelection;
				if (doLayout)
				{
					this._selectButton.Layout(null, true);
				}
			}
			else
			{
				bool flag2 = this.AllowedDirectories != null;
				if (flag2)
				{
					foreach (string text in selectedFiles)
					{
						bool flag3 = false;
						foreach (string text2 in this.AllowedDirectories)
						{
							bool flag4 = text.StartsWith(text2.TrimStart(new char[]
							{
								'/'
							}));
							if (flag4)
							{
								flag3 = true;
								break;
							}
						}
						bool flag5 = !flag3;
						if (flag5)
						{
							this._selectButton.Disabled = true;
							if (doLayout)
							{
								this._selectButton.Layout(null, true);
							}
							return;
						}
					}
				}
				this._selectButton.Disabled = false;
				if (doLayout)
				{
					this._selectButton.Layout(null, true);
				}
			}
		}

		// Token: 0x06005CF4 RID: 23796 RVA: 0x001D6AE4 File Offset: 0x001D4CE4
		public void FocusSearch()
		{
			this.Desktop.FocusElement(this._searchInput, true);
		}

		// Token: 0x06005CF9 RID: 23801 RVA: 0x001D6BE4 File Offset: 0x001D4DE4
		[CompilerGenerated]
		private void <UpdateDirectoryLabel>g__CreateButton|69_0(string text, string path)
		{
			TextButton textButton = new TextButton(this.Desktop, this._currentDirectoryInfo);
			textButton.Text = text;
			textButton.Anchor = new Anchor
			{
				Horizontal = new int?(2)
			};
			textButton.Padding = new Padding
			{
				Horizontal = new int?(3)
			};
			textButton.Activating = delegate()
			{
				this.OpenDirectory(path);
			};
			textButton.Style = this._breadcrumbButtonStyle;
			Label label = new Label(this.Desktop, this._currentDirectoryInfo);
			label.Text = ">";
			label.Style = this._breadcrumbArrowLabelStyle;
		}

		// Token: 0x04003A13 RID: 14867
		private List<FileSelector.File> _files = new List<FileSelector.File>();

		// Token: 0x04003A14 RID: 14868
		private HashSet<string> _selectedFiles = new HashSet<string>();

		// Token: 0x04003A15 RID: 14869
		public bool AllowMultipleFileSelection;

		// Token: 0x04003A16 RID: 14870
		public bool AllowDirectorySelection = false;

		// Token: 0x04003A17 RID: 14871
		public bool SupportsUiTextures = false;

		// Token: 0x04003A18 RID: 14872
		private bool _allowDirectoryCreation;

		// Token: 0x04003A19 RID: 14873
		public string[] AllowedDirectories;

		// Token: 0x04003A1A RID: 14874
		public Action Cancelling;

		// Token: 0x04003A1B RID: 14875
		public Action ActivatingSelection;

		// Token: 0x04003A1C RID: 14876
		public Action Selecting;

		// Token: 0x04003A1D RID: 14877
		public Action<string, Action<FormattedMessage>> CreatingDirectory;

		// Token: 0x04003A1E RID: 14878
		private readonly Func<List<FileSelector.File>> _fileGetter;

		// Token: 0x04003A20 RID: 14880
		public bool IsSearchEnabled = true;

		// Token: 0x04003A21 RID: 14881
		private TextField _searchInput;

		// Token: 0x04003A22 RID: 14882
		private Group _currentDirectoryInfo;

		// Token: 0x04003A23 RID: 14883
		private TextButton _selectButton;

		// Token: 0x04003A24 RID: 14884
		private TextButton _createDirectoryButton;

		// Token: 0x04003A25 RID: 14885
		private TextField _createDirectoryField;

		// Token: 0x04003A26 RID: 14886
		private FileSelectorList _fileList;

		// Token: 0x04003A27 RID: 14887
		private TextButton _backButton;

		// Token: 0x04003A28 RID: 14888
		private TextButton _forwardButton;

		// Token: 0x04003A29 RID: 14889
		private Group _previewContainer;

		// Token: 0x04003A2A RID: 14890
		private Label _selectedFileLabel;

		// Token: 0x04003A2B RID: 14891
		private Label _errorLabel;

		// Token: 0x04003A2C RID: 14892
		private TextButton.TextButtonStyle _breadcrumbButtonStyle;

		// Token: 0x04003A2D RID: 14893
		private LabelStyle _breadcrumbArrowLabelStyle;

		// Token: 0x04003A2E RID: 14894
		private LabelStyle _breadcrumbCurrentLabelStyle;

		// Token: 0x04003A2F RID: 14895
		private Stack<string> _forwardStack = new Stack<string>();

		// Token: 0x04003A30 RID: 14896
		private bool _isSingleDirectoryForNavigationSelected;

		// Token: 0x02000FB1 RID: 4017
		[UIMarkupData]
		public class File
		{
			// Token: 0x04004BBD RID: 19389
			public string Name;

			// Token: 0x04004BBE RID: 19390
			public bool IsDirectory;
		}
	}
}
