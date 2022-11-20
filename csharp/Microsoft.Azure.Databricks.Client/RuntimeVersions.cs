// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// ReSharper disable InconsistentNaming

namespace Microsoft.Azure.Databricks.Client;

public class RuntimeVersions
{
    #region 7.3 LTS
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

    #region 9.1 LTS

    /// <summary>
    /// 9.1 LTS (includes Apache Spark 3.1.2, Scala 2.12)
    /// </summary>
    public const string Runtime_9_1 = "9.1.x-scala2.12";

    /// <summary>
    /// 9.1 LTS ML (includes Apache Spark 3.1.2, Scala 2.12)
    /// </summary>
    public const string Runtime_9_1_CPU_ML = "9.1.x-cpu-ml-scala2.12";

    /// <summary>
    /// 9.1 LTS ML (includes Apache Spark 3.1.2, GPU, Scala 2.12)
    /// </summary>
    public const string Runtime_9_1_GPU_ML = "9.1.x-gpu-ml-scala2.12";

    /// <summary>
    /// 9.1 LTS Photon (includes Apache Spark 3.1.2, Scala 2.12)
    /// </summary>
    public const string Runtime_9_1_PHOTON = "9.1.x-photon-scala2.12";

    #endregion

    #region 10.4 LTS

    /// <summary>
    /// 10.4 LTS (includes Apache Spark 3.2.1, Scala 2.12)
    /// </summary>
    public const string Runtime_10_4 = "10.4.x-scala2.12";

    /// <summary>
    /// 10.4 LTS ML (includes Apache Spark 3.2.1, Scala 2.12)
    /// </summary>
    public const string Runtime_10_4_CPU_ML = "10.4.x-cpu-ml-scala2.12";

    /// <summary>
    /// 10.4 LTS ML (includes Apache Spark 3.2.1, GPU, Scala 2.12)
    /// </summary>
    public const string Runtime_10_4_GPU_ML = "10.4.x-gpu-ml-scala2.12";

    /// <summary>
    /// 10.4 LTS Photon (includes Apache Spark 3.2.1, Scala 2.12)
    /// </summary>
    public const string Runtime_10_4_PHOTON = "10.4.x-photon-scala2.12";

    #endregion

    #region 11.0

    /// <summary>
    /// 11.0 (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_0 = "11.0.x-scala2.12";

    /// <summary>
    /// 11.0 ML (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_0_CPU_ML = "11.0.x-cpu-ml-scala2.12";

    /// <summary>
    /// 11.0 ML (includes Apache Spark 3.3.0, GPU, Scala 2.12)
    /// </summary>
    public const string Runtime_11_0_GPU_ML = "11.0.x-gpu-ml-scala2.12";

    /// <summary>
    /// 11.0 Photon (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_0_PHOTON = "11.0.x-photon-scala2.12";

    #endregion

    #region 11.1

    /// <summary>
    /// 11.1 (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_1 = "11.1.x-scala2.12";

    /// <summary>
    /// 11.1 ML (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_1_CPU_ML = "11.1.x-cpu-ml-scala2.12";

    /// <summary>
    /// 11.1 ML (includes Apache Spark 3.3.0, GPU, Scala 2.12)
    /// </summary>
    public const string Runtime_11_1_GPU_ML = "11.1.x-gpu-ml-scala2.12";

    /// <summary>
    /// 11.1 Photon (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_1_PHOTON = "11.1.x-photon-scala2.12";

    #endregion

    #region 11.2

    /// <summary>
    /// 11.2 (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_2 = "11.2.x-scala2.12";

    /// <summary>
    /// 11.2 ML (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_2_CPU_ML = "11.2.x-cpu-ml-scala2.12";

    /// <summary>
    /// 11.2 ML (includes Apache Spark 3.3.0, GPU, Scala 2.12)
    /// </summary>
    public const string Runtime_11_2_GPU_ML = "11.2.x-gpu-ml-scala2.12";

    /// <summary>
    /// 11.2 Photon (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_2_PHOTON = "11.2.x-photon-scala2.12";

    #endregion

    #region 11.3 LTS

    /// <summary>
    /// 11.3 LTS (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_3 = "11.3.x-scala2.12";

    /// <summary>
    /// 11.3 LTS ML (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_3_CPU_ML = "11.3.x-cpu-ml-scala2.12";

    /// <summary>
    /// 11.3 LTS ML (includes Apache Spark 3.3.0, GPU, Scala 2.12)
    /// </summary>
    public const string Runtime_11_3_GPU_ML = "11.3.x-gpu-ml-scala2.12";

    /// <summary>
    /// 11.3 LTS Photon (includes Apache Spark 3.3.0, Scala 2.12)
    /// </summary>
    public const string Runtime_11_3_PHOTON = "11.3.x-photon-scala2.12";

    #endregion

    #region Light 2.4

    /// <summary>
    /// Light 2.4 Extended Support (includes Apache Spark 2.4, Scala 2.11)
    /// </summary>
    public const string Runtime_Light_2_4_ESR = "apache-spark-2.4.x-esr-scala2.11";

    #endregion
}