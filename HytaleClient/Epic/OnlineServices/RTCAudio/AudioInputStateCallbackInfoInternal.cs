using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.RTCAudio
{
	// Token: 0x0200020D RID: 525
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct AudioInputStateCallbackInfoInternal : ICallbackInfoInternal, IGettable<AudioInputStateCallbackInfo>, ISettable<AudioInputStateCallbackInfo>, IDisposable
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x00016508 File Offset: 0x00014708
		// (set) Token: 0x06000F32 RID: 3890 RVA: 0x00016529 File Offset: 0x00014729
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

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x0001653C File Offset: 0x0001473C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x00016554 File Offset: 0x00014754
		// (set) Token: 0x06000F35 RID: 3893 RVA: 0x00016575 File Offset: 0x00014775
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

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x00016588 File Offset: 0x00014788
		// (set) Token: 0x06000F37 RID: 3895 RVA: 0x000165A9 File Offset: 0x000147A9
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

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x000165BC File Offset: 0x000147BC
		// (set) Token: 0x06000F39 RID: 3897 RVA: 0x000165D4 File Offset: 0x000147D4
		public RTCAudioInputStatus Status
		{
			get
			{
				return this.m_Status;
			}
			set
			{
				this.m_Status = value;
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x000165DE File Offset: 0x000147DE
		public void Set(ref AudioInputStateCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.RoomName = other.RoomName;
			this.Status = other.Status;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00016618 File Offset: 0x00014818
		public void Set(ref AudioInputStateCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.RoomName = other.Value.RoomName;
				this.Status = other.Value.Status;
			}
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00016686 File Offset: 0x00014886
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_RoomName);
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x000166AD File Offset: 0x000148AD
		public void Get(out AudioInputStateCallbackInfo output)
		{
			output = default(AudioInputStateCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040006DA RID: 1754
		private IntPtr m_ClientData;

		// Token: 0x040006DB RID: 1755
		private IntPtr m_LocalUserId;

		// Token: 0x040006DC RID: 1756
		private IntPtr m_RoomName;

		// Token: 0x040006DD RID: 1757
		private RTCAudioInputStatus m_Status;
	}
}
