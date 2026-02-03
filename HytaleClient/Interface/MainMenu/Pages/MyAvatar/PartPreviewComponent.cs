using System;
using HytaleClient.Application;
using HytaleClient.Data.Characters;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.Interface.MainMenu.Pages.MyAvatar
{
	// Token: 0x02000823 RID: 2083
	internal class PartPreviewComponent : Button
	{
		// Token: 0x06003A19 RID: 14873 RVA: 0x00082FD4 File Offset: 0x000811D4
		public PartPreviewComponent(MyAvatarPage myAvatarPage, Element parent, int row) : base(myAvatarPage.Desktop, parent)
		{
			this._myAvatarPage = myAvatarPage;
			this.Row = row;
			base.TextTooltipShowDelay = new float?(0.2f);
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00083004 File Offset: 0x00081204
		public void Setup(PlayerSkinProperty property, CharacterPart part, CharacterPartId id, UInt32Color backgroundColor, UInt32Color backgroundColorHovered, bool updateRender = true)
		{
			base.TooltipText = part.Name;
			this._id = id;
			this._property = property;
			this._backgroundColor = backgroundColor;
			this._backgroundColorHovered = backgroundColorHovered;
			if (updateRender)
			{
				this.Update();
			}
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x0008304C File Offset: 0x0008124C
		public void Update()
		{
			bool flag = !this.IsInView;
			if (!flag)
			{
				this._myAvatarPage.RenderCharacterPartPreviewCommandQueue.Add(new AppMainMenu.RenderCharacterPartPreviewCommand
				{
					Id = this._id,
					Property = this._property,
					Selected = this.IsSelected,
					BackgroundColor = new ColorRgba?(new ColorRgba(base.IsHovered ? this._backgroundColorHovered.ABGR : this._backgroundColor.ABGR))
				});
			}
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x000830D3 File Offset: 0x000812D3
		protected override void OnMouseEnter()
		{
			base.OnMouseEnter();
			this.Update();
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x000830E4 File Offset: 0x000812E4
		protected override void OnMouseLeave()
		{
			base.OnMouseLeave();
			this.Update();
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x000830F8 File Offset: 0x000812F8
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool flag = this.Texture != null;
			if (flag)
			{
				Rectangle rectangle = new TextureArea(this.Texture, 0, 0, this.Texture.Width, this.Texture.Height, 1).Rectangle;
				this.Desktop.Batcher2D.RequestDrawTexture(this.Texture, rectangle, this._anchoredRectangle, UInt32Color.White);
			}
		}

		// Token: 0x04001A14 RID: 6676
		private CharacterPartId _id;

		// Token: 0x04001A15 RID: 6677
		private PlayerSkinProperty _property;

		// Token: 0x04001A16 RID: 6678
		private UInt32Color _backgroundColor;

		// Token: 0x04001A17 RID: 6679
		private UInt32Color _backgroundColorHovered;

		// Token: 0x04001A18 RID: 6680
		public readonly int Row;

		// Token: 0x04001A19 RID: 6681
		public bool IsInView;

		// Token: 0x04001A1A RID: 6682
		public Texture Texture;

		// Token: 0x04001A1B RID: 6683
		public bool IsSelected;

		// Token: 0x04001A1C RID: 6684
		private readonly MyAvatarPage _myAvatarPage;
	}
}
