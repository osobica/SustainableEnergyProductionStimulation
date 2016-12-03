﻿using Acme.Domain.Base.Factory;
using Acme.Seps.Domain.Base.ValueType;
using Acme.Seps.Domain.Parameter.DomainService;
using Acme.Seps.Domain.Parameter.Entity;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Acme.Seps.Domain.Parameter.Test.Unit.Entity
{
    public class CogenerationParameterServiceTests
    {
        private readonly ICogenerationParameterService _cogenerationParameterService;

        private readonly NaturalGasSellingPrice _naturalGasSellingPrice;
        private readonly IEnumerable<NaturalGasSellingPrice> _yearsNaturalGasSellingPrices;
        private readonly Mock<IIdentityFactory<Guid>> _identityFactory;

        public CogenerationParameterServiceTests()
        {
            _identityFactory = new Mock<IIdentityFactory<Guid>>();
            _naturalGasSellingPrice = Activator.CreateInstance(
                typeof(NaturalGasSellingPrice),
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new object[] {
                    10M,
                    nameof(NaturalGasSellingPrice),
                    new MonthlyPeriod(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(-2)),
                    _identityFactory.Object },
                null) as NaturalGasSellingPrice;
            _yearsNaturalGasSellingPrices = new List<NaturalGasSellingPrice> { _naturalGasSellingPrice };

            _cogenerationParameterService = new CogenerationParameterService();
        }

        public void RatesAreCorrectlyCalculated()
        {
            var result = _cogenerationParameterService.GetFrom(_yearsNaturalGasSellingPrices, _naturalGasSellingPrice);

            result.Should().Be(10M);
        }
    }
}