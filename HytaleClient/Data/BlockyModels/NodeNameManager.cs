using System;
using System.Collections.Concurrent;
using System.Threading;

namespace HytaleClient.Data.BlockyModels
{
	// Token: 0x02000B74 RID: 2932
	internal class NodeNameManager
	{
		// Token: 0x06005A0D RID: 23053 RVA: 0x001BF6D4 File Offset: 0x001BD8D4
		public static NodeNameManager Copy(NodeNameManager other)
		{
			return new NodeNameManager
			{
				_nextIndex = other._nextIndex,
				_nodeIdsByName = new ConcurrentDictionary<string, int>(other._nodeIdsByName),
				_nodeNamesByIds = new ConcurrentDictionary<int, string>(other._nodeNamesByIds)
			};
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x001BF71C File Offset: 0x001BD91C
		public bool TryGetNameFromId(int nodeNameId, out string nodeName)
		{
			return this._nodeNamesByIds.TryGetValue(nodeNameId, ref nodeName);
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x001BF73C File Offset: 0x001BD93C
		public int GetOrAddNameId(string nodeName)
		{
			int num;
			bool flag = !this._nodeIdsByName.TryGetValue(nodeName, ref num);
			if (flag)
			{
				int nextIndex;
				do
				{
					nextIndex = this._nextIndex;
					num = nextIndex + 1;
				}
				while (nextIndex != Interlocked.CompareExchange(ref this._nextIndex, num, nextIndex));
				this._nodeIdsByName[nodeName] = num;
				this._nodeNamesByIds[num] = nodeName;
			}
			return num;
		}

		// Token: 0x04003849 RID: 14409
		private int _nextIndex;

		// Token: 0x0400384A RID: 14410
		private ConcurrentDictionary<int, string> _nodeNamesByIds = new ConcurrentDictionary<int, string>();

		// Token: 0x0400384B RID: 14411
		private ConcurrentDictionary<string, int> _nodeIdsByName = new ConcurrentDictionary<string, int>();
	}
}
