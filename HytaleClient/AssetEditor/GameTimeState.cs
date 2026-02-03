using System;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.AssetEditor
{
	// Token: 0x02000B8D RID: 2957
	internal class GameTimeState
	{
		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06005B3C RID: 23356 RVA: 0x001C7C23 File Offset: 0x001C5E23
		// (set) Token: 0x06005B3D RID: 23357 RVA: 0x001C7C2B File Offset: 0x001C5E2B
		public DateTime Time { get; private set; }

		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06005B3E RID: 23358 RVA: 0x001C7C34 File Offset: 0x001C5E34
		// (set) Token: 0x06005B3F RID: 23359 RVA: 0x001C7C3C File Offset: 0x001C5E3C
		public float GameDayProgressInHours { get; private set; }

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06005B40 RID: 23360 RVA: 0x001C7C45 File Offset: 0x001C5E45
		// (set) Token: 0x06005B41 RID: 23361 RVA: 0x001C7C4D File Offset: 0x001C5E4D
		public bool IsPaused { get; private set; }

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x06005B42 RID: 23362 RVA: 0x001C7C56 File Offset: 0x001C5E56
		// (set) Token: 0x06005B43 RID: 23363 RVA: 0x001C7C5E File Offset: 0x001C5E5E
		public bool IsLocked { get; private set; }

		// Token: 0x06005B44 RID: 23364 RVA: 0x001C7C67 File Offset: 0x001C5E67
		public GameTimeState(AssetEditorApp app)
		{
			this._app = app;
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x001C7C83 File Offset: 0x001C5E83
		public void Cleanup()
		{
			this.Time = DateTime.MinValue;
			this.IsPaused = false;
			this.SecondsPerGameDay = 1800;
			this.GameDayProgressInHours = 0f;
			this.IsLocked = false;
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x001C7CBC File Offset: 0x001C5EBC
		public void ProcessServerTimeUpdate(InstantData gameTime, bool isPaused)
		{
			this.IsPaused = isPaused;
			DateTime gameTime2 = TimeHelper.InstantDataToDateTime(gameTime);
			this.SetGameTime(gameTime2);
			this._app.Interface.AssetEditor.ConfigEditorContextPane.DayTimeControls.OnGameTimePauseUpdated(isPaused);
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x001C7D04 File Offset: 0x001C5F04
		public void SetTimeOverride(float time)
		{
			DateTime time2 = this.Time;
			DateTime d = new DateTime(time2.Year, time2.Month, time2.Day, 0, 0, 0, DateTimeKind.Utc);
			this.SetGameTime(d + TimeSpan.FromTicks((long)(time * 8.64E+11f)));
			this._app.Editor.Backend.SetGameTime(this.Time, this.IsPaused);
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x001C7D75 File Offset: 0x001C5F75
		public void SetTimePaused(bool paused)
		{
			this.IsPaused = paused;
			this._app.Editor.Backend.SetGameTime(this.Time, this.IsPaused);
		}

		// Token: 0x06005B49 RID: 23369 RVA: 0x001C7DA2 File Offset: 0x001C5FA2
		public void SetLocked(bool locked)
		{
			this.IsLocked = locked;
			this._app.Editor.Backend.SetWeatherAndTimeLock(locked);
		}

		// Token: 0x06005B4A RID: 23370 RVA: 0x001C7DC4 File Offset: 0x001C5FC4
		public void ModifyDayOverride(int dayModifier)
		{
			bool flag = dayModifier <= -1 && this.Time < GameTimeState.SecondDay;
			if (!flag)
			{
				bool flag2 = dayModifier >= 1 && this.Time > GameTimeState.PenultimateDay;
				if (!flag2)
				{
					this.Time = this.Time.AddDays((double)dayModifier);
					this._app.Editor.Backend.SetGameTime(this.Time, this.IsPaused);
				}
			}
		}

		// Token: 0x06005B4B RID: 23371 RVA: 0x001C7E45 File Offset: 0x001C6045
		private void SetGameTime(DateTime gameTime)
		{
			this.Time = gameTime;
			this.GameDayProgressInHours = TimeHelper.GetDayProgressInHours(gameTime);
		}

		// Token: 0x06005B4C RID: 23372 RVA: 0x001C7E60 File Offset: 0x001C6060
		public void OnNewFrame(float deltaTime)
		{
			bool isPaused = this.IsPaused;
			if (!isPaused)
			{
				this.SetGameTime(TimeHelper.IncrementDateTimeBySeconds(this.Time, deltaTime, this.SecondsPerGameDay));
			}
		}

		// Token: 0x0400391F RID: 14623
		private const int DefaultSecondsPerGameDay = 1800;

		// Token: 0x04003920 RID: 14624
		private static readonly DateTime PenultimateDay = TimeHelper.MaxTime.AddDays(-1.0);

		// Token: 0x04003921 RID: 14625
		private static readonly DateTime SecondDay = TimeHelper.ZeroYear.AddDays(1.0);

		// Token: 0x04003925 RID: 14629
		public int SecondsPerGameDay = 1800;

		// Token: 0x04003927 RID: 14631
		private readonly AssetEditorApp _app;
	}
}
