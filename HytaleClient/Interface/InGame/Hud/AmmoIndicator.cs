using System;
using System.Collections.Generic;
using HytaleClient.Data.EntityStats;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B2 RID: 2226
	internal class AmmoIndicator : InterfaceComponent
	{
		// Token: 0x0600407C RID: 16508 RVA: 0x000BA2DC File Offset: 0x000B84DC
		public AmmoIndicator(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x000BA304 File Offset: 0x000B8504
		public void Build()
		{
			this.ResetState();
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/AmmoIndicator/AmmoIndicator.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._containerSize = document.ResolveNamedValue<int>(this.Interface, "ContainerSize");
			this._containerSpacing = document.ResolveNamedValue<int>(this.Interface, "ContainerSpacing");
			this._containerEmptyBackground = document.ResolveNamedValue<PatchStyle>(this.Interface, "ContainerEmptyBackground");
			this._containerFullBackground = document.ResolveNamedValue<PatchStyle>(this.Interface, "ContainerFullBackground");
			this._ammoContainersParent = uifragment.Get<Group>("AmmoContainers");
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x000BA3B0 File Offset: 0x000B85B0
		public void OnAmmoChanged(ClientEntityStatValue value)
		{
			int num = (int)value.Max;
			this._loaded = (int)value.Value;
			bool flag = this._max != num;
			if (flag)
			{
				this._max = num;
				this.BuildContainers();
			}
			else
			{
				this.Update();
			}
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x000BA400 File Offset: 0x000B8600
		private void BuildContainers()
		{
			this._ammoContainersParent.Clear();
			this._ammoContainers.Clear();
			for (int i = 0; i < this._max; i++)
			{
				List<Group> ammoContainers = this._ammoContainers;
				Group group = new Group(this._inGameView.Interface.Desktop, this._ammoContainersParent);
				group.Anchor.Width = new int?(this._containerSize);
				group.Anchor.Height = new int?(this._containerSize);
				group.Anchor.Horizontal = new int?(this._containerSpacing);
				group.Background = ((i <= this._loaded - 1) ? this._containerFullBackground : this._containerEmptyBackground);
				ammoContainers.Add(group);
			}
			base.Layout(null, true);
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x000BA4E0 File Offset: 0x000B86E0
		private void Update()
		{
			for (int i = 0; i < this._ammoContainers.Count; i++)
			{
				this._ammoContainers[i].Background = ((i < this._loaded) ? this._containerFullBackground : this._containerEmptyBackground);
			}
			base.Layout(null, true);
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x000BA543 File Offset: 0x000B8743
		public void ResetState()
		{
			this._ammoContainers.Clear();
			this._max = 0;
			this._loaded = 0;
		}

		// Token: 0x04001ECE RID: 7886
		private readonly InGameView _inGameView;

		// Token: 0x04001ECF RID: 7887
		private int _containerSize;

		// Token: 0x04001ED0 RID: 7888
		private int _containerSpacing;

		// Token: 0x04001ED1 RID: 7889
		private PatchStyle _containerEmptyBackground;

		// Token: 0x04001ED2 RID: 7890
		private PatchStyle _containerFullBackground;

		// Token: 0x04001ED3 RID: 7891
		private Group _ammoContainersParent;

		// Token: 0x04001ED4 RID: 7892
		private readonly List<Group> _ammoContainers = new List<Group>();

		// Token: 0x04001ED5 RID: 7893
		private int _max;

		// Token: 0x04001ED6 RID: 7894
		private int _loaded;
	}
}
