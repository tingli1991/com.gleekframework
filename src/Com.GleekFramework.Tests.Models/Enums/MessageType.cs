using Com.GleekFramework.CommonSdk;
using System;
using System.ComponentModel;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 功能定义
    /// </summary>
    [Serializable]
    public enum MessageType
    {
        /// <summary>
        /// 队列测试方法
        /// </summary>
        [Description("队列测试方法")]
        [Action("test_queue_rpc_name")]
        TEST_QUERY_RPC_NAME,

        /// <summary>
        /// 队列测试方法
        /// </summary>
        [Description("队列测试方法")]
        [Action("customer_queue_rpc_name")]
        CUSTOMER_QUERY_RPC_NAME,

        /// <summary>
        /// 自定义Kafka测试类
        /// </summary>
        [Description("自定义Kafka测试类")]
        [Action("customer_test_kafka_name")]
        CUSTOMER_TEST_KAFKA_NAME,

        /// <summary>
        /// 自定义Queue测试类
        /// </summary>
        [Description("自定义queue测试类")]
        [Action("customer_test_queue_name")]
        CUSTOMER_TEST_QUEUE_NAME,

        /// <summary>
        /// 自定义Stack测试类
        /// </summary>
        [Description("自定义stack测试类")]
        [Action("customer_test_stack_name")]
        CUSTOMER_TEST_STACK_NAME,
    }
}