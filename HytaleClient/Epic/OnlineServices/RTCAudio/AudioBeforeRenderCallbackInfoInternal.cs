using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000203 RID: 515
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AudioBeforeRenderCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioBeforeRenderCallbackInfo>, ISettable<AudioBeforeRenderCallbackInfo>, IDisposable
	{
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00015B88 File Offset: 0x00013D88
		// (set) Token: 0x06000ED5 RID: 3797 RVA: 0x00015BA9 File Offset: 0x00013DA9
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

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x00015BBC File Offset: 0x00013DBC
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x00015BD4 File Offset: 0x00013DD4
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x00015BF5 File Offset: 0x00013DF5
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

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00015C08 File Offset: 0x00013E08
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x00015C29 File Offset: 0x00013E29
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

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00015C3C File Offset: 0x00013E3C
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x00015C5D File Offset: 0x00013E5D
		public AudioBuffer? Buffer
		{
			get
			{
				AudioBuffer? result;
				Helper.Get<AudioBufferInternal, AudioBuffer>(this.m_Buffer, out result);
				return result;
			}
			set
			{
				Helper.Set<AudioBuffer, AudioBufferInternal>(ref value, ref this.m_Buffer);
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00015C70 File Offset: 0x00013E70
		// (set) Token: 0x06000EDE RID: 3806 RVA: 0x00015C91 File Offset: 0x00013E91
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

		// Token: 0x06000EDF RID: 3807 RVA: 0x00015CA4 File Offset: 0x00013EA4
		public void Set(ref AudioBeforeRenderCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Buffer = other.Buffer;
			this.ParticipantId = other.ParticipantId;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00015CF4 File Offset: 0x00013EF4
		public void Set(ref AudioBeforeRenderCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Buffer = other.Value.Buffer;
				this.ParticipantId = other.Value.ParticipantId;
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00015D77 File Offset: 0x00013F77
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_Buffer);
			Helper.Dispose(ref this.m_ParticipantId);
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00015DB6 File Offset: 0x00013FB6
		public void Get(out AudioBeforeRenderCallbackInfo output)
		{
			output = default(AudioBeforeRenderCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040006B8 RID: 1720
		private IntPtr m_ClientData;

		// Token: 0x040006B9 RID: 1721
		private IntPtr m_LocalUserId;

		// Token: 0x040006BA RID: 1722
		private IntPtr m_RoomName;

		// Token: 0x040006BB RID: 1723
		private IntPtr m_Buffer;

		// Token: 0x040006BC RID: 1724
		private IntPtr m_ParticipantId;
	}
}
