using System;

namespace Epic.OnlineServices
{
	// Token: 0x02000009 RID: 9
	internal class CachedArrayAllocationException : AllocationException
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00004009 File Offset: 0x00002209
		public CachedArrayAllocationException(IntPtr address, int foundLength, int expectedLength) : base(string.Format("Cached array allocation has length {0} but expected {1} at {2}", foundLength, expectedLength, address.ToString("X")))
		{
		}
	}
}
