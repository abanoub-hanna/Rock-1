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

namespace Rock.Api.Groups
{
	[ServiceContract]
    public partial interface IMemberService
    {
		[OperationContract]
        Rock.Models.Groups.Member Get( string id );

        [OperationContract]
        void UpdateMember( string id, Rock.Models.Groups.Member Member );

        [OperationContract]
        void CreateMember( Rock.Models.Groups.Member Member );

        [OperationContract]
        void DeleteMember( string id );
    }
}
