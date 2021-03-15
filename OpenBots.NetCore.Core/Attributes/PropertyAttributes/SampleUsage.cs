﻿using System;

namespace OpenBots.NetCore.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SampleUsage : Attribute
    {
        public string Usage { get; private set; }
        public SampleUsage(string sample)
        {
            Usage = sample;
        }
    }
}
