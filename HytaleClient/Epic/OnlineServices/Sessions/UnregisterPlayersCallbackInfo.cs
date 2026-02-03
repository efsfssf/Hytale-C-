using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200018D RID: 397
	public struct UnregisterPlayersCallbackInfo : ICallbackInfo
	{
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x00010FC5 File Offset: 0x0000F1C5
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x00010FCD File Offset: 0x0000F1CD
		public Result ResultCode { get; set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x00010FD6 File Offset: 0x0000F1D6
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x00010FDE File Offset: 0x0000F1DE
		public object ClientData { get; set; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x00010FE7 File Offset: 0x0000F1E7
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x00010FEF File Offset: 0x0000F1EF
		public ProductUserId[] UnregisteredPlayers { get; set; }

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00010FF8 File Offset: 0x0000F1F8
		public Result? GetResultCode()
		{
			return new Result?(this.ResultCode);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00011015 File Offset: 0x0000F215
		internal void Set(ref UnregisterPlayersCallbackInfoInternal other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.UnregisteredPlayers = other.UnregisteredPlayers;
		}
	}
}
