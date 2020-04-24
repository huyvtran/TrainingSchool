' This file is used to provide the Classes and PA's that need Context based data. Application context does not exist in PAs,
' so this class will abstract all context based configuration data.

Imports System
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Collections
Imports System.IO
Imports System.Web.UI



Public Class Configuration



    

    Public Shared Function GetCurrentUSerID() As String 

        Return HttpContext.Current.User.Identity.Name
    Return False 
 End function

End Class



