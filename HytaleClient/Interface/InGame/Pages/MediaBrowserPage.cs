using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using HytaleClient.Graphics;
using HytaleClient.InGame.Modules.ImmersiveScreen;
using HytaleClient.InGame.Modules.ImmersiveScreen.Data;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;
using NLog;
using SDL2;

namespace HytaleClient.Interface.InGame.Pages
{
	// Token: 0x0200088E RID: 2190
	internal class MediaBrowserPage : InterfaceComponent
	{
		// Token: 0x06003EC0 RID: 16064 RVA: 0x000AA400 File Offset: 0x000A8600
		public MediaBrowserPage(InGameView inGameView) : base(inGameView.Interface, null)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x000AA450 File Offset: 0x000A8650
		public void Build()
		{
			base.Clear();
			this.DisposeThumbnails();
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/MediaBrowserPage.ui", out document);
			this._playButtonBackground = document.ResolveNamedValue<PatchStyle>(this.Interface, "PlayButtonBackground");
			this._pauseButtonBackground = document.ResolveNamedValue<PatchStyle>(this.Interface, "PauseButtonBackground");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._mediaInfo = uifragment.Get<Group>("MediaInfo");
			this._contentTitle = uifragment.Get<Label>("ContentTitle");
			this._searchInput = uifragment.Get<CompactTextField>("SearchField");
			this._searchInput.KeyDown = new Action<SDL.SDL_Keycode>(this.OnSearchInputKeyPress);
			this._searchInput.ValueChanged = delegate()
			{
				bool flag = this._searchInput.Value.Trim() == "" && this._activeContentType == MediaBrowserPage.ContentType.Search;
				if (flag)
				{
					this.ShowPopularContent(true);
				}
			};
			this._results = uifragment.Get<Group>("Results");
			this._results.Visible = !this._isContentLoading;
			this._spinnerContainer = uifragment.Get<Group>("SpinnerContainer");
			this._spinnerContainer.Visible = this._isContentLoading;
			this._currentTimeLabel = uifragment.Get<Label>("CurrentTimeLabel");
			this._currentTimeBar = uifragment.Get<Button>("CurrentTimeBar");
			this._currentTimeBar.Activating = new Action(this.OnCurrentTimeBarActivate);
			this._currentTimeBarValue = uifragment.Get<Group>("CurrentTimeBarValue");
			this._currentTimeBarValue.Anchor.Width = new int?(0);
			this._mediaTitle = uifragment.Get<Label>("MediaTitle");
			this._mediaChannel = uifragment.Get<Label>("MediaChannel");
			this._playPauseButton = uifragment.Get<Button>("PlayPauseButton");
			this._playPauseButton.Activating = new Action(this.OnPlayPauseButtonActivate);
			uifragment.Get<Button>("StopButton").Activating = new Action(this.OnStopActivate);
			this._mediaLiveBadge = uifragment.Get<Label>("MediaLiveBadge");
			this._twitchLogo = uifragment.Get<Group>("TwitchLogo");
			this._youtubeLogo = uifragment.Get<Group>("YouTubeLogo");
			TabNavigation platformTabs = uifragment.Get<TabNavigation>("PlatformTabs");
			platformTabs.Tabs = new TabNavigation.Tab[]
			{
				new TabNavigation.Tab
				{
					Id = 1.ToString(),
					Icon = new PatchStyle("InGame/Pages/TwitchIcon.png")
				},
				new TabNavigation.Tab
				{
					Id = 0.ToString(),
					Icon = new PatchStyle("InGame/Pages/YouTubeIcon.png")
				}
			};
			platformTabs.SelectedTab = 1.ToString();
			platformTabs.SelectedTabChanged = delegate()
			{
				this.SetContentPlatform((MediaService)Enum.Parse(typeof(MediaService), platformTabs.SelectedTab));
				this.Layout(null, true);
			};
			this.SetContentPlatform(1);
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x000AA722 File Offset: 0x000A8922
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this._initializeCurrentTimeBar = true;
			this.ShowPopularContent(true);
		}

