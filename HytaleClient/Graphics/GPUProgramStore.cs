using System;
using System.IO;
using System.Reflection;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics.Programs;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Graphics
{
	// Token: 0x02000A3A RID: 2618
	public class GPUProgramStore
	{
		// Token: 0x0600521A RID: 21018 RVA: 0x001681A0 File Offset: 0x001663A0
		public GPUProgramStore(GraphicsDevice graphicsDevice)
		{
			this._graphicsDevice = graphicsDevice;
			GPUProgram.CreateFallbacks();
			GPUProgram.SetShaderCodeDumpPolicy(GPUProgram.ShaderCodeDumpPolicy.OnError);
			GPUProgram.SetResourcePaths("HytaleClient.Graphics.Shaders", Path.Combine(new string[]
			{
				Paths.App,
				"..",
				"..",
				"..",
				"..",
				"HytaleClient",
				"Graphics",
				"Shaders"
			}), Path.Combine(Paths.UserData, "Shaders"));
			int maxNodeCount = BlockyModel.MaxNodeCount;
			this.BasicProgram = new BasicProgram(true, "BasicProgram");
			this.BasicFogProgram = new BasicFogProgram();
			this.BlockyModelForwardProgram = new BlockyModelProgram(false, true, false, false, false, false, "BlockyModelForwardProgram");
			this.BlockyModelDitheringProgram = new BlockyModelProgram(false, false, !this._graphicsDevice.UseDeferredLight, false, true, false, "BlockyModelDitheringProgram");
			this.BlockyModelProgram = new BlockyModelProgram(this._graphicsDevice.UseDeferredLight, false, !this._graphicsDevice.UseDeferredLight, false, true, false, "BlockyModelProgram");
			this.FirstPersonBlockyModelProgram = new BlockyModelProgram(this._graphicsDevice.UseDeferredLight, false, !this._graphicsDevice.UseDeferredLight, true, false, false, "FirstPersonBlockyModelProgram");
			this.FirstPersonClippingBlockyModelProgram = new BlockyModelProgram(this._graphicsDevice.UseDeferredLight, false, !this._graphicsDevice.UseDeferredLight, false, false, false, "FirstPersonClippingBlockyModelProgram");
			this.BlockyModelDistortionProgram = new BlockyModelProgram(false, false, !this._graphicsDevice.UseDeferredLight, false, true, true, "BlockyModelDistortionProgram");
			this.FirstPersonDistortionBlockyModelProgram = new BlockyModelProgram(false, false, !this._graphicsDevice.UseDeferredLight, true, false, true, "FirstPersonDistortionBlockyModelProgram");
			this.MapChunkNearOpaqueProgram = new MapChunkNearProgram(false, this._graphicsDevice.UseDeferredLight, false, "MapChunkNearOpaqueProgram");
			this.MapChunkNearAlphaTestedProgram = new MapChunkNearProgram(true, this._graphicsDevice.UseDeferredLight, true, "MapChunkNearAlphaTestedProgram");
			this.MapChunkFarOpaqueProgram = new MapChunkFarProgram(false, this._graphicsDevice.UseDeferredLight, false, "MapChunkFarOpaqueProgram");
			this.MapChunkFarAlphaTestedProgram = new MapChunkFarProgram(true, this._graphicsDevice.UseDeferredLight, true, "MapChunkFarAlphaTestedProgram");
			this.MapChunkAlphaBlendedProgram = new MapChunkAlphaBlendedProgram(false, "MapChunkAlphaBlendedProgram");
			this.MapBlockAnimatedProgram = new MapBlockAnimatedProgram(maxNodeCount, this._graphicsDevice.UseDeferredLight, false, true, "MapBlockAnimatedProgram");
			this.MapBlockAnimatedForwardProgram = new MapBlockAnimatedProgram(maxNodeCount, false, true, false, "MapBlockAnimatedForwardProgram");
			this.SkyProgram = new SkyProgram();
			this.CloudsProgram = new CloudsProgram();
			this.TextProgram = new TextProgram();
			this.ParticleProgram = new ParticleProgram(true, true, true, true, false, false, null);
			this.ParticleErosionProgram = new ParticleProgram(true, true, true, true, false, true, "ParticleErosionProgram");
			this.ParticleDistortionProgram = new ParticleProgram(false, false, false, false, true, false, "ParticleDistortionProgram");
			this.ForceFieldProgram = new ForceFieldProgram(false, false, null);
			this.BuilderToolProgram = new ForceFieldProgram(true, true, "BuilderToolProgram");
			this.WorldMapProgram = new WorldMapProgram();
			this.PostEffectProgram = new PostEffectProgram(this._graphicsDevice.UseReverseZ, false, false, "PostEffectProgram");
			this.InventoryPostEffectProgram = new PostEffectProgram(this._graphicsDevice.UseReverseZ, true, true, "InventoryPostEffectProgram");
			this.MainMenuPostEffectProgram = new PostEffectProgram(this._graphicsDevice.UseReverseZ, true, false, "MainMenuPostEffectProgram");
			this.TemporalAAProgram = new TemporalAAProgram();
			this.OITCompositeProgram = new OITCompositeProgram();
			this.CubemapProgram = new CubemapProgram();
			this.ZDownsampleProgram = new ZDownsampleProgram(false, true, false, "ZDownsampleProgram");
			this.LinearZDownsampleProgram = new ZDownsampleProgram(true, false, true, "LinearZDownsampleProgram");
			this.EdgeDetectionProgram = new EdgeDetectionProgram();
			this.LinearZProgram = new LinearZProgram();
			this.LightMixProgram = new LightMixProgram(null);
			this.LightProgram = new LightProgram(256);
			this.LightLowResProgram = new LightProgram(256);
			this.LightClusteredProgram = new LightClusteredProgram();
			this.MapChunkShadowMapProgram = new ZOnlyChunkProgram(true, false, 0, true, true, true, 0f, "MapChunkShadowMapProgram");
			this.MapBlockAnimatedShadowMapProgram = new ZOnlyChunkProgram(true, true, maxNodeCount, true, true, true, 0f, "MapBlockAnimatedShadowMapProgram");
			this.BlockyModelShadowMapProgram = new ZOnlyBlockyModelProgram(this.BlockyModelProgram, true, null);
			this.BlockyModelOcclusionMapProgram = new ZOnlyBlockyModelProgram(this.BlockyModelProgram, true, null);
			this.DeferredShadowProgram = new DeferredShadowProgram();
			this.VolumetricSunshaftProgram = new VolumetricSunshaftProgram();
			this.SSAOProgram = new SSAOProgram();
			this.BlurSSAOAndShadowProgram = new BlurProgram(true, "ra", "gb", "BlurSSAOAndShadowProgram");
			this.DeferredProgram = new DeferredProgram(this._graphicsDevice.UseReverseZ, this._graphicsDevice.UseDownsampledZ, true, this._graphicsDevice.UseDeferredLight, this._graphicsDevice.UseLowResDeferredLighting, true);
			this.ScreenBlitProgram = new ScreenBlitProgram(true, null);
			this.BlurProgram = new BlurProgram(false, "rgba", "", "BlurProgram");
			this.BlurProgram.UseEdgeAwareness = false;
			this.DoFBlurProgram = new DoFBlurProgram();
			this.DoFCircleOfConfusionProgram = new DoFCircleOfConfusionProgram();
			this.DoFDownsampleProgram = new DoFDownsampleProgram();
			this.DoFNearCoCMaxProgram = new MaxProgram(false, 6, "DoFNearCoCMaxProgram");
			this.DoFNearCoCBlurProgram = new DoFNearCoCBlurProgram();
			this.DepthOfFieldAdvancedProgram = new DepthOfFieldAdvancedProgram();
			this.DoFFillProgram = new DoFFillProgram();
			this.BloomSelectProgram = new BloomSelectProgram();
			this.BloomDownsampleBlurProgram = new BloomDownsampleBlurProgram();
			this.BloomUpsampleBlurProgram = new BloomUpsampleBlurProgram();
			this.BloomMaxProgram = new MaxProgram(true, 1, "BloomMaxProgram");
			this.BloomCompositeProgram = new BloomCompositeProgram();
			this.RadialGlowMaskProgram = new RadialGlowMaskProgram();
			this.RadialGlowLuminanceProgram = new RadialGlowLuminanceProgram(8);
			this.SunOcclusionDownsampleProgram = new SunOcclusionDownsampleProgram();
			this.SceneBrightnessPackProgram = new SceneBrightnessPackProgram();
			this.ZOnlyMapChunkProgram = new ZOnlyChunkProgram(false, false, 0, true, true, false, -2f, "ZOnlyMapChunkProgram");
			this.ZOnlyMapChunkPlanesProgram = new ZOnlyChunkPlanesProgram(null);
			this.ZOnlyProgram = new ZOnlyProgram(false, "ZOnlyProgram");
			this.HiZReprojectProgram = new HiZReprojectProgram();
			this.HiZFillHoleProgram = new HiZFillHoleProgram();
			this.HiZBuildProgram = new HiZBuildProgram();
			this.HiZCullProgram = new HiZCullProgram();
			this.DebugDrawMapProgram = new DebugDrawMapProgram();
			this.Batcher2DProgram = new Batcher2DProgram();
			this.InitializeAllPrograms(false, false);
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x001687C9 File Offset: 0x001669C9
		public void Release()
		{
			this.ReleaseAllPrograms(false);
			GPUProgram.DestroyFallbacks();
		}

		// Token: 0x0600521C RID: 21020 RVA: 0x001687DC File Offset: 0x001669DC
		private void InitializeAllPrograms(bool releaseFirst = false, bool forceReset = false)
		{
			string text = "";
			foreach (FieldInfo fieldInfo in base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				bool flag = !fieldInfo.FieldType.IsSubclassOf(typeof(GPUProgram));
				if (!flag)
				{
					GPUProgram gpuprogram = fieldInfo.GetValue(this) as GPUProgram;
					bool flag2;
					if (releaseFirst)
					{
						flag2 = gpuprogram.Reset(forceReset);
					}
					else
					{
						flag2 = gpuprogram.Initialize();
					}
					bool flag3 = !flag2;
					if (flag3)
					{
						text = text + fieldInfo.Name + "\n";
					}
				}
			}
			bool flag4 = text.Length == 0;
			bool flag5 = !flag4;
			if (flag5)
			{
				string message = "Summary : Errors encountered during the building of GPU Programs :\n" + text + "...(see details above).";
				GPUProgramStore.Logger.Error(message);
			}
			else
			{
				GPUProgramStore.Logger.Info("Summary : all GPU Programs were built successfully!");
			}
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x001688CC File Offset: 0x00166ACC
		private void ReleaseAllPrograms(bool releaseFirst = false)
		{
			foreach (FieldInfo fieldInfo in base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				bool flag = !fieldInfo.FieldType.IsSubclassOf(typeof(GPUProgram));
				if (!flag)
				{
					GPUProgram gpuprogram = fieldInfo.GetValue(this) as GPUProgram;
					gpuprogram.Release();
				}
			}
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x00168934 File Offset: 0x00166B34
		public void ResetPrograms(bool forceReset)
		{
			this.PostEffectProgram.ReverseZ = this._graphicsDevice.UseReverseZ;
			this.MainMenuPostEffectProgram.ReverseZ = this._graphicsDevice.UseReverseZ;
			this.BlockyModelProgram.Deferred = this._graphicsDevice.UseDeferredLight;
			this.MapChunkNearOpaqueProgram.Deferred = this._graphicsDevice.UseDeferredLight;
			this.MapChunkFarOpaqueProgram.Deferred = this._graphicsDevice.UseDeferredLight;
			this.MapBlockAnimatedProgram.Deferred = this._graphicsDevice.UseDeferredLight;
			this.DeferredProgram.UseLight = this._graphicsDevice.UseDeferredLight;
			this.DeferredProgram.UseDownsampledZ = this._graphicsDevice.UseDownsampledZ;
			this.DeferredProgram.UseLowResLighting = this._graphicsDevice.UseLowResDeferredLighting;
			this.DeferredProgram.UseLinearZ = this._graphicsDevice.UseLinearZ;
			this.LightProgram.UseLinearZ = this._graphicsDevice.UseLinearZForLight;
			this.LightLowResProgram.UseLinearZ = this._graphicsDevice.UseLinearZForLight;
			this.LightClusteredProgram.UseLinearZ = this._graphicsDevice.UseLinearZForLight;
			this.InitializeAllPrograms(true, forceReset);
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x00168A6C File Offset: 0x00166C6C
		public void ResetProgramUniforms()
		{
			foreach (FieldInfo fieldInfo in base.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				bool flag = !fieldInfo.FieldType.IsSubclassOf(typeof(GPUProgram));
				if (!flag)
				{
					GPUProgram gpuprogram = fieldInfo.GetValue(this) as GPUProgram;
					gpuprogram.ResetUniforms();
				}
			}
		}

		// Token: 0x04002CD5 RID: 11477
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04002CD6 RID: 11478
		internal readonly BasicProgram BasicProgram;

		// Token: 0x04002CD7 RID: 11479
		internal readonly BasicFogProgram BasicFogProgram;

		// Token: 0x04002CD8 RID: 11480
		internal readonly BlockyModelProgram BlockyModelForwardProgram;

		// Token: 0x04002CD9 RID: 11481
		internal readonly BlockyModelProgram BlockyModelDitheringProgram;

		// Token: 0x04002CDA RID: 11482
		internal readonly BlockyModelProgram BlockyModelProgram;

		// Token: 0x04002CDB RID: 11483
		internal readonly BlockyModelProgram FirstPersonBlockyModelProgram;

		// Token: 0x04002CDC RID: 11484
		internal readonly BlockyModelProgram BlockyModelDistortionProgram;

		// Token: 0x04002CDD RID: 11485
		internal readonly BlockyModelProgram FirstPersonDistortionBlockyModelProgram;

		// Token: 0x04002CDE RID: 11486
		internal readonly BlockyModelProgram FirstPersonClippingBlockyModelProgram;

		// Token: 0x04002CDF RID: 11487
		internal readonly MapChunkNearProgram MapChunkNearOpaqueProgram;

		// Token: 0x04002CE0 RID: 11488
		internal readonly MapChunkNearProgram MapChunkNearAlphaTestedProgram;

		// Token: 0x04002CE1 RID: 11489
		internal readonly MapChunkFarProgram MapChunkFarOpaqueProgram;

		// Token: 0x04002CE2 RID: 11490
		internal readonly MapChunkFarProgram MapChunkFarAlphaTestedProgram;

		// Token: 0x04002CE3 RID: 11491
		internal readonly MapChunkAlphaBlendedProgram MapChunkAlphaBlendedProgram;

		// Token: 0x04002CE4 RID: 11492
		internal readonly MapBlockAnimatedProgram MapBlockAnimatedProgram;

		// Token: 0x04002CE5 RID: 11493
		internal readonly MapBlockAnimatedProgram MapBlockAnimatedForwardProgram;

		// Token: 0x04002CE6 RID: 11494
		internal readonly SkyProgram SkyProgram;

		// Token: 0x04002CE7 RID: 11495
		internal readonly CloudsProgram CloudsProgram;

		// Token: 0x04002CE8 RID: 11496
		internal readonly TextProgram TextProgram;

		// Token: 0x04002CE9 RID: 11497
		internal readonly ParticleProgram ParticleProgram;

		// Token: 0x04002CEA RID: 11498
		internal readonly ParticleProgram ParticleErosionProgram;

		// Token: 0x04002CEB RID: 11499
		internal readonly ParticleProgram ParticleDistortionProgram;

		// Token: 0x04002CEC RID: 11500
		internal readonly ForceFieldProgram ForceFieldProgram;

		// Token: 0x04002CED RID: 11501
		internal readonly ForceFieldProgram BuilderToolProgram;

		// Token: 0x04002CEE RID: 11502
		internal readonly WorldMapProgram WorldMapProgram;

		// Token: 0x04002CEF RID: 11503
		internal readonly PostEffectProgram PostEffectProgram;

		// Token: 0x04002CF0 RID: 11504
		internal readonly PostEffectProgram InventoryPostEffectProgram;

		// Token: 0x04002CF1 RID: 11505
		internal readonly PostEffectProgram MainMenuPostEffectProgram;

		// Token: 0x04002CF2 RID: 11506
		internal readonly TemporalAAProgram TemporalAAProgram;

		// Token: 0x04002CF3 RID: 11507
		internal readonly OITCompositeProgram OITCompositeProgram;

		// Token: 0x04002CF4 RID: 11508
		internal readonly CubemapProgram CubemapProgram;

		// Token: 0x04002CF5 RID: 11509
		internal readonly ZDownsampleProgram ZDownsampleProgram;

		// Token: 0x04002CF6 RID: 11510
		internal readonly ZDownsampleProgram LinearZDownsampleProgram;

		// Token: 0x04002CF7 RID: 11511
		internal readonly EdgeDetectionProgram EdgeDetectionProgram;

		// Token: 0x04002CF8 RID: 11512
		internal readonly LinearZProgram LinearZProgram;

		// Token: 0x04002CF9 RID: 11513
		internal readonly LightMixProgram LightMixProgram;

		// Token: 0x04002CFA RID: 11514
		internal readonly LightProgram LightProgram;

		// Token: 0x04002CFB RID: 11515
		internal readonly LightProgram LightLowResProgram;

		// Token: 0x04002CFC RID: 11516
		internal readonly LightClusteredProgram LightClusteredProgram;

		// Token: 0x04002CFD RID: 11517
		internal readonly ZOnlyChunkProgram MapChunkShadowMapProgram;

		// Token: 0x04002CFE RID: 11518
		internal readonly ZOnlyChunkProgram MapBlockAnimatedShadowMapProgram;

		// Token: 0x04002CFF RID: 11519
		internal readonly ZOnlyBlockyModelProgram BlockyModelShadowMapProgram;

		// Token: 0x04002D00 RID: 11520
		internal readonly ZOnlyBlockyModelProgram BlockyModelOcclusionMapProgram;

		// Token: 0x04002D01 RID: 11521
		internal readonly DeferredShadowProgram DeferredShadowProgram;

		// Token: 0x04002D02 RID: 11522
		internal readonly VolumetricSunshaftProgram VolumetricSunshaftProgram;

		// Token: 0x04002D03 RID: 11523
		internal readonly SSAOProgram SSAOProgram;

		// Token: 0x04002D04 RID: 11524
		internal readonly BlurProgram BlurSSAOAndShadowProgram;

		// Token: 0x04002D05 RID: 11525
		internal readonly DeferredProgram DeferredProgram;

		// Token: 0x04002D06 RID: 11526
		internal readonly ScreenBlitProgram ScreenBlitProgram;

		// Token: 0x04002D07 RID: 11527
		internal readonly BlurProgram BlurProgram;

		// Token: 0x04002D08 RID: 11528
		internal readonly DoFBlurProgram DoFBlurProgram;

		// Token: 0x04002D09 RID: 11529
		internal readonly DoFCircleOfConfusionProgram DoFCircleOfConfusionProgram;

		// Token: 0x04002D0A RID: 11530
		internal readonly DoFDownsampleProgram DoFDownsampleProgram;

		// Token: 0x04002D0B RID: 11531
		internal readonly MaxProgram DoFNearCoCMaxProgram;

		// Token: 0x04002D0C RID: 11532
		internal readonly DoFNearCoCBlurProgram DoFNearCoCBlurProgram;

		// Token: 0x04002D0D RID: 11533
		internal readonly DepthOfFieldAdvancedProgram DepthOfFieldAdvancedProgram;

		// Token: 0x04002D0E RID: 11534
		internal readonly DoFFillProgram DoFFillProgram;

		// Token: 0x04002D0F RID: 11535
		internal readonly BloomSelectProgram BloomSelectProgram;

		// Token: 0x04002D10 RID: 11536
		internal readonly BloomDownsampleBlurProgram BloomDownsampleBlurProgram;

		// Token: 0x04002D11 RID: 11537
		internal readonly BloomUpsampleBlurProgram BloomUpsampleBlurProgram;

		// Token: 0x04002D12 RID: 11538
		internal readonly MaxProgram BloomMaxProgram;

		// Token: 0x04002D13 RID: 11539
		internal readonly BloomCompositeProgram BloomCompositeProgram;

		// Token: 0x04002D14 RID: 11540
		internal readonly RadialGlowMaskProgram RadialGlowMaskProgram;

		// Token: 0x04002D15 RID: 11541
		internal readonly RadialGlowLuminanceProgram RadialGlowLuminanceProgram;

		// Token: 0x04002D16 RID: 11542
		internal readonly SunOcclusionDownsampleProgram SunOcclusionDownsampleProgram;

		// Token: 0x04002D17 RID: 11543
		internal readonly SceneBrightnessPackProgram SceneBrightnessPackProgram;

		// Token: 0x04002D18 RID: 11544
		internal readonly ZOnlyChunkProgram ZOnlyMapChunkProgram;

		// Token: 0x04002D19 RID: 11545
		internal readonly ZOnlyChunkPlanesProgram ZOnlyMapChunkPlanesProgram;

		// Token: 0x04002D1A RID: 11546
		internal readonly ZOnlyProgram ZOnlyProgram;

		// Token: 0x04002D1B RID: 11547
		internal readonly HiZReprojectProgram HiZReprojectProgram;

		// Token: 0x04002D1C RID: 11548
		internal readonly HiZFillHoleProgram HiZFillHoleProgram;

		// Token: 0x04002D1D RID: 11549
		internal readonly HiZBuildProgram HiZBuildProgram;

		// Token: 0x04002D1E RID: 11550
		internal readonly HiZCullProgram HiZCullProgram;

		// Token: 0x04002D1F RID: 11551
		internal readonly DebugDrawMapProgram DebugDrawMapProgram;

		// Token: 0x04002D20 RID: 11552
		internal readonly Batcher2DProgram Batcher2DProgram;

		// Token: 0x04002D21 RID: 11553
		private GraphicsDevice _graphicsDevice;
	}
}
