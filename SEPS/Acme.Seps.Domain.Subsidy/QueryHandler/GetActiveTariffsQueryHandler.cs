﻿using Acme.Domain.Base.QueryHandler;
using Acme.Seps.Domain.Subsidy.Query;
using Acme.Seps.Domain.Subsidy.QueryResult;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace Acme.Seps.Domain.Subsidy.QueryHandler
{
    public class GetActiveTariffsQueryHandler
        : IQueryHandler<GetActiveTariffsQuery, IReadOnlyList<ActiveTariffsQueryResult>>
    {
        private readonly IDbConnection _connection;

        public GetActiveTariffsQueryHandler(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        IReadOnlyList<ActiveTariffsQueryResult>
            IQueryHandler<GetActiveTariffsQuery, IReadOnlyList<ActiveTariffsQueryResult>>
            .Handle(GetActiveTariffsQuery query) =>
            _connection
                .Query<ActiveTariffsQueryResult>(
                    "SELECT " +
                    "FORMAT(trf.ActiveFrom, 'd') AS ActiveFrom, " +
                    "FORMAT(trf.ActiveTill, 'd') AS ActiveTill, " +
                    "trf.LowerProductionLimit, " +
                    "trf.UpperProductionLimit, " +
                    "trf.LowerRate, " +
                    "trf.HigherRate, " +
                    "EconometricIndexAmount = eix.Amount " +
                    "FROM parameter.Tariffs AS trf " +
                    "INNER JOIN parameter.EconometricIndexes AS eix " +
                    "ON (trf.ConsumerPriceIndexId = eix.Id OR trf.NaturalGasSellingPriceId = eix.Id) " +
                    "WHERE trf.TariffType = @Type",
                    new { Type = query.TariffType.Name }).AsList();
    }
}
