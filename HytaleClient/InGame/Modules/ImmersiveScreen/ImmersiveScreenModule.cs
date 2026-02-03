using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using HytaleClient.Graphics;
using HytaleClient.InGame.Modules.ImmersiveScreen.Data;
using HytaleClient.InGame.Modules.ImmersiveScreen.Screens;
using HytaleClient.Interface.CoherentUI;
using HytaleClient.Math;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.ImmersiveScreen
{
	// Token: 0x0200093C RID: 2364
	internal class ImmersiveScreenModule : Module
	{
		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x060048BD RID: 18621 RVA: 0x00119984 File Offset: 0x00117B84
		// (set) Token: 0x060048BE RID: 18622 RVA: 0x0011998C File Offset: 0x00117B8C
		public ImmersiveWebScreen ActiveWebScreen { get; private set; }

		// Token: 0x060048BF RID: 18623 RVA: 0x00119995 File Offset: 0x00117B95
		public int GetScreenCount()
		{
			return this._screens.Count;
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x001199A4 File Offset: 0x00117BA4
		public ImmersiveScreenModule(GameInstance gameInstance) : base(gameInstance)
		{
			GraphicsDevice graphics = this._gameInstance.Engine.Graphics;
			GLFunctions gl = graphics.GL;
			this.CoUIQuadRenderer = new QuadRenderer(graphics, graphics.GPUProgramStore.BasicProgram.AttribPosition, graphics.GPUProgramStore.BasicProgram.AttribTexCoords);
			this.CoUIWebView = new WebView(this._gameInstance.Engine, this._gameInstance.App.CoUIManager, "blank", 360, 360, 1f);
			this.CoUIWebViewTexture = gl.GenTexture();
			gl.BindTexture(GL.TEXTURE_2D, this.CoUIWebViewTexture);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, 9728);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_S, 33071);
			gl.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_WRAP_T, 33071);
			this.CoUIWebView.RegisterForEvent("immersiveScreens.mediaData.request", this, delegate()
			{
				ImmersiveWebScreen activeWebScreen = this.ActiveWebScreen;
				if (activeWebScreen != null)
				{
					activeWebScreen.SendMediaData();
				}
			});
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x00119AEC File Offset: 0x00117CEC
		protected override void DoDispose()
		{
			this.CoUIWebView.UnregisterFromEvent("immersiveScreens.mediaData.request");
			YouTubeDataRequest youTubeSearchRequest = this._youTubeSearchRequest;
			if (youTubeSearchRequest != null)
			{
				youTubeSearchRequest.Dispose();
			}
			YouTubeDataRequest youTubeMostPopularVideosRequest = this._youTubeMostPopularVideosRequest;
			if (youTubeMostPopularVideosRequest != null)
			{
				youTubeMostPopularVideosRequest.Dispose();
			}
			TwitchDataRequest twitchMostPopularVideosRequest = this._twitchMostPopularVideosRequest;
			if (twitchMostPopularVideosRequest != null)
			{
				twitchMostPopularVideosRequest.Dispose();
			}
			TwitchDataRequest twitchSearchRequest = this._twitchSearchRequest;
			if (twitchSearchRequest != null)
			{
				twitchSearchRequest.Dispose();
			}
			for (int i = 0; i < this._screens.Count; i++)
			{
				this._screens[i].Dispose();
			}
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			gl.DeleteTexture(this.CoUIWebViewTexture);
			this._gameInstance.App.CoUIManager.RunInThread(delegate
			{
				this.CoUIWebView.Destroy();
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance.Engine, delegate
				{
					this.CoUIWebView.Dispose();
				}, false, false);
			});
			this.CoUIQuadRenderer.Dispose();
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x00119BD0 File Offset: 0x00117DD0
		public void Update(float deltaTime)
		{
			float num = float.PositiveInfinity;
			ImmersiveWebScreen immersiveWebScreen = null;
			foreach (BaseImmersiveScreen baseImmersiveScreen in this._screens)
			{
				baseImmersiveScreen.Update(deltaTime);
				float num2 = Vector3.Distance(baseImmersiveScreen.GetOffsetPosition(), this._gameInstance.LocalPlayer.Position);
				bool flag = num2 <= baseImmersiveScreen.MaxVisibilityDistance;
				if (flag)
				{
					bool flag2 = baseImmersiveScreen is ImmersiveWebScreen && num2 < num;
					if (flag2)
					{
						num = num2;
						immersiveWebScreen = (ImmersiveWebScreen)baseImmersiveScreen;
					}
				}
			}
			bool flag3 = this.ActiveWebScreen != immersiveWebScreen;
			if (flag3)
			{
				bool flag4 = this.ActiveWebScreen != null && (immersiveWebScreen == null || this.ActiveWebScreen.BlockPosition != immersiveWebScreen.BlockPosition);
				if (flag4)
				{
					this.ActiveWebScreen.OnDeactivate();
				}
				this.ActiveWebScreen = immersiveWebScreen;
				bool flag5 = this.ActiveWebScreen != null;
				if (flag5)
				{
					float scale = this._gameInstance.App.Interface.Desktop.Scale;
					this.CoUIWebView.Resize((int)(this.ActiveWebScreen.ScreenSizeInPixels.X * scale), (int)(this.ActiveWebScreen.ScreenSizeInPixels.Y * scale), scale);
					string url = this.ActiveWebScreen.GetUrl();
					bool flag6 = url != null;
					if (flag6)
					{
						this.CoUIWebView.LoadURL(url);
					}
					this.ActiveWebScreen.OnActivate();
				}
				else
				{
					this.CoUIWebView.SetVolume(0.0);
					this.CoUIWebView.LoadURL("blank");
				}
			}
			bool flag7 = this.ActiveWebScreen != null;
			if (flag7)
			{
				float num3 = MathHelper.Clamp(1f - num / this.ActiveWebScreen.MaxSoundDistance, 0f, 1f);
				this.CoUIWebView.SetVolume((double)num3);
			}
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x00119DE8 File Offset: 0x00117FE8
		public bool NeedsDrawing()
		{
			foreach (BaseImmersiveScreen baseImmersiveScreen in this._screens)
			{
				bool flag = baseImmersiveScreen.NeedsDrawing();
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x00119E4C File Offset: 0x0011804C
		public void PrepareForDraw(ref Matrix viewProjectionMatrix)
		{
			bool flag = !this.NeedsDrawing();
			if (flag)
			{
				throw new Exception("PrepareForDraw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			foreach (BaseImmersiveScreen baseImmersiveScreen in this._screens)
			{
				bool flag2 = baseImmersiveScreen.NeedsDrawing();
				if (flag2)
				{
					baseImmersiveScreen.PrepareForDraw(ref viewProjectionMatrix);
				}
			}
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x00119EC8 File Offset: 0x001180C8
		public void Draw()
		{
			bool flag = !this.NeedsDrawing();
			if (flag)
			{
				throw new Exception("Draw called when it was not required. Please check with NeedsDrawing() first before calling this.");
			}
			foreach (BaseImmersiveScreen baseImmersiveScreen in this._screens)
			{
				bool flag2 = baseImmersiveScreen.NeedsDrawing();
				if (flag2)
				{
					baseImmersiveScreen.Draw();
				}
			}
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x00119F44 File Offset: 0x00118144
		public void HandleUpdatePacket(UpdateImmersiveView packet)
		{
			bool flag = packet.View == null;
			if (!flag)
			{
				Vector3 vector = new Vector3((float)packet.X, (float)packet.Y, (float)packet.Z);
				BaseImmersiveScreen screenAtBlockPosition = this.GetScreenAtBlockPosition(vector);
				bool flag2 = screenAtBlockPosition != null;
				if (flag2)
				{
					bool flag3 = packet.UpdateType_ != 2;
					if (flag3)
					{
						ImmersiveViewType viewType = packet.View.Screen.ViewType;
						ImmersiveViewType immersiveViewType = viewType;
						if (immersiveViewType > 1)
						{
							if (immersiveViewType == 2)
							{
								ImmersiveWebScreen immersiveWebScreen = screenAtBlockPosition as ImmersiveWebScreen;
								bool flag4 = immersiveWebScreen != null;
								if (flag4)
								{
									immersiveWebScreen.SetViewData(packet.View);
									return;
								}
							}
						}
						else
						{
							ImmersiveImageScreen immersiveImageScreen = screenAtBlockPosition as ImmersiveImageScreen;
							bool flag5 = immersiveImageScreen != null;
							if (flag5)
							{
								immersiveImageScreen.SetViewData(packet.View);
								return;
							}
						}
					}
					screenAtBlockPosition.Dispose();
					this._screens.Remove(screenAtBlockPosition);
				}
				bool flag6 = packet.UpdateType_ != 2;
				if (flag6)
				{
					try
					{
						ImmersiveViewType viewType2 = packet.View.Screen.ViewType;
						ImmersiveViewType immersiveViewType2 = viewType2;
						if (immersiveViewType2 > 1)
						{
							if (immersiveViewType2 == 2)
							{
								ImmersiveWebScreen immersiveWebScreen2 = new ImmersiveWebScreen(this._gameInstance, vector, packet.View.Screen);
								immersiveWebScreen2.SetViewData(packet.View);
								this._screens.Add(immersiveWebScreen2);
							}
						}
						else
						{
							ImmersiveImageScreen immersiveImageScreen2 = new ImmersiveImageScreen(this._gameInstance, vector, packet.View.Screen);
							immersiveImageScreen2.SetViewData(packet.View);
							this._screens.Add(immersiveImageScreen2);
						}
					}
					catch (Exception arg)
					{
						this._gameInstance.App.DevTools.Error(string.Format("Error creating {0} view at {1} - {2}", packet.View.Screen.ViewType, vector, arg));
					}
				}
			}
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x0011A138 File Offset: 0x00118338
		private BaseImmersiveScreen GetScreenAtBlockPosition(Vector3 blockPosition)
		{
			for (int i = 0; i < this._screens.Count; i++)
			{
				bool flag = this._screens[i].BlockPosition == blockPosition;
				if (flag)
				{
					return this._screens[i];
				}
			}
			return null;
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x0011A194 File Offset: 0x00118394
		public void SearchYouTube(ImmersiveScreenModule.MediaDataEvent e)
		{
			YouTubeDataRequest youTubeSearchRequest = this._youTubeSearchRequest;
			if (youTubeSearchRequest != null)
			{
				youTubeSearchRequest.Dispose();
			}
			this._youTubeSearchRequest = new YouTubeDataRequest(delegate(JObject data, string err)
			{
				this._gameInstance.Engine.RunOnMainThread(this, delegate
				{
					this._youTubeSearchRequest.Dispose();
					this._youTubeSearchRequest = null;
					bool flag2 = err != null;
					if (flag2)
					{
						this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnSearchError(e.Nonce, err);
					}
					else
					{
						this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnYouTubeSearchResults(e.Nonce, data.ToString());
					}
				}, false, false);
			});
			string text = ImmersiveScreenModule.<SearchYouTube>g__GetYouTubeVideoId|21_1(e.Query);
			bool flag = !string.IsNullOrEmpty(text);
			if (flag)
			{
				this._youTubeSearchRequest.GetVideo(text);
			}
			else
			{
				this._youTubeSearchRequest.SearchVideos(e.Query, e.MaxResults, e.PageToken);
			}
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x0011A23C File Offset: 0x0011843C
		public void GetMostPopularYouTubeVideos(ImmersiveScreenModule.MediaDataEvent e)
		{
			YouTubeDataRequest youTubeMostPopularVideosRequest = this._youTubeMostPopularVideosRequest;
			if (youTubeMostPopularVideosRequest != null)
			{
				youTubeMostPopularVideosRequest.Dispose();
			}
			this._youTubeMostPopularVideosRequest = new YouTubeDataRequest(delegate(JObject data, string err)
			{
				this._gameInstance.Engine.RunOnMainThread(this, delegate
				{
					this._youTubeMostPopularVideosRequest.Dispose();
					this._youTubeMostPopularVideosRequest = null;
					bool flag = err != null;
					if (flag)
					{
						this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnGetPopularContentError(e.Nonce, err);
					}
					else
					{
						this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnGetYouTubePopularVideos(e.Nonce, data.ToString());
					}
				}, false, false);
			});
			this._youTubeMostPopularVideosRequest.OnGetPopularVideos(e.MaxResults);
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x0011A2A0 File Offset: 0x001184A0
		public void GetMostPopularTwitchStreams(ImmersiveScreenModule.MediaDataEvent e)
		{
			TwitchDataRequest twitchMostPopularVideosRequest = this._twitchMostPopularVideosRequest;
			if (twitchMostPopularVideosRequest != null)
			{
				twitchMostPopularVideosRequest.Dispose();
			}
			this._twitchMostPopularVideosRequest = new TwitchDataRequest(delegate(JObject data, string err)
			{
				this._gameInstance.Engine.RunOnMainThread(this, delegate
				{
					this._twitchMostPopularVideosRequest.Dispose();
					this._twitchMostPopularVideosRequest = null;
					bool flag = err != null;
					if (flag)
					{
						this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnGetPopularContentError(e.Nonce, err);
					}
					else
					{
						this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnGetTwitchPopularStreams(e.Nonce, data.ToString());
					}
				}, false, false);
			});
			this._twitchMostPopularVideosRequest.GetPopularStreams(e.MaxResults);
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x0011A304 File Offset: 0x00118504
		public void SearchTwitch(ImmersiveScreenModule.MediaDataEvent e)
		{
			TwitchDataRequest twitchSearchRequest = this._twitchSearchRequest;
			if (twitchSearchRequest != null)
			{
				twitchSearchRequest.Dispose();
			}
			this._twitchSearchRequest = new TwitchDataRequest(delegate(JObject data, string err)
			{
				this._gameInstance.Engine.RunOnMainThread(this, delegate
				{
					this._twitchSearchRequest.Dispose();
					this._twitchSearchRequest = null;
					JObject data = data;
					Trace.WriteLine((data != null) ? data.ToString() : null);
					bool flag = err != null;
					if (flag)
					{
						this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnSearchError(e.Nonce, err);
					}
					else
					{
						this._gameInstance.App.Interface.InGameView.MediaBrowserPage.OnTwitchSearchResults(e.Nonce, data.ToString());
					}
				}, false, false);
			});
			this._twitchSearchRequest.SearchStreams(e.Query, e.MaxResults, e.Page);
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x0011A37C File Offset: 0x0011857C
		public void TriggerMediaAction(ImmersiveViewMediaAction action, ClientMediaData media)
		{
			bool flag = this._gameInstance.ImmersiveScreenModule.ActiveWebScreen == null;
			if (!flag)
			{
				Vector3 blockPosition = this._gameInstance.ImmersiveScreenModule.ActiveWebScreen.BlockPosition;
				this._gameInstance.Connection.SendPacket(new ImmersiveViewUpdateMedia(new BlockPosition((int)blockPosition.X, (int)blockPosition.Y, (int)blockPosition.Z), action, (media != null) ? media.ToPacket() : null));
			}
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x0011A454 File Offset: 0x00118654
		[CompilerGenerated]
		internal static string <SearchYouTube>g__GetYouTubeVideoId|21_1(string input)
		{
			Regex regex = new Regex("(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+", 8);
			foreach (object obj in regex.Matches(input))
			{
				Match match = (Match)obj;
				using (IEnumerator<Group> enumerator2 = Enumerable.Where<Group>(Enumerable.Cast<Group>(match.Groups), (Group groupdata) => !groupdata.ToString().StartsWith("http://") && !groupdata.ToString().StartsWith("https://") && !groupdata.ToString().StartsWith("youtu") && !groupdata.ToString().StartsWith("www.")).GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						Group group = enumerator2.Current;
						return group.ToString();
					}
				}
			}
			return "";
		}

		// Token: 0x040024CE RID: 9422
		private readonly List<BaseImmersiveScreen> _screens = new List<BaseImmersiveScreen>();

		// Token: 0x040024CF RID: 9423
		public readonly QuadRenderer CoUIQuadRenderer;

		// Token: 0x040024D0 RID: 9424
		public readonly WebView CoUIWebView;

		// Token: 0x040024D1 RID: 9425
		public readonly GLTexture CoUIWebViewTexture;

		// Token: 0x040024D2 RID: 9426
		private YouTubeDataRequest _youTubeSearchRequest;

		// Token: 0x040024D3 RID: 9427
		private YouTubeDataRequest _youTubeMostPopularVideosRequest;

		// Token: 0x040024D4 RID: 9428
		private TwitchDataRequest _twitchMostPopularVideosRequest;

		// Token: 0x040024D5 RID: 9429
		private TwitchDataRequest _twitchSearchRequest;

		// Token: 0x02000E1B RID: 3611
		public class MediaDataEvent
		{
			// Token: 0x0400452B RID: 17707
			public int MaxResults;

			// Token: 0x0400452C RID: 17708
			public string Query;

			// Token: 0x0400452D RID: 17709
			public string PageToken;

			// Token: 0x0400452E RID: 17710
			public int Page;

			// Token: 0x0400452F RID: 17711
			public string Id;

			// Token: 0x04004530 RID: 17712
			public string Nonce;
		}
	}
}
