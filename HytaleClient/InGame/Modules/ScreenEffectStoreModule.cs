using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Core;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Programs;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008FB RID: 2299
	internal class ScreenEffectStoreModule : Module
	{
		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x06004488 RID: 17544 RVA: 0x000E9D66 File Offset: 0x000E7F66
		// (set) Token: 0x06004489 RID: 17545 RVA: 0x000E9D6E File Offset: 0x000E7F6E
		public int ScreenEffectDrawCount { get; private set; } = 0;

		// Token: 0x0600448A RID: 17546 RVA: 0x000E9D78 File Offset: 0x000E7F78
		public ScreenEffectStoreModule(GameInstance gameInstance) : base(gameInstance)
		{
			this.WeatherScreenEffectRenderer = new ScreenEffectRenderer(this._gameInstance.Engine);
			BasicProgram basicProgram = gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram;
			this._quadRenderer = new QuadRenderer(gameInstance.Engine.Graphics, basicProgram.AttribPosition, basicProgram.AttribTexCoords);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x000E9E08 File Offset: 0x000E8008
		protected override void DoDispose()
		{
			this._quadRenderer.Dispose();
			this.WeatherScreenEffectRenderer.Dispose();
			foreach (ScreenEffectStoreModule.UniqueScreenEffect uniqueScreenEffect in this.EntityScreenEffects.Values)
			{
				uniqueScreenEffect.ScreenEffectRenderer.Dispose();
			}
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x000E9E80 File Offset: 0x000E8080
		public override void Initialize()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.WeatherScreenEffectRenderer.Initialize();
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x000E9E9C File Offset: 0x000E809C
		public void AddEntityScreenEffect(string assetPath)
		{
			ScreenEffectStoreModule.UniqueScreenEffect uniqueScreenEffect;
			bool flag = this.EntityScreenEffects.TryGetValue(assetPath, out uniqueScreenEffect);
			if (flag)
			{
				uniqueScreenEffect.Instances++;
			}
			else
			{
				uniqueScreenEffect = new ScreenEffectStoreModule.UniqueScreenEffect(this._gameInstance.Engine);
				string targetScreenEffectTextureChecksum;
				bool flag2 = !this._gameInstance.HashesByServerAssetPath.TryGetValue(assetPath, ref targetScreenEffectTextureChecksum);
				if (flag2)
				{
					this._gameInstance.App.DevTools.Error("Missing entity screen effect asset " + assetPath);
				}
				else
				{
					uniqueScreenEffect.ScreenEffectRenderer.RequestTextureUpdate(targetScreenEffectTextureChecksum, false);
					this.EntityScreenEffects.Add(assetPath, uniqueScreenEffect);
				}
			}
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x000E9F3C File Offset: 0x000E813C
		public void RemoveEntityScreenEffect(string assetPath)
		{
			ScreenEffectStoreModule.UniqueScreenEffect uniqueScreenEffect;
			bool flag = this.EntityScreenEffects.TryGetValue(assetPath, out uniqueScreenEffect);
			if (flag)
			{
				uniqueScreenEffect.Instances--;
			}
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x000E9F6C File Offset: 0x000E816C
		public void Update(float deltaTime)
		{
			this.ScreenEffectDrawCount = 0;
			foreach (KeyValuePair<string, ScreenEffectStoreModule.UniqueScreenEffect> keyValuePair in this.EntityScreenEffects)
			{
				bool flag = keyValuePair.Value.Instances > 0;
				if (flag)
				{
					bool isScreenEffectTextureLoading = keyValuePair.Value.ScreenEffectRenderer.IsScreenEffectTextureLoading;
					if (isScreenEffectTextureLoading)
					{
						keyValuePair.Value.ScreenEffectRenderer.Color.W = 0f;
					}
					else
					{
						keyValuePair.Value.ScreenEffectRenderer.Color.W = MathHelper.Lerp(keyValuePair.Value.ScreenEffectRenderer.Color.W, 1f, 3f * deltaTime);
					}
				}
				else
				{
					keyValuePair.Value.ScreenEffectRenderer.Color.W = MathHelper.Lerp(keyValuePair.Value.ScreenEffectRenderer.Color.W, 0f, 3f * deltaTime);
					bool flag2 = keyValuePair.Value.ScreenEffectRenderer.Color.W <= 0.01f;
					if (flag2)
					{
						keyValuePair.Value.ScreenEffectRenderer.Color.W = 0f;
						this._entityScreenEffectToRemove.Add(keyValuePair.Key);
					}
				}
				bool flag3 = keyValuePair.Value.ScreenEffectRenderer.Color.W > 0f;
				if (flag3)
				{
					this.RequestDraw(keyValuePair.Value.ScreenEffectRenderer);
				}
			}
			for (int i = 0; i < this._entityScreenEffectToRemove.Count; i++)
			{
				this.EntityScreenEffects[this._entityScreenEffectToRemove[i]].ScreenEffectRenderer.Dispose();
				this.EntityScreenEffects.Remove(this._entityScreenEffectToRemove[i]);
			}
			this._entityScreenEffectToRemove.Clear();
			bool flag4 = this.WeatherScreenEffectRenderer.HasTexture && this.WeatherScreenEffectRenderer.Color.W > 0f;
			if (flag4)
			{
				this.RequestDraw(this.WeatherScreenEffectRenderer);
			}
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x000EA1D0 File Offset: 0x000E83D0
		public void RequestDraw(ScreenEffectRenderer screenEffectRenderer)
		{
			bool flag = this.ScreenEffectDrawCount == this.ScreenEffectDraw.Length;
			if (flag)
			{
				Array.Resize<ScreenEffectRenderer>(ref this.ScreenEffectDraw, this.ScreenEffectDraw.Length + 2);
			}
			this.ScreenEffectDraw[this.ScreenEffectDrawCount] = screenEffectRenderer;
			int screenEffectDrawCount = this.ScreenEffectDrawCount;
			this.ScreenEffectDrawCount = screenEffectDrawCount + 1;
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x000EA227 File Offset: 0x000E8427
		[Obsolete]
		public bool NeedsDrawing()
		{
			return this.ScreenEffectDrawCount > 0;
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x000EA234 File Offset: 0x000E8434
		public void Draw()
		{
			GLFunctions gl = this._gameInstance.Engine.Graphics.GL;
			BasicProgram basicProgram = this._gameInstance.Engine.Graphics.GPUProgramStore.BasicProgram;
			basicProgram.MVPMatrix.SetValue(ref this._gameInstance.Engine.Graphics.ScreenMatrix);
			for (int i = 0; i < this.ScreenEffectDrawCount; i++)
			{
				gl.BindTexture(GL.TEXTURE_2D, this.ScreenEffectDraw[i].ScreenEffectTexture);
				Vector4 color = this.ScreenEffectDraw[i].Color;
				basicProgram.Color.SetValue(color.X, color.Y, color.Z);
				basicProgram.Opacity.SetValue(color.W);
				this._quadRenderer.Draw();
			}
		}

		// Token: 0x04002200 RID: 8704
		private const float EntityEffectLerp = 3f;

		// Token: 0x04002201 RID: 8705
		private QuadRenderer _quadRenderer;

		// Token: 0x04002202 RID: 8706
		private List<string> _entityScreenEffectToRemove = new List<string>();

		// Token: 0x04002203 RID: 8707
		public readonly ScreenEffectRenderer WeatherScreenEffectRenderer;

		// Token: 0x04002204 RID: 8708
		public readonly Dictionary<string, ScreenEffectStoreModule.UniqueScreenEffect> EntityScreenEffects = new Dictionary<string, ScreenEffectStoreModule.UniqueScreenEffect>();

		// Token: 0x04002205 RID: 8709
		private const int ScreenEffectDrawDefaultSize = 2;

		// Token: 0x04002206 RID: 8710
		private const int ScreenEffectDrawGrowth = 2;

		// Token: 0x04002208 RID: 8712
		public ScreenEffectRenderer[] ScreenEffectDraw = new ScreenEffectRenderer[2];

		// Token: 0x02000DC9 RID: 3529
		public class UniqueScreenEffect
		{
			// Token: 0x06006649 RID: 26185 RVA: 0x00213950 File Offset: 0x00211B50
			public UniqueScreenEffect(Engine engine)
			{
				this.ScreenEffectRenderer = new ScreenEffectRenderer(engine);
				this.ScreenEffectRenderer.Initialize();
				this.ScreenEffectRenderer.Color = new Vector4(1f, 1f, 1f, 0f);
				this.Instances++;
			}

			// Token: 0x040043F1 RID: 17393
			public int Instances = 0;

			// Token: 0x040043F2 RID: 17394
			public ScreenEffectRenderer ScreenEffectRenderer;
		}
	}
}
