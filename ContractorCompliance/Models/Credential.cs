namespace ContractorCompliance.Models;
public record Credential(int Id, string Contractor, string CredentialType, DateOnly ExpiresOn, string Status);
