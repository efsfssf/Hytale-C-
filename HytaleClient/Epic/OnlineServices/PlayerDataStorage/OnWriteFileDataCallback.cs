using System;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000313 RID: 787
	// (Invoke) Token: 0x06001561 RID: 5473
	public delegate WriteResult OnWriteFileDataCallback(ref WriteFileDataCallbackInfo data, out ArraySegment<byte> outDataBuffer);
}
