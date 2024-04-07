template name
<!--paper-dashboard-2-pro-html-v2.1.1-->

Programming languages : 
# Dotnet Core 3.1 
# Vuejs Stable release	2.6.10 / December 13, 2019
# element-ui for frontend UI
https://element.eleme.io/

IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'VoterCards')) 
BEGIN
    EXEC ('CREATE SCHEMA [VoterCards] AUTHORIZATION [dbo]')
END

ALTER SCHEMA dbo
    TRANSFER VoterCards.Errors

     Scaffold-DbContext "server=DESKTOP-G795488;database=WorkplaceReservation;uid=Ahmed;pwd=35087124567Ahmed;Trusted_Connection=false;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models 

    Scaffold-DbContext "server=DESKTOP-G795488;database=Prisoners;uid=Ahmed;pwd=35087124567Ahmed;Trusted_Connection=false;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -F

 Scaffold-DbContext "Server=95.216.93.102;Database=VoterCards;User Id=VoterCards;password=abdABD123!@#;Trusted_Connection=false;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
 
 Scaffold-DbContext "Server=server=95.216.93.102;database=ScreenControl;uid=ScreenControl;pwd=abdABD123!@#;Trusted_Connection=false;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -F

Scaffold-DbContext "Server=DESKTOP-4AI87L8\SQLEXPRESS;Database=QualityAssuranceCenter;Trusted_Connection=True;;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -F

Scaffold-DbContext "Server=DESKTOP-4AI87L8\SQLEXPRESS;Database=QualityAssuranceCenter;Trusted_Connection=True;;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -context QualityAssuranceCenterContext  -OutputDir Models

Scaffold-DbContext "Server=95.216.93.102;Database=qac;User Id=QAC;password=abdABD123!@#;Trusted_Connection=false;MultipleActiveResultSets=true;" Microsoft.EntityFrameworkCore.SqlServer -context QualityAssuranceCenterContext  -OutputDir Models

Add External Models

https://www.learnentityframeworkcore.com/walkthroughs/existing-database


// update module
Scaffold-DbContext "Server=localhost;port=3308;user=root;database=cra_accounts;" MySql.Data.EntityFrameworkCore -OutputDir "" -F