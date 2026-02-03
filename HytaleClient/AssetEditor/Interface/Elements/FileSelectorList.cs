using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BAE RID: 2990
	internal class FileSelectorList : Element
	{
		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06005CFA RID: 23802 RVA: 0x001D6C9E File Offset: 0x001D4E9E
		public bool AreAllSelectedFilesInList
		{
			get
			{
				return this._activeEntryIndexes.Length == this._fileSelector.SelectedFiles.Count;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (set) Token: 0x06005CFB RID: 23803 RVA: 0x001D6CBA File Offset: 0x001D4EBA
		public ScrollbarStyle ScrollbarStyle
		{
			set
			{
				this._scrollbarStyle = value;
			}
		}

		// Token: 0x06005CFC RID: 23804 RVA: 0x001D6CC3 File Offset: 0x001D4EC3
		public FileSelectorList(Desktop desktop, Element parent, FileSelector fileSelector) : base(desktop, parent)
		{
			this._fileSelector = fileSelector;
		}

		// Token: 0x06005CFD RID: 23805 RVA: 0x001D6CF0 File Offset: 0x001D4EF0
		protected override void OnUnmounted()
		{
			this._hoveredEntryIndex = -1;
			this._focusedEntryIndex = -1;
			this._activeEntryIndexes = new int[0];
			this._entries = null;
		}

		// Token: 0x06005CFE RID: 23806 RVA: 0x001D6D14 File Offset: 0x001D4F14
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._folderIcon = this.Desktop.MakeTexturePatch(new PatchStyle("AssetEditor/AssetIcons/Folder.png")
			{
				Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 150)
			});
			this._fileIcon = this.Desktop.MakeTexturePatch(new PatchStyle("AssetEditor/AssetIcons/File.png"));
			this._textureIcon = this.Desktop.MakeTexturePatch(new PatchStyle("AssetEditor/AssetIcons/Texture.png"));
			this._modelIcon = this.Desktop.MakeTexturePatch(new PatchStyle("AssetEditor/AssetIcons/Model.png"));
			this._animationIcon = this.Desktop.MakeTexturePatch(new PatchStyle("AssetEditor/AssetIcons/Animation.png"));
			this._audioIcon = this.Desktop.MakeTexturePatch(new PatchStyle("AssetEditor/AssetIcons/Audio.png"));
			this._fontFamily = this.Desktop.Provider.GetFontFamily("Default");
		}

		// Token: 0x06005CFF RID: 23807 RVA: 0x001D6E08 File Offset: 0x001D5008
		private TexturePatch GetIcon(FileSelector.File file)
		{
			bool isDirectory = file.IsDirectory;
			TexturePatch result;
			if (isDirectory)
			{
				result = this._folderIcon;
			}
			else
			{
				bool flag = file.Name.EndsWith(".png");
				if (flag)
				{
					result = this._textureIcon;
				}
				else
				{
					bool flag2 = file.Name.EndsWith(".ogg");
					if (flag2)
					{
						result = this._audioIcon;
					}
					else
					{
						bool flag3 = file.Name.EndsWith(".blockymodel");
						if (flag3)
						{
							result = this._modelIcon;
						}
						else
						{
							bool flag4 = file.Name.EndsWith(".blockyanim");
							if (flag4)
							{
								result = this._animationIcon;
							}
							else
							{
								result = this._fileIcon;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005D00 RID: 23808 RVA: 0x001D6EB0 File Offset: 0x001D50B0
		protected override void LayoutSelf()
		{
			this._entries = new FileSelectorList.FileListEntry[this._fileSelector.Files.Count];
			List<int> list = new List<int>();
			string text = this._fileSelector.SearchQuery.ToLowerInvariant().Trim();
			string[] array = Enumerable.ToArray<string>(Enumerable.Select<string, string>(text.Split(new char[]
			{
				' '
			}, StringSplitOptions.RemoveEmptyEntries), (string k) => k.Trim()));
			Regex regex = new Regex("(" + string.Join("|", array) + ")", 1);
			for (int i = 0; i < this._fileSelector.Files.Count; i++)
			{
				FileSelector.File file = this._fileSelector.Files[i];
				bool flag = this._fileSelector.SearchQuery != "";
				if (flag)
				{
					string[] array2 = regex.Split(file.Name);
					FileSelectorList.FileListEntry fileListEntry = new FileSelectorList.FileListEntry
					{
						Text = new FileSelectorList.LabelSpanPortion[array2.Length],
						Icon = this.GetIcon(file)
					};
					int num = 0;
					for (int j = 0; j < array2.Length; j++)
					{
						string text2 = array2[j];
						bool flag2 = Enumerable.Contains<string>(array, text2.ToLowerInvariant());
						fileListEntry.Text[j] = new FileSelectorList.LabelSpanPortion(text2)
						{
							Color = (flag2 ? UInt32Color.White : UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 200)),
							IsBold = flag2,
							X = num
						};
						Font font = flag2 ? this._fontFamily.BoldFont : this._fontFamily.RegularFont;
						num += (int)(font.CalculateTextWidth(text2) * 13f / (float)font.BaseSize * this.Desktop.Scale);
					}
					this._entries[i] = fileListEntry;
				}
				else
				{
					this._entries[i] = new FileSelectorList.FileListEntry
					{
						Text = new FileSelectorList.LabelSpanPortion[]
						{
							new FileSelectorList.LabelSpanPortion(file.Name)
							{
								Color = (this._fileSelector.IsFileInAllowedDirectory(file) ? UInt32Color.White : FileSelectorList.DisabledColor)
							}
						},
						Icon = this.GetIcon(file)
					};
				}
				bool flag3 = this._fileSelector.SelectedFiles != null && this._fileSelector.SelectedFiles.Contains(this._fileSelector.GetFullPathOfFile(file.Name));
				if (flag3)
				{
					list.Add(i);
				}
			}
			this._activeEntryIndexes = list.ToArray();
			this.ContentHeight = new int?(this._entries.Length * 25);
		}

		// Token: 0x06005D01 RID: 23809 RVA: 0x001D718C File Offset: 0x001D538C
		public override Element HitTest(Point position)
		{
			bool flag = !base.Visible || !this._rectangleAfterPadding.Contains(position);
			Element result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = this;
			}
			return result;
		}

		// Token: 0x06005D02 RID: 23810 RVA: 0x001D71C4 File Offset: 0x001D53C4
		private void RefreshHoveredEntry()
		{
			bool flag = !this._viewRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this._hoveredEntryIndex = -1;
			}
			else
			{
				int num = this.Desktop.ScaleRound(25f);
				int num2 = (int)((float)(this.Desktop.MousePosition.Y - this._rectangleAfterPadding.Y + this._scaledScrollOffset.Y) / (float)num);
				this._hoveredEntryIndex = ((num2 < this._entries.Length) ? num2 : -1);
			}
		}

		// Token: 0x06005D03 RID: 23811 RVA: 0x001D7250 File Offset: 0x001D5450
		private void ScrollEntryIntoView(int index)
		{
			int num = this.Desktop.ScaleRound(25f);
			bool flag = num * (index + 1) > this._anchoredRectangle.Height + this._scaledScrollOffset.Y;
			if (flag)
			{
				base.SetScroll(new int?(0), new int?(num * (index + 1) - this._anchoredRectangle.Height));
			}
			else
			{
				bool flag2 = num * index < this._scaledScrollOffset.Y;
				if (flag2)
				{
					base.SetScroll(new int?(0), new int?(num * index));
				}
			}
		}

		// Token: 0x06005D04 RID: 23812 RVA: 0x001D72E4 File Offset: 0x001D54E4
		public void ScrollFirstActiveEntryIntoView()
		{
			bool flag = this._activeEntryIndexes.Length == 0;
			if (!flag)
			{
				this.ScrollEntryIntoView(this._activeEntryIndexes[0]);
			}
		}

		// Token: 0x06005D05 RID: 23813 RVA: 0x001D7314 File Offset: 0x001D5514
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = !activate;
			if (!flag)
			{
				bool flag2 = this._hoveredEntryIndex != -1;
				if (flag2)
				{
					FileSelector.File file = this._fileSelector.Files[this._hoveredEntryIndex];
					bool flag3 = !this._fileSelector.IsFileInAllowedDirectory(file);
					if (!flag3)
					{
						bool flag4 = evt.Clicks == 1 || this.Desktop.IsShortcutKeyDown || this.Desktop.IsShiftKeyDown;
						if (flag4)
						{
							this._focusedEntryIndex = this._hoveredEntryIndex;
							this._fileSelector.OnSelectFile(file);
						}
						else
						{
							bool flag5 = evt.Clicks == 2 && this._hoveredEntryIndex == this._focusedEntryIndex;
							if (flag5)
							{
								this._focusedEntryIndex = -1;
								this._fileSelector.OnOpenFile(file);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005D06 RID: 23814 RVA: 0x001D73ED File Offset: 0x001D55ED
		protected override void OnMouseMove()
		{
			this.RefreshHoveredEntry();
		}

		// Token: 0x06005D07 RID: 23815 RVA: 0x001D73F6 File Offset: 0x001D55F6
		protected override void OnMouseEnter()
		{
			this.RefreshHoveredEntry();
		}

		// Token: 0x06005D08 RID: 23816 RVA: 0x001D73FF File Offset: 0x001D55FF
		protected override void OnMouseLeave()
		{
			this._focusedEntryIndex = -1;
			this.RefreshHoveredEntry();
		}

		// Token: 0x06005D09 RID: 23817 RVA: 0x001D7410 File Offset: 0x001D5610
		protected override void PrepareForDrawSelf()
		{
			UInt32Color color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 140);
			UInt32Color color2 = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 64);
			TextureArea whitePixel = this.Desktop.Provider.WhitePixel;
			int num = this.Desktop.ScaleRound(25f);
			int num2 = this.Desktop.ScaleRound(5f);
			int num3 = this.Desktop.ScaleRound(4f);
			this.Desktop.Batcher2D.PushScissor(this._rectangleAfterPadding);
			for (int i = 0; i < this._activeEntryIndexes.Length; i++)
			{
				int num4 = this._rectangleAfterPadding.Y + num * this._activeEntryIndexes[i] - this._scaledScrollOffset.Y;
				bool flag = num4 > this._rectangleAfterPadding.Bottom;
				if (!flag)
				{
					int num5 = num4 + num;
					bool flag2 = num5 < this._rectangleAfterPadding.Top;
					if (!flag2)
					{
						this.Desktop.Batcher2D.RequestDrawTexture(whitePixel.Texture, whitePixel.Rectangle, new Rectangle(this._rectangleAfterPadding.X, num4, this._rectangleAfterPadding.Width, num), color);
					}
				}
			}
			for (int j = 0; j < this._entries.Length; j++)
			{
				FileSelectorList.FileListEntry fileListEntry = this._entries[j];
				int num6 = this._rectangleAfterPadding.Y + num * j - this._scaledScrollOffset.Y;
				int x = this._rectangleAfterPadding.X + num2;
				bool flag3 = num6 > this._rectangleAfterPadding.Bottom;
				if (!flag3)
				{
					int num7 = num6 + num;
					bool flag4 = num7 < this._rectangleAfterPadding.Top;
					if (!flag4)
					{
						bool flag5 = j == this._hoveredEntryIndex;
						if (flag5)
						{
							this.Desktop.Batcher2D.RequestDrawTexture(whitePixel.Texture, whitePixel.Rectangle, new Rectangle(this._rectangleAfterPadding.X, num6, this._rectangleAfterPadding.Width, num), color2);
						}
						bool flag6 = fileListEntry.Icon != null;
						if (flag6)
						{
							Rectangle destRect = new Rectangle(x, num6, num, num);
							this.Desktop.Batcher2D.RequestDrawPatch(fileListEntry.Icon, destRect, this.Desktop.Scale);
						}
					}
				}
			}
			for (int k = 0; k < this._entries.Length; k++)
			{
				FileSelectorList.FileListEntry fileListEntry2 = this._entries[k];
				int num8 = this._rectangleAfterPadding.Y + num * k - this._scaledScrollOffset.Y;
				int num9 = this._rectangleAfterPadding.X + num2;
				bool flag7 = num8 > this._rectangleAfterPadding.Bottom;
				if (!flag7)
				{
					int num10 = num8 + num;
					bool flag8 = num10 < this._rectangleAfterPadding.Top;
					if (!flag8)
					{
						float num11 = 13f * this.Desktop.Scale;
						float num12 = (float)num8 + (float)num / 2f;
						foreach (FileSelectorList.LabelSpanPortion labelSpanPortion in fileListEntry2.Text)
						{
							Font font = labelSpanPortion.IsBold ? this._fontFamily.BoldFont : this._fontFamily.RegularFont;
							float num13 = num11 / (float)font.BaseSize;
							float y = num12 - (float)((int)((float)font.Height * num13 / 2f));
							this.Desktop.Batcher2D.RequestDrawText(font, num11, labelSpanPortion.Text, new Vector3((float)(num9 + num + labelSpanPortion.X + num3), y, 0f), labelSpanPortion.Color, false, false, 0f);
						}
					}
				}
			}
			this.Desktop.Batcher2D.PopScissor();
		}

		// Token: 0x04003A31 RID: 14897
		private static readonly UInt32Color DisabledColor = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 80);

		// Token: 0x04003A32 RID: 14898
		private const int FontSize = 13;

		// Token: 0x04003A33 RID: 14899
		private const int RowHeight = 25;

		// Token: 0x04003A34 RID: 14900
		private readonly FileSelector _fileSelector;

		// Token: 0x04003A35 RID: 14901
		private FileSelectorList.FileListEntry[] _entries;

		// Token: 0x04003A36 RID: 14902
		private int _hoveredEntryIndex = -1;

		// Token: 0x04003A37 RID: 14903
		private int _focusedEntryIndex = -1;

		// Token: 0x04003A38 RID: 14904
		private int[] _activeEntryIndexes = new int[0];

		// Token: 0x04003A39 RID: 14905
		private FontFamily _fontFamily;

		// Token: 0x04003A3A RID: 14906
		private TexturePatch _folderIcon;

		// Token: 0x04003A3B RID: 14907
		private TexturePatch _fileIcon;

		// Token: 0x04003A3C RID: 14908
		private TexturePatch _textureIcon;

		// Token: 0x04003A3D RID: 14909
		private TexturePatch _modelIcon;

		// Token: 0x04003A3E RID: 14910
		private TexturePatch _animationIcon;

		// Token: 0x04003A3F RID: 14911
		private TexturePatch _audioIcon;

		// Token: 0x02000FB6 RID: 4022
		private class FileListEntry
		{
			// Token: 0x04004BC7 RID: 19399
			public FileSelectorList.LabelSpanPortion[] Text;

			// Token: 0x04004BC8 RID: 19400
			public TexturePatch Icon;
		}

		// Token: 0x02000FB7 RID: 4023
		private class LabelSpanPortion
		{
			// Token: 0x0600697E RID: 27006 RVA: 0x0021E413 File Offset: 0x0021C613
			public LabelSpanPortion(string text)
			{
				this.Text = text;
			}

			// Token: 0x04004BC9 RID: 19401
			public readonly string Text;

			// Token: 0x04004BCA RID: 19402
			public UInt32Color Color = UInt32Color.White;

			// Token: 0x04004BCB RID: 19403
			public bool IsBold;

			// Token: 0x04004BCC RID: 19404
			public int X;
		}
	}
}
