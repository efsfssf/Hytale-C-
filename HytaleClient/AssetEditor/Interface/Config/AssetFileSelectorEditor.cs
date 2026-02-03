using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Data;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BB6 RID: 2998
	internal class AssetFileSelectorEditor : ValueEditor
	{
		// Token: 0x06005DD3 RID: 24019 RVA: 0x001DEA04 File Offset: 0x001DCC04
		public AssetFileSelectorEditor(Desktop desktop, Element parent, SchemaNode schema, PropertyPath path, PropertyEditor parentPropertyEditor, SchemaNode parentSchema, ConfigEditor configEditor, JToken value) : base(desktop, parent, schema, path, parentPropertyEditor, parentSchema, configEditor, value)
		{
		}

		// Token: 0x06005DD4 RID: 24020 RVA: 0x001DEA28 File Offset: 0x001DCC28
		protected override void Build()
		{
			FileDropdownBox fileDropdownBox = new FileDropdownBox(this.Desktop, this, "AssetEditor/FileSelector.ui", () => this.GetFiles(null));
			object selectedFiles;
			if (base.Value == null)
			{
				selectedFiles = null;
			}
			else
			{
				(selectedFiles = new HashSet<string>()).Add((string)base.Value);
			}
			fileDropdownBox.SelectedFiles = selectedFiles;
			fileDropdownBox.AllowedDirectories = this.Schema.AllowedDirectories;
			fileDropdownBox.SupportsUITextures = this.Schema.IsUITexture;
			fileDropdownBox.ValueChanged = new Action(this.OnSelectFile);
			fileDropdownBox.DropdownToggled = new Action(this.OnToggleDropdown);
			fileDropdownBox.SelectingInList = new Action(this.UpdatePreview);
			fileDropdownBox.Style = this.ConfigEditor.FileDropdownBoxStyle;
			this._dropdown = fileDropdownBox;
		}

		// Token: 0x06005DD5 RID: 24021 RVA: 0x001DEAF0 File Offset: 0x001DCCF0
		private void OnSelectFile()
		{
			HashSet<string> selectedFiles = this._dropdown.SelectedFiles;
			bool flag = selectedFiles == null || selectedFiles.Count == 0;
			if (!flag)
			{
				base.HandleChangeValue(Enumerable.First<string>(selectedFiles), false, false, false);
				this.Validate();
			}
		}

		// Token: 0x06005DD6 RID: 24022 RVA: 0x001DEB3C File Offset: 0x001DCD3C
		private void FetchTextureAsset(string path, Action<Image> action, CancellationToken cancellationToken)
		{
			this.ConfigEditor.AssetEditorOverlay.Backend.FetchAsset(new AssetReference("Texture", AssetPathUtils.GetAssetPathWithCommon(path)), delegate(object res, FormattedMessage error)
			{
				bool flag = !this._dropdown.IsMounted || cancellationToken.IsCancellationRequested;
				if (!flag)
				{
					this._previewCancellationToken = null;
					Image image;
					bool flag2;
					if (error == null)
					{
						image = (res as Image);
						flag2 = (image == null);
					}
					else
					{
						flag2 = true;
					}
					bool flag3 = flag2;
					if (flag3)
					{
						AssetFileSelectorEditor.Logger.Info("Failed to fetch preview for " + path);
						action(null);
					}
					else
					{
						action(image);
					}
				}
			}, false);
		}

		// Token: 0x06005DD7 RID: 24023 RVA: 0x001DEBA4 File Offset: 0x001DCDA4
		private void UpdatePreview()
		{
			bool flag = !this._dropdown.IsMounted;
			if (!flag)
			{
				HashSet<string> selectedFilesInList = this._dropdown.SelectedFilesInList;
				bool flag2 = selectedFilesInList == null || selectedFilesInList.Count != 1;
				if (!flag2)
				{
					string selectedFile = Enumerable.First<string>(selectedFilesInList);
					bool flag3 = !selectedFile.EndsWith(".png");
					if (!flag3)
					{
						CancellationTokenSource previewCancellationToken = this._previewCancellationToken;
						if (previewCancellationToken != null)
						{
							previewCancellationToken.Cancel();
						}
						this._previewCancellationToken = new CancellationTokenSource();
						CancellationToken cancellationToken = this._previewCancellationToken.Token;
						Action<Image> <>9__1;
						this.FetchTextureAsset(selectedFile, delegate(Image image)
						{
							bool flag4 = image == null;
							if (flag4)
							{
								bool flag5 = this._dropdown.SupportsUITextures && !selectedFile.EndsWith("@2x.png");
								if (flag5)
								{
									AssetFileSelectorEditor <>4__this = this;
									string path = selectedFile.Substring(0, selectedFile.Length - ".png".Length) + "@2x.png";
									Action<Image> action;
									if ((action = <>9__1) == null)
									{
										action = (<>9__1 = delegate(Image scaledImage)
										{
											bool flag6 = scaledImage == null;
											if (!flag6)
											{
												this._dropdown.SetPreviewImage(scaledImage);
											}
										});
									}
									<>4__this.FetchTextureAsset(path, action, cancellationToken);
								}
							}
							else
							{
								this._dropdown.SetPreviewImage(image);
							}
						}, cancellationToken);
					}
				}
			}
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x001DEC70 File Offset: 0x001DCE70
		private void OnToggleDropdown()
		{
			bool flag = !this._dropdown.IsOpen;
			if (!flag)
			{
				string text = null;
				bool flag2 = base.Value != null;
				if (flag2)
				{
					string[] array = ((string)base.Value).Split(new char[]
					{
						'/'
					}, StringSplitOptions.RemoveEmptyEntries);
					string text2 = string.Join("/", Enumerable.Take<string>(array, array.Length - 1));
					AssetFile assetFile;
					bool flag3 = this.ConfigEditor.AssetEditorOverlay.Assets.TryGetDirectory(AssetPathUtils.CombinePaths("Common", text2), out assetFile, false);
					if (flag3)
					{
						text = "/" + text2;
					}
				}
				bool flag4 = text == null && this.ConfigEditor.LastOpenedFileSelectorDirectory != null;
				if (flag4)
				{
					string text3 = this.ConfigEditor.LastOpenedFileSelectorDirectory;
					bool flag5 = text3 != "";
					if (flag5)
					{
						text3 += "/";
					}
					bool flag6 = this.Schema.AllowedDirectories != null && this.Schema.AllowedDirectories.Length != 0;
					if (flag6)
					{
						foreach (string value in this.Schema.AllowedDirectories)
						{
							bool flag7 = text3.StartsWith(value);
							if (flag7)
							{
								text = this.ConfigEditor.LastOpenedFileSelectorDirectory;
								break;
							}
						}
					}
					else
					{
						text = this.ConfigEditor.LastOpenedFileSelectorDirectory;
					}
				}
				bool flag8 = text == null && this.Schema.AllowedDirectories != null && this.Schema.AllowedDirectories.Length != 0;
				if (flag8)
				{
					string text4 = this.Schema.AllowedDirectories[0];
					bool flag9 = text4 != "/";
					if (flag9)
					{
						text4 = text4.TrimEnd(new char[]
						{
							'/'
						});
					}
					AssetFile assetFile2;
					bool flag10 = this.ConfigEditor.AssetEditorOverlay.Assets.TryGetDirectory(AssetPathUtils.CombinePaths("Common", text4), out assetFile2, false);
					if (flag10)
					{
						text = text4;
					}
				}
				bool flag11 = text == null;
				if (flag11)
				{
					text = this._dropdown.CurrentPath;
				}
				this._dropdown.Setup(text, this.GetFiles(text));
				this.UpdatePreview();
			}
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x001DEEA0 File Offset: 0x001DD0A0
		private List<FileSelector.File> GetFiles(string currentPath = null)
		{
			string text = this._dropdown.SearchQuery.Trim();
			bool flag = text != "" && text.Length < 3;
			List<FileSelector.File> result;
			if (flag)
			{
				result = new List<FileSelector.File>();
			}
			else
			{
				this.ConfigEditor.LastOpenedFileSelectorDirectory = (currentPath ?? this._dropdown.CurrentPath);
				result = this.ConfigEditor.AssetEditorOverlay.GetCommonFileSelectorFiles(AssetPathUtils.CombinePaths("Common", currentPath ?? this._dropdown.CurrentPath), text, this.Schema.AllowedFileExtensions, this.Schema.AllowedDirectories, 1000);
			}
			return result;
		}

		// Token: 0x06005DDA RID: 24026 RVA: 0x001DEF4A File Offset: 0x001DD14A
		public override void Focus()
		{
			this._dropdown.Open();
		}

		// Token: 0x06005DDB RID: 24027 RVA: 0x001DEF58 File Offset: 0x001DD158
		protected internal override void UpdateDisplayedValue()
		{
			FileDropdownBox dropdown = this._dropdown;
			object selectedFiles;
			if (base.Value == null)
			{
				selectedFiles = null;
			}
			else
			{
				(selectedFiles = new HashSet<string>()).Add((string)base.Value);
			}
			dropdown.SelectedFiles = selectedFiles;
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x001DEF89 File Offset: 0x001DD189
		protected override bool ValidateType(JToken value)
		{
			return value.Type == 8;
		}

		// Token: 0x04003AAC RID: 15020
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003AAD RID: 15021
		protected FileDropdownBox _dropdown;

		// Token: 0x04003AAE RID: 15022
		private CancellationTokenSource _previewCancellationToken;
	}
}
