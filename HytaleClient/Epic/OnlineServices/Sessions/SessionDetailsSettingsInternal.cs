using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x02000159 RID: 345
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SessionDetailsSettingsInternal : IGettable<SessionDetailsSettings>, ISettable<SessionDetailsSettings>, IDisposable
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0000E7C8 File Offset: 0x0000C9C8
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0000E7E9 File Offset: 0x0000C9E9
		public Utf8String BucketId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_BucketId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_BucketId);
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0000E7FC File Offset: 0x0000C9FC
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x0000E814 File Offset: 0x0000CA14
		public uint NumPublicConnections
		{
			get
			{
				return this.m_NumPublicConnections;
			}
			set
			{
				this.m_NumPublicConnections = value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0000E820 File Offset: 0x0000CA20
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x0000E841 File Offset: 0x0000CA41
		public bool AllowJoinInProgress
		{
			get
			{
				bool result;
				Helper.Get(this.m_AllowJoinInProgress, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_AllowJoinInProgress);
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0000E854 File Offset: 0x0000CA54
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0000E86C File Offset: 0x0000CA6C
		public OnlineSessionPermissionLevel PermissionLevel
		{
			get
			{
				return this.m_PermissionLevel;
			}
			set
			{
				this.m_PermissionLevel = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0000E878 File Offset: 0x0000CA78
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x0000E899 File Offset: 0x0000CA99
		public bool InvitesAllowed
		{
			get
			{
				bool result;
				Helper.Get(this.m_InvitesAllowed, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_InvitesAllowed);
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0000E8AC File Offset: 0x0000CAAC
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0000E8CD File Offset: 0x0000CACD
		public bool SanctionsEnabled
		{
			get
			{
				bool result;
				Helper.Get(this.m_SanctionsEnabled, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_SanctionsEnabled);
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0000E8E0 File Offset: 0x0000CAE0
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x0000E907 File Offset: 0x0000CB07
		public uint[] AllowedPlatformIds
		{
			get
			{
				uint[] result;
				Helper.Get<uint>(this.m_AllowedPlatformIds, out result, this.m_AllowedPlatformIdsCount);
				return result;
			}
			set
			{
				Helper.Set<uint>(value, ref this.m_AllowedPlatformIds, out this.m_AllowedPlatformIdsCount);
			}
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0000E920 File Offset: 0x0000CB20
		public void Set(ref SessionDetailsSettings other)
		{
			this.m_ApiVersion = 4;
			this.BucketId = other.BucketId;
			this.NumPublicConnections = other.NumPublicConnections;
			this.AllowJoinInProgress = other.AllowJoinInProgress;
			this.PermissionLevel = other.PermissionLevel;
			this.InvitesAllowed = other.InvitesAllowed;
			this.SanctionsEnabled = other.SanctionsEnabled;
			this.AllowedPlatformIds = other.AllowedPlatformIds;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0000E990 File Offset: 0x0000CB90
		public void Set(ref SessionDetailsSettings? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 4;
				this.BucketId = other.Value.BucketId;
				this.NumPublicConnections = other.Value.NumPublicConnections;
				this.AllowJoinInProgress = other.Value.AllowJoinInProgress;
				this.PermissionLevel = other.Value.PermissionLevel;
				this.InvitesAllowed = other.Value.InvitesAllowed;
				this.SanctionsEnabled = other.Value.SanctionsEnabled;
				this.AllowedPlatformIds = other.Value.AllowedPlatformIds;
			}
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0000EA47 File Offset: 0x0000CC47
		public void Dispose()
		{
			Helper.Dispose(ref this.m_BucketId);
			Helper.Dispose(ref this.m_AllowedPlatformIds);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0000EA62 File Offset: 0x0000CC62
		public void Get(out SessionDetailsSettings output)
		{
			output = default(SessionDetailsSettings);
			output.Set(ref this);
		}

		// Token: 0x040004B4 RID: 1204
		private int m_ApiVersion;

		// Token: 0x040004B5 RID: 1205
		private IntPtr m_BucketId;

		// Token: 0x040004B6 RID: 1206
		private uint m_NumPublicConnections;

		// Token: 0x040004B7 RID: 1207
		private int m_AllowJoinInProgress;

		// Token: 0x040004B8 RID: 1208
		private OnlineSessionPermissionLevel m_PermissionLevel;

		// Token: 0x040004B9 RID: 1209
		private int m_InvitesAllowed;

		// Token: 0x040004BA RID: 1210
		private int m_SanctionsEnabled;

		// Token: 0x040004BB RID: 1211
		private IntPtr m_AllowedPlatformIds;

		// Token: 0x040004BC RID: 1212
		private uint m_AllowedPlatformIdsCount;
	}
}
