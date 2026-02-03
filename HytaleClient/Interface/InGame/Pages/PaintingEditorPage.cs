using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Pages
{
	// Token: 0x0200088F RID: 2191
	internal class PaintingEditorPage : InterfaceComponent
	{
		// Token: 0x06003EDB RID: 16091 RVA: 0x000ABC8C File Offset: 0x000A9E8C
		public PaintingEditorPage(InGameView inGameView) : base(inGameView.Interface, null)
		{
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x000ABCA0 File Offset: 0x000A9EA0
		public void Build()
		{
			base.Clear();
			Element element = new Element(this.Desktop, this);
			element.Anchor = new Anchor
			{
				Width = new int?(200),
				Height = new int?(200)
			};
			element.Background = new PatchStyle(305420031U);
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x000ABD02 File Offset: 0x000A9F02
		protected override void OnMounted()
		{
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x000ABD05 File Offset: 0x000A9F05
		protected override void OnUnmounted()
		{
		}
	}
}
