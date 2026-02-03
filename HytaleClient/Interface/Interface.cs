using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HytaleClient.Application;
using HytaleClient.Application.Services;
using HytaleClient.Auth.Proto.Protocol;
using HytaleClient.Core;
using HytaleClient.Interface.DevTools;
using HytaleClient.Interface.InGame;
using HytaleClient.Interface.MainMenu;
using HytaleClient.Interface.Services;
using HytaleClient.Interface.Settings;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Interface
{
	// Token: 0x02000803 RID: 2051
	internal class Interface : BaseInterface
	{
		// Token: 0x060038C6 RID: 14534 RVA: 0x00075AEC File Offset: 0x00073CEC
		public Interface(App app, string resourcePath, bool isDevModeEnabled) : base(app.Engine, app.Fonts, app.CoUIManager, resourcePath, isDevModeEnabled)
		{
			this.App = app;
			this.RegisterServicesLifecycleEvents();
			Regex bgRegex = new Regex("^Zone[0-9]+\\.png$");
			string[] array = Enumerable.ToArray<string>(Enumerable.Where<string>(Directory.GetFiles(Path.Combine(Paths.GameData, "Backgrounds")), (string f) => bgRegex.IsMatch(Path.GetFileName(f))));
			this.MainMenuBackgroundImagePath = array[new Random().Next(array.Length)];
			this.ModalDialog = new ModalDialog(this, null);
			this.SettingsComponent = new SettingsComponent(this, null);
			this.StartupView = new StartupView(this);
			this.MainMenuView = new MainMenuView(this);
			this.GameLoadingView = new GameLoadingView(this);
			this.DisconnectionView = new DisconnectionView(this);
			this.InGameCustomUIProvider = new CustomUIProvider(this);
			this.InGameView = new InGameView(this);
			this.DevToolsLayer = new DevToolsLayer(this);
			this.DevToolsNotificationPanel = new DevToolsNotificationPanel(this);
			this.SocialBar = new SocialBar(this, null);
			this.QueueStatus = new QueueStatus(this);
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x00075C29 File Offset: 0x00073E29
		protected override void DoDispose()
		{
			this.InGameCustomUIProvider.Dispose();
			base.DoDispose();
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x00075C40 File Offset: 0x00073E40
		protected override void Build()
		{
			this.ModalDialog.Build();
			this.SettingsComponent.Build();
			this.QueueStatus.Build();
			this.SocialBar.Build();
			this.MainMenuView.Build();
			this.GameLoadingView.Build();
			this.DisconnectionView.Build();
			this.InGameView.Build();
			this.DevToolsLayer.Build();
			this.DevToolsNotificationPanel.Build();
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x00075CC6 File Offset: 0x00073EC6
		protected override void SetDrawOutlines(bool draw)
		{
			base.SetDrawOutlines(draw);
			this.InGameView.CustomHud.OnChangeDrawOutlines();
			this.InGameView.CustomPage.OnChangeDrawOutlines();
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x00075CF4 File Offset: 0x00073EF4
		protected override float GetScale()
		{
			return (float)this.Engine.Window.Viewport.Height / 1080f;
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x00075D24 File Offset: 0x00073F24
		protected override void LoadTextures(bool use2x)
		{
			base.LoadTextures(use2x);
			bool isMounted = this.InGameView.IsMounted;
			if (isMounted)
			{
				this.InGameCustomUIProvider.LoadTextures(use2x);
			}
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x00075D56 File Offset: 0x00073F56
		public new void OnWindowSizeChanged()
		{
			base.OnWindowSizeChanged();
			this.SettingsComponent.OnWindowSizeChanged();
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x060038CD RID: 14541 RVA: 0x00075D6C File Offset: 0x00073F6C
		// (set) Token: 0x060038CE RID: 14542 RVA: 0x00075D74 File Offset: 0x00073F74
		public string QueueTicketName { get; private set; }

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x060038CF RID: 14543 RVA: 0x00075D7D File Offset: 0x00073F7D
		// (set) Token: 0x060038D0 RID: 14544 RVA: 0x00075D85 File Offset: 0x00073F85
		public string QueueTicketStatus { get; private set; }

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x060038D1 RID: 14545 RVA: 0x00075D8E File Offset: 0x00073F8E
		// (set) Token: 0x060038D2 RID: 14546 RVA: 0x00075D96 File Offset: 0x00073F96
		public HytaleServices.ServiceState ServiceState { get; private set; } = HytaleServices.ServiceState.Disconnected;

		// Token: 0x060038D3 RID: 14547 RVA: 0x00075DA0 File Offset: 0x00073FA0
		public void OnAppStageChanged()
		{
			InterfaceComponent interfaceComponent;
			switch (this.App.Stage)
			{
			case App.AppStage.Startup:
				interfaceComponent = this.StartupView;
				break;
			case App.AppStage.MainMenu:
				interfaceComponent = this.MainMenuView;
				break;
			case App.AppStage.GameLoading:
				interfaceComponent = this.GameLoadingView;
				break;
			case App.AppStage.InGame:
				interfaceComponent = this.InGameView;
				break;
			case App.AppStage.Disconnection:
				interfaceComponent = this.DisconnectionView;
				break;
			default:
				throw new NotSupportedException();
			}
			bool flag = this._currentView == interfaceComponent;
			if (!flag)
			{
				Element parent = this.DevToolsNotificationPanel.Parent;
				if (parent != null)
				{
					parent.Remove(this.DevToolsNotificationPanel);
				}
				interfaceComponent.Add(this.DevToolsNotificationPanel, -1);
				this.Desktop.ClearAllLayers();
				this.Desktop.SetLayer(0, interfaceComponent);
				this._currentView = interfaceComponent;
				Element parent2 = this.QueueStatus.Parent;
				if (parent2 != null)
				{
					parent2.Remove(this.QueueStatus);
				}
				this._currentView.Add(this.QueueStatus, -1);
				this._currentView.Layout(null, true);
			}
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x00075EB4 File Offset: 0x000740B4
		public void OnServicesInitialized()
		{
			this.ServiceState = HytaleServices.ServiceState.Connected;
			this.SocialBar.UpdateServiceInformation();
			this.MainMenuView.MinigamesPage.OnGamesUpdated();
			this.MainMenuView.SharedSinglePlayerPage.OnWorldsUpdated();
			this.QueueStatus.Update();
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x00075F04 File Offset: 0x00074104
		public void OnServicesStateChanged(HytaleServices.ServiceState state)
		{
			this.ServiceState = state;
			this.SocialBar.UpdateServiceInformation();
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x00075F1B File Offset: 0x0007411B
		public void OnServicesUserStateChanged(string uuid, ClientUserState state)
		{
			this.SocialBar.UpdateServiceInformation();
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x00075F2A File Offset: 0x0007412A
		public void OnServicesFriendsAdded(string uuid)
		{
			this.SocialBar.UpdateServiceInformation();
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x00075F39 File Offset: 0x00074139
		public void OnServicesFriendsRemoved(string uuid)
		{
			this.SocialBar.UpdateServiceInformation();
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x00075F48 File Offset: 0x00074148
		public void OnServicesQueueError(string causeId)
		{
			this.OnServicesQueueLeft();
			bool isMounted = this.ModalDialog.IsMounted;
			if (isMounted)
			{
				Interface.Logger.Warn("Skipped modal dialog for queue error {0} since another modal was already opened.", causeId);
			}
			else
			{
				Debug.Assert(this.MainMenuView.IsMounted);
				this.ModalDialog.Setup(new ModalDialog.DialogSetup
				{
					Title = "ui.socialMenu.queue",
					Text = "ui.socialMenu.queueErrors." + causeId,
					Cancellable = false
				});
				this.Desktop.SetLayer(4, this.ModalDialog);
			}
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x00075FD8 File Offset: 0x000741D8
		public void OnServicesQueueLeft()
		{
			this.QueueTicketName = null;
			this.QueueTicketStatus = null;
			this.QueueStatus.Update();
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x00075FF8 File Offset: 0x000741F8
		public void OnServicesQueueJoined(string joinKey)
		{
			string queueTicketName = joinKey;
			foreach (ClientGameWrapper clientGameWrapper in this.App.HytaleServices.Games)
			{
				bool flag = clientGameWrapper.JoinKey == joinKey;
				if (flag)
				{
					queueTicketName = clientGameWrapper.DefaultName;
					break;
				}
			}
			this.QueueTicketName = queueTicketName;
			this.QueueStatus.Update();
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x00076084 File Offset: 0x00074284
		public void OnServicesQueueStatusUpdate(string status)
		{
			bool flag = this.QueueTicketName == null;
			if (!flag)
			{
				this.QueueTicketStatus = status;
				this.QueueStatus.Update();
			}
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x000760B5 File Offset: 0x000742B5
		public void RegisterForEvent(string name, Disposable disposeGate, Action callback)
		{
			this._handlersForInterfaceEvents.Add(name, new Interface.InterfaceEventHandler
			{
				DisposeGate = disposeGate,
				Callback = callback
			});
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x000760D8 File Offset: 0x000742D8
		public void RegisterForEvent<T>(string name, Disposable disposeGate, Action<T> callback)
		{
			this._handlersForInterfaceEvents.Add(name, new Interface.InterfaceEventHandler
			{
				DisposeGate = disposeGate,
				Callback = callback
			});
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x000760FB File Offset: 0x000742FB
		public void RegisterForEvent<T1, T2>(string name, Disposable disposeGate, Action<T1, T2> callback)
		{
			this._handlersForInterfaceEvents.Add(name, new Interface.InterfaceEventHandler
			{
				DisposeGate = disposeGate,
				Callback = callback
			});
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0007611E File Offset: 0x0007431E
		public void RegisterForEvent<T1, T2, T3>(string name, Disposable disposeGate, Action<T1, T2, T3> callback)
		{
			this._handlersForInterfaceEvents.Add(name, new Interface.InterfaceEventHandler
			{
				DisposeGate = disposeGate,
				Callback = callback
			});
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x00076141 File Offset: 0x00074341
		public void UnregisterFromEvent(string name)
		{
			this._handlersForInterfaceEvents.Remove(name);
		}

		// Token: 0x060038E2 RID: 14562 RVA: 0x00076154 File Offset: 0x00074354
		public void TriggerEvent(string name, object data1 = null, object data2 = null, object data3 = null, object data4 = null, object data5 = null, object data6 = null)
		{
			Interface.EngineEventHandler engineEventHandler;
			bool flag = !this._handlersForEngineEvents.TryGetValue(name, out engineEventHandler);
			if (flag)
			{
				Interface.Logger.Warn("No interface-side handler for engine event: {0}", name);
			}
			else
			{
				Delegate callback = engineEventHandler.Callback;
				switch (callback.Method.GetParameters().Length)
				{
				case 0:
					callback.DynamicInvoke(Array.Empty<object>());
					break;
				case 1:
					callback.DynamicInvoke(new object[]
					{
						data1
					});
					break;
				case 2:
					callback.DynamicInvoke(new object[]
					{
						data1,
						data2
					});
					break;
				case 3:
					callback.DynamicInvoke(new object[]
					{
						data1,
						data2,
						data3
					});
					break;
				case 4:
					callback.DynamicInvoke(new object[]
					{
						data1,
						data2,
						data3,
						data4
					});
					break;
				case 5:
					callback.DynamicInvoke(new object[]
					{
						data1,
						data2,
						data3,
						data4,
						data5
					});
					break;
				case 6:
					callback.DynamicInvoke(new object[]
					{
						data1,
						data2,
						data3,
						data4,
						data5,
						data6
					});
					break;
				default:
					throw new NotSupportedException();
				}
			}
		}

		// Token: 0x060038E3 RID: 14563 RVA: 0x000762A3 File Offset: 0x000744A3
		public void RegisterForEventFromEngine(string name, Action callback)
		{
			this._handlersForEngineEvents.Add(name, new Interface.EngineEventHandler
			{
				Callback = callback
			});
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x000762BF File Offset: 0x000744BF
		public void RegisterForEventFromEngine<T>(string name, Action<T> callback)
		{
			this._handlersForEngineEvents.Add(name, new Interface.EngineEventHandler
			{
				Callback = callback
			});
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x000762DB File Offset: 0x000744DB
		public void RegisterForEventFromEngine<T1, T2>(string name, Action<T1, T2> callback)
		{
			this._handlersForEngineEvents.Add(name, new Interface.EngineEventHandler
			{
				Callback = callback
			});
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x000762F7 File Offset: 0x000744F7
		public void RegisterForEventFromEngine<T1, T2, T3>(string name, Action<T1, T2, T3> callback)
		{
			this._handlersForEngineEvents.Add(name, new Interface.EngineEventHandler
			{
				Callback = callback
			});
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x00076313 File Offset: 0x00074513
		public void RegisterForEventFromEngine<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> callback)
		{
			this._handlersForEngineEvents.Add(name, new Interface.EngineEventHandler
			{
				Callback = callback
			});
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x0007632F File Offset: 0x0007452F
		public void RegisterForEventFromEngine<T1, T2, T3, T4, T5>(string name, Action<T1, T2, T3, T4, T5> callback)
		{
			this._handlersForEngineEvents.Add(name, new Interface.EngineEventHandler
			{
				Callback = callback
			});
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x0007634B File Offset: 0x0007454B
		public void RegisterForEventFromEngine<T1, T2, T3, T4, T5, T6>(string name, Action<T1, T2, T3, T4, T5, T6> callback)
		{
			this._handlersForEngineEvents.Add(name, new Interface.EngineEventHandler
			{
				Callback = callback
			});
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x00076368 File Offset: 0x00074568
		public void TriggerEventFromInterface(string name, object data1 = null, object data2 = null, object data3 = null)
		{
			Interface.InterfaceEventHandler interfaceEventHandler;
			bool flag = !this._handlersForInterfaceEvents.TryGetValue(name, out interfaceEventHandler);
			if (flag)
			{
				Interface.Logger.Warn("No engine-side handler for engine event: {0}", name);
			}
			else
			{
				bool disposed = interfaceEventHandler.DisposeGate.Disposed;
				if (!disposed)
				{
					Delegate callback = interfaceEventHandler.Callback;
					switch (callback.Method.GetParameters().Length)
					{
					case 0:
						callback.DynamicInvoke(Array.Empty<object>());
						break;
					case 1:
						callback.DynamicInvoke(new object[]
						{
							data1
						});
						break;
					case 2:
						callback.DynamicInvoke(new object[]
						{
							data1,
							data2
						});
						break;
					case 3:
						callback.DynamicInvoke(new object[]
						{
							data1,
							data2,
							data3
						});
						break;
					default:
						throw new NotSupportedException();
					}
				}
			}
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x00076444 File Offset: 0x00074644
		private void RegisterServicesLifecycleEvents()
		{
			this.RegisterForEvent<string>("services.sendFriendRequestByUsername", this.Engine, new Action<string>(this.OnSendFriendRequestByUsername));
			this.RegisterForEvent<string, bool>("services.answerFriendRequest", this.Engine, delegate(string userId, bool accept)
			{
				this.App.HytaleServices.AnswerFriendRequest(new Guid(userId), accept, null, null);
			});
			this.RegisterForEvent<string>("services.removeFriend", this.Engine, delegate(string userId)
			{
				this.App.HytaleServices.RemoveFriend(new Guid(userId), null, null);
			});
			this.RegisterForEvent<string, string>("services.sendMessage", this.Engine, delegate(string userId, string message)
			{
				this.App.HytaleServices.SendMessage(new Guid(userId), message, null, null);
			});
			this.RegisterForEvent<string>("services.sendPartyMessage", this.Engine, delegate(string message)
			{
				this.App.HytaleServices.SendPartyMessage(message, null, null);
			});
			this.RegisterForEvent("services.disbandParty", this.Engine, delegate()
			{
				this.App.HytaleServices.DisbandParty(null, null);
			});
			this.RegisterForEvent<string, bool>("services.answerPartyInvitation", this.Engine, delegate(string partyId, bool accept)
			{
				this.App.HytaleServices.AnswerPartyInvite(partyId, accept, null, null);
			});
			this.RegisterForEvent("services.leaveParty", this.Engine, delegate()
			{
				this.App.HytaleServices.LeaveParty(null, null);
			});
			this.RegisterForEvent<string>("services.removeMemberFromParty", this.Engine, delegate(string userId)
			{
				this.App.HytaleServices.RemoveMemberFromParty(new Guid(userId), null, null);
			});
			this.RegisterForEvent("services.createParty", this.Engine, delegate()
			{
				this.App.HytaleServices.CreateParty(null, null);
			});
			this.RegisterForEvent<string, bool>("services.answerPartyInvite", this.Engine, delegate(string partyId, bool accept)
			{
				this.App.HytaleServices.AnswerPartyInvite(partyId, accept, null, null);
			});
			this.RegisterForEvent<string>("services.inviteUserToParty", this.Engine, delegate(string userId)
			{
				this.App.HytaleServices.InviteUserToParty(new Guid(userId), null, null);
			});
			this.RegisterForEvent<string>("services.makeUserPartyLeader", this.Engine, delegate(string userId)
			{
				this.App.HytaleServices.MakeUserPartyLeader(new Guid(userId), null, null);
			});
			this.RegisterForEvent<string, bool>("services.toggleUserBlocked", this.Engine, delegate(string userId, bool blocked)
			{
				this.App.HytaleServices.ToggleUserBlocked(new Guid(userId), blocked, null, null);
			});
			this.RegisterForEvent("services.leaveGameQueue", this.Engine, new Action(this.OnLeaveQueue));
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x00076614 File Offset: 0x00074814
		private void UnregisterServicesLifecycleEvents()
		{
			this.UnregisterFromEvent("services.sendFriendRequestByUsername");
			this.UnregisterFromEvent("services.answerFriendRequest");
			this.UnregisterFromEvent("services.removeFriend");
			this.UnregisterFromEvent("services.sendMessage");
			this.UnregisterFromEvent("services.sendPartyMessage");
			this.UnregisterFromEvent("services.disbandParty");
			this.UnregisterFromEvent("services.answerPartyInvitation");
			this.UnregisterFromEvent("services.leaveParty");
			this.UnregisterFromEvent("services.removeMemberFromParty");
			this.UnregisterFromEvent("services.createParty");
			this.UnregisterFromEvent("services.answerPartyInvite");
			this.UnregisterFromEvent("services.inviteUserToParty");
			this.UnregisterFromEvent("services.makeUserPartyLeader");
			this.UnregisterFromEvent("services.toggleUserBlocked");
			this.UnregisterFromEvent("services.leaveGameQueue");
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x000766D8 File Offset: 0x000748D8
		private void OnSendFriendRequestByUsername(string username)
		{
			Action <>9__3;
			this.App.HytaleServices.SendFriendRequestByUsername(username, delegate(ClientFailureNotification err)
			{
				this.Engine.RunOnMainThread(this, delegate
				{
					this.TriggerEvent("services.sendFriendRequestByUsername.reply", username, err.CauseLocalizable, null, null, null, null);
				}, false, false);
			}, delegate(ClientSuccessNotification res)
			{
				Engine engine = this.Engine;
				Disposable <>4__this = this;
				Action action;
				if ((action = <>9__3) == null)
				{
					action = (<>9__3 = delegate()
					{
						this.TriggerEvent("services.sendFriendRequestByUsername.reply", username, null, null, null, null, null);
					});
				}
				engine.RunOnMainThread(<>4__this, action, false, false);
			});
		}

		// Token: 0x060038EE RID: 14574 RVA: 0x00076729 File Offset: 0x00074929
		private void OnLeaveQueue()
		{
			this.App.HytaleServices.LeaveGameQueue(delegate(ClientFailureNotification err)
			{
				Interface.Logger.Warn<ClientFailureNotification>("Failed to leave queue with error {0}", err);
				this.Engine.RunOnMainThread(this, delegate
				{
					this.App.Interface.TriggerEvent("services.queue.error", err.CauseLocalizable, null, null, null, null, null);
				}, false, false);
			}, delegate(ClientSuccessNotification success)
			{
				Interface.Logger.Info("Successfully left queue with token {0}", success.Token);
				this.Engine.RunOnMainThread(this, delegate
				{
					this.TriggerEvent("services.queue.left", null, null, null, null, null, null);
				}, false, false);
			});
		}

		// Token: 0x040018A7 RID: 6311
		public new static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040018A8 RID: 6312
		public const float ReferenceHeight = 1080f;

		// Token: 0x040018A9 RID: 6313
		public readonly App App;

		// Token: 0x040018AA RID: 6314
		public readonly string MainMenuBackgroundImagePath;

		// Token: 0x040018AB RID: 6315
		public readonly StartupView StartupView;

		// Token: 0x040018AC RID: 6316
		public readonly MainMenuView MainMenuView;

		// Token: 0x040018AD RID: 6317
		public readonly GameLoadingView GameLoadingView;

		// Token: 0x040018AE RID: 6318
		public readonly DisconnectionView DisconnectionView;

		// Token: 0x040018AF RID: 6319
		public readonly InGameView InGameView;

		// Token: 0x040018B0 RID: 6320
		public readonly CustomUIProvider InGameCustomUIProvider;

		// Token: 0x040018B1 RID: 6321
		public readonly DevToolsLayer DevToolsLayer;

		// Token: 0x040018B2 RID: 6322
		public readonly DevToolsNotificationPanel DevToolsNotificationPanel;

		// Token: 0x040018B3 RID: 6323
		public readonly ModalDialog ModalDialog;

		// Token: 0x040018B4 RID: 6324
		private InterfaceComponent _currentView;

		// Token: 0x040018B5 RID: 6325
		public readonly QueueStatus QueueStatus;

		// Token: 0x040018B6 RID: 6326
		public readonly SettingsComponent SettingsComponent;

		// Token: 0x040018B7 RID: 6327
		public readonly SocialBar SocialBar;

		// Token: 0x040018BB RID: 6331
		private readonly Dictionary<string, Interface.InterfaceEventHandler> _handlersForInterfaceEvents = new Dictionary<string, Interface.InterfaceEventHandler>();

		// Token: 0x040018BC RID: 6332
		private readonly Dictionary<string, Interface.EngineEventHandler> _handlersForEngineEvents = new Dictionary<string, Interface.EngineEventHandler>();

		// Token: 0x02000CD5 RID: 3285
		private class InterfaceEventHandler
		{
			// Token: 0x04004008 RID: 16392
			public Disposable DisposeGate;

			// Token: 0x04004009 RID: 16393
			public Delegate Callback;
		}

		// Token: 0x02000CD6 RID: 3286
		private class EngineEventHandler
		{
			// Token: 0x0400400A RID: 16394
			public Delegate Callback;
		}
	}
}
