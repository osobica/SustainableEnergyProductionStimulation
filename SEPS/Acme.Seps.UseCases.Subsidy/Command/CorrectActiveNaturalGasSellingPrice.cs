﻿using Acme.Domain.Base.CommandHandler;
using Acme.Domain.Base.Factory;
using Acme.Domain.Base.Repository;
using Acme.Seps.Domain.Base.CommandHandler;
using Acme.Seps.Domain.Base.Entity;
using Acme.Seps.Domain.Base.Repository;
using Acme.Seps.UseCases.Subsidy.Command.DomainService;
using Acme.Seps.UseCases.Subsidy.Command.Entity;
using Acme.Seps.UseCases.Subsidy.Command.Infrastructure;
using Acme.Seps.UseCases.Subsidy.Command.Repository;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Acme.Seps.UseCases.Subsidy.Command
{
    public sealed class CorrectActiveNaturalGasSellingPriceCommandHandler
        : BaseCommandHandler, ISepsCommandHandler<CorrectActiveNaturalGasSellingPriceCommand>
    {
        private readonly ICogenerationParameterService _cogenerationParameterService;
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityFactory<Guid> _identityFactory;

        public CorrectActiveNaturalGasSellingPriceCommandHandler(
            ICogenerationParameterService cogenerationParameterService,
            IRepository repository,
            IUnitOfWork unitOfWork,
            IIdentityFactory<Guid> identityFactory)
        {
            _cogenerationParameterService =
                cogenerationParameterService ?? throw new ArgumentNullException(nameof(cogenerationParameterService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _identityFactory = identityFactory ?? throw new ArgumentNullException(nameof(identityFactory));
        }

        void ICommandHandler<CorrectActiveNaturalGasSellingPriceCommand>.Handle(
            CorrectActiveNaturalGasSellingPriceCommand command)
        {
            var activeNaturalGasSellingPrice = GetActiveNaturalGasSellingPrice();
            var previousActiveNaturalGasSellingPrice =
                GetPreviousActiveNaturalGasSellingPrice(activeNaturalGasSellingPrice);
            var previousCogenerations = GetPreviousActiveCogenerationTariffs(activeNaturalGasSellingPrice);

            activeNaturalGasSellingPrice.Correct(
                command.Amount, command.Remark, command.Year, command.Month, previousActiveNaturalGasSellingPrice);
            CorrectCogenerationTariffs(activeNaturalGasSellingPrice, previousCogenerations);

            _unitOfWork.Update(activeNaturalGasSellingPrice);
            _unitOfWork.Update(previousActiveNaturalGasSellingPrice);
            _unitOfWork.Commit();

            LogNaturalGasSellingPriceCorrection(activeNaturalGasSellingPrice);
            LogSuccessfulCommit();
        }

        private NaturalGasSellingPrice GetActiveNaturalGasSellingPrice() =>
            _repository.GetSingle(new ActiveSpecification<NaturalGasSellingPrice>());

        private NaturalGasSellingPrice GetPreviousActiveNaturalGasSellingPrice(NaturalGasSellingPrice ngsp) =>
            _repository.GetSingle(new PreviousActiveSpecification<NaturalGasSellingPrice>(ngsp));

        private IReadOnlyList<CogenerationTariff> GetPreviousActiveCogenerationTariffs(NaturalGasSellingPrice ngsp) =>
            _repository.GetAll(new PreviousActiveSpecification<CogenerationTariff>(ngsp));

        private void CorrectCogenerationTariffs(
            NaturalGasSellingPrice correctedNgsp, IEnumerable<CogenerationTariff> previousCogenerations)
        {
            GetActiveCogenerationTariffs().ForEach(ctf =>
            {
                var previousCogeneration = CogenerationTariffByProjectType(ctf.ProjectTypeId);

                ctf.NgspCorrection(
                    GetNaturalGasPricesWithinYear(correctedNgsp.Active.Since.Year),
                    _cogenerationParameterService,
                    correctedNgsp,
                    previousCogeneration);

                _unitOfWork.Update(ctf);
                _unitOfWork.Update(previousCogeneration);

                LogNewCogenerationTariffCorrection(ctf);
            });

            CogenerationTariff CogenerationTariffByProjectType(Guid projectTypeId) =>
                previousCogenerations.Single(ctf => ctf.ProjectTypeId.Equals(projectTypeId));
        }

        private List<CogenerationTariff> GetActiveCogenerationTariffs() =>
            _repository.GetAll(new ActiveSpecification<CogenerationTariff>()).ToList();

        private IReadOnlyList<NaturalGasSellingPrice> GetNaturalGasPricesWithinYear(int year) =>
            _repository.GetAll(new NaturalGasSellingPricesInAYearSpecification(year));

        private void LogNewCogenerationTariffCorrection(CogenerationTariff cogenerationTariff) =>
            Log(new EntityExecutionLoggingEventArgs
            {
                Message = string.Format(
                    SubsidyMessages.TariffCorrectionLog,
                    nameof(CogenerationTariff).Humanize(LetterCasing.LowerCase),
                    cogenerationTariff.Active,
                    cogenerationTariff.LowerRate,
                    cogenerationTariff.HigherRate)
            });

        private void LogNaturalGasSellingPriceCorrection(NaturalGasSellingPrice naturalGasSellingPrice) =>
            Log(new EntityExecutionLoggingEventArgs
            {
                Message = string.Format(
                    SubsidyMessages.ParameterCorrectionLog,
                    nameof(NaturalGasSellingPrice).Humanize(LetterCasing.LowerCase),
                    naturalGasSellingPrice.Active,
                    naturalGasSellingPrice.Amount)
            });
    }

    public sealed class CorrectActiveNaturalGasSellingPriceCommand
    {
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}