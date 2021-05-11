// ReSharper disable InconsistentNaming

namespace Microsoft.Azure.Databricks.Client
{
    public class RuntimeVersions
    {
        #region 5.5
        /// <summary>
        /// 5.5 LTS (includes Apache Spark 2.4.3, Scala 2.11)
        /// </summary>
        public const string Runtime_5_5 = "5.5.x-scala2.11";

        /// <summary>
        /// 5.5 LTS (includes Apache Spark 2.4.3, GPU, Scala 2.11)
        /// </summary>
        public const string Runtime_5_5_GPU = "5.5.x-gpu-scala2.11";

        /// <summary>
        /// 5.5 LTS ML (includes Apache Spark 2.4.3, GPU, Scala 2.11)
        /// </summary>
        public const string Runtime_5_5_GPU_ML = "5.5.x-gpu-ml-scala2.11";

        /// <summary>
        /// 5.5 LTS ML (includes Apache Spark 2.4.3, Scala 2.11)
        /// </summary>
        public const string Runtime_5_5_CPU_ML = "5.5.x-cpu-ml-scala2.11";
        #endregion

        #region 6.4
        /// <summary>
        /// 6.4 Extended Support (includes Apache Spark 2.4.5, Scala 2.11)
        /// </summary>
        public const string Runtime_6_4_ESR = "6.4.x-esr-scala2.11";
        #endregion

        #region 7.3
        /// <summary>
        /// 7.3 LTS (includes Apache Spark 3.0.1, Scala 2.12)
        /// </summary>
        public const string Runtime_7_3 = "7.3.x-scala2.12";

        /// <summary>
        /// 7.3 LTS ML (includes Apache Spark 3.0.1, Scala 2.12)
        /// </summary>
        public const string Runtime_7_3_CPU_ML = "7.3.x-cpu-ml-scala2.12";

        /// <summary>
        /// 7.3 LTS ML (includes Apache Spark 3.0.1, GPU, Scala 2.12)
        /// </summary>
        public const string Runtime_7_3_GPU_ML = "7.3.x-gpu-ml-scala2.12";

        /// <summary>
        /// 7.3 LTS Genomics (includes Apache Spark 3.0.1, Scala 2.12)
        /// </summary>
        public const string Runtime_7_3_HLS = "7.3.x-hls-scala2.12";
        #endregion

        #region 7.5
        /// <summary>
        /// 7.5 (includes Apache Spark 3.0.1, Scala 2.12)
        /// </summary>
        public const string Runtime_7_5 = "7.5.x-scala2.12";

        /// <summary>
        /// 7.5 ML (includes Apache Spark 3.0.1, Scala 2.12)
        /// </summary>
        public const string Runtime_7_5_CPU_ML = "7.5.x-cpu-ml-scala2.12";

        /// <summary>
        /// 7.5 ML (includes Apache Spark 3.0.1, GPU, Scala 2.12)
        /// </summary>
        public const string Runtime_7_5_GPU_ML = "7.5.x-gpu-ml-scala2.12";

        /// <summary>
        /// 7.5 Genomics (includes Apache Spark 3.0.1, Scala 2.12)
        /// </summary>
        public const string Runtime_7_5_HLS = "7.5.x-hls-scala2.12";
        #endregion

        #region 7.6
        /// <summary>
        /// 7.6 (includes Apache Spark 3.0.1, Scala 2.12)
        /// </summary>
        public const string Runtime_7_6 = "7.6.x-scala2.12";

        /// <summary>
        /// 7.6 ML (includes Apache Spark 3.0.1, Scala 2.12)
        /// </summary>
        public const string Runtime_7_6_CPU_ML = "7.6.x-cpu-ml-scala2.12";

        /// <summary>
        /// 7.6 ML (includes Apache Spark 3.0.1, GPU, Scala 2.12)
        /// </summary>
        public const string Runtime_7_6_GPU_ML = "7.6.x-gpu-ml-scala2.12";
        #endregion

        #region 8.0
        /// <summary>
        /// 8.0 (includes Apache Spark 3.1.1, Scala 2.12)
        /// </summary>
        public const string Runtime_8_0 = "8.0.x-scala2.12";

        /// <summary>
        /// 8.0 ML (includes Apache Spark 3.1.1, Scala 2.12)
        /// </summary>
        public const string Runtime_8_0_CPU_ML = "8.0.x-cpu-ml-scala2.12";
        #endregion

        #region 8.1
        /// <summary>
        /// 8.1 (includes Apache Spark 3.1.1, Scala 2.12)
        /// </summary>
        public const string Runtime_8_1 = "8.1.x-scala2.12";

        /// <summary>
        /// 8.1 ML (includes Apache Spark 3.1.1, Scala 2.12)
        /// </summary>
        public const string Runtime_8_1_CPU_ML = "8.1.x-cpu-ml-scala2.12";

        /// <summary>
        /// 8.1 ML (includes Apache Spark 3.1.1, GPU, Scala 2.12)
        /// </summary>
        public const string Runtime_8_1_GPU_ML = "8.1.x-gpu-ml-scala2.12";
        #endregion

        #region 8.2
        /// <summary>
        /// 8.2 (includes Apache Spark 3.1.1, Scala 2.12)
        /// </summary>
        public const string Runtime_8_2 = "8.2.x-scala2.12";

        /// <summary>
        /// 8.2 ML (includes Apache Spark 3.1.1, Scala 2.12)
        /// </summary>
        public const string Runtime_8_2_CPU_ML = "8.2.x-cpu-ml-scala2.12";

        /// <summary>
        /// 8.2 ML (includes Apache Spark 3.1.1, GPU, Scala 2.12)
        /// </summary>
        public const string Runtime_8_2_GPU_ML = "8.2.x-gpu-ml-scala2.12";
        #endregion

        #region light 2.4
        /// <summary>
        /// Light 2.4 (includes Apache Spark 2.4, Scala 2.11)
        /// </summary>
        public const string Runtime_Light_2_4 = "apache-spark-2.4.x-scala2.11";
        #endregion
    }
}