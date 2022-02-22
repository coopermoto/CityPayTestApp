# CityPayTestApp
## Arguments: `<ClientId> <LicenceKey> <BatchId>`
e.g. `CityPayTestApp AB123456 ABCDEFGHIJKLMNOP 9999`

Sample output from my dev machine:
```
20:44:48: ## Start
20:44:48: Created BatchTransaction object 4_CP-MS-R
20:44:48: Created ProcessBatchRequest object
20:44:48: > BatchProcessRequest
20:44:48: >> Creating Configuration
20:44:48: >> Generating Key
20:44:48: >> Creating Api
20:44:48: >> Calling Api Method
20:44:48: < BatchProcessRequest
20:44:48: Request is valid
20:44:48: Created CheckBatchStatus object
20:44:48: > CheckBatchStatusRequest
20:44:48: >> Creating Configuration
20:44:48: >> Generating Key
20:44:48: >> Creating Api
20:44:48: >> Calling Api Method
20:44:48: < CheckBatchStatusRequest
20:44:48: Batch Status INITIALISED
...
20:59:49: > CheckBatchStatusRequest
20:59:49: >> Creating Configuration
20:59:49: >> Generating Key
20:59:49: >> Creating Api
20:59:49: >> Calling Api Method
20:59:50: < CheckBatchStatusRequest
20:59:50: Batch Status COMPLETE
20:59:50: Created BatchReportRequest object
20:59:50: > BatchReportRequest
20:59:50: >> Creating Configuration
20:59:50: >> Generating Key
20:59:50: >> Creating Api
20:59:50: >> Calling Api Method
20:59:50: < BatchReportRequest
20:59:50: Batch Status COMPLETE
20:59:50: Transaction Message 000:Accepted Transaction
20:59:50: ## End
```

Sample output from our server:
```
21:06:44: ## Start
21:06:44: Created BatchTransaction object 5_CP-MS-R
21:06:44: Created ProcessBatchRequest object
21:06:44: > BatchProcessRequest
21:06:44: >> Creating Configuration
21:06:44: >> Generating Key
21:06:44: >> Creating Api
21:06:44: >> Calling Api Method
21:14:45: ApiException: Error calling BatchProcessRequest: {"code":"S005","context":"","message":"Not Authorised: Unauthorized, invalid api key"}
21:14:45:    at CityPayAPI.Api.BatchProcessingApi.BatchProcessRequestWithHttpInfo(ProcessBatchRequest processBatchRequest)
   at CityPayTestApp.Program.Main(String[] args) in D:\Dev\DJH\CityPayTestApp\CityPayTestApp\CityPayTestApp\Program.cs:line 50
21:14:45: ## End
```
