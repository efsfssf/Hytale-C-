using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A5B RID: 2651
	internal class BlurProgram : GPUProgram
	{
		// Token: 0x0600544A RID: 21578 RVA: 0x00182E7E File Offset: 0x0018107E
		public BlurProgram(bool blurCustomChannels, string customChannelsToBlur = "rgba", string depthChannels = "", string variationName = null) : base("ScreenVS.glsl", "BlurFS.glsl", variationName)
		{
			this._blurCustomChannels = blurCustomChannels;
			this._channelsToBlur = customChannelsToBlur;
			this._depthChannels = depthChannels;
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x00182EAC File Offset: 0x001810AC
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(null);
			uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
			{
				{
					"USE_CUSTOM_CHANNELS",
					this._blurCustomChannels ? "1" : "0"
				},
				{
					"BLUR_CHANNELS",
					this._channelsToBlur
				},
				{
					"USE_EDGE_AWARENESS",
					this.UseEdgeAwareness ? "1" : "0"
				},
				{
					"DEPTH_CHANNELS",
					this._depthChannels
				}
			});
			return base.MakeProgram(vertexShader, fragmentShader, null, true, null);
		}

		// Token: 0x0600544C RID: 21580 RVA: 0x00182F4A File Offset: 0x0018114A
		protected override void InitUniforms()
		{
			GPUProgram._gl.UseProgram(this);
			this.ColorTexture.SetValue(0);
		}

		// Token: 0x04002F7A RID: 12154
		public Uniform PixelSize;

		// Token: 0x04002F7B RID: 12155
		public Uniform BlurScale;

		// Token: 0x04002F7C RID: 12156
		public Uniform HorizontalPass;

		// Token: 0x04002F7D RID: 12157
		private Uniform ColorTexture;

		// Token: 0x04002F7E RID: 12158
		public bool UseEdgeAwareness;

		// Token: 0x04002F7F RID: 12159
		private bool _blurCustomChannels;

		// Token: 0x04002F80 RID: 12160
		private string _depthChannels;

		// Token: 0x04002F81 RID: 12161
		private string _channelsToBlur;
	}
}
