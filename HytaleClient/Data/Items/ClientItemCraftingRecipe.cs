using System;
using Coherent.UI.Binding;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Items
{
	// Token: 0x02000AF8 RID: 2808
	[CoherentType]
	public class ClientItemCraftingRecipe
	{
		// Token: 0x06005846 RID: 22598 RVA: 0x001AC820 File Offset: 0x001AAA20
		public ClientItemCraftingRecipe(CraftingRecipe recipe)
		{
			bool flag = recipe.BenchRequirement_ != null;
			if (flag)
			{
				this.BenchRequirement = new ClientItemCraftingRecipe.ClientBenchRequirement[recipe.BenchRequirement_.Length];
				for (int i = 0; i < recipe.BenchRequirement_.Length; i++)
				{
					this.BenchRequirement[i] = new ClientItemCraftingRecipe.ClientBenchRequirement(recipe.BenchRequirement_[i]);
				}
			}
			bool flag2 = recipe.Input != null;
			if (flag2)
			{
				this.Input = new ClientItemCraftingRecipe.ClientCraftingMaterial[recipe.Input.Length];
				for (int j = 0; j < recipe.Input.Length; j++)
				{
					this.Input[j] = new ClientItemCraftingRecipe.ClientCraftingMaterial(recipe.Input[j]);
				}
			}
			else
			{
				this.Input = new ClientItemCraftingRecipe.ClientCraftingMaterial[0];
			}
			bool flag3 = recipe.Output != null;
			if (flag3)
			{
				this.Output = new ClientItemCraftingRecipe.ClientCraftingMaterial[recipe.Output.Length];
				for (int k = 0; k < recipe.Output.Length; k++)
				{
					this.Output[k] = new ClientItemCraftingRecipe.ClientCraftingMaterial(recipe.Output[k]);
				}
			}
			else
			{
				this.Output = new ClientItemCraftingRecipe.ClientCraftingMaterial[0];
			}
			this.TimeSeconds = recipe.TimeSeconds;
			this.KnowledgeRequired = recipe.KnowledgeRequired;
		}

		// Token: 0x040036CA RID: 14026
		[CoherentProperty("benchRequirement")]
		public readonly ClientItemCraftingRecipe.ClientBenchRequirement[] BenchRequirement;

		// Token: 0x040036CB RID: 14027
		[CoherentProperty("input")]
		public readonly ClientItemCraftingRecipe.ClientCraftingMaterial[] Input;

		// Token: 0x040036CC RID: 14028
		[CoherentProperty("output")]
		public readonly ClientItemCraftingRecipe.ClientCraftingMaterial[] Output;

		// Token: 0x040036CD RID: 14029
		[CoherentProperty("timeSeconds")]
		public readonly float TimeSeconds;

		// Token: 0x040036CE RID: 14030
		[CoherentProperty("knowledgeRequired")]
		public readonly bool KnowledgeRequired;

		// Token: 0x02000F21 RID: 3873
		[CoherentType]
		public class ClientCraftingMaterial
		{
			// Token: 0x0600683D RID: 26685 RVA: 0x0021A6BF File Offset: 0x002188BF
			public ClientCraftingMaterial(CraftingMaterial craftingMaterial)
			{
				this.ItemId = craftingMaterial.ItemId;
				this.ResourceTypeId = craftingMaterial.ResourceTypeId;
				this.Quantity = craftingMaterial.Quantity;
			}

			// Token: 0x04004A32 RID: 18994
			[CoherentProperty("itemId")]
			public readonly string ItemId;

			// Token: 0x04004A33 RID: 18995
			[CoherentProperty("resourceTypeId")]
			public readonly string ResourceTypeId;

			// Token: 0x04004A34 RID: 18996
			[CoherentProperty("quantity")]
			public readonly int Quantity;
		}

		// Token: 0x02000F22 RID: 3874
		[CoherentType]
		public class ClientBenchRequirement
		{
			// Token: 0x0600683E RID: 26686 RVA: 0x0021A6ED File Offset: 0x002188ED
			public ClientBenchRequirement(CraftingRecipe.BenchRequirement benchRequirement)
			{
				this.Type = benchRequirement.Type;
				this.Id = benchRequirement.Id;
				this.Categories = benchRequirement.Categories;
			}

			// Token: 0x04004A35 RID: 18997
			[CoherentProperty("type")]
			public readonly BenchType Type;

			// Token: 0x04004A36 RID: 18998
			[CoherentProperty("id")]
			public readonly string Id;

			// Token: 0x04004A37 RID: 18999
			[CoherentProperty("categories")]
			public readonly string[] Categories;
		}
	}
}
