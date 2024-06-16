using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using Splitit.Automation.NG.PaymentsSystem.Sku.SkuMatching.Tests.SkuTestsData;

namespace Splitit.Automation.NG.Backend.BaseActions;

public class MongoHandler
{
    public IMongoDatabase? MongoConnect(string connectionString, string dbName)
    {
        try
        {
            Console.WriteLine("\nConnecting to Mongo DB");
            var dbClient = new MongoClient(connectionString);
            var db = dbClient.GetDatabase(dbName);
            Console.WriteLine("Connection Complete\n");
            return db;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in connecting to the MongoDB \n" + exception + "\n");
            throw;
        }
    }

    public async Task<List<string?>> SendMongoQueryActivityAsync(IMongoDatabase mongoDatabase, string documentToFind,
        string field, string value, string elementName, string isFunMerchant = null!)
    {
        try
        {            
            Console.WriteLine("\nStarting SendMongoQueryActivity");
            var mongoDocument = mongoDatabase.GetCollection<BsonDocument>(documentToFind);
            var filter = Builders<BsonDocument>.Filter.Eq(field, value);
            var docAsync = (await mongoDocument.FindAsync(filter)).ToList();
            if (!docAsync.Any())
            {
                await Task.Delay(3*1000);
                docAsync = (await mongoDocument.FindAsync(filter)).ToList();
            }

            List<string?> listValues ;
            if (isFunMerchant != null!)
            {
                listValues = new List<string?>();
                foreach (var docFun in docAsync)
                {
                    Console.WriteLine("NEED TO ADD FUNCTIONALITY IF NECESSARY -> " + docFun);
                }
            }
            else
            {
                listValues = docAsync.Select(doc => doc.GetElement(elementName).Value.ToString()).ToList();
            }

            Console.WriteLine("Done with SendMongoQueryActivity\n");
            return listValues;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendMongoQueryActivity \n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateActivityValueExistence(List<string?> listActivities,
        SkuTestsData.PaymentSettings paymentSettings)
    {
        try
        {
            Console.WriteLine("\nStarting ValidateActivityValueExistence");
            foreach (var activity in listActivities)
            {
                Console.WriteLine("Starting to validate activity -> " + activity + "in collection");
                Assert.That(paymentSettings.SetActivity, Does.Contain(activity!));
                Console.WriteLine("activity -> " + activity + " was found in the collection");
            }

            Console.WriteLine("Done with ValidateActivityValueExistence\n");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in ValidateActivityValueExistence\n" + exception + "\n");
            throw;
        }
    }

    public async Task<string> SendMongoQueryForSkuAsync(IMongoDatabase mongoDatabase, string documentToFind, string field, string value,
        string elementName, int counter = 0)
    {
        try
        {
            Console.WriteLine("\nStarting SendMongoQueryForSku");
            string ans = string.Empty;
            var mongoDocument = mongoDatabase.GetCollection<BsonDocument>(documentToFind);
            var filter = Builders<BsonDocument>.Filter.Eq(field, value);
            var doc = (await mongoDocument.FindAsync(filter)).FirstOrDefault();
            if(doc is null)
            {
                if (counter == 10)
                {
                    Assert.Fail("Mongo document was not found!!!");
                }
                else
                {
                    await Task.Delay(2 * 1000);
                    return await SendMongoQueryForSkuAsync(mongoDatabase, documentToFind, field, value, elementName, ++counter);
                }
            }
            else
            {
                ans = doc.GetElement(elementName).Value.ToString()!;
            }

            return ans;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendMongoQueryForSku \n" + exception + "\n");
            throw;
        }
    }

    public async Task<(string, string)> SendMongoQueryForStatusAndIdAsync(IMongoDatabase mongoDatabase, string documentToFind,
        string field, string value, string elementName, string elementName2, int counter = 0)
    {
        try
        {
            Console.WriteLine("\nStarting SendMongoQueryForStatusAndId");
            var ans = (string.Empty, string.Empty);
            var mongoDocument = mongoDatabase.GetCollection<BsonDocument>(documentToFind);
            var filter = Builders<BsonDocument>.Filter.Eq(field, value);
            var doc = (await mongoDocument.FindAsync(filter)).FirstOrDefault();

            if (doc is null)
            {
                if (counter == 10)
                {
                    Assert.Fail("Mongo document was not found!!!");
                }
                else
                {
                    await Task.Delay(2 * 1000);
                    return await SendMongoQueryForStatusAndIdAsync(mongoDatabase, documentToFind, field, value, elementName,elementName2, ++counter);
                }
            }
            else
            {
                ans = (doc.GetElement(elementName).Value.ToString()!, doc.GetElement(elementName2).Value.ToString()!);
            }

            return ans;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendMongoQueryForStatusAndId \n" + exception + "\n");
            throw;
        }
    }

    public async Task<(string, string)> SendMongoQueryWith2KeysAsync(IMongoDatabase mongoDatabase, string documentToFind, string field1,
        string value1, string field2, string value2, string elementName, string elementName2, int counter = 0)
    {
        try
        {

            Console.WriteLine("\nStarting SendMongoQueryWith2keys");
            var ans = (string.Empty,string.Empty);
            var mongoDocument = mongoDatabase.GetCollection<BsonDocument>(documentToFind);

            FilterDefinition<BsonDocument> filter;
            if (value2 == "true")
            {
                filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq(field1, value1),
                    Builders<BsonDocument>.Filter.Eq(field2, true)
                );
            }

            else if (value2 == "false")
            {
                filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq(field1, value1),
                    Builders<BsonDocument>.Filter.Eq(field2, false)
                );
            }
            else 
            {
                filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq(field1, value1),
                    Builders<BsonDocument>.Filter.Eq(field2, value2)
                );
            }
            
            var doc = (await mongoDocument.FindAsync(filter)).FirstOrDefault();
            if (doc is null)
            {
                if (counter == 10)
                {
                    Assert.Fail("Mongo document was not found!!!");
                }
                else
                {
                    await Task.Delay(2 * 1000);
                    return await SendMongoQueryWith2KeysAsync(mongoDatabase, documentToFind, field1, value1,field2,value2, elementName,elementName2, ++counter);
                }
            }
            else
            {
                ans = (doc.GetElement(elementName).Value.ToString()!, doc.GetElement(elementName2).Value.ToString()!);
            }

            return ans;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendMongoQueryWith2keys \n" + exception + "\n");
            throw;
        }
    }

    public async Task<(string, string)> SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(IMongoDatabase mongoDatabase, string documentToFind, string field1,
        string value1, string field2, string value2, string field3, string value3, string elementName,
        string elementName2, int counter = 0)
    {
        try
        {

            Console.WriteLine("\nStarting SendMongoQueryWith3keys");
            var ans = (string.Empty, string.Empty);
            var mongoDocument = mongoDatabase.GetCollection<BsonDocument>(documentToFind);
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq(field1, value1),
                Builders<BsonDocument>.Filter.Eq(field2, value2),
                Builders<BsonDocument>.Filter.Eq(field3, value3)
            );
            var doc = (await mongoDocument.FindAsync(filter)).FirstOrDefault();
            if (doc is null)
            {
                if (counter == 10)
                {
                    Assert.Fail("Mongo document was not found!!!");
                }
                else
                {
                    await Task.Delay(2 * 1000);
                    return await SendMongoQueryWith3KeysReturn2StringColumnValuesAsync(mongoDatabase, documentToFind, 
                        field1, value1, field2, value2, 
                        field3, value3, elementName, elementName2, ++counter);
                }
            }
            else
            {
                ans = (doc.GetElement(elementName).Value.ToString()!, doc.GetElement(elementName2).Value.ToString()!);
            }

            return ans;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendMongoQueryWith3keys \n" + exception + "\n");
            throw;
        }
    }

    public async Task<List<BsonDocument>?> SendMongoQueryWith2KeysAndReturnListOfResultAsync(IMongoDatabase mongoDatabase,
        string documentToFind, string field1, string value1, string field2, string value2, string elementName,
        string elementName2, int counter = 0)
    {
        try
        {
            Console.WriteLine("\nStarting SendMongoQueryWith2KeysAndReturnListOfResult");
            await Task.Delay(3 * 1000);
            var ans = new List<BsonDocument>();
            var mongoDocument = mongoDatabase.GetCollection<BsonDocument>(documentToFind);
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq(field1, value1),
                Builders<BsonDocument>.Filter.Eq(field2, value2)
            );

            var docs = (await mongoDocument.FindAsync(filter)).ToList();
            if (!docs.Any())
            {
                if (counter == 10)
                {
                    Assert.Fail("Mongo document was not found!!!");
                }
                else
                {
                    await Task.Delay(2 * 1000);
                    return await SendMongoQueryWith2KeysAndReturnListOfResultAsync(mongoDatabase, documentToFind, field1, value1, field2, value2, elementName, elementName2, ++counter);
                }
            }
            else
            {
                ans = docs;
            }

            return ans;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SendMongoQueryWith2KeysAndReturnListOfResult \n" + exception + "\n");
            throw;
        }
    }

    public bool ValidateValueInDocsList(List<BsonDocument> docsList, string key, string value)
    {
        try
        {
            Console.WriteLine("Starting ValidateValueInDocsList");
            foreach (var item in docsList)
            {
                var actual = item.GetElement(key);
                if (actual.ToString()!.Contains(value) || actual.ToString()!.Equals(value))
                {
                    Console.WriteLine("Value found Done with ValidateValueInDocsList");
                    return true;
                }
            }

            return false;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Value in ValidateValueInDocsList was not found-> " + exception);
            throw;
        }
    }

    public string ReturnValueFromDocsList(List<BsonDocument> docsList, string key, string value, string columnValueToReturn)
    {
        try
        {
            Console.WriteLine("Starting ValidateValueInDocsList");
            foreach (var item in docsList)
            {
                var actual = item.GetElement(key);
                if (actual.ToString()!.Contains(value) || actual.ToString()!.Equals(value))
                {
                    if (columnValueToReturn == "_id")
                    {
                        Console.WriteLine("Value found Done with ValidateValueInDocsList");
                        return item.GetElement(columnValueToReturn).ToString()!.Remove(0, 4);
                    }

                    if (columnValueToReturn == "IsDeleted")
                    {
                        Console.WriteLine("Value found Done with ValidateValueInDocsList");
                        return item.GetElement(columnValueToReturn).ToString()!.Remove(0, 10);
                    }

                    if (columnValueToReturn == "Status")
                    {
                        Console.WriteLine("Value found Done with ValidateValueInDocsList");
                        return item.GetElement(columnValueToReturn).ToString()!.Remove(0, 7);
                    }
                    Console.WriteLine("Value found Done with ValidateValueInDocsList");
                    return item.GetElement(columnValueToReturn).ToString()!;
                }
            }

            return "";
        }
        catch (Exception exception)
        {
            Console.WriteLine("Value in ValidateValueInDocsList was not found-> " + exception);
            throw;
        }
    }

    public async Task<BsonDocument> ReturnBsonDocumentAfterQueryAsync(IMongoDatabase mongoDatabase, string collection, string key1, string value1
        , string key2, string value2, string key3, string value3, string value3Optional = null!)
    {
        try
        {
            Console.WriteLine("Starting ReturnBsonDocumentAfterQuery");
            var mongoDocument = mongoDatabase.GetCollection<BsonDocument>(collection);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq(key1, value1),
                Builders<BsonDocument>.Filter.Eq(key2, value2),
                Builders<BsonDocument>.Filter.Eq(key3, value3)
            );
            var documentFound = (await mongoDocument.FindAsync(filter)).FirstOrDefault();
            if (documentFound is null)
            {
                await Task.Delay(3 * 1000);
                documentFound = (await mongoDocument.FindAsync(filter)).FirstOrDefault();
            }

            Assert.That(documentFound, Is.Not.Null);
            
            Console.WriteLine("Done With ReturnBsonDocumentAfterQuery");
            return documentFound;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Value in ReturnBsonDocumentAfterQuery was not found-> " + exception);
            throw;
        }
    }

    public bool SetValueForGivenKeyInMongoDocument(IMongoDatabase mongoDatabase, string collectionName, BsonDocument docFound, 
        string key, string value)
    {
        try
        {
            Console.WriteLine("Starting SetValueForGivenKeyInMongoDocument");
            var collection = mongoDatabase.GetCollection<BsonDocument>(collectionName);
            docFound[key] = value;
            var filter = Builders<BsonDocument>.Filter.Eq("_id", docFound["_id"]);
            var result = collection.ReplaceOne(filter, docFound);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                Console.WriteLine("Document updated successfully.");
            }
            else
            {
                Assert.Fail("Document not found or update failed.");
            }
            Console.WriteLine("Done with SetValueForGivenKeyInMongoDocument");
            return true;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in SetValueForGivenKeyInMongoDocument -> " + exception);
            throw;
        }
    }

    public BsonDocument? QueryMongoWithDocId(IMongoDatabase mongoDatabase, string collectionName, string id)
    {
        try
        {
            Console.WriteLine("Starting QueryMongoWithDocId");
            var collection = mongoDatabase.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var foundDocument = collection.Find(filter).FirstOrDefault();
            if (foundDocument != null)
            {
                Console.WriteLine("Found document:");
                Console.WriteLine(foundDocument.ToJson());
            }
            else
            {
                Console.WriteLine("Document not found.");
            }
            Console.WriteLine("Done with QueryMongoWithDocId");
            return foundDocument;
        }
        catch (Exception exception)
        {
            Console.WriteLine("Error in QueryMongoWithDocId -> " + exception);
            throw;
        }
    }

    public bool ValidateBillingAmountInDocsLists(List<BsonDocument> docsLists, double amountToCheck)
    {
        try
        {
            Console.WriteLine("Starting ValidateBillingAmountInDocsLists");
            var firstBillingAmount = docsLists[0].ToList()[13].Value.ToString();
            var secondBillingAmount = docsLists[1].ToList()[13].Value.ToString();
            var sum = double.Parse(firstBillingAmount!) - double.Parse(secondBillingAmount!);
            Assert.That(amountToCheck.Equals(sum));
            Console.WriteLine("Done with ValidateBillingAmountInDocsLists");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in ValidateBillingAmountInDocsLists" + e);
            throw;
        }
    }

    public async Task<List<BsonDocument>> QueryMongoAndReturnDoc(IMongoDatabase mongoDatabase, string collection, string field, string value)
    {
        try
        {
            var mongoDocument = mongoDatabase.GetCollection<BsonDocument>(collection);
            var filter = Builders<BsonDocument>.Filter.Eq(field, value);
            var docAsync = (await mongoDocument.FindAsync(filter)).ToList();
            if (docAsync.Any()) return docAsync;
            await Task.Delay(3 * 1000);
            docAsync = (await mongoDocument.FindAsync(filter)).ToList();

            return docAsync;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error in QueryMongo" + e);
            throw;
        }
    }
}