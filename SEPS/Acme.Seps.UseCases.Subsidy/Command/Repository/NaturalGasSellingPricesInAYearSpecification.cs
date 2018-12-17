﻿using Acme.Domain.Base.Repository;
using Acme.Seps.UseCases.Subsidy.Command.Entity;
using System;
using System.Linq.Expressions;

namespace Acme.Seps.UseCases.Subsidy.Command.Repository
{
    public sealed class NaturalGasSellingPricesInAYearSpecification : BaseSpecification<NaturalGasSellingPrice>
    {
        private readonly int _year;

        public NaturalGasSellingPricesInAYearSpecification(int year)
        {
            _year = year;
        }

        public override Expression<Func<NaturalGasSellingPrice, bool>> ToExpression() =>
            nsp => nsp.Active.Since.Year.Equals(_year);
    }
}