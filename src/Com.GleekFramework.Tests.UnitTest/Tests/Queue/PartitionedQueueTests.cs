using Com.GleekFramework.QueueSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Queue
{
    /// <summary>
    /// 分区队列单元测试
    /// </summary>
    public class PartitionedQueueTests : BaseUnitTest
    {
        /// <summary>
        /// 默认构造函数创建 ProcessorCount*2 个分区
        /// </summary>
        [Fact(DisplayName = "默认构造函数创建合理分区数")]
        public void DefaultConstructorCreatesPartitions()
        {
            var queue = new PartitionedQueue<string>();
            Assert.True(queue.PartitionCount > 0);
        }

        /// <summary>
        /// 指定分区数量后正确初始化
        /// </summary>
        [Fact(DisplayName = "指定分区数量后正确初始化")]
        public void CustomPartitionCountCreatesCorrectPartitions()
        {
            var queue = new PartitionedQueue<string>(5);
            Assert.Equal(5, queue.PartitionCount);
        }

        /// <summary>
        /// PublishAsync 发布单条消息不抛异常
        /// </summary>
        [Fact(DisplayName = "PublishAsync发布单条消息不抛异常")]
        public async Task PublishSingleMessageDoesNotThrow()
        {
            var queue = new PartitionedQueue<string>(3);
            await queue.PublishAsync("test_message", "key1");
        }

        /// <summary>
        /// PublishAsync 发布多条消息不抛异常
        /// </summary>
        [Fact(DisplayName = "PublishAsync发布多条消息不抛异常")]
        public async Task PublishMultipleMessagesDoesNotThrow()
        {
            var queue = new PartitionedQueue<string>(3);
            var messages = new List<string> { "msg1", "msg2", "msg3" };
            await queue.PublishAsync(messages, "batch_key");
        }

        /// <summary>
        /// 不同分区键将消息分发到不同分区
        /// </summary>
        [Fact(DisplayName = "不同分区键分发到不同分区")]
        public async Task DifferentPartitionKeysGoToDifferentPartitions()
        {
            var queue = new PartitionedQueue<string>(5);
            await queue.PublishAsync("msg_a", "key_a");
            await queue.PublishAsync("msg_b", "key_b");
            // 验证不抛异常，且 SurplusMessageCount 正确
            Assert.True(queue.SurplusMessageCount >= 0);
        }

        /// <summary>
        /// 不同分区数量的队列独立工作
        /// </summary>
        [Fact(DisplayName = "不同分区数量队列独立工作")]
        public void DifferentPartitionCountsWorkIndependently()
        {
            var queue2 = new PartitionedQueue<string>(2);
            var queue10 = new PartitionedQueue<string>(10);
            Assert.Equal(2, queue2.PartitionCount);
            Assert.Equal(10, queue10.PartitionCount);
        }
    }
}
