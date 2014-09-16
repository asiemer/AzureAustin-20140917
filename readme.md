#AzureAustin - 20140917

Code presented at AzureAustin 20140917.

## Azure Search and Azure DocumentDB

An attempt to use Azure Search as a search first experience to navigate a product catalog stored in Azure DocumentDB.

## Slides / Task List

- Both search and db are in "preview"
	- Can be found here portal.azure.com
- Create Azure Search instance
	- shared or standard?
	- standard supports dedicated resource that can be scaled based on workload
	- shared is free and is solely meant for testing
	- shared == no performance guarantees
	- pricing
		- partitions == document counts (size)
		- replicas == performance (query load)
	- HA requires a minimum of 3 replicas (3 copies of the same index)
	- REST API (no SDK): [http://msdn.microsoft.com/en-us/library/azure/dn798935.aspx](http://msdn.microsoft.com/en-us/library/azure/dn798935.aspx "http://msdn.microsoft.com/en-us/library/azure/dn798935.aspx")
	- !!Security is limited to api-key in HTTP header!!
	- Simple query syntax: [http://msdn.microsoft.com/en-us/library/azure/dn798920.aspx](http://msdn.microsoft.com/en-us/library/azure/dn798920.aspx "http://msdn.microsoft.com/en-us/library/azure/dn798920.aspx")
- Create Azure DocumentDB instance
	- Has SDK
	- Uses auth key to connect
	- Throughput == more partitions
	- Pricing
		- 73 cents per day per unit
		- 2000 request units per second per capacity unit
		- 10gb per capacity unit
		- 5 standard capacity units while in preview
	- Performance
		- Operations per second per capacity unit (TPS)
		- 2000tps for single document read
		- 500tps for insert, replace, delete of single document
		- 1000tps for simple predicate query returning a single document
- Populate the DocumentDB with data 
	- (no more than 10,000 documents)
- Populate the Azure Search instance with data 
	- (no more than 10,000 documents)
	- explain difference between more partitions