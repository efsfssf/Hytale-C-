using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using HytaleClient.AssetEditor.Data;

namespace HytaleClient.AssetEditor.Utils
{
	// Token: 0x02000B8F RID: 2959
	public static class AssetPathUtils
	{
		// Token: 0x06005B51 RID: 23377 RVA: 0x001C802C File Offset: 0x001C622C
		public static string[] GetAssetFilePathElements(string filePath, bool usesSharedAssetFile)
		{
			string[] result;
			if (usesSharedAssetFile)
			{
				string[] array = filePath.Split(new char[]
				{
					'/'
				}, StringSplitOptions.RemoveEmptyEntries);
				Array.Resize<string>(ref array, array.Length + 1);
				string text = array[array.Length - 2];
				int num = text.LastIndexOf('#');
				array[array.Length - 2] = text.Substring(0, num);
				array[array.Length - 1] = text.Substring(num + 1, text.Length - (num + 1));
				result = array;
			}
			else
			{
				result = filePath.Split(new char[]
				{
					'/'
				}, StringSplitOptions.RemoveEmptyEntries);
			}
			return result;
		}

		// Token: 0x06005B52 RID: 23378 RVA: 0x001C80B8 File Offset: 0x001C62B8
		public static string CombinePaths(string path1, string path2)
		{
			bool flag = path1 == "";
			string result;
			if (flag)
			{
				result = path2;
			}
			else
			{
				bool flag2 = path2 == "";
				if (flag2)
				{
					result = path1;
				}
				else
				{
					bool flag3 = path1[path1.Length - 1] == '/' || path2[0] == '/';
					if (flag3)
					{
						result = path1 + path2;
					}
					else
					{
						result = path1 + "/" + path2;
					}
				}
			}
			return result;
		}

		// Token: 0x06005B53 RID: 23379 RVA: 0x001C812C File Offset: 0x001C632C
		public static string GetAssetPathWithCommon(string relativeCommonPath)
		{
			return AssetPathUtils.CombinePaths("Common", relativeCommonPath);
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x001C814C File Offset: 0x001C634C
		public static string GetPathWithoutAssetId(string filePath)
		{
			string[] array = filePath.Split(new char[]
			{
				'#'
			}, StringSplitOptions.RemoveEmptyEntries);
			return string.Join("#", array, 0, array.Length - 1);
		}

		// Token: 0x06005B55 RID: 23381 RVA: 0x001C8184 File Offset: 0x001C6384
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasAnyFileExtension(string filename, string[] allowedFileExtensions)
		{
			foreach (string str in allowedFileExtensions)
			{
				bool flag = filename.EndsWith("." + str);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005B56 RID: 23382 RVA: 0x001C81CC File Offset: 0x001C63CC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAnyDirectory(string filename, string[] allowedDirectories)
		{
			foreach (string value in allowedDirectories)
			{
				bool flag = filename.StartsWith(value);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005B57 RID: 23383 RVA: 0x001C8208 File Offset: 0x001C6408
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAnyDirectory(string filename, List<string> allowedDirectories)
		{
			foreach (string value in allowedDirectories)
			{
				bool flag = filename.StartsWith(value);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005B58 RID: 23384 RVA: 0x001C8268 File Offset: 0x001C6468
		public static string GetAssetIdFromReference(string path, bool isSingleAssetFile)
		{
			return isSingleAssetFile ? Enumerable.Last<string>(path.Split(new char[]
			{
				'#'
			})) : Path.GetFileNameWithoutExtension(path);
		}

		// Token: 0x06005B59 RID: 23385 RVA: 0x001C829C File Offset: 0x001C649C
		public static bool IsAssetTreeRootDirectory(string path)
		{
			return path == "Common" || path == "Server" || path == "Cosmetics/CharacterCreator";
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x001C82D8 File Offset: 0x001C64D8
		public static bool TryGetAssetTreeFolder(string path, out AssetTreeFolder assetTree, bool ignoreCase = false)
		{
			bool result;
			if (ignoreCase)
			{
				result = AssetPathUtils.TryGetAssetTreeFolderIgnoreCase(path, out assetTree);
			}
			else
			{
				bool flag = path.StartsWith("Common/");
				if (flag)
				{
					assetTree = AssetTreeFolder.Common;
					result = true;
				}
				else
				{
					bool flag2 = path.StartsWith("Server/");
					if (flag2)
					{
						assetTree = AssetTreeFolder.Server;
						result = true;
					}
					else
					{
						bool flag3 = path.StartsWith("Cosmetics/CharacterCreator/");
						if (flag3)
						{
							assetTree = AssetTreeFolder.Cosmetics;
							result = true;
						}
						else
						{
							assetTree = AssetTreeFolder.Common;
							result = false;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x001C8348 File Offset: 0x001C6548
		private static bool TryGetAssetTreeFolderIgnoreCase(string path, out AssetTreeFolder assetTree)
		{
			path = path.ToLowerInvariant();
			bool flag = path.StartsWith(AssetPathUtils.PathCommonLowercase + "/");
			bool result;
			if (flag)
			{
				assetTree = AssetTreeFolder.Common;
				result = true;
			}
			else
			{
				bool flag2 = path.StartsWith(AssetPathUtils.PathServerLowercase + "/");
				if (flag2)
				{
					assetTree = AssetTreeFolder.Server;
					result = true;
				}
				else
				{
					bool flag3 = path.StartsWith(AssetPathUtils.PathCosmeticsLowercase + "/");
					if (flag3)
					{
						assetTree = AssetTreeFolder.Cosmetics;
						result = true;
					}
					else
					{
						assetTree = AssetTreeFolder.Common;
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x0400392B RID: 14635
		public const string PathCosmetics = "Cosmetics/CharacterCreator";

		// Token: 0x0400392C RID: 14636
		public const string PathCommon = "Common";

		// Token: 0x0400392D RID: 14637
		public const string PathServer = "Server";

		// Token: 0x0400392E RID: 14638
		public const char PathSeparator = '/';

		// Token: 0x0400392F RID: 14639
		public const string PathSeparatorString = "/";

		// Token: 0x04003930 RID: 14640
		public const char SingleAssetFileIdSeparator = '#';

		// Token: 0x04003931 RID: 14641
		public const string SingleAssetFileIdSeparatorString = "#";

		// Token: 0x04003932 RID: 14642
		public static readonly string PathCosmeticsLowercase = "Cosmetics/CharacterCreator".ToLowerInvariant();

		// Token: 0x04003933 RID: 14643
		public static readonly string PathCommonLowercase = "Common".ToLowerInvariant();

		// Token: 0x04003934 RID: 14644
		public static readonly string PathServerLowercase = "Server".ToLowerInvariant();
	}
}
