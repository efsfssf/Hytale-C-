using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Core
{
	// Token: 0x02000B79 RID: 2937
	internal static class AssetManager
	{
		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06005A1B RID: 23067 RVA: 0x001BFDF8 File Offset: 0x001BDFF8
		// (set) Token: 0x06005A1C RID: 23068 RVA: 0x001BFDFF File Offset: 0x001BDFFF
		public static bool IsAssetsDirectoryImmutable { get; private set; }

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x06005A1D RID: 23069 RVA: 0x001BFE07 File Offset: 0x001BE007
		public static int ActivelyReferencedAssetsCount
		{
			get
			{
				return AssetManager.ActivelyReferencedAssets.Count;
			}
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x06005A1E RID: 23070 RVA: 0x001BFE13 File Offset: 0x001BE013
		public static int BuiltInAssetsCount
		{
			get
			{
				return AssetManager.ActivelyReferencedAssets.Count - AssetManager.CachedAssetsOnDisk.Count;
			}
		}

		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06005A1F RID: 23071 RVA: 0x001BFE2A File Offset: 0x001BE02A
		public static int CachedAssetsCount
		{
			get
			{
				return AssetManager.CachedAssetsOnDisk.Count;
			}
		}

		// Token: 0x06005A20 RID: 23072 RVA: 0x001BFE38 File Offset: 0x001BE038
		public static void Initialize(Engine engine, CancellationToken cancellationToken, out HashSet<string> newAssets)
		{
			bool flag = File.Exists(Path.GetFullPath(Path.Combine(Paths.BuiltInAssets, "CommonAssetsIndex.hashes")));
			if (flag)
			{
				AssetManager.IsAssetsDirectoryImmutable = true;
			}
			AssetManager.LoadMetadataForBuiltInCommonAssets(engine, cancellationToken, out newAssets);
			AssetManager.LoadCachedAssetsIndex();
			AssetManager.IsInitialized = !cancellationToken.IsCancellationRequested;
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x001BFE8C File Offset: 0x001BE08C
		public static byte[] GetBuiltInAsset(string assetPath)
		{
			bool flag = ThreadHelper.IsMainThread();
			if (flag)
			{
				AssetManager.Logger.Warn<string, string>("{0} was called from main thread: {1}", "GetBuiltInAsset", assetPath);
			}
			return File.ReadAllBytes(Path.Combine(Paths.BuiltInAssets, assetPath));
		}

		// Token: 0x06005A22 RID: 23074 RVA: 0x001BFED0 File Offset: 0x001BE0D0
		public static string GetAssetLocalPathUsingHash(string hash)
		{
			AssetManager.AssetMetadata assetMetadata;
			bool flag = !AssetManager.ActivelyReferencedAssets.TryGetValue(hash, ref assetMetadata);
			if (flag)
			{
				throw new Exception("Failed to find asset with hash " + hash);
			}
			bool flag2 = assetMetadata.CachedFileName != null;
			string result;
			if (flag2)
			{
				result = assetMetadata.CachedFileName;
			}
			else
			{
				result = assetMetadata.BuiltInFileNames[0];
			}
			return result;
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x001BFF2C File Offset: 0x001BE12C
		public static byte[] GetAssetUsingHash(string hash, bool allowFromMainThread = false)
		{
			bool flag = !allowFromMainThread && ThreadHelper.IsMainThread();
			if (flag)
			{
				AssetManager.Logger.Warn<string, string>("{0} was called from main thread: {1}", "GetAssetUsingHash", hash);
			}
			return File.ReadAllBytes(AssetManager.GetAssetLocalPathUsingHash(hash));
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x001BFF70 File Offset: 0x001BE170
		public static void MarkAssetAsServerRequired(string name, string hash, out bool foundInCache)
		{
			AssetManager.AssetMetadata assetMetadata;
			bool flag = !AssetManager.ActivelyReferencedAssets.TryGetValue(hash, ref assetMetadata);
			if (flag)
			{
				bool flag2 = !AssetManager.CachedAssetsOnDisk.ContainsKey(hash);
				if (flag2)
				{
					foundInCache = false;
					return;
				}
				assetMetadata = (AssetManager.ActivelyReferencedAssets[hash] = new AssetManager.AssetMetadata());
				assetMetadata.CachedFileName = AssetManager.GetCachedAssetFilePathFromHash(hash);
				AssetManager.CachedAssetsOnDisk[hash] = TimeHelper.GetEpochSeconds(null);
			}
			string item = Path.Combine(Paths.BuiltInAssets, "Common", name);
			bool flag3 = assetMetadata.BuiltInFileNames == null || !assetMetadata.BuiltInFileNames.Contains(item);
			if (flag3)
			{
				assetMetadata.ServerFileReferences++;
			}
			foundInCache = true;
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x001C0028 File Offset: 0x001BE228
		public static void UnloadServerRequiredAssets()
		{
			foreach (AssetManager.AssetMetadata assetMetadata in AssetManager.ActivelyReferencedAssets.Values)
			{
				assetMetadata.ServerFileReferences = 0;
			}
			List<KeyValuePair<string, AssetManager.AssetMetadata>> list = Enumerable.ToList<KeyValuePair<string, AssetManager.AssetMetadata>>(Enumerable.Where<KeyValuePair<string, AssetManager.AssetMetadata>>(AssetManager.ActivelyReferencedAssets, (KeyValuePair<string, AssetManager.AssetMetadata> x) => x.Value.BuiltInFileNames == null || x.Value.BuiltInFileNames.Count == 0));
			foreach (KeyValuePair<string, AssetManager.AssetMetadata> keyValuePair in list)
			{
				AssetManager.AssetMetadata assetMetadata2;
				AssetManager.ActivelyReferencedAssets.TryRemove(keyValuePair.Key, ref assetMetadata2);
			}
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x001C00F8 File Offset: 0x001BE2F8
		public static void AddServerAssetToCache(string name, string hash)
		{
			AssetManager.CachedAssetsOnDisk[hash] = TimeHelper.GetEpochSeconds(null);
			AssetManager.AssetMetadata assetMetadata;
			bool flag = !AssetManager.ActivelyReferencedAssets.TryGetValue(hash, ref assetMetadata);
			if (flag)
			{
				assetMetadata = (AssetManager.ActivelyReferencedAssets[hash] = new AssetManager.AssetMetadata());
			}
			bool flag2 = assetMetadata.CachedFileName == null;
			if (flag2)
			{
				assetMetadata.CachedFileName = AssetManager.GetCachedAssetFilePathFromHash(hash);
			}
			assetMetadata.ServerFileReferences++;
			AssetManager.Logger.Info<string, int, string>("Added cached reference to asset {0}: {1}, {2}", hash, assetMetadata.ServerFileReferences, name);
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x001C0188 File Offset: 0x001BE388
		public static void RemoveServerAssetFromCache(string name, string hash)
		{
			AssetManager.AssetMetadata assetMetadata;
			bool flag = !AssetManager.ActivelyReferencedAssets.TryGetValue(hash, ref assetMetadata);
			if (flag)
			{
				AssetManager.Logger.Warn<string, string>("Attempted to remove asset from cache that doesn't exist! {0} ({1})", name, hash);
			}
			else
			{
				string item = Path.Combine(Paths.BuiltInAssets, "Common", name);
				bool flag2 = assetMetadata.BuiltInFileNames == null || !assetMetadata.BuiltInFileNames.Remove(item);
				if (flag2)
				{
					assetMetadata.ServerFileReferences--;
				}
				bool flag3 = (assetMetadata.BuiltInFileNames == null || assetMetadata.BuiltInFileNames.Count == 0) && assetMetadata.ServerFileReferences == 0;
				if (flag3)
				{
					AssetManager.Logger.Info<string, string>("Removing asset {0} ({1}) from memory, no more references.", name, hash);
					AssetManager.AssetMetadata assetMetadata2;
					AssetManager.ActivelyReferencedAssets.TryRemove(hash, ref assetMetadata2);
				}
				else
				{
					bool isInfoEnabled = AssetManager.Logger.IsInfoEnabled;
					if (isInfoEnabled)
					{
						Logger logger = AssetManager.Logger;
						string message = "Removed reference to asset {0} ({1}) but retained in memory due to other references! BuiltInFileNames: {2}, CachedReferences: {3}";
						object[] array = new object[4];
						array[0] = name;
						array[1] = hash;
						int num = 2;
						List<string> builtInFileNames = assetMetadata.BuiltInFileNames;
						array[num] = ((builtInFileNames != null) ? builtInFileNames.Count : 0);
						array[3] = assetMetadata.ServerFileReferences;
						logger.Info(message, array);
					}
				}
			}
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x001C02A4 File Offset: 0x001BE4A4
		public static void Shutdown()
		{
			bool flag = !AssetManager.IsInitialized;
			if (!flag)
			{
				AssetManager.Logger.Info("Shutting down...");
				AssetManager.EvictCachedAssets();
				AssetManager.UpdateCachedAssetsIndex();
			}
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x001C02E0 File Offset: 0x001BE4E0
		public static string GetAssetCountInfo()
		{
			return string.Format("Total Assets: {0}, BuiltIn: {1}, Cached: {2}", AssetManager.ActivelyReferencedAssets.Count, AssetManager.ActivelyReferencedAssets.Count - AssetManager.CachedAssetsOnDisk.Count, AssetManager.CachedAssetsOnDisk.Count);
		}

		// Token: 0x06005A2A RID: 23082 RVA: 0x001C0334 File Offset: 0x001BE534
		private static void LoadMetadataForBuiltInCommonAssets(Engine engine, CancellationToken cancellationToken, out HashSet<string> updatedAssets)
		{
			updatedAssets = new HashSet<string>();
			string text = Path.Combine(Paths.BuiltInAssets, "Common");
			string text2 = Path.Combine(Paths.BuiltInAssets, "CommonAssetsIndex.hashes");
			string text3 = Path.Combine(Paths.BuiltInAssets, "CommonAssetsIndex.cache");
			AssetManager.Logger.Info("Loading built-in assets from {0}.", text);
			char[] separator = new char[]
			{
				' '
			};
			File.Delete(Path.Combine(Paths.BuiltInAssets, text3 + ".tmp"));
			FileStream fileStream = null;
			Stopwatch stopwatch;
			try
			{
				fileStream = File.OpenRead(text2);
			}
			catch
			{
				AssetManager.<>c__DisplayClass29_0 CS$<>8__locals1 = new AssetManager.<>c__DisplayClass29_0();
				AssetManager.Logger.Info("Could not find {0}, recomputing hashes for built-in assets.", text2);
				stopwatch = Stopwatch.StartNew();
				CS$<>8__locals1.fileNames = Enumerable.ToArray<string>(Enumerable.Where<string>(Directory.GetFiles(text, "*.*", 1), (string x) => !x.EndsWith(".sha") && !x.EndsWith(".hash")));
				Dictionary<string, AssetManager.HashCacheEntry> dictionary = new Dictionary<string, AssetManager.HashCacheEntry>();
				foreach (string text4 in CS$<>8__locals1.fileNames)
				{
					string key = text4.Substring(text.Length + 1);
					dictionary[key] = null;
				}
				FileStream fileStream2 = null;
				try
				{
					fileStream2 = File.OpenRead(text3);
				}
				catch
				{
				}
				bool flag = fileStream2 != null;
				if (flag)
				{
					using (StreamReader streamReader = new StreamReader(fileStream2))
					{
						bool flag2 = false;
						string text5 = streamReader.ReadLine();
						bool flag3 = text5 == "VERSION=1";
						if (flag3)
						{
							flag2 = true;
							text5 = streamReader.ReadLine();
						}
						for (;;)
						{
							bool flag4 = text5 == null;
							if (flag4)
							{
								break;
							}
							string[] array = text5.Split(separator, 3);
							string hash = array[0];
							long num = long.Parse(array[1]);
							string text6 = array[2];
							bool flag5 = !flag2;
							if (flag5)
							{
								num = TimeHelper.GetEpochSeconds(new DateTime?(DateTime.FromFileTimeUtc(num)));
							}
							bool flag6 = dictionary.ContainsKey(text6);
							if (flag6)
							{
								dictionary[text6] = new AssetManager.HashCacheEntry
								{
									HashTime = num,
									Hash = hash
								};
							}
							else
							{
								updatedAssets.Add(text6);
							}
							text5 = streamReader.ReadLine();
						}
					}
					fileStream2.Close();
				}
				int num2 = 0;
				int i2;
				int i;
				for (i = 0; i < CS$<>8__locals1.fileNames.Length; i = i2 + 1)
				{
					bool isCancellationRequested = cancellationToken.IsCancellationRequested;
					if (isCancellationRequested)
					{
						return;
					}
					string text7 = CS$<>8__locals1.fileNames[i];
					string text8 = text7.Substring(text.Length + 1);
					try
					{
						long epochSeconds = TimeHelper.GetEpochSeconds(new DateTime?(File.GetLastWriteTimeUtc(text7)));
						AssetManager.HashCacheEntry hashCacheEntry = dictionary[text8];
						bool flag7 = hashCacheEntry == null || hashCacheEntry.HashTime != epochSeconds;
						string hash2;
						if (flag7)
						{
							hash2 = AssetManager.ComputeHash(File.ReadAllBytes(text7));
							updatedAssets.Add(text8);
						}
						else
						{
							hash2 = hashCacheEntry.Hash;
							num2++;
						}
						dictionary[text8] = new AssetManager.HashCacheEntry
						{
							HashTime = epochSeconds,
							Hash = hash2
						};
						AssetManager.AddBuiltInFileNameReference(hash2, text7);
						engine.RunOnMainThread(engine, delegate
						{
							AssetManager.BuiltInAssetsMetadataLoadProgress = (float)i / (float)CS$<>8__locals1.fileNames.Length;
						}, false, false);
					}
					catch (Exception exception)
					{
						AssetManager.Logger.Error(exception, "Failed to hash: " + text7);
						dictionary.Remove(text8);
					}
					i2 = i;
				}
				FileStream fileStream3;
				fileStream2 = (fileStream3 = File.Create(text3 + ".tmp"));
				try
				{
					using (StreamWriter streamWriter = new StreamWriter(fileStream2))
					{
						streamWriter.WriteLine("VERSION=1");
						foreach (KeyValuePair<string, AssetManager.HashCacheEntry> keyValuePair in dictionary)
						{
							streamWriter.WriteLine(string.Format("{0} {1} {2}", keyValuePair.Value.Hash, keyValuePair.Value.HashTime, keyValuePair.Key));
						}
					}
				}
				finally
				{
					if (fileStream3 != null)
					{
						((IDisposable)fileStream3).Dispose();
					}
				}
				stopwatch.Stop();
				AssetManager.Logger.Info<long, int, int>("Spent {0}ms recomputing hashes for {1} built-in assets, reused {2} hashes from cache.", stopwatch.ElapsedMilliseconds, CS$<>8__locals1.fileNames.Length, num2);
				return;
			}
			stopwatch = Stopwatch.StartNew();
			using (StreamReader streamReader2 = new StreamReader(fileStream))
			{
				for (;;)
				{
					string text9 = streamReader2.ReadLine();
					bool flag8 = text9 == null;
					if (flag8)
					{
						break;
					}
					string[] array2 = text9.Split(separator, 2);
					string hash3 = array2[0];
					string fileName = Path.Combine(text, array2[1]);
					AssetManager.AddBuiltInFileNameReference(hash3, fileName);
				}
			}
			stopwatch.Stop();
			AssetManager.Logger.Info<long, string>("Spent {0}ms loading hashes from {1}.", stopwatch.ElapsedMilliseconds, text2);
			fileStream.Close();
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x001C0918 File Offset: 0x001BEB18
		private static void AddBuiltInFileNameReference(string hash, string fileName)
		{
			AssetManager.AssetMetadata assetMetadata;
			bool flag = !AssetManager.ActivelyReferencedAssets.TryGetValue(hash, ref assetMetadata);
			if (flag)
			{
				ConcurrentDictionary<string, AssetManager.AssetMetadata> activelyReferencedAssets = AssetManager.ActivelyReferencedAssets;
				AssetManager.AssetMetadata assetMetadata2 = new AssetManager.AssetMetadata();
				assetMetadata2.BuiltInFileNames = new List<string>();
				assetMetadata = assetMetadata2;
				activelyReferencedAssets[hash] = assetMetadata2;
			}
			assetMetadata.BuiltInFileNames.Add(fileName);
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x001C0968 File Offset: 0x001BEB68
		public static HashSet<string> GetUpdatedAssets(string directoryPath, string cacheFilePath, CancellationToken cancellationToken)
		{
			HashSet<string> hashSet = new HashSet<string>();
			string[] array = Enumerable.ToArray<string>(Directory.GetFiles(directoryPath, "*.*", 1));
			Dictionary<string, AssetManager.HashCacheEntry> dictionary = new Dictionary<string, AssetManager.HashCacheEntry>();
			foreach (string text in array)
			{
				string key = text.Substring(directoryPath.Length + 1);
				dictionary[key] = null;
			}
			char[] separator = new char[]
			{
				' '
			};
			FileStream fileStream = null;
			try
			{
				fileStream = File.OpenRead(cacheFilePath);
			}
			catch
			{
			}
			bool flag = fileStream != null;
			if (flag)
			{
				using (StreamReader streamReader = new StreamReader(fileStream))
				{
					bool flag2 = false;
					string text2 = streamReader.ReadLine();
					bool flag3 = text2 == "VERSION=1";
					if (flag3)
					{
						flag2 = true;
						text2 = streamReader.ReadLine();
					}
					for (;;)
					{
						bool flag4 = text2 == null;
						if (flag4)
						{
							break;
						}
						string[] array3 = text2.Split(separator, 3);
						long num = long.Parse(array3[0]);
						string text3 = array3[1];
						bool flag5 = !flag2;
						if (flag5)
						{
							num = TimeHelper.GetEpochSeconds(new DateTime?(DateTime.FromFileTimeUtc(num)));
						}
						bool flag6 = dictionary.ContainsKey(text3);
						if (flag6)
						{
							dictionary[text3] = new AssetManager.HashCacheEntry
							{
								HashTime = num
							};
						}
						else
						{
							hashSet.Add(text3);
						}
						text2 = streamReader.ReadLine();
					}
				}
				fileStream.Close();
			}
			for (int j = 0; j < array.Length; j++)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					return hashSet;
				}
				string text4 = array[j];
				string text5 = text4.Substring(directoryPath.Length + 1);
				long epochSeconds = TimeHelper.GetEpochSeconds(new DateTime?(File.GetLastWriteTimeUtc(text4)));
				AssetManager.HashCacheEntry hashCacheEntry = dictionary[text5];
				bool flag7 = hashCacheEntry == null || hashCacheEntry.HashTime != epochSeconds;
				if (flag7)
				{
					hashSet.Add(text5);
				}
				dictionary[text5] = new AssetManager.HashCacheEntry
				{
					HashTime = epochSeconds
				};
			}
			FileStream fileStream2;
			fileStream = (fileStream2 = File.Create(cacheFilePath));
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.WriteLine("VERSION=1");
					foreach (KeyValuePair<string, AssetManager.HashCacheEntry> keyValuePair in dictionary)
					{
						streamWriter.WriteLine(string.Format("{0} {1}", keyValuePair.Value.HashTime, keyValuePair.Key));
					}
				}
			}
			finally
			{
				if (fileStream2 != null)
				{
					((IDisposable)fileStream2).Dispose();
				}
			}
			return hashSet;
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x001C0C6C File Offset: 0x001BEE6C
		private static void LoadCachedAssetsIndex()
		{
			string text = "CachedAssetsIndex.cache";
			AssetManager.Logger.Info("Loading cached assets from {0}...", Paths.CachedAssets);
			FileStream fileStream = null;
			try
			{
				fileStream = File.OpenRead(Path.Combine(Paths.CachedAssets, "..", text));
			}
			catch
			{
				AssetManager.Logger.Info("Could not find {0}.", text);
				return;
			}
			char[] separator = new char[]
			{
				' '
			};
			using (StreamReader streamReader = new StreamReader(fileStream))
			{
				string text2 = streamReader.ReadLine();
				bool flag = text2 != "VERSION=1";
				if (flag)
				{
					AssetManager.Logger.Info<string, string>("Cached assets index in {0} has invalid header, found {1}.", Paths.CachedAssets, text2);
					return;
				}
				for (;;)
				{
					text2 = streamReader.ReadLine();
					bool flag2 = text2 == null;
					if (flag2)
					{
						break;
					}
					string[] array = text2.Split(separator, 2);
					string text3 = array[0];
					long num = long.Parse(array[1]);
					string cachedAssetFilePathFromHash = AssetManager.GetCachedAssetFilePathFromHash(text3);
					bool flag3 = !File.Exists(cachedAssetFilePathFromHash);
					if (!flag3)
					{
						AssetManager.CachedAssetsOnDisk[text3] = num;
						AssetManager.AssetMetadata assetMetadata;
						bool flag4 = AssetManager.ActivelyReferencedAssets.TryGetValue(text3, ref assetMetadata);
						if (flag4)
						{
							AssetManager.Logger.Info<string, string>("Linked BuiltIn asset to cached on disk path: {0}, {1}", text3, string.Join(", ", assetMetadata.BuiltInFileNames));
							assetMetadata.CachedFileName = cachedAssetFilePathFromHash;
						}
					}
				}
			}
			fileStream.Close();
		}

		// Token: 0x06005A2E RID: 23086 RVA: 0x001C0DF4 File Offset: 0x001BEFF4
		private static void EvictCachedAssets()
		{
			long cutoffEpochSeconds = TimeHelper.GetEpochSeconds(null) - 2592000L;
			KeyValuePair<string, long>[] array = Enumerable.ToArray<KeyValuePair<string, long>>(Enumerable.Where<KeyValuePair<string, long>>(AssetManager.CachedAssetsOnDisk, (KeyValuePair<string, long> x) => x.Value < cutoffEpochSeconds));
			bool flag = array.Length == 0;
			if (!flag)
			{
				AssetManager.Logger.Info("Evicting {0} assets from cache.", array.Length);
				foreach (KeyValuePair<string, long> keyValuePair in array)
				{
					long num;
					AssetManager.CachedAssetsOnDisk.TryRemove(keyValuePair.Key, ref num);
					AssetManager.AssetMetadata assetMetadata;
					bool flag2 = AssetManager.ActivelyReferencedAssets.TryGetValue(keyValuePair.Key, ref assetMetadata);
					if (flag2)
					{
						assetMetadata.CachedFileName = null;
						assetMetadata.ServerFileReferences = 0;
					}
					try
					{
						string text = Path.Combine(Paths.CachedAssets, keyValuePair.Key.Substring(0, 2));
						string text2 = Path.Combine(text, keyValuePair.Key.Substring(2));
						File.Delete(text2);
						bool flag3 = Directory.GetFiles(text).Length == 0;
						if (flag3)
						{
							Directory.Delete(text);
						}
					}
					catch
					{
						throw;
					}
				}
			}
		}

		// Token: 0x06005A2F RID: 23087 RVA: 0x001C0F38 File Offset: 0x001BF138
		private static void UpdateCachedAssetsIndex()
		{
			string fullPath = Path.GetFullPath(Path.Combine(Paths.CachedAssets, "..", "CachedAssetsIndex.cache"));
			using (FileStream fileStream = File.Open(fullPath, FileMode.Create))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.WriteLine("VERSION=1");
					foreach (KeyValuePair<string, long> keyValuePair in AssetManager.CachedAssetsOnDisk)
					{
						streamWriter.WriteLine(string.Format("{0} {1}", keyValuePair.Key, keyValuePair.Value));
					}
				}
			}
		}

		// Token: 0x06005A30 RID: 23088 RVA: 0x001C1010 File Offset: 0x001BF210
		private static string GetCachedAssetFilePathFromHash(string hash)
		{
			return Path.Combine(Paths.CachedAssets, hash.Substring(0, 2), hash.Substring(2));
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x001C102C File Offset: 0x001BF22C
		public static string ComputeHash(byte[] data)
		{
			byte[] rawHash = AssetManager.HashProvider.ComputeHash(data);
			return AssetManager.HashBytesAsString(rawHash);
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x001C1050 File Offset: 0x001BF250
		private static string HashBytesAsString(byte[] rawHash)
		{
			StringBuilder stringBuilder = new StringBuilder(rawHash.Length * 2);
			for (int i = 0; i < rawHash.Length; i++)
			{
				stringBuilder.Append(rawHash[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003855 RID: 14421
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003856 RID: 14422
		private static readonly ConcurrentDictionary<string, AssetManager.AssetMetadata> ActivelyReferencedAssets = new ConcurrentDictionary<string, AssetManager.AssetMetadata>();

		// Token: 0x04003857 RID: 14423
		private static readonly ConcurrentDictionary<string, long> CachedAssetsOnDisk = new ConcurrentDictionary<string, long>();

		// Token: 0x04003858 RID: 14424
		private static volatile bool IsInitialized;

		// Token: 0x04003859 RID: 14425
		private static readonly SHA256CryptoServiceProvider HashProvider = new SHA256CryptoServiceProvider();

		// Token: 0x0400385A RID: 14426
		private const long CachedAssetMaxAgeSeconds = 2592000L;

		// Token: 0x0400385C RID: 14428
		public static float BuiltInAssetsMetadataLoadProgress;

		// Token: 0x02000F5E RID: 3934
		private class AssetMetadata
		{
			// Token: 0x04004AC4 RID: 19140
			public string CachedFileName;

			// Token: 0x04004AC5 RID: 19141
			public int ServerFileReferences;

			// Token: 0x04004AC6 RID: 19142
			public List<string> BuiltInFileNames;
		}

		// Token: 0x02000F5F RID: 3935
		private class HashCacheEntry
		{
			// Token: 0x04004AC7 RID: 19143
			public long HashTime;

			// Token: 0x04004AC8 RID: 19144
			public string Hash;
		}
	}
}
