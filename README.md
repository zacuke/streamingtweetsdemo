# streamingtweetsdemo

Put the Twitter bearer token in appsettings.json

  "StreamingTweetsDemo": {
    "BearerToken": "<token here>"
  }

# To keep secrets out of source control:

Initially I use secrets.json (right click project and click Manage Secrets)
Eventually, I use Azure key vault and add code like this:

```
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
    new DefaultAzureCredential());//DefaultAzureCredential gets your sso
```