using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCData
{
	// Token: 0x020001DF RID: 479
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct DataReceivedCallbackInfoInternal : ICallbackInfoInternal, IGettable<DataReceivedCallbackInfo>, ISettable<DataReceivedCallbackInfo>, IDisposable
	{
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0001449C File Offset: 0x0001269C
		// (set) Token: 0x06000DDE RID: 3550 RVA: 0x000144BD File Offset: 0x000126BD
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

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x000144D0 File Offset: 0x000126D0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000DE0 RID: 3552 RVA: 0x000144E8 File Offset: 0x000126E8
		// (set) Token: 0x06000DE1 RID: 3553 RVA: 0x00014509 File Offset: 0x00012709
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

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x0001451C File Offset: 0x0001271C
		// (set) Token: 0x06000DE3 RID: 3555 RVA: 0x0001453D File Offset: 0x0001273D
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

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x00014550 File Offset: 0x00012750
		// (set) Token: 0x06000DE5 RID: 3557 RVA: 0x00014577 File Offset: 0x00012777
		public ArraySegment<byte> Data
		{
			get
			{
				ArraySegment<byte> result;
				Helper.Get(this.m_Data, out result, this.m_DataLengthBytes);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Data, out this.m_DataLengthBytes);
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x00014590 File Offset: 0x00012790
		// (set) Token: 0x06000DE7 RID: 3559 RVA: 0x000145B1 File Offset: 0x000127B1
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

		// Token: 0x06000DE8 RID: 3560 RVA: 0x000145C4 File Offset: 0x000127C4
		public void Set(ref DataReceivedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Data = other.Data;
			this.ParticipantId = other.ParticipantId;
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00014614 File Offset: 0x00012814
		public void Set(ref DataReceivedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Data = other.Value.Data;
				this.ParticipantId = other.Value.ParticipantId;
			}
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00014697 File Offset: 0x00012897
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_Data);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x000146D6 File Offset: 0x000128D6
		public void Get(out DataReceivedCallbackInfo output)
		{
			output = default(DataReceivedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400064F RID: 1615
		private IntPtr m_ClientData;

		// Token: 0x04000650 RID: 1616
		private IntPtr m_LocalUserId;

		// Token: 0x04000651 RID: 1617
		private IntPtr m_RoomName;

		// Token: 0x04000652 RID: 1618
		private uint m_DataLengthBytes;

		// Token: 0x04000653 RID: 1619
		private IntPtr m_Data;

		// Token: 0x04000654 RID: 1620
		private IntPtr m_ParticipantId;
	}
}
