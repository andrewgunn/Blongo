namespace Blongo.Data
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Conventions;

    public static class MongoDbConfig
    {
        public static void RegisterCamelCaseElementNameConvention()
        {
            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            pack.Add(new EnumRepresentationConvention(BsonType.String));
            ConventionRegistry.Register("Blongo", pack, t => true);
        }
    }
}