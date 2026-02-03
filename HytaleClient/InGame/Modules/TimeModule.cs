using System;
using System.Collections.Generic;
using HytaleClient.InGame.Commands;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008FC RID: 2300
	internal class TimeModule : Module
	{
		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06004493 RID: 17555 RVA: 0x000EA310 File Offset: 0x000E8510
		public float OperationTimeoutThreshold
		{
			get
			{
				return (float)Math.Round(this._pingInfoArray[2].Ping.Average.Val * 1.2000000476837158) * 0.001f + 75f;
			}
		}

		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x06004494 RID: 17556 RVA: 0x000EA349 File Offset: 0x000E8549
		public long StatTimeoutThreshold
		{
			get
			{
				return (long)(Math.Round(this._pingInfoArray[2].Ping.Average.Val) * 0.0010000000474974513 + 50.0);
			}
		}

		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x06004495 RID: 17557 RVA: 0x000EA380 File Offset: 0x000E8580
		// (set) Token: 0x06004496 RID: 17558 RVA: 0x000EA388 File Offset: 0x000E8588
		public bool IsEditorTimeOverrideActive { get; private set; }

		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x000EA391 File Offset: 0x000E8591
		// (set) Token: 0x06004498 RID: 17560 RVA: 0x000EA399 File Offset: 0x000E8599
		public DateTime GameTime { get; private set; }

		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x06004499 RID: 17561 RVA: 0x000EA3A2 File Offset: 0x000E85A2
		// (set) Token: 0x0600449A RID: 17562 RVA: 0x000EA3AA File Offset: 0x000E85AA
		public float GameDayProgressInHours { get; private set; }

		// Token: 0x0600449B RID: 17563 RVA: 0x000EA3B4 File Offset: 0x000E85B4
		public TimeModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._gameInstance.RegisterCommand("ping", new GameInstance.Command(this.PingCommand));
			for (int i = 0; i < this._pingInfoArray.Length; i++)
			{
				this._pingInfoArray[i] = new TimeModule.PingInfo(i);
			}
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x000EA43C File Offset: 0x000E863C
		public void ProcessGameTimeFromServer(InstantData gameTime)
		{
			this._lastServerInstantData = gameTime;
			bool isEditorTimeOverrideActive = this.IsEditorTimeOverrideActive;
			if (!isEditorTimeOverrideActive)
			{
				DateTime gameTime2 = TimeHelper.InstantDataToDateTime(gameTime);
				this.UpdateGameTime(gameTime2);
			}
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x000EA46C File Offset: 0x000E866C
		public void ProcessEditorTimeOverride(InstantData gameTime, bool isPaused)
		{
			this.IsEditorTimeOverrideActive = true;
			this._isTimePausedByEditor = isPaused;
			DateTime gameTime2 = TimeHelper.InstantDataToDateTime(gameTime);
			this.UpdateGameTime(gameTime2);
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x000EA498 File Offset: 0x000E8698
		public void ProcessClearEditorTimeOverride()
		{
			this.IsEditorTimeOverrideActive = false;
			this._isTimePausedByEditor = false;
			DateTime gameTime = TimeHelper.InstantDataToDateTime(this._lastServerInstantData);
			this.UpdateGameTime(gameTime);
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x000EA4C9 File Offset: 0x000E86C9
		private void UpdateGameTime(DateTime gameTime)
		{
			this.GameTime = gameTime;
			this.GameDayProgressInHours = TimeHelper.GetDayProgressInHours(gameTime);
			this._gameInstance.WeatherModule.UpdateMoonPhase();
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x000EA4F4 File Offset: 0x000E86F4
		public double GetAveragePing(PongType type)
		{
			return this._pingInfoArray[type].Ping.Average.Val;
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x000EA524 File Offset: 0x000E8724
		[Obsolete]
		public override void Tick()
		{
			bool flag = this._isTimePausedByEditor || (this.IsServerTimePaused && !this.IsEditorTimeOverrideActive);
			if (!flag)
			{
				this.UpdateGameTime(TimeHelper.IncrementDateTimeBySeconds(this.GameTime, 0.016666668f, TimeModule.SecondsPerGameDay));
			}
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x000EA573 File Offset: 0x000E8773
		[Obsolete]
		public override void OnNewFrame(float deltaTime)
		{
			this._estimatedDateTime += TimeSpan.FromSeconds((double)deltaTime);
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x000EA590 File Offset: 0x000E8790
		public void UpdatePing(InstantData serverTime, DateTime now, PongType type, int lastPingValueMicro)
		{
			DateTime serverDateTime = TimeHelper.InstantDataToDateTime(serverTime);
			this._pingInfoArray[type].StoreData(serverDateTime, now, lastPingValueMicro);
			bool flag = type != 2;
			if (!flag)
			{
				this._serverDateTime = serverDateTime;
				this._estimatedDateTime = this._serverDateTime;
			}
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x000EA5DC File Offset: 0x000E87DC
		[Usage("ping", new string[]
		{

		})]
		[Description("Prints latency information to the server")]
		private void PingCommand(string[] args)
		{
			string text = "Ping:\n           Min  /  Avg  /  Max\n";
			foreach (TimeModule.PingInfo pingInfo in this._pingInfoArray)
			{
				string[] array = new string[15];
				array[0] = text;
				array[1] = pingInfo.Type.ToString();
				array[2] = ":\n  Ping:   ";
				int num = 3;
				Metric metric = pingInfo.Ping;
				array[num] = TimeHelper.FormatMicros(metric.Min);
				array[4] = " / ";
				int num2 = 5;
				AverageCollector average = pingInfo.Ping.Average;
				array[num2] = TimeHelper.FormatMicros((long)average.Val);
				array[6] = " / ";
				int num3 = 7;
				metric = pingInfo.Ping;
				array[num3] = TimeHelper.FormatMicros(metric.Max);
				array[8] = "\n  Midway: ";
				int num4 = 9;
				metric = pingInfo.PingMidway;
				array[num4] = TimeHelper.FormatMicros(metric.Min);
				array[10] = " / ";
				int num5 = 11;
				average = pingInfo.PingMidway.Average;
				array[num5] = TimeHelper.FormatMicros((long)average.Val);
				array[12] = " / ";
				int num6 = 13;
				metric = pingInfo.PingMidway;
				array[num6] = TimeHelper.FormatMicros(metric.Max);
				array[14] = "\n";
				text = string.Concat(array);
			}
			this._gameInstance.Chat.Log(text);
		}

		// Token: 0x04002209 RID: 8713
		public static int SecondsPerGameDay = 1800;

		// Token: 0x0400220A RID: 8714
		private readonly TimeModule.PingInfo[] _pingInfoArray = new TimeModule.PingInfo[Enum.GetValues(typeof(PongType)).Length];

		// Token: 0x0400220B RID: 8715
		private DateTime _estimatedDateTime = DateTime.MinValue;

		// Token: 0x0400220C RID: 8716
		private DateTime _serverDateTime;

		// Token: 0x0400220E RID: 8718
		public bool IsServerTimePaused;

		// Token: 0x04002211 RID: 8721
		private bool _isTimePausedByEditor;

		// Token: 0x04002212 RID: 8722
		private InstantData _lastServerInstantData;

		// Token: 0x02000DCA RID: 3530
		private struct PingInfo
		{
			// Token: 0x0600664A RID: 26186 RVA: 0x002139B8 File Offset: 0x00211BB8
			public PingInfo(PongType type)
			{
				this.Type = type;
				this._pingHistory = new List<long>();
				this._pingMidwayHistory = new List<long>();
				this.Ping = new Metric(default(AverageCollector));
				this.PingMidway = new Metric(default(AverageCollector));
			}

			// Token: 0x0600664B RID: 26187 RVA: 0x00213A0C File Offset: 0x00211C0C
			public void StoreData(DateTime serverDateTime, DateTime now, int lastPingValueMicros)
			{
				TimeModule.PingInfo.StoreData(this._pingHistory, ref this.Ping, (long)lastPingValueMicros);
				TimeModule.PingInfo.StoreData(this._pingMidwayHistory, ref this.PingMidway, (now - serverDateTime).Ticks / 10L);
			}

			// Token: 0x0600664C RID: 26188 RVA: 0x00213A54 File Offset: 0x00211C54
			private static void StoreData(IList<long> history, ref Metric metric, long value)
			{
				history.Add(value);
				metric.Add(value);
				bool flag = history.Count <= 10;
				if (!flag)
				{
					long value2 = history[0];
					history.RemoveAt(0);
					metric.Remove(value2);
				}
			}

			// Token: 0x0600664D RID: 26189 RVA: 0x00213AA0 File Offset: 0x00211CA0
			public override string ToString()
			{
				return string.Format("{0}: {1}, {2}: {3}, {4}: {5}", new object[]
				{
					"Type",
					this.Type,
					"Ping",
					this.Ping,
					"PingMidway",
					this.PingMidway
				});
			}

			// Token: 0x040043F3 RID: 17395
			private const int PingHistoryLength = 10;

			// Token: 0x040043F4 RID: 17396
			public readonly PongType Type;

			// Token: 0x040043F5 RID: 17397
			private readonly List<long> _pingHistory;

			// Token: 0x040043F6 RID: 17398
			private readonly List<long> _pingMidwayHistory;

			// Token: 0x040043F7 RID: 17399
			public Metric Ping;

			// Token: 0x040043F8 RID: 17400
			public Metric PingMidway;
		}
	}
}
