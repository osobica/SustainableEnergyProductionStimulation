﻿using Acme.Domain.Base.Factory;
using Acme.Seps.Domain.Base.Infrastructure;
using Acme.Seps.Domain.Subsidy.Command.Entity;
using Acme.Seps.Domain.Subsidy.Command.Infrastructure;
using Acme.Seps.Domain.Subsidy.Test.Unit.Factory;
using FluentAssertions;
using NSubstitute;
using System;
using System.Reflection;

namespace Acme.Seps.Domain.Subsidy.Test.Unit.Entity
{
    public class RenewableEnergySourceTariffTests
    {
        private readonly IIdentityFactory<Guid> _identityFactory;
        private IEconometricIndexFactory<ConsumerPriceIndex> _cpiFactory;
        private ITariffFactory<RenewableEnergySourceTariff> _resFactory;

        public RenewableEnergySourceTariffTests()
        {
            _identityFactory = Substitute.For<IIdentityFactory<Guid>>();
            _cpiFactory = new EconometricIndexFactory<ConsumerPriceIndex>(DateTime.Now.AddYears(-3));
            _resFactory = new TariffFactory<RenewableEnergySourceTariff>(_cpiFactory.Create());
        }

        public void ConsumerPriceIndexMustBeSet()
        {
            Action action = () => _resFactory.Create().CreateNewWith(null, _identityFactory);

            action
                .Should()
                .ThrowExactly<ArgumentNullException>()
                .WithMessage(SubsidyMessages.ConsumerPriceIndexNotSetException);
        }

        public void ConsumerPriceIndexMustBeActive()
        {
            var cpi = _cpiFactory.Create();
            typeof(ConsumerPriceIndex)
                .BaseType.BaseType.BaseType
                .GetMethod("SetInactive", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(cpi, new object[] { DateTimeOffset.Now });

            Action action = () => _resFactory.Create().CreateNewWith(cpi, _identityFactory);

            action
                .Should()
                .Throw<Exception>()
                .WithMessage(SepsBaseMessage.InactiveException);
        }

        public void CreatesProperly()
        {
            var activeRenewableEnergySourceTariff = _resFactory.Create();

            _cpiFactory = new EconometricIndexFactory<ConsumerPriceIndex>(DateTime.Now.AddYears(-2));
            var newConsumerPriceIndex = _cpiFactory.Create();

            var newRenewableEnergySourceTariff =
                activeRenewableEnergySourceTariff.CreateNewWith(newConsumerPriceIndex, _identityFactory);

            activeRenewableEnergySourceTariff.Active.Until.Should().Be(newConsumerPriceIndex.Active.Since);
            newRenewableEnergySourceTariff.LowerRate.Should().Be(activeRenewableEnergySourceTariff.LowerRate);
            newRenewableEnergySourceTariff.HigherRate.Should().Be(
                (newConsumerPriceIndex.Amount / 100M) * activeRenewableEnergySourceTariff.HigherRate);
            newRenewableEnergySourceTariff.ConsumerPriceIndex.Should().Be(newConsumerPriceIndex);
            newRenewableEnergySourceTariff.Active.Should().Be(newConsumerPriceIndex.Active);
        }
    }
}