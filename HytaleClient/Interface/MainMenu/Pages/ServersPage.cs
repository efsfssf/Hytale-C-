using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.Application;
using HytaleClient.Application.Services.Api;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Utils;

namespace HytaleClient.Interface.MainMenu.Pages
{
	// Token: 0x0200081F RID: 2079
	internal class ServersPage : InterfaceComponent
	{
		// Token: 0x060039BB RID: 14779 RVA: 0x0007EB23 File Offset: 0x0007CD23
		public ServersPage(MainMenuView mainMenuView) : base(mainMenuView.Interface, null)
		{
			this.MainMenuView = mainMenuView;
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x0007EB48 File Offset: 0x0007CD48
		public void Build()
		{
			this._serverButtons.Clear();
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/Servers/ServersPage.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._serverBrowserTopTextButtonStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Interface, "ServerBrowserTopTextButtonStyle");
			this._serverBrowserTopTextButtonSelectedStyle = document.ResolveNamedValue<TextButton.TextButtonStyle>(this.Interface, "ServerBrowserTopTextButtonSelectedStyle");
			this._serverBrowserRowButtonStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Interface, "ServerBrowserRowButtonStyle");
			this._serverBrowserRowButtonSelectedStyle = document.ResolveNamedValue<Button.ButtonStyle>(this.Interface, "ServerBrowserRowButtonSelectedStyle");
			this._serversTableBody = uifragment.Get<Group>("ServersTableBody");
			this._serversTableLoading = uifragment.Get<Element>("ServersTableLoading");
			this._serversTableStatus = uifragment.Get<Group>("ServersTableStatus");
			this._serversTableStatusText = uifragment.Get<Label>("ServersTableStatusText");
			this._joinServerSound = document.ResolveNamedValue<SoundStyle>(this.Interface, "JoinServerSound");
			this._connectionErrorSound = document.ResolveNamedValue<SoundStyle>(this.Interface, "ConnectionErrorSound");
			this._columnButtonSortCarets = new Group[3];
			this._columnButtonReverseSortCarets = new Group[3];
			this.<Build>g__BuildColumnHeaderButton|44_0(0, uifragment.Get<Button>("NameColumnHeaderButton"), Server.NameSort);
			this.<Build>g__BuildColumnHeaderButton|44_0(1, uifragment.Get<Button>("RatingColumnHeaderButton"), Server.RatingSort);
			this.<Build>g__BuildColumnHeaderButton|44_0(2, uifragment.Get<Button>("OnlineColumnHeaderButton"), Server.OnlinePlayersSort);
			this.SetServerSortOptions(0, Server.NameSort);
			this._showInternetServersButton = uifragment.Get<TextButton>("ShowInternetServersButton");
			this._showInternetServersButton.Activating = delegate()
			{
				this.ClearSearchTextInput();
				this.Interface.App.MainMenu.FetchAndShowPublicServers(null, null);
			};
			this._showFavoriteServersButton = uifragment.Get<TextButton>("ShowFavoriteServersButton");
			this._showFavoriteServersButton.Activating = delegate()
			{
				this.ClearSearchTextInput();
				this.Interface.App.MainMenu.FetchAndShowFavoriteServers();
			};
			this._showRecentServersButton = uifragment.Get<TextButton>("ShowRecentServersButton");
			this._showRecentServersButton.Activating = delegate()
			{
				this.ClearSearchTextInput();
				this.Interface.App.MainMenu.FetchAndShowRecentServers();
			};
			this._showFriendServersButton = uifragment.Get<TextButton>("ShowFriendsServersButton");
			this._showFriendServersButton.Activating = delegate()
			{
				this.ClearSearchTextInput();
				this.Interface.App.MainMenu.ShowFriendsServers();
			};
			this._searchTextInput = uifragment.Get<TextField>("ServerSearchField");
			this._searchTextInput.ValueChanged = new Action(this.Search);
			this._activeTagsTableBody = uifragment.Get<Group>("ActiveTags");
			this._showDirectConnectButton = uifragment.Get<TextButton>("DirectConnectButton");
			this._showDirectConnectButton.Activating = new Action(this.OnDirectConnectButtonActivate);
			Document document2;
			this.Interface.TryGetDocument("MainMenu/Servers/DirectConnectPopup.ui", out document2);
			UIFragment uifragment2 = document2.Instantiate(this.Desktop, null);
			this._directConnectPopup = (Group)uifragment2.RootElements[0];
			this._directConnectPopup.Validating = new Action(this.OnDirectConnectPopupValidate);
			this._directConnectPopup.Dismissing = new Action(this.OnDirectConnectPopupDismiss);
			this._directConnectAddressTextInput = uifragment2.Get<TextField>("ServerAddress");
			this._directConnectPopupStatusLabel = uifragment2.Get<Label>("StatusLabel");
			this._directConnectPopupCancelButton = uifragment2.Get<TextButton>("CancelButton");
			this._directConnectPopupCancelButton.Activating = new Action(this.OnDirectConnectPopupDismiss);
			this._directConnectPopupConnectButton = uifragment2.Get<TextButton>("ConnectButton");
			this._directConnectPopupConnectButton.Activating = new Action(this.OnDirectConnectPopupValidate);
			this._serverJoinButton = uifragment.Get<TextButton>("ServerJoin");
			this._serverJoinButton.Activating = delegate()
			{
				this.Interface.App.MainMenu.TryConnectToServer(this._selectedServerDetails);
				this.Interface.PlaySound(this._joinServerSound);
			};
			this._serverDetailsGroup = uifragment.Get<Group>("ServerDetails");
			this._serverDescriptionLabel = uifragment.Get<Label>("ServerDescriptionCell");
			this._ipLabel = uifragment.Get<Label>("Ip");
			this._languagesLabel = uifragment.Get<Label>("Languages");
			this._uuidLabel = uifragment.Get<Label>("Uuid");
			this._onlineLabel = uifragment.Get<Label>("Online");
			this._favoriteServerLabel = uifragment.Get<Label>("FavoriteServerLabel");
			this._branchLabel = uifragment.Get<Label>("ServerBranch");
			this._favoriteServerButton = uifragment.Get<TextButton>("FavoriteServerButton");
			this._favoriteServerButton.Activating = new Action(this.OnActiveFavoriteServerButton);
			this._tagsSelected = new List<string>();
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.BuildServerList();
			}
			this.ApplyTabButtonStyles();
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x0007EF9E File Offset: 0x0007D19E
		protected override void OnMounted()
		{
			this.MainMenuView.ShowTopBar(true);
			this.Interface.App.MainMenu.FetchAndShowPublicServers(null, null);
			this.ClearSearchTextInput();
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x0007EFCD File Offset: 0x0007D1CD
		protected override void OnUnmounted()
		{
			this._selectedServerDetails = null;
			this.Interface.App.MainMenu.CancelFetchServerDetails();
		}

		// Token: 0x060039BF RID: 14783 RVA: 0x0007EFED File Offset: 0x0007D1ED
		public void FocusTextSearchInput()
		{
			this.Desktop.FocusElement(this._searchTextInput, true);
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x0007F004 File Offset: 0x0007D204
		private void OnDirectConnectButtonActivate()
		{
			this._directConnectPopupStatusLabel.Text = "";
			this._directConnectPopupStatusLabel.Visible = false;
			this._directConnectAddressTextInput.SelectAll();
			this.Desktop.SetLayer(2, this._directConnectPopup);
			this.Desktop.FocusElement(this._directConnectAddressTextInput, true);
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x0007F064 File Offset: 0x0007D264
		private void OnActiveFavoriteServerButton()
		{
			bool flag = this._selectedServerDetails == null;
			if (!flag)
			{
				AppMainMenu mainMenu = this.Interface.App.MainMenu;
				bool isFavorite = this._selectedServerDetails.IsFavorite;
				if (isFavorite)
				{
					mainMenu.RemoveServerFromFavorites(this._selectedServerDetails.UUID);
					int serverIndex = this.GetServerIndex(this._selectedServerDetails.UUID);
					bool flag2 = mainMenu.ActiveServerListTab == AppMainMenu.ServerListTab.Favorites && serverIndex > -1;
					if (flag2)
					{
						ArrayUtils.RemoveAt<Server>(ref this._servers, serverIndex);
						this.BuildServerList();
						return;
					}
				}
				else
				{
					mainMenu.AddServerToFavorites(this._selectedServerDetails.UUID);
				}
				this._selectedServerDetails.IsFavorite = !this._selectedServerDetails.IsFavorite;
				this.BuildServerDetailsPanel(true);
			}
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x0007F12E File Offset: 0x0007D32E
		private void OnDirectConnectPopupDismiss()
		{
			this.Desktop.ClearLayer(2);
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x0007F140 File Offset: 0x0007D340
		private void DirectConnect()
		{
			string text = this._directConnectAddressTextInput.Value;
			bool flag = text.Length == 0;
			if (flag)
			{
				text = "127.0.0.1";
			}
			string text2;
			bool flag2 = !this.Interface.App.MainMenu.CanConnectToServer("connect to " + text, out text2);
			if (flag2)
			{
				this._directConnectPopupStatusLabel.Text = text2;
				this._directConnectPopupStatusLabel.Visible = true;
				this._directConnectPopup.Layout(null, true);
			}
			else
			{
				Interface.Logger.Info("Direct connecting to multiplayer server at {0}", text);
				string hostname;
				int port;
				string text3;
				bool flag3 = !HostnameHelper.TryParseHostname(text, 5520, out hostname, out port, out text3);
				if (flag3)
				{
					Interface.Logger.Warn("Invalid address: {0}", text3);
					this._directConnectPopupStatusLabel.Text = "Invalid address: " + text3;
					this._directConnectPopupStatusLabel.Visible = true;
					this._directConnectPopup.Layout(null, true);
					this.Interface.PlaySound(this._connectionErrorSound);
				}
				else
				{
					this.Interface.PlaySound(this._joinServerSound);
					this.Interface.FadeOut(delegate
					{
						this.Interface.App.GameLoading.Open(hostname, port, null);
						this.Interface.FadeIn(null, false);
					});
				}
			}
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x0007F29C File Offset: 0x0007D49C
		private void OnDirectConnectPopupValidate()
		{
			this.DirectConnect();
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x0007F2A6 File Offset: 0x0007D4A6
		private void OnChangeSort(int columnIndex, Comparison<Server> comparison)
		{
			this.SetServerSortOptions(columnIndex, comparison);
			this.BuildServerList();
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x0007F2BC File Offset: 0x0007D4BC
		private void SetServerSortOptions(int columnIndex, Comparison<Server> comparison)
		{
			bool flag = this._listComparison == comparison;
			if (flag)
			{
				this._reverseSort = !this._reverseSort;
			}
			else
			{
				this._listComparison = comparison;
				this._reverseSort = false;
			}
			for (int i = 0; i < this._columnButtonSortCarets.Length; i++)
			{
				bool flag2 = i == columnIndex;
				this._columnButtonSortCarets[i].Visible = (flag2 && !this._reverseSort);
				this._columnButtonReverseSortCarets[i].Visible = (flag2 && this._reverseSort);
			}
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x0007F354 File Offset: 0x0007D554
		public void HandleAutoConnectOnStartup(string address)
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				this.OnDirectConnectButtonActivate();
				this._directConnectAddressTextInput.Value = address;
				this.DirectConnect();
			}
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x0007F390 File Offset: 0x0007D590
		public void OnFailedToToggleFavoriteServer(string errorMessage)
		{
			AppMainMenu mainMenu = this.Interface.App.MainMenu;
			bool flag = base.IsMounted && mainMenu.ActiveServerListTab == AppMainMenu.ServerListTab.Favorites;
			if (flag)
			{
				mainMenu.FetchAndShowFavoriteServers();
			}
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x0007F3D0 File Offset: 0x0007D5D0
		public void OnServersReceived(Server[] servers)
		{
			this._servers = servers;
			this._serversErrorMessage = ((servers == null) ? this.Desktop.Provider.GetText("ui.mainMenu.servers.failedToFetch", null, true) : null);
			this.BuildServerList();
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x0007F404 File Offset: 0x0007D604
		public void SetSelectedServerDetails(Server server, bool doLayout = true)
		{
			this._selectedServerDetails = server;
			this.BuildServerDetailsPanel(doLayout);
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x0007F418 File Offset: 0x0007D618
		private void BuildServerDetailsPanel(bool doLayout = true)
		{
			Server selectedServerDetails = this._selectedServerDetails;
			bool flag = selectedServerDetails != null;
			if (flag)
			{
				this._serverDetailsGroup.Visible = true;
				this._serverDescriptionLabel.Text = selectedServerDetails.Description;
				this._ipLabel.Text = this.Desktop.Provider.GetText("ui.mainMenu.servers.details.host", new Dictionary<string, string>
				{
					{
						"host",
						selectedServerDetails.Host
					}
				}, true);
				this._languagesLabel.Text = this.Desktop.Provider.GetText("ui.mainMenu.servers.details.languages", new Dictionary<string, string>
				{
					{
						"languages",
						string.Join(", ", selectedServerDetails.Languages)
					}
				}, true);
				this._uuidLabel.Text = this.Desktop.Provider.GetText("ui.mainMenu.servers.details.uuid", new Dictionary<string, string>
				{
					{
						"uuid",
						selectedServerDetails.UUID.ToString()
					}
				}, true);
				this._onlineLabel.Text = this.Desktop.Provider.GetText("ui.mainMenu.servers.details.online", new Dictionary<string, string>
				{
					{
						"online",
						selectedServerDetails.IsOnline.ToString()
					}
				}, true);
				this._branchLabel.Text = this.Desktop.Provider.GetText("ui.mainMenu.servers.details.branch", new Dictionary<string, string>
				{
					{
						"name",
						selectedServerDetails.Version
					}
				}, true);
				bool isFavorite = selectedServerDetails.IsFavorite;
				if (isFavorite)
				{
					this._favoriteServerLabel.Text = this.Desktop.Provider.GetText("ui.mainMenu.servers.details.favoriteYes", null, true);
					this._favoriteServerButton.Text = this.Desktop.Provider.GetText("ui.general.remove", null, true);
				}
				else
				{
					this._favoriteServerLabel.Text = this.Desktop.Provider.GetText("ui.mainMenu.servers.details.favoriteNo", null, true);
					this._favoriteServerButton.Text = this.Desktop.Provider.GetText("ui.general.add", null, true);
				}
			}
			else
			{
				this._serverDetailsGroup.Visible = false;
			}
			if (doLayout)
			{
				base.Layout(null, true);
			}
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x0007F654 File Offset: 0x0007D854
		private void BuildServerListRows()
		{
			Document document;
			this.Interface.TryGetDocument("MainMenu/Servers/ServersTableRow.ui", out document);
			Document document2;
			this.Interface.TryGetDocument("MainMenu/Servers/ServersRowTags.ui", out document2);
			Server[] array = (Server[])this._servers.Clone();
			Array.Sort<Server>(array, this._listComparison);
			bool reverseSort = this._reverseSort;
			if (reverseSort)
			{
				Array.Reverse(array);
			}
			this._serverButtons.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				Server server = array[i];
				UIFragment uifragment = document.Instantiate(this.Desktop, this._serversTableBody);
				Button button = uifragment.Get<Button>("Row");
				this._serverButtons[server.UUID] = button;
				uifragment.Get<Label>("NameCellLabel").Text = server.Name;
				uifragment.Get<Label>("RatingCellLabel").Text = "Rating";
				uifragment.Get<Label>("OnlineCellLabel").Text = string.Format("{0} / {1}", server.OnlinePlayers, server.MaxPlayers);
				bool flag = server.UUID.Equals(this._selectedServer);
				if (flag)
				{
					button.Style = this._serverBrowserRowButtonSelectedStyle;
				}
				Group root = uifragment.Get<Group>("TagsTableBody");
				using (List<string>.Enumerator enumerator = server.Tags.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string tag = enumerator.Current;
						UIFragment uifragment2 = document2.Instantiate(this.Desktop, root);
						TextButton textButton = uifragment2.Get<TextButton>("ServerTag");
						textButton.Text = tag;
						textButton.Activating = delegate()
						{
							bool flag2 = this._tagsSelected.Contains(tag);
							if (!flag2)
							{
								this._tagsSelected.Add(tag);
								this.SearchByTag();
							}
						};
					}
				}
				uifragment.Get<Button>("Row").Activating = delegate()
				{
					this.OnSelectServer(server);
				};
				uifragment.Get<Button>("Row").DoubleClicking = delegate()
				{
					this.Interface.App.MainMenu.TryConnectToServer(server);
					this.Interface.PlaySound(this._joinServerSound);
				};
			}
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x0007F8C4 File Offset: 0x0007DAC4
		public void BuildServerList()
		{
			bool hasMarkupError = this.Interface.HasMarkupError;
			if (!hasMarkupError)
			{
				this._selectedServer = Guid.Empty;
				this._selectedServerDetails = null;
				this._serversTableBody.Clear();
				this.SetSelectedServerDetails(null, false);
				bool isFetchingList = this.Interface.App.MainMenu.IsFetchingList;
				bool flag = this._servers != null && !isFetchingList;
				if (flag)
				{
					this._serversTableStatus.Visible = false;
					bool flag2 = this._servers.Length != 0;
					if (flag2)
					{
						this._selectedServer = this._servers[0].UUID;
						this.Interface.App.MainMenu.FetchServerDetails(this._selectedServer);
					}
					this.BuildServerListRows();
				}
				else
				{
					this._serversTableStatus.Visible = true;
					this._serversTableLoading.Visible = isFetchingList;
					bool flag3 = isFetchingList;
					if (flag3)
					{
						this._serversTableStatusText.Text = this.Desktop.Provider.GetText("ui.general.loading", null, true);
					}
					else
					{
						this._serversTableStatusText.Text = (this._serversErrorMessage ?? "failedToFetchServers");
					}
				}
				base.Layout(null, true);
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x0007FA04 File Offset: 0x0007DC04
		private void OnSelectServer(Server server)
		{
			bool isShiftKeyDown = this.Desktop.IsShiftKeyDown;
			if (isShiftKeyDown)
			{
				this.Interface.ModalDialog.Setup(new ModalDialog.DialogSetup
				{
					Title = "Reboot server?",
					Text = string.Format("{0}\n{1} / {2}", server.Name, server.OnlinePlayers, server.MaxPlayers),
					OnConfirm = delegate()
					{
						this.Interface.App.MainMenu.RebootServer(server.Host);
					}
				});
				this.Desktop.SetLayer(4, this.Interface.ModalDialog);
			}
			else
			{
				this.Interface.App.MainMenu.FetchServerDetails(server.UUID);
				Button button;
				bool flag = this._selectedServer != Guid.Empty && this._serverButtons.TryGetValue(this._selectedServer, out button);
				if (flag)
				{
					button.Style = this._serverBrowserRowButtonStyle;
				}
				this._selectedServer = server.UUID;
				this._serverButtons[this._selectedServer].Style = this._serverBrowserRowButtonSelectedStyle;
				base.Layout(null, true);
			}
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x0007FB64 File Offset: 0x0007DD64
		public void OnActiveTabChanged(bool cleanTags = true)
		{
			this.ApplyTabButtonStyles();
			if (cleanTags)
			{
				this._tagsSelected.Clear();
			}
			this.BuildSearchedTags();
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x0007FB94 File Offset: 0x0007DD94
		public void ApplyTabButtonStyles()
		{
			this._showInternetServersButton.Style = this._serverBrowserTopTextButtonStyle;
			this._showFriendServersButton.Style = this._serverBrowserTopTextButtonStyle;
			this._showFavoriteServersButton.Style = this._serverBrowserTopTextButtonStyle;
			this._showRecentServersButton.Style = this._serverBrowserTopTextButtonStyle;
			switch (this.Interface.App.MainMenu.ActiveServerListTab)
			{
			case AppMainMenu.ServerListTab.Internet:
				this._showInternetServersButton.Style = this._serverBrowserTopTextButtonSelectedStyle;
				break;
			case AppMainMenu.ServerListTab.Recent:
				this._showRecentServersButton.Style = this._serverBrowserTopTextButtonSelectedStyle;
				break;
			case AppMainMenu.ServerListTab.Favorites:
				this._showFavoriteServersButton.Style = this._serverBrowserTopTextButtonSelectedStyle;
				break;
			}
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x0007FC4B File Offset: 0x0007DE4B
		public void ClearTags()
		{
			this._tagsSelected.Clear();
			this.BuildSearchedTags();
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x0007FC64 File Offset: 0x0007DE64
		private void BuildSearchedTags()
		{
			this._activeTagsTableBody.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/Servers/ActiveSearchedTags.ui", out document);
			using (List<string>.Enumerator enumerator = this._tagsSelected.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string tag = enumerator.Current;
					UIFragment uifragment = document.Instantiate(this.Desktop, this._activeTagsTableBody);
					TextButton textButton = uifragment.Get<TextButton>("ActiveTag");
					textButton.Text = tag;
					textButton.Activating = delegate()
					{
						this._tagsSelected.Remove(tag);
						this.SearchByTag();
					};
				}
			}
			base.Layout(null, true);
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x0007FD3C File Offset: 0x0007DF3C
		private void ClearSearchTextInput()
		{
			this._searchTextInput.Value = "";
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x0007FD50 File Offset: 0x0007DF50
		private void SearchByTag()
		{
			this.ClearSearchTextInput();
			this.Interface.App.MainMenu.FetchAndShowPublicServers(null, this._tagsSelected.ToArray());
			this.BuildSearchedTags();
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x0007FD84 File Offset: 0x0007DF84
		private void Search()
		{
			string text = this._searchTextInput.Value.Trim();
			bool flag = text.Length > 2;
			if (flag)
			{
				this.ClearTags();
			}
			this.Interface.App.MainMenu.FetchAndShowPublicServers(text, null);
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x0007FDD4 File Offset: 0x0007DFD4
		private int GetServerIndex(Guid uuid)
		{
			for (int i = 0; i < this._servers.Length; i++)
			{
				Server server = this._servers[i];
				bool flag = server.UUID.Equals(uuid);
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x0007FE20 File Offset: 0x0007E020
		[CompilerGenerated]
		private void <Build>g__BuildColumnHeaderButton|44_0(int columnIndex, Button button, Comparison<Server> comparison)
		{
			button.Activating = delegate()
			{
				this.OnChangeSort(columnIndex, comparison);
			};
			this._columnButtonSortCarets[columnIndex] = button.Find<Group>("SortCaret");
			this._columnButtonReverseSortCarets[columnIndex] = button.Find<Group>("ReverseSortCaret");
		}

		// Token: 0x040019A6 RID: 6566
		public readonly MainMenuView MainMenuView;

		// Token: 0x040019A7 RID: 6567
		private TextButton _showInternetServersButton;

		// Token: 0x040019A8 RID: 6568
		private TextButton _showRecentServersButton;

		// Token: 0x040019A9 RID: 6569
		private TextButton _showDirectConnectButton;

		// Token: 0x040019AA RID: 6570
		private TextButton _showFavoriteServersButton;

		// Token: 0x040019AB RID: 6571
		private TextButton _showFriendServersButton;

		// Token: 0x040019AC RID: 6572
		private TextButton _serverJoinButton;

		// Token: 0x040019AD RID: 6573
		private TextButton _favoriteServerButton;

		// Token: 0x040019AE RID: 6574
		private Button.ButtonStyle _serverBrowserRowButtonStyle;

		// Token: 0x040019AF RID: 6575
		private Button.ButtonStyle _serverBrowserRowButtonSelectedStyle;

		// Token: 0x040019B0 RID: 6576
		private TextButton.TextButtonStyle _serverBrowserTopTextButtonStyle;

		// Token: 0x040019B1 RID: 6577
		private TextButton.TextButtonStyle _serverBrowserTopTextButtonSelectedStyle;

		// Token: 0x040019B2 RID: 6578
		private TextField _searchTextInput;

		// Token: 0x040019B3 RID: 6579
		private Group _directConnectPopup;

		// Token: 0x040019B4 RID: 6580
		private TextField _directConnectAddressTextInput;

		// Token: 0x040019B5 RID: 6581
		private Label _directConnectPopupStatusLabel;

		// Token: 0x040019B6 RID: 6582
		private TextButton _directConnectPopupCancelButton;

		// Token: 0x040019B7 RID: 6583
		private TextButton _directConnectPopupConnectButton;

		// Token: 0x040019B8 RID: 6584
		private Group _serversTableBody;

		// Token: 0x040019B9 RID: 6585
		private Group _activeTagsTableBody;

		// Token: 0x040019BA RID: 6586
		private Group _serversTableStatus;

		// Token: 0x040019BB RID: 6587
		private Element _serversTableLoading;

		// Token: 0x040019BC RID: 6588
		private Label _serversTableStatusText;

		// Token: 0x040019BD RID: 6589
		private List<string> _tagsSelected;

		// Token: 0x040019BE RID: 6590
		private Server[] _servers;

		// Token: 0x040019BF RID: 6591
		private Guid _selectedServer;

		// Token: 0x040019C0 RID: 6592
		private Server _selectedServerDetails;

		// Token: 0x040019C1 RID: 6593
		private Comparison<Server> _listComparison;

		// Token: 0x040019C2 RID: 6594
		private Group[] _columnButtonSortCarets;

		// Token: 0x040019C3 RID: 6595
		private Group[] _columnButtonReverseSortCarets;

		// Token: 0x040019C4 RID: 6596
		private bool _reverseSort;

		// Token: 0x040019C5 RID: 6597
		private string _serversErrorMessage;

		// Token: 0x040019C6 RID: 6598
		private Group _serverDetailsGroup;

		// Token: 0x040019C7 RID: 6599
		private Label _serverDescriptionLabel;

		// Token: 0x040019C8 RID: 6600
		private Label _ipLabel;

		// Token: 0x040019C9 RID: 6601
		private Label _languagesLabel;

		// Token: 0x040019CA RID: 6602
		private Label _uuidLabel;

		// Token: 0x040019CB RID: 6603
		private Label _onlineLabel;

		// Token: 0x040019CC RID: 6604
		private Label _favoriteServerLabel;

		// Token: 0x040019CD RID: 6605
		private Label _branchLabel;

		// Token: 0x040019CE RID: 6606
		private SoundStyle _joinServerSound;

		// Token: 0x040019CF RID: 6607
		private SoundStyle _connectionErrorSound;

		// Token: 0x040019D0 RID: 6608
		private readonly Dictionary<Guid, Button> _serverButtons = new Dictionary<Guid, Button>();
	}
}
