using System;
using System.Collections.Generic;

namespace HytaleClient.Utils
{
	// Token: 0x020007BC RID: 1980
	public class ComparisonUtils
	{
		// Token: 0x0600335A RID: 13146 RVA: 0x0004F0E0 File Offset: 0x0004D2E0
		public static bool CompareKeyValuePairLists<A, B>(IReadOnlyList<KeyValuePair<string, A>> list1, IReadOnlyList<KeyValuePair<string, B>> list2) where A : class where B : class
		{
			bool flag = list1.Count != list2.Count;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < list1.Count; i++)
				{
					bool flag2 = !list1[i].Key.Equals(list2[i].Key) || !list1[i].Value.Equals(list2[i].Value);
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x0004F190 File Offset: 0x0004D390
		public static Comparison<T> Compose<T>(params Comparison<T>[] comparisons) where T : class
		{
			return delegate(T o1, T o2)
			{
				int num = 0;
				int num2 = 0;
				while (num2 == 0 && num < comparisons.Length)
				{
					num2 = comparisons[num++](o1, o2);
				}
				return num2;
			};
		}
	}
}
