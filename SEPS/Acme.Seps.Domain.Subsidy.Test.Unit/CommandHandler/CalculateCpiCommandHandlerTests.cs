﻿using Acme.Domain.Base.Factory;
using Acme.Domain.Base.Repository;
using Acme.Seps.Domain.Base.CommandHandler;
using Acme.Seps.Domain.Subsidy.Command;
using Acme.Seps.Domain.Subsidy.CommandHandler;
using Acme.Seps.Domain.Subsidy.Entity;
using Acme.Seps.Domain.Subsidy.Repository;
using Acme.Seps.Domain.Subsidy.Test.Unit.Factory;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Acme.Seps.Domain.Subsidy.Test.Unit.CommandHandler
{
    public class CalculateCpiCommandHandlerTests
    {
        private readonly ISepsCommandHandler<CalculateCpiCommand> _calculateCpi;
        private readonly IUnitOfWork _unitOfWork;

        public CalculateCpiCommandHandlerTests()
        {
            IEconometricIndexFactory<ConsumerPriceIndex> cpiFactory =
                new EconometricIndexFactory<ConsumerPriceIndex>(DateTime.Now.AddYears(-4));
            var activeCpi = cpiFactory.Create();

            ITariffFactory<RenewableEnergySourceTariff> resFactory =
                new TariffFactory<RenewableEnergySourceTariff>(activeCpi);
            var activeRenewableEnergySourceTariff = resFactory.Create();

            var repository = Substitute.For<IRepository>();
            repository
                .GetSingle(Arg.Any<ActiveSpecification<ConsumerPriceIndex>>())
                .Returns(activeCpi);
            repository
                .GetAll(Arg.Any<CpiRenewableEnergySourceTariffSpecification>())
                .Returns(new List<RenewableEnergySourceTariff> { activeRenewableEnergySourceTariff });

            _unitOfWork = Substitute.For<IUnitOfWork>();

            _calculateCpi = new CalculateCpiCommandHandler(
                repository, _unitOfWork, Substitute.For<IIdentityFactory<Guid>>());
        }

        public void ExecutesProperly()
        {
            var calculateCommand = new CalculateCpiCommand
            {
                Amount = 100M,
                Remark = nameof(CalculateCpiCommand)
            };

            using (var monitoredEvent = _calculateCpi.Monitor())
            {
                _calculateCpi.Handle(calculateCommand);

                _unitOfWork.Received().Insert(Arg.Any<ConsumerPriceIndex>());
                _unitOfWork.Received().Insert(Arg.Any<RenewableEnergySourceTariff>());
                _unitOfWork.Received().Commit();
                monitoredEvent.Should().Raise("UseCaseExecutionProcessing");
            }
        }
    }
}