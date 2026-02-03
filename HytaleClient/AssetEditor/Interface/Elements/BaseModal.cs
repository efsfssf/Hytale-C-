using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Elements
{
	// Token: 0x02000BA8 RID: 2984
	internal abstract class BaseModal : Element
	{
		// Token: 0x06005C8F RID: 23695 RVA: 0x001D3658 File Offset: 0x001D1858
		protected BaseModal(AssetEditorInterface @interface, string documentPath) : base(@interface.Desktop, null)
		{
			this._interface = @interface;
			this._documentPath = documentPath;
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x001D3678 File Offset: 0x001D1878
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument(this._documentPath, out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._container = uifragment.Get<Group>("Container");
			this._content = uifragment.Get<Group>("Content");
			this._title = uifragment.Get<Label>("Title");
			uifragment.Get<TextButton>("CloseButton").Activating = new Action(this.Dismiss);
			this.BuildModal(document, uifragment);
		}

		// Token: 0x06005C91 RID: 23697
		protected abstract void BuildModal(Document doc, UIFragment fragment);

		// Token: 0x06005C92 RID: 23698 RVA: 0x001D3710 File Offset: 0x001D1910
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x001D3759 File Offset: 0x001D1959
		protected void OpenInLayer()
		{
			this.Desktop.SetLayer(4, this);
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x001D3769 File Offset: 0x001D1969
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(4);
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x001D3778 File Offset: 0x001D1978
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x040039FC RID: 14844
		protected Group _container;

		// Token: 0x040039FD RID: 14845
		protected Group _content;

		// Token: 0x040039FE RID: 14846
		protected Label _title;

		// Token: 0x040039FF RID: 14847
		private readonly string _documentPath;

		// Token: 0x04003A00 RID: 14848
		protected readonly AssetEditorInterface _interface;
	}
}
