using System;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Networking;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008C2 RID: 2242
	internal class PlayerListComponent : InterfaceComponent
	{
		// Token: 0x06004106 RID: 16646 RVA: 0x000BDB6B File Offset: 0x000BBD6B
		public PlayerListComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x000BDB88 File Offset: 0x000BBD88
		public void ResetState()
		{
		}

		// Token: 0x06004108 RID: 16648 RVA: 0x000BDB8C File Offset: 0x000BBD8C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/PlayerList.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._serverDetails = uifragment.Get<Group>("ServerDetails");
			this._serverName = uifragment.Get<Label>("ServerName");
			this._motd = uifragment.Get<Label>("Motd");
			this._playerCount = uifragment.Get<Label>("PlayerCount");
			this._listContainer = uifragment.Get<Group>("ListContainer");
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.UpdateServerDetails();
				this.UpdateList();
			}
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x000BDC31 File Offset: 0x000BBE31
		protected override void OnMounted()
		{
			this.UpdateServerDetails();
			this.UpdateList();
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x000BDC44 File Offset: 0x000BBE44
		public void UpdateServerDetails()
		{
			this._serverDetails.Visible = (this._inGameView.ServerName != null);
			this._serverName.Text = this._inGameView.ServerName;
			this._motd.Text = this._inGameView.Motd;
			bool isMounted = this._serverDetails.IsMounted;
			if (isMounted)
			{
				this._serverDetails.Layout(null, true);
			}
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x000BDCC4 File Offset: 0x000BBEC4
		public void UpdateList()
		{
			this._listContainer.Clear();
			this._playerCount.Text = ((this._inGameView.MaxPlayers > 0) ? string.Format("({0}/{1})", this._inGameView.Players.Count, this._inGameView.MaxPlayers) : string.Format("({0})", this._inGameView.Players.Count));
			this._playerCount.Layout(null, true);
			bool flag = this._inGameView.Players.Count > 0;
			if (flag)
			{
				foreach (PacketHandler.PlayerListPlayer playerListPlayer in this._inGameView.Players.Values)
				{
					Group parent = new Group(this.Desktop, this._listContainer)
					{
						LayoutMode = LayoutMode.Left
					};
					Label label = new Label(this.Desktop, parent);
					label.Anchor.Width = new int?(250);
					Label label2 = label;
					Label label3 = new Label(this.Desktop, parent);
					label3.Anchor.Width = new int?(250);
					Label label4 = label3;
					label2.Text = playerListPlayer.DisplayName;
					label4.Text = playerListPlayer.Ping.ToString() + "ms";
				}
				this._listContainer.Layout(null, true);
			}
		}

		// Token: 0x04001F3E RID: 7998
		private readonly InGameView _inGameView;

		// Token: 0x04001F3F RID: 7999
		private Group _serverDetails;

		// Token: 0x04001F40 RID: 8000
		private Label _serverName;

		// Token: 0x04001F41 RID: 8001
		private Label _motd;

		// Token: 0x04001F42 RID: 8002
		private Label _playerCount;

		// Token: 0x04001F43 RID: 8003
		private Group _listContainer;
	}
}
