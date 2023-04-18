
LOCATION=eastus
RESOURCE_GROUP_NAME=ftp-health-monitor
KEY_VAULT_NAME=ftp-health-monitor-vault
FUNCTION_APP_NAME=ftp-health-monitor
FUNCTION_STORAGE_ACCOUNT_NAME=ftphealthmonitorstorage

# Create the resource group
az group create \
    --name $RESOURCE_GROUP_NAME \
    --location $LOCATION

# Create the Storage needed by the function app
az storage account create \
  --name $FUNCTION_STORAGE_ACCOUNT_NAME \
  --resource-group $RESOURCE_GROUP_NAME \
  --sku Standard_LRS \
  --kind StorageV2 \
  --access-tier Hot


# Create the Functions App
az functionapp create \
    --name $FUNCTION_APP_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --storage-account $FUNCTION_STORAGE_ACCOUNT_NAME \
    --consumption-plan-location $LOCATION \
    --functions-version 4 \
    --os-type Linux \
    --runtime dotnet \
    --runtime-version 6 \
    --assign-identity '[system]' \
    --disable-app-insights \
    --https-only

# Create the Key Vault
az keyvault create \
    --name $KEY_VAULT_NAME \
    --resource-group $RESOURCE_GROUP_NAME 
#    --enable-rbac-authorization



# This is the id of the managed identity for the function app
FUNCTION_APP_PRINCIPAL_ID=$(az webapp show --name $FUNCTION_APP_NAME --resource-group $RESOURCE_GROUP_NAME --query identity.principalId --output tsv)

# This is the resource id of the key vault
KEY_VAULT_RESOURCE_ID=$(az keyvault show --name $KEY_VAULT_NAME --query id --output tsv)


# For the managed identity of the web app, give it read access to the key vault
az role assignment create \
    --assignee $FUNCTION_APP_PRINCIPAL_ID \
    --scope $KEY_VAULT_RESOURCE_ID \
    --role "Key Vault Reader"

# Now the service principal of managed identity needs get and list permission in the vault.  Note the app cannot write to the vault
az keyvault set-policy \
    --name $KEY_VAULT_NAME \
    --object-id $FUNCTION_APP_PRINCIPAL_ID \
    --secret-permissions get list   




# This puts a secret in the key vault
# DO NOT store commands like this in a script, otherwise someone can look at your script and get your secrets
# This is just a sample so you 
az keyvault secret set \
    --vault-name $KEY_VAULT_NAME \
    --name ExampleSecret \
    --value "SuperSecretValue" 


az functionapp config appsettings set \
    --name $FUNCTION_APP_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --settings MySetting="@Microsoft.KeyVault(VaultName=$KEY_VAULT_NAME;SecretName=ExampleSecret)"



# These commands put config settings in the function app that tells the function app to go to key vault and get the value
az functionapp config appsettings set \
    --name $FUNCTION_APP_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --settings FtpServer="@Microsoft.KeyVault(VaultName=$KEY_VAULT_NAME;SecretName=FtpServer)"

az functionapp config appsettings set \
    --name $FUNCTION_APP_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --settings FtpUsername="@Microsoft.KeyVault(VaultName=$KEY_VAULT_NAME;SecretName=FtpUsername)"

az functionapp config appsettings set \
    --name $FUNCTION_APP_NAME \
    --resource-group $RESOURCE_GROUP_NAME \
    --settings FtpPassword="@Microsoft.KeyVault(VaultName=$KEY_VAULT_NAME;SecretName=FtpPassword)"





   



