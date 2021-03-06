﻿/**
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

namespace OrcSharp.External
{
    using Google.ProtocolBuffers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class Arrays
    {
        public static void fill<T>(T[] array, int start, int end, T value)
        {
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

        public static void fill<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static T[] copyOf<T>(T[] array, int newLength)
        {
            T[] result = new T[newLength];
            Array.Copy(array, result, Math.Min(array.Length, newLength));
            return result;
        }
    }

    public static class Dictionaries
    {
        public static V get<K, V>(this IDictionary<K, V> dictionary, K key) where V : class
        {
            V result = default(V);
            dictionary.TryGetValue(key, out result);
            return result;
        }
    }

    public static class Lists
    {
        public static List<T> subList<T>(this List<T> list, int start, int end)
        {
            List<T> result = new List<T>(end - start);
            for (int i = start; i < end; i++)
            {
                result.Add(list[i]);
            }
            return result;
        }

        public static T[] subList<T>(this IList<T> list, int start, int end)
        {
            T[] result = new T[end - start];
            for (int i = start; i < end; i++)
            {
                result[i - start] = list[i];
            }
            return result;
        }

        public static bool AreEqual<T>(IList<T> list1, IList<T> list2) where T : IEquatable<T>
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AreEqual<T>(IList<T> list1, int offset1, IList<T> list2, int length) where T : IEquatable<T>
        {
            if (list1.Count < offset1 + length || list2.Count < length)
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                if (!list1[i + offset1].Equals(list2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static IList<int> flip(this IList<uint> array)
        {
            int[] result = new int[array.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (int)array[i];
            }
            return result;
        }

        public static IList<uint> flip(this IList<int> array)
        {
            uint[] result = new uint[array.Count];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (uint)array[i];
            }
            return result;
        }

        public static List<T> newArrayList<T>(params T[] v)
        {
            return v.ToList();
        }
    }

    public static class Float
    {
        public static float intBitsToFloat(int value)
        {
            // TODO: use unsafe code
            return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }

        public static int floatToIntBits(float value)
        {
            // TODO: use unsafe code
            return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        }
    }

    public static class Epoch
    {
        public readonly static DateTime Start = new DateTime(1970, 1, 1);

        public static long getTimestamp(this DateTime dateTime)
        {
            return (long)Math.Floor((dateTime - Epoch.Start).TotalMilliseconds);
        }

        public static int getDays(this DateTime dateTime)
        {
            return (int)Math.Floor((dateTime - Epoch.Start).TotalDays);
        }

        public static DateTime getDate(int days)
        {
            return Start.AddDays(days);
        }

        public static DateTime getTimestamp(long millis)
        {
            return Start.AddTicks(TimeSpan.TicksPerMillisecond * millis);
        }
    }

    public static class Integer
    {
        public static int numberOfLeadingZeros(int x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (sizeof(int) * 8 - NumberOfOnes(x));
        }

        public static int NumberOfOnes(int x)
        {
            x -= ((x >> 1) & 0x55555555);
            x = (((x >> 2) & 0x33333333) + (x & 0x33333333));
            x = (((x >> 4) + x) & 0x0f0f0f0f);
            x += (x >> 8);
            x += (x >> 16);
            return (x & 0x0000003f);
        }

        internal static string toHexString(int v)
        {
            return v.ToString("x");
        }

        internal static string toBinaryString(int v)
        {
            // TODO:
            return v.ToString("x");
        }
    }

    public static class Long
    {
        public static int NumberOfOnes(long i)
        {
            i = i - ((i >> 1) & 0x5555555555555555);
            i = (i & 0x3333333333333333) + ((i >> 2) & 0x3333333333333333);
            return (int)((((i + (i >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56);
        }

        internal static string toHexString(long v)
        {
            return v.ToString("x");
        }
    }

    public static class Streams
    {
        public static byte[] readFully(this Stream stream, long streamPosition, byte[] buffer, int position, int length)
        {
            long originalPosition = stream.Position;
            stream.Position = streamPosition;
            stream.readFully(buffer, position, length);
            stream.Position = originalPosition;
            return buffer;
        }

        public static void readFully(this Stream stream, byte[] buffer, int position, int length)
        {
            while (length > 0)
            {
                int n = stream.Read(buffer, position, length);
                if (n <= 0)
                {
                    throw new InvalidOperationException();
                }
                length -= n;
                position += n;
            }
        }
    }

    public static class ByteBuffers
    {
        public static ByteBuffer asReadOnlyByteBuffer(this ByteString buffer)
        {
            return new ByteBuffer(buffer.ToByteArray());
        }
    }
}
