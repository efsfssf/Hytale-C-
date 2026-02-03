using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Presence
{
	// Token: 0x020002D5 RID: 725
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct PresenceModificationDataRecordIdInternal : IGettable<PresenceModificationDataRecordId>, ISettable<PresenceModificationDataRecordId>, IDisposable
	{
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0001D548 File Offset: 0x0001B748
		// (set) Token: 0x06001411 RID: 5137 RVA: 0x0001D569 File Offset: 0x0001B769
		public Utf8String Key
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Key, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Key);
			}
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0001D579 File Offset: 0x0001B779
		public void Set(ref PresenceModificationDataRecordId other)
		{
			this.m_ApiVersion = 1;
			this.Key = other.Key;
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0001D590 File Offset: 0x0001B790
		public void Set(ref PresenceModificationDataRecordId? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Key = other.Value.Key;
			}
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0001D5C6 File Offset: 0x0001B7C6
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Key);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0001D5D5 File Offset: 0x0001B7D5
		public void Get(out PresenceModificationDataRecordId output)
		{
			output = default(PresenceModificationDataRecordId);
			output.Set(ref this);
		}

		// Token: 0x040008CF RID: 2255
		private int m_ApiVersion;

		// Token: 0x040008D0 RID: 2256
		private IntPtr m_Key;
	}
}
