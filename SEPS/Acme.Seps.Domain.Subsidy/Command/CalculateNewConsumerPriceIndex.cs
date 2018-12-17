﻿using Acme.Domain.Base.CommandHandler;
using Acme.Domain.Base.Factory;
using Acme.Domain.Base.Repository;
using Acme.Seps.Domain.Base.CommandHandler;
using Acme.Seps.Domain.Base.Entity;
using Acme.Seps.Domain.Base.Repository;
using Acme.Seps.Domain.Subsidy.Command.Entity;
using Acme.Seps.Domain.Subsidy.Command.Infrastructure;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.Seps.Domain.Subsidy.Command
{
    public sealed class CalculateCpiCommandHandler
        : BaseCommandHandler, ISepsCommandHandler<CalculateConsumerPriceIndexCommand>
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityFactory<Guid> _identityFactory;

        public CalculateCpiCommandHandler(
            IRepository repository, IUnitOfWork unitOfWork, IIdentityFactory<Guid> identityFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _identityFactory = identityFactory ?? throw new ArgumentNullException(nameof(identityFactory));
        }

        void ICommandHandler<CalculateConsumerPriceIndexCommand>.Handle(CalculateConsumerPriceIndexCommand command)
        {
            var activeCpi = GetActiveConsumerPriceIndex();
            var newCpi = CreateNewConsumerPriceIndex(command, activeCpi);

            CreateNewRenewableEnergySourceTariffs(newCpi);

            _unitOfWork.Update(activeCpi);
            _unitOfWork.Insert(newCpi);
            _unitOfWork.Commit();

            LogNewConsumerPriceIndex(newCpi);
            LogSuccessfulCommit();
        }

        private ConsumerPriceIndex GetActiveConsumerPriceIndex() =>
            _repository.GetSingle(new ActiveSpecification<ConsumerPriceIndex>());

        private ConsumerPriceIndex CreateNewConsumerPriceIndex(
            CalculateConsumerPriceIndexCommand command, ConsumerPriceIndex cpi) =>
            cpi.CreateNew(command.Amount, command.Remark, _identityFactory);

        private void CreateNewRenewableEnergySourceTariffs(ConsumerPriceIndex newCpi)
        {
            GetActiveRenewableEnergySourceTariffs().ForEach(res =>
            {
                var newRenewableEnergyTariff = CreateNewRenewableEnergySourceTariff(res);

                _unitOfWork.Update(res);
                _unitOfWork.Insert(newRenewableEnergyTariff);

                LogNewRenewableEnergySourceTariff(newRenewableEnergyTariff);
            });

            RenewableEnergySourceTariff CreateNewRenewableEnergySourceTariff(RenewableEnergySourceTariff res) =>
                res.CreateNewWith(newCpi, _identityFactory);
        }

        private List<RenewableEnergySourceTariff> GetActiveRenewableEnergySourceTariffs() =>
            _repository.GetAll(new ActiveSpecification<RenewableEnergySourceTariff>()).ToList();

        private void LogNewRenewableEnergySourceTariff(RenewableEnergySourceTariff res) =>
            Log(new EntityExecutionLoggingEventArgs
            {
                Message = string.Format(
                    SubsidyMessages.InsertTariffLog,
                    nameof(RenewableEnergySourceTariff).Humanize(LetterCasing.LowerCase),
                    res.Active,
                    res.LowerRate,
                    res.HigherRate)
            });

        private void LogNewConsumerPriceIndex(ConsumerPriceIndex cpi) =>
            Log(new EntityExecutionLoggingEventArgs
            {
                Message = string.Format(
                    SubsidyMessages.InsertParameterLog,
                    nameof(ConsumerPriceIndex).Humanize(LetterCasing.LowerCase),
                    cpi.Active,
                    cpi.Amount)
            });
    }

    public sealed class CalculateConsumerPriceIndexCommand
    {
        public decimal Amount { get; set; }
        public string Remark { get; set; }
    }
}