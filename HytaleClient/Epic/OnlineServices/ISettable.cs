using System;

namespace Epic.OnlineServices
{
	// Token: 0x0200000E RID: 14
	internal interface ISettable<T> where T : struct
	{
		// Token: 0x06000084 RID: 132
		void Set(ref T other);

		// Token: 0x06000085 RID: 133
		void Set(ref T? other);
	}
}
