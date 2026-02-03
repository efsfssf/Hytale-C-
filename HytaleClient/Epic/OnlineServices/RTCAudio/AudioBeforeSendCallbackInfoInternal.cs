using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x02000205 RID: 517
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AudioBeforeSendCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioBeforeSendCallbackInfo>, ISettable<AudioBeforeSendCallbackInfo>, IDisposable
	{
		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06000EED RID: 3821 RVA: 0x00015E60 File Offset: 0x00014060
		// (set) Token: 0x06000EEE RID: 3822 RVA: 0x00015E81 File Offset: 0x00014081
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

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06000EEF RID: 3823 RVA: 0x00015E94 File Offset: 0x00014094
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x00015EAC File Offset: 0x000140AC
		// (set) Token: 0x06000EF1 RID: 3825 RVA: 0x00015ECD File Offset: 0x000140CD
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

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06000EF2 RID: 3826 RVA: 0x00015EE0 File Offset: 0x000140E0
		// (set) Token: 0x06000EF3 RID: 3827 RVA: 0x00015F01 File Offset: 0x00014101
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

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x00015F14 File Offset: 0x00014114
		// (set) Token: 0x06000EF5 RID: 3829 RVA: 0x00015F35 File Offset: 0x00014135
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

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00015F46 File Offset: 0x00014146
		public void Set(ref AudioBeforeSendCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Buffer = other.Buffer;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00015F80 File Offset: 0x00014180
		public void Set(ref AudioBeforeSendCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Buffer = other.Value.Buffer;
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00015FEE File Offset: 0x000141EE
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
			Helper.Dispose(ref this.m_Buffer);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00016021 File Offset: 0x00014221
		public void Get(out AudioBeforeSendCallbackInfo output)
		{
			output = default(AudioBeforeSendCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040006C1 RID: 1729
		private IntPtr m_ClientData;

		// Token: 0x040006C2 RID: 1730
		private IntPtr m_LocalUserId;

		// Token: 0x040006C3 RID: 1731
		private IntPtr m_RoomName;

		// Token: 0x040006C4 RID: 1732
		private IntPtr m_Buffer;
	}
}
