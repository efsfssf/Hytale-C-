using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.Application.Services;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using NLog;

namespace HytaleClient.Interface.MainMenu.Pages
{
	// Token: 0x0200081E RID: 2078
	internal class MinigamesPage : InterfaceComponent
	{
		// Token: 0x060039B1 RID: 14769 RVA: 0x0007E575 File Offset: 0x0007C775
		public MinigamesPage(MainMenuView mainMenuView) : base(mainMenuView.Interface, null)
		{
			this.MainMenuView = mainMenuView;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x0007E5A4 File Offset: 0x0007C7A4
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/Minigames/MinigamesPage.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._container = uifragment.Get<Group>("MinigamesContainer");
			this._framesPerRow = document.ResolveNamedValue<int>(this.Desktop.Provider, "FramesPerRow");
			this._frameWidth = document.ResolveNamedValue<int>(this.Desktop.Provider, "FrameWidth");
			this._frameHeight = document.ResolveNamedValue<int>(this.Desktop.Provider, "FrameHeight");
			this._frameSpacing = document.ResolveNamedValue<int>(this.Desktop.Provider, "FrameSpacing");
			this.BuildGamesList();
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x0007E664 File Offset: 0x0007C864
		public void DisposeGameTextures()
		{
			foreach (TextureArea textureArea in this._downloadedGameTextureAreas.Values)
			{
				textureArea.Texture.Dispose();
			}
			this._downloadedGameTextureAreas.Clear();
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x0007E6D0 File Offset: 0x0007C8D0
		private void Queue(string joinKey)
		{
			this.Interface.App.MainMenu.QueueForMinigame(joinKey);
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x0007E6EC File Offset: 0x0007C8EC
		private void BuildGamesList()
		{
			bool flag = !base.IsMounted;
			if (!flag)
			{
				List<ClientGameWrapper> games = this.Interface.App.HytaleServices.Games;
				bool flag2 = games == null;
				if (!flag2)
				{
					this._container.Clear();
					Document document;
					this.Interface.TryGetDocument("MainMenu/Minigames/MinigameButton.ui", out document);
					this._nameLabelHoverOffset = document.ResolveNamedValue<int>(this.Desktop.Provider, "NameLabelHoverOffset");
					int num = 0;
					using (List<ClientGameWrapper>.Enumerator enumerator = games.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MinigamesPage.<>c__DisplayClass14_0 CS$<>8__locals1 = new MinigamesPage.<>c__DisplayClass14_0();
							CS$<>8__locals1.<>4__this = this;
							CS$<>8__locals1.game = enumerator.Current;
							bool flag3 = !CS$<>8__locals1.game.Display;
							if (!flag3)
							{
								Group group = this.<BuildGamesList>g__MakeButtonContainer|14_0(num);
								UIFragment uifragment = document.Instantiate(this.Desktop, group);
								Button button = uifragment.Get<Button>("Button");
								num++;
								button.Activating = (button.Find<TextButton>("PlayButton").Activating = delegate()
								{
									CS$<>8__locals1.<>4__this.Queue(CS$<>8__locals1.game.JoinKey);
								});
								button.MouseEntered = delegate()
								{
									CS$<>8__locals1.<>4__this._container.Reorder(group, -1);
									group.Find<Element>("LowerGlow").Visible = true;
									group.Find<Element>("UpperGlow").Visible = true;
									group.Find<Element>("PlayButton").Visible = true;
									Element element = group.Find<Element>("Name");
									element.Anchor.Bottom = element.Anchor.Bottom + CS$<>8__locals1.<>4__this._nameLabelHoverOffset;
									group.Layout(null, true);
								};
								button.MouseExited = delegate()
								{
									group.Find<Element>("LowerGlow").Visible = false;
									group.Find<Element>("UpperGlow").Visible = false;
									group.Find<Element>("PlayButton").Visible = false;
									Element element = group.Find<Element>("Name");
									element.Anchor.Bottom = element.Anchor.Bottom - CS$<>8__locals1.<>4__this._nameLabelHoverOffset;
									group.Layout(null, true);
								};
								uifragment.Get<Label>("Name").Text = CS$<>8__locals1.game.DefaultName;
								Element imageElt = uifragment.Get<Element>("Image");
								TextureArea textureArea;
								bool flag4 = this._downloadedGameTextureAreas.TryGetValue(CS$<>8__locals1.game.ImageUrl, out textureArea);
								if (flag4)
								{
									imageElt.Background = new PatchStyle(textureArea);
								}
								else
								{
									ExternalTextureLoader externalTextureLoader;
									bool flag5 = !this._gameTextureLoaders.TryGetValue(CS$<>8__locals1.game.ImageUrl, out externalTextureLoader);
									if (flag5)
									{
										externalTextureLoader = (this._gameTextureLoaders[CS$<>8__locals1.game.ImageUrl] = ExternalTextureLoader.FromUrl(this.Interface.App, CS$<>8__locals1.game.ImageUrl));
										externalTextureLoader.OnComplete += delegate(object sender, TextureArea downloadedTextureArea)
										{
											CS$<>8__locals1.<>4__this._gameTextureLoaders.Remove(CS$<>8__locals1.game.ImageUrl);
											CS$<>8__locals1.<>4__this._downloadedGameTextureAreas.Add(CS$<>8__locals1.game.ImageUrl, downloadedTextureArea);
										};
										externalTextureLoader.OnFailure += delegate(object sender, Exception exception)
										{
											CS$<>8__locals1.<>4__this._gameTextureLoaders.Remove(CS$<>8__locals1.game.ImageUrl);
											MinigamesPage.Logger.Error(exception, "Failed to load game image from {0}.", new object[]
											{
												CS$<>8__locals1.game.ImageUrl
											});
										};
									}
									externalTextureLoader.OnComplete += delegate(object sender, TextureArea downloadedTextureArea)
									{
										imageElt.Background = new PatchStyle(downloadedTextureArea);
										imageElt.Layout(null, true);
									};
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x0007E9C0 File Offset: 0x0007CBC0
		protected override void OnMounted()
		{
			this.MainMenuView.ShowTopBar(true);
			this.BuildGamesList();
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x0007E9D8 File Offset: 0x0007CBD8
		protected override void OnUnmounted()
		{
			foreach (ExternalTextureLoader externalTextureLoader in this._gameTextureLoaders.Values)
			{
				externalTextureLoader.Cancel();
			}
			this._gameTextureLoaders.Clear();
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x0007EA40 File Offset: 0x0007CC40
		public void OnGamesUpdated()
		{
			this.BuildGamesList();
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._container.Layout(null, true);
			}
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x0007EA84 File Offset: 0x0007CC84
		[CompilerGenerated]
		private Group <BuildGamesList>g__MakeButtonContainer|14_0(int index)
		{
			int value = index % this._framesPerRow * (this._frameWidth + this._frameSpacing);
			int value2 = index / this._framesPerRow * (this._frameHeight + this._frameSpacing);
			return new Group(this.Desktop, this._container)
			{
				Anchor = new Anchor
				{
					Left = new int?(value),
					Top = new int?(value2),
					Width = new int?(this._frameWidth),
					Height = new int?(this._frameHeight)
				}
			};
		}

		// Token: 0x0400199C RID: 6556
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400199D RID: 6557
		private readonly Dictionary<string, TextureArea> _downloadedGameTextureAreas = new Dictionary<string, TextureArea>();

		// Token: 0x0400199E RID: 6558
		private readonly Dictionary<string, ExternalTextureLoader> _gameTextureLoaders = new Dictionary<string, ExternalTextureLoader>();

		// Token: 0x0400199F RID: 6559
		public readonly MainMenuView MainMenuView;

		// Token: 0x040019A0 RID: 6560
		private Group _container;

		// Token: 0x040019A1 RID: 6561
		private int _framesPerRow;

		// Token: 0x040019A2 RID: 6562
		private int _frameWidth;

		// Token: 0x040019A3 RID: 6563
		private int _frameHeight;

		// Token: 0x040019A4 RID: 6564
		private int _frameSpacing;

		// Token: 0x040019A5 RID: 6565
		private int _nameLabelHoverOffset;
	}
}
