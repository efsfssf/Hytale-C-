using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;

namespace HytaleClient.AssetEditor.Utils
{
	// Token: 0x02000B8E RID: 2958
	public class AssetFileComparer : IComparer<AssetFile>
	{
		// Token: 0x06005B4E RID: 23374 RVA: 0x001C7ED7 File Offset: 0x001C60D7
		public AssetFileComparer(bool ignoreLowerCase)
		{
			this._ignoreCase = ignoreLowerCase;
		}

		// Token: 0x06005B4F RID: 23375 RVA: 0x001C7EE8 File Offset: 0x001C60E8
		public int Compare(AssetFile a, AssetFile b)
		{
			int num = Math.Max(a.PathElements.Length, b.PathElements.Length);
			int i = 0;
			while (i < num)
			{
				bool flag = i >= a.PathElements.Length;
				int result;
				if (flag)
				{
					result = -1;
				}
				else
				{
					bool flag2 = i >= b.PathElements.Length;
					if (flag2)
					{
						result = 1;
					}
					else
					{
						bool flag3 = i == a.PathElements.Length - 1 && !a.IsDirectory;
						bool flag4 = i == b.PathElements.Length - 1 && !b.IsDirectory;
						bool flag5 = flag3 && !flag4;
						if (flag5)
						{
							result = 1;
						}
						else
						{
							bool flag6 = flag4 && !flag3;
							if (flag6)
							{
								result = -1;
							}
							else
							{
								int num2 = string.Compare(a.PathElements[i], b.PathElements[i], this._ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
								bool flag7 = num2 != 0;
								if (!flag7)
								{
									i++;
									continue;
								}
								result = num2;
							}
						}
					}
				}
				return result;
			}
			return string.Compare(a.Path, b.Path, this._ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
		}

		// Token: 0x04003928 RID: 14632
		public static readonly AssetFileComparer Instance = new AssetFileComparer(false);

		// Token: 0x04003929 RID: 14633
		public static readonly AssetFileComparer IgnoreCaseInstance = new AssetFileComparer(true);

		// Token: 0x0400392A RID: 14634
		private readonly bool _ignoreCase;
	}
}
