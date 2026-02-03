using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BAC RID: 2988
	internal class FileDropdownLayer : Element
	{
		// Token: 0x06005CCA RID: 23754 RVA: 0x001D4D88 File Offset: 0x001D2F88
		public FileDropdownLayer(FileDropdownBox dropdownBox, string templatePath, Func<List<FileSelector.File>> fileGetter) : base(dropdownBox.Desktop, null)
		{
			this._fileDropdownBox = dropdownBox;
			this.FileSelector = new FileSelector(dropdownBox.Desktop, this, templatePath, fileGetter)
			{
				Cancelling = new Action(dropdownBox.CloseDropdown)
			};
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x001D4DC6 File Offset: 0x001D2FC6
		protected override void OnUnmounted()
		{
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x001D4DCC File Offset: 0x001D2FCC
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			int num = this.Desktop.UnscaleRound((float)this._fileDropdownBox.AnchoredRectangle.X);
			int num2 = this.Desktop.UnscaleRound((float)this._fileDropdownBox.AnchoredRectangle.Top);
			int num3 = this.Desktop.UnscaleRound((float)this._fileDropdownBox.AnchoredRectangle.Width);
			int num4 = this.Desktop.UnscaleRound((float)this._fileDropdownBox.AnchoredRectangle.Height);
			FileSelector fileSelector = this.FileSelector;
			fileSelector.Anchor.Width = fileSelector.Children[0].Anchor.Width;
			fileSelector.Anchor.Height = fileSelector.Children[0].Anchor.Height;
			int num5 = this.Desktop.UnscaleRound((float)this.Desktop.ViewportRectangle.Height);
			int num6 = this.Desktop.UnscaleRound((float)this.Desktop.ViewportRectangle.Width);
			FileDropdownBoxStyle style = this._fileDropdownBox.Style;
			fileSelector.Anchor.Top = new int?(num2 + num4 + style.PanelOffset);
			int? num7 = fileSelector.Anchor.Top + fileSelector.Anchor.Height;
			int num8 = num5;
			bool flag = num7.GetValueOrDefault() > num8 & num7 != null;
			if (flag)
			{
				fileSelector.Anchor.Top = new int?(Math.Max((num2 - fileSelector.Anchor.Height - style.PanelOffset).Value, 0));
			}
			fileSelector.Anchor.Left = new int?(num);
			num7 = fileSelector.Anchor.Left + fileSelector.Anchor.Width;
			num8 = num6;
			bool flag2 = num7.GetValueOrDefault() > num8 & num7 != null;
			if (flag2)
			{
				fileSelector.Anchor.Left = num + num3 - fileSelector.Anchor.Width;
			}
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x001D50C4 File Offset: 0x001D32C4
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

		// Token: 0x06005CCE RID: 23758 RVA: 0x001D5108 File Offset: 0x001D3308
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			bool flag = !activate || (long)evt.Button != 1L;
			if (!flag)
			{
				this._fileDropdownBox.CloseDropdown();
			}
		}

		// Token: 0x06005CCF RID: 23759 RVA: 0x001D513C File Offset: 0x001D333C
		protected internal override void OnKeyUp(SDL.SDL_Keycode keycode)
		{
			this.FileSelector.OnKeyUp(keycode);
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x001D514C File Offset: 0x001D334C
		protected internal override void Dismiss()
		{
			this._fileDropdownBox.CloseDropdown();
		}

		// Token: 0x04003A11 RID: 14865
		public readonly FileSelector FileSelector;

		// Token: 0x04003A12 RID: 14866
		private FileDropdownBox _fileDropdownBox;
	}
}
