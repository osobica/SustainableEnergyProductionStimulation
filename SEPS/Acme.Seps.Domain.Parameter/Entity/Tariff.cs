﻿using Acme.Domain.Base.Entity;
using Acme.Domain.Base.Factory;
using Acme.Seps.Domain.Base.Entity;
using Acme.Seps.Domain.Base.Factory;
using Light.GuardClauses;
using System;
using Message = Acme.Seps.Domain.Parameter.Infrastructure.Parameter;

namespace Acme.Seps.Domain.Parameter.Entity
{
    public abstract class Tariff : SepsBaseAggregate
    {
        public int LowerProductionLimit { get; private set; }
        public int UpperProductionLimit { get; private set; }
        public decimal LowerRate { get; private set; }
        public decimal HigherRate { get; private set; }

        protected Tariff() { }

        protected Tariff(
            decimal lowerRate,
            decimal higherRate,
            IPeriodFactory periodFactory,
            IIdentityFactory<Guid> identityFactory) : base(periodFactory, identityFactory)
        {
            lowerRate.MustBeGreaterThanOrEqualTo(0m, (_, __) =>
                new DomainException(Message.BelowZeroLowerRateException));
            higherRate.MustBeGreaterThanOrEqualTo(0m, (_, __) =>
                new DomainException(Message.BelowZeroUpperRateException));
            lowerRate.MustBeLessThanOrEqualTo(higherRate, (_, __) =>
                new DomainException(Message.LowerRateAboveUpperException));

            LowerRate = lowerRate;
            HigherRate = higherRate;
        }
    }
}