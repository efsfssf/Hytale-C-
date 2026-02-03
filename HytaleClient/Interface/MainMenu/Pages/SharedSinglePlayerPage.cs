using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HytaleClient.Application.Services;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using NLog;

namespace HytaleClient.Interface.MainMenu.Pages
{
	// Token: 0x02000821 RID: 2081
	internal class SharedSinglePlayerPage : InterfaceComponent
	{
		// Token: 0x060039E2 RID: 14818 RVA: 0x00080068 File Offset: 0x0007E268
		public SharedSinglePlayerPage(MainMenuView mainMenuView) : base(mainMenuView.Interface, null)
		{
			this.MainMenuView = mainMenuView;
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x00080080 File Offset: 0x0007E280
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("MainMenu/SharedSinglePlayer/SharedSinglePlayerPage.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._container = uifragment.Get<Group>("WorldsContainer");
			this._framesPerRow = document.ResolveNamedValue<int>(this.Desktop.Provider, "FramesPerRow");
			this._frameWidth = document.ResolveNamedValue<int>(this.Desktop.Provider, "FrameWidth");
			this._frameHeight = document.ResolveNamedValue<int>(this.Desktop.Provider, "FrameHeight");
			this._frameSpacing = document.ResolveNamedValue<int>(this.Desktop.Provider, "FrameSpacing");
			this.BuildWorldsList();
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x0008013E File Offset: 0x0007E33E
		private void Queue(Guid worldId)
		{
			this.Interface.App.MainMenu.QueueForSharedSinglePlayerWorld(worldId);
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x00080158 File Offset: 0x0007E358
		private void BuildWorldsList()
		{
			SharedSinglePlayerPage.Logger.Info("Building worlds list!");
			bool flag = !base.IsMounted;
			if (!flag)
			{
				List<ClientSharedSinglePlayerJoinableWorldWrapper> sharedSinglePlayerJoinableWorlds = this.Interface.App.HytaleServices.SharedSinglePlayerJoinableWorlds;
				bool flag2 = sharedSinglePlayerJoinableWorlds == null;
				if (!flag2)
				{
					this._container.Clear();
					Document document;
					this.Interface.TryGetDocument("MainMenu/SharedSinglePlayer/SharedSinglePlayerButton.ui", out document);
					this._nameLabelHoverOffset = document.ResolveNamedValue<int>(this.Desktop.Provider, "NameLabelHoverOffset");
					int num = 0;
					using (List<ClientSharedSinglePlayerJoinableWorldWrapper>.Enumerator enumerator = sharedSinglePlayerJoinableWorlds.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SharedSinglePlayerPage.<>c__DisplayClass11_0 CS$<>8__locals1 = new SharedSinglePlayerPage.<>c__DisplayClass11_0();
							CS$<>8__locals1.<>4__this = this;
							CS$<>8__locals1.world = enumerator.Current;
							SharedSinglePlayerPage.Logger.Info(string.Format("Creating a UI element for world {0}", CS$<>8__locals1.world));
							bool hidden = CS$<>8__locals1.world.Hidden;
							if (!hidden)
							{
								Group group = this.<BuildWorldsList>g__MakeButtonContainer|11_0(num);
								UIFragment uifragment = document.Instantiate(this.Desktop, group);
								Button button = uifragment.Get<Button>("Button");
								num++;
								button.Activating = (button.Find<TextButton>("PlayButton").Activating = delegate()
								{
									CS$<>8__locals1.<>4__this.Queue(CS$<>8__locals1.world.WorldId);
								});
								button.MouseEntered = delegate()
								{
									CS$<>8__locals1.<>4__this._container.Reorder(group, -1);
									group.Find<Element>("LowerGlow").Visible = true;
									group.Find<Element>("UpperGlow").Visible = true;
									group.Find<Element>("PlayButton").Visible = true;
									Element element2 = group.Find<Element>("Name");
									element2.Anchor.Bottom = element2.Anchor.Bottom + CS$<>8__locals1.<>4__this._nameLabelHoverOffset;
									group.Layout(null, true);
								};
								button.MouseExited = delegate()
								{
									group.Find<Element>("LowerGlow").Visible = false;
									group.Find<Element>("UpperGlow").Visible = false;
									group.Find<Element>("PlayButton").Visible = false;
									Element element2 = group.Find<Element>("Name");
									element2.Anchor.Bottom = element2.Anchor.Bottom - CS$<>8__locals1.<>4__this._nameLabelHoverOffset;
									group.Layout(null, true);
								};
								uifragment.Get<Label>("Name").Text = CS$<>8__locals1.world.Name;
								Element element = uifragment.Get<Element>("Image");
								element.Layout(null, true);
							}
						}
					}
					this.AddCreateButton(this.<BuildWorldsList>g__MakeButtonContainer|11_0(num++), document);
				}
			}
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x00080384 File Offset: 0x0007E584
		private void AddCreateButton(Group group, Document doc)
		{
			UIFragment uifragment = doc.Instantiate(this.Desktop, group);
			Button button = uifragment.Get<Button>("Button");
			button.Activating = (button.Find<TextButton>("PlayButton").Activating = delegate()
			{
				this.Interface.App.MainMenu.CreateSharedSinglePlayerWorld("new world");
			});
			button.MouseEntered = delegate()
			{
				this._container.Reorder(group, -1);
				group.Find<Element>("LowerGlow").Visible = true;
				group.Find<Element>("UpperGlow").Visible = true;
				group.Find<Element>("PlayButton").Visible = true;
				Element element2 = group.Find<Element>("Name");
				element2.Anchor.Bottom = element2.Anchor.Bottom + this._nameLabelHoverOffset;
				group.Layout(null, true);
			};
			button.MouseExited = delegate()
			{
				group.Find<Element>("LowerGlow").Visible = false;
				group.Find<Element>("UpperGlow").Visible = false;
				group.Find<Element>("PlayButton").Visible = false;
				Element element2 = group.Find<Element>("Name");
				element2.Anchor.Bottom = element2.Anchor.Bottom - this._nameLabelHoverOffset;
				group.Layout(null, true);
			};
			uifragment.Get<Label>("Name").Text = "Create a new world";
			Element element = uifragment.Get<Element>("Image");
			element.Layout(null, true);
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x00080444 File Offset: 0x0007E644
		protected override void OnMounted()
		{
			this.MainMenuView.ShowTopBar(true);
			this.BuildWorldsList();
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x0008045C File Offset: 0x0007E65C
		public void OnWorldsUpdated()
		{
			this.BuildWorldsList();
			base.Layout(null, true);
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x00080490 File Offset: 0x0007E690
		[CompilerGenerated]
		private Group <BuildWorldsList>g__MakeButtonContainer|11_0(int index)
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

		// Token: 0x040019D3 RID: 6611
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040019D4 RID: 6612
		public readonly MainMenuView MainMenuView;

		// Token: 0x040019D5 RID: 6613
		private Group _container;

		// Token: 0x040019D6 RID: 6614
		private int _framesPerRow;

		// Token: 0x040019D7 RID: 6615
		private int _frameWidth;

		// Token: 0x040019D8 RID: 6616
		private int _frameHeight;

		// Token: 0x040019D9 RID: 6617
		private int _frameSpacing;

		// Token: 0x040019DA RID: 6618
		private int _nameLabelHoverOffset;
	}
}
