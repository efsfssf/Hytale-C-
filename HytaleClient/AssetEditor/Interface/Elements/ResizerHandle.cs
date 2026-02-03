using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;
using SDL2;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BAF RID: 2991
	public class ResizerHandle : Element
	{
		// Token: 0x06005D0B RID: 23819 RVA: 0x001D7831 File Offset: 0x001D5A31
		public ResizerHandle(Desktop desktop, Element parent) : base(desktop, parent)
		{
		}

		// Token: 0x06005D0C RID: 23820 RVA: 0x001D7840 File Offset: 0x001D5A40
		public override Element HitTest(Point position)
		{
			return this._rectangleAfterPadding.Contains(position) ? this : null;
		}

		// Token: 0x06005D0D RID: 23821 RVA: 0x001D7864 File Offset: 0x001D5A64
		protected override void OnMouseEnter()
		{
			this.UpdateResizerState();
		}

		// Token: 0x06005D0E RID: 23822 RVA: 0x001D786D File Offset: 0x001D5A6D
		protected override void OnMouseLeave()
		{
			SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
			this._isResizerHovered = false;
		}

		// Token: 0x06005D0F RID: 23823 RVA: 0x001D788D File Offset: 0x001D5A8D
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			Action mouseButtonReleased = this.MouseButtonReleased;
			if (mouseButtonReleased != null)
			{
				mouseButtonReleased();
			}
			this.Desktop.RefreshHover();
			this.UpdateResizerState();
		}

		// Token: 0x06005D10 RID: 23824 RVA: 0x001D78B8 File Offset: 0x001D5AB8
		protected override void OnMouseMove()
		{
			this.UpdateResizerState();
			bool flag = base.CapturedMouseButton == null;
			if (!flag)
			{
				Action resizing = this.Resizing;
				if (resizing != null)
				{
					resizing();
				}
			}
		}

		// Token: 0x06005D11 RID: 23825 RVA: 0x001D78F8 File Offset: 0x001D5AF8
		private void UpdateResizerState()
		{
			bool isResizerHovered = this._isResizerHovered;
			if (isResizerHovered)
			{
				bool flag = !this._rectangleAfterPadding.Contains(this.Desktop.MousePosition) && base.CapturedMouseButton == null;
				if (flag)
				{
					this._isResizerHovered = false;
					SDL.SDL_SetCursor(this.Desktop.Cursors.Arrow);
				}
			}
			else
			{
				bool flag2 = this._rectangleAfterPadding.Contains(this.Desktop.MousePosition) || base.CapturedMouseButton != null;
				if (flag2)
				{
					this._isResizerHovered = true;
					SDL.SDL_SetCursor(this.Desktop.Cursors.SizeWE);
				}
			}
		}

		// Token: 0x04003A40 RID: 14912
		public Action MouseButtonReleased;

		// Token: 0x04003A41 RID: 14913
		public Action Resizing;

		// Token: 0x04003A42 RID: 14914
		private bool _isResizerHovered;
	}
}
