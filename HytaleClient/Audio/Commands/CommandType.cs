using System;

namespace HytaleClient.Audio.Commands
{
	// Token: 0x02000B86 RID: 2950
	public enum CommandType : byte
	{
		// Token: 0x040038DE RID: 14558
		RefreshBanks,
		// Token: 0x040038DF RID: 14559
		RegisterSoundObject,
		// Token: 0x040038E0 RID: 14560
		UnregisterSoundObject,
		// Token: 0x040038E1 RID: 14561
		SetPosition,
		// Token: 0x040038E2 RID: 14562
		SetListenerPosition,
		// Token: 0x040038E3 RID: 14563
		PostEvent,
		// Token: 0x040038E4 RID: 14564
		ActionOnEvent,
		// Token: 0x040038E5 RID: 14565
		SetRTPC
	}
}
