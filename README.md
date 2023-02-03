## To run the app

Put the Twitter bearer token in appsettings.json
```
  "StreamingTweetsDemo": {
    "BearerToken": "<token here>"
  }
```  
  Hit Go and it should display  a web page  and show statistics in a few moments.

## To keep secrets out of source control:

For dev, I use secrets.json (right click project and click Manage Secrets)

For servers (works for dev too), I use Azure key vault and add code like this:

```
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
    new DefaultAzureCredential());//DefaultAzureCredential gets your sso
```