		// Token: 0x06003EC3 RID: 16067 RVA: 0x000AA74C File Offset: 0x000A894C
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x000AA768 File Offset: 0x000A8968
		protected override void AfterChildrenLayout()
		{
			bool initializeCurrentTimeBar = this._initializeCurrentTimeBar;
			if (initializeCurrentTimeBar)
			{
				this._initializeCurrentTimeBar = false;
				bool flag = this._currentMediaData != null;
				if (flag)
				{
					this._currentTimeBarValue.Anchor.Width = new int?(this._targetCurrentTimeBarWidth = this.GetCurrentTimeBarWidth());
					this._currentTimeBarValue.Layout(null, true);
				}
			}
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x000AA7D4 File Offset: 0x000A89D4
		private void Animate(float dt)
		{
			bool flag;
			if (this._targetCurrentTimeBarWidth != -1)
			{
				int targetCurrentTimeBarWidth = this._targetCurrentTimeBarWidth;
				int? width = this._currentTimeBarValue.Anchor.Width;
				flag = !(targetCurrentTimeBarWidth == width.GetValueOrDefault() & width != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this._currentTimeBarValue.Anchor.Width = new int?((int)MathHelper.Lerp((float)this._currentTimeBarValue.Anchor.Width.Value, (float)this._targetCurrentTimeBarWidth, Math.Min(dt * 20f, 1f)));
				this._currentTimeBarValue.Layout(null, true);
			}
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x000AA884 File Offset: 0x000A8A84
		private void SetContentPlatform(MediaService platform)
		{
			this._activeContentPlatform = platform;
			this._twitchLogo.Visible = (platform == 1);
			this._youtubeLogo.Visible = (platform == 0);
			this._searchInput.Value = "";
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.ShowPopularContent(false);
			}
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x000AA8DC File Offset: 0x000A8ADC
		private int GetCurrentTimeBarWidth()
		{
			float num = (this._currentMediaData.Duration > 0) ? ((float)this._currentMediaData.Position / (float)this._currentMediaData.Duration) : 0f;
			return (int)(num * (float)this.Desktop.UnscaleRound((float)this._currentTimeBar.AnchoredRectangle.Width));
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x000AA940 File Offset: 0x000A8B40
		private void OnSearchInputKeyPress(SDL.SDL_Keycode key)
		{
			bool flag = key != SDL.SDL_Keycode.SDLK_RETURN || this._searchInput.Value.Trim() == "" || this._isContentLoading;
			if (!flag)
			{
				this.PerformSearch(this._searchInput.Value, true);
			}
		}

		// Token: 0x06003EC9 RID: 16073 RVA: 0x000AA994 File Offset: 0x000A8B94
		private void OnCurrentTimeBarActivate()
		{
			bool flag = this._currentMediaData == null || this._currentMediaData.Duration <= 0;
			if (!flag)
			{
				float num = (float)(this.Desktop.MousePosition.X - this._currentTimeBar.AnchoredRectangle.X) / (float)this._currentTimeBar.AnchoredRectangle.Width;
				this.TriggerMediaAction(4, new ClientMediaData
				{
					Position = (int)(num * (float)this._currentMediaData.Duration)
				});
			}
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x000AAA1C File Offset: 0x000A8C1C
		private void OnPlayPauseButtonActivate()
		{
			bool flag = this._currentMediaData == null;
			if (!flag)
			{
				this.TriggerMediaAction(this._currentMediaData.Playing ? 1 : 0, null);
			}
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x000AAA54 File Offset: 0x000A8C54
		private void OnStopActivate()
		{
			bool flag = this._currentMediaData == null;
			if (!flag)
			{
				this.TriggerMediaAction(2, null);
			}
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x000AAA7C File Offset: 0x000A8C7C
		public void OnImmersiveViewDataUpdated(ClientMediaData data)
		{
			this._currentMediaData = data;
			ClientMediaData currentMediaData = this._currentMediaData;
			bool flag = ((currentMediaData != null) ? currentMediaData.Id : null) != null;
			if (flag)
			{
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					this._targetCurrentTimeBarWidth = this.GetCurrentTimeBarWidth();
				}
				this._playPauseButton.Background = (this._currentMediaData.Playing ? this._pauseButtonBackground : this._playButtonBackground);
				this._playPauseButton.Layout(null, true);
				this._mediaTitle.Text = this._currentMediaData.Platform.ToString() + ": " + this._currentMediaData.Title;
				this._mediaTitle.Layout(null, true);
				this._mediaChannel.Text = this._currentMediaData.ChannelName;
				this._mediaChannel.Layout(null, true);
				TimeSpan timeSpan = TimeSpan.FromSeconds((double)this._currentMediaData.Duration);
				TimeSpan timeSpan2 = TimeSpan.FromSeconds((double)this._currentMediaData.Position);
				this._currentTimeLabel.Text = string.Format("{0:m\\:ss} / {1:m\\:ss}", timeSpan2, timeSpan);
				this._currentTimeLabel.Layout(null, true);
				this._mediaLiveBadge.Visible = this._currentMediaData.Stream;
				this._mediaLiveBadge.Parent.Layout(null, true);
				bool flag2 = !this._mediaInfo.Visible;
				if (flag2)
				{
					this._mediaInfo.Visible = true;
					base.Layout(null, true);
				}
			}
			else
			{
				this._currentTimeBarValue.Anchor.Width = new int?(0);
				this._currentTimeBarValue.Layout(null, true);
				this._playPauseButton.Background = this._playButtonBackground;
				this._playPauseButton.Layout(null, true);
				this._mediaTitle.Text = "";
				this._mediaTitle.Layout(null, true);
				this._currentTimeLabel.Text = "0:00 / 0:00";
				this._currentTimeLabel.Layout(null, true);
				this._mediaLiveBadge.Visible = false;
				bool visible = this._mediaInfo.Visible;
				if (visible)
				{
					this._mediaInfo.Visible = false;
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x000AAD30 File Offset: 0x000A8F30
		private void ShowPopularContent(bool doLayout)
		{
			this._activeContentType = MediaBrowserPage.ContentType.Popular;
			this._contentTitle.Text = this.Interface.GetText((this._activeContentPlatform == 1) ? "ui.mediaBrowser.popularStreams" : "ui.mediaBrowser.popularVideos", null, true);
			int num = ++MediaBrowserPage.NextNonce;
			this._contentLoadNonce = num.ToString();
			this._spinnerContainer.Visible = true;
			this._results.Clear();
			this._results.Visible = false;
			this.DisposeThumbnails();
			if (doLayout)
			{
				base.Layout(null, true);
			}
			ImmersiveScreenModule.MediaDataEvent e = new ImmersiveScreenModule.MediaDataEvent
			{
				MaxResults = 25,
				Nonce = this._contentLoadNonce
			};
			bool flag = this._activeContentPlatform == 1;
			if (flag)
			{
				this._inGameView.InGame.Instance.ImmersiveScreenModule.GetMostPopularTwitchStreams(e);
			}
			else
			{
				this._inGameView.InGame.Instance.ImmersiveScreenModule.GetMostPopularYouTubeVideos(e);
			}
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x000AAE34 File Offset: 0x000A9034
		private void PerformSearch(string query, bool doLayout)
		{
			this._activeContentType = MediaBrowserPage.ContentType.Search;
			this._contentTitle.Text = this.Interface.GetText("ui.mediaBrowser.search", new Dictionary<string, string>
			{
				{
					"query",
					query
				}
			}, true);
			this._isContentLoading = true;
			int num = ++MediaBrowserPage.NextNonce;
			this._contentLoadNonce = num.ToString();
			this._spinnerContainer.Visible = true;
			this._results.Clear();
			this._results.Visible = false;
			this.DisposeThumbnails();
			if (doLayout)
			{
				base.Layout(null, true);
			}
			ImmersiveScreenModule.MediaDataEvent e = new ImmersiveScreenModule.MediaDataEvent
			{
				MaxResults = 50,
				Query = this._searchInput.Value.Trim(),
				Nonce = this._contentLoadNonce
			};
			bool flag = this._activeContentPlatform == 1;
			if (flag)
			{
				this._inGameView.InGame.Instance.ImmersiveScreenModule.SearchTwitch(e);
			}
			else
			{
				this._inGameView.InGame.Instance.ImmersiveScreenModule.SearchYouTube(e);
			}
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x000AAF58 File Offset: 0x000A9158
		private void DisposeThumbnails()
		{
			foreach (ExternalTextureLoader externalTextureLoader in this._thumbnailTextureLoaders)
			{
				externalTextureLoader.Cancel();
			}
			this._thumbnailTextureLoaders.Clear();
			foreach (TextureArea textureArea in this._thumbnailTextureAreas)
			{
				textureArea.Texture.Dispose();
			}
			this._thumbnailTextureAreas.Clear();
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x000AB010 File Offset: 0x000A9210
		private void BuildStreamList(List<ClientMediaData> items)
		{
			Document document;
			this.Interface.TryGetDocument("InGame/Pages/MediaBrowserListingEntry.ui", out document);
			using (List<ClientMediaData>.Enumerator enumerator = items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClientMediaData media = enumerator.Current;
					try
					{
						UIFragment uifragment = document.Instantiate(this.Desktop, null);
						uifragment.Get<Label>("Title").Text = media.Title;
						uifragment.Get<Label>("ChannelName").Text = media.ChannelName;
						uifragment.Get<Label>("Duration").Visible = false;
						uifragment.Get<Label>("UploadDate").Visible = false;
						uifragment.Get<Label>("ViewCount").Text = this.Interface.FormatNumber(media.ViewCount);
						uifragment.Get<Label>("LiveBadge").Visible = media.Stream;
						bool flag = media.Duration > 0;
						if (flag)
						{
							TimeSpan timeSpan = TimeSpan.FromSeconds((double)media.Duration);
							uifragment.Get<Label>("Duration").Visible = true;
							uifragment.Get<Label>("Duration").Text = string.Format("{0:m\\:ss}", timeSpan);
						}
						bool flag2 = media.GameTitle != null;
						if (flag2)
						{
							uifragment.Get<Label>("GameTitle").Visible = true;
							uifragment.Get<Label>("GameTitle").Text = media.GameTitle;
						}
						uifragment.Get<Button>("Button").Activating = delegate()
						{
							this.TriggerMediaAction(3, media);
						};
						foreach (Element child in uifragment.RootElements)
						{
							this._results.Add(child, -1);
						}
					}
					catch (Exception exception)
					{
						MediaBrowserPage.Logger.Error(exception, "Failed to add element for video.");
						MediaBrowserPage.Logger.Error<ClientMediaData>(media);
					}
				}
			}
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x000AB29C File Offset: 0x000A949C
		public void OnSearchError(string nonce, string error)
		{
			bool flag = this._contentLoadNonce != nonce;
			if (!flag)
			{
				this._contentLoadNonce = null;
				this._isContentLoading = false;
				MediaBrowserPage.Logger.Error("Failed to search content: {error}", error);
				this._results.Clear();
				new Label(this.Desktop, this._results).Text = "Failed to perform search.";
				this._results.Layout(null, true);
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x000AB31C File Offset: 0x000A951C
		public void OnGetPopularContentError(string nonce, string error)
		{
			bool flag = this._contentLoadNonce != nonce;
			if (!flag)
			{
				this._contentLoadNonce = null;
				this._isContentLoading = false;
				MediaBrowserPage.Logger.Error("Failed to fetch content: {error}", error);
				this._results.Clear();
				new Label(this.Desktop, this._results).Text = "Failed to fetch content.";
				this._results.Layout(null, true);
			}
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x000AB399 File Offset: 0x000A9599
		private void TriggerMediaAction(ImmersiveViewMediaAction action, ClientMediaData video = null)
		{
			this._inGameView.InGame.Instance.ImmersiveScreenModule.TriggerMediaAction(action, video);
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x000AB3BC File Offset: 0x000A95BC
		public void ResetState()
		{
			this._activeContentType = MediaBrowserPage.ContentType.Popular;
			this._searchInput.Value = "";
			this._mediaTitle.Text = "";
			this._mediaLiveBadge.Visible = false;
			this._currentTimeBarValue.Anchor.Width = new int?(0);
			this._playPauseButton.Background = this._playButtonBackground;
			this._results.Clear();
			this._results.Visible = true;
			this._spinnerContainer.Visible = false;
			this._isContentLoading = false;
			this._contentLoadNonce = null;
			this.DisposeThumbnails();
		}

		// Token: 0x06003ED5 RID: 16085 RVA: 0x000AB464 File Offset: 0x000A9664
		public void OnGetTwitchPopularStreams(string nonce, string json)
		{
			bool flag = this._contentLoadNonce != nonce;
			if (!flag)
			{
				this._contentLoadNonce = null;
				this._spinnerContainer.Visible = false;
				this._results.Visible = true;
				JObject jobject = JObject.Parse(json);
				this._isContentLoading = false;
				JArray jarray = Extensions.Value<JArray>(jobject["data"]);
				List<ClientMediaData> list = new List<ClientMediaData>();
				foreach (JToken jtoken in jarray)
				{
					try
					{
						List<ClientMediaData> list2 = list;
						ClientMediaData clientMediaData = new ClientMediaData();
						clientMediaData.Id = Extensions.Value<string>(jtoken["id"]);
						clientMediaData.Title = Extensions.Value<string>(jtoken["title"]);
						clientMediaData.Duration = 0;
						clientMediaData.ChannelId = Extensions.Value<string>(jtoken["user_id"]);
						clientMediaData.ChannelName = Extensions.Value<string>(jtoken["user_name"]);
						JToken jtoken2 = jtoken["game_title"];
						clientMediaData.GameTitle = ((jtoken2 != null) ? Extensions.Value<string>(jtoken2) : null);
						clientMediaData.PublicationDate = "";
						clientMediaData.Position = 0;
						clientMediaData.Stream = true;
						clientMediaData.Playing = true;
						clientMediaData.ViewCount = Extensions.Value<int>(jtoken["viewer_count"]);
						clientMediaData.Platform = 1;
						clientMediaData.Thumbnail = new ClientMediaData.ClientThumbnails
						{
							Small = Extensions.Value<string>(jtoken["thumbnail_url"]).Replace("{width}", "200").Replace("{height}", "112"),
							Normal = Extensions.Value<string>(jtoken["thumbnail_url"]).Replace("{width}", "400").Replace("{height}", "225")
						};
						list2.Add(clientMediaData);
					}
					catch (Exception exception)
					{
						MediaBrowserPage.Logger.Error(exception, "Failed to add parse stream");
						MediaBrowserPage.Logger.Error<JToken>(jtoken);
					}
				}
				this.BuildStreamList(list);
				base.Layout(null, true);
			}
		}

		// Token: 0x06003ED6 RID: 16086 RVA: 0x000AB6B8 File Offset: 0x000A98B8
		public void OnTwitchSearchResults(string nonce, string json)
		{
			bool flag = this._contentLoadNonce != nonce;
			if (!flag)
			{
				this._contentLoadNonce = null;
				this._spinnerContainer.Visible = false;
				this._results.Visible = true;
				JObject jobject = JObject.Parse(json);
				this._isContentLoading = false;
				JArray jarray = Extensions.Value<JArray>(jobject["streams"]);
				List<ClientMediaData> list = new List<ClientMediaData>();
				foreach (JToken jtoken in jarray)
				{
					try
					{
						List<ClientMediaData> list2 = list;
						ClientMediaData clientMediaData = new ClientMediaData();
						clientMediaData.Id = Extensions.Value<string>(jtoken["_id"]);
						clientMediaData.Title = Extensions.Value<string>(jtoken["channel"]["status"]);
						clientMediaData.Duration = 0;
						clientMediaData.ChannelId = Extensions.Value<string>(jtoken["channel"]["_id"]);
						clientMediaData.ChannelName = Extensions.Value<string>(jtoken["channel"]["name"]);
						clientMediaData.PublicationDate = "";
						clientMediaData.Position = 0;
						clientMediaData.Stream = true;
						clientMediaData.Playing = true;
						JToken jtoken2 = jtoken["game"];
						clientMediaData.GameTitle = ((jtoken2 != null) ? Extensions.Value<string>(jtoken2) : null);
						clientMediaData.ViewCount = Extensions.Value<int>(jtoken["viewers"]);
						clientMediaData.Platform = 1;
						clientMediaData.Thumbnail = new ClientMediaData.ClientThumbnails
						{
							Small = Extensions.Value<string>(jtoken["preview"]["small"]),
							Normal = Extensions.Value<string>(jtoken["preview"]["medium"])
						};
						list2.Add(clientMediaData);
					}
					catch (Exception exception)
					{
						MediaBrowserPage.Logger.Error(exception, "Failed to add parse stream");
						MediaBrowserPage.Logger.Error<JToken>(jtoken);
					}
				}
				this.BuildStreamList(list);
				base.Layout(null, true);
			}
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x000AB904 File Offset: 0x000A9B04
		public void OnYouTubeSearchResults(string nonce, string json)
		{
			bool flag = this._contentLoadNonce != nonce;
			if (!flag)
			{
				this._contentLoadNonce = null;
				this._spinnerContainer.Visible = false;
				this._results.Visible = true;
				JObject jobject = JObject.Parse(json);
				this._isContentLoading = false;
				List<ClientMediaData> list = new List<ClientMediaData>();
				this.AddYouTubeVideos(Extensions.Value<JArray>(jobject["items"]), list);
				this.BuildStreamList(list);
				base.Layout(null, true);
			}
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x000AB98C File Offset: 0x000A9B8C
		public void OnGetYouTubePopularVideos(string nonce, string json)
		{
			bool flag = this._contentLoadNonce != nonce;
			if (!flag)
			{
				this._contentLoadNonce = null;
				this._spinnerContainer.Visible = false;
				this._results.Visible = true;
				JObject jobject = JObject.Parse(json);
				this._isContentLoading = false;
				List<ClientMediaData> list = new List<ClientMediaData>();
				this.AddYouTubeVideos(Extensions.Value<JArray>(jobject["items"]), list);
				this.BuildStreamList(list);
				base.Layout(null, true);
			}
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x000ABA14 File Offset: 0x000A9C14
		private void AddYouTubeVideos(JArray videos, List<ClientMediaData> list)
		{
			foreach (JToken jtoken in videos)
			{
				try
				{
					TimeSpan timeSpan = XmlConvert.ToTimeSpan(Extensions.Value<string>(jtoken["contentDetails"]["duration"]));
					list.Add(new ClientMediaData
					{
						Id = Extensions.Value<string>(jtoken["id"]),
						Title = WebUtility.HtmlDecode(Extensions.Value<string>(jtoken["snippet"]["title"])),
						Duration = (int)timeSpan.TotalMilliseconds / 1000,
						ChannelId = Extensions.Value<string>(jtoken["snippet"]["channelId"]),
						ChannelName = Extensions.Value<string>(jtoken["snippet"]["channelTitle"]),
						PublicationDate = Extensions.Value<string>(jtoken["snippet"]["publishedAt"]),
						Position = 0,
						Stream = (Extensions.Value<string>(jtoken["snippet"]["liveBroadcastContent"]) == "live" || Extensions.Value<string>(jtoken["snippet"]["liveBroadcastContent"]) == "upcoming"),
						Playing = true,
						ViewCount = Extensions.Value<int>(jtoken["statistics"]["viewCount"]),
						Platform = 0,
						Thumbnail = new ClientMediaData.ClientThumbnails
						{
							Small = Extensions.Value<string>(jtoken["snippet"]["thumbnails"]["default"]["url"]),
							Normal = Extensions.Value<string>(jtoken["snippet"]["thumbnails"]["medium"]["url"])
						}
					});
				}
				catch (Exception exception)
				{
					MediaBrowserPage.Logger.Error(exception, "Failed to add parse stream");
					MediaBrowserPage.Logger.Error<JToken>(jtoken);
				}
			}
		}

		// Token: 0x04001D91 RID: 7569
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001D92 RID: 7570
		private static int NextNonce;

		// Token: 0x04001D93 RID: 7571
		private readonly InGameView _inGameView;

		// Token: 0x04001D94 RID: 7572
		private Label _contentTitle;

		// Token: 0x04001D95 RID: 7573
		private Group _mediaInfo;

		// Token: 0x04001D96 RID: 7574
		private Label _mediaTitle;

		// Token: 0x04001D97 RID: 7575
		private Label _mediaChannel;

		// Token: 0x04001D98 RID: 7576
		private Label _mediaLiveBadge;

		// Token: 0x04001D99 RID: 7577
		private CompactTextField _searchInput;

		// Token: 0x04001D9A RID: 7578
		private Group _results;

		// Token: 0x04001D9B RID: 7579
		private Group _spinnerContainer;

		// Token: 0x04001D9C RID: 7580
		private Button _currentTimeBar;

		// Token: 0x04001D9D RID: 7581
		private Group _currentTimeBarValue;

		// Token: 0x04001D9E RID: 7582
		private Button _playPauseButton;

		// Token: 0x04001D9F RID: 7583
		private Label _currentTimeLabel;

		// Token: 0x04001DA0 RID: 7584
		private Group _youtubeLogo;

		// Token: 0x04001DA1 RID: 7585
		private Group _twitchLogo;

		// Token: 0x04001DA2 RID: 7586
		private PatchStyle _playButtonBackground;

		// Token: 0x04001DA3 RID: 7587
		private PatchStyle _pauseButtonBackground;

		// Token: 0x04001DA4 RID: 7588
		private ClientMediaData _currentMediaData;

		// Token: 0x04001DA5 RID: 7589
		private int _targetCurrentTimeBarWidth = -1;

		// Token: 0x04001DA6 RID: 7590
		private string _contentLoadNonce;

		// Token: 0x04001DA7 RID: 7591
		private bool _isContentLoading;

		// Token: 0x04001DA8 RID: 7592
		private bool _initializeCurrentTimeBar;

		// Token: 0x04001DA9 RID: 7593
		private MediaBrowserPage.ContentType _activeContentType = MediaBrowserPage.ContentType.Popular;

		// Token: 0x04001DAA RID: 7594
		private MediaService _activeContentPlatform = 1;

		// Token: 0x04001DAB RID: 7595
		private readonly List<ExternalTextureLoader> _thumbnailTextureLoaders = new List<ExternalTextureLoader>();

		// Token: 0x04001DAC RID: 7596
		private List<TextureArea> _thumbnailTextureAreas = new List<TextureArea>();

		// Token: 0x02000D5E RID: 3422
		private enum ContentType
		{
			// Token: 0x040041B6 RID: 16822
			Popular,
			// Token: 0x040041B7 RID: 16823
			Search
		}
	}
}
