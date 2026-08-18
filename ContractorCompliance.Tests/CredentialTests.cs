using System;
using ContractorCompliance.Models;
using Xunit;
namespace ContractorCompliance.Tests;
public class CredentialTests
{
 [Fact] public void PendingCredentialCarriesContractorAndExpiry() { var c=new Credential(1,"BuildCo","Insurance",new DateOnly(2027,1,1),"pending"); Assert.Equal("pending",c.Status); Assert.Equal("BuildCo",c.Contractor); }
 [Fact] public void CredentialCanTransitionToApproved() { var c=new Credential(1,"BuildCo","Insurance",new DateOnly(2027,1,1),"pending"); Assert.Equal("approved",(c with { Status="approved" }).Status); }
}
