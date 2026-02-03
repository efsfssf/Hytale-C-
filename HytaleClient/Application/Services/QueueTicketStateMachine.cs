using System;
using Hypixel.ProtoPlus;
using HytaleClient.Auth.Proto.Protocol;
using HytaleClient.Core;
using NLog;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF8 RID: 3064
	internal class QueueTicketStateMachine
	{
		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x0600618C RID: 24972 RVA: 0x00201178 File Offset: 0x001FF378
		// (set) Token: 0x0600618D RID: 24973 RVA: 0x00201180 File Offset: 0x001FF380
		public string Game { get; private set; }

		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x0600618E RID: 24974 RVA: 0x00201189 File Offset: 0x001FF389
		// (set) Token: 0x0600618F RID: 24975 RVA: 0x00201191 File Offset: 0x001FF391
		public string Status { get; private set; }

		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x06006190 RID: 24976 RVA: 0x0020119A File Offset: 0x001FF39A
		// (set) Token: 0x06006191 RID: 24977 RVA: 0x002011A2 File Offset: 0x001FF3A2
		public long EstimatedTimeMillis { get; private set; }

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x06006192 RID: 24978 RVA: 0x002011AB File Offset: 0x001FF3AB
		// (set) Token: 0x06006193 RID: 24979 RVA: 0x002011B3 File Offset: 0x001FF3B3
		public string Ticket { get; private set; }

		// Token: 0x06006194 RID: 24980 RVA: 0x002011BC File Offset: 0x001FF3BC
		public QueueTicketStateMachine(App app)
		{
			this._app = app;
			this._engine = app.Engine;
			this.ResetState();
		}

		// Token: 0x06006195 RID: 24981 RVA: 0x002011E0 File Offset: 0x001FF3E0
		public bool TryQueue(string key, sbyte[] extra, out string ticket)
		{
			ticket = null;
			long timestamp = QueueTicketStateMachine.GetTimestamp();
			bool flag = this.Key == key && QueueTicketStateMachine.AreEqual(this.Extra, extra) && this._ticketStartedProcessing + 5000L > timestamp;
			bool result;
			if (flag)
			{
				bool isInfoEnabled = QueueTicketStateMachine.Logger.IsInfoEnabled;
				if (isInfoEnabled)
				{
					QueueTicketStateMachine.Logger.Info("Disregarding queue attempt because I'm already queued for {0} and {1}, and we only most recently tried to queue at {2}. Current time {3}", new object[]
					{
						key,
						extra,
						this._ticketStartedProcessing,
						timestamp
					});
				}
				result = false;
			}
			else
			{
				bool flag2 = this.Ticket == null || (this.Ticket != null && this._ticketAcknowledgedPending && this._ticketAcknowledgedPending && this._ticketStartedProcessing + 5000L < timestamp) || (this.Ticket != null && !this._ticketAcknowledgedPending);
				if (flag2)
				{
					ticket = (this.Ticket = QueueTicketStateMachine.GenerateNewTicket());
					this._ticketStartedProcessing = timestamp;
					this._ticketAcknowledgedPending = true;
					this.Key = key;
					this.Extra = extra;
					this._engine.RunOnMainThread(this._engine, delegate
					{
						this._app.Interface.OnServicesQueueJoined(key);
					}, true, false);
					result = true;
				}
				else
				{
					bool isInfoEnabled2 = QueueTicketStateMachine.Logger.IsInfoEnabled;
					if (isInfoEnabled2)
					{
						QueueTicketStateMachine.Logger.Info("Disregarding queue attempt because didn't match secondary condition. Current state {0} input {1} and {2}. Current time {3}", new object[]
						{
							this,
							key,
							extra,
							timestamp
						});
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06006196 RID: 24982 RVA: 0x00201386 File Offset: 0x001FF586
		public void HandleConnectionClose()
		{
			this.LeaveQueueUI();
		}

		// Token: 0x06006197 RID: 24983 RVA: 0x00201390 File Offset: 0x001FF590
		public void HandleConnectionOpen()
		{
			bool flag = this.Ticket != null;
			if (flag)
			{
			}
		}

		// Token: 0x06006198 RID: 24984 RVA: 0x002013AD File Offset: 0x001FF5AD
		private void ResetState()
		{
			this.Ticket = null;
			this._ticketAcknowledgedPending = false;
			this._ticketStartedProcessing = 0L;
			this.Game = null;
			this.Status = null;
			this.EstimatedTimeMillis = 0L;
		}

		// Token: 0x06006199 RID: 24985 RVA: 0x002013E0 File Offset: 0x001FF5E0
		public void OnLeaveQueueConfirm()
		{
			this.ResetState();
			this.LeaveQueueUI();
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x002013F4 File Offset: 0x001FF5F4
		private void ShowQueueErrorUI(string cause)
		{
			this._engine.RunOnMainThread(this._engine, delegate
			{
				this._app.Interface.OnServicesQueueError(cause);
			}, false, false);
		}

		// Token: 0x0600619B RID: 24987 RVA: 0x00201438 File Offset: 0x001FF638
		private void UpdateQueueStatusUI(string status)
		{
			this._engine.RunOnMainThread(this._engine, delegate
			{
				this._app.Interface.OnServicesQueueStatusUpdate(status);
			}, false, false);
		}

		// Token: 0x0600619C RID: 24988 RVA: 0x00201479 File Offset: 0x001FF679
		private void LeaveQueueUI()
		{
			this._engine.RunOnMainThread(this._engine, delegate
			{
				this._app.Interface.OnServicesQueueLeft();
			}, false, false);
		}

		// Token: 0x0600619D RID: 24989 RVA: 0x0020149C File Offset: 0x001FF69C
		private void JoinQueueUI(string game)
		{
			this._engine.RunOnMainThread(this._engine, delegate
			{
				this._app.Interface.OnServicesQueueJoined(game);
			}, false, false);
		}

		// Token: 0x0600619E RID: 24990 RVA: 0x002014E0 File Offset: 0x001FF6E0
		public void HandleResponse(ClientServerQueueReply reply)
		{
			ProtoSerializable protoSerializable = ServerQueueUtil.ReadResponseFrom(reply);
			bool flag = this.Ticket == null;
			if (flag)
			{
				QueueTicketStateMachine.Logger.Info<ClientServerQueueReply, ProtoSerializable>("Got ticket response when we don't actually have our own ticket set: {0} {1}", reply, protoSerializable);
			}
			else
			{
				bool flag2 = this.Ticket != reply.Ticket;
				if (flag2)
				{
					QueueTicketStateMachine.Logger.Info("Got ticket response with mismatching ticket! Our ticket is {0} but received ticket {1}: {2} {3}", new object[]
					{
						this.Ticket,
						reply.Ticket,
						reply,
						protoSerializable
					});
				}
				else
				{
					bool flag3 = protoSerializable.GetType() == typeof(ClientServerQueueFailure);
					if (flag3)
					{
						ClientServerQueueFailure clientServerQueueFailure = (ClientServerQueueFailure)protoSerializable;
						QueueTicketStateMachine.Logger.Info<ProtoSerializable>("Got queue failure response from server {0}", protoSerializable);
						this.Ticket = null;
						this.ResetState();
						this.ShowQueueErrorUI(clientServerQueueFailure.Cause.ToString());
					}
					else
					{
						bool flag4 = protoSerializable.GetType() == typeof(ClientServerQueueStatus);
						if (flag4)
						{
							QueueTicketStateMachine.Logger.Info<ProtoSerializable>("Received status update for queue {0}", protoSerializable);
							ClientServerQueueStatus clientServerQueueStatus = (ClientServerQueueStatus)protoSerializable;
							bool flag5 = !this._ticketAcknowledgedPending;
							if (flag5)
							{
								QueueTicketStateMachine.Logger.Info<string, string>("Got status update to completed ticket {0}: {1}", this.Ticket, clientServerQueueStatus.Message);
							}
							else
							{
								this.Status = clientServerQueueStatus.Message;
								this.UpdateQueueStatusUI(clientServerQueueStatus.Message);
							}
						}
						else
						{
							bool flag6 = protoSerializable.GetType() == typeof(ClientServerQueueTicket);
							if (flag6)
							{
								QueueTicketStateMachine.Logger.Info<ProtoSerializable>("Received ticket from server for queue {0}", protoSerializable);
								ClientServerQueueTicket clientServerQueueTicket = (ClientServerQueueTicket)protoSerializable;
								this.Game = clientServerQueueTicket.Game;
								this.EstimatedTimeMillis = clientServerQueueTicket.EstimatedTimeMillis;
								bool ticketAcknowledgedPending = this._ticketAcknowledgedPending;
								if (ticketAcknowledgedPending)
								{
									this.JoinQueueUI(this.Game);
								}
							}
							else
							{
								bool flag7 = protoSerializable.GetType() == typeof(ClientServerQueueWorldTransfer);
								if (flag7)
								{
									QueueTicketStateMachine.Logger.Info<ProtoSerializable>("Received world transfer notice {0}", protoSerializable);
									this.ResetState();
									this.LeaveQueueUI();
								}
								else
								{
									bool flag8 = protoSerializable.GetType() == typeof(ClientServerQueueFinal);
									if (!flag8)
									{
										string str = "Illegal response from server queue: ";
										ProtoSerializable protoSerializable2 = protoSerializable;
										throw new Exception(str + ((protoSerializable2 != null) ? protoSerializable2.ToString() : null));
									}
									QueueTicketStateMachine.Logger.Info<ProtoSerializable>("Got queue final response! Time to join with {0}", protoSerializable);
									ClientServerQueueFinal clientServerQueueFinal = (ClientServerQueueFinal)protoSerializable;
									this.ResetState();
									this.LeaveQueueUI();
									this.ConnectToServer(clientServerQueueFinal.Ip, clientServerQueueFinal.Port);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600619F RID: 24991 RVA: 0x00201778 File Offset: 0x001FF978
		private void ConnectToServer(string hostname, int port)
		{
			this._engine.RunOnMainThread(this._engine, delegate
			{
				this._app.Interface.OnServicesQueueLeft();
				bool flag = this._app.Stage == App.AppStage.GameLoading;
				if (flag)
				{
					this._app.GameLoading.Abort();
				}
				this._app.GameLoading.Open(hostname, port, new AppMainMenu.MainMenuPage?(AppMainMenu.MainMenuPage.Home));
				this._app.Interface.FadeIn(null, false);
			}, false, false);
		}

		// Token: 0x060061A0 RID: 24992 RVA: 0x002017C4 File Offset: 0x001FF9C4
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}, {10}: {11}, {12}: {13}, {14}: {15}", new object[]
			{
				"Key",
				this.Key,
				"Extra",
				this.Extra,
				"_ticketAcknowledgedPending",
				this._ticketAcknowledgedPending,
				"_ticketStartedProcessing",
				this._ticketStartedProcessing,
				"Game",
				this.Game,
				"Status",
				this.Status,
				"EstimatedTimeMillis",
				this.EstimatedTimeMillis,
				"Ticket",
				this.Ticket
			});
		}

		// Token: 0x060061A1 RID: 24993 RVA: 0x00201888 File Offset: 0x001FFA88
		private static string GenerateNewTicket()
		{
			return "T" + Guid.NewGuid().ToString();
		}

		// Token: 0x060061A2 RID: 24994 RVA: 0x002018B8 File Offset: 0x001FFAB8
		private static long GetTimestamp()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x002018EC File Offset: 0x001FFAEC
		private static bool AreEqual(sbyte[] a, sbyte[] b)
		{
			bool flag = a == b;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = a == null || b == null;
				result = (!flag2 && QueueTicketStateMachine.HaveSameContents(a, b));
			}
			return result;
		}

		// Token: 0x060061A4 RID: 24996 RVA: 0x00201924 File Offset: 0x001FFB24
		private static bool HaveSameContents(sbyte[] a, sbyte[] b)
		{
			int i = a.Length;
			bool flag = i != b.Length;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				while (i != 0)
				{
					i--;
					bool flag2 = a[i] != b[i];
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04003CB0 RID: 15536
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003CB1 RID: 15537
		private readonly App _app;

		// Token: 0x04003CB2 RID: 15538
		private readonly Engine _engine;

		// Token: 0x04003CB3 RID: 15539
		public string Key;

		// Token: 0x04003CB4 RID: 15540
		public sbyte[] Extra;

		// Token: 0x04003CB9 RID: 15545
		private bool _ticketAcknowledgedPending;

		// Token: 0x04003CBA RID: 15546
		private long _ticketStartedProcessing;
	}
}
