using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.FX;
using HytaleClient.Graphics.Trails;
using HytaleClient.Math;
using HytaleClient.Networking;
using HytaleClient.Protocol;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules.Trails
{
	// Token: 0x020008FF RID: 2303
	internal class TrailStoreModule : Module
	{
		// Token: 0x06004526 RID: 17702 RVA: 0x000F0744 File Offset: 0x000EE944
		public TrailStoreModule(GameInstance gameInstance) : base(gameInstance)
		{
			this._gameInstance.Engine.FXSystem.Trails.InitializeFunction(new UpdateTrailLightingFunc(this.UpdateTrailLighting));
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x000F0794 File Offset: 0x000EE994
		public bool TrySpawnTrailProxy(string trailId, out TrailProxy trailProxy, bool isLocalPlayer = false)
		{
			trailProxy = null;
			TrailSettings trailSettings;
			bool flag = !this._trailSettingsById.TryGetValue(trailId, out trailSettings);
			bool result;
			if (flag)
			{
				this._gameInstance.App.DevTools.Error("Could not find trail settings: " + trailId);
				result = false;
			}
			else
			{
				Vector2 textureAltasInverseSize = new Vector2(1f / (float)this._gameInstance.FXModule.TextureAtlas.Width, 1f / (float)this._gameInstance.FXModule.TextureAtlas.Height);
				bool flag2 = this._gameInstance.Engine.FXSystem.Trails.TrySpawnTrail(trailSettings, textureAltasInverseSize, out trailProxy, isLocalPlayer);
				bool flag3 = !flag2;
				if (flag3)
				{
					this._gameInstance.App.DevTools.Error("Failed to spawn Trail '" + trailId + "' : max trails count limit reached.");
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x000F087C File Offset: 0x000EEA7C
		private void UpdateTrailLighting(Trail trail)
		{
			bool flag = trail.LightInfluence > 0f && !this._gameInstance.MapModule.Disposed;
			if (flag)
			{
				Vector4 vector = this._gameInstance.MapModule.GetLightColorAtBlockPosition((int)Math.Floor((double)trail.Position.X), (int)Math.Floor((double)trail.Position.Y), (int)Math.Floor((double)trail.Position.Z));
				vector = Vector4.Lerp(Vector4.One, vector, trail.LightInfluence);
				trail.UpdateLight(vector);
			}
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x000F0914 File Offset: 0x000EEB14
		protected override void DoDispose()
		{
			this._gameInstance.Engine.FXSystem.Trails.DisposeFunction();
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x000F0932 File Offset: 0x000EEB32
		public void Update(Vector3 cameraPosition)
		{
			this._gameInstance.Engine.FXSystem.Trails.UpdateProxies(cameraPosition, this.ProxyCheck);
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x000F0958 File Offset: 0x000EEB58
		public void PrepareTrails(Dictionary<string, Trail> networkTrails, out Dictionary<string, PacketHandler.TextureInfo> upcomingTextureInfo, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			upcomingTextureInfo = new Dictionary<string, PacketHandler.TextureInfo>();
			foreach (Trail trail in networkTrails.Values)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					break;
				}
				string text;
				bool flag = trail.Texture == null || !this._gameInstance.HashesByServerAssetPath.TryGetValue(trail.Texture, ref text);
				if (flag)
				{
					this._gameInstance.App.DevTools.Error("Missing trail texture: " + trail.Texture + " for trail " + trail.Id);
				}
				else
				{
					PacketHandler.TextureInfo textureInfo;
					bool flag2 = !upcomingTextureInfo.TryGetValue(text, out textureInfo);
					if (flag2)
					{
						textureInfo = new PacketHandler.TextureInfo
						{
							Checksum = text
						};
						bool flag3 = Image.TryGetPngDimensions(AssetManager.GetAssetLocalPathUsingHash(text), out textureInfo.Width, out textureInfo.Height);
						if (flag3)
						{
							upcomingTextureInfo[text] = textureInfo;
							bool flag4 = textureInfo.Width % 32 != 0 || textureInfo.Height % 32 != 0 || textureInfo.Width < 32 || textureInfo.Height < 32;
							if (flag4)
							{
								this._gameInstance.App.DevTools.Error(string.Format("Texture width/height must be a multiple of 32 and at least 32x32: {0} ({1}x{2})", trail.Texture, textureInfo.Width, textureInfo.Height));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x000F0B04 File Offset: 0x000EED04
		public void UpdateTextures()
		{
			foreach (TrailSettings trailSettings in this._trailSettingsById.Values)
			{
				string key;
				Rectangle imageLocation;
				bool flag = !this._gameInstance.HashesByServerAssetPath.TryGetValue(trailSettings.Texture, ref key) || !this._gameInstance.FXModule.ImageLocations.TryGetValue(key, out imageLocation);
				if (flag)
				{
					this._gameInstance.App.DevTools.Error("Failed to update trail texture: " + trailSettings.Texture + " for trail " + trailSettings.Id);
				}
				else
				{
					trailSettings.ImageLocation = imageLocation;
				}
			}
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x000F0BDC File Offset: 0x000EEDDC
		public void SetupTrailSettings(Dictionary<string, Trail> networkTrails)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			foreach (KeyValuePair<string, Trail> keyValuePair in networkTrails)
			{
				string key = keyValuePair.Key;
				Trail value = keyValuePair.Value;
				TrailSettings trailSettings = new TrailSettings();
				TrailProtocolInitializer.Initialize(value, ref trailSettings);
				bool flag = trailSettings.Texture == null;
				if (!flag)
				{
					bool flag2 = !this._gameInstance.HashesByServerAssetPath.ContainsKey(trailSettings.Texture);
					if (flag2)
					{
						this._gameInstance.App.DevTools.Error("Failed to find trail texture: " + trailSettings.Texture + " for trail " + key);
						bool flag3 = this._trailSettingsById.ContainsKey(key);
						if (flag3)
						{
							this._trailSettingsById.Remove(key);
						}
					}
					else
					{
						this._trailSettingsById[key] = trailSettings;
					}
				}
			}
		}

		// Token: 0x040022B3 RID: 8883
		private Dictionary<string, TrailSettings> _trailSettingsById = new Dictionary<string, TrailSettings>();

		// Token: 0x040022B4 RID: 8884
		public bool ProxyCheck = true;
	}
}
