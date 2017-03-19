using System;
using System.Collections.Generic;
using System.Linq;
using Zirpl.AppEngine.Model;
using Zirpl.AppEngine.Model.Metadata;
using Zirpl.AppEngine.Model.Extensibility;

namespace Zirpl.Examples.ContactManager.DataService
{
    public partial class ProjectMapping
    {
        public ProjectMapping()
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
                Type type = typeof(Zirpl.Examples.ContactManager.Model.Project);
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
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.Id_IsRequired);
            this.Property(o => o.RowVersion)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.RowVersion_IsRequired);
            this.Property(o => o.CreatedUserId)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.CreatedUserId_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.CreatedUserId_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.CreatedUserId_IsMaxLength);
            this.Property(o => o.UpdatedUserId)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.UpdatedUserId_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.UpdatedUserId_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.UpdatedUserId_IsMaxLength);
            this.Property(o => o.CreatedDate)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.CreatedDate_IsRequired)
                .IsDateTime();
            this.Property(o => o.UpdatedDate)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.UpdatedDate_IsRequired)
                .IsDateTime();
            this.Property(o => o.Title)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.Title_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.Title_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.Title_IsMaxLength);
            this.Property(o => o.SubTitle)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.SubTitle_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.SubTitle_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.SubTitle_IsMaxLength);
            this.Property(o => o.Year)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.Year_IsRequired);
            this.Property(o => o.Description)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.Description_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.Description_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.Description_IsMaxLength);
            this.Property(o => o.UrlSuffix)
                .IsRequired(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.UrlSuffix_IsRequired)
                .HasMaxLength(Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.UrlSuffix_MaxLength, Zirpl.Examples.ContactManager.Model.ProjectMetadataConstants.UrlSuffix_IsMaxLength);


            this.OnMapProperties();
        }

        partial void OnMapProperties();

        private String GetTableName()
        {
            return typeof(Zirpl.Examples.ContactManager.Model.Project).Name;
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
