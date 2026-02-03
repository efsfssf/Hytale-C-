using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Hypixel.ProtoPlus;
using HytaleClient.AssetEditor.Backends;
using HytaleClient.AssetEditor.Data;
using HytaleClient.Interface.Messages;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.AssetEditor.Networking
{
	// Token: 0x02000B94 RID: 2964
	internal class AssetEditorPacketHandler : BasePacketHandler
	{
		// Token: 0x06005B74 RID: 23412 RVA: 0x001C9880 File Offset: 0x001C7A80
		private void ProcessAssetEditorAssetListSetup(AssetEditorAssetListSetup packet)
		{
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.SetupAssetList(packet.Tree, packet.Paths);
			}, false, false);
		}

		// Token: 0x06005B75 RID: 23413 RVA: 0x001C98C4 File Offset: 0x001C7AC4
		private void ProcessAssetEditorAssetListUpdate(AssetEditorAssetListUpdate packet)
		{
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.UpdateAssetList(packet.Additions, packet.Deletions);
			}, false, false);
		}

		// Token: 0x06005B76 RID: 23414 RVA: 0x001C9908 File Offset: 0x001C7B08
		private void ProcessAssetEditorPopupNotification(AssetEditorPopupNotification packet)
		{
			this._app.Engine.RunOnMainThread(this, delegate
			{
				FormattedMessage message;
				bool flag = FormattedMessage.TryParseFromBson(packet.Message, out message);
				if (flag)
				{
					this._app.Interface.AssetEditor.ToastNotifications.AddNotification(packet.Type, message);
				}
				else
				{
					AssetEditorPacketHandler.Logger.Warn("Failed to parse asset editor popup notification");
				}
			}, false, false);
		}

		// Token: 0x06005B77 RID: 23415 RVA: 0x001C994C File Offset: 0x001C7B4C
		private void ProcessAssetEditorSetupSchemas(AssetEditorSetupSchemas packet)
		{
			AssetEditorPacketHandler.Logger.Info("Received schemas for asset editor setup");
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.SetupSchemas(packet.Schemas);
			}, false, false);
		}

		// Token: 0x06005B78 RID: 23416 RVA: 0x001C99A0 File Offset: 0x001C7BA0
		private void ProcessAssetEditorSetupAssetTypes(AssetEditorSetupAssetTypes packet)
		{
			AssetEditorPacketHandler.Logger.Info("Received asset types for asset editor setup");
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.SetupAssetTypes(packet.AssetTypes);
			}, false, false);
		}

		// Token: 0x06005B79 RID: 23417 RVA: 0x001C99F4 File Offset: 0x001C7BF4
		private void ProcessAssetEditorAssetUpdated(AssetEditorAssetUpdated packet)
		{
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.OnAssetUpdated(packet.Path, packet.Data);
			}, false, false);
		}

		// Token: 0x06005B7A RID: 23418 RVA: 0x001C9A38 File Offset: 0x001C7C38
		private void ProcessAssetEditorAssetUpdated(AssetEditorJsonAssetUpdated packet)
		{
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.OnJsonAssetUpdated(packet.Path, packet.Commands);
			}, false, false);
		}

		// Token: 0x06005B7B RID: 23419 RVA: 0x001C9A7A File Offset: 0x001C7C7A
		private void ProcessAssetEditorFetchAssetReply(AssetEditorFetchAssetReply packet)
		{
			base.CallPendingCallback(packet.Token, packet, null);
		}

		// Token: 0x06005B7C RID: 23420 RVA: 0x001C9A8C File Offset: 0x001C7C8C
		private void ProcessAssetEditorFetchJsonAssetWithParentsReply(AssetEditorFetchJsonAssetWithParentsReply packet)
		{
			base.CallPendingCallback(packet.Token, packet, null);
		}

		// Token: 0x06005B7D RID: 23421 RVA: 0x001C9AA0 File Offset: 0x001C7CA0
		private void ProcessAssetEditorRequestDatasetReply(AssetEditorRequestDatasetReply packet)
		{
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.OnDropdownDatasetReceived(packet.Name, Enumerable.ToList<string>(packet.Ids));
			}, false, false);
		}

		// Token: 0x06005B7E RID: 23422 RVA: 0x001C9AE2 File Offset: 0x001C7CE2
		private void ProcessAssetEditorFetchAutoCompleteDataReply(AssetEditorFetchAutoCompleteDataReply packet)
		{
			base.CallPendingCallback(packet.Token, packet, null);
		}

		// Token: 0x06005B7F RID: 23423 RVA: 0x001C9AF4 File Offset: 0x001C7CF4
		private void ProcessAssetEditorDeleteAssets(AssetEditorExportDeleteAssets packet)
		{
			bool flag = !this._app.Editor.Backend.IsExportingAssets;
			if (flag)
			{
				throw new Exception("Received export asset while not exporting");
			}
			string localAssetsDirectoryPathForCurrentExport = this._app.Editor.ServerBackend.LocalAssetsDirectoryPathForCurrentExport;
			Asset[] asset_ = packet.Asset_;
			for (int i = 0; i < asset_.Length; i++)
			{
				Asset asset = asset_[i];
				bool flag2 = !this._app.Editor.ServerBackend.AssetExportStatuses.ContainsKey(asset.Name);
				if (flag2)
				{
					throw new Exception("Received unexpected asset during export");
				}
				string fullPath = Path.GetFullPath(Path.Combine(localAssetsDirectoryPathForCurrentExport, asset.Name));
				string fullPath2 = Path.GetFullPath(Path.Combine(localAssetsDirectoryPathForCurrentExport, "Common"));
				string fullPath3 = Path.GetFullPath(Path.Combine(localAssetsDirectoryPathForCurrentExport, "Server"));
				bool flag3 = !Paths.IsSubPathOf(fullPath, fullPath2) && !Paths.IsSubPathOf(fullPath, fullPath3);
				if (flag3)
				{
					throw new Exception("Path must be within assets directory");
				}
				bool flag4 = Path.GetFileName(fullPath).StartsWith(".");
				if (flag4)
				{
					throw new Exception("File cannot start with .");
				}
				bool flag5 = File.Exists(fullPath);
				if (flag5)
				{
					File.Delete(fullPath);
				}
				AssetEditorPacketHandler.Logger.Info("Exported (deleted) {0} from asset editor", fullPath);
				this._app.Engine.RunOnMainThread(this, delegate
				{
					this._app.Editor.ServerBackend.AssetExportStatuses[asset.Name] = ServerAssetEditorBackend.AssetExportStatus.Complete;
					this._app.Editor.ServerBackend.OnExportProgress();
				}, false, false);
			}
		}

		// Token: 0x06005B80 RID: 23424 RVA: 0x001C9C84 File Offset: 0x001C7E84
		private void ProcessAssetEditorInitialize(AssetEditorExportAssetInitialize packet)
		{
			bool flag = !this._app.Editor.Backend.IsExportingAssets;
			if (flag)
			{
				throw new Exception("Received export asset while not exporting");
			}
			bool flag2 = !this._app.Editor.ServerBackend.AssetExportStatuses.ContainsKey(packet.Asset_.Name);
			if (flag2)
			{
				throw new Exception("Received unexpected asset during export");
			}
			bool flag3 = this._blobAsset != null;
			if (flag3)
			{
				throw new Exception("A blob download has already started! Name: " + this._blobAsset.Name + ", Hash: " + this._blobAsset.Hash);
			}
			bool failed = packet.Failed;
			if (failed)
			{
				this._app.Engine.RunOnMainThread(this, delegate
				{
					this._app.Editor.ServerBackend.AssetExportStatuses[packet.Asset_.Name] = ServerAssetEditorBackend.AssetExportStatus.Failed;
					this._app.Editor.ServerBackend.OnExportProgress();
				}, false, false);
			}
			else
			{
				this._blobFileStream = File.Create(Paths.TempAssetEditorDownload);
				this._blobAsset = packet.Asset_;
			}
		}

		// Token: 0x06005B81 RID: 23425 RVA: 0x001C9D98 File Offset: 0x001C7F98
		private void ProcessAssetEditorPart(AssetEditorExportAssetPart packet)
		{
			bool flag = !this._app.Editor.ServerBackend.IsExportingAssets;
			if (flag)
			{
				throw new Exception("Received export asset while not exporting");
			}
			this._blobFileStream.Write((byte[])packet.Part, 0, packet.Part.Length);
		}

		// Token: 0x06005B82 RID: 23426 RVA: 0x001C9DF0 File Offset: 0x001C7FF0
		private void ProcessAssetEditorFinalize(AssetEditorExportAssetFinalize packet)
		{
			bool flag = !this._app.Editor.ServerBackend.IsExportingAssets;
			if (flag)
			{
				throw new Exception("Received export asset while not exporting");
			}
			string localAssetsDirectoryPathForCurrentExport = this._app.Editor.ServerBackend.LocalAssetsDirectoryPathForCurrentExport;
			this._blobFileStream.Flush(true);
			this._blobFileStream.Close();
			this._blobFileStream = null;
			string assetName = this._blobAsset.Name;
			this._blobAsset = null;
			string fullPath = Path.GetFullPath(Path.Combine(localAssetsDirectoryPathForCurrentExport, assetName));
			string fullPath2 = Path.GetFullPath(Path.Combine(localAssetsDirectoryPathForCurrentExport, "Common"));
			string fullPath3 = Path.GetFullPath(Path.Combine(localAssetsDirectoryPathForCurrentExport, "Server"));
			bool flag2 = !Paths.IsSubPathOf(fullPath, fullPath2) && !Paths.IsSubPathOf(fullPath, fullPath3);
			if (flag2)
			{
				throw new Exception("Path must be within assets directory");
			}
			bool flag3 = Path.GetFileName(fullPath).StartsWith(".");
			if (flag3)
			{
				throw new Exception("File cannot start with .");
			}
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			bool flag4 = File.Exists(fullPath);
			if (flag4)
			{
				File.Delete(fullPath);
			}
			File.Move(Paths.TempAssetEditorDownload, fullPath);
			AssetEditorPacketHandler.Logger.Info("Exported {0} from asset editor", fullPath);
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.AssetExportStatuses[assetName] = ServerAssetEditorBackend.AssetExportStatus.Complete;
				this._app.Editor.ServerBackend.OnExportProgress();
			}, false, false);
		}

		// Token: 0x06005B83 RID: 23427 RVA: 0x001C9F5C File Offset: 0x001C815C
		public void ProcessAssetEditorExportComplete(AssetEditorExportComplete packet)
		{
			bool flag = !this._app.Editor.ServerBackend.IsExportingAssets;
			if (flag)
			{
				throw new Exception("Received $ProcessAssetEditorExportComplete asset while no asset export is active");
			}
			this._app.Engine.RunOnMainThread(this, delegate
			{
				TimestampedAssetReference[] array = new TimestampedAssetReference[packet.Assets.Length];
				for (int i = 0; i < packet.Assets.Length; i++)
				{
					TimestampedAssetReference timestampedAssetReference = packet.Assets[i];
					array[i] = new TimestampedAssetReference(timestampedAssetReference.Path, timestampedAssetReference.Timestamp);
				}
				this._app.Editor.ServerBackend.OnExportComplete(array);
			}, false, false);
		}

		// Token: 0x06005B84 RID: 23428 RVA: 0x001C9FC5 File Offset: 0x001C81C5
		private void ProcessAssetEditorUndoRedoReply(AssetEditorUndoRedoReply packet)
		{
			base.CallPendingCallback(packet.Token, packet, null);
		}

		// Token: 0x06005B85 RID: 23429 RVA: 0x001C9FD8 File Offset: 0x001C81D8
		private void ProcessAssetEditorReceiveLastModifiedAssets(AssetEditorLastModifiedAssets packet)
		{
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.SetupLastModifiedAssets(packet.Assets);
			}, false, false);
		}

		// Token: 0x06005B86 RID: 23430 RVA: 0x001CA01C File Offset: 0x001C821C
		private void ProcessAssetEditorModifiedAssetsCount(AssetEditorModifiedAssetsCount packet)
		{
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.ServerBackend.SetupModifiedAssetsCount(packet.Count);
			}, false, false);
		}

		// Token: 0x06005B87 RID: 23431 RVA: 0x001CA060 File Offset: 0x001C8260
		private void ProcessAuth2Packet(Auth2 packet)
		{
			sbyte[] array;
			sbyte[] array2;
			this._app.AuthManager.HandleAuth2(packet.NonceA, packet.Cert, out array, out array2);
			this._connection.SendPacket(new Auth3(array, array2));
		}

		// Token: 0x06005B88 RID: 23432 RVA: 0x001CA0A2 File Offset: 0x001C82A2
		private void ProcessAuth4Packet(Auth4 packet)
		{
			this._app.AuthManager.HandleAuth4(packet.Secret, packet.NonceB);
			this._connection.SendPacket(new Auth5());
		}

		// Token: 0x06005B89 RID: 23433 RVA: 0x001CA0D3 File Offset: 0x001C82D3
		private void ProcessAuth6Packet(Auth6 packet)
		{
			this._app.AuthManager.HandleAuth6();
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.MainMenu.OnAuthenticated();
			}, false, false);
			this.SetStage(AssetEditorPacketHandler.ConnectionStage.Complete);
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x001CA10F File Offset: 0x001C830F
		private void ProcessFailureReply(FailureReply packet)
		{
			base.CallPendingCallback(packet.Token, null, packet);
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x001CA120 File Offset: 0x001C8320
		private void ProcessSuccessReply(SuccessReply packet)
		{
			base.CallPendingCallback(packet.Token, packet, null);
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x001CA131 File Offset: 0x001C8331
		public AssetEditorPacketHandler(AssetEditorApp app, ConnectionToServer connection) : base(app.Engine, connection)
		{
			this._app = app;
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x001CA166 File Offset: 0x001C8366
		private void SetStage(AssetEditorPacketHandler.ConnectionStage stage)
		{
			AssetEditorPacketHandler.Logger.Info<AssetEditorPacketHandler.ConnectionStage, AssetEditorPacketHandler.ConnectionStage, long>("Stage {0} -> {1} took {2}ms", this._stage, stage, this._stageStopwatch.ElapsedMilliseconds);
			this._stage = stage;
			this._stageStopwatch.Restart();
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x001CA1A0 File Offset: 0x001C83A0
		protected override void SetDisconnectReason(string reason)
		{
			this._app.Engine.RunOnMainThread(this._app.Engine, delegate
			{
				this._app.MainMenu.ServerDisconnectReason = reason;
			}, true, false);
		}

		// Token: 0x06005B8F RID: 23439 RVA: 0x001CA1EC File Offset: 0x001C83EC
		protected override void ProcessPacket(ProtoPacket packet)
		{
			Debug.Assert(ThreadHelper.IsOnThread(this._thread));
			AssetEditorPacketHandler.ConnectionStage stage = this._stage;
			AssetEditorPacketHandler.ConnectionStage connectionStage = stage;
			if (connectionStage != AssetEditorPacketHandler.ConnectionStage.Auth)
			{
				if (connectionStage == AssetEditorPacketHandler.ConnectionStage.Complete)
				{
					int id = packet.GetId();
					int num = id;
					if (num <= 52)
					{
						switch (num)
						{
						case 7:
							this.ProcessAssetEditorAssetListSetup((AssetEditorAssetListSetup)packet);
							goto IL_3BF;
						case 8:
							this.ProcessAssetEditorAssetListUpdate((AssetEditorAssetListUpdate)packet);
							goto IL_3BF;
						case 9:
							this.ProcessAssetEditorAssetUpdated((AssetEditorAssetUpdated)packet);
							goto IL_3BF;
						case 10:
						case 11:
						case 12:
						case 13:
						case 14:
						case 18:
						case 21:
						case 23:
						case 25:
						case 27:
						case 28:
						case 29:
							break;
						case 15:
							this.ProcessAssetEditorFinalize((AssetEditorExportAssetFinalize)packet);
							goto IL_3BF;
						case 16:
							this.ProcessAssetEditorInitialize((AssetEditorExportAssetInitialize)packet);
							goto IL_3BF;
						case 17:
							this.ProcessAssetEditorPart((AssetEditorExportAssetPart)packet);
							goto IL_3BF;
						case 19:
							this.ProcessAssetEditorExportComplete((AssetEditorExportComplete)packet);
							goto IL_3BF;
						case 20:
							this.ProcessAssetEditorDeleteAssets((AssetEditorExportDeleteAssets)packet);
							goto IL_3BF;
						case 22:
							this.ProcessAssetEditorFetchAssetReply((AssetEditorFetchAssetReply)packet);
							goto IL_3BF;
						case 24:
							this.ProcessAssetEditorFetchAutoCompleteDataReply((AssetEditorFetchAutoCompleteDataReply)packet);
							goto IL_3BF;
						case 26:
							this.ProcessAssetEditorFetchJsonAssetWithParentsReply((AssetEditorFetchJsonAssetWithParentsReply)packet);
							goto IL_3BF;
						case 30:
							this.ProcessAssetEditorAssetUpdated((AssetEditorJsonAssetUpdated)packet);
							goto IL_3BF;
						case 31:
							this.ProcessAssetEditorReceiveLastModifiedAssets((AssetEditorLastModifiedAssets)packet);
							goto IL_3BF;
						case 32:
							this.ProcessAssetEditorModifiedAssetsCount((AssetEditorModifiedAssetsCount)packet);
							goto IL_3BF;
						case 33:
							this.ProcessAssetEditorPopupNotification((AssetEditorPopupNotification)packet);
							goto IL_3BF;
						default:
							switch (num)
							{
							case 41:
								this.ProcessAssetEditorRequestDatasetReply((AssetEditorRequestDatasetReply)packet);
								goto IL_3BF;
							case 42:
							case 43:
								break;
							case 44:
								this.ProcessAssetEditorSetupAssetTypes((AssetEditorSetupAssetTypes)packet);
								goto IL_3BF;
							case 45:
								this.ProcessAssetEditorSetupSchemas((AssetEditorSetupSchemas)packet);
								goto IL_3BF;
							default:
								switch (num)
								{
								case 48:
									this.ProcessAssetEditorUndoRedoReply((AssetEditorUndoRedoReply)packet);
									goto IL_3BF;
								case 51:
									this.ProcessUpdateModelPreview((AssetEditorUpdateModelPreview)packet);
									goto IL_3BF;
								case 52:
									this.ProcessAssetEditorUpdateSecondsPerGameDay((AssetEditorUpdateSecondsPerGameDay)packet);
									goto IL_3BF;
								}
								break;
							}
							break;
						}
					}
					else if (num <= 164)
					{
						if (num == 103)
						{
							this.ProcessFailureReply((FailureReply)packet);
							goto IL_3BF;
						}
						if (num == 164)
						{
							this.ProcessSuccessReply((SuccessReply)packet);
							goto IL_3BF;
						}
					}
					else
					{
						if (num == 194)
						{
							this.ProcessUpdateEditorTimeOverride((UpdateEditorTimeOverride)packet);
							goto IL_3BF;
						}
						if (num == 229)
						{
							this.ProcessUpdateTranslations((UpdateTranslations)packet);
							goto IL_3BF;
						}
					}
					bool flag = this._unhandledPacketTypes.Add(packet.GetType().Name);
					if (flag)
					{
						AssetEditorPacketHandler.Logger.Warn("Received unhandled packet type: {0}", packet.GetType().Name);
					}
					IL_3BF:;
				}
			}
			else
			{
				switch (packet.GetId())
				{
				case 58:
					this.ProcessAuth2Packet((Auth2)packet);
					goto IL_BB;
				case 60:
					this.ProcessAuth4Packet((Auth4)packet);
					goto IL_BB;
				case 62:
					this.ProcessAuth6Packet((Auth6)packet);
					goto IL_BB;
				}
				bool flag2 = this._unhandledPacketTypes.Add(packet.GetType().Name);
				if (flag2)
				{
					AssetEditorPacketHandler.Logger.Warn("Received unhandled packet type: {0}", packet.GetType().Name);
				}
				IL_BB:;
			}
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x001CA5BC File Offset: 0x001C87BC
		private void ProcessUpdateTranslations(UpdateTranslations packet)
		{
			UpdateType updateType = packet.Type;
			Dictionary<string, string> translations = new Dictionary<string, string>(packet.Translations.Count);
			foreach (KeyValuePair<string, string> keyValuePair in packet.Translations)
			{
				translations[string.Copy(keyValuePair.Key)] = string.Copy(keyValuePair.Value);
			}
			this._app.Engine.RunOnMainThread(this, delegate
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				AssetEditorPacketHandler.Logger.Info(string.Format("[UpdateTranslations] Starting update... ({0})", updateType));
				switch (updateType)
				{
				case 0:
					this._app.Interface.SetServerMessages(translations);
					break;
				case 1:
					this._app.Interface.AddServerMessages(translations);
					break;
				case 2:
					this._app.Interface.RemoveServerMessages(translations.Keys);
					break;
				}
				stopwatch.Stop();
				AssetEditorPacketHandler.Logger.Info(string.Format("[UpdateTranslations] Update complete. Took {0}ms", stopwatch.Elapsed.TotalMilliseconds));
			}, false, false);
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x001CA680 File Offset: 0x001C8880
		private void ProcessUpdateModelPreview(AssetEditorUpdateModelPreview packet)
		{
			string assetPath = packet.AssetPath;
			bool flag = assetPath == null;
			if (!flag)
			{
				AssetEditorPreviewCameraSettings camera = (packet.Camera != null) ? new AssetEditorPreviewCameraSettings(packet.Camera) : null;
				BlockType block = (packet.Block != null) ? new BlockType(packet.Block) : null;
				Model model = (packet.Model_ != null) ? new Model(packet.Model_) : null;
				this._app.Engine.RunOnMainThread(this, delegate
				{
					bool flag2 = this._app.Interface.AssetEditor.CurrentAsset.FilePath != assetPath;
					if (!flag2)
					{
						bool flag3 = block != null;
						if (flag3)
						{
							this._app.Editor.SetBlockPreview(block, camera);
						}
						else
						{
							bool flag4 = model != null;
							if (flag4)
							{
								this._app.Editor.SetModelPreview(model, camera);
							}
							else
							{
								this._app.Editor.ClearPreview(true);
							}
						}
					}
				}, false, false);
			}
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x001CA72C File Offset: 0x001C892C
		private void ProcessUpdateEditorTimeOverride(UpdateEditorTimeOverride packet)
		{
			InstantData gameTime = new InstantData(packet.GameTime);
			bool isPaused = packet.Paused;
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.GameTime.ProcessServerTimeUpdate(gameTime, isPaused);
			}, false, false);
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x001CA784 File Offset: 0x001C8984
		private void ProcessAssetEditorUpdateSecondsPerGameDay(AssetEditorUpdateSecondsPerGameDay packet)
		{
			int seconds = packet.SecondsPerGameDay;
			this._app.Engine.RunOnMainThread(this, delegate
			{
				this._app.Editor.GameTime.SecondsPerGameDay = seconds;
			}, false, false);
		}

		// Token: 0x0400393C RID: 14652
		private FileStream _blobFileStream;

		// Token: 0x0400393D RID: 14653
		private Asset _blobAsset;

		// Token: 0x0400393E RID: 14654
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400393F RID: 14655
		private readonly AssetEditorApp _app;

		// Token: 0x04003940 RID: 14656
		private readonly Stopwatch _stageStopwatch = Stopwatch.StartNew();

		// Token: 0x04003941 RID: 14657
		private readonly HashSet<string> _unhandledPacketTypes = new HashSet<string>();

		// Token: 0x04003942 RID: 14658
		private AssetEditorPacketHandler.ConnectionStage _stage = AssetEditorPacketHandler.ConnectionStage.Auth;

		// Token: 0x02000F83 RID: 3971
		[Flags]
		private enum ConnectionStage : byte
		{
			// Token: 0x04004B46 RID: 19270
			Auth = 2,
			// Token: 0x04004B47 RID: 19271
			Complete = 4
		}
	}
}
