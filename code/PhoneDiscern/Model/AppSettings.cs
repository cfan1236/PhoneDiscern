namespace PhoneDiscern.Model
{
    public class AppSettings
    {
		public MongoDb mongodb { get; set; }
    }
   
	public class MongoDb
	{
		public string ConnectionStr { get; set; }
		public string Database { get; set; }
	}
}
