﻿using Acme.Seps.Domain.Base.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Seps.Repository.Subsidy.Configuration
{
    internal class BaseParameterConfiguration<TParameterEntity> where TParameterEntity : SepsAggregate
    {
        public virtual void Configure(EntityTypeBuilder<TParameterEntity> builder)
        {
            //builder.Property<byte[]>("RowVersion").IsRowVersion();

            builder.OwnsOne(vte => vte.Period, vte =>
            {
                vte.Property(ppy => ppy.ValidFrom).HasColumnName("ValidFrom").IsRequired();
                vte.Property(ppy => ppy.ValidTill).HasColumnName("ValidTill");
            });
        }
    }
}