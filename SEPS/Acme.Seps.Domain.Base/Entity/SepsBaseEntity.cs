﻿using Acme.Domain.Base.DomainService;
using Acme.Domain.Base.Entity;
using Acme.Domain.Base.ValueType;
using System;

namespace Acme.Seps.Domain.Base.Entity
{
    public abstract class SepsBaseEntity : BaseEntity
    {
        private readonly ITimeZone _timeZone;

        public Period Period { get; private set; }

        protected SepsBaseEntity(ITimeZone timeZone)
        {
            if (timeZone == null)
                throw new ArgumentNullException(nameof(timeZone));

            _timeZone = timeZone;
            Period = new Period(_timeZone.GetCurrentRepositoryDateTime());
        }

        public void Archive()
        {
            if (IsActive())
                Period = Period.SetValidTill(_timeZone.GetCurrentRepositoryDateTime());
        }

        public void Delete() => Period = new Period(Period.ValidFrom, Period.ValidFrom);

        public bool IsActive() => Period.IsWithin(_timeZone.GetCurrentRepositoryDateTime());

        public bool IsDeleted() => Period.ValidTill.HasValue && Period.ValidFrom.Equals(Period.ValidTill);
    }
}