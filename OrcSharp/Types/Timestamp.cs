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

namespace OrcSharp.Types
{
    using System;
    using OrcSharp.External;

    public struct Timestamp : IEquatable<Timestamp>, IComparable<Timestamp>, IComparable
    {
        private readonly long milliseconds;

        public Timestamp(long milliseconds)
        {
            this.milliseconds = milliseconds;
        }

        public Timestamp(DateTime date)
        {
            milliseconds = Epoch.getTimestamp(date);
        }

        public Timestamp(int year1900, int month, int day, int hour, int minute, int second, int nanos)
        {
            // TODO:
            DateTime datetime = new DateTime(1900 + year1900, month, day, hour, minute, second);
            milliseconds = Epoch.getTimestamp(datetime);
        }

        public DateTime AsDateTime
        {
            get { return Epoch.getTimestamp(milliseconds); }
        }

        public long Milliseconds
        {
            get { return milliseconds; }
        }

        public long getSeconds()
        {
            return milliseconds / 1000;
        }

        public int getNanos()
        {
            // TODO:
            return AsDateTime.getNanos();
        }

        public static Timestamp Parse(string timestamp)
        {
            return new Timestamp(DateTime.Parse(timestamp));
        }

        public override string ToString()
        {
            return Epoch.getTimestamp(milliseconds).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public override int GetHashCode()
        {
            return milliseconds.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Timestamp)
            {
                return Equals((Timestamp)obj);
            }
            return false;
        }

        public bool Equals(Timestamp other)
        {
            return milliseconds == other.milliseconds;
        }

        public int CompareTo(Timestamp other)
        {
            return milliseconds.CompareTo(other.milliseconds);
        }

        public int CompareTo(object obj)
        {
            return CompareTo((Timestamp)obj);
        }

        public static bool operator <(Timestamp left, Timestamp right)
        {
            return left.milliseconds < right.milliseconds;
        }

        public static bool operator >(Timestamp left, Timestamp right)
        {
            return left.milliseconds > right.milliseconds;
        }
    }
}
