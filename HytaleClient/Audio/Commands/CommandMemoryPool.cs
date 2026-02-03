using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using HytaleClient.Utils;
using NLog;

namespace HytaleClient.Audio.Commands
{
	// Token: 0x02000B88 RID: 2952
	internal class CommandMemoryPool
	{
		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x06005AD8 RID: 23256 RVA: 0x001C506E File Offset: 0x001C326E
		public CommandBuffers Commands
		{
			get
			{
				return this._commands;
			}
		}

		// Token: 0x06005AD9 RID: 23257 RVA: 0x001C5078 File Offset: 0x001C3278
		public void Initialize()
		{
			this._priorityCommandMaxCount = 64;
			this._priorityMemoryPoolHelper = new MemoryPoolHelper(this._priorityCommandMaxCount);
			this._commandMaxCount = 8192;
			this._memoryPoolHelper = new MemoryPoolHelper(this._commandMaxCount);
			this._commands = new CommandBuffers(this._priorityCommandMaxCount + this._commandMaxCount);
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x001C50D4 File Offset: 0x001C32D4
		public int TakeSlot()
		{
			int num = this._memoryPoolHelper.ThreadSafeTakeMemorySlot(1);
			bool flag = num < 0;
			if (flag)
			{
				CommandMemoryPool.Logger.Warn("Could not find a free slot for basic sound command!");
			}
			return num + this._priorityCommandMaxCount;
		}

		// Token: 0x06005ADB RID: 23259 RVA: 0x001C5113 File Offset: 0x001C3313
		public void ReleaseSlot(int slot)
		{
			this._commands.Data[slot] = default(CommandBuffers.CommandData);
			this._memoryPoolHelper.ThreadSafeReleaseMemorySlot(slot - this._priorityCommandMaxCount, 1);
		}

		// Token: 0x06005ADC RID: 23260 RVA: 0x001C5144 File Offset: 0x001C3344
		public int TakePrioritySlot()
		{
			int num = this._priorityMemoryPoolHelper.ThreadSafeTakeMemorySlot(1);
			bool flag = num < 0;
			if (flag)
			{
				CommandMemoryPool.Logger.Warn("Could not find a free slot for priority sound command!");
			}
			return num;
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x001C517C File Offset: 0x001C337C
		public void ReleasePrioritySlot(int slot)
		{
			this._commands.Data[slot] = default(CommandBuffers.CommandData);
			this._priorityMemoryPoolHelper.ThreadSafeReleaseMemorySlot(slot, 1);
		}

		// Token: 0x06005ADE RID: 23262 RVA: 0x001C51A4 File Offset: 0x001C33A4
		public static bool StressTest(bool continuous, out string resultLog)
		{
			ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();
			CommandMemoryPool testSystem = new CommandMemoryPool();
			testSystem.Initialize();
			int requestCancel = 0;
			int registeredCommands = 0;
			int processedCommands = 0;
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			string audioThreadLog = "";
			Thread thread = new Thread(delegate()
			{
				CommandMemoryPool.AudioDeviceThreadStart(testSystem, concurrentQueue, ref requestCancel, ref processedCommands, ref registeredCommands, cancellationTokenSource.Token, out audioThreadLog);
			})
			{
				Name = "ConsumerStressThread",
				IsBackground = true
			};
			thread.Start();
			Stopwatch stopwatch = Stopwatch.StartNew();
			float num = 1f;
			resultLog = ".CommandMemoryPool.StressTest().\n";
			string concurrentLog = "";
			ConcurrentQueue<string> concurrentQueue2 = new ConcurrentQueue<string>();
			bool flag = false;
			while (num > 0f && !flag)
			{
				Parallel.For(0, 8000, delegate(int i)
				{
					int num2 = testSystem.TakeSlot();
					bool flag5 = num2 < 64;
					if (!flag5)
					{
						bool flag6 = testSystem.Commands.Data[num2].Volume == 0f;
						bool flag7 = !flag6;
						if (flag7)
						{
							concurrentLog += string.Format(" Take error at: {0} - {1}\n", num2, testSystem.Commands.Data[num2].Volume);
						}
						testSystem.Commands.Data[num2].Volume = (float)num2;
						concurrentQueue.Enqueue(num2);
						Interlocked.Increment(ref registeredCommands);
					}
				});
				bool flag2 = !continuous;
				if (flag2)
				{
					num -= (float)stopwatch.Elapsed.TotalSeconds;
					stopwatch.Restart();
				}
				else
				{
					flag = (flag || concurrentLog != "" || audioThreadLog != "");
				}
			}
			Interlocked.Increment(ref requestCancel);
			cancellationTokenSource.CancelAfter(new TimeSpan(0, 0, 1));
			thread.Join();
			num -= (float)stopwatch.Elapsed.TotalSeconds;
			stopwatch.Restart();
			bool flag3 = registeredCommands == processedCommands;
			resultLog += concurrentLog;
			resultLog += audioThreadLog;
			resultLog += string.Format("  Processed {0}/{1} commands in {2} seconds", processedCommands, registeredCommands, 1f - num);
			bool flag4 = !flag3;
			if (flag4)
			{
				resultLog += "\n  Not all commands were proccessed!";
			}
			Debug.Assert(flag3, "  Not all commands were proccessed!");
			cancellationTokenSource.Dispose();
			return flag3;
		}

		// Token: 0x06005ADF RID: 23263 RVA: 0x001C53C4 File Offset: 0x001C35C4
		public static void AudioDeviceThreadStart(CommandMemoryPool testSystem, ConcurrentQueue<int> concurrentQueue, ref int requestCancel, ref int processedCommands, ref int registeredCommands, CancellationToken cancellationToken, out string errorLog)
		{
			errorLog = "    ...AudioThread log...\n";
			while (!cancellationToken.IsCancellationRequested && (requestCancel == 0 || (requestCancel == 1 && processedCommands != registeredCommands)))
			{
				for (;;)
				{
					int num;
					bool flag = concurrentQueue.TryDequeue(out num);
					if (!flag)
					{
						break;
					}
					bool flag2 = testSystem.Commands.Data[num].Volume == (float)num;
					bool flag3 = !flag2;
					if (flag3)
					{
						errorLog += string.Format("    Release error at:  {0} - {1}\n", num, testSystem.Commands.Data[num].Volume);
					}
					testSystem.Commands.Data[num].Volume = 0f;
					testSystem.ReleaseSlot(num);
					Interlocked.Increment(ref processedCommands);
				}
				Thread.Sleep(16);
			}
			bool isCancellationRequested = cancellationToken.IsCancellationRequested;
			if (isCancellationRequested)
			{
				errorLog += "    was forced canceled.\n";
			}
		}

		// Token: 0x06005AE0 RID: 23264 RVA: 0x001C54D0 File Offset: 0x001C36D0
		public static void UnitTest()
		{
			CommandMemoryPool.<>c__DisplayClass18_0 CS$<>8__locals1 = new CommandMemoryPool.<>c__DisplayClass18_0();
			CS$<>8__locals1.testSystem = new CommandMemoryPool();
			CS$<>8__locals1.testSystem.Initialize();
			int num = 127;
			CommandMemoryPool.TestData[] array = new CommandMemoryPool.TestData[num];
			for (int l = 0; l < num; l++)
			{
				array[l] = new CommandMemoryPool.TestData
				{
					ExpectedResult = l + 64
				};
				array[l].Result = CS$<>8__locals1.testSystem.TakeSlot();
			}
			for (int j = 0; j < num; j++)
			{
				Debug.Assert(array[j].Result == array[j].ExpectedResult, string.Format("Error in test data {0}.", j));
			}
			int num2 = num;
			int num3 = 128;
			int[] reservedSlots = new int[num3];
			ConcurrentQueue<int> concurrentQueue = new ConcurrentQueue<int>();
			Parallel.For(0, num3, delegate(int i)
			{
				concurrentQueue.Enqueue(CS$<>8__locals1.testSystem.TakeSlot() - 64);
			});
			int num4 = 0;
			for (;;)
			{
				int num5;
				bool flag = concurrentQueue.TryDequeue(out num5);
				if (!flag)
				{
					break;
				}
				reservedSlots[num4] = num5;
				num4++;
			}
			for (int k = 0; k < num3; k++)
			{
				Debug.Assert(reservedSlots[k] != 0, string.Format("Error in test data {0}", k));
			}
			Parallel.For(0, num3, delegate(int i)
			{
				CS$<>8__locals1.testSystem.ReleaseSlot(reservedSlots[i]);
			});
			int num6 = CS$<>8__locals1.testSystem.TakeSlot();
			Debug.Assert(num6 == num2, "Error in test data.");
			CS$<>8__locals1.testSystem.ReleaseSlot(num6);
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x001C56A0 File Offset: 0x001C38A0
		public static void CommandBufferUnitTest()
		{
			string str;
			Debug.Assert(CommandMemoryPool.Test(new CommandMemoryPool.CommandField[]
			{
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "Type"
				}
			}, out str), "Command field overlaps: " + str);
			Debug.Assert(CommandMemoryPool.Test(new CommandMemoryPool.CommandField[]
			{
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "Type"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 8,
					Name = "SoundObjectReference"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 12,
					Name = "WorldPosition"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 12,
					Name = "WorldOrientation"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "BoolData"
				}
			}, out str), "Command field overlaps: " + str);
			Debug.Assert(CommandMemoryPool.Test(new CommandMemoryPool.CommandField[]
			{
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "Type"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 8,
					Name = "SoundObjectReference"
				}
			}, out str), "Command field overlaps: " + str);
			Debug.Assert(CommandMemoryPool.Test(new CommandMemoryPool.CommandField[]
			{
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "Type"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 8,
					Name = "SoundObjectReference"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 12,
					Name = "WorldPosition"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 12,
					Name = "WorldOrientation"
				}
			}, out str), "Command field overlaps: " + str);
			Debug.Assert(CommandMemoryPool.Test(new CommandMemoryPool.CommandField[]
			{
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "Type"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 12,
					Gap = 8,
					Name = "WorldPosition"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 12,
					Name = "WorldOrientation"
				}
			}, out str), "Command field overlaps: " + str);
			Debug.Assert(CommandMemoryPool.Test(new CommandMemoryPool.CommandField[]
			{
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "Type"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 8,
					Name = "SoundObjectReference"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 4,
					Name = "EventId"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 4,
					Name = "PlaybackId"
				}
			}, out str), "Command field overlaps: " + str);
			Debug.Assert(CommandMemoryPool.Test(new CommandMemoryPool.CommandField[]
			{
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "Type"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 4,
					Name = "TransitionDuration"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "FadeCurveType"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "ActionType"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 4,
					Gap = 6,
					Name = "PlaybackId"
				}
			}, out str), "Command field overlaps: " + str);
			Debug.Assert(CommandMemoryPool.Test(new CommandMemoryPool.CommandField[]
			{
				new CommandMemoryPool.CommandField
				{
					Size = 1,
					Name = "Type"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 4,
					Name = "Volume"
				},
				new CommandMemoryPool.CommandField
				{
					Size = 4,
					Name = "RTPCId"
				}
			}, out str), "Command field overlaps: " + str);
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x001C5BC8 File Offset: 0x001C3DC8
		private static bool Test(CommandMemoryPool.CommandField[] test, out string errorMessage)
		{
			bool result = true;
			errorMessage = "";
			int num = 0;
			foreach (CommandMemoryPool.CommandField commandField in test)
			{
				int num2 = num + commandField.Gap;
				int num3 = Marshal.OffsetOf(typeof(CommandBuffers.CommandData), commandField.Name).ToInt32();
				bool flag = num2 != num3;
				if (flag)
				{
					result = false;
					errorMessage = commandField.Name;
					break;
				}
				num = num2 + commandField.Size;
			}
			return result;
		}

		// Token: 0x040038EA RID: 14570
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040038EB RID: 14571
		public const int RequestedPriorityCommandMaxCount = 64;

		// Token: 0x040038EC RID: 14572
		private const int RequestedCommandMaxCount = 8192;

		// Token: 0x040038ED RID: 14573
		private int _commandMaxCount;

		// Token: 0x040038EE RID: 14574
		private int _priorityCommandMaxCount;

		// Token: 0x040038EF RID: 14575
		private CommandBuffers _commands;

		// Token: 0x040038F0 RID: 14576
		private MemoryPoolHelper _memoryPoolHelper;

		// Token: 0x040038F1 RID: 14577
		private MemoryPoolHelper _priorityMemoryPoolHelper;

		// Token: 0x02000F70 RID: 3952
		private struct TestData
		{
			// Token: 0x04004B05 RID: 19205
			public int Result;

			// Token: 0x04004B06 RID: 19206
			public int ExpectedResult;
		}

		// Token: 0x02000F71 RID: 3953
		private struct CommandField
		{
			// Token: 0x04004B07 RID: 19207
			public int Gap;

			// Token: 0x04004B08 RID: 19208
			public int Size;

			// Token: 0x04004B09 RID: 19209
			public string Name;
		}
	}
}
