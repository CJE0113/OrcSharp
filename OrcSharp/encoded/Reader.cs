/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace org.apache.hadoop.hive.ql.io.orc.encoded
{
    using org.apache.hadoop.hive.ql.io.orc.external;
    using OrcProto = global::orc.proto;
    using ColumnStreamData = EncodedColumnBatch<OrcBatchKey>.ColumnStreamData;

    /**
     * The interface for reading encoded data from ORC files.
     */
    public abstract class Reader : org.apache.hadoop.hive.ql.io.orc.Reader
    {

        /** The factory that can create (or return) the pools used by encoded reader. */
        public interface PoolFactory
        {
            Pool<T> createPool<T>(int size, PoolObjectHelper<T> helper);
            Pool<OrcEncodedColumnBatch> createEncodedColumnBatchPool();
            Pool<ColumnStreamData> createColumnStreamDataPool();
        }

        /** Implementation of EncodedColumnBatch for ORC. */
        public class OrcEncodedColumnBatch : EncodedColumnBatch<OrcBatchKey>
        {
            /** RG index indicating the data applies for all RGs (e.g. a string dictionary). */
            public static int ALL_RGS = -1;
            /**
             * All the previous streams are data streams, this and the next ones are index streams.
             * We assume the sort will stay the same for backward compat.
             */
            public static int MAX_DATA_STREAMS = (int)OrcProto.Stream.Types.Kind.ROW_INDEX;
            public void init(long fileId, int stripeIx, int rgIx, int columnCount)
            {
                if (batchKey == null)
                {
                    batchKey = new OrcBatchKey(fileId, stripeIx, rgIx);
                }
                else
                {
                    batchKey.set(fileId, stripeIx, rgIx);
                }
                resetColumnArrays(columnCount);
            }
        }

        /**
         * Creates the encoded reader.
         * @param fileId File ID to read, to use for cache lookups and such.
         * @param dataCache Data cache to use for cache lookups.
         * @param dataReader Data reader to read data not found in cache (from disk, HDFS, and such).
         * @param pf Pool factory to create object pools.
         * @return The reader.
         */
        public abstract EncodedReader encodedReader(
            long fileId, DataCache dataCache, DataReader dataReader, PoolFactory pf);
    }
}
