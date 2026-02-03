using System;
using System.Collections.Generic;
using HytaleClient.Application;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using SDL2;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B5 RID: 2229
	internal class ChatComponent : InterfaceComponent
	{
		// Token: 0x0600408E RID: 16526 RVA: 0x000BAACB File Offset: 0x000B8CCB
		public bool IsOpen()
		{
			return this.Interface.App.Stage == App.AppStage.InGame && this.InGameView.InGame.Instance.Chat.IsOpen;
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x000BAB00 File Offset: 0x000B8D00
		public ChatComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x000BAB5C File Offset: 0x000B8D5C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/Chat.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, null);
			this._itemTooltip = new ItemTooltipLayer(this.InGameView)
			{
				ShowArrow = false
			};
			this._floatingContainer = uifragment.Get<Group>("FloatingContainer");
			this._floatingChatLog = uifragment.Get<Group>("FloatingChatLog");
			this._fullContainer = uifragment.Get<Group>("FullContainer");
			this._fullChatLog = uifragment.Get<Group>("FullChatLog");
			this._chatInput = uifragment.Get<TextField>("ChatInput");
			this._chatInput.Validating = new Action(this.OnSendMessage);
			this._chatInput.KeyDown = delegate(SDL.SDL_Keycode keycode)
			{
				bool flag2 = keycode == SDL.SDL_Keycode.SDLK_DOWN || keycode == SDL.SDL_Keycode.SDLK_UP;
				if (flag2)
				{
					this._sentMessageCursor = MathHelper.Clamp(this._sentMessageCursor + ((keycode == SDL.SDL_Keycode.SDLK_UP) ? 1 : -1), -1, this._sentMessages.Count - 1);
					string value = "";
					bool flag3 = this._sentMessageCursor > -1;
					if (flag3)
					{
						value = this._sentMessages.PeekAt(this._sentMessages.Count - this._sentMessageCursor - 1);
					}
					this._chatInput.Value = value;
				}
			};
			this._logHeight = document.ResolveNamedValue<int>(this.Desktop.Provider, "LogHeight");
			this._fontSize = document.ResolveNamedValue<int>(this.Desktop.Provider, "FontSize");
			this._messageSpacing = document.ResolveNamedValue<int>(this.Desktop.Provider, "MessageSpacing");
			this._textColor = document.ResolveNamedValue<UInt32Color>(this.Desktop.Provider, "TextColor");
			foreach (ChatComponent.ChatLogEntry chatLogEntry in this._chatLogEntries)
			{
				bool flag = chatLogEntry.FloatingGroup != null;
				if (flag)
				{
					chatLogEntry.FloatingGroup.Parent.Remove(chatLogEntry.FloatingGroup);
					this._floatingChatLog.Add(chatLogEntry.FloatingGroup, -1);
				}
				chatLogEntry.FullGroup.Parent.Remove(chatLogEntry.FullGroup);
				this._fullChatLog.Add(chatLogEntry.FullGroup, -1);
			}
			base.Add(this._fullContainer, -1);
			base.Add(this._floatingContainer, -1);
			this._fullContainer.Visible = this.IsOpen();
			this._floatingContainer.Visible = !this._fullContainer.Visible;
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x000BAD90 File Offset: 0x000B8F90
		protected override void OnMounted()
		{
			this._currentTime = 0f;
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x000BADB6 File Offset: 0x000B8FB6
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x000BADD4 File Offset: 0x000B8FD4
		public void ResetState()
		{
			this._chatInput.Value = "";
			this._chatLogEntries.Clear();
			this._sentMessages.Clear();
			this._sentMessageCursor = -1;
			this._currentTime = 0f;
			this._floatingContainer.Visible = true;
			this._fullContainer.Visible = false;
			this._floatingChatLog.Clear();
			this._floatingChatLogEntries.Clear();
			this._fullChatLog.Clear();
			this._itemTooltip.Stop();
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x000BAE68 File Offset: 0x000B9068
		private void Animate(float deltaTime)
		{
			bool flag = false;
			this._currentTime += deltaTime;
			while (this._floatingChatLogEntries.Count > 0)
			{
				ChatComponent.ChatLogEntry chatLogEntry = this._floatingChatLogEntries[0];
				bool flag2 = chatLogEntry.FloatingExpirationTime < this._currentTime;
				if (!flag2)
				{
					break;
				}
				this._floatingChatLogEntries.RemoveAt(0);
				this._floatingChatLog.Remove(chatLogEntry.FloatingGroup);
				chatLogEntry.FloatingGroup = null;
				flag = true;
			}
			bool flag3 = flag && !this.IsOpen() && this.InGameView.InGame.IsHudVisible;
			if (flag3)
			{
				this.LayoutFloatingChat(true);
			}
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x000BAF16 File Offset: 0x000B9116
		internal void OnHudVisibilityChanged()
		{
			this.ApplyVisibility();
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x000BAF1F File Offset: 0x000B911F
		internal void OnOpened(SDL.SDL_Keycode? keyCodeTrigger, bool isCommand)
		{
			this._keyCodeTrigger = keyCodeTrigger;
			this._chatInput.Value = (isCommand ? "/" : "");
			this.ApplyVisibility();
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x000BAF4B File Offset: 0x000B914B
		internal void OnClosed()
		{
			this._chatInput.Value = "";
			this.ApplyVisibility();
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x000BAF68 File Offset: 0x000B9168
		private void ApplyVisibility()
		{
			bool flag = this.IsOpen();
			this._floatingContainer.Visible = (!flag && this.InGameView.InGame.IsHudVisible);
			this._fullContainer.Visible = flag;
			bool flag2 = flag;
			if (flag2)
			{
				this._fullChatLog.SetScroll(new int?(0), new int?(int.MaxValue));
				this.Desktop.FocusElement(this, true);
			}
			else
			{
				bool isHudVisible = this.InGameView.InGame.IsHudVisible;
				if (isHudVisible)
				{
					this._floatingChatLog.SetScroll(new int?(0), new int?(int.MaxValue));
					bool flag3 = this.Desktop.FocusedElement == this;
					if (flag3)
					{
						this.Desktop.FocusElement(null, true);
					}
				}
			}
			base.Layout(null, true);
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x000BB044 File Offset: 0x000B9244
		private void OnSendMessage()
		{
			string text = this._chatInput.Value.Trim();
			bool flag = !this.IsOpen();
			if (!flag)
			{
				this._chatInput.Value = "";
				bool flag2 = text.Length > 0;
				if (flag2)
				{
					bool flag3 = this._sentMessages.PeekAt(this._sentMessages.Count - 1) != text;
					if (flag3)
					{
						this._sentMessages.Push(text);
					}
					this._sentMessageCursor = -1;
					this.InGameView.InGame.SendChatMessageOrExecuteCommand(text);
				}
				this.InGameView.InGame.Instance.Chat.Close();
			}
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x000BB0F8 File Offset: 0x000B92F8
		protected internal override void OnKeyDown(SDL.SDL_Keycode keycode, int repeat)
		{
			SDL.SDL_Keycode? keyCodeTrigger = this._keyCodeTrigger;
			this._discardNextTextInput = ((keycode == keyCodeTrigger.GetValueOrDefault() & keyCodeTrigger != null) && repeat == 0);
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x000BB130 File Offset: 0x000B9330
		protected internal override void OnKeyUp(SDL.SDL_Keycode keycode)
		{
			base.OnKeyUp(keycode);
			bool flag = this.IsOpen();
			if (flag)
			{
				this.Desktop.FocusElement(this._chatInput, true);
			}
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x000BB164 File Offset: 0x000B9364
		protected internal override void OnTextInput(string text)
		{
			bool discardNextTextInput = this._discardNextTextInput;
			if (!discardNextTextInput)
			{
				this._chatInput.OnTextInput(text);
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x000BB18C File Offset: 0x000B938C
		public void OnReceiveMessage(FormattedMessage message)
		{
			ChatComponent.ChatLogEntry chatLogEntry = new ChatComponent.ChatLogEntry
			{
				Message = message,
				FloatingGroup = new Group(this.Desktop, this._floatingChatLog)
				{
					LayoutMode = LayoutMode.Top
				},
				FullGroup = new Group(this.Desktop, this._fullChatLog)
				{
					LayoutMode = LayoutMode.Top
				},
				FloatingExpirationTime = this._currentTime + 5f
			};
			this._chatLogEntries.Add(chatLogEntry);
			this._floatingChatLogEntries.Add(chatLogEntry);
			while (this._fullChatLog.Children.Count > this.Interface.App.Settings.MaxChatMessages)
			{
				this._fullChatLog.RemoveAt(0);
			}
			List<Label.LabelSpan> labelSpans = FormattedMessageConverter.GetLabelSpans(message, this.Interface, new SpanStyle
			{
				Color = new UInt32Color?(this._textColor)
			}, true);
			Label label = new Label(this.Desktop, chatLogEntry.FloatingGroup);
			label.Anchor.Vertical = new int?(this._messageSpacing);
			label.Style.Wrap = true;
			label.Style.FontSize = (float)this._fontSize;
			label.TextSpans = labelSpans;
			Label label2 = new Label(this.Desktop, chatLogEntry.FullGroup);
			label2.Anchor.Vertical = new int?(this._messageSpacing);
			label2.Style.Wrap = true;
			label2.Style.FontSize = (float)this._fontSize;
			label2.TextSpans = labelSpans;
			label2.TagMouseEntered = new Action<Label.LabelSpanPortion>(this.OnHoverTag);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				bool flag = this.IsOpen();
				if (flag)
				{
					this._fullChatLog.Layout(null, true);
				}
				else
				{
					bool isHudVisible = this.InGameView.InGame.IsHudVisible;
					if (isHudVisible)
					{
						this.LayoutFloatingChat(true);
					}
				}
			}
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x000BB374 File Offset: 0x000B9574
		private void OnHoverTag(Label.LabelSpanPortion portion = null)
		{
			bool flag = portion == null;
			if (flag)
			{
				this._itemTooltip.Stop();
			}
			else
			{
				ChatTagType chatTagType = (ChatTagType)Enum.Parse(typeof(ChatTagType), portion.Span.Params["tagType"].ToString());
				ChatTagType chatTagType2 = chatTagType;
				if (chatTagType2 == null)
				{
					ClientItemStack stack = new ClientItemStack((string)portion.Span.Params["id"], 1);
					this._itemTooltip.UpdateTooltip(portion.CenterPoint, stack, null, null, null);
					this._itemTooltip.Start(false);
				}
			}
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x000BB418 File Offset: 0x000B9618
		public void InsertItemTag(string itemId)
		{
			bool flag = !this.IsOpen();
			if (flag)
			{
				this.InGameView.InGame.Instance.Chat.TryOpen(null, false);
			}
			this._chatInput.InsertAtCursor(" <Item:" + itemId + ">");
			this.Desktop.FocusElement(this._chatInput, true);
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x000BB488 File Offset: 0x000B9688
		private void LayoutFloatingChat(bool layoutChildren = true)
		{
			bool flag = this._floatingChatLog.Children.Count == 0;
			if (flag)
			{
				this._floatingContainer.Visible = false;
			}
			else
			{
				bool flag2 = !this._floatingContainer.Visible || layoutChildren;
				if (flag2)
				{
					this._floatingContainer.Visible = true;
					this._floatingContainer.Layout(new Rectangle?(this._rectangleAfterPadding), true);
				}
				int num = this._floatingChatLog.Anchor.Top.Value * 2 - 2;
				foreach (Element element in this._floatingChatLog.Children)
				{
					num += this.Desktop.UnscaleRound((float)element.AnchoredRectangle.Height);
				}
				bool flag3 = num >= this._logHeight;
				if (flag3)
				{
					num = this._logHeight;
				}
				this._floatingContainer.Anchor.Height = new int?(num);
				this._floatingContainer.Layout(null, false);
			}
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x000BB5BC File Offset: 0x000B97BC
		protected override void AfterChildrenLayout()
		{
			bool flag = !this.IsOpen() && this.InGameView.InGame.IsHudVisible;
			if (flag)
			{
				this.LayoutFloatingChat(false);
			}
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x000BB5F1 File Offset: 0x000B97F1
		protected internal override void Dismiss()
		{
			this.InGameView.InGame.Instance.Chat.Close();
		}

		// Token: 0x04001EE5 RID: 7909
		public const float FloatingChatDuration = 5f;

		// Token: 0x04001EE6 RID: 7910
		public readonly InGameView InGameView;

		// Token: 0x04001EE7 RID: 7911
		private float _currentTime;

		// Token: 0x04001EE8 RID: 7912
		private readonly List<ChatComponent.ChatLogEntry> _chatLogEntries = new List<ChatComponent.ChatLogEntry>();

		// Token: 0x04001EE9 RID: 7913
		private readonly List<ChatComponent.ChatLogEntry> _floatingChatLogEntries = new List<ChatComponent.ChatLogEntry>();

		// Token: 0x04001EEA RID: 7914
		private readonly DropOutStack<string> _sentMessages = new DropOutStack<string>(20);

		// Token: 0x04001EEB RID: 7915
		private int _sentMessageCursor = -1;

		// Token: 0x04001EEC RID: 7916
		private Group _floatingContainer;

		// Token: 0x04001EED RID: 7917
		private Group _floatingChatLog;

		// Token: 0x04001EEE RID: 7918
		private Group _fullContainer;

		// Token: 0x04001EEF RID: 7919
		private Group _fullChatLog;

		// Token: 0x04001EF0 RID: 7920
		private TextField _chatInput;

		// Token: 0x04001EF1 RID: 7921
		private ItemTooltipLayer _itemTooltip;

		// Token: 0x04001EF2 RID: 7922
		private SDL.SDL_Keycode? _keyCodeTrigger;

		// Token: 0x04001EF3 RID: 7923
		private bool _discardNextTextInput = false;

		// Token: 0x04001EF4 RID: 7924
		private int _logHeight;

		// Token: 0x04001EF5 RID: 7925
		private int _fontSize;

		// Token: 0x04001EF6 RID: 7926
		private int _messageSpacing;

		// Token: 0x04001EF7 RID: 7927
		private UInt32Color _textColor;

		// Token: 0x02000D7C RID: 3452
		private class ChatLogEntry
		{
			// Token: 0x04004217 RID: 16919
			public FormattedMessage Message;

			// Token: 0x04004218 RID: 16920
			public float FloatingExpirationTime;

			// Token: 0x04004219 RID: 16921
			public Group FloatingGroup;

			// Token: 0x0400421A RID: 16922
			public Group FullGroup;
		}
	}
}
