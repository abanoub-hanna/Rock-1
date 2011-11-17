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

namespace Rock.Api.Groups
{
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class MemberService : IMemberService
    {
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Models.Groups.Member Get( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using (Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope())
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Services.Groups.MemberService MemberService = new Rock.Services.Groups.MemberService();
                Rock.Models.Groups.Member Member = MemberService.Get( int.Parse( id ) );
                if ( Member.Authorized( "View", currentUser ) )
                    return Member;
                else
                    throw new FaultException( "Unauthorized" );
            }
        }
		
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateMember( string id, Rock.Models.Groups.Member Member )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Groups.MemberService MemberService = new Rock.Services.Groups.MemberService();
                Rock.Models.Groups.Member existingMember = MemberService.Get( int.Parse( id ) );
                if ( existingMember.Authorized( "Edit", currentUser ) )
                {
                    uow.objectContext.Entry(existingMember).CurrentValues.SetValues(Member);
                    MemberService.Save( existingMember, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateMember( Rock.Models.Groups.Member Member )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Groups.MemberService MemberService = new Rock.Services.Groups.MemberService();
                MemberService.Add( Member, currentUser.PersonId() );
                MemberService.Save( Member, currentUser.PersonId() );
            }
        }

		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteMember( string id )
        {
            var currentUser = System.Web.Security.Membership.GetUser();
            if ( currentUser == null )
                throw new FaultException( "Must be logged in" );

            using ( Rock.Helpers.UnitOfWorkScope uow = new Rock.Helpers.UnitOfWorkScope() )
            {
                uow.objectContext.Configuration.ProxyCreationEnabled = false;

                Rock.Services.Groups.MemberService MemberService = new Rock.Services.Groups.MemberService();
                Rock.Models.Groups.Member Member = MemberService.Get( int.Parse( id ) );
                if ( Member.Authorized( "Edit", currentUser ) )
                {
                    MemberService.Delete( Member, currentUser.PersonId() );
                }
                else
                    throw new FaultException( "Unauthorized" );
            }
        }

    }
}
