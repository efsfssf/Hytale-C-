using System;
using System.Collections.Generic;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.UI.Elements
{
	// Token: 0x02000876 RID: 2166
	[UIMarkupElement(AcceptsChildren = true)]
	internal class TabNavigation : Element
	{
		// Token: 0x17001081 RID: 4225
		// (set) Token: 0x06003D36 RID: 15670 RVA: 0x0009BCCF File Offset: 0x00099ECF
		public TabNavigation.Tab[] Tabs
		{
			set
			{
				this.BuildTabNavigation(value);
			}
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x0009BCD9 File Offset: 0x00099ED9
		public TabNavigation(Desktop desktop, Element parent) : base(desktop, parent)
		{
			this._layoutMode = LayoutMode.Left;
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x0009BCF8 File Offset: 0x00099EF8
		internal override void AddFromMarkup(Element child)
		{
			TabNavigation.TabButton tabButton = child as TabNavigation.TabButton;
			bool flag = tabButton == null;
			if (flag)
			{
				throw new Exception("Children of TabNavigation must be of type TabButton");
			}
			bool flag2 = this.Style.SeparatorBackground != null && this._tabButtons.Count > 0;
			if (flag2)
			{
				Group group = new Group(this.Desktop, this);
				group.Anchor = this.Style.SeparatorAnchor;
				group.Background = this.Style.SeparatorBackground;
			}
			base.Add(child, -1);
			bool flag3 = this.SelectedTab == tabButton.Id;
			tabButton.IsSelected = flag3;
			tabButton.TabStyle = (flag3 ? this.Style.SelectedTabStyle : this.Style.TabStyle);
			tabButton.Sounds = this.Style.TabSounds;
			tabButton.Activating = delegate()
			{
				this.OnActivateTab(tabButton.Id);
			};
			this._tabButtons.Add(tabButton);
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x0009BE20 File Offset: 0x0009A020
		private void BuildTabNavigation(TabNavigation.Tab[] tabs)
		{
			base.Clear();
			this._tabButtons.Clear();
			for (int i = 0; i < tabs.Length; i++)
			{
				TabNavigation.Tab tab = tabs[i];
				bool isSelected = this.SelectedTab == tab.Id;
				this._tabButtons.Add(new TabNavigation.TabButton(this.Desktop, this)
				{
					Id = tab.Id,
					IsSelected = isSelected,
					Icon = tab.Icon,
					IconSelected = tab.IconSelected,
					IconAnchor = tab.IconAnchor,
					Text = tab.Text,
					Activating = delegate()
					{
						this.OnActivateTab(tab.Id);
					}
				});
				bool flag = this.Style.SeparatorBackground != null && i < tabs.Length - 1;
				if (flag)
				{
					Group group = new Group(this.Desktop, this);
					group.Anchor = this.Style.SeparatorAnchor;
					group.Background = this.Style.SeparatorBackground;
				}
			}
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x0009BF5C File Offset: 0x0009A15C
		private void OnActivateTab(string tabId)
		{
			bool flag = this.AllowUnselection && this.SelectedTab == tabId;
			if (flag)
			{
				this.SelectedTab = null;
			}
			else
			{
				this.SelectedTab = tabId;
			}
			base.Layout(null, true);
			Action selectedTabChanged = this.SelectedTabChanged;
			if (selectedTabChanged != null)
			{
				selectedTabChanged();
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x0009BFB8 File Offset: 0x0009A1B8
		public override Point ComputeScaledMinSize(int? maxWidth, int? maxHeight)
		{
			this.ApplyStyles();
			return base.ComputeScaledMinSize(maxWidth, maxHeight);
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x0009BFDC File Offset: 0x0009A1DC
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			foreach (TabNavigation.TabButton tabButton in this._tabButtons)
			{
				tabButton.IsSelected = (this.SelectedTab == tabButton.Id);
				tabButton.TabStyle = (tabButton.IsSelected ? this.Style.SelectedTabStyle : this.Style.TabStyle);
				tabButton.Sounds = this.Style.TabSounds;
				tabButton.UpdateStyle();
			}
		}

		// Token: 0x04001C7E RID: 7294
		[UIMarkupProperty]
		public TabNavigation.TabNavigationStyle Style;

		// Token: 0x04001C7F RID: 7295
		[UIMarkupProperty]
		public string SelectedTab;

		// Token: 0x04001C80 RID: 7296
		[UIMarkupProperty]
		public bool AllowUnselection;

		// Token: 0x04001C81 RID: 7297
		public Action SelectedTabChanged;

		// Token: 0x04001C82 RID: 7298
		private readonly List<TabNavigation.TabButton> _tabButtons = new List<TabNavigation.TabButton>();

		// Token: 0x02000D41 RID: 3393
		[UIMarkupElement]
		public class TabButton : Button
		{
			// Token: 0x17001428 RID: 5160
			// (set) Token: 0x060064FA RID: 25850 RVA: 0x00210801 File Offset: 0x0020EA01
			public ButtonSounds Sounds
			{
				set
				{
					this.Style.Sounds = value;
				}
			}

			// Token: 0x17001429 RID: 5161
			// (set) Token: 0x060064FB RID: 25851 RVA: 0x0021080F File Offset: 0x0020EA0F
			[UIMarkupProperty]
			public string Text
			{
				set
				{
					this._label.Text = value;
					this._label.Visible = (value != null);
				}
			}

			// Token: 0x060064FC RID: 25852 RVA: 0x0021082F File Offset: 0x0020EA2F
			public TabButton(Desktop desktop, Element parent) : base(desktop, parent)
			{
				this._iconElement = new Group(desktop, this);
				this._overlayElement = new Group(desktop, this);
				this._label = new Label(desktop, this)
				{
					Visible = false
				};
			}

			// Token: 0x060064FD RID: 25853 RVA: 0x0021086C File Offset: 0x0020EA6C
			protected override void OnMouseEnter()
			{
				base.OnMouseEnter();
				this.UpdateStyle();
				base.Layout(null, true);
			}

			// Token: 0x060064FE RID: 25854 RVA: 0x0021089C File Offset: 0x0020EA9C
			protected override void OnMouseLeave()
			{
				base.OnMouseLeave();
				this.UpdateStyle();
				base.Layout(null, true);
			}

			// Token: 0x060064FF RID: 25855 RVA: 0x002108CC File Offset: 0x0020EACC
			protected override void OnMouseButtonDown(MouseButtonEvent evt)
			{
				base.OnMouseButtonDown(evt);
				this.UpdateStyle();
				base.Layout(null, true);
			}

			// Token: 0x06006500 RID: 25856 RVA: 0x002108FC File Offset: 0x0020EAFC
			protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
			{
				this.UpdateStyle();
				base.Layout(null, true);
				TabNavigation tabNavigation = (TabNavigation)this.Parent;
				bool flag = !activate || (long)evt.Button != 1L || (this.IsSelected && !tabNavigation.AllowUnselection);
				if (!flag)
				{
					ButtonSounds sounds = this.Style.Sounds;
					bool flag2 = ((sounds != null) ? sounds.Activate : null) != null;
					if (flag2)
					{
						IUIProvider provider = this.Desktop.Provider;
						ButtonSounds sounds2 = this.Style.Sounds;
						provider.PlaySound((sounds2 != null) ? sounds2.Activate : null);
					}
					Action activating = this.Activating;
					if (activating != null)
					{
						activating();
					}
				}
			}

			// Token: 0x06006501 RID: 25857 RVA: 0x002109B4 File Offset: 0x0020EBB4
			public void UpdateStyle()
			{
				TabNavigation.TabStyleState tabStyleState = this.TabStyle.Default;
				int? capturedMouseButton = base.CapturedMouseButton;
				long? num = (capturedMouseButton != null) ? new long?((long)capturedMouseButton.GetValueOrDefault()) : null;
				long num2 = (long)((ulong)1);
				bool flag = (num.GetValueOrDefault() == num2 & num != null) && this.TabStyle.Pressed != null;
				if (flag)
				{
					tabStyleState = this.TabStyle.Pressed;
				}
				else
				{
					bool flag2 = base.IsHovered && this.TabStyle.Hovered != null;
					if (flag2)
					{
						tabStyleState = this.TabStyle.Hovered;
					}
				}
				this.Anchor = tabStyleState.Anchor;
				this.FlexWeight = tabStyleState.FlexWeight;
				this.Background = tabStyleState.Background;
				this.Padding = tabStyleState.Padding;
				this._overlayElement.Background = tabStyleState.Overlay;
				this._iconElement.Background = ((this.IsSelected && this.IconSelected != null) ? this.IconSelected : this.Icon);
				bool flag3 = this._iconElement.Background != null;
				if (flag3)
				{
					this._iconElement.Visible = true;
					this._iconElement.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)((int)(tabStyleState.IconOpacity * 255f)));
					this._iconElement.Anchor = (this.IconAnchor ?? tabStyleState.IconAnchor);
				}
				else
				{
					this._iconElement.Visible = false;
				}
				this._label.Style = tabStyleState.LabelStyle;
			}

			// Token: 0x0400414D RID: 16717
			public TabNavigation.TabStyle TabStyle;

			// Token: 0x0400414E RID: 16718
			public bool IsSelected;

			// Token: 0x0400414F RID: 16719
			[UIMarkupProperty]
			public PatchStyle Icon;

			// Token: 0x04004150 RID: 16720
			[UIMarkupProperty]
			public PatchStyle IconSelected;

			// Token: 0x04004151 RID: 16721
			[UIMarkupProperty]
			public Anchor? IconAnchor;

			// Token: 0x04004152 RID: 16722
			[UIMarkupProperty]
			public string Id;

			// Token: 0x04004153 RID: 16723
			private Group _overlayElement;

			// Token: 0x04004154 RID: 16724
			private Group _iconElement;

			// Token: 0x04004155 RID: 16725
			private Label _label;
		}

		// Token: 0x02000D42 RID: 3394
		[UIMarkupData]
		public class TabNavigationStyle
		{
			// Token: 0x04004156 RID: 16726
			public TabNavigation.TabStyle TabStyle;

			// Token: 0x04004157 RID: 16727
			public TabNavigation.TabStyle SelectedTabStyle;

			// Token: 0x04004158 RID: 16728
			public Anchor SeparatorAnchor;

			// Token: 0x04004159 RID: 16729
			public PatchStyle SeparatorBackground;

			// Token: 0x0400415A RID: 16730
			public ButtonSounds TabSounds;
		}

		// Token: 0x02000D43 RID: 3395
		[UIMarkupData]
		public class TabStyle
		{
			// Token: 0x0400415B RID: 16731
			public TabNavigation.TabStyleState Default;

			// Token: 0x0400415C RID: 16732
			public TabNavigation.TabStyleState Hovered;

			// Token: 0x0400415D RID: 16733
			public TabNavigation.TabStyleState Pressed;
		}

		// Token: 0x02000D44 RID: 3396
		[UIMarkupData]
		public class TabStyleState
		{
			// Token: 0x0400415E RID: 16734
			public PatchStyle Background;

			// Token: 0x0400415F RID: 16735
			public PatchStyle Overlay;

			// Token: 0x04004160 RID: 16736
			public Anchor Anchor;

			// Token: 0x04004161 RID: 16737
			public Padding Padding;

			// Token: 0x04004162 RID: 16738
			public Anchor IconAnchor;

			// Token: 0x04004163 RID: 16739
			public float IconOpacity = 1f;

			// Token: 0x04004164 RID: 16740
			public LabelStyle LabelStyle;

			// Token: 0x04004165 RID: 16741
			public int FlexWeight;
		}

		// Token: 0x02000D45 RID: 3397
		[UIMarkupData]
		public class Tab
		{
			// Token: 0x04004166 RID: 16742
			public string Id;

			// Token: 0x04004167 RID: 16743
			public PatchStyle Icon;

			// Token: 0x04004168 RID: 16744
			public PatchStyle IconSelected;

			// Token: 0x04004169 RID: 16745
			public Anchor? IconAnchor;

			// Token: 0x0400416A RID: 16746
			public string Text;
		}
	}
}
