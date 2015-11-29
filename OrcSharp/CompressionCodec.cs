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

namespace OrcSharp
{
    using OrcSharp.External;

    public interface CompressionCodec
    {
        /**
         * Compress the in buffer to the out buffer.
         * @param in the bytes to compress
         * @param out the uncompressed bytes
         * @param overflow put any additional bytes here
         * @return true if the output is smaller than input
         * @
         */
        bool compress(ByteBuffer @in, ByteBuffer @out, ByteBuffer overflow);

        /**
         * Decompress the in buffer to the out buffer.
         * @param in the bytes to decompress
         * @param out the decompressed bytes
         * @
         */
        void decompress(ByteBuffer @in, ByteBuffer @out);

        /**
         * Produce a modified compression codec if the underlying algorithm allows
         * modification.
         *
         * This does not modify the current object, but returns a new object if
         * modifications are possible. Returns the same object if no modifications
         * are possible.
         * @param modifiers compression modifiers
         * @return codec for use after optional modification
         */
        CompressionCodec modify(CompressionModifier[] modifiers);
    }

    public enum CompressionModifier
    {
        /* speed/compression tradeoffs */
        FASTEST,
        FAST,
        DEFAULT,
        /* data sensitivity modifiers */
        TEXT,
        BINARY
    }
}
