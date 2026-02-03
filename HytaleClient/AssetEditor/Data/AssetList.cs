using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BD6 RID: 3030
	public class AssetList
	{
		// Token: 0x06005F6C RID: 24428 RVA: 0x001ECEDA File Offset: 0x001EB0DA
		public AssetList(AssetTypeRegistry assetTypeRegistry)
		{
			this._assetTypeRegistry = assetTypeRegistry;
		}

		// Token: 0x06005F6D RID: 24429 RVA: 0x001ECF0C File Offset: 0x001EB10C
		public void SetupAssets(List<AssetFile> serverAssetFiles, List<AssetFile> commonAssetFiles, List<AssetFile> cosmeticAssetFiles)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this._cosmeticAssets = cosmeticAssetFiles;
			this._serverAssets = serverAssetFiles;
			this._commonAssets = commonAssetFiles;
		}

		// Token: 0x06005F6E RID: 24430 RVA: 0x001ECF30 File Offset: 0x001EB130
		public bool TryGetDirectory(string path, out AssetFile assetFile, bool ignoreCase = false)
		{
			AssetTreeFolder assetTree;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTree, ignoreCase);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryGetDirectory: Invalid folder path " + path);
				assetFile = null;
				result = false;
			}
			else
			{
				List<AssetFile> assets = this.GetAssets(assetTree);
				int directoryIndex = AssetList.GetDirectoryIndex(assets, path, ignoreCase);
				bool flag2 = directoryIndex > -1;
				if (flag2)
				{
					assetFile = assets[directoryIndex];
					result = true;
				}
				else
				{
					assetFile = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005F6F RID: 24431 RVA: 0x001ECFA4 File Offset: 0x001EB1A4
		public bool TryGetDirectoryIndex(string path, out int index, bool ignoreCase = false)
		{
			AssetTreeFolder assetTree;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTree, ignoreCase);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryGetDirectoryIndex: Invalid folder path " + path);
				index = -1;
				result = false;
			}
			else
			{
				List<AssetFile> assets = this.GetAssets(assetTree);
				index = AssetList.GetDirectoryIndex(assets, path, ignoreCase);
				result = (index > -1);
			}
			return result;
		}

		// Token: 0x06005F70 RID: 24432 RVA: 0x001ECFFC File Offset: 0x001EB1FC
		private static int GetDirectoryIndex(List<AssetFile> files, string path, bool ignoreCase)
		{
			AssetFile item = AssetFile.CreateDirectory(null, path, AssetPathUtils.GetAssetFilePathElements(path, false));
			return files.BinarySearch(item, ignoreCase ? AssetFileComparer.IgnoreCaseInstance : AssetFileComparer.Instance);
		}

		// Token: 0x06005F71 RID: 24433 RVA: 0x001ED034 File Offset: 0x001EB234
		public bool TryGetFile(string path, out AssetFile assetFile, bool ignoreCase = false)
		{
			AssetTreeFolder assetTreeFolder;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTreeFolder, ignoreCase);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryGetFile: Invalid folder path " + path);
				assetFile = null;
				result = false;
			}
			else
			{
				List<AssetFile> assets = this.GetAssets(assetTreeFolder);
				int assetFileIndex = AssetList.GetAssetFileIndex(assets, path, ignoreCase, assetTreeFolder == AssetTreeFolder.Cosmetics);
				bool flag2 = assetFileIndex > -1;
				if (flag2)
				{
					assetFile = assets[assetFileIndex];
					result = true;
				}
				else
				{
					assetFile = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005F72 RID: 24434 RVA: 0x001ED0AC File Offset: 0x001EB2AC
		public bool TryGetFileIndex(string path, out int index, bool ignoreCase = false)
		{
			AssetTreeFolder assetTreeFolder;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTreeFolder, ignoreCase);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryGetFile: Invalid folder path " + path);
				index = -1;
				result = false;
			}
			else
			{
				List<AssetFile> assets = this.GetAssets(assetTreeFolder);
				index = AssetList.GetAssetFileIndex(assets, path, ignoreCase, assetTreeFolder == AssetTreeFolder.Cosmetics);
				result = (index > -1);
			}
			return result;
		}

		// Token: 0x06005F73 RID: 24435 RVA: 0x001ED108 File Offset: 0x001EB308
		private static int GetAssetFileIndex(List<AssetFile> files, string path, bool ignoreCase, bool usesSharedAssetFile)
		{
			AssetFile item = AssetFile.CreateFile(null, path, null, AssetPathUtils.GetAssetFilePathElements(path, usesSharedAssetFile));
			return files.BinarySearch(item, ignoreCase ? AssetFileComparer.IgnoreCaseInstance : AssetFileComparer.Instance);
		}

		// Token: 0x06005F74 RID: 24436 RVA: 0x001ED140 File Offset: 0x001EB340
		public bool TryGetAsset(string path, out AssetFile assetFile, bool ignoreCase = false)
		{
			bool flag = this.TryGetFile(path, out assetFile, ignoreCase);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this.TryGetDirectory(path, out assetFile, ignoreCase);
				if (flag2)
				{
					result = true;
				}
				else
				{
					assetFile = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005F75 RID: 24437 RVA: 0x001ED178 File Offset: 0x001EB378
		public List<AssetFile> GetAssets(AssetTreeFolder assetTree)
		{
			List<AssetFile> result;
			switch (assetTree)
			{
			case AssetTreeFolder.Server:
				result = this._serverAssets;
				break;
			case AssetTreeFolder.Common:
				result = this._commonAssets;
				break;
			case AssetTreeFolder.Cosmetics:
				result = this._cosmeticAssets;
				break;
			default:
				throw new Exception("Invalid Asset folder: " + assetTree.ToString());
			}
			return result;
		}

		// Token: 0x06005F76 RID: 24438 RVA: 0x001ED1D8 File Offset: 0x001EB3D8
		public bool TryReplaceDirectoryContents(string path, List<AssetFile> newAssetFiles)
		{
			AssetTreeFolder assetTree;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTree, false);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryReplaceDirectoryContents: Invalid folder path " + path);
				result = false;
			}
			else
			{
				int index;
				int num;
				bool flag2 = !this.TryGetDirectoryContentsBounds(path, out index, out num);
				if (flag2)
				{
					result = false;
				}
				else
				{
					List<AssetFile> assets = this.GetAssets(assetTree);
					bool flag3 = num > 0;
					if (flag3)
					{
						assets.RemoveRange(index, num);
					}
					assets.InsertRange(index, newAssetFiles);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005F77 RID: 24439 RVA: 0x001ED258 File Offset: 0x001EB458
		public bool TryInsertDirectory(string path)
		{
			AssetTreeFolder assetTree;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTree, false);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryInsertDirectory: Invalid folder path {0}", path);
				result = false;
			}
			else
			{
				string[] array = path.Split(new char[]
				{
					'/'
				});
				string directoryPath = string.Join('/'.ToString(), array, 0, array.Length - 1);
				List<AssetFile> assets = this.GetAssets(assetTree);
				int index;
				int num;
				bool flag2 = !this.TryGetDirectoryContentsBounds(directoryPath, out index, out num);
				if (flag2)
				{
					result = false;
				}
				else
				{
					string id = Enumerable.Last<string>(array);
					assets.Insert(index, AssetFile.CreateDirectory(id, path, path.Split(new char[]
					{
						'/'
					})));
					assets.Sort(index, num + 1, AssetFileComparer.Instance);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005F78 RID: 24440 RVA: 0x001ED320 File Offset: 0x001EB520
		public bool TryMoveDirectory(string oldPath, string newPath, out Dictionary<string, AssetFile> renamedAssets)
		{
			renamedAssets = null;
			AssetTreeFolder assetTreeFolder;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(newPath, out assetTreeFolder, false);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryMoveDirectory: Invalid new folder path " + newPath);
				result = false;
			}
			else
			{
				AssetTreeFolder assetTreeFolder2;
				bool flag2 = !AssetPathUtils.TryGetAssetTreeFolder(oldPath, out assetTreeFolder2, false);
				if (flag2)
				{
					AssetList.Logger.Warn("TryMoveDirectory: Invalid old folder path " + oldPath);
					result = false;
				}
				else
				{
					bool flag3 = assetTreeFolder != assetTreeFolder2;
					if (flag3)
					{
						AssetList.Logger.Warn("TryMoveDirectory: Directory cannot be moved to another asset tree");
						result = false;
					}
					else
					{
						List<AssetFile> assets = this.GetAssets(assetTreeFolder);
						int directoryIndex = AssetList.GetDirectoryIndex(assets, oldPath, false);
						bool flag4 = directoryIndex < 0;
						if (flag4)
						{
							result = false;
						}
						else
						{
							string[] assetFilePathElements = AssetPathUtils.GetAssetFilePathElements(newPath, false);
							assets[directoryIndex] = AssetFile.CreateDirectory(Path.GetFileName(newPath), newPath, assetFilePathElements);
							renamedAssets = new Dictionary<string, AssetFile>();
							for (int i = directoryIndex + 1; i < assets.Count; i++)
							{
								AssetFile assetFile = assets[i];
								bool flag5 = !assetFile.Path.StartsWith(oldPath + "/");
								if (flag5)
								{
									break;
								}
								string path = assetFile.Path;
								assetFile.Path = newPath + assetFile.Path.Substring(oldPath.Length);
								string text;
								assetFile.AssetType = (this._assetTypeRegistry.TryGetAssetTypeFromPath(assetFile.Path, out text) ? text : null);
								assetFile.PathElements = AssetPathUtils.GetAssetFilePathElements(assetFile.Path, !assetFile.IsDirectory && assetTreeFolder == AssetTreeFolder.Cosmetics);
								renamedAssets.Add(path, assetFile);
							}
							assets.Sort(AssetFileComparer.Instance);
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x001ED4E4 File Offset: 0x001EB6E4
		public bool TryRemoveDirectory(string path, out List<AssetFile> removedEntries)
		{
			removedEntries = null;
			AssetTreeFolder assetTree;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTree, false);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryRemoveDirectory: Invalid folder path " + path);
				result = false;
			}
			else
			{
				List<AssetFile> assets = this.GetAssets(assetTree);
				int directoryIndex = AssetList.GetDirectoryIndex(assets, path, false);
				bool flag2 = directoryIndex < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					int num = -1;
					removedEntries = new List<AssetFile>();
					for (int i = directoryIndex + 1; i < assets.Count; i++)
					{
						AssetFile assetFile = assets[i];
						bool flag3 = !assetFile.Path.StartsWith(path + "/");
						if (flag3)
						{
							break;
						}
						num = i;
						removedEntries.Add(assetFile);
					}
					bool flag4 = num > -1;
					if (flag4)
					{
						assets.RemoveRange(directoryIndex, num - directoryIndex + 1);
					}
					else
					{
						assets.RemoveAt(directoryIndex);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x001ED5D4 File Offset: 0x001EB7D4
		public bool TryRemoveFile(string path)
		{
			AssetTreeFolder assetTreeFolder;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTreeFolder, false);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryRemoveFile: Invalid folder path " + path);
				result = false;
			}
			else
			{
				List<AssetFile> assets = this.GetAssets(assetTreeFolder);
				int assetFileIndex = AssetList.GetAssetFileIndex(assets, path, false, assetTreeFolder == AssetTreeFolder.Cosmetics);
				bool flag2 = assetFileIndex < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					assets.RemoveAt(assetFileIndex);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005F7B RID: 24443 RVA: 0x001ED644 File Offset: 0x001EB844
		public bool TryMoveFile(string oldPath, string newPath)
		{
			AssetTreeFolder assetTreeFolder;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(newPath, out assetTreeFolder, false);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryMoveFile: Invalid new folder path " + newPath);
				result = false;
			}
			else
			{
				AssetTreeFolder assetTreeFolder2;
				bool flag2 = !AssetPathUtils.TryGetAssetTreeFolder(oldPath, out assetTreeFolder2, false);
				if (flag2)
				{
					AssetList.Logger.Warn("TryMoveFile: Invalid old folder path " + oldPath);
					result = false;
				}
				else
				{
					bool flag3 = assetTreeFolder != assetTreeFolder2;
					if (flag3)
					{
						result = (this.TryRemoveFile(oldPath) && this.TryInsertDirectory(newPath));
					}
					else
					{
						string assetType;
						bool flag4 = !this._assetTypeRegistry.TryGetAssetTypeFromPath(newPath, out assetType);
						if (flag4)
						{
							result = false;
						}
						else
						{
							List<AssetFile> assets = this.GetAssets(assetTreeFolder);
							int assetFileIndex = AssetList.GetAssetFileIndex(assets, oldPath, false, assetTreeFolder2 == AssetTreeFolder.Cosmetics);
							bool flag5 = assetFileIndex < 0;
							if (flag5)
							{
								result = false;
							}
							else
							{
								string assetIdFromReference = AssetPathUtils.GetAssetIdFromReference(newPath, assetTreeFolder == AssetTreeFolder.Cosmetics);
								string[] assetFilePathElements = AssetPathUtils.GetAssetFilePathElements(newPath, assetTreeFolder == AssetTreeFolder.Cosmetics);
								assets[assetFileIndex] = AssetFile.CreateFile(assetIdFromReference, newPath, assetType, assetFilePathElements);
								assets.Sort(AssetFileComparer.Instance);
								result = true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005F7C RID: 24444 RVA: 0x001ED75C File Offset: 0x001EB95C
		public bool TryInsertFile(string path)
		{
			string text;
			AssetTypeConfig assetTypeConfig;
			bool flag = !this._assetTypeRegistry.TryGetAssetTypeFromPath(path, out text) || !this._assetTypeRegistry.AssetTypes.TryGetValue(text, out assetTypeConfig);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryInsertFile: No asset type found matching path " + path);
				result = false;
			}
			else
			{
				int index;
				int num;
				bool flag2 = !this.TryGetDirectoryContentsBounds(assetTypeConfig.Path, out index, out num);
				if (flag2)
				{
					AssetList.Logger.Warn("TryInsertFile: Asset type directory not found " + assetTypeConfig.Path);
					result = false;
				}
				else
				{
					AssetTreeFolder assetTree;
					AssetPathUtils.TryGetAssetTreeFolder(path, out assetTree, false);
					List<AssetFile> assets = this.GetAssets(assetTree);
					string assetIdFromReference = AssetPathUtils.GetAssetIdFromReference(path, assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics);
					string[] assetFilePathElements = AssetPathUtils.GetAssetFilePathElements(path, assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics);
					assets.Insert(index, AssetFile.CreateFile(assetIdFromReference, path, text, assetFilePathElements));
					assets.Sort(index, num + 1, AssetFileComparer.Instance);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005F7D RID: 24445 RVA: 0x001ED854 File Offset: 0x001EBA54
		public bool TryInsertAssets(string path, List<AssetFile> newAssets)
		{
			AssetTreeFolder assetTree;
			bool flag = !AssetPathUtils.TryGetAssetTreeFolder(path, out assetTree, false);
			bool result;
			if (flag)
			{
				AssetList.Logger.Warn("TryInsertAssets: Invalid new folder path " + path);
				result = false;
			}
			else
			{
				int index;
				int num;
				bool flag2 = !this.TryGetDirectoryContentsBounds(path, out index, out num);
				if (flag2)
				{
					AssetList.Logger.Warn("TryInsertAssets: Folder not found " + path);
					result = false;
				}
				else
				{
					List<AssetFile> assets = this.GetAssets(assetTree);
					assets.InsertRange(index, newAssets);
					assets.Sort(index, num + newAssets.Count, AssetFileComparer.Instance);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06005F7E RID: 24446 RVA: 0x001ED8F0 File Offset: 0x001EBAF0
		private bool TryGetDirectoryContentsBounds(string directoryPath, out int index, out int size)
		{
			bool result;
			if (!(directoryPath == "Common"))
			{
				if (!(directoryPath == "Server"))
				{
					if (!(directoryPath == "Cosmetics/CharacterCreator"))
					{
						index = -1;
						size = 0;
						AssetTreeFolder assetTree;
						bool flag = !AssetPathUtils.TryGetAssetTreeFolder(directoryPath, out assetTree, false);
						if (flag)
						{
							result = false;
						}
						else
						{
							List<AssetFile> assets = this.GetAssets(assetTree);
							index = assets.BinarySearch(AssetFile.CreateDirectory(null, directoryPath, AssetPathUtils.GetAssetFilePathElements(directoryPath, false)), AssetFileComparer.Instance);
							bool flag2 = index == -1;
							if (flag2)
							{
								result = false;
							}
							else
							{
								int num = assets[index].PathElements.Length;
								index++;
								for (int i = index; i < assets.Count; i++)
								{
									AssetFile assetFile = assets[i];
									bool flag3 = assetFile.PathElements.Length <= num;
									if (flag3)
									{
										size = i - index;
										return true;
									}
								}
								size = assets.Count - index;
								result = true;
							}
						}
					}
					else
					{
						index = 0;
						size = this._cosmeticAssets.Count;
						result = true;
					}
				}
				else
				{
					index = 0;
					size = this._serverAssets.Count;
					result = true;
				}
			}
			else
			{
				index = 0;
				size = this._commonAssets.Count;
				result = true;
			}
			return result;
		}

		// Token: 0x06005F7F RID: 24447 RVA: 0x001EDA3C File Offset: 0x001EBC3C
		public bool TryGetPathForAssetId(string assetType, string assetId, out string filePath, bool ignoreCase = false)
		{
			filePath = null;
			AssetTypeConfig assetTypeConfig;
			bool flag = !this._assetTypeRegistry.AssetTypes.TryGetValue(assetType, out assetTypeConfig);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<AssetFile> assets = this.GetAssets(assetTypeConfig.AssetTree);
				int directoryIndex = AssetList.GetDirectoryIndex(assets, assetTypeConfig.Path, false);
				bool flag2 = directoryIndex < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					int num = assets[directoryIndex].PathElements.Length;
					for (int i = directoryIndex + 1; i < assets.Count; i++)
					{
						AssetFile assetFile = assets[i];
						bool flag3 = assetFile.PathElements.Length <= num;
						if (flag3)
						{
							break;
						}
						bool flag4 = assetFile.IsDirectory || assetFile.AssetType != assetType;
						if (!flag4)
						{
							bool flag5 = !assetFile.DisplayName.Equals(assetId, ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
							if (!flag5)
							{
								filePath = assetFile.Path;
								return true;
							}
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x04003B64 RID: 15204
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003B65 RID: 15205
		private List<AssetFile> _cosmeticAssets = new List<AssetFile>();

		// Token: 0x04003B66 RID: 15206
		private List<AssetFile> _serverAssets = new List<AssetFile>();

		// Token: 0x04003B67 RID: 15207
		private List<AssetFile> _commonAssets = new List<AssetFile>();

		// Token: 0x04003B68 RID: 15208
		private readonly AssetTypeRegistry _assetTypeRegistry;
	}
}
