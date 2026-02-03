using System;
using HytaleClient.Protocol;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD1 RID: 2769
	public class DebugSettings
	{
		// Token: 0x06005764 RID: 22372 RVA: 0x001A7BA4 File Offset: 0x001A5DA4
		public DebugSettings Clone()
		{
			return new DebugSettings
			{
				ShowDebugMarkers = this.ShowDebugMarkers
			};
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x001A7BC8 File Offset: 0x001A5DC8
		public SyncPlayerPreferences CreatePacket()
		{
			return new SyncPlayerPreferences(this.ShowDebugMarkers);
		}

		// Token: 0x0400351E RID: 13598
		public bool ShowDebugMarkers = false;
	}
}
