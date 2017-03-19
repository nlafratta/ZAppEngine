using System;
using System.Collections.Generic;
using System.Linq;
using Zirpl.AppEngine.Model;
using Zirpl.AppEngine.Model.Metadata;
using Zirpl.AppEngine.Model.Extensibility;

namespace Zirpl.Examples.ContactManager.DataService
{
    public partial class ProjectImageMapping
    {
        public ProjectImageMapping()
        {
            this.ToTable(this.GetTableName());
            //this.HasKey(this.GetKeyExpression());

            this.MapProperties();

            // ignore IsPersisted
            this.Ignore(entity => entity.IsPersisted);
        }

        protected virtual void MapProperties()
        {
            if (this.MapEntityBaseProperties)
            {
                Type type = typeof(Zirpl.Examples.ContactManager.Model.ProjectImage);
                if (typeof(IAuditable).IsAssignableFrom(type))
                {
                    this.Property(s => ((IAuditable)s).CreatedDate).IsRequired().IsDateTime();
                    this.Property(s => ((IAuditable)s).CreatedUserId).IsRequired();
                    this.Property(s => ((IAuditable)s).UpdatedDate).IsRequired().IsDateTime();
                    this.Property(s => ((IAuditable)s).UpdatedUserId).IsRequired();
                }
                if (typeof(IVersionable).IsAssignableFrom(type))
                {
                    this.Property(s => ((IVersionable)s).RowVersion).IsRequired().IsRowVersion();
                }
                // TODO: map IExtensible
            }

            this.Property(o => o.Id)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.Id_IsRequired);
            this.Property(o => o.RowVersion)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.RowVersion_IsRequired);
            this.Property(o => o.CreatedUserId)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.CreatedUserId_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.CreatedUserId_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.CreatedUserId_IsMaxLength);
            this.Property(o => o.UpdatedUserId)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.UpdatedUserId_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.UpdatedUserId_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.UpdatedUserId_IsMaxLength);
            this.Property(o => o.CreatedDate)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.CreatedDate_IsRequired)
                .IsDateTime();
            this.Property(o => o.UpdatedDate)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.UpdatedDate_IsRequired)
                .IsDateTime();
            this.Property(o => o.Image)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.Image_IsRequired)
                .HasColumnType("image");
            this.Property(o => o.Caption)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.Caption_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.Caption_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.Caption_IsMaxLength);
            this.Property(o => o.ProjectId)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectImageMetadataConstants.ProjectId_IsRequired);


            this.OnMapProperties();
        }

        partial void OnMapProperties();

        private String GetTableName()
        {
            return typeof(Zirpl.Examples.ContactManager.Model.ProjectImage).Name;
        }

        //protected virtual Expression<Func<TEntity, TId>> GetKeyExpression()
        //{
        //    return o => o.Id;
        //}

        protected virtual bool MapEntityBaseProperties
        {
            // TODO: should be FALSE if base properties have already been mapped
            get { return true; }
        }
    }
}
