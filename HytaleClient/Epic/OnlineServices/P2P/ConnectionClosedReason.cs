using System;

namespace Epic.OnlineServices.P2P
{
	// Token: 0x0200077D RID: 1917
	public enum ConnectionClosedReason
	{
		// Token: 0x0400165C RID: 5724
		Unknown,
		// Token: 0x0400165D RID: 5725
		ClosedByLocalUser,
		// Token: 0x0400165E RID: 5726
		ClosedByPeer,
		// Token: 0x0400165F RID: 5727
		TimedOut,
		// Token: 0x04001660 RID: 5728
		TooManyConnections,
		// Token: 0x04001661 RID: 5729
		InvalidMessage,
		// Token: 0x04001662 RID: 5730
		InvalidData,
		// Token: 0x04001663 RID: 5731
		ConnectionFailed,
		// Token: 0x04001664 RID: 5732
		ConnectionClosed,
		// Token: 0x04001665 RID: 5733
		NegotiationFailed,
		// Token: 0x04001666 RID: 5734
		UnexpectedError,
		// Token: 0x04001667 RID: 5735
		ConnectionIgnored
	}
}
