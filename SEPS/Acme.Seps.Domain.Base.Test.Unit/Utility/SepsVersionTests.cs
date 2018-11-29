﻿using Acme.Seps.Domain.Base.Utility;
using FluentAssertions;
using System;

namespace Acme.Seps.Domain.Base.Test.Unit.Utility
{
    public class SepsVersionTests
    {
        public void InitialPeriodIsCorrectlySet()
        {
            SepsVersion.InitialPeriod().Should().Be(new DateTimeOffset(new DateTime(2007, 7, 1)));
        }
    }
}