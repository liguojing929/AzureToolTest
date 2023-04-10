using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.KeyVault
{
    public class KeyVaultCommon
    {
        // keyvault-app-for-credential:
        private readonly string ClientID = "5165ef84-ae32-4cea-86cd-d6fbb758df47";
        private readonly string ObjectID = "d4e10996-3029-4c2e-bf5e-5de6821bc5f1";
        private readonly string TenantID = "ffdc92d4-d261-48ce-a9d8-2aa2075456f3";
        private readonly string KeyVaultCredentialSecret_Value = "yYg8Q~yUuUpUkVVKQmmvGQ5XOouIriryZd6s2aYn";
        private readonly string KeyVaultCredentialSecret_SecretID = "8c39c92c-b0a6-49a7-871e-449c0f754f76";


        private readonly string VaultUrl = "https://personal-testkeyvault-1.vault.azure.net/";
        private KeyClient keyClient;
        private SecretClient secretClient;
        private ClientSecretCredential credential;

        public KeyVaultCommon() 
        {
            Uri keyVaultUri = new Uri(VaultUrl);
            this.credential = new ClientSecretCredential(TenantID, ClientID, KeyVaultCredentialSecret_Value);
            //this.keyClient = new KeyClient(keyVaultUri, new DefaultAzureCredential());
            //this.secretClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());
            this.keyClient = new KeyClient(keyVaultUri, credential);
            this.secretClient = new SecretClient(keyVaultUri, credential);
        }

        public async Task<string> GetSecreteValue(string secreteName)
        {
            KeyVaultSecret screte = await this.secretClient.GetSecretAsync(secreteName);
            return screte.Value;
        }

        public async Task<string> GetKeyValue(string keyName)
        {
            KeyVaultKey res = await this.keyClient.GetKeyAsync(keyName);
            return res.ToString();
        }


    }
}
