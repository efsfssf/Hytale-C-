using System;
using System.Diagnostics;

namespace HytaleClient.Graphics
{
	// Token: 0x020009A3 RID: 2467
	internal class FXMemoryPool
	{
		// Token: 0x06004F3A RID: 20282 RVA: 0x0016450C File Offset: 0x0016270C
		public static void UnitTest()
		{
			FXMemoryPool<FXMemoryPool.TestDataPooled> fxmemoryPool = new FXMemoryPool<FXMemoryPool.TestDataPooled>();
			fxmemoryPool.Initialize(10000);
			FXMemoryPool.TestData[] array = new FXMemoryPool.TestData[]
			{
				new FXMemoryPool.TestData
				{
					Input = 1,
					ExpectedResult = 0
				},
				new FXMemoryPool.TestData
				{
					Input = 31,
					ExpectedResult = 0
				},
				new FXMemoryPool.TestData
				{
					Input = 127,
					ExpectedResult = 0
				},
				new FXMemoryPool.TestData
				{
					Input = 128,
					ExpectedResult = 0
				}
			};
			array[0].Result = fxmemoryPool.TakeSlots(array[0].Input);
			fxmemoryPool.ReleaseSlots(array[0].Result, array[0].Input);
			array[1].Result = fxmemoryPool.TakeSlots(array[1].Input);
			fxmemoryPool.ReleaseSlots(array[1].Result, array[1].Input);
			array[2].Result = fxmemoryPool.TakeSlots(array[2].Input);
			fxmemoryPool.ReleaseSlots(array[2].Result, array[2].Input);
			array[3].Result = fxmemoryPool.TakeSlots(array[3].Input);
			fxmemoryPool.ReleaseSlots(array[3].Result, array[3].Input);
			for (int i = 0; i < 4; i++)
			{
				Debug.Assert(array[i].Result == array[i].ExpectedResult, string.Format("Error in test data {0}.", i));
			}
			array[0] = new FXMemoryPool.TestData
			{
				Input = 1,
				ExpectedResult = 0
			};
			array[1] = new FXMemoryPool.TestData
			{
				Input = 31,
				ExpectedResult = 32
			};
			array[2] = new FXMemoryPool.TestData
			{
				Input = 127,
				ExpectedResult = 64
			};
			array[3] = new FXMemoryPool.TestData
			{
				Input = 128,
				ExpectedResult = 192
			};
			array[0].Result = fxmemoryPool.TakeSlots(array[0].Input);
			array[1].Result = fxmemoryPool.TakeSlots(array[1].Input);
			array[2].Result = fxmemoryPool.TakeSlots(array[2].Input);
			array[3].Result = fxmemoryPool.TakeSlots(array[3].Input);
			fxmemoryPool.ReleaseSlots(array[0].Result, array[0].Input);
			fxmemoryPool.ReleaseSlots(array[1].Result, array[1].Input);
			fxmemoryPool.ReleaseSlots(array[2].Result, array[2].Input);
			fxmemoryPool.ReleaseSlots(array[3].Result, array[3].Input);
			for (int j = 0; j < 4; j++)
			{
				Debug.Assert(array[j].Result == array[j].ExpectedResult, string.Format("Error in test data {0}.", j));
			}
			array[0] = new FXMemoryPool.TestData
			{
				Input = 127,
				ExpectedResult = 0
			};
			array[1] = new FXMemoryPool.TestData
			{
				Input = 128,
				ExpectedResult = 128
			};
			array[2] = new FXMemoryPool.TestData
			{
				Input = 1,
				ExpectedResult = 0
			};
			array[3] = new FXMemoryPool.TestData
			{
				Input = 31,
				ExpectedResult = 32
			};
			array[0].Result = fxmemoryPool.TakeSlots(array[0].Input);
			array[1].Result = fxmemoryPool.TakeSlots(array[1].Input);
			fxmemoryPool.ReleaseSlots(array[0].Result, array[0].Input);
			array[2].Result = fxmemoryPool.TakeSlots(array[2].Input);
			array[3].Result = fxmemoryPool.TakeSlots(array[3].Input);
			fxmemoryPool.ReleaseSlots(array[1].Result, array[1].Input);
			fxmemoryPool.ReleaseSlots(array[2].Result, array[2].Input);
			fxmemoryPool.ReleaseSlots(array[3].Result, array[3].Input);
			for (int k = 0; k < 4; k++)
			{
				Debug.Assert(array[k].Result == array[k].ExpectedResult, string.Format("Error in test data {0}.", k));
			}
			array[0] = new FXMemoryPool.TestData
			{
				Input = 127,
				ExpectedResult = 0
			};
			array[1] = new FXMemoryPool.TestData
			{
				Input = 128,
				ExpectedResult = 128
			};
			array[2] = new FXMemoryPool.TestData
			{
				Input = 129,
				ExpectedResult = 256
			};
			array[3] = new FXMemoryPool.TestData
			{
				Input = 31,
				ExpectedResult = 0
			};
			array[0].Result = fxmemoryPool.TakeSlots(array[0].Input);
			array[1].Result = fxmemoryPool.TakeSlots(array[1].Input);
			fxmemoryPool.ReleaseSlots(array[0].Result, array[0].Input);
			array[2].Result = fxmemoryPool.TakeSlots(array[2].Input);
			array[3].Result = fxmemoryPool.TakeSlots(array[3].Input);
			fxmemoryPool.ReleaseSlots(array[1].Result, array[1].Input);
			fxmemoryPool.ReleaseSlots(array[2].Result, array[2].Input);
			fxmemoryPool.ReleaseSlots(array[3].Result, array[3].Input);
			for (int l = 0; l < 4; l++)
			{
				Debug.Assert(array[l].Result == array[l].ExpectedResult, string.Format("Error in test data {0}.", l));
			}
			array[0] = new FXMemoryPool.TestData
			{
				Input = 512,
				ExpectedResult = 0
			};
			array[1] = new FXMemoryPool.TestData
			{
				Input = 510,
				ExpectedResult = 512
			};
			array[2] = new FXMemoryPool.TestData
			{
				Input = 1020,
				ExpectedResult = 1024
			};
			array[3] = new FXMemoryPool.TestData
			{
				Input = 31,
				ExpectedResult = 0
			};
			array[0].Result = fxmemoryPool.TakeSlots(array[0].Input);
			array[1].Result = fxmemoryPool.TakeSlots(array[1].Input);
			fxmemoryPool.ReleaseSlots(array[0].Result, array[0].Input);
			array[2].Result = fxmemoryPool.TakeSlots(array[2].Input);
			array[3].Result = fxmemoryPool.TakeSlots(array[3].Input);
			fxmemoryPool.ReleaseSlots(array[1].Result, array[1].Input);
			fxmemoryPool.ReleaseSlots(array[2].Result, array[2].Input);
			fxmemoryPool.ReleaseSlots(array[3].Result, array[3].Input);
			for (int m = 0; m < 4; m++)
			{
				Debug.Assert(array[m].Result == array[m].ExpectedResult, string.Format("Error in test data {0}.", m));
			}
			array[0] = new FXMemoryPool.TestData
			{
				Input = 1024,
				ExpectedResult = 0
			};
			array[1] = new FXMemoryPool.TestData
			{
				Input = 1000,
				ExpectedResult = 1024
			};
			array[2] = new FXMemoryPool.TestData
			{
				Input = 1020,
				ExpectedResult = 2048
			};
			array[3] = new FXMemoryPool.TestData
			{
				Input = 31,
				ExpectedResult = 1024
			};
			array[0].Result = fxmemoryPool.TakeSlots(array[0].Input);
			array[1].Result = fxmemoryPool.TakeSlots(array[1].Input);
			array[2].Result = fxmemoryPool.TakeSlots(array[2].Input);
			fxmemoryPool.ReleaseSlots(array[1].Result, array[1].Input);
			array[3].Result = fxmemoryPool.TakeSlots(array[3].Input);
			fxmemoryPool.ReleaseSlots(array[0].Result, array[0].Input);
			fxmemoryPool.ReleaseSlots(array[2].Result, array[2].Input);
			fxmemoryPool.ReleaseSlots(array[3].Result, array[3].Input);
			for (int n = 0; n < 4; n++)
			{
				Debug.Assert(array[n].Result == array[n].ExpectedResult, string.Format("Error in test data {0}.", n));
			}
			array[0] = new FXMemoryPool.TestData
			{
				Input = 1024,
				ExpectedResult = 0
			};
			array[1] = new FXMemoryPool.TestData
			{
				Input = 1025,
				ExpectedResult = 2048
			};
			array[2] = new FXMemoryPool.TestData
			{
				Input = 2040,
				ExpectedResult = 4096
			};
			array[3] = new FXMemoryPool.TestData
			{
				Input = 100,
				ExpectedResult = 1024
			};
			array[0].Result = fxmemoryPool.TakeSlots(array[0].Input);
			array[1].Result = fxmemoryPool.TakeSlots(array[1].Input);
			array[2].Result = fxmemoryPool.TakeSlots(array[2].Input);
			array[3].Result = fxmemoryPool.TakeSlots(array[3].Input);
			fxmemoryPool.ReleaseSlots(array[0].Result, array[0].Input);
			fxmemoryPool.ReleaseSlots(array[1].Result, array[1].Input);
			fxmemoryPool.ReleaseSlots(array[2].Result, array[2].Input);
			fxmemoryPool.ReleaseSlots(array[3].Result, array[3].Input);
			for (int num = 0; num < 4; num++)
			{
				Debug.Assert(array[num].Result == array[num].ExpectedResult, string.Format("Error in test data {0}.", num));
			}
			array[0] = new FXMemoryPool.TestData
			{
				Input = 1024,
				ExpectedResult = 0
			};
			array[1] = new FXMemoryPool.TestData
			{
				Input = 1025,
				ExpectedResult = 0
			};
			array[2] = new FXMemoryPool.TestData
			{
				Input = 2048,
				ExpectedResult = 0
			};
			array[3] = new FXMemoryPool.TestData
			{
				Input = 2049,
				ExpectedResult = 1
			};
			for (int num2 = 0; num2 < 4; num2++)
			{
				try
				{
					array[num2].Result = 0;
					int num3 = fxmemoryPool.TakeSlots(array[num2].Input);
				}
				catch (Exception)
				{
					array[num2].Result = 1;
				}
			}
			for (int num4 = 0; num4 < 4; num4++)
			{
				Debug.Assert(array[num4].Result == array[num4].ExpectedResult, string.Format("Error in test data {0}.", num4));
			}
			fxmemoryPool.Clear();
			int num5 = fxmemoryPool.SlotCount + 100;
			int slotCount = fxmemoryPool.SlotCount;
			int num6 = -1;
			for (int num7 = 0; num7 < num5; num7++)
			{
				int num8 = fxmemoryPool.TakeSlots(fxmemoryPool.SlotSize);
				bool flag = num8 < 0;
				if (flag)
				{
					num6 = num7;
					break;
				}
			}
			Debug.Assert(num6 == slotCount, string.Format("Error in test data - failure came at {0}, and was expected at {1}.", num6, slotCount));
			fxmemoryPool.Clear();
			fxmemoryPool.Release();
		}

		// Token: 0x04002A78 RID: 10872
		private const int RequestedTestsMaxCount = 10000;

		// Token: 0x02000E9F RID: 3743
		private struct TestData
		{
			// Token: 0x04004766 RID: 18278
			public int Input;

			// Token: 0x04004767 RID: 18279
			public int Result;

			// Token: 0x04004768 RID: 18280
			public int ExpectedResult;
		}

		// Token: 0x02000EA0 RID: 3744
		private struct TestDataPooled : IFXDataStorage
		{
			// Token: 0x060067E2 RID: 26594 RVA: 0x0021903C File Offset: 0x0021723C
			public void Initialize(int items)
			{
			}

			// Token: 0x060067E3 RID: 26595 RVA: 0x0021903F File Offset: 0x0021723F
			public void Release()
			{
			}
		}
	}
}
