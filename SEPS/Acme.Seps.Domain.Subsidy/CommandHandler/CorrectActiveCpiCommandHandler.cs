﻿using Acme.Domain.Base.CommandHandler;
using Acme.Domain.Base.Factory;
using Acme.Domain.Base.Repository;
using Acme.Seps.Domain.Base.CommandHandler;
using Acme.Seps.Domain.Base.Entity;
using Acme.Seps.Domain.Subsidy.Command;
using Acme.Seps.Domain.Subsidy.Entity;
using Acme.Seps.Domain.Subsidy.Infrastructure;
using Acme.Seps.Domain.Subsidy.Repository;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.Seps.Domain.Subsidy.CommandHandler
{
    public sealed class CorrectActiveCpiCommandHandler
        : BaseCommandHandler, ISepsCommandHandler<CorrectActiveCpiCommand>
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityFactory<Guid> _identityFactory;

        public CorrectActiveCpiCommandHandler(
            IRepository repository, IUnitOfWork unitOfWork, IIdentityFactory<Guid> identityFactory)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _identityFactory = identityFactory ?? throw new ArgumentNullException(nameof(identityFactory));
        }

        void ICommandHandler<CorrectActiveCpiCommand>.Handle(CorrectActiveCpiCommand command)
        {
            var activeCpi = GetActiveConsumerPriceIndex();
            var previousRes = GetPreviousActiveRenewableEnergySourceTariffs(activeCpi);

            activeCpi.Correct(command.Amount, command.Remark);
            CorrectRenewableEnergySourceTariffs(activeCpi, previousRes);

            _unitOfWork.Commit();

            LogConsumerPriceIndexCorrection(activeCpi);
            LogSuccessfulCommit();
        }

        private ConsumerPriceIndex GetActiveConsumerPriceIndex() =>
            _repository.GetSingle(new ActiveSpecification<ConsumerPriceIndex>());

        private IReadOnlyList<RenewableEnergySourceTariff> GetPreviousActiveRenewableEnergySourceTariffs(
            ConsumerPriceIndex cpi) =>
            _repository.GetAll(new PreviousActiveSpecification<RenewableEnergySourceTariff>(cpi));

        private void CorrectRenewableEnergySourceTariffs(
            ConsumerPriceIndex correctedCpi, IEnumerable<RenewableEnergySourceTariff> previousRes)
        {
            GetActiveRenewableEnergySourceTariffs().ForEach(res =>
            {
                res.CpiCorrection(
                    correctedCpi, PreviousRenewableEnergySourceBy(res.ProjectTypeId, res.LowerProductionLimit));

                _unitOfWork.Update(res);

                LogRenewableEnergySourceTariffCorrection(res);
            });

            RenewableEnergySourceTariff PreviousRenewableEnergySourceBy(
                Guid projectTypeId, decimal? lowerProductionLimit) =>
                previousRes.Single(res =>
                    res.ProjectTypeId.Equals(projectTypeId) && res.LowerProductionLimit.Equals(lowerProductionLimit));
        }

        private List<RenewableEnergySourceTariff> GetActiveRenewableEnergySourceTariffs() =>
            _repository.GetAll(new ActiveSpecification<RenewableEnergySourceTariff>()).ToList();

        private void LogRenewableEnergySourceTariffCorrection(RenewableEnergySourceTariff resTariff) =>
            Log(new EntityExecutionLoggingEventArgs
            {
                Message = string.Format(
                    SubsidyMessages.TariffCorrectionLog,
                    nameof(RenewableEnergySourceTariff).Humanize(LetterCasing.LowerCase),
                    resTariff.Active,
                    resTariff.LowerRate,
                    resTariff.HigherRate)
            });

        private void LogConsumerPriceIndexCorrection(ConsumerPriceIndex cpi) =>
            Log(new EntityExecutionLoggingEventArgs
            {
                Message = string.Format(
                    SubsidyMessages.ParameterCorrectionLog,
                    nameof(ConsumerPriceIndex).Humanize(LetterCasing.LowerCase),
                    cpi.Active,
                    cpi.Amount)
            });
    }
}