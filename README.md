# EmailAPI


Steps to Create the Credentials
Register an Application in Azure AD:

Go to the Azure Portal.
Navigate to Azure Active Directory > App registrations.
Click New registration and provide:
A name for your application.
Supported account types (e.g., single-tenant or multi-tenant).
Redirect URI (optional for client secret flow).
Generate a Client Secret:

After registration, navigate to Certificates & secrets under your app.
Click New client secret and provide a description.
Copy the generated value. You'll use it in your application.
Configure API Permissions:

Go to API Permissions in your app.
Add a permission for Microsoft Graph and select the appropriate permission type:
Application Permissions: For server-to-server authentication.
Grant admin consent for the permissions.
Collect Required Information:

Tenant ID: Found under Azure Active Directory > Overview.
Client ID: Found in your app's registration overview.
Client Secret: Generated earlier.
Integrate in Code: Replace placeholders with actual values:


var clientSecretCredential = new ClientSecretCredential(
    "your-tenant-id", 
    "your-client-id", 
    "your-client-secret"
);
Use Cases
This approach is commonly used in server-side applications, background services, or API integrations.
It's not suitable for frontend apps because the client secret must remain confidential.
Let me know if you need more details or help implementing this!


