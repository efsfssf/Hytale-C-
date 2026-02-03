using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using HytaleClient.Application;
using HytaleClient.Interface.Messages;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using NLog;
using SDL2;
using Utf8Json;

namespace HytaleClient.InGame
{
	// Token: 0x020008E3 RID: 2275
	internal class Chat
	{
		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x06004267 RID: 16999 RVA: 0x000C8BF8 File Offset: 0x000C6DF8
		// (set) Token: 0x06004268 RID: 17000 RVA: 0x000C8C00 File Offset: 0x000C6E00
		public bool IsOpen { get; private set; }

		// Token: 0x06004269 RID: 17001 RVA: 0x000C8C0C File Offset: 0x000C6E0C
		public Chat(GameInstance gameInstance)
		{
			this._gameInstance = gameInstance;
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x000C8C60 File Offset: 0x000C6E60
		public void TryOpen(SDL.SDL_Keycode? keyCodeTrigger = null, bool isCommand = false)
		{
			Debug.Assert(!this.IsOpen);
			bool flag = this._gameInstance.App.InGame.CurrentOverlay > AppInGame.InGameOverlay.None;
			if (!flag)
			{
				bool flag2 = this._gameInstance.App.Interface.InGameView.HasFocusedElement || this._gameInstance.App.Interface.Desktop.GetInteractiveLayer() != this._gameInstance.App.Interface.InGameView;
				if (!flag2)
				{
					this.IsOpen = true;
					this._gameInstance.App.Interface.InGameView.OnChatOpened(keyCodeTrigger, isCommand);
					this._gameInstance.App.InGame.OnChatOpenChanged();
				}
			}
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x000C8D34 File Offset: 0x000C6F34
		public void Close()
		{
			Debug.Assert(this.IsOpen);
			this.IsOpen = false;
			this._gameInstance.App.Interface.InGameView.OnChatClosed();
			this._gameInstance.App.InGame.OnChatOpenChanged();
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x000C8D88 File Offset: 0x000C6F88
		public void Log(string message)
		{
			bool flag = !ThreadHelper.IsMainThread();
			if (flag)
			{
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this.Log(message);
				}, false, false);
			}
			else
			{
				this.AddMessage(message, "fff");
			}
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x000C8DF0 File Offset: 0x000C6FF0
		public void Error(string message)
		{
			bool flag = !ThreadHelper.IsMainThread();
			if (flag)
			{
				this._gameInstance.Engine.RunOnMainThread(this._gameInstance, delegate
				{
					this.Error(message);
				}, false, false);
			}
			else
			{
				Chat.Logger.Info(message);
				bool diagnosticMode = this._gameInstance.App.Settings.DiagnosticMode;
				if (diagnosticMode)
				{
					long num = this._lastMessage.ElapsedMilliseconds / 1000L;
					this._lastMessage.Restart();
					this._diagnosticMessageScore += (double)num * 5.0;
					bool flag2 = this._diagnosticMessageScore > 5.0;
					if (flag2)
					{
						this._diagnosticMessageScore = 5.0;
					}
					else
					{
						bool flag3 = this._diagnosticMessageScore < -5.0;
						if (flag3)
						{
							this._diagnosticMessageScore = -5.0;
						}
					}
					bool flag4 = this._diagnosticMessageScore < 1.0;
					if (flag4)
					{
						bool hasLoggedDiagnosticMessage = this._hasLoggedDiagnosticMessage;
						if (hasLoggedDiagnosticMessage)
						{
							this.AddMessage("[Warning] Diagnostic message rate limit reached!", "f55");
						}
						this._hasLoggedDiagnosticMessage = false;
					}
					else
					{
						this._hasLoggedDiagnosticMessage = true;
						this._lastSuccessfulMessage.Restart();
						bool flag5 = this._skippedDiagnosticMessageCount > 0U;
						if (flag5)
						{
							this.AddMessage(string.Format("[Warning] {0} skipped diagnostic messages!", this._skippedDiagnosticMessageCount), "f55");
						}
						this._skippedDiagnosticMessageCount = 0U;
						this.AddMessage(message, "f55");
					}
					this._diagnosticMessageScore -= 1.0;
				}
			}
		}

		// Token: 0x0600426E RID: 17006 RVA: 0x000C8FB4 File Offset: 0x000C71B4
		public void NotifyPlayerOfSkippedDiagnosticMessages()
		{
			bool flag = !this._gameInstance.App.Settings.DiagnosticMode;
			if (!flag)
			{
				bool flag2 = this._lastSuccessfulMessage.ElapsedMilliseconds < 10000L;
				if (!flag2)
				{
					this._lastSuccessfulMessage.Restart();
					bool flag3 = this._skippedDiagnosticMessageCount > 0U;
					if (flag3)
					{
						this.AddMessage(string.Format("[Warning] {0} skipped diagnostic messages!", this._skippedDiagnosticMessageCount), "f55");
					}
					this._skippedDiagnosticMessageCount = 0U;
				}
			}
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x000C903C File Offset: 0x000C723C
		public void HandleBeforePlayingMessages()
		{
			foreach (FormattedMessage message in this._beforePlayingMessages)
			{
				this._gameInstance.App.Interface.InGameView.ChatComponent.OnReceiveMessage(message);
			}
			this._beforePlayingMessages.Clear();
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x000C90B8 File Offset: 0x000C72B8
		private void AddMessage(string messageId, string color)
		{
			this.AddMessage(new FormattedMessage
			{
				MessageId = messageId,
				Color = color
			});
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x000C90D8 File Offset: 0x000C72D8
		private void AddMessage(FormattedMessage formattedMessage)
		{
			bool isPlaying = this._gameInstance.IsPlaying;
			if (isPlaying)
			{
				this._gameInstance.App.Interface.InGameView.ChatComponent.OnReceiveMessage(formattedMessage);
			}
			else
			{
				this._beforePlayingMessages.Add(formattedMessage);
			}
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x000C9128 File Offset: 0x000C7328
		public void AddJsonMessage(string message)
		{
			FormattedMessage formattedMessage;
			try
			{
				formattedMessage = JsonSerializer.Deserialize<FormattedMessage>(message);
			}
			catch (Exception value)
			{
				this._gameInstance.Chat.Log("Failed to parse chat message!");
				Chat.Logger.Error<Exception>(value);
				return;
			}
			this.AddMessage(formattedMessage);
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x000C9180 File Offset: 0x000C7380
		public void AddBsonMessage(sbyte[] encodedMessage)
		{
			FormattedMessage formattedMessage;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream((byte[])encodedMessage))
				{
					using (BsonDataReader bsonDataReader = new BsonDataReader(memoryStream))
					{
						JsonSerializer jsonSerializer = JsonSerializer.Create();
						formattedMessage = jsonSerializer.Deserialize<FormattedMessage>(bsonDataReader);
					}
				}
			}
			catch (Exception value)
			{
				this.Error("Failed to chat parse message!");
				Chat.Logger.Error<Exception>(value);
				return;
			}
			this.AddMessage(formattedMessage);
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x000C9220 File Offset: 0x000C7420
		public void SendMessage(string message)
		{
			this._gameInstance.Connection.SendPacket(new ChatMessage(message));
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x000C923C File Offset: 0x000C743C
		public void SendCommand(string command, params object[] args)
		{
			bool flag = args.Length != 0;
			if (flag)
			{
				command = command + " " + string.Join(" ", args);
			}
			this._gameInstance.Connection.SendPacket(new ChatMessage("/" + command));
		}

		// Token: 0x04002071 RID: 8305
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002072 RID: 8306
		private readonly GameInstance _gameInstance;

		// Token: 0x04002073 RID: 8307
		private readonly List<FormattedMessage> _beforePlayingMessages = new List<FormattedMessage>();

		// Token: 0x04002074 RID: 8308
		private const double DiagnosticMessageRate = 5.0;

		// Token: 0x04002075 RID: 8309
		private readonly Stopwatch _lastMessage = Stopwatch.StartNew();

		// Token: 0x04002076 RID: 8310
		private readonly Stopwatch _lastSuccessfulMessage = Stopwatch.StartNew();

		// Token: 0x04002077 RID: 8311
		private double _diagnosticMessageScore = 5.0;

		// Token: 0x04002078 RID: 8312
		private uint _skippedDiagnosticMessageCount;

		// Token: 0x04002079 RID: 8313
		private bool _hasLoggedDiagnosticMessage = true;
	}
}
