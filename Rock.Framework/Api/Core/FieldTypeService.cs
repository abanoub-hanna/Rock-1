//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

using Rock.Cms.Security;

namespace Rock.Api.Core
{
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class FieldTypeService : IFieldTypeService
    {
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Models.Core.FieldType Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using (Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope())
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Services.Core.FieldTypeService FieldTypeService = new Rock.Services.Core.FieldTypeService();
                Rock.Models.Core.FieldType FieldType = FieldTypeService.Get( int.Parse( id ) );
                if ( FieldType.Authorized( "View", currentUser ) )
                    return FieldType;
                else
                    throw new FaultException( "Unauthorized" );
            }
        }
		
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateFieldType( string id, Rock.Models.Core.FieldType FieldType )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Core.FieldTypeService FieldTypeService = new Rock.Services.Core.FieldTypeService();
                Rock.Models.Core.FieldType existingFieldType = FieldTypeService.Get( int.Parse( id ) );
                if ( existingFieldType.Authorized( "Edit", currentUser ) )
                {
                    uow.objectContext.Entry(existingFieldType).CurrentValues.SetValues(FieldType);
                    FieldTypeService.Save( existingFieldType, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateFieldType( Rock.Models.Core.FieldType FieldType )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Core.FieldTypeService FieldTypeService = new Rock.Services.Core.FieldTypeService();
                FieldTypeService.Add( FieldType, currentUser.PersonId() );
                FieldTypeService.Save( FieldType, currentUser.PersonId() );
            }
        }

		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteFieldType( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Core.FieldTypeService FieldTypeService = new Rock.Services.Core.FieldTypeService();
                Rock.Models.Core.FieldType FieldType = FieldTypeService.Get( int.Parse( id ) );
                if ( FieldType.Authorized( "Edit", currentUser ) )
                {
                    FieldTypeService.Delete( FieldType, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

    }
}
