using System;

namespace Epic.OnlineServices.Sessions
{
	// Token: 0x0200014B RID: 331
	public sealed class SessionDetails : Handle
	{
		// Token: 0x06000A09 RID: 2569 RVA: 0x0000DFAC File Offset: 0x0000C1AC
		public SessionDetails()
		{
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0000DFB6 File Offset: 0x0000C1B6
		public SessionDetails(IntPtr innerHandle) : base(innerHandle)
		{
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
		public Result CopyInfo(ref SessionDetailsCopyInfoOptions options, out SessionDetailsInfo? outSessionInfo)
		{
			SessionDetailsCopyInfoOptionsInternal sessionDetailsCopyInfoOptionsInternal = default(SessionDetailsCopyInfoOptionsInternal);
			sessionDetailsCopyInfoOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_SessionDetails_CopyInfo(base.InnerHandle, ref sessionDetailsCopyInfoOptionsInternal, ref zero);
			Helper.Dispose<SessionDetailsCopyInfoOptionsInternal>(ref sessionDetailsCopyInfoOptionsInternal);
			Helper.Get<SessionDetailsInfoInternal, SessionDetailsInfo>(zero, out outSessionInfo);
			bool flag = outSessionInfo != null;
			if (flag)
			{
				Bindings.EOS_SessionDetails_Info_Release(zero);
			}
			return result;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0000E024 File Offset: 0x0000C224
		public Result CopySessionAttributeByIndex(ref SessionDetailsCopySessionAttributeByIndexOptions options, out SessionDetailsAttribute? outSessionAttribute)
		{
			SessionDetailsCopySessionAttributeByIndexOptionsInternal sessionDetailsCopySessionAttributeByIndexOptionsInternal = default(SessionDetailsCopySessionAttributeByIndexOptionsInternal);
			sessionDetailsCopySessionAttributeByIndexOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_SessionDetails_CopySessionAttributeByIndex(base.InnerHandle, ref sessionDetailsCopySessionAttributeByIndexOptionsInternal, ref zero);
			Helper.Dispose<SessionDetailsCopySessionAttributeByIndexOptionsInternal>(ref sessionDetailsCopySessionAttributeByIndexOptionsInternal);
			Helper.Get<SessionDetailsAttributeInternal, SessionDetailsAttribute>(zero, out outSessionAttribute);
			bool flag = outSessionAttribute != null;
			if (flag)
			{
				Bindings.EOS_SessionDetails_Attribute_Release(zero);
			}
			return result;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0000E084 File Offset: 0x0000C284
		public Result CopySessionAttributeByKey(ref SessionDetailsCopySessionAttributeByKeyOptions options, out SessionDetailsAttribute? outSessionAttribute)
		{
			SessionDetailsCopySessionAttributeByKeyOptionsInternal sessionDetailsCopySessionAttributeByKeyOptionsInternal = default(SessionDetailsCopySessionAttributeByKeyOptionsInternal);
			sessionDetailsCopySessionAttributeByKeyOptionsInternal.Set(ref options);
			IntPtr zero = IntPtr.Zero;
			Result result = Bindings.EOS_SessionDetails_CopySessionAttributeByKey(base.InnerHandle, ref sessionDetailsCopySessionAttributeByKeyOptionsInternal, ref zero);
			Helper.Dispose<SessionDetailsCopySessionAttributeByKeyOptionsInternal>(ref sessionDetailsCopySessionAttributeByKeyOptionsInternal);
			Helper.Get<SessionDetailsAttributeInternal, SessionDetailsAttribute>(zero, out outSessionAttribute);
			bool flag = outSessionAttribute != null;
			if (flag)
			{
				Bindings.EOS_SessionDetails_Attribute_Release(zero);
			}
			return result;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
		public uint GetSessionAttributeCount(ref SessionDetailsGetSessionAttributeCountOptions options)
		{
			SessionDetailsGetSessionAttributeCountOptionsInternal sessionDetailsGetSessionAttributeCountOptionsInternal = default(SessionDetailsGetSessionAttributeCountOptionsInternal);
			sessionDetailsGetSessionAttributeCountOptionsInternal.Set(ref options);
			uint result = Bindings.EOS_SessionDetails_GetSessionAttributeCount(base.InnerHandle, ref sessionDetailsGetSessionAttributeCountOptionsInternal);
			Helper.Dispose<SessionDetailsGetSessionAttributeCountOptionsInternal>(ref sessionDetailsGetSessionAttributeCountOptionsInternal);
			return result;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0000E11E File Offset: 0x0000C31E
		public void Release()
		{
			Bindings.EOS_SessionDetails_Release(base.InnerHandle);
		}

		// Token: 0x0400048C RID: 1164
		public const int SessiondetailsAttributeApiLatest = 1;

		// Token: 0x0400048D RID: 1165
		public const int SessiondetailsCopyinfoApiLatest = 1;

		// Token: 0x0400048E RID: 1166
		public const int SessiondetailsCopysessionattributebyindexApiLatest = 1;

		// Token: 0x0400048F RID: 1167
		public const int SessiondetailsCopysessionattributebykeyApiLatest = 1;

		// Token: 0x04000490 RID: 1168
		public const int SessiondetailsGetsessionattributecountApiLatest = 1;

		// Token: 0x04000491 RID: 1169
		public const int SessiondetailsInfoApiLatest = 2;

		// Token: 0x04000492 RID: 1170
		public const int SessiondetailsSettingsApiLatest = 4;
	}
}
