using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTC
{
	// Token: 0x020001D1 RID: 465
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct ParticipantStatusChangedCallbackInfoInternal : ICallbackInfoInternal, IGettable<ParticipantStatusChangedCallbackInfo>, ISettable<ParticipantStatusChangedCallbackInfo>, IDisposable
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00013648 File Offset: 0x00011848
		// (set) Token: 0x06000D69 RID: 3433 RVA: 0x00013669 File Offset: 0x00011869
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0001367C File Offset: 0x0001187C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x00013694 File Offset: 0x00011894
		// (set) Token: 0x06000D6C RID: 3436 RVA: 0x000136B5 File Offset: 0x000118B5
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x000136C8 File Offset: 0x000118C8
		// (set) Token: 0x06000D6E RID: 3438 RVA: 0x000136E9 File Offset: 0x000118E9
		public Utf8String RoomName
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_RoomName, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_RoomName);
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x000136FC File Offset: 0x000118FC
		// (set) Token: 0x06000D70 RID: 3440 RVA: 0x0001371D File Offset: 0x0001191D
		public ProductUserId ParticipantId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_ParticipantId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ParticipantId);
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x00013730 File Offset: 0x00011930
		// (set) Token: 0x06000D72 RID: 3442 RVA: 0x00013748 File Offset: 0x00011948
		public RTCParticipantStatus ParticipantStatus
		{
			get
			{
				return this.m_ParticipantStatus;
			}
			set
			{
				this.m_ParticipantStatus = value;
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x00013754 File Offset: 0x00011954
		// (set) Token: 0x06000D74 RID: 3444 RVA: 0x0001377B File Offset: 0x0001197B
		public ParticipantMetadata[] ParticipantMetadata
		{
			get
			{
				ParticipantMetadata[] result;
				Helper.Get<ParticipantMetadataInternal, ParticipantMetadata>(this.m_ParticipantMetadata, out result, this.m_ParticipantMetadataCount);
				return result;
			}
			set
			{
				Helper.Set<ParticipantMetadata, ParticipantMetadataInternal>(ref value, ref this.m_ParticipantMetadata, out this.m_ParticipantMetadataCount);
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x00013794 File Offset: 0x00011994
		// (set) Token: 0x06000D76 RID: 3446 RVA: 0x000137B5 File Offset: 0x000119B5
		public bool ParticipantInBlocklist
		{
			get
			{
				bool result;
				Helper.Get(this.m_ParticipantInBlocklist, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ParticipantInBlocklist);
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000137C8 File Offset: 0x000119C8
		public void Set(ref ParticipantStatusChangedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.ParticipantId = other.ParticipantId;
			this.ParticipantStatus = other.ParticipantStatus;
			this.ParticipantMetadata = other.ParticipantMetadata;
			this.ParticipantInBlocklist = other.ParticipantInBlocklist;
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00013834 File Offset: 0x00011A34
		public void Set(ref ParticipantStatusChangedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.ParticipantId = other.Value.ParticipantId;
				this.ParticipantStatus = other.Value.ParticipantStatus;
				this.ParticipantMetadata = other.Value.ParticipantMetadata;
				this.ParticipantInBlocklist = other.Value.ParticipantInBlocklist;
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x000138E4 File Offset: 0x00011AE4
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_ParticipantId);
			Helper.Dispose(ref this.m_ParticipantMetadata);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00013923 File Offset: 0x00011B23
		public void Get(out ParticipantStatusChangedCallbackInfo output)
		{
			output = default(ParticipantStatusChangedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000611 RID: 1553
		private IntPtr m_ClientData;

		// Token: 0x04000612 RID: 1554
		private IntPtr m_LocalUserId;

		// Token: 0x04000613 RID: 1555
		private IntPtr m_RoomName;

		// Token: 0x04000614 RID: 1556
		private IntPtr m_ParticipantId;

		// Token: 0x04000615 RID: 1557
		private RTCParticipantStatus m_ParticipantStatus;

		// Token: 0x04000616 RID: 1558
		private uint m_ParticipantMetadataCount;

		// Token: 0x04000617 RID: 1559
		private IntPtr m_ParticipantMetadata;

		// Token: 0x04000618 RID: 1560
		private int m_ParticipantInBlocklist;
	}
}
