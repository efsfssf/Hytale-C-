using System;
using System.Collections.Generic;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A8D RID: 2701
	internal class ZOnlyProgram : GPUProgram
	{
		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x0600551D RID: 21789 RVA: 0x00188498 File Offset: 0x00186698
		// (set) Token: 0x0600551E RID: 21790 RVA: 0x001884B0 File Offset: 0x001866B0
		public bool AlphaTest
		{
			get
			{
				return this._alphaTest;
			}
			set
			{
				this._alphaTest = value;
				this._fragmentShaderResource.FileName = (value ? "ZOnlyFS.glsl" : null);
			}
		}

		// Token: 0x0600551F RID: 21791 RVA: 0x001884D0 File Offset: 0x001866D0
		public ZOnlyProgram(bool alphaTest, string variationName = null) : base("ZOnlyVS.glsl", "ZOnlyFS.glsl", variationName)
		{
			this.AlphaTest = alphaTest;
		}

		// Token: 0x06005520 RID: 21792 RVA: 0x001884F0 File Offset: 0x001866F0
		public override bool Initialize()
		{
			base.Initialize();
			uint vertexShader = base.CompileVertexShader(new Dictionary<string, string>
			{
				{
					"ALPHA_TEST",
					this.AlphaTest ? "1" : "0"
				}
			});
			List<GPUProgram.AttribBindingInfo> list = new List<GPUProgram.AttribBindingInfo>(5);
			list.Add(new GPUProgram.AttribBindingInfo(0U, "vertPosition"));
			bool alphaTest = this.AlphaTest;
			bool result;
			if (alphaTest)
			{
				uint fragmentShader = base.CompileFragmentShader(new Dictionary<string, string>
				{
					{
						"ALPHA_TEST",
						this.AlphaTest ? "1" : "0"
					}
				});
				bool alphaTest2 = this.AlphaTest;
				if (alphaTest2)
				{
					list.Add(new GPUProgram.AttribBindingInfo(2U, "vertTexCoords"));
				}
				result = base.MakeProgram(vertexShader, fragmentShader, list, true, null);
			}
			else
			{
				result = base.MakeProgram(vertexShader, list, true, null);
			}
			return result;
		}

		// Token: 0x06005521 RID: 21793 RVA: 0x001885D0 File Offset: 0x001867D0
		protected override void InitUniforms()
		{
			bool alphaTest = this.AlphaTest;
			if (alphaTest)
			{
				GPUProgram._gl.UseProgram(this);
				this.Texture.SetValue(0);
			}
		}

		// Token: 0x040031AA RID: 12714
		public Uniform ModelMatrix;

		// Token: 0x040031AB RID: 12715
		public Uniform ViewProjectionMatrix;

		// Token: 0x040031AC RID: 12716
		public Uniform Time;

		// Token: 0x040031AD RID: 12717
		public Uniform TargetCascades;

		// Token: 0x040031AE RID: 12718
		public Uniform ViewportInfos;

		// Token: 0x040031AF RID: 12719
		private Uniform Texture;

		// Token: 0x040031B0 RID: 12720
		public readonly Attrib AttribPosition;

		// Token: 0x040031B1 RID: 12721
		private bool _alphaTest;
	}
}
