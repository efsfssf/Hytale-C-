using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HytaleClient.Data.Audio;
using HytaleClient.Utils;

namespace HytaleClient.Audio
{
	// Token: 0x02000B83 RID: 2947
	internal class ResourceManager
	{
		// Token: 0x06005ACB RID: 23243 RVA: 0x001C4E24 File Offset: 0x001C3024
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint GetNetworkWwiseId(int value)
		{
			return (uint)(value ^ int.MinValue);
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x001C4E40 File Offset: 0x001C3040
		public void SetupWwiseIds(Dictionary<string, WwiseResource> upcomingWwiseIds)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.WwiseEventIds.Clear();
			this.WwiseGameParameterIds.Clear();
			this.DebugWwiseIds.Clear();
			foreach (KeyValuePair<string, WwiseResource> keyValuePair in upcomingWwiseIds)
			{
				uint id = keyValuePair.Value.Id;
				bool flag = keyValuePair.Value.Type == WwiseResource.WwiseResourceType.Event;
				if (flag)
				{
					this.WwiseEventIds[keyValuePair.Key] = id;
				}
				else
				{
					this.WwiseGameParameterIds[keyValuePair.Key] = id;
				}
				this.DebugWwiseIds[id] = keyValuePair.Key;
			}
		}

		// Token: 0x040038CA RID: 14538
		public readonly ConcurrentDictionary<string, string> FilePathsByFileName = new ConcurrentDictionary<string, string>();

		// Token: 0x040038CB RID: 14539
		public Dictionary<string, uint> WwiseEventIds = new Dictionary<string, uint>();

		// Token: 0x040038CC RID: 14540
		public Dictionary<string, uint> WwiseGameParameterIds = new Dictionary<string, uint>();

		// Token: 0x040038CD RID: 14541
		public Dictionary<uint, string> DebugWwiseIds = new Dictionary<uint, string>();
	}
}
